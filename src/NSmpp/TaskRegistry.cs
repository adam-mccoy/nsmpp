using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace NSmpp
{
    internal class TaskRegistry
    {
        private static readonly Type DefaultTaskType = typeof(bool);

        internal class OutstandingTask
        {
            private Delegate _getTask;
            private Delegate _setResult;
            private Delegate _setException;

            internal OutstandingTask(Type resultType)
            {
                ResultType = resultType;
                CreateSource();
            }

            internal Type ResultType { get; private set; }

            internal Task GetTask()
            {
                var result = _getTask.DynamicInvoke();
                return (Task)result;
            }

            internal Task<T> GetTask<T>()
            {
                VerifyType(typeof(T));
                var result = _getTask.DynamicInvoke();
                return (Task<T>)result;
            }

            internal void SetResult(object result)
            {
                VerifyType(result.GetType());
                _setResult.DynamicInvoke(result);
            }

            internal void SetException(Exception exception)
            {
                _setException.DynamicInvoke(exception);
            }

            private void CreateSource()
            {
                var sourceType = typeof(TaskCompletionSource<>).MakeGenericType(ResultType);
                var taskType = typeof(Task<>).MakeGenericType(ResultType);
                var source = Activator.CreateInstance(sourceType, TaskContinuationOptions.RunContinuationsAsynchronously);

                var getTask = sourceType.GetProperty("Task").GetGetMethod();
                _getTask = Delegate.CreateDelegate(typeof(Func<>).MakeGenericType(taskType), source, getTask);

                var setResultMethod = sourceType.GetMethod("SetResult");
                _setResult = Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(ResultType), source, setResultMethod);

                var setExceptionMethod = sourceType.GetMethod("SetException", new[] { typeof(Exception) });
                _setException = Delegate.CreateDelegate(typeof(Action<Exception>), source, setExceptionMethod);
            }

            private void VerifyType(Type type)
            {
                if (!ResultType.IsAssignableFrom(type))
                    throw new ArgumentException("Object is not of the correct type.");
            }
        }

        private readonly ConcurrentDictionary<uint, OutstandingTask> _registry = new ConcurrentDictionary<uint, OutstandingTask>();

        internal OutstandingTask Register(uint sequence)
        {
            return RegisterInternal(sequence, DefaultTaskType);
        }

        internal OutstandingTask Register<T>(uint sequence)
        {
            return RegisterInternal(sequence, typeof(T));
        }

        internal OutstandingTask Unregister(uint sequence)
        {
            OutstandingTask task;
            if (!_registry.TryRemove(sequence, out task))
                return null;
            return task;
        }

        private OutstandingTask RegisterInternal(uint sequence, Type resultType)
        {
            var task = new OutstandingTask(resultType);
            if (!_registry.TryAdd(sequence, task))
                throw new ArgumentException("A task with the same sequence number has already been registered.");

            return task;
        }
    }
}

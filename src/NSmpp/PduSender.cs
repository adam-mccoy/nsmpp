using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp
{
    internal class PduSender
    {
        private const int DequeueTimeout = 1000;

        private readonly BlockingCollection<PduBase> _sendQueue;
        private readonly Stream _outputStream;
        private readonly object _lock = new object();
        private readonly CancellationTokenSource _token = new CancellationTokenSource();

        private Task _task;
        private bool _running;

        internal PduSender(BlockingCollection<PduBase> sendQueue, Stream outputStream)
        {
            _sendQueue = sendQueue;
            _outputStream = outputStream;
        }

        internal Task Start()
        {
            if (_running) return _task;
            lock (_lock)
            {
                if (_running) return _task;

                _task = Task.Factory.StartNew(
                    function: Run,
                    cancellationToken: _token.Token,
                    creationOptions: TaskCreationOptions.LongRunning,
                    scheduler: TaskScheduler.Default).Unwrap();

                _running = true;
            }
            return _task;
        }

        internal void Stop()
        {
            if (!_running) return;
            lock (_lock)
            {
                if (!_running) return;
                _token.Cancel();
                _task.Wait();
                _task = null;
            }
        }

        private async Task Run()
        {
            PduBase pdu;
            while (!_token.IsCancellationRequested)
            {
                try
                {
                    if (!_sendQueue.TryTake(out pdu, DequeueTimeout, _token.Token))
                        continue;

                    var serializer = PduSerializerFactory.Create(pdu.Command);
                    var bytes = serializer.Serialize(pdu);
                    await _outputStream.WriteAsync(bytes, 0, bytes.Length);
                }
                catch (OperationCanceledException)
                {
                    // shutting down
                }
            }
        }
    }
}

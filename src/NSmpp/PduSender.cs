using System.Collections.Concurrent;
using NSmpp.Pdu;
using NSmpp.Serialization;

namespace NSmpp;

internal class PduSender
{
    private const int DequeueTimeout = 1000;

    private readonly BlockingCollection<PduBase> _sendQueue = new();
    private readonly Stream _outputStream;
    private readonly object _lock = new();
    private readonly CancellationTokenSource _token = new();

    private Task? _task;

    internal PduSender(Stream outputStream)
    {
        _outputStream = outputStream;
    }

    internal void Enqueue(PduBase pdu)
    {
        _sendQueue.Add(pdu);
    }

    internal Task Start()
    {
        lock (_lock)
        {
            if (_task is not null) return _task;

            _task = Task.Factory.StartNew(
                function: Run,
                cancellationToken: _token.Token,
                creationOptions: TaskCreationOptions.LongRunning,
                scheduler: TaskScheduler.Default).Unwrap();

            return _task;
        }
    }

    internal void Stop()
    {
        lock (_lock)
        {
            if (_task is null) return;
            _token.Cancel();
            _task.Wait();
            _task = null;
        }
    }

    private async Task Run()
    {
        while (!_token.IsCancellationRequested)
        {
            try
            {
                if (!_sendQueue.TryTake(out var pdu, DequeueTimeout, _token.Token))
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
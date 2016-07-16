﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NSmpp.Serialization;
using NSmpp.Pdu;

namespace NSmpp
{
    internal class PduReceiver
    {
        private readonly IPduReceivedHandler _handler;
        private readonly Stream _inputStream;
        private readonly object _lock = new object();
        private readonly CancellationTokenSource _token = new CancellationTokenSource();

        private Task _task;
        private bool _running;

        internal PduReceiver(
            Stream inputStream,
            IPduReceivedHandler handler)
        {
            _inputStream = inputStream;
            _handler = handler;
        }

        internal void Start()
        {
            if (_running) return;
            lock (_lock)
            {
                if (_running) return;

                _task = Task.Factory.StartNew(
                    function: Run,
                    cancellationToken: _token.Token,
                    creationOptions: TaskCreationOptions.LongRunning,
                    scheduler: TaskScheduler.Default).Unwrap();

                _running = true;
            }
        }

        internal void Stop()
        {
            if (!_running) return;
            lock(_lock)
            {
                if (!_running) return;
                _token.Cancel();
                _task.Wait();
                _task = null;
            }
        }

        private async Task Run()
        {
            var lengthBuffer = new byte[4];
            Task<int> requestTask = null;
            int bytesRead = 0;
            while (!_token.IsCancellationRequested)
            {
                try
                {
                    if (requestTask == null)
                        requestTask = _inputStream.ReadAsync(lengthBuffer, bytesRead, 4 - bytesRead);

                    var timeoutTask = Task.Delay(TimeSpan.FromMilliseconds(1000));

                    var completed = await Task.WhenAny(requestTask, timeoutTask);
                    if (completed == requestTask)
                    {
                        if ((bytesRead += requestTask.Result) == 4)
                        {
                            var length = PduReader.ReadInteger(lengthBuffer, 0);
                            var pduBuffer = new byte[length];
                            Buffer.BlockCopy(lengthBuffer, 0, pduBuffer, 0, 4);

                            var toRead = length - 4;
                            while (toRead > 0)
                                toRead -= await _inputStream.ReadAsync(pduBuffer, length - toRead, toRead);

                            bytesRead = 0;
                            ProcessPdu(pduBuffer);
                        }
                        requestTask = null;
                    }
                }
                catch
                {
                }
            }
        }

        private void ProcessPdu(byte[] pduBuffer)
        {
            var command = (SmppCommand)PduReader.ReadInteger(pduBuffer, 4);
            try
            {
                var serializer = PduSerializerFactory.Create(command);
                var pdu = serializer.Deserialize(pduBuffer);
                _handler.HandlePdu((dynamic)pdu);
            }
            catch (ArgumentException ex)
            {
                _handler.HandleError(pduBuffer, ex.Message);
            }
        }
    }
}

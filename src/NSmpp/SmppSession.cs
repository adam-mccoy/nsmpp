using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NSmpp.Pdu;

namespace NSmpp
{
    internal class SmppSession : IPduReceivedHandler, IDisposable
    {
        private readonly ConcurrentDictionary<uint, object> _outstandingTasks = new ConcurrentDictionary<uint, object>();

        private SessionState _state;
        private int _sequenceNumber = 0;

        private PduSender _pduSender;
        private PduReceiver _pduReceiver;

        internal SmppSession(Stream inputStream, Stream outputStream)
        {
            _pduSender = new PduSender(outputStream);
            _pduReceiver = new PduReceiver(
                inputStream,
                this);
            _pduReceiver.Start();
        }

        internal SessionState SessionState
        {
            get { return _state; }
        }

        internal void Close()
        {
            if (_state != SessionState.Open && _state != SessionState.Closed)
                Unbind().Wait();
            _pduReceiver.Stop();
            _state = SessionState.Closed;
        }

        internal Task Bind(BindType type, string systemId, string password)
        {
            return Bind(type, systemId, password, new BindOptions());
        }

        internal async Task Bind(BindType type, string systemId, string password, BindOptions options)
        {
            var pdu = CreateBindPdu(type, systemId, password, options);
            var tcs = RegisterTask<bool>(pdu.SequenceNumber);

            await _pduSender.SendAsync(pdu);
            await tcs.Task;
        }

        internal async Task Unbind()
        {
            var sequence = GetNextSequenceNumber();
            var tcs = RegisterTask<bool>(sequence);
            var pdu = new Unbind(sequence);

            await _pduSender.SendAsync(pdu);
            await tcs.Task.ConfigureAwait(false);
        }

        private TaskCompletionSource<T> RegisterTask<T>(uint sequenceNumber)
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            if (!_outstandingTasks.TryAdd(sequenceNumber, tcs))
                throw new ArgumentException("A task with the same sequence number has already been registerd.");

            return tcs;
        }

        private TaskCompletionSource<T> RetrieveTask<T>(uint sequenceNumber)
        {
            object tcs;
            if (!_outstandingTasks.TryRemove(sequenceNumber, out tcs))
                return null;

            return (TaskCompletionSource<T>)tcs;
        }

        public void HandlePdu(BindTransmitterResponse pdu)
        {
            var tcs = RetrieveTask<bool>(pdu.SequenceNumber);
            if (tcs == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var bindException = new Exception("The bind operation failed with error code: " + pdu.Status);
                tcs.SetException(bindException);
            }
            else
            {
                _state = SessionState.BoundTransmitter;
                tcs.SetResult(true);
            }
        }

        public void HandlePdu(UnbindResponse pdu)
        {
            var tcs = RetrieveTask<bool>(pdu.SequenceNumber);
            if (tcs == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var unbindException = new Exception("The unbind operation failed with error code: " + pdu.Status);
                tcs.SetException(unbindException);
            }
            else
            {
                _state = SessionState.Open;
                tcs.SetResult(true);
            }
        }

        public void HandleError(byte[] buffer, string error)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }
        }

        private PduBase CreateBindPdu(
            BindType bindType,
            string systemId,
            string password,
            BindOptions options)
        {
            var sequence = GetNextSequenceNumber();

            switch (bindType)
            {
                case BindType.Transmitter:
                    return new BindTransmitter(
                        SmppStatus.Ok,
                        sequence,
                        systemId,
                        password,
                        options.SystemType,
                        options.InterfaceVersion,
                        options.Ton,
                        options.Npi,
                        options.AddressRange);

                default:
                    return null;
            }
        }

        private uint GetNextSequenceNumber()
        {
            return (uint)Interlocked.Increment(ref _sequenceNumber);
        }
    }
}

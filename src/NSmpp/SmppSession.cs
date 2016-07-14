using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NSmpp.Pdu;

namespace NSmpp
{
    internal class SmppSession : IPduReceivedHandler
    {
        private readonly Dictionary<uint, object> _outstandingTasks = new Dictionary<uint, object>();

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
            _pduReceiver.Stop();
        }

        internal Task Bind(BindType type, string systemId, string password)
        {
            return Bind(type, systemId, password, new BindOptions());
        }

        internal async Task Bind(BindType type, string systemId, string password, BindOptions options)
        {
            var pdu = CreateBindPdu(type, systemId, password, options);
            var tcs = new TaskCompletionSource<bool>();
            _outstandingTasks.Add(pdu.SequenceNumber, tcs);

            await _pduSender.SendAsync(pdu);
            await tcs.Task;
        }

        public void HandlePdu(BindTransmitterResponse pdu)
        {
            object task;
            if (!_outstandingTasks.TryGetValue(pdu.SequenceNumber, out task))
                return;

            _state = SessionState.BoundTransmitter;
            var tcs = (TaskCompletionSource<bool>)task;
            tcs.SetResult(true);
        }

        public void HandleError(byte[] buffer, string error)
        {
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

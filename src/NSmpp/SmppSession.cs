using System.IO;
using NSmpp.Pdu;

namespace NSmpp
{
    internal class SmppSession
    {
        private SessionState _state;
        private object _sequenceLock = new object();
        private uint _sequenceNumber = 0u;

        private PduSender _pduSender;
        private PduReceiver _pduReceiver;

        internal SmppSession(Stream inputStream, Stream outputStream)
        {
            _pduSender = new PduSender(outputStream);
            _pduReceiver = new PduReceiver(
                inputStream,
                PduReceivedCallback,
                PduError);
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

        private void PduReceivedCallback(PduBase pdu)
        {
        }

        private void PduError(byte[] buffer, string error)
        {
        }

        private PduBase CreateBindPdu(
            BindType bindType,
            string systemId,
            string password,
            string systemType,
            byte interfaceVersion,
            TypeOfNumber ton,
            NumericPlanIndicator npi,
            string addressRange)
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
                        systemType,
                        interfaceVersion,
                        ton,
                        npi,
                        addressRange);

                default:
                    return null;
            }
        }

        private uint GetNextSequenceNumber()
        {
            lock (_sequenceLock)
            {
                return _sequenceNumber++;
            }
        }
    }
}

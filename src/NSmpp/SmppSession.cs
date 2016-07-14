using System.IO;
using NSmpp.Pdu;

namespace NSmpp
{
    internal class SmppSession
    {
        private SessionState _state;
        private int _sequenceNumber = 0;

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

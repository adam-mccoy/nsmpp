using NSmpp.Pdu;

namespace NSmpp.Helpers
{
    internal static class BindHelper
    {
        internal static PduBase CreateBindPdu(
            uint sequenceNumber,
            BindType bindType,
            string systemId,
            string password,
            BindOptions options)
        {
            switch (bindType)
            {
                case BindType.Transmitter:
                    return new BindTransmitter(
                        sequenceNumber,
                        systemId,
                        password,
                        options.SystemType,
                        options.InterfaceVersion,
                        options.Ton,
                        options.Npi,
                        options.AddressRange);

                case BindType.Receiver:
                    return new BindReceiver(
                        sequenceNumber,
                        systemId,
                        password,
                        options.SystemType,
                        options.InterfaceVersion,
                        options.Ton,
                        options.Npi,
                        options.AddressRange);

                case BindType.Transceiver:
                    return new BindTransceiver(
                        sequenceNumber,
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
    }
}

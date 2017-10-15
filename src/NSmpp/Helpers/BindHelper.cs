using NSmpp.Pdu;

namespace NSmpp.Helpers
{
    internal static class BindHelper
    {
        internal static PduBase CreateBindPdu(
            BindType bindType,
            string systemId,
            string password,
            BindOptions options)
        {
            switch (bindType)
            {
                case BindType.Transmitter:
                    return new BindTransmitter(
                        0,
                        systemId,
                        password,
                        options.SystemType,
                        options.InterfaceVersion,
                        options.Ton,
                        options.Npi,
                        options.AddressRange);

                case BindType.Receiver:
                    return new BindReceiver(
                        0,
                        systemId,
                        password,
                        options.SystemType,
                        options.InterfaceVersion,
                        options.Ton,
                        options.Npi,
                        options.AddressRange);

                case BindType.Transceiver:
                    return new BindTransceiver(
                        0,
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

using NSmpp.Pdu;

namespace NSmpp.Helpers;

internal static class BindHelper
{
    internal static PduBase CreateBindPdu(
        uint sequenceNumber,
        BindType bindType,
        string systemId,
        string password,
        BindOptions options)
    {
        return bindType switch
        {
            BindType.Transmitter => new BindTransmitter(sequenceNumber, systemId, password, options.SystemType,
                options.InterfaceVersion, options.Ton, options.Npi, options.AddressRange),
            BindType.Receiver => new BindReceiver(sequenceNumber, systemId, password, options.SystemType,
                options.InterfaceVersion, options.Ton, options.Npi, options.AddressRange),
            BindType.Transceiver => new BindTransceiver(sequenceNumber, systemId, password, options.SystemType,
                options.InterfaceVersion, options.Ton, options.Npi, options.AddressRange),
            _ => throw new ArgumentOutOfRangeException(nameof(bindType), "Invalid bind type")
        };
    }
}
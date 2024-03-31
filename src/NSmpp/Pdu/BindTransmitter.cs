namespace NSmpp.Pdu;

internal class BindTransmitter(
    uint sequenceNumber,
    string systemId,
    string password,
    string systemType,
    byte interfaceVersion,
    TypeOfNumber addressTon,
    NumericPlanIndicator addressNpi,
    string? addressRange)
    : PduBase(sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.BindTransmitter;

    internal string SystemId { get; private set; } = systemId;
    internal string Password { get; private set; } = password;
    internal string SystemType { get; private set; } = systemType;
    internal byte InterfaceVersion { get; private set; } = interfaceVersion;
    internal TypeOfNumber AddressTon { get; private set; } = addressTon;
    internal NumericPlanIndicator AddressNpi { get; private set; } = addressNpi;
    internal string? AddressRange { get; private set; } = addressRange;
}
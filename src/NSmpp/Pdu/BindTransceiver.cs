namespace NSmpp.Pdu;

internal class BindTransceiver(
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
    internal override SmppCommand Command => SmppCommand.BindTransceiver;

    internal string SystemId { get; } = systemId;
    internal string Password { get; } = password;
    internal string SystemType { get; } = systemType;
    internal byte InterfaceVersion { get; } = interfaceVersion;
    internal TypeOfNumber AddressTon { get; } = addressTon;
    internal NumericPlanIndicator AddressNpi { get; } = addressNpi;
    internal string? AddressRange { get; } = addressRange;
}
namespace NSmpp.Pdu;

internal class Submit(
    uint sequenceNumber,
    string? serviceType,
    TypeOfNumber sourceTon,
    NumericPlanIndicator sourceNpi,
    string? sourceAddress,
    TypeOfNumber destTon,
    NumericPlanIndicator destNpi,
    string destAddress,
    int esmClass,
    int protocolId,
    int priorityFlag,
    string shortMessage)
    : PduBase(sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.Submit;

    internal string? ServiceType { get; private set; } = serviceType;
    internal TypeOfNumber SourceTon { get; private set; } = sourceTon;
    internal NumericPlanIndicator SourceNpi { get; private set; } = sourceNpi;
    internal string? SourceAddress { get; private set; } = sourceAddress;
    internal TypeOfNumber DestTon { get; private set; } = destTon;
    internal NumericPlanIndicator DestNpi { get; private set; } = destNpi;
    internal string DestAddress { get; private set; } = destAddress;
    internal int EsmClass { get; private set; } = esmClass;
    internal int ProtocolId { get; private set; } = protocolId;
    internal int PriorityFlag { get; private set; } = priorityFlag;
    internal string ShortMessage { get; private set; } = shortMessage;
}
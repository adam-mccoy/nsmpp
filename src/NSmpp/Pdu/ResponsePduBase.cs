namespace NSmpp.Pdu;

internal abstract class ResponsePduBase(SmppStatus status, uint sequenceNumber)
    : PduBase(sequenceNumber)
{
    internal SmppStatus Status { get; private set; } = status;
}
namespace NSmpp.Pdu;

internal abstract class ResponsePduBase(SmppStatus status) : PduBase
{
    internal SmppStatus Status { get; private set; } = status;
}
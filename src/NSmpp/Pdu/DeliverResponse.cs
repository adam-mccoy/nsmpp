namespace NSmpp.Pdu;

internal class DeliverResponse(SmppStatus status)
    : ResponsePduBase(status)
{
    internal override SmppCommand Command => SmppCommand.DeliverResp;
}
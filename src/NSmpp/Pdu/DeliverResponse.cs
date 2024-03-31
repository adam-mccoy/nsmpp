namespace NSmpp.Pdu;

internal class DeliverResponse(SmppStatus status, uint sequenceNumber)
    : ResponsePduBase(status, sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.DeliverResp;
}
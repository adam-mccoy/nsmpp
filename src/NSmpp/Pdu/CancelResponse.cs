namespace NSmpp.Pdu;

internal class CancelResponse(SmppStatus status, uint sequenceNumber)
    : ResponsePduBase(status, sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.CancelResp;
}
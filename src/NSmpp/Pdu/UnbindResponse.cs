namespace NSmpp.Pdu;

internal class UnbindResponse(SmppStatus status, uint sequenceNumber)
    : ResponsePduBase(status, sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.UnbindResp;
}
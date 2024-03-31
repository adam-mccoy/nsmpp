namespace NSmpp.Pdu;

internal class EnquireLinkResponse(SmppStatus status, uint sequenceNumber)
    : ResponsePduBase(status, sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.EnquireLinkResp;
}
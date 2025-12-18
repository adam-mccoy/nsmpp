namespace NSmpp.Pdu;

internal class EnquireLinkResponse(SmppStatus status)
    : ResponsePduBase(status)
{
    internal override SmppCommand Command => SmppCommand.EnquireLinkResp;
}
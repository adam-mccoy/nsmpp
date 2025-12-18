namespace NSmpp.Pdu;

internal class UnbindResponse(SmppStatus status)
    : ResponsePduBase(status)
{
    internal override SmppCommand Command => SmppCommand.UnbindResp;
}
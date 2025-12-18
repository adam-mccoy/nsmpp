namespace NSmpp.Pdu;

internal class CancelResponse(SmppStatus status)
    : ResponsePduBase(status)
{
    internal override SmppCommand Command => SmppCommand.CancelResp;
}
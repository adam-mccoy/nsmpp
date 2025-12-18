namespace NSmpp.Pdu;

internal class SubmitResponse(
    SmppStatus status,
    string? messageId = null)
    : ResponsePduBase(status)
{
    internal override SmppCommand Command => SmppCommand.SubmitResp;

    internal string? MessageId { get; } = messageId;
}
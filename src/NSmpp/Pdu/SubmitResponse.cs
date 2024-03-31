namespace NSmpp.Pdu;

internal class SubmitResponse(
    SmppStatus status,
    uint sequenceNumber,
    string? messageId = null)
    : ResponsePduBase(status, sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.SubmitResp;

    internal string? MessageId { get; } = messageId;
}
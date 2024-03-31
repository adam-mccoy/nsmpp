namespace NSmpp.Pdu;

internal class QueryResponse(
    SmppStatus status,
    uint sequenceNumber,
    string messageId,
    DateTimeOffset? finalDate,
    MessageState messageState,
    int errorCode)
    : ResponsePduBase(status, sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.QueryResp;

    internal string MessageId { get; private set; } = messageId;
    internal DateTimeOffset? FinalDate { get; private set; } = finalDate;
    internal MessageState MessageState { get; private set; } = messageState;
    internal int ErrorCode { get; private set; } = errorCode;
}
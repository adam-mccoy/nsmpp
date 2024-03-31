namespace NSmpp.Pdu;

internal class Query : PduBase
{
    internal Query(
        uint sequence,
        string messageId,
        Address source)
        : base(sequence)
    {
        MessageId = messageId;
        Source = source;
    }

    internal override SmppCommand Command => SmppCommand.Query;

    internal string MessageId { get; }
    internal Address Source { get; }
}
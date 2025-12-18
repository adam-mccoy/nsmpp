namespace NSmpp.Pdu;

internal class Query : PduBase
{
    internal Query(string messageId, Address source)
    {
        MessageId = messageId;
        Source = source;
    }

    internal override SmppCommand Command => SmppCommand.Query;

    internal string MessageId { get; }
    internal Address Source { get; }
}
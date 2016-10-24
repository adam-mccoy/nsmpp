namespace NSmpp.Pdu
{
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

        internal override SmppCommand Command
        {
            get { return SmppCommand.Query; }
        }

        internal string MessageId { get; private set; }
        internal Address Source { get; private set; }
    }
}

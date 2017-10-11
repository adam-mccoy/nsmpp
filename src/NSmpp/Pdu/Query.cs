namespace NSmpp.Pdu
{
    internal class Query : PduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.Query; }
        }

        internal string MessageId { get; set; }
        internal Address Source { get; set; }
    }
}

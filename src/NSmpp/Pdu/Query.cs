namespace NSmpp.Pdu
{
    internal class Query : PduBase
    {
        internal Query(
            uint sequence,
            string messageId,
            TypeOfNumber sourceTon,
            NumericPlanIndicator sourceNpi,
            string sourceAddress)
            : base(sequence)
        {
            MessageId = messageId;
            SourceTon = sourceTon;
            SourceNpi = sourceNpi;
            SourceAddress = sourceAddress;
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.Query; }
        }

        internal string MessageId { get; private set; }
        internal TypeOfNumber SourceTon { get; private set; }
        internal NumericPlanIndicator SourceNpi { get; private set; }
        internal string SourceAddress { get; private set; }
    }
}

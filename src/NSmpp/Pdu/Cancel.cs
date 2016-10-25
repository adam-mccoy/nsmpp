namespace NSmpp.Pdu
{
    internal class Cancel : PduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.Cancel; }
        }

        internal string ServiceType { get; private set; }
        internal string MessageId { get; private set; }
        internal Address Source { get; private set; }
        internal Address Destination { get; private set; }

        public Cancel(
            uint sequenceNumber,
            string serviceType,
            string messageId,
            Address source,
            Address destination)
            : base(sequenceNumber)
        {
            ServiceType = serviceType;
            MessageId = messageId;
            Source = source;
            Destination = destination;
        }
    }
}

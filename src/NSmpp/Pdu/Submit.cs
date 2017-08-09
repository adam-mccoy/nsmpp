namespace NSmpp.Pdu
{
    internal class Submit : PduBase
    {
        public Submit(
            uint sequenceNumber,
            string serviceType,
            Address source,
            Address destination,
            int esmClass,
            int protocolId,
            int priorityFlag,
            string shortMessage)
            : base(sequenceNumber)
        {
            ServiceType = serviceType;
            Source = source;
            Destination = destination;
            EsmClass = esmClass;
            ProtocolId = protocolId;
            PriorityFlag = priorityFlag;
            ShortMessage = shortMessage;
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.Submit; }
        }

        internal string ServiceType { get; private set; }
        internal Address Source { get; private set; }
        internal Address Destination { get; private set; }
        internal int EsmClass { get; private set; }
        internal int ProtocolId { get; private set; }
        internal int PriorityFlag { get; private set; }
        internal string ShortMessage { get; private set; }
    }
}

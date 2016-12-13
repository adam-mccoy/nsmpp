namespace NSmpp.Pdu
{
    internal class Deliver : PduBase
    {
        public Deliver(
            uint sequenceNumber,
            string serviceType,
            Address source,
            Address destination,
            MessagingMode mode,
            MessageType type,
            NetworkSpecificFeatures features,
            byte protocolId,
            PriorityFlag priority,
            byte registeredDelivery,
            byte dataCoding,
            int length,
            string shortMessage)
            : base(sequenceNumber)
        {
            ServiceType = serviceType;
            Source = source;
            Destination = destination;
            Mode = mode;
            Type = type;
            Features = features;
            ProtocolId = protocolId;
            Priority = priority;
            RegisteredDelivery = registeredDelivery;
            DataCoding = dataCoding;
            Length = length;
            ShortMessage = shortMessage;
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.Deliver; }
        }

        internal string ServiceType { get; set; }
        internal Address Source { get; set; }
        internal Address Destination { get; set; }
        internal MessagingMode Mode { get; set; }
        internal MessageType Type { get; set; }
        internal NetworkSpecificFeatures Features { get; set; }
        internal byte ProtocolId { get; set; }
        internal PriorityFlag Priority { get; set; }
        internal byte RegisteredDelivery { get; set; }
        internal byte DataCoding { get; set; }
        internal int Length { get; set; }
        internal string ShortMessage { get; set; }
    }
}

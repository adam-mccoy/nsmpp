namespace NSmpp.Pdu
{
    internal class Deliver : PduBase
    {
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

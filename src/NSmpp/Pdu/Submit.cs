namespace NSmpp.Pdu
{
    internal class Submit : PduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.Submit; }
        }

        internal string ServiceType { get; set; }
        internal Address Source { get; set; }
        internal Address Destination { get; set; }
        internal byte EsmClass { get; set; }
        internal int ProtocolId { get; set; }
        internal PriorityFlag PriorityFlag { get; set; }
        internal string ScheduleDeliveryTime { get; set; }
        internal byte[] ShortMessage { get; set; }
    }
}

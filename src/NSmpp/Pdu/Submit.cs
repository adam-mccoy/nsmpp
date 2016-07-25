namespace NSmpp.Pdu
{
    internal class Submit : PduBase
    {
        public Submit(
            uint sequenceNumber,
            string serviceType,
            TypeOfNumber sourceTon,
            NumericPlanIndicator sourceNpi,
            string sourceAddress,
            TypeOfNumber destTon,
            NumericPlanIndicator destNpi,
            string destAddress,
            int esmClass,
            int protocolId,
            int priorityFlag,
            string shortMessage)
            : base(sequenceNumber)
        {
            ServiceType = serviceType;
            SourceTon = sourceTon;
            SourceNpi = sourceNpi;
            SourceAddress = sourceAddress;
            DestTon = destTon;
            DestNpi = destNpi;
            DestAddress = destAddress;
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
        internal TypeOfNumber SourceTon { get; private set; }
        internal NumericPlanIndicator SourceNpi { get; private set; }
        internal string SourceAddress { get; private set; }
        internal TypeOfNumber DestTon { get; private set; }
        internal NumericPlanIndicator DestNpi { get; private set; }
        internal string DestAddress { get; private set; }
        internal int EsmClass { get; private set; }
        internal int ProtocolId { get; private set; }
        internal int PriorityFlag { get; private set; }
        internal string ShortMessage { get; private set; }
    }
}

namespace NSmpp.Pdu
{
    internal class DeliverResponse : ResponsePduBase
    {
        public DeliverResponse(SmppStatus status, uint sequenceNumber)
            : base(status, sequenceNumber)
        {
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.DeliverResp; }
        }
    }
}

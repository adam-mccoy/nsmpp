namespace NSmpp.Pdu
{
    internal class EnquireLinkResponse : ResponsePduBase
    {
        public EnquireLinkResponse(SmppStatus status, uint sequenceNumber)
            : base(status, sequenceNumber)
        {
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.EnquireLinkResp; }
        }
    }
}

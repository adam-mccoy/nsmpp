namespace NSmpp.Pdu
{
    internal class EnquireLink : PduBase
    {
        public EnquireLink(uint sequenceNumber)
            : base(sequenceNumber)
        {
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.EnquireLink; }
        }
    }
}

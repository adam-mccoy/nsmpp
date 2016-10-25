namespace NSmpp.Pdu
{
    internal class GenericNack : ResponsePduBase
    {
        public GenericNack(SmppStatus status, uint sequenceNumber)
            : base(status, sequenceNumber)
        {
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.GenericNack; }
        }
    }
}

namespace NSmpp.Pdu
{
    internal class Cancel : PduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.Cancel; }
        }

        internal string ServiceType { get; set; }
        internal string MessageId { get; set; }
        internal Address Source { get; set; }
        internal Address Destination { get; set; }
    }
}

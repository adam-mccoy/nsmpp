namespace NSmpp.Pdu
{
    internal class GenericNack : ResponsePduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.GenericNack; }
        }
    }
}

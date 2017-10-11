namespace NSmpp.Pdu
{
    internal class Unbind : PduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.Unbind; }
        }
    }
}

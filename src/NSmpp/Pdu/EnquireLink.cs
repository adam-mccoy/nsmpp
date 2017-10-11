namespace NSmpp.Pdu
{
    internal class EnquireLink : PduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.EnquireLink; }
        }
    }
}

namespace NSmpp.Pdu
{
    internal class BindTransmitter : BindPduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.BindTransmitter; }
        }
    }
}

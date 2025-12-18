namespace NSmpp.Pdu;

internal class EnquireLink : PduBase
{
    internal override SmppCommand Command => SmppCommand.EnquireLink;
}
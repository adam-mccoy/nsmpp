namespace NSmpp.Pdu;

internal class EnquireLink(uint sequenceNumber)
    : PduBase(sequenceNumber)
{
    internal override SmppCommand Command => SmppCommand.EnquireLink;
}
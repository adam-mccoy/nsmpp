namespace NSmpp.Pdu;

internal class GenericNack(SmppStatus status)
    : ResponsePduBase(status)
{
    internal override SmppCommand Command => SmppCommand.GenericNack;
}
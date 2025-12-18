namespace NSmpp.Pdu;

internal class Cancel(
    string? serviceType,
    string? messageId,
    Address? source,
    Address? destination)
    : PduBase
{
    internal override SmppCommand Command => SmppCommand.Cancel;

    internal string? ServiceType { get; private set; } = serviceType;
    internal string? MessageId { get; private set; } = messageId;
    internal Address? Source { get; private set; } = source;
    internal Address? Destination { get; private set; } = destination;
}
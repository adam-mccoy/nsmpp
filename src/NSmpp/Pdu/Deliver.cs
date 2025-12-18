namespace NSmpp.Pdu;

internal class Deliver(
    string serviceType,
    Address source,
    Address destination,
    MessagingMode mode,
    MessageType type,
    NetworkSpecificFeatures features,
    byte protocolId,
    PriorityFlag priority,
    byte registeredDelivery,
    byte dataCoding,
    int length,
    string shortMessage)
    : PduBase
{
    internal override SmppCommand Command => SmppCommand.Deliver;

    internal string ServiceType { get; } = serviceType;
    internal Address Source { get; } = source;
    internal Address Destination { get; } = destination;
    internal MessagingMode Mode { get; } = mode;
    internal MessageType Type { get; } = type;
    internal NetworkSpecificFeatures Features { get; } = features;
    internal byte ProtocolId { get; } = protocolId;
    internal PriorityFlag Priority { get; } = priority;
    internal byte RegisteredDelivery { get; } = registeredDelivery;
    internal byte DataCoding { get; } = dataCoding;
    internal int Length { get; } = length;
    internal string ShortMessage { get; } = shortMessage;
}
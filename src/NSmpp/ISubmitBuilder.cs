using System;
using System.IO;

namespace NSmpp
{
    public interface ISubmitBuilder
    {
        ISubmitBuilder UseServiceType(string serviceType);
        ISubmitBuilder UseSource(Address address);
        ISubmitBuilder UseDestination(Address address);

        ISubmitBuilder UseMessagingMode(MessagingMode mode);
        ISubmitBuilder UseMessageType(MessageType type);
        ISubmitBuilder UseGsmFeatures(NetworkSpecificFeatures features);

        ISubmitBuilder UseProtocolId(int protocolId);
        ISubmitBuilder UsePriorityFlag(PriorityFlag priority);

        ISubmitBuilder UseScheduledDeliveryTime(DateTimeOffset absolute);
        ISubmitBuilder UseScheduledDeliveryTime(RelativeTime relative);

        ISubmitBuilder UseValidityPeriod(DateTimeOffset absolute);
        ISubmitBuilder UseValidityPeriod(RelativeTime relative);

        ISubmitBuilder UseDeliveryReceiptOption(DeliveryReceiptOption option);

        ISubmitBuilder UseReplacement(bool replaceIfPresent = true);

        ISubmitBuilder UseShortMessage(string message);
        ISubmitBuilder UseShortMessage(Stream stream);
    }
}

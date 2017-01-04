using NSmpp.Pdu;

namespace NSmpp.Helpers
{
    internal static class PduHelper
    {
        internal static byte BuildEsmClass(MessagingMode mode, MessageType type, NetworkSpecificFeatures features)
        {
            return (byte)((byte)features << 6 | (byte)type << 2 | (byte)mode);
        }
    }
}

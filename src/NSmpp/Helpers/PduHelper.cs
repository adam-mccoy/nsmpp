namespace NSmpp.Helpers
{
    internal static class PduHelper
    {
        internal static byte BuildEsmClass(MessagingMode mode, MessageType type, NetworkSpecificFeatures features)
        {
            return (byte)((byte)features | (byte)type | (byte)mode);
        }
    }
}

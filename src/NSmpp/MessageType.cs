namespace NSmpp
{
    public enum MessageType : byte
    {
        Default                   = 0x00,
        DeliveryAcknowledgement   = 0x08,
        ManualUserAcknowledgement = 0x10
    }
}

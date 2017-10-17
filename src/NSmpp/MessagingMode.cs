namespace NSmpp
{
    public enum MessagingMode : byte
    {
        Default         = 0x00,
        Datagram        = 0x01,
        Forward         = 0x02,
        StoreAndForward = 0x03
    }
}

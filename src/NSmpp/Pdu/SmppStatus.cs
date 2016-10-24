namespace NSmpp.Pdu
{
    internal enum SmppStatus
    {
        Ok                   = 0x00000000,
        InvalidMessageLength = 0x00000001,
        InvalidCommandLength = 0x00000002,
        InvalidCommandId     = 0x00000003,
        MessageQueueFull     = 0x00000014
    }
}

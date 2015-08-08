namespace NSmpp.Pdu
{
    internal enum SmppCommand : uint
    {
        BindTransmitter     = 0x00000002,
        BindTransmitterResp = 0x80000002,
    }
}

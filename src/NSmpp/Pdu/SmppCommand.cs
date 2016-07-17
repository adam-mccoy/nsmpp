namespace NSmpp.Pdu
{
    internal enum SmppCommand : uint
    {
        BindReceiver        = 0x00000001,
        BindReceiverResp    = 0x80000001,
        BindTransmitter     = 0x00000002,
        BindTransmitterResp = 0x80000002,
        Unbind              = 0x00000006,
        UnbindResp          = 0x80000006,
        BindTransceiver     = 0x00000009,
        BindTransceiverResp = 0x80000009
    }
}

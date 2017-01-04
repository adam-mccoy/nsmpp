namespace NSmpp.Pdu
{
    internal enum SmppCommand : uint
    {
        GenericNack         = 0x80000000,
        BindReceiver        = 0x00000001,
        BindReceiverResp    = 0x80000001,
        BindTransmitter     = 0x00000002,
        BindTransmitterResp = 0x80000002,
        Query               = 0x00000003,
        QueryResp           = 0x80000003,
        Submit              = 0x00000004,
        SubmitResp          = 0x80000004,
        Deliver             = 0x00000005,
        DeliverResp         = 0x80000005,
        Unbind              = 0x00000006,
        UnbindResp          = 0x80000006,
        Cancel              = 0x00000008,
        CancelResp          = 0x80000008,
        BindTransceiver     = 0x00000009,
        BindTransceiverResp = 0x80000009
    }
}

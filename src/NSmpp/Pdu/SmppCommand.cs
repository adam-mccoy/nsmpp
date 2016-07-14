﻿namespace NSmpp.Pdu
{
    internal enum SmppCommand : uint
    {
        BindTransmitter     = 0x00000002,
        BindTransmitterResp = 0x80000002,
        Unbind              = 0x00000006,
        UnbindResp          = 0x80000006
    }
}

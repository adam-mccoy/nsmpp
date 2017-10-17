using System;

namespace NSmpp
{
    [Flags]
    public enum NetworkSpecificFeatures : byte
    {
        None          = 0x00,
        UdhiIndicator = 0x40,
        SetReplyPath  = 0x80,
    }
}

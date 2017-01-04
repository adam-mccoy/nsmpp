using System;

namespace NSmpp.Pdu
{
    [Flags]
    internal enum NetworkSpecificFeatures
    {
        None          = 0,
        UdhiIndicator = 1,
        SetReplyPath  = 2,
    }
}

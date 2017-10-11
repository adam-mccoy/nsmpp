using System;

namespace NSmpp
{
    [Flags]
    public enum NetworkSpecificFeatures
    {
        None          = 0,
        UdhiIndicator = 1,
        SetReplyPath  = 2,
    }
}

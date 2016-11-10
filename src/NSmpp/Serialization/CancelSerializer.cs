using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class CancelSerializer : PduSerializer<Cancel>
    {
        internal override Cancel Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        internal override byte[] Serialize(Cancel pdu)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class CancelResponseSerializer : PduSerializer<CancelResponse>
    {
        internal override byte[] Serialize(CancelResponse pdu)
        {
            throw new NotImplementedException();
        }

        internal override CancelResponse Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}

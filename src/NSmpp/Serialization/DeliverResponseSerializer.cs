using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class DeliverResponseSerializer : PduSerializer<DeliverResponse>
    {
        internal override byte[] Serialize(DeliverResponse pdu)
        {
            throw new NotImplementedException();
        }

        internal override DeliverResponse Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}

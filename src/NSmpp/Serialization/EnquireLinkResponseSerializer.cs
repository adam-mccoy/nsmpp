using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class EnquireLinkResponseSerializer : PduSerializer<EnquireLinkResponse>
    {
        internal override byte[] Serialize(EnquireLinkResponse pdu)
        {
            throw new NotImplementedException();
        }

        internal override EnquireLinkResponse Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}

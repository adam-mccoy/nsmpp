using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class QueryResponseSerializer : PduSerializer<QueryResponse>
    {
        internal override QueryResponse Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        internal override byte[] Serialize(QueryResponse pdu)
        {
            throw new NotImplementedException();
        }
    }
}

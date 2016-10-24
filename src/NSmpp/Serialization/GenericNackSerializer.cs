using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class GenericNackSerializer : PduSerializer<GenericNack>
    {
        internal override byte[] Serialize(GenericNack pdu)
        {
            throw new NotImplementedException();
        }

        internal override GenericNack Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}

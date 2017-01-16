using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class EnquireLinkSerializer : PduSerializer<EnquireLink>
    {
        internal override byte[] Serialize(EnquireLink pdu)
        {
            throw new NotImplementedException();
        }

        internal override EnquireLink Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}

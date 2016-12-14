using NSmpp.Helpers;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class DeliverSerializer : PduSerializer<Deliver>
    {
        internal override byte[] Serialize(Deliver pdu)
        {
            throw new NotImplementedException();
        }

        internal override Deliver Deserialize(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}

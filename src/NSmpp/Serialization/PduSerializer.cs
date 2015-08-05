using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal abstract class PduSerializer<T> : IPduSerializer
        where T : PduBase
    {
        internal abstract byte[] Serialize(T pdu);

        internal abstract T Deserialize(byte[] bytes);

        byte[] IPduSerializer.Serialize(PduBase pdu)
        {
            return Serialize((T)pdu);
        }

        PduBase IPduSerializer.Deserialize(byte[] bytes)
        {
            return Deserialize(bytes);
        }
    }
}

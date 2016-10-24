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

        protected byte[] Finalize(PduWriter writer)
        {
            var bytes = writer.GetBytes();
            PduWriter.WriteInteger(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}

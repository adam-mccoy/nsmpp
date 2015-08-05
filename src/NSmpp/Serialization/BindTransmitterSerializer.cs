using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class BindTransmitterSerializer : PduSerializer<BindTransmitter>
    {
        internal override byte[] Serialize(BindTransmitter pdu)
        {
            var builder = new PduWriter(pdu.Length);
            builder.WritePduHeader(pdu);
            return builder.GetBytes();
        }

        internal override BindTransmitter Deserialize(byte[] bytes)
        {
            throw new System.NotImplementedException();
        }
    }
}

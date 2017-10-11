using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class EnquireLinkSerializer : PduSerializer<EnquireLink>
    {
        internal override byte[] Serialize(EnquireLink pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);

            return Finalize(writer);
        }

        internal override EnquireLink Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            reader.Skip(12);
            var sequence = (uint)reader.ReadInteger();

            return new EnquireLink
            {
                SequenceNumber = sequence
            };
        }
    }
}

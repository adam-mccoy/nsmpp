using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class EnquireLinkResponseSerializer : PduSerializer<EnquireLinkResponse>
    {
        internal override byte[] Serialize(EnquireLinkResponse pdu)
        {
            var writer = new PduWriter(16);
            writer.WritePduHeader(pdu);

            return Finalize(writer);
        }

        internal override EnquireLinkResponse Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            reader.Skip(8); // skip length and command id
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();

            return new EnquireLinkResponse
            {
                SequenceNumber = sequence,
                Status = status
            };
        }
    }
}

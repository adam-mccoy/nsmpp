using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class BindTransmitterResponseSerializer : PduSerializer<BindTransmitterResponse>
    {
        internal override byte[] Serialize(BindTransmitterResponse pdu)
        {
            var writer = new PduWriter(pdu.Length);
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.SystemId);
            return writer.GetBytes();
        }

        internal override BindTransmitterResponse Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            var length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = reader.ReadInteger();
            var systemId = reader.ReadString();

            return new BindTransmitterResponse(
                length,
                command,
                status,
                sequence,
                systemId);
        }
    }
}

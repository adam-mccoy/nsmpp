using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class BindTransceiverResponseSerializer : PduSerializer<BindTransceiverResponse>
    {
        internal override byte[] Serialize(BindTransceiverResponse pdu)
        {
            var writer = new PduWriter(pdu.Length);
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.SystemId);
            return writer.GetBytes();
        }

        internal override BindTransceiverResponse Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            var length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();
            var systemId = reader.ReadString();

            return new BindTransceiverResponse(status, sequence, systemId);
        }
    }
}

using NSmpp.Pdu;

namespace NSmpp.Serialization;

internal class BindTransceiverResponseSerializer : PduSerializer<BindTransceiverResponse>
{
    internal override byte[] Serialize(BindTransceiverResponse pdu)
    {
        var writer = new PduWriter();
        writer.WritePduHeader(pdu);

        if (pdu.Status == SmppStatus.Ok)
            writer.WriteString(pdu.SystemId);

        var bytes = writer.GetBytes();
        PduWriter.WriteInt32(bytes, 0, bytes.Length);
        return bytes;
    }

    internal override BindTransceiverResponse Deserialize(byte[] bytes)
    {
        try
        {
            var reader = new PduReader(bytes);
            var header = reader.ReadHeader();

            if (header.Status != SmppStatus.Ok)
                return new BindTransceiverResponse(header.Status) { SequenceNumber = header.Sequence };
            var systemId = reader.ReadString();

            return new BindTransceiverResponse(header.Status, systemId) { SequenceNumber = header.Sequence };
        }
        catch (Exception ex)
        {
            throw new PduSerializationException(ex);
        }
    }
}
using NSmpp.Pdu;

namespace NSmpp.Serialization;

internal class BindReceiverResponseSerializer : PduSerializer<BindReceiverResponse>
{
    internal override byte[] Serialize(BindReceiverResponse pdu)
    {
        var writer = new PduWriter();
        writer.WritePduHeader(pdu);
        if (pdu.Status == SmppStatus.Ok)
            writer.WriteString(pdu.SystemId);

        var bytes = writer.GetBytes();
        PduWriter.WriteInt32(bytes, 0, bytes.Length);
        return bytes;
    }

    internal override BindReceiverResponse Deserialize(byte[] bytes)
    {
        try
        {
            var reader = new PduReader(bytes);
            var header = reader.ReadHeader();

            if (header.Status != SmppStatus.Ok)
                return new BindReceiverResponse(header.Status) { SequenceNumber = header.Sequence };

            var systemId = reader.ReadString();

            return new BindReceiverResponse(header.Status, systemId) { SequenceNumber = header.Sequence };
        }
        catch (Exception ex)
        {
            throw new PduSerializationException(ex);
        }
    }
}
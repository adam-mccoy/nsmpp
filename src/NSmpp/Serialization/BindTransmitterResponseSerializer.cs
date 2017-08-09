using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class BindTransmitterResponseSerializer : PduSerializer<BindTransmitterResponse>
    {
        internal override byte[] Serialize(BindTransmitterResponse pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);

            if (pdu.Status == SmppStatus.Ok)
                writer.WriteString(pdu.SystemId);

            return Finalize(writer);
        }

        internal override BindTransmitterResponse Deserialize(byte[] bytes)
        {
            try
            {
                var reader = new PduReader(bytes);
                var header = reader.ReadHeader();

                if (header.Status != SmppStatus.Ok)
                    return new BindTransmitterResponse(header.Status, header.Sequence);

                var systemId = reader.ReadString();
                return new BindTransmitterResponse(header.Status, header.Sequence, systemId);
            }
            catch (Exception ex)
            {
                throw new PduSerializationException(ex);
            }
        }
    }
}

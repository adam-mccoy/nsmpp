﻿using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class BindTransmitterResponseSerializer : PduSerializer<BindTransmitterResponse>
    {
        internal override byte[] Serialize(BindTransmitterResponse pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);
            writer.WriteString(pdu.SystemId);

            var bytes = writer.GetBytes();
            PduWriter.WriteInteger(bytes, 0, bytes.Length);
            return bytes;
        }

        internal override BindTransmitterResponse Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            var length = reader.ReadInteger();
            var command = (SmppCommand)reader.ReadInteger();
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();
            var systemId = reader.ReadString();

            return new BindTransmitterResponse(status, sequence, systemId);
        }
    }
}

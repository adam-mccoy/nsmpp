using System;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal class CancelResponseSerializer : PduSerializer<CancelResponse>
    {
        internal override byte[] Serialize(CancelResponse pdu)
        {
            var writer = new PduWriter();
            writer.WritePduHeader(pdu);

            return Finalize(writer);
        }

        internal override CancelResponse Deserialize(byte[] bytes)
        {
            var reader = new PduReader(bytes);
            reader.Skip(8); // skip length and command
            var status = (SmppStatus)reader.ReadInteger();
            var sequence = (uint)reader.ReadInteger();

            return new CancelResponse(status, sequence);
        }
    }
}

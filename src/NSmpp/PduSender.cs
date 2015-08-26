using NSmpp.Pdu;
using NSmpp.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace NSmpp
{
    internal class PduSender
    {
        private readonly Stream _outputStream;

        internal PduSender(Stream outputStream)
        {
            _outputStream = outputStream;
        }

        internal async Task SendAsync(PduBase pdu)
        {
            var serializer = PduSerializerFactory.Create(pdu.Command);
            var bytes = serializer.Serialize(pdu);
            await _outputStream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}

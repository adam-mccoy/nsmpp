using System.IO;

namespace NSmpp
{
    internal class SmppSession
    {
        private readonly Stream _inputStream;
        private readonly Stream _outputStream;

        public SmppSession(Stream inputStream, Stream outputStream)
        {
            _inputStream = inputStream;
            _outputStream = outputStream;
        }
    }
}

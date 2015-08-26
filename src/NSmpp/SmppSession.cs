using System.IO;

namespace NSmpp
{
    internal class SmppSession
    {
        private readonly Stream _inputStream;
        private readonly Stream _outputStream;
        private SessionState _state;

        public SmppSession(Stream inputStream, Stream outputStream)

        internal SessionState SessionState
        {
            get { return _state; }
        }
        {
            _inputStream = inputStream;
            _outputStream = outputStream;
        }
    }
}

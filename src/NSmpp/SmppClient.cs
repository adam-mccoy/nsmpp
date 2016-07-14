using System.Net.Sockets;
using System.Threading.Tasks;
using NSmpp.Extensions;
using NSmpp.Pdu;

namespace NSmpp
{
    public class SmppClient
    {
        private SmppSession _currentSession = null;

        public async Task Connect(string host, int port)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(host, port);

            var stream = new NetworkStream(socket);
            _currentSession = new SmppSession(stream, stream);
        }

        public Task Bind(BindType type, string systemId, string password)
        {
            return _currentSession.Bind(type, systemId, password);
        }
    }
}

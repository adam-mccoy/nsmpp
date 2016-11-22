using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NSmpp.Extensions;

namespace NSmpp
{
    public class SmppClient : IDisposable
    {
        private SmppSession _currentSession = null;

        public async Task Connect(string host, int port)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(host, port);

            var stream = new NetworkStream(socket, true);
            _currentSession = new SmppSession(stream, stream);
        }

        public Task<BindResult> Bind(BindType type, string systemId, string password)
        {
            return _currentSession.Bind(type, systemId, password);
        }

        public Task<SubmitResult> Submit(string source, string dest, string message)
        {
            return _currentSession.Submit(source, dest, message);
        }

        public Task<QueryResult> Query(string messageId, Address source)
        {
            return _currentSession.Query(messageId, source);
        }

        public Task Cancel(string messageId, Address source, Address destination)
        {
            return _currentSession.Cancel(messageId, source, destination);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _currentSession.Dispose();
                _currentSession = null;
            }
        }
    }
}

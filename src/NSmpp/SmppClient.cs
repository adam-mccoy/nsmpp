using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NSmpp.Extensions;

namespace NSmpp
{
    public class SmppClient : IDisposable
    {
        public event EventHandler<DeliverReceivedEventArgs> DeliverReceived;

        private SmppSession _currentSession = null;

        public TimeSpan EnquireLinkFrequency
        {
            get
            {
                EnsureSession();
                return _currentSession.EnquireLinkFrequency;
            }
            set
            {
                EnsureSession();
                _currentSession.EnquireLinkFrequency = value;
            }
        }

        public async Task Connect(string host, int port)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(host, port);

            var stream = new NetworkStream(socket, true);
            _currentSession = new SmppSession(stream, stream);
            _currentSession.DeliverReceived += Session_DeliverReceived;
        }

        public Task<BindResult> Bind(BindType type, string systemId, string password)
        {
            return _currentSession.Bind(type, systemId, password);
        }

        public Task<SubmitResult> Submit(string source, string dest, string message)
        {
            return _currentSession.Submit(source, dest, message);
        }

        public Task<SubmitResult> Submit(SubmitBuilder builder)
        {
            return _currentSession.Submit(builder.Build());
        }

        public Task<QueryResult> Query(string messageId, Address source)
        {
            return _currentSession.Query(messageId, source);
        }

        public Task Cancel(string messageId, Address source, Address destination)
        {
            return _currentSession.Cancel(messageId, source, destination);
        }

        public Task EnquireLink()
        {
            return _currentSession.EnquireLink();
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
                if (_currentSession != null)
                {
                    _currentSession.Dispose();
                    _currentSession = null;
                }
            }
        }

        private void EnsureSession()
        {
            if (_currentSession == null)
                throw new InvalidOperationException("Client is not connected.");
        }

        private void Session_DeliverReceived(object sender, DeliverReceivedEventArgs e)
        {
            OnDeliverReceived(e);
        }

        private void OnDeliverReceived(DeliverReceivedEventArgs e)
        {
            DeliverReceived?.Invoke(this, e);
        }
    }
}

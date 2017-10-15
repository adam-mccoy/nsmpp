using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NSmpp.Extensions;
using NSmpp.Helpers;
using NSmpp.Pdu;

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
            var pdu = BindHelper.CreateBindPdu(type, systemId, password, new BindOptions());
            return _currentSession.SendPdu<BindResult>(pdu);
        }

        public Task<SubmitResult> Submit(string source, string dest, string message)
        {
            var pdu = new Submit
            {
                ServiceType = null,
                Source = source,
                Destination = dest,
                EsmClass = 0,
                PriorityFlag = 0,
                ProtocolId = 0,
                ShortMessage = System.Text.Encoding.ASCII.GetBytes(message)
            };
            return _currentSession.SendPdu<SubmitResult>(pdu);
        }

        public Task<QueryResult> Query(string messageId, Address source)
        {
            var pdu = new Query
            {
                MessageId = messageId,
                Source = source
            };
            return _currentSession.SendPdu<QueryResult>(pdu);
        }

        public Task Cancel(string messageId, Address source, Address destination)
        {
            var pdu = new Cancel
            {
                ServiceType = null,
                MessageId = messageId,
                Source = source,
                Destination = destination
            };
            return _currentSession.SendPdu(pdu);
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

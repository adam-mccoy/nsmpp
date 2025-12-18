using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using NSmpp.Helpers;
using NSmpp.Pdu;

namespace NSmpp;

public class SmppClient : IAsyncDisposable
{
    public event EventHandler<DeliverReceivedEventArgs>? DeliverReceived;

    private SmppSession? _currentSession;

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
        EnsureSession();
        var pdu = BindHelper.CreateBindPdu(type, systemId, password, new BindOptions());
        return _currentSession.SendPdu<BindResult>(pdu);
    }

    public Task<SubmitResult> Submit(string source, string dest, string message)
    {
        EnsureSession();
        var pdu = new Submit(
            null,
            TypeOfNumber.Unknown,
            NumericPlanIndicator.Unknown,
            source,
            TypeOfNumber.Unknown,
            NumericPlanIndicator.Unknown,
            dest,
            0,
            0,
            0,
            message);

        return _currentSession.SendPdu<SubmitResult>(pdu);
    }

    public Task<QueryResult> Query(string messageId, Address source)
    {
        EnsureSession();
        var pdu = new Query(messageId, source);
        return _currentSession.SendPdu<QueryResult>(pdu);
    }

    public Task Cancel(string messageId, Address source, Address destination)
    {
        EnsureSession();
        var pdu = new Cancel(
            null,
            messageId,
            source,
            destination);

        return _currentSession.SendPdu(pdu);
    }

    public Task EnquireLink()
    {
        EnsureSession();
        return _currentSession.EnquireLink();
    }

    [MemberNotNull(nameof(_currentSession))]
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

    public async ValueTask DisposeAsync()
    {
        if (_currentSession != null) await _currentSession.DisposeAsync();
    }
}
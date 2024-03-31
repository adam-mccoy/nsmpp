using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;

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
        return _currentSession.Bind(type, systemId, password);
    }

    public Task<SubmitResult> Submit(string source, string dest, string message)
    {
        EnsureSession();
        return _currentSession.Submit(source, dest, message);
    }

    public Task<QueryResult> Query(string messageId, Address source)
    {
        EnsureSession();
        return _currentSession.Query(messageId, source);
    }

    public Task Cancel(string messageId, Address source, Address destination)
    {
        EnsureSession();
        return _currentSession.Cancel(messageId, source, destination);
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
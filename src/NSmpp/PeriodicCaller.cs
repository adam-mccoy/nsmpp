namespace NSmpp;

internal class PeriodicCaller : IDisposable
{
    private bool _isDisposed;
    private readonly Timer _timer;

    public PeriodicCaller(TimeSpan delay, Action callback)
    {
        Delay = delay;
        _timer = new Timer(OnTick, callback, Timeout.InfiniteTimeSpan, delay);
    }

    public TimeSpan Delay { get; }

    public void Reset()
    {
        EnsureNotDisposed();
        _timer.Change(Delay, Delay);
    }

    public void Stop()
    {
        EnsureNotDisposed();
        _timer.Change(Timeout.InfiniteTimeSpan, Delay);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
        {
            var e = new ManualResetEvent(false);
            _timer.Dispose(e);
            e.WaitOne();
            _isDisposed = true;
        }
    }

    private void OnTick(object state)
    {
        if (_isDisposed)
            return;

        var callback = (Action)state;
        callback();
    }

    private void EnsureNotDisposed()
    {
        if (_isDisposed)
            throw new ObjectDisposedException("PeriodicCaller");
    }
}
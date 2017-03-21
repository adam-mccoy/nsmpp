using System;
using System.Threading;

namespace NSmpp
{
    internal class PeriodicCaller : IDisposable
    {
        private TimeSpan _delay;
        private bool _isDisposed;
        private readonly Timer _timer;

        public PeriodicCaller(TimeSpan delay, Action callback)
        {
            _delay = delay;
            _timer = new Timer(OnTick, callback, Timeout.InfiniteTimeSpan, delay);
        }

        public TimeSpan Delay => _delay;

        public void Reset()
        {
            EnsureNotDisposed();
            _timer.Change(_delay, _delay);
        }

        public void Stop()
        {
            EnsureNotDisposed();
            _timer.Change(Timeout.InfiniteTimeSpan, _delay);
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
}

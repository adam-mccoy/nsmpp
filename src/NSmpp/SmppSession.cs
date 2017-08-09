using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NSmpp.Helpers;
using NSmpp.Pdu;

namespace NSmpp
{
    internal class SmppSession : IPduReceivedHandler, IDisposable
    {
        private readonly TaskRegistry _taskRegistry = new TaskRegistry();

        private SessionState _state;
        private int _sequenceNumber = 0;

        private PduSender _pduSender;
        private PduReceiver _pduReceiver;
        private Task _receiverTask;
        private Task _senderTask;

        private PeriodicCaller _enquireLinkRunner;

        internal event EventHandler<DeliverReceivedEventArgs> DeliverReceived;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal SmppSession(Stream inputStream, Stream outputStream)
        {
            _pduSender = new PduSender(outputStream);
            _pduReceiver = new PduReceiver(inputStream, this);
            _receiverTask = _pduReceiver.Start().ContinueWith(t =>
            {
                _state = SessionState.Closed;
                Console.WriteLine("Receiver task completed.");
                if (t.IsFaulted)
                    Console.WriteLine("Failed with exception: {0}", t.Exception.Flatten());
            });
            _senderTask = _pduSender.Start();
        }

        internal SessionState SessionState
        {
            get { return _state; }
        }

        internal TimeSpan EnquireLinkFrequency
        {
            get
            {
                return _enquireLinkRunner?.Delay ?? Timeout.InfiniteTimeSpan;
            }
            set
            {
                if (value == Timeout.InfiniteTimeSpan)
                {
                    _enquireLinkRunner?.Stop();
                    _enquireLinkRunner = null;
                }
                else
                {
                    _enquireLinkRunner = new PeriodicCaller(value, async () => await EnquireLink());
                    if (SessionState.IsBound())
                        _enquireLinkRunner.Reset();
                }

            }
        }

        internal void Close()
        {
            if (_state != SessionState.Open && _state != SessionState.Closed)
                AsyncHelper.RunSync(Unbind);
            _enquireLinkRunner?.Stop();
            _pduSender.Stop();
            _pduReceiver.Stop();
        }

        internal Task<BindResult> Bind(BindType type, string systemId, string password)
        {
            return Bind(type, systemId, password, new BindOptions());
        }

        internal Task<BindResult> Bind(BindType type, string systemId, string password, BindOptions options)
        {
            EnsureOpen();

            var sequence = GetNextSequenceNumber();
            var pdu = BindHelper.CreateBindPdu(sequence, type, systemId, password, options);
            var task = _taskRegistry.Register<BindResult>(sequence);

            _pduSender.Enqueue(pdu);
            _enquireLinkRunner?.Reset();
            return task.GetTask<BindResult>();
        }

        private void EnsureOpen()
        {
            if (_state != SessionState.Open)
                throw new InvalidOperationException("This operation can only be performed when the session is in the Open state.");
        }

        internal Task Unbind()
        {
            EnsureBound();
            var sequence = GetNextSequenceNumber();
            var task = _taskRegistry.Register(sequence);
            var pdu = new Unbind(sequence);

            _pduSender.Enqueue(pdu);
            _enquireLinkRunner?.Reset();
            return task.GetTask();
        }

        internal Task<SubmitResult> Submit(string source, string dest, string message)
        {
            EnsureCanTransmit();
            var sequence = GetNextSequenceNumber();
            var task = _taskRegistry.Register<SubmitResult>(sequence);
            var pdu = new Submit(
                sequence,
                null,
                source,
                dest,
                0, 0, 0,
                message);

            _pduSender.Enqueue(pdu);
            _enquireLinkRunner?.Reset();
            return task.GetTask<SubmitResult>();
        }

        internal Task<QueryResult> Query(string messageId, Address source)
        {
            EnsureCanTransmit();
            var sequence = GetNextSequenceNumber();
            var task = _taskRegistry.Register<QueryResult>(sequence);
            var pdu = new Query(sequence, messageId, source);

            _pduSender.Enqueue(pdu);
            _enquireLinkRunner?.Reset();
            return task.GetTask<QueryResult>();
        }

        internal Task Cancel(string messageId, Address source, Address destination)
        {
            EnsureCanTransmit();
            var sequence = GetNextSequenceNumber();
            var task = _taskRegistry.Register(sequence);
            var pdu = new Cancel(sequence, null, messageId, source, destination);

            _pduSender.Enqueue(pdu);
            _enquireLinkRunner?.Reset();
            return task.GetTask();
        }

        internal Task EnquireLink()
        {
            EnsureBound();
            var sequence = GetNextSequenceNumber();
            var task = _taskRegistry.Register(sequence);
            var pdu = new EnquireLink(sequence);

            _pduSender.Enqueue(pdu);
            _enquireLinkRunner?.Reset();
            return task.GetTask();
        }

        private void EnsureBound()
        {
            if (!_state.IsBound())
                throw new InvalidOperationException("This operation can only be performed when the session is in the Bound state.");
        }

        private void EnsureCanTransmit()
        {
            if (_state != SessionState.BoundTransmitter && _state != SessionState.BoundTransceiver)
                throw new InvalidOperationException("This operation can only be performed when the session is bound as a transmitter or transceiver.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
                _enquireLinkRunner?.Dispose();
            }
        }

        void IPduReceivedHandler.HandlePdu(GenericNack pdu)
        {
            _enquireLinkRunner?.Reset();
            var task = _taskRegistry.Unregister(pdu.SequenceNumber);
            if (task == null)
                return;

            var exception = new Exception("The operation failed with a generic error: " + pdu.Status);
            task.SetException(exception);
        }

        void IPduReceivedHandler.HandlePdu(BindReceiverResponse pdu)
        {
            var task = _taskRegistry.Unregister(pdu.SequenceNumber);
            if (task == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var bindException = new Exception("The bind operation failed with error code: " + pdu.Status);
                task.SetException(bindException);
            }
            else
            {
                _enquireLinkRunner?.Reset();
                _state = SessionState.BoundReceiver;
                task.SetResult(new BindResult(pdu.SystemId));
            }
        }

        void IPduReceivedHandler.HandlePdu(BindTransmitterResponse pdu)
        {
            var task = _taskRegistry.Unregister(pdu.SequenceNumber);
            if (task == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var bindException = new Exception("The bind operation failed with error code: " + pdu.Status);
                task.SetException(bindException);
            }
            else
            {
                _enquireLinkRunner?.Reset();
                _state = SessionState.BoundTransmitter;
                task.SetResult(new BindResult(pdu.SystemId));
            }
        }

        void IPduReceivedHandler.HandlePdu(BindTransceiverResponse pdu)
        {
            var task = _taskRegistry.Unregister(pdu.SequenceNumber);
            if (task == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var unbindException = new Exception("The unbind operation failed with error code: " + pdu.Status);
                task.SetException(unbindException);
            }
            else
            {
                _state = SessionState.BoundTransceiver;
                _enquireLinkRunner?.Reset();
                task.SetResult(new BindResult(pdu.SystemId));
            }
        }

        void IPduReceivedHandler.HandlePdu(Unbind pdu)
        {
            var response = new UnbindResponse(SmppStatus.Ok, pdu.SequenceNumber);
            _pduSender.Enqueue(response);
            // TODO: cancel outstanding tasks
            _enquireLinkRunner?.Stop();
            _state = SessionState.Open;
        }

        void IPduReceivedHandler.HandlePdu(UnbindResponse pdu)
        {
            var task = _taskRegistry.Unregister(pdu.SequenceNumber);
            if (task == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var unbindException = new Exception("The unbind operation failed with error code: " + pdu.Status);
                task.SetException(unbindException);
            }
            else
            {
                _enquireLinkRunner?.Stop();
                _state = SessionState.Open;
                task.SetResult(true);
            }
        }

        void IPduReceivedHandler.HandlePdu(SubmitResponse pdu)
        {
            _enquireLinkRunner?.Reset();
            var task = _taskRegistry.Unregister(pdu.SequenceNumber);
            if (task == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var exception = new Exception("The submit operation failed with error code: " + pdu.Status);
                task.SetException(exception);
            }
            else
            {
                task.SetResult(new SubmitResult(pdu.MessageId));
            }
        }

        void IPduReceivedHandler.HandlePdu(QueryResponse pdu)
        {
            _enquireLinkRunner?.Reset();
            var task = _taskRegistry.Unregister(pdu.SequenceNumber);
            if (task == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var exception = new Exception("The query operation failed with the error: " + pdu.Status);
                task.SetException(exception);
            }
            else
            {
                var result = new QueryResult(pdu.MessageId, pdu.FinalDate, pdu.MessageState, pdu.ErrorCode);
                task.SetResult(result);
            }
        }

        void IPduReceivedHandler.HandlePdu(Deliver pdu)
        {
            _enquireLinkRunner?.Reset();
            OnDeliverReceived(pdu.Source, pdu.Destination, pdu.ShortMessage);

            var response = new DeliverResponse(SmppStatus.Ok, pdu.SequenceNumber);
            _pduSender.Enqueue(response);
        }

        void IPduReceivedHandler.HandlePdu(CancelResponse pdu)
        {
            _enquireLinkRunner?.Reset();
            var task = _taskRegistry.Unregister(pdu.SequenceNumber);
            if (task == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var exception = new Exception("The cancel operation failed with the error: " + pdu.Status);
                task.SetException(exception);
            }
            else
            {
                task.SetResult();
            }
        }

        void IPduReceivedHandler.HandlePdu(EnquireLink pdu)
        {
            _enquireLinkRunner?.Reset();
            var response = new EnquireLinkResponse(SmppStatus.Ok, pdu.SequenceNumber);
            _pduSender.Enqueue(response);
        }

        void IPduReceivedHandler.HandlePdu(EnquireLinkResponse pdu)
        {
            _enquireLinkRunner?.Reset();
            var task = _taskRegistry.Unregister(pdu.SequenceNumber);
            if (task == null)
                return;

            task.SetResult();
        }

        void IPduReceivedHandler.HandleError(byte[] buffer, string error)
        {
        }

        private uint GetNextSequenceNumber()
        {
            return (uint)Interlocked.Increment(ref _sequenceNumber);
        }

        private void OnDeliverReceived(Address source, Address destination, string message)
        {
            DeliverReceived?.Invoke(this, new DeliverReceivedEventArgs(source, destination, message));
        }
    }
}

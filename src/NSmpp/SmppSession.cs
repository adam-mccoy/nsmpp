using NSmpp.Helpers;
using NSmpp.Pdu;

namespace NSmpp
{
    internal class SmppSession : IPduReceivedHandler, IAsyncDisposable
    {
        private readonly TaskRegistry _taskRegistry = new();

        private int _sequenceNumber;

        private readonly PduSender _pduSender;
        private readonly PduReceiver _pduReceiver;
        private readonly Task? _receiverTask;
        private readonly Task? _senderTask;

        private PeriodicCaller? _enquireLinkRunner;

        internal event EventHandler<DeliverReceivedEventArgs>? DeliverReceived;

        internal SmppSession(Stream inputStream, Stream outputStream)
        {
            _pduSender = new PduSender(outputStream);
            _pduReceiver = new PduReceiver(inputStream, this);
            _receiverTask = _pduReceiver.Start().ContinueWith(t =>
            {
                SessionState = SessionState.Closed;
                Console.WriteLine("Receiver task completed.");
                if (t.IsFaulted)
                    Console.WriteLine("Failed with exception: {0}", t.Exception!.Flatten());
            });
            _senderTask = _pduSender.Start();
        }

        internal SessionState SessionState { get; private set; }

        internal TimeSpan EnquireLinkFrequency
        {
            get => _enquireLinkRunner?.Delay ?? Timeout.InfiniteTimeSpan;
            set
            {
                if (value == Timeout.InfiniteTimeSpan)
                {
                    _enquireLinkRunner?.Stop();
                    _enquireLinkRunner = null;
                }
                else
                {
                    _enquireLinkRunner = new PeriodicCaller(value, () => EnquireLink());
                    if (SessionState.IsBound())
                        _enquireLinkRunner.Reset();
                }
            }
        }

        internal void Close()
        {
            if (SessionState != SessionState.Open && SessionState != SessionState.Closed)
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
            if (SessionState != SessionState.Open)
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
                TypeOfNumber.Unknown,
                NumericPlanIndicator.Unknown,
                source,
                TypeOfNumber.Unknown,
                NumericPlanIndicator.Unknown,
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
            if (!SessionState.IsBound())
                throw new InvalidOperationException("This operation can only be performed when the session is in the Bound state.");
        }

        private void EnsureCanTransmit()
        {
            if (SessionState != SessionState.BoundTransmitter && SessionState != SessionState.BoundTransceiver)
                throw new InvalidOperationException("This operation can only be performed when the session is bound as a transmitter or transceiver.");
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
                SessionState = SessionState.BoundReceiver;
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
                SessionState = SessionState.BoundTransmitter;
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
                SessionState = SessionState.BoundTransceiver;
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
            SessionState = SessionState.Open;
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
                SessionState = SessionState.Open;
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

        public async ValueTask DisposeAsync()
        {
            if (_receiverTask != null) await CastAndDispose(_receiverTask);
            if (_senderTask != null) await CastAndDispose(_senderTask);
            if (_enquireLinkRunner != null) await CastAndDispose(_enquireLinkRunner);

            return;

            static async ValueTask CastAndDispose(IDisposable resource)
            {
                if (resource is IAsyncDisposable resourceAsyncDisposable)
                    await resourceAsyncDisposable.DisposeAsync();
                else
                    resource.Dispose();
            }
        }
    }
}

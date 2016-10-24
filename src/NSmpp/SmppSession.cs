using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NSmpp.Helpers;
using NSmpp.Pdu;

namespace NSmpp
{
    internal class SmppSession : IPduReceivedHandler, IDisposable
    {
        private readonly ConcurrentDictionary<uint, object> _outstandingTasks = new ConcurrentDictionary<uint, object>();

        private SessionState _state;
        private int _sequenceNumber = 0;

        private PduSender _pduSender;
        private PduReceiver _pduReceiver;
        private Task _receiverTask;
        private Task _senderTask;

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

        internal void Close()
        {
            if (_state != SessionState.Open && _state != SessionState.Closed)
                AsyncHelper.RunSync(Unbind);
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
            var tcs = RegisterTask<BindResult>(sequence);

            _pduSender.Enqueue(pdu);
            return tcs.Task;
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
            var tcs = RegisterTask<bool>(sequence);
            var pdu = new Unbind(sequence);

            _pduSender.Enqueue(pdu);
            return tcs.Task;
        }

        internal Task<SubmitResult> Submit(string source, string dest, string message)
        {
            EnsureCanTransmit();
            var sequence = GetNextSequenceNumber();
            var tcs = RegisterTask<SubmitResult>(sequence);
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
            return tcs.Task;
        }

        internal Task<QueryResult> Query(string messageId, Address source)
        {
            EnsureCanTransmit();
            var sequence = GetNextSequenceNumber();
            var tcs = RegisterTask<QueryResult>(sequence);
            var pdu = new Query(sequence, messageId, source);

            _pduSender.Enqueue(pdu);
            return tcs.Task;
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
            }
        }

        private TaskCompletionSource<T> RegisterTask<T>(uint sequenceNumber)
        {
            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            if (!_outstandingTasks.TryAdd(sequenceNumber, tcs))
                throw new ArgumentException("A task with the same sequence number has already been registerd.");

            return tcs;
        }

        private TaskCompletionSource<T> RetrieveTask<T>(uint sequenceNumber)
        {
            object tcs;
            if (!_outstandingTasks.TryRemove(sequenceNumber, out tcs))
                return null;

            return (TaskCompletionSource<T>)tcs;
        }

        void IPduReceivedHandler.HandlePdu(BindReceiverResponse pdu)
        {
            var tcs = RetrieveTask<BindResult>(pdu.SequenceNumber);
            if (tcs == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var bindException = new Exception("The bind operation failed with error code: " + pdu.Status);
                tcs.SetException(bindException);
            }
            else
            {
                _state = SessionState.BoundReceiver;
                tcs.SetResult(new BindResult(pdu.SystemId));
            }
        }

        void IPduReceivedHandler.HandlePdu(BindTransmitterResponse pdu)
        {
            var tcs = RetrieveTask<BindResult>(pdu.SequenceNumber);
            if (tcs == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var bindException = new Exception("The bind operation failed with error code: " + pdu.Status);
                tcs.SetException(bindException);
            }
            else
            {
                _state = SessionState.BoundTransmitter;
                tcs.SetResult(new BindResult(pdu.SystemId));
            }
        }

        void IPduReceivedHandler.HandlePdu(BindTransceiverResponse pdu)
        {
            var tcs = RetrieveTask<BindResult>(pdu.SequenceNumber);
            if (tcs == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var unbindException = new Exception("The unbind operation failed with error code: " + pdu.Status);
                tcs.SetException(unbindException);
            }
            else
            {
                _state = SessionState.BoundTransceiver;
                tcs.SetResult(new BindResult(pdu.SystemId));
            }
        }

        void IPduReceivedHandler.HandlePdu(Unbind pdu)
        {
            var response = new UnbindResponse(SmppStatus.Ok, pdu.SequenceNumber);
            _pduSender.Enqueue(response);
            // TODO: cancel outstanding tasks
            _state = SessionState.Open;
        }

        void IPduReceivedHandler.HandlePdu(UnbindResponse pdu)
        {
            var tcs = RetrieveTask<bool>(pdu.SequenceNumber);
            if (tcs == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var unbindException = new Exception("The unbind operation failed with error code: " + pdu.Status);
                tcs.SetException(unbindException);
            }
            else
            {
                _state = SessionState.Open;
                tcs.SetResult(true);
            }
        }

        void IPduReceivedHandler.HandlePdu(SubmitResponse pdu)
        {
            var tcs = RetrieveTask<SubmitResult>(pdu.SequenceNumber);
            if (tcs == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var exception = new Exception("The submit operation failed with error code: " + pdu.Status);
                tcs.SetException(exception);
            }
            else
            {
                tcs.SetResult(new SubmitResult(pdu.MessageId));
            }
        }

        void IPduReceivedHandler.HandlePdu(QueryResponse pdu)
        {
            var tcs = RetrieveTask<QueryResult>(pdu.SequenceNumber);
            if (tcs == null)
                return;

            if (pdu.Status != SmppStatus.Ok)
            {
                var exception = new Exception("The query operation failed with the error: " + pdu.Status);
                tcs.SetException(exception);
            }
            else
            {
                var result = new QueryResult(pdu.MessageId, pdu.FinalDate, pdu.MessageState, pdu.ErrorCode);
                tcs.SetResult(result);
            }
        }

        void IPduReceivedHandler.HandleError(byte[] buffer, string error)
        {
        }

        private uint GetNextSequenceNumber()
        {
            return (uint)Interlocked.Increment(ref _sequenceNumber);
        }
    }
}

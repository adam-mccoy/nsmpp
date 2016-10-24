using System;

namespace NSmpp
{
    /// <summary>
    /// Represents the result of an SMPP query operation.
    /// </summary>
    public class QueryResult
    {
        internal QueryResult(
            string messageId,
            DateTimeOffset? finalDate,
            MessageState state,
            int? errorCode)
        {
            MessageId = messageId;
            FinalDate = finalDate;
            State = state;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the SMSC message ID of the queried message.
        /// </summary>
        public string MessageId { get; private set; }
        /// <summary>
        /// Gets the a <see cref="System.DateTimeOffset"/> representing the date
        /// and time the message reached a final state. Messages that have not yet
        /// reached a final state will have a value of <value>null</value>.
        /// </summary>
        public DateTimeOffset? FinalDate { get; private set; }
        /// <summary>
        /// Gets a <see cref="NSmpp.MessageState"/> representing the message's current
        /// state.
        /// </summary>
        public MessageState State { get; private set; }
        /// <summary>
        /// Gets a value representing the error code returned from the SMSC. Messages that
        /// did not result in an error will have a value of <value>null</value>.
        /// </summary>
        public int? ErrorCode { get; private set; }
    }
}

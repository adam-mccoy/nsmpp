using System;

namespace NSmpp.Pdu
{
    internal class QueryResponse : ResponsePduBase
    {
        public QueryResponse(
            SmppStatus status,
            uint sequenceNumber,
            string messageId,
            DateTime? finalDate,
            MessageState messageState,
            int errorCode)
            : base(status, sequenceNumber)
        {
            MessageId = messageId;
            FinalDate = finalDate;
            MessageState = messageState;
            ErrorCode = errorCode;
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.QueryResp; }
        }

        internal string MessageId { get; private set; }
        internal DateTime? FinalDate { get; private set; }
        internal MessageState MessageState { get; private set; }
        internal int ErrorCode { get; private set; }
    }
}

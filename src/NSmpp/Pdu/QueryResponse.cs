using System;

namespace NSmpp.Pdu
{
    internal class QueryResponse : ResponsePduBase
    {
        internal override SmppCommand Command
        {
            get { return SmppCommand.QueryResp; }
        }

        internal string MessageId { get; set; }
        internal DateTimeOffset? FinalDate { get; set; }
        internal MessageState MessageState { get; set; }
        internal int ErrorCode { get; set; }
    }
}

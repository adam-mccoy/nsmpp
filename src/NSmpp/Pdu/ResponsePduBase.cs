namespace NSmpp.Pdu
{
    internal abstract class ResponsePduBase : PduBase
    {
        internal SmppStatus Status { get; private set; }

        public ResponsePduBase(SmppStatus status, uint sequenceNumber)
            : base(sequenceNumber)
        {
        }
    }
}

namespace NSmpp.Pdu
{
    internal abstract class ResponsePduBase : PduBase
    {
        internal SmppStatus Status { get; set; }
    }
}

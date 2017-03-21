using NSmpp.Pdu;

namespace NSmpp
{
    internal interface IPduReceivedHandler
    {
        void HandlePdu(GenericNack pdu);
        void HandlePdu(BindReceiverResponse pdu);
        void HandlePdu(BindTransmitterResponse pdu);
        void HandlePdu(BindTransceiverResponse pdu);
        void HandlePdu(Unbind pdu);
        void HandlePdu(UnbindResponse pdu);
        void HandlePdu(SubmitResponse pdu);
        void HandlePdu(QueryResponse pdu);
        void HandlePdu(Deliver pdu);
        void HandlePdu(CancelResponse pdu);
        void HandlePdu(EnquireLink pdu);
        void HandlePdu(EnquireLinkResponse pdu);

        void HandleError(byte[] buffer, string error);
    }
}

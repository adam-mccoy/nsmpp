using NSmpp.Pdu;

namespace NSmpp
{
    internal interface IPduReceivedHandler
    {
        void HandlePdu(BindTransmitterResponse pdu);
        void HandlePdu(UnbindResponse pdu);

        void HandleError(byte[] buffer, string error);
    }
}

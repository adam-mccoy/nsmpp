using NSmpp.Pdu;

namespace NSmpp
{
    internal interface IPduReceivedHandler
    {
        void HandlePdu(BindTransmitterResponse pdu);

        void HandleError(byte[] buffer, string error);
    }
}

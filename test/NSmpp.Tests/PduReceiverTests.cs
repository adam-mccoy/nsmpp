using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using NSmpp.Pdu;
using Moq;

namespace NSmpp.Tests
{
    [TestFixture]
    public class PduReceiverTests
    {
        [Test]
        public void Receives_Bind_Transmitter_Resp()
        {
            var e = new ManualResetEvent(false);
            var data = new byte[] {
                0x00, 0x00, 0x00, 0x17, // length
                0x80, 0x00, 0x00, 0x02, // command
                0x00, 0x00, 0x00, 0x00, // status
                0x00, 0x00, 0x00, 0x20, // sequence
                0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x00 // system ID
            };

            var expectedPdu = new BindTransmitterResponse(SmppStatus.Ok, 32, "System");

            PduReceiver receiver = null;
            var handler = new Mock<IPduReceivedHandler>();
            handler.Setup(h => h.HandlePdu(It.IsAny<BindTransmitterResponse>()))
                .Callback(() => e.Set());

            receiver = new PduReceiver(
                new MemoryStream(data),
                handler.Object);

            receiver.Start();
            e.WaitOne();
            handler.Verify(h => h.HandlePdu(expectedPdu));
            receiver.Stop();
        }
    }
}

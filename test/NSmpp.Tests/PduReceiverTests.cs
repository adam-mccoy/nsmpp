using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSmpp;
using NSmpp.Pdu;
using System.IO;
using System.Threading;

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

            PduReceiver receiver = null;
            PduBase receivedPdu = null;
            Action<PduBase> successCallback = p => 
            {
                receivedPdu = p;
                e.Set();
            };
            Action<byte[], string> errorCallback = (b, m) => { };

            receiver = new PduReceiver(new MemoryStream(data), 
                successCallback, errorCallback);

            receiver.Start();
            e.WaitOne();
            receiver.Stop();

            var bindPdu = receivedPdu as BindTransmitterResponse;
            Assert.IsNotNull(bindPdu);
            Assert.AreEqual(23, bindPdu.Length);
            Assert.AreEqual(SmppCommand.BindTransmitterResp, bindPdu.Command);
            Assert.AreEqual(SmppStatus.Ok, bindPdu.Status);
            Assert.AreEqual(32, bindPdu.SequenceNumber);
            Assert.AreEqual("System", bindPdu.SystemId);
        }
    }
}

using NSmpp.Pdu;
using NSubstitute;

namespace NSmpp.Tests;

[TestFixture]
public class PduReceiverTests
{
    [Test]
    public void Receives_Bind_Transmitter_Resp()
    {
        var e = new ManualResetEvent(false);
        byte[] data =
        [
            0x00, 0x00, 0x00, 0x17, // length
            0x80, 0x00, 0x00, 0x02, // command
            0x00, 0x00, 0x00, 0x00, // status
            0x00, 0x00, 0x00, 0x20, // sequence
            0x53, 0x79, 0x73, 0x74, 0x65, 0x6d, 0x00 // system ID
        ];

        var expectedPdu = new BindTransmitterResponse(SmppStatus.Ok, 32, "System");

        var handler = Substitute.For<IPduReceivedHandler>();
        handler.When(x => x.HandlePdu(Arg.Any<BindTransmitterResponse>()))
            .Do(_ => e.Set());

        var receiver = new PduReceiver(
            new MemoryStream(data),
            handler);

        receiver.Start();
        e.WaitOne();
        handler.Received().HandlePdu(expectedPdu);
        receiver.Stop();
    }
}
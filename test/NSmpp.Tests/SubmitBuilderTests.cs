using System;
using NUnit.Framework;

namespace NSmpp.Tests
{
    [TestFixture]
    public class SubmitBuilderTests
    {
        [Test]
        public void Sets_Messaging_Mode_Idempotently()
        {
            var builder = new SubmitBuilder();

            builder.UseMessagingMode(MessagingMode.Datagram);
            Assert.AreEqual(0x01, builder.Build().EsmClass);
            builder.UseMessagingMode(MessagingMode.Forward);
            Assert.AreEqual(0x02, builder.Build().EsmClass);
            builder.UseMessagingMode(MessagingMode.Default);
            Assert.AreEqual(0x00, builder.Build().EsmClass);
        }

        [Test]
        public void Sets_Message_Type_Idempotently()
        {
            var builder = new SubmitBuilder();

            builder.UseMessageType(MessageType.DeliveryAcknowledgement);
            Assert.AreEqual(0x08, builder.Build().EsmClass);
            builder.UseMessageType(MessageType.ManualUserAcknowledgement);
            Assert.AreEqual(0x10, builder.Build().EsmClass);
            builder.UseMessageType(MessageType.Default);
            Assert.AreEqual(0x00, builder.Build().EsmClass);
        }

        [Test]
        public void Sets_Network_Specific_Features_Idempotently()
        {
            var builder = new SubmitBuilder();

            builder.UseGsmFeatures(NetworkSpecificFeatures.SetReplyPath);
            Assert.AreEqual(0x80, builder.Build().EsmClass);
            builder.UseGsmFeatures(NetworkSpecificFeatures.UdhiIndicator);
            Assert.AreEqual(0x40, builder.Build().EsmClass);
            builder.UseGsmFeatures(NetworkSpecificFeatures.SetReplyPath | NetworkSpecificFeatures.UdhiIndicator);
            Assert.AreEqual(0xc0, builder.Build().EsmClass);
            builder.UseGsmFeatures(NetworkSpecificFeatures.None);
            Assert.AreEqual(0x00, builder.Build().EsmClass);
        }

        [Test]
        public void Sets_Absolute_Schedule_Delivery_Time()
        {
            var builder = new SubmitBuilder();

            builder.UseScheduledDeliveryTime(new DateTimeOffset(2017, 8, 18, 1, 23, 45, 720, TimeSpan.FromHours(10.0)));

            Assert.AreEqual("170818012345740+", builder.Build().ScheduleDeliveryTime);
        }
    }
}

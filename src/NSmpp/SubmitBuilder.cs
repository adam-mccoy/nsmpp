using System;
using System.IO;
using NSmpp.Pdu;
using NSmpp.Encoding;

namespace NSmpp
{
    public class SubmitBuilder : ISubmitBuilder
    {
        private Submit _pdu = new Submit();

        public ISubmitBuilder UseServiceType(string serviceType)
        {
            if (serviceType != null && serviceType.Length > 5)
                throw new ArgumentOutOfRangeException(nameof(serviceType), "Cannot exceed 5 characters.");

            _pdu.ServiceType = serviceType;
            return this;
        }

        public ISubmitBuilder UseSource(Address source)
        {
            _pdu.Source = source ?? throw new ArgumentNullException(nameof(source));
            return this;
        }

        public ISubmitBuilder UseDestination(Address destination)
        {
            _pdu.Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            return this;
        }

        public ISubmitBuilder UseMessagingMode(MessagingMode mode)
        {
            throw new NotImplementedException();
        }

        public ISubmitBuilder UseMessageType(MessageType type)
        {
            throw new NotImplementedException();
        }

        public ISubmitBuilder UseGsmFeatures(NetworkSpecificFeatures features)
        {
            throw new NotImplementedException();
        }

        public ISubmitBuilder UseProtocolId(int protocolId)
        {
            _pdu.ProtocolId = protocolId;
            return this;
        }

        public ISubmitBuilder UsePriorityFlag(PriorityFlag priority)
        {
            _pdu.PriorityFlag = priority;
            return this;
        }

        public ISubmitBuilder UseScheduledDeliveryTime(DateTimeOffset absolute)
        {
            var offset = Math.Abs(absolute.Offset.TotalMinutes) / 15;
            string scheduleDeliveryTime = absolute.ToString("YYMMddHHmmssf") +
                offset.ToString("00") +
                (absolute.Offset < TimeSpan.Zero ? "-" : "+");
            return this;
        }

        public ISubmitBuilder UseScheduledDeliveryTime(TimeSpan relative)
        {
            throw new NotImplementedException();
        }

        public ISubmitBuilder UseValidityPeriod(DateTimeOffset absolute)
        {
            var offset = Math.Abs(absolute.Offset.TotalMinutes) / 15;
            string validityPeriod = absolute.ToString("YYMMddHHmmssf") +
                offset.ToString("00") +
                (absolute.Offset < TimeSpan.Zero ? "-" : "+");
            return this;
        }

        public ISubmitBuilder UseValidityPeriod(TimeSpan relative)
        {
            throw new NotImplementedException();
        }

        public ISubmitBuilder UseDeliveryReceiptOption(DeliveryReceiptOption option)
        {
            var registeredDelivery = (byte)option;
            return this;
        }

        public ISubmitBuilder UseReplacement(bool replaceIfPresent)
        {
            bool replaceIfPresentFlag = replaceIfPresent;
            return this;
        }

        public ISubmitBuilder UseShortMessage(string message)
        {
            _pdu.ShortMessage = DataCoding.Default.Encode(message);
            return this;
        }

        public ISubmitBuilder UseShortMessage(Stream stream)
        {
            byte[] buffer;
            if (stream.CanSeek)
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
            }
            else
            {
                buffer = new byte[0];
            }
            _pdu.ShortMessage = buffer;
            return this;
        }

        internal Submit Build()
        {
            return _pdu;
        }
    }
}

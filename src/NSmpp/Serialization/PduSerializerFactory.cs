using System;
using System.Collections.Generic;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal static class PduSerializerFactory
    {
        private static readonly IDictionary<SmppCommand, IPduSerializer> Serializers = new Dictionary<SmppCommand, IPduSerializer>()
        {
            [SmppCommand.BindReceiver] = new BindReceiverSerializer(),
            [SmppCommand.BindReceiverResp] = new BindReceiverResponseSerializer(),
            [SmppCommand.BindTransmitter] = new BindTransmitterSerializer(),
            [SmppCommand.BindTransmitterResp] = new BindTransmitterResponseSerializer(),
            [SmppCommand.BindTransceiver] = new BindTransceiverSerializer(),
            [SmppCommand.BindTransceiverResp] = new BindTransceiverResponseSerializer(),
            [SmppCommand.Unbind] = new UnbindSerializer(),
            [SmppCommand.UnbindResp] = new UnbindResponseSerializer(),
            [SmppCommand.Submit] = new SubmitSerializer(),
            [SmppCommand.SubmitResp] = new SubmitResponseSerializer(),
            [SmppCommand.Query] = new QuerySerializer(),
            [SmppCommand.QueryResp] = new QueryResponseSerializer(),
            [SmppCommand.Deliver] = new DeliverSerializer(),
            [SmppCommand.DeliverResp] = new DeliverResponseSerializer(),
            [SmppCommand.Cancel] = new CancelSerializer(),
            [SmppCommand.CancelResp] = new CancelResponseSerializer(),
            [SmppCommand.EnquireLink] = new EnquireLinkSerializer(),
            [SmppCommand.EnquireLinkResp] = new EnquireLinkResponseSerializer()
        };

        internal static IPduSerializer Create(SmppCommand command)
        {
            IPduSerializer result;
            if (!Serializers.TryGetValue(command, out result))
                throw new ArgumentException("Unsupported command type.");

            return result;
        }
    }
}

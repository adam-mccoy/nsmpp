using System;
using System.Collections.Generic;
using NSmpp.Pdu;

namespace NSmpp.Serialization
{
    internal static class PduSerializerFactory
    {
        private static readonly IDictionary<SmppCommand, IPduSerializer> Serializers = new Dictionary<SmppCommand, IPduSerializer>()
        {
            { SmppCommand.BindTransmitter, new BindTransmitterSerializer() },
            { SmppCommand.BindTransmitterResp, new BindTransmitterResponseSerializer() }
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

using System;
using System.Collections.Generic;
using NSmpp.Pdu;

namespace NSmpp.Helpers
{
    internal static class BindHelper
    {
        private static readonly Dictionary<BindType, Func<BindPduBase>> Factories = new Dictionary<BindType, Func<BindPduBase>>
        {
            { BindType.Receiver, () => new BindReceiver() },
            { BindType.Transmitter, () => new BindTransmitter() },
            { BindType.Transceiver, () => new BindTransceiver() }
        };

        internal static PduBase CreateBindPdu(
            uint sequenceNumber,
            BindType bindType,
            string systemId,
            string password,
            BindOptions options)
        {
            var pdu = Factories[bindType]();

            pdu.SequenceNumber = sequenceNumber;
            pdu.SystemId = systemId;
            pdu.Password = password;
            pdu.SystemType = options.SystemType;
            pdu.AddressTon = options.Ton;
            pdu.AddressNpi = options.Npi;
            pdu.AddressRange = options.AddressRange;

            return pdu;
        }
    }
}

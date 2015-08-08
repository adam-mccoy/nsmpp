﻿namespace NSmpp.Pdu
{
    internal class BindTransmitterResponse : PduBase
    {
        public string SystemId { get; private set; }

        public BindTransmitterResponse(
            int length,
            SmppCommand command,
            SmppStatus status,
            int sequenceNumber,
            string systemId)
            : base(length, command, status, sequenceNumber)
        {
            SystemId = systemId;
        }
    }
}
using System;

namespace NSmpp.Pdu
{
    internal class Unbind : PduBase
    {
        public Unbind(uint sequenceNumber)
            : base(SmppStatus.Ok, sequenceNumber)
        {
        }

        internal override SmppCommand Command
        {
            get { return SmppCommand.Unbind; }
        }

        internal override int Length
        {
            get { return HeaderLength; }
        }
    }
}

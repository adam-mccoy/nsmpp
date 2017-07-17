using System;
using System.Runtime.Serialization;

namespace NSmpp.Serialization
{
    [Serializable]
    internal class PduSerializationException : Exception
    {
        private const string DefaultMessage = "An error occurred during PDU serialization.";

        public PduSerializationException()
        {
        }

        public PduSerializationException(string message)
            : base(message)
        {
        }

        public PduSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PduSerializationException(Exception innerException)
            : base(DefaultMessage, innerException)
        {
        }

        protected PduSerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

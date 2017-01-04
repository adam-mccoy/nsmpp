namespace NSmpp
{
    /// <summary>
    /// Provides data for the DeliverReceived event.
    /// </summary>
    public class DeliverReceivedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the NSmpp.DeliverReceivedEventArgs class.
        /// </summary>
        /// <param name="source">The source address of the delivery.</param>
        /// <param name="destination">The destination address of the delivery.</param>
        /// <param name="shortMessage">The short message of the delivery.</param>
        public DeliverReceivedEventArgs(
            Address source,
            Address destination,
            string shortMessage)
        {
            Source = source;
            Destination = destination;
            ShortMessage = shortMessage;
        }

        /// <summary>
        /// Gets the address which originated the message.
        /// </summary>
        public Address Source { get; private set; }
        /// <summary>
        /// Gets the address of the message's destination.
        /// </summary>
        public Address Destination { get; private set; }
        /// <summary>
        /// Gets the message contained in the delivery.
        /// </summary>
        public string ShortMessage { get; private set; }
    }
}

namespace NSmpp
{
    /// <summary>
    /// The following is a list of allowable states for a short message.
    /// The message_state value is returned by the SMSC to the ESME as
    /// part of the query_sm_resp PDU.
    /// </summary>
    public enum MessageState
    {
        /// <summary>
        /// The message is in enroute state.
        /// </summary>
        Enroute = 1,
        /// <summary>
        /// The message is delivered to destination.
        /// </summary>
        Delivered = 2,
        /// <summary>
        /// Message validity period has expired.
        /// </summary>
        Expired = 3,
        /// <summary>
        /// Message has been deleted.
        /// </summary>
        Deleted = 4,
        /// <summary>
        /// Message is undeliverable.
        /// </summary>
        Undeliverable = 5,
        /// <summary>
        /// Message is in accepted state (i.e. has been manually read
        /// on behalf of the subscriber by customer service)
        /// </summary>
        Accepted = 6,
        /// <summary>
        /// Message is in invalid state.
        /// </summary>
        Unknown = 7,
        /// <summary>
        /// Message is in a rejected state.
        /// </summary>
        Rejected = 8
    }
}

namespace NSmpp
{
    public enum SessionState
    {
        Open,
        BoundTransmitter,
        BoundReceiver,
        BoundTransceiver,
        Closed
    }

    public static class SessionStateExtensions
    {
        public static bool IsBound(this SessionState state)
        {
            return state >= SessionState.BoundTransmitter && state <= SessionState.BoundTransceiver;
        }
    }
}

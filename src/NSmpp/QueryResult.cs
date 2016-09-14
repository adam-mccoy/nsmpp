namespace NSmpp
{
    public class QueryResult
    {
        internal QueryResult(MessageState state)
        {
            State = state;
        }

        public MessageState State { get; private set; }
    }
}

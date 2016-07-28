namespace NSmpp
{
    public class SubmitResult
    {
        public SubmitResult(string messageId)
        {
            MessageId = messageId;
        }

        public string MessageId { get; private set; }
    }
}

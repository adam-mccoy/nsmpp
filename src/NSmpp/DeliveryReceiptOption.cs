namespace NSmpp
{
    public enum DeliveryReceiptOption : byte
    {
        None             = 0x00,
        SuccessOrFailure = 0x01,
        FailureOnly      = 0x02
    }
}

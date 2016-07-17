namespace NSmpp
{
    public class BindResult
    {
        public BindResult(string systemId, byte interfaceVersion = 0x34)
        {
            SystemId = systemId;
            InterfaceVersion = interfaceVersion;
        }

        public string SystemId { get; }
        public byte InterfaceVersion { get; }
    }
}

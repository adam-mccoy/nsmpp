using NSmpp.Serialization;

namespace NSmpp.Pdu.Parameters;

internal class SystemIdParameter(string value) : StringPduParameter(value)
{
    public override void Encode(IPduWriter writer)
    {
        writer.WriteString(Value);
    }
}

internal abstract class StringPduParameter(string value) : PduParameter
{
    protected string Value { get; } = value;
}

internal abstract class PduParameter
{
    public abstract void Encode(IPduWriter writer);
}
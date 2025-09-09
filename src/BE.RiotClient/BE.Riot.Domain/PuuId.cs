namespace BE.Riot;

public readonly record struct PuuId(string Value) : IEquatable<PuuId>, IComparable<PuuId>
{
    public static PuuId Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new PuuId(value);
    }

    public static bool TryParse(string? value, out PuuId result)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        result = new PuuId(value);
        return true;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public int CompareTo(PuuId other) => string.Compare(Value, other.Value, StringComparison.Ordinal);

    public static implicit operator string(PuuId puuId) => puuId.Value;
    public static implicit operator PuuId(string value) => new(value);

    public override string ToString() => Value;
}
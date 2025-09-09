namespace BE.Riot;

public readonly record struct MatchId(string Value) : IEquatable<MatchId>, IComparable<MatchId>
{
    public static MatchId Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return new MatchId(value);
    }

    public static bool TryParse(string? value, out MatchId result)
    {
        if (string.IsNullOrEmpty(value))
        {
            result = default;
            return false;
        }

        result = new MatchId(value);
        return true;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public int CompareTo(MatchId other) => string.Compare(Value, other.Value, StringComparison.Ordinal);

    public static implicit operator string(MatchId puuId) => puuId.Value;
    public static implicit operator MatchId(string value) => new(value);

    public override string ToString() => Value;
}
namespace BE.Riot.Tests;

public class PuuIdTests
{
    private const string ValidPuuIdValue = "test-puuid-123";

    [Fact]
    public void Constructor_WithValidValue_CreatesInstance()
    {
        var puuId = new PuuId(ValidPuuIdValue);

        Assert.Equal(ValidPuuIdValue, puuId.Value);
    }

    [Fact]
    public void Parse_WithValidString_ReturnsPuuId()
    {
        var puuId = PuuId.Parse(ValidPuuIdValue);

        Assert.Equal(ValidPuuIdValue, puuId.Value);
    }

    [Fact]
    public void Parse_WithNull_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => PuuId.Parse(null!));
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData(ValidPuuIdValue, true)]
    public void TryParse_WithVariousInputs_ReturnsExpectedResults(string? input, bool expectedResult)
    {
        bool success = PuuId.TryParse(input, out var result);

        Assert.Equal(expectedResult, success);
        if (success)
        {
            Assert.Equal(input, result.Value);
        }
    }

    [Fact]
    public void ImplicitConversion_FromString_CreatesPuuId()
    {
        PuuId puuId = ValidPuuIdValue;

        Assert.Equal(ValidPuuIdValue, puuId.Value);
    }

    [Fact]
    public void ImplicitConversion_ToPuuIdToString_ReturnsOriginalValue()
    {
        PuuId puuId = new (ValidPuuIdValue);

        string result = puuId;

        Assert.Equal(ValidPuuIdValue, result);
    }

    [Fact]
    public void CompareTo_WithEqualValues_ReturnsZero()
    {
        var puuId1 = new PuuId(ValidPuuIdValue);
        var puuId2 = new PuuId(ValidPuuIdValue);

        var result = puuId1.CompareTo(puuId2);

        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareTo_WithDifferentValues_ReturnsNonZero()
    {
        var puuId1 = new PuuId("aaa");
        var puuId2 = new PuuId("bbb");

        var result = puuId1.CompareTo(puuId2);

        Assert.True(result < 0);
    }

    [Fact]
    public void Equals_WithSameValue_ReturnsTrue()
    {
        var puuId1 = new PuuId(ValidPuuIdValue);
        var puuId2 = new PuuId(ValidPuuIdValue);

        Assert.Equal(puuId1, puuId2);
    }

    [Fact]
    public void GetHashCode_WithSameValue_ReturnsSameHash()
    {
        var puuId1 = new PuuId(ValidPuuIdValue);
        var puuId2 = new PuuId(ValidPuuIdValue);

        Assert.Equal(puuId1.GetHashCode(), puuId2.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        var puuId = new PuuId(ValidPuuIdValue);
        var result = puuId.ToString();

        Assert.Equal(ValidPuuIdValue, result);
    }
}
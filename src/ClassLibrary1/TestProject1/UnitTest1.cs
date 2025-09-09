namespace TestProject1;

public class UnitTest1
{
    [Fact]
    public void AddsTwoNumbers()
    {
        var result = ClassLibrary1.Class1.Add(1, 2);

        Assert.Equal(3, result);
    }

    [Fact]
    public void AddNegativeNumers()
    {
        var result = ClassLibrary1.Class1.Add(-1, -2);

        Assert.Equal(-3, result);
    }
}
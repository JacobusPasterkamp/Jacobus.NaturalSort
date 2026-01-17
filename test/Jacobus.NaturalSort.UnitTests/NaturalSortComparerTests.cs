namespace Jacobus.NaturalSort.UnitTests;

public class NaturalSortComparerTests
{
    [Theory]
    [InlineData("", "a")]
    [InlineData("1", "a")]
    [InlineData("a", "b")]
    [InlineData("4", "5")]
    [InlineData("4", "11")]
    [InlineData("7a", "7b")]
    [InlineData("aa7aa", "aa8aa")]
    [InlineData("zz7", "zz8")]
    [InlineData("7zz", "8zz")]
    [InlineData("2zz", "11z")]
    [InlineData("11z", "11zy")]
    [InlineData("a1b23c69", "a1b23c70")]
    [InlineData("A", "b")]
    [InlineData("a", "B")]
    [InlineData("0", "1")]
    [InlineData("10", "011")]
    [InlineData("010", "10")]
    public void LessThan(string a, string b)
    {
        NaturalSortComparer comparer = new();

        int result = comparer.Compare(a, b);

        Assert.True(result < 0);
    }

    [Theory]
    [InlineData("a", "")]
    [InlineData("a", "1")]
    [InlineData("b", "a")]
    [InlineData("5", "4")]
    [InlineData("11", "4")]
    [InlineData("7b", "7a")]
    [InlineData("aa8aa", "aa7aa")]
    [InlineData("zz8", "zz7")]
    [InlineData("8zz", "7zz")]
    [InlineData("11z", "2zz")]
    [InlineData("11zy", "11z")]
    [InlineData("a1b23c70", "a1b23c69")]
    [InlineData("b", "A")]
    [InlineData("B", "a")]
    [InlineData("1", "0")]
    [InlineData("011", "10")]
    [InlineData("10", "010")]
    public void GreaterThan(string a, string b)
    {
        NaturalSortComparer comparer = new();

        int result = comparer.Compare(a, b);

        Assert.True(result > 0);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("a", "a")]
    [InlineData("1", "1")]
    [InlineData("12", "12")]
    [InlineData("a12a", "a12a")]
    [InlineData("12a", "12a")]
    [InlineData("a12", "a12")]
    public void Equal(string a, string b)
    {
        NaturalSortComparer comparer = new();

        int result = comparer.Compare(a, b);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GreaterThan_LargeString()
    {
        string a = new string('a', 1000) + "11";
        string b = new string('a', 1000) + "2";
        NaturalSortComparer comparer = new();

        int result = comparer.Compare(a, b);

        Assert.True(result > 0);
    }

    [Fact]
    public void LesserThan_LargeString()
    {
        string a = new string('a', 1000) + "2";
        string b = new string('a', 1000) + "11";
        NaturalSortComparer comparer = new();

        int result = comparer.Compare(a, b);

        Assert.True(result < 0);
    }

    [Fact]
    public void GreaterThan_Equal()
    {
        string a = new string('a', 1000) + "11";
        string b = new string('a', 1000) + "11";
        NaturalSortComparer comparer = new();

        int result = comparer.Compare(a, b);

        Assert.Equal(0, result);
    }
}
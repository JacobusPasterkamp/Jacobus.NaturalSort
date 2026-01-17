namespace Jacobus.NaturalSort.UnitTests;

public class SortingStringsTests
{
    private readonly string[] _originalCollection =
    [
        "file20.txt",
        "file3.txt",
        "file100.txt",
        "file1.txt",
        "file10.txt",
        "file2.txt"
    ];

    private readonly string[] _expectedOutput =
    [
        "file1.txt",
        "file2.txt",
        "file3.txt",
        "file10.txt",
        "file20.txt",
        "file100.txt"
    ];

    private readonly NaturalStringComparer _comparer = new();

    [Fact]
    public void SortLists()
    {
        List<string> input = _originalCollection.ToList();
        List<string> expectedOutput = _expectedOutput.ToList();

        input.Sort(_comparer);

        Assert.Equal(expectedOutput, input);
    }

    [Fact]
    public void OrderByLists()
    {
        List<string> input = _originalCollection.ToList();
        List<string> expectedOutput = _expectedOutput.ToList();

        List<string> output = input.OrderBy(f => f, _comparer).ToList();

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void OrderByDescendingLists()
    {
        List<string> input = _originalCollection.ToList();
        List<string> expectedOutput = _expectedOutput.Reverse().ToList();

        List<string> output = input.OrderByDescending(f => f, _comparer).ToList();

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void OrderLists()
    {
        List<string> input = _originalCollection.ToList();
        List<string> expectedOutput = _expectedOutput.ToList();

        List<string> output = input.Order(_comparer).ToList();

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void OrderDescendingLists()
    {
        List<string> input = _originalCollection.ToList();
        List<string> expectedOutput = _expectedOutput.Reverse().ToList();

        List<string> output = input.OrderDescending(_comparer).ToList();

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void SortArrays()
    {
        string[] input = _originalCollection;
        string[] expectedOutput = _expectedOutput;

        input.Sort(_comparer);

        Assert.Equal(expectedOutput, input);
    }

    [Fact]
    public void OrderByArrays()
    {
        string[] input = _originalCollection;
        string[] expectedOutput = _expectedOutput;

        string[] output = input.OrderBy(f => f, _comparer).ToArray();

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void OrderByDescendingArrays()
    {
        string[] input = _originalCollection;
        string[] expectedOutput = _expectedOutput.Reverse().ToArray();

        string[] output = input.OrderByDescending(f => f, _comparer).ToArray();

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void OrderArrays()
    {
        string[] input = _originalCollection;
        string[] expectedOutput = _expectedOutput;

        string[] output = input.Order(_comparer).ToArray();

        Assert.Equal(expectedOutput, output);
    }

    [Fact]
    public void OrderDescendingArrays()
    {
        string[] input = _originalCollection;
        string[] expectedOutput = _expectedOutput.Reverse().ToArray();

        string[] output = input.OrderDescending(_comparer).ToArray();

        Assert.Equal(expectedOutput, output);
    }
}
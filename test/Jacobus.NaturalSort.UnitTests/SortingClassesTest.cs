namespace Jacobus.NaturalSort.UnitTests;

public class SortingTests
{
    record File2(string Name);

    private class File(string name)
    {
        public string Name { get; set; } = name;
    }

    private readonly File[] _originalCollection =
    [
        new("file20.txt"),
        new("file3.txt"),
        new("file100.txt"),
        new("file1.txt"),
        new("file10.txt"),
        new("file2.txt"),
    ];

    private readonly File[] _expectedOutput =
    [
        new("file1.txt"),
        new("file2.txt"),
        new("file3.txt"),
        new("file10.txt"),
        new("file20.txt"),
        new("file100.txt"),
    ];

    private readonly NaturalStringComparer _comparer = new();

    [Fact]
    public void SortLists()
    {
        List<File> input = _originalCollection.ToList();
        List<File> expectedOutput = _expectedOutput.ToList();

        input.Sort((a, b) => _comparer.Compare(a.Name, b.Name));

        Assert.SequenceEqual(expectedOutput, input, file => file.Name);
    }

    [Fact]
    public void OrderByLists()
    {
        List<File> input = _originalCollection.ToList();
        List<File> expectedOutput = _expectedOutput.ToList();

        List<File> output = input.OrderBy(f => f.Name, _comparer).ToList();

        Assert.SequenceEqual(expectedOutput, output, file => file.Name);
    }

    [Fact]
    public void OrderByDescendingLists()
    {
        List<File> input = _originalCollection.ToList();
        List<File> expectedOutput = _expectedOutput.Reverse().ToList();

        List<File> output = input.OrderByDescending(f => f.Name, _comparer).ToList();

        Assert.SequenceEqual(expectedOutput, output, file => file.Name);
    }

    [Fact]
    public void SortArrays()
    {
        File[] input = _originalCollection;
        File[] expectedOutput = _expectedOutput;

        input.Sort((a, b) => _comparer.Compare(a.Name, b.Name));

        Assert.SequenceEqual(expectedOutput, input, file => file.Name);
    }

    [Fact]
    public void OrderArrays()
    {
        File[] input = _originalCollection;
        File[] expectedOutput = _expectedOutput;

        File[] output = input.OrderBy(f => f.Name, _comparer).ToArray();

        Assert.SequenceEqual(expectedOutput, output, file => file.Name);
    }

    [Fact]
    public void OrderDescendingArrays()
    {
        File[] input = _originalCollection;
        File[] expectedOutput = _expectedOutput.Reverse().ToArray();

        File[] output = input.OrderByDescending(f => f.Name, _comparer).ToArray();

        Assert.SequenceEqual(expectedOutput, output, file => file.Name);
    }
}
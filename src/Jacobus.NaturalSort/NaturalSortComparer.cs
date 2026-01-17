using System.Globalization;

namespace Jacobus.NaturalSort;

/// <summary>
/// Compares strings naturally/alphanumerically.
/// See <see href="https://en.wikipedia.org/wiki/Natural_sort_order"/> for more details.
/// </summary>
/// <remarks>
/// Does not support decimal fractions, only full numbers are compared.<br/>
/// Does not support negative numbers, so '-' and '+' will be compared just like any other non-digit character.<br/>
/// Does not support comparing scientific number representations like 1e3.
/// </remarks>
/// <example>
/// Compare("a", "a") == 0
/// Compare("a", "b") == -1
/// Compare("b", "a") == 1
/// Compare("a", "1") == 1
/// Compare("11", "4") == 1
/// Compare("11z", "2zzz") == 1
/// Compare("11zy", "11z") == 1
/// Compare("aa8aa", "aa7aa") == 1
/// Compare("b", "A") == 1
/// Compare("011", "10") == 1
/// Compare("10", "010") == 1 (this one was an arbitrary choice, because Windows also orders its folders that way).
/// Compare("21.49", "21.5") = -1 (no support for decimal fractions on purpose, because that would cause other issues with for example comparing semantic version numbers https://semver.org/)
/// </example>
/// <returns>
/// <b>Exactly zero</b> if equal.<br/>
/// <b>Greater than zero</b> if x is greater than y.<br/>
/// <b>Less than zero</b> if x is less than y.
/// </returns>
public sealed class NaturalSortComparer : IComparer<string>
{
    private readonly CultureInfo _culture;

    /// <summary>
    /// Creates a natural sort comparer using <see cref="CultureInfo.InvariantCulture"/> to compare single characters.
    /// </summary>
    public NaturalSortComparer() : this(CultureInfo.InvariantCulture) { }

    /// <summary>
    /// Creates a natural sort comparer using the specified culture.
    /// </summary>
    /// <param name="culture">The culture to use for comparing single characters.</param>
    public NaturalSortComparer(CultureInfo culture)
    {
        _culture = culture ?? throw new ArgumentNullException(nameof(culture));
    }

    public int Compare(string? x, string? y)
    {
        if (x is null) return y is null ? 0 : -1;
        else if (y is null) return 1;

        // Loop while both strings still contain more characters...
        for (int index = 0; index < x.Length && index < y.Length; index++)
        {
            bool xIsNumber = char.IsDigit(x[index]);
            bool yIsNumber = char.IsDigit(y[index]);

            if (xIsNumber && yIsNumber)
            {
                int numberStartIndex = index;
                int xNumberEndIndex = index;
                int yNumberEndIndex = index;

                bool findingXNumberEnd;
                bool findingYNumberEnd;

                // Looping to find the end index of the number in both x and y.
                do
                {
                    index++;

                    findingXNumberEnd = index < x.Length && char.IsDigit(x[index]);
                    if (findingXNumberEnd) xNumberEndIndex = index;

                    findingYNumberEnd = index < y.Length && char.IsDigit(y[index]);
                    if (findingYNumberEnd) yNumberEndIndex = index;
                }
                while (findingXNumberEnd || findingYNumberEnd);

                // Creating spans that start and end exactly where the numbers are.
                int xNumberLength = xNumberEndIndex - numberStartIndex + 1;
                ReadOnlySpan<char> xNumber = x.AsSpan(numberStartIndex, xNumberLength);
                int yNumberLength = yNumberEndIndex - numberStartIndex + 1;
                ReadOnlySpan<char> yNumber = y.AsSpan(numberStartIndex, yNumberLength);

                int numberComparison = CompareNumbers(xNumber, yNumber);
                bool numbersAreDifferent = numberComparison != 0;
                if (numbersAreDifferent) return numberComparison;

                // If we get here, the numbers were equal and we need to continue looking for differences.

                // Both xNumberEndIndex and yNumberEndIndex are equal here, so we can use any one of them.
                index = xNumberEndIndex;
                continue;
            }
            else
            {
                // Compare characters normally.
                int charComparison = CompareCharacters(x[index], y[index]);
                bool charactersAreDifferent = charComparison != 0;
                if (charactersAreDifferent) return charComparison;
            }
        }

        // If we get here, either:
        // - both strings were equal. For example: Compare("a1a", "a1a") == 0.
        // - or they were equal up until one string ended. For example: Compare("a1a", "a1aa") == -1
        return x.Length - y.Length;
    }

    private int CompareCharacters(char x, char y)
    {
        if (x == y) return 0;

        // ToUpperInvariant would be slightly faster (2ns vs 6ns according to benchmarks),
        // but not worth sacrificing the flexibility of supplying your preferred culture.
        char upperX = char.ToUpper(x, _culture);
        char upperY = char.ToUpper(y, _culture);

        return upperX.CompareTo(upperY);
    }

    /// <summary>
    /// Compares two texts numerically.
    /// </summary>
    /// <remarks>
    /// Ignores leading '0' characters, so CompareNumbers("013", "12") => 1
    /// </remarks>
    /// <example>
    /// CompareNumbers("12", "13") => -1
    /// CompareNumbers("13", "12") => 1
    /// CompareNumbers("12", "12") => 0
    /// CompareNumbers("012", "12") => -1
    /// </example>
    /// <param name="x">A span of characters containing only digits (0-9).</param>
    /// <param name="y">A span of characters containing only digits (0-9).</param>
    /// <returns>
    /// 0 if equal.<br/>
    /// 1 if <paramref name="x"/> is greater than <paramref name="y"/>.<br/>
    /// -1 if <paramref name="y"/> is greater than <paramref name="x"/>.
    /// </returns>
    private static int CompareNumbers(ReadOnlySpan<char> x, ReadOnlySpan<char> y)
    {
        // Trim leading '0' of x and y number, for example: "00123" becomes "123".
        ReadOnlySpan<char> trimmedX = x.ExcludeLeadingZeroes();
        ReadOnlySpan<char> trimmedY = y.ExcludeLeadingZeroes();

        // Return early if a number only contains zeroes: CompareNumbers("1", "0000") == 1.
        bool xContainsOnlyZeroes = trimmedX.Length is 0;
        bool yContainsOnlyZeroes = trimmedY.Length is 0;
        if (xContainsOnlyZeroes) return yContainsOnlyZeroes ? 0 : -1;
        else if (yContainsOnlyZeroes) return 1;

        // After leading zeroes are excluded, a longer number is always greater: CompareNumbers("1000", "999") == 1.
        if (trimmedX.Length > trimmedY.Length) return 1;
        else if (trimmedX.Length < trimmedY.Length) return -1;

        // If we get here, we know there are no leading zeroes and the numbers have the same length.
        // Now it's just a matter of comparing each character normally.
        for (int index = 0; index < trimmedX.Length; index++)
        {
            char xDigit = trimmedX[index];
            char yDigit = trimmedY[index];

            int comparison = xDigit.CompareTo(yDigit);
            if (comparison != 0) return comparison;
        }

        // TODO: Should "010" < "10" like Windows sorts its folders and files?
        // Or is "010" > "10" more logical? I'm clueless here.

        // Comparing original length to make sure "010" < "10".
        return y.Length - x.Length;
    }
}
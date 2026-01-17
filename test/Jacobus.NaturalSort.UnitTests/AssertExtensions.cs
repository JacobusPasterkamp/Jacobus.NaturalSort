namespace Jacobus.NaturalSort.UnitTests;

internal static class AssertExtensions
{
    extension(Assert)
    {
        internal static void SequenceEqual<T, TComparisonType>(IEnumerable<T> expected, IEnumerable<T> actual, Func<T, TComparisonType> comparisonValueSelector)
        {
            IEnumerable<TComparisonType> expectedValues = expected.Select(comparisonValueSelector);
            IEnumerable<TComparisonType> actualValues = actual.Select(comparisonValueSelector);

            Assert.Equal(expectedValues, actualValues);
        }
    }
}
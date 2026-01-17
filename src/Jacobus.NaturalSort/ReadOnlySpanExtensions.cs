namespace Jacobus.NaturalSort;

internal static class ReadOnlySpanExtensions
{
    extension(ReadOnlySpan<char> source)
    {
        /// <summary>
        /// Shortens a span to exclude leading '0' characters.
        /// </summary>
        /// <example>
        /// ExcludeLeadingZeroes("00100200") => "100200"
        /// </example>
        /// <returns>A span without leading zeroes.<</returns>
        internal ReadOnlySpan<char> ExcludeLeadingZeroes()
        {
            int startIndex = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] is '0') startIndex++;
                else break;
            }

            // We can just return the same span if it had no leading zeroes.
            if (startIndex is 0) return source;

            // The range operator is only supported on .NET version 5 or greater,
            // in other versions .Slice() must be used.
#if NET5_0_OR_GREATER
            return source[startIndex..];
#else
            return source.Slice(startIndex);
#endif
        }
    }
}
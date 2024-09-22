namespace Burrows_Wheeler_Transform;


/// <summary>
/// Compares two strings for ordering, giving priority to a specified sentinel character.
/// </summary>
/// <remarks>
/// This class extends <see cref="StringComparer"/> to provide a custom comparison mechanism 
/// for strings used in the Burrows-Wheeler Transform. <br/>
/// The comparison treats a specified 
/// sentinel character with the highest priority, ensuring it is always considered first 
/// in comparisons.
/// </remarks>
internal class BurrowsWheelerComparer : StringComparer
{
    private readonly char _sentinelChar;

    /// <summary>
    /// Initializes a new instance of the <see cref="BurrowsWheelerComparer"/> class with a specified sentinel character.
    /// </summary>
    /// <param name="sentinel">The character to be given the highest comparison priority.</param>
    public BurrowsWheelerComparer(char sentinel)
    {
        _sentinelChar = sentinel;
    }

    public override int Compare(string? first, string? second)
    {
        if (first == null && second == null)
        {
            return 0;
        }
        if (first == null)
        {
            return -1;
        }
        if (second == null)
        {
            return 1;
        }

        int minLength = Math.Min(first.Length, second.Length);
        for (int i = 0; i < minLength; i++)
        {
            char xChar = first[i];
            char yChar = second[i];

            // Give the sentinel character the highest priority
            if (xChar == _sentinelChar && yChar != _sentinelChar) return -1;
            if (yChar == _sentinelChar && xChar != _sentinelChar) return 1;

            // Normal lexicographic comparison
            int result = xChar.CompareTo(yChar);
            if (result != 0)
            {
                return result;
            }
        }

        return first.Length.CompareTo(second.Length);
    }

    public override bool Equals(string? x, string? y)
    {
        return string.Equals(x, y);
    }

    public override int GetHashCode(string obj)
    {
        return obj?.GetHashCode() ?? 0;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burrows_Wheeler_Transform;

/// <summary>
/// Contains helper functions for the Burrows-Wheeler Transform and its inverse.
/// </summary>
/// <remarks>
/// This class provides static methods to facilitate various operations required for
/// the Burrows-Wheeler Transform algorithm and its inverse.
/// </remarks>
internal static class BWTUtils
{
    // Transform Helper functions

    /// <summary>
    /// Generates a list of all possible rotations of the input string by shifting its characters to the right.<br/>
    /// Characters that overflow beyond the end of the string wrap around to the beginning.
    /// </summary>
    /// <param name="input">The input string to generate rotations from. Each rotation shifts the string by one character to the right.</param>
    /// <returns>
    /// A list of strings where each string is a rightward shift of the input string. <br/>
    /// The list contains all possible rotations of the input string, including the original string.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when the number of generated rotations does not match the length of the input string, indicating an error in the rotation production.
    /// </exception>
    /// <remarks>
    /// For the input string "abc", the generated rotations would be:
    /// <code>
    /// "abc"
    /// "cab"
    /// "bca"
    /// </code>
    /// </remarks>
    internal static List<string> GenerateRotations(string input)
    {
        List<string> rotations = [input];
        for (int i = 1; i < input.Length; i++)
        {
            string newRotation = ShiftRight(rotations.Last());
            rotations.Add(newRotation);
        }
        if (rotations.Count != input.Length)
        {
            throw new InvalidOperationException("Error: Invalid Rotation Production");
        }
        return rotations;
    }

    /// <summary>
    /// Shifts the characters of the input string to the right by one position.<br/>
    /// Characters that overflow beyond the end of the string wrap around to the beginning.
    /// </summary>
    /// <param name="input">The input string to be shifted. It must not be null or empty.</param>
    /// <returns>
    /// A new string with all characters shifted to the right.<br/> 
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the input string is null or empty.
    /// </exception>
    /// <remarks>
    /// For example, shifting the string "abc" results in "cab", where 'c' is moved to the front.
    /// </remarks>
    internal static string ShiftRight(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Input string cannot be null or empty.", nameof(input));
        }

        char lastChar = input.Last();
        string restOfString = input.Substring(0, input.Length - 1);

        return lastChar + restOfString;
    }

    /// <summary>
    /// Generates a transformation string by concatenating the last character of each string in the input list.
    /// </summary>
    /// <param name="input">A list of strings from which the last character of each string will be extracted. The list must not be null.</param>
    /// <returns>
    /// A string composed of the last characters of each string in the input list. If the input list is empty, an empty string is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input list is null.
    /// </exception>
    /// <remarks>
    /// For example, given the input list ["abc", "def", "ghi"], the result would be "cfi", 
    /// </remarks>
    internal static string GetTransform(List<string> input)
    {
        ArgumentNullException.ThrowIfNull(nameof(input));
        if (input.Count == 0)
        {
            return string.Empty;
        }
        StringBuilder sb = new StringBuilder();
        foreach (string str in input)
        {
            sb.Append(str.Last());
        }
        return sb.ToString();
    }

    // InverseTransform Helper functions

    /// <summary>
    /// Extracts the original string from a list of lines, identified by a sentinel character at the end of the line.
    /// </summary>
    /// <param name="lines">A list of strings (lines) to search for the original string. The list must not be null.</param>
    /// <param name="sentinel">The character that indicates the end of the original string.</param>
    /// <returns>
    /// The original string, excluding the sentinel character.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no line in the input list ends with the sentinel character, indicating that the extraction failed.
    /// </exception>
    /// <remarks>
    /// For example, if the input lines are ["a$c", "def$", "$hi"], and the sentinel is '$', 
    /// the method returns "def" as the original string.
    /// </remarks>
    internal static string ExtractOriginalString(List<string> lines, char sentinel)
    {
        string? target = null;

        foreach (string line in lines)
        {
            if (line.Last() == sentinel)
            {
                target = line;
                break;
            }
        }
        if (target is null)
        {
            throw new InvalidOperationException("The extraction of the original string failed; no row ended with the sentinel character.");
        }
        return target[..^1];
    }

    /// <summary>
    /// Converts a two-dimensional array (grid) of characters into a list of strings, 
    /// where each string represents a row of the grid.
    /// </summary>
    /// <param name="grid">A two-dimensional array of characters representing the grid. The array must not be null or empty.</param>
    /// <returns>
    /// A list of strings where each string corresponds to a row in the input grid.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input grid is null.
    /// </exception>
    /// <remarks>
    /// For example, given the input grid:
    /// <code>
    /// char[][] grid = new char[][] {
    ///     new char[] { 'a', 'b', 'c' },
    ///     new char[] { 'd', 'e', 'f' }
    /// };
    /// </code>
    /// The method returns ["abc", "def"].
    /// </remarks>
    internal static List<string> ConvertGridToLines(char[][] grid)
    {
        List<string> lines = [];
        foreach (char[] line in grid)
        {
            lines.Add(new string(line));
        }
        return lines;
    }

    /// <summary>
    /// Performs a series of shifts and sorts on a two-dimensional array (grid) of characters based on the specified encoded input.
    /// </summary>
    /// <param name="grid">A two-dimensional array of characters to be modified. The array must not be null.</param>
    /// <param name="encodedInput">A string that determines the number of shifts to perform on the grid. Must contain at least three characters.</param>
    /// <param name="comparer">An instance of <see cref="StringComparer"/> used for sorting the rows of the grid.</param>
    /// <returns>
    /// A modified two-dimensional array of characters after performing the specified shifts and sorts.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input grid or encoded input is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the encoded input does not contain at least three characters.
    /// </exception>
    /// <remarks>
    /// The method performs shifts based on the length of the encoded input. <br/>
    /// After each shift, the rows of the grid are sorted using the provided comparer.
    /// </remarks>
    internal static char[][] PerformGridShiftsAndSorts(char[][] grid, string encodedInput, StringComparer comparer)
    {
        int maxNeededShifts = encodedInput.Length - 2;
        for (int i = 0; i < maxNeededShifts; i++)
        {
            grid = PerformStep(grid, encodedInput, comparer);
        }
        return grid;
    }

    /// <summary>
    /// Performs the initial setup of a two-dimensional array (grid) of characters 
    /// by populating its columns based on the provided encoded input and a specified comparer.
    /// </summary>
    /// <param name="grid">A two-dimensional array of characters to be modified. The array must not be null.</param>
    /// <param name="encodedInput">A string used to populate the last column of the grid. It must not be null or empty.</param>
    /// <param name="comparer">An instance of <see cref="StringComparer"/> used for sorting characters for the first column.</param>
    /// <returns>
    /// A modified two-dimensional array of characters after populating the columns and performing a shift and sort operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input grid or encoded input is null.
    /// </exception>
    /// <remarks>
    /// The method populates the last column of the grid using the encoded input, 
    /// sorts the characters of the encoded input for the first column, 
    /// and then performs a shift and sort operation on the grid.
    /// </remarks>
    internal static char[][] InitialSetup(char[][] grid, string encodedInput, StringComparer comparer)
    {
        PopulateLastColumn(grid, encodedInput);
        PopulateFirstColumn(grid, SortCharacters(encodedInput, comparer));
        return ShiftSortStep(grid, comparer);
    }

    /// <summary>
    /// Performs a single step in the transformation of a two-dimensional array (grid) of characters <br/>
    /// by populating the last column with characters from the original string and then applying <br/>
    /// a shift and sort operation on the grid.
    /// </summary>
    /// <param name="grid">A two-dimensional array of characters to be modified. The array must not be null.</param>
    /// <param name="original">A string used to populate the last column of the grid. It must not be null or empty.</param>
    /// <param name="comparer">An instance of <see cref="StringComparer"/> used for sorting the rows of the grid.</param>
    /// <returns>
    /// A modified two-dimensional array of characters after populating the last column and performing a shift and sort operation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input grid or original string is null.
    /// </exception>
    /// <remarks>
    /// The method first populates the last column of the grid using the original string, 
    /// and then it performs a shift and sort operation, returning the updated grid.
    /// </remarks>
    internal static char[][] PerformStep(char[][] grid, string original, StringComparer comparer)
    {
        PopulateLastColumn(grid, original);
        char[][] shiftsorted = ShiftSortStep(grid, comparer);
        return shiftsorted;
    }

    /// <summary>
    /// Performs a combined operation of shifting and sorting on a two-dimensional array (grid) of characters.
    /// </summary>
    /// <param name="grid">A two-dimensional array of characters to be modified. The array must not be null.</param>
    /// <param name="comparer">An instance of <see cref="StringComparer"/> used for sorting the rows of the grid.</param>
    /// <returns>
    /// A modified two-dimensional array of characters after performing a right shift followed by sorting the rows.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input grid is null.
    /// </exception>
    /// <remarks>
    /// The method first applies a right shift to the grid, then sorts the rows using the specified comparer. 
    /// The final result is returned as a new modified grid.
    /// </remarks>
    internal static char[][] ShiftSortStep(char[][] grid, StringComparer comparer)
    {
        char[][] shifted = ShiftRight(grid);
        char[][] sorted = SortRows(shifted, comparer);
        return sorted;
    }

    /// <summary>
    /// Generates a square two-dimensional array (grid) of characters with the specified length.
    /// </summary>
    /// <param name="length">The length of each side of the square grid. Must be a positive integer.</param>
    /// <returns>
    /// A two-dimensional array of characters where each dimension has the specified length.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified length is less than or equal to zero.
    /// </exception>
    /// <remarks>
    /// The method initializes a square grid where each row is an array of characters 
    /// with the specified length. <br/>
    /// All elements are initialized to their default value (null character).
    /// </remarks>
    internal static char[][] GenerateSquareGrid(int length)
    {
        char[][] grid = new char[length][];
        for (int i = 0; i < grid.Length; i++)
        {
            grid[i] = new char[length];
        }
        return grid;
    }

    /// <summary>
    /// Shifts the elements of a two-dimensional array (grid) of characters to the right by one position.<br/>
    /// The last element of each row wraps around to the first position.
    /// </summary>
    /// <param name="input">A two-dimensional array of characters to be shifted. The array must not be null and must have at least one row and one column.</param>
    /// <returns>
    /// A new two-dimensional array of characters with the elements shifted to the right.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input array is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the input array does not contain any rows or columns.
    /// </exception>
    /// <remarks>
    /// The method creates a new array where each row is shifted right, with the last element of the row moving to the first position.<br/>
    /// Any overflow from the rightmost element wraps around to the beginning of the row.
    /// </remarks>
    internal static char[][] ShiftRight(char[][] input)
    {
        int rowCount = input.Length;
        int colCount = input[0].Length;

        char[][] shifted = new char[rowCount][];
        for (int i = 0; i < rowCount; i++)
        {
            shifted[i] = new char[colCount];
            shifted[i][0] = input[i][colCount - 1];
            Array.Copy(input[i], 0, shifted[i], 1, colCount - 1);
        }

        return shifted;
    }

    /// <summary>
    /// Sorts the rows of a two-dimensional array (grid) of characters based on a specified string comparer.
    /// </summary>
    /// <param name="input">A two-dimensional array of characters whose rows are to be sorted. The array must not be null and must contain at least one row.</param>
    /// <param name="comparer">An instance of <see cref="StringComparer"/> used to determine the order of the rows.</param>
    /// <returns>
    /// A new two-dimensional array of characters with the rows sorted according to the specified comparer.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input array or the comparer is null.
    /// </exception>
    /// <remarks>
    /// The method first extracts the rows from the input array, sorts them using the provided comparer, <br/>
    /// and then converts the sorted rows back into a two-dimensional character array.
    /// </remarks>
    internal static char[][] SortRows(char[][] input, StringComparer comparer)
    {
        List<string> rows = ExtractRows(input);

        rows.Sort(comparer);

        return ConvertTo2DCharArray(rows, input[0].Length);
    }

    /// <summary>
    /// Extracts the rows from a two-dimensional array (grid) of characters and converts each row to a string.
    /// </summary>
    /// <param name="input">A two-dimensional array of characters from which to extract rows. The array must not be null and must contain at least one row.</param>
    /// <returns>
    /// A list of strings, each representing a row from the input array.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input array is null.
    /// </exception>
    /// <remarks>
    /// The method iterates through each row of the input array,<br/>
    /// converts each row to a string using the 
    /// <see cref="ConvertRowToString"/> method,<br/>
    /// and adds the resulting strings to a list, which is then returned.
    /// </remarks>
    internal static List<string> ExtractRows(char[][] input)
    {
        List<string> rows = new List<string>();

        for (int i = 0; i < input.Length; i++)
        {
            rows.Add(ConvertRowToString(input[i]));
        }

        return rows;
    }

    /// <summary>
    /// Converts a one-dimensional array of characters (row) into a string, stopping at the first null character.
    /// </summary>
    /// <param name="row">A one-dimensional array of characters to be converted to a string. The array must not be null.</param>
    /// <returns>
    /// A string representation of the characters in the row, up to (but not including) the first null character.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the input array is null.
    /// </exception>
    /// <remarks>
    /// The method iterates through the character array and appends each character to a 
    /// <see cref="StringBuilder"/> until a null character ('\0') is encountered. <br/>
    /// The resulting string is then returned.
    /// </remarks>
    internal static string ConvertRowToString(char[] row)
    {
        StringBuilder sb = new StringBuilder();
        foreach (char ch in row)
        {
            if (ch == '\0')
            {
                break;
            }
            sb.Append(ch);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Converts a list of strings into a two-dimensional array of characters with a specified row length.
    /// </summary>
    /// <param name="rows">A list of strings to be converted into a two-dimensional character array.
    /// The list must not be null and must contain at least one string.</param>
    /// <param name="rowLength">The length of each row in the resulting two-dimensional array. Must be a positive integer.</param>
    /// <returns>
    /// A two-dimensional array of characters where each row corresponds to a string from the list.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the list of rows is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified row length is less than or equal to zero.
    /// </exception>
    /// <remarks>
    /// The method initializes a new two-dimensional character array and fills each row with the characters <br/>
    /// from the corresponding string in the list using the <see cref="FillRow"/> method. <br/>
    /// Each row will have a length defined by the <paramref name="rowLength"/> parameter.<br/>
    /// </remarks>
    internal static char[][] ConvertTo2DCharArray(List<string> rows, int rowLength)
    {
        char[][] sorted = new char[rows.Count][];

        for (int i = 0; i < rows.Count; i++)
        {
            sorted[i] = new char[rowLength];
            FillRow(sorted[i], rows[i]);
        }

        return sorted;
    }

    /// <summary>
    /// Fills a target array of characters with the characters from a specified source string.
    /// </summary>
    /// <param name="targetRow">A one-dimensional array of characters to be filled. The array must not be null and must have a length equal to or greater than the length of the source string.</param>
    /// <param name="source">The source string from which to fill the target array. The string must not be null.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the target array or the source string is null.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown if the length of the source string exceeds the length of the target array.
    /// </exception>
    /// <remarks>
    /// The method iterates through the source string and copies each character to the corresponding 
    /// position in the target array.<br/>
    /// If the length of the source string exceeds that of the target 
    /// array, an exception will be thrown.
    /// </remarks>
    internal static void FillRow(char[] targetRow, string source)
    {
        for (int i = 0; i < source.Length; i++)
        {
            targetRow[i] = source[i];
        }
    }

    /// <summary>
    /// Populates the last column of a two-dimensional array of characters (grid) with characters from a specified input string.
    /// </summary>
    /// <param name="grid">A two-dimensional array of characters to be modified. The array must not be null and must contain at least one row.</param>
    /// <param name="input">A string containing characters to populate the last column of the grid. The length of the string must match the number of rows in the grid.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the grid or input string is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the last column of the grid is not empty (i.e., not filled with null characters) before populating.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown when the length of the input string does not match the number of rows in the grid.
    /// </exception>
    /// <remarks>
    /// The method first checks if the last column of the grid is empty (filled with null characters).<br/>
    /// If it is not, an exception is thrown. If the last column is empty, the method then fills the<br/>
    /// last column with the characters from the input string, ensuring that each character corresponds<br/>
    /// to the respective row in the grid.
    /// </remarks>
    internal static void PopulateLastColumn(char[][] grid, string input)
    {
        int rowCount = grid.Length;

        // Check if the last column is filled with null characters
        for (int i = 0; i < rowCount; i++)
        {
            if (grid[i][grid[i].Length - 1] != '\0')
            {
                throw new InvalidOperationException("The last column must be empty before populating.");
            }
        }

        // Populate the last column with characters from the input string
        for (int i = 0; i < rowCount; i++)
        {
            grid[i][grid[i].Length - 1] = input[i];
        }
    }

    /// <summary>
    /// Populates the first column of a two-dimensional array of characters (grid) with characters from a specified input string.
    /// </summary>
    /// <param name="grid">A two-dimensional array of characters to be modified. The array must not be null and must contain at least one row.</param>
    /// <param name="input">A string containing characters to populate the first column of the grid. The length of the string must match the number of rows in the grid.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the grid or input string is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the first column of the grid is not empty (i.e., not filled with null characters) before populating.
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown when the length of the input string does not match the number of rows in the grid.
    /// </exception>
    /// <remarks>
    /// The method first checks if the first column of the grid is empty (filled with null characters).<br/>
    /// If it is not, an exception is thrown. If the first column is empty, the method then fills the<br/>
    /// first column with the characters from the input string, ensuring that each character corresponds<br/>
    /// to the respective row in the grid.
    /// </remarks>
    internal static void PopulateFirstColumn(char[][] grid, string input)
    {
        int rowCount = grid.Length;

        // Check if the first column is filled with null characters
        for (int i = 0; i < rowCount; i++)
        {
            if (grid[i][0] != '\0')
            {
                throw new InvalidOperationException("The first column must be empty before populating.");
            }
        }

        // Populate the first column with characters from the input string
        for (int i = 0; i < rowCount; i++)
        {
            grid[i][0] = input[i];
        }
    }

    /// <summary>
    /// Sorts the characters of the specified input string using the provided string comparer.
    /// </summary>
    /// <param name="input">The input string whose characters are to be sorted. If null or empty, the method returns the input unchanged.</param>
    /// <param name="comparer">The <see cref="StringComparer"/> to determine the order of the characters.</param>
    /// <returns>
    /// A new string containing the characters of the input string sorted according to the specified comparer.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the comparer is null.
    /// </exception>
    /// <remarks>
    /// If the input string is null or empty, the method will return the input unchanged. <br/>
    /// The method converts the input string into a character array, sorts the characters <br/>
    /// using the specified comparer, and then concatenates them back into a single string.<br/>
    /// </remarks>
    internal static string SortCharacters(string input, StringComparer comparer)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        char[] charArray = input.ToCharArray();

        List<string> stringList = new List<string>();
        foreach (char c in charArray)
        {
            stringList.Add(c.ToString());
        }

        // Changed this from instance of the Comparer
        stringList.Sort(comparer);

        return string.Join(string.Empty, stringList);
    }
}

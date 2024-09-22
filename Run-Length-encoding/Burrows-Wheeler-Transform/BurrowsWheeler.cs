using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils = Burrows_Wheeler_Transform.BWTUtils;

namespace Burrows_Wheeler_Transform;

//TODO: Add documentation
/// <summary>
/// Provides methods to perform the Burrows-Wheeler Transform and its inverse.
/// </summary>
/// <remarks>
/// This class implements the Burrows-Wheeler Transform algorithm,<br/>
/// which rearranges a string into a more compressible form.<br/>
/// It also provides 
/// functionality to recover the original string from the transformed output.
/// </remarks>
public class BurrowsWheeler
{
    private readonly char _sentinelCharacter;
    private readonly BurrowsWheelerComparer _comparer;

    /// <summary>
    /// Initializes a new instance of the <see cref="BurrowsWheeler"/> class with a default sentinel character (bell character).
    /// </summary>
    public BurrowsWheeler() 
    {
        _sentinelCharacter = '\a';
        _comparer = new BurrowsWheelerComparer(_sentinelCharacter);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BurrowsWheeler"/> class with a specified sentinel character.
    /// </summary>
    /// <param name="sentinel">The character to be used as the sentinel in transformations.</param>
    public BurrowsWheeler(char sentinel)
    {
        _sentinelCharacter = sentinel;
        _comparer = new BurrowsWheelerComparer(_sentinelCharacter);
    }

    /// <summary>
    /// Transforms the specified input string using the Burrows-Wheeler Transform.
    /// </summary>
    /// <param name="input">The input string to transform.</param>
    /// <returns>The transformed string.</returns>
    public string Transform(string input)
    {
        List<string> rotations = Utils.GenerateRotations(input + _sentinelCharacter);

        rotations.Sort(_comparer);

        return Utils.GetTransform(rotations);
    }

    /// <summary>
    /// Recovers the original string from its Burrows-Wheeler Transform.
    /// </summary>
    /// <param name="encodedInput">The transformed string to decode.</param>
    /// <returns>The original string.</returns>
    public string Inverse(string encodedInput)
    {
        int length = encodedInput.Length;
        char[][] grid = Utils.GenerateSquareGrid(length);
        grid = Utils.InitialSetup(grid, encodedInput, _comparer);
        grid = Utils.PerformGridShiftsAndSorts(grid, encodedInput, _comparer);
        //Now the entire grid should be filled with values

        List<string> lines = Utils.ConvertGridToLines(grid);

        return Utils.ExtractOriginalString(lines, _sentinelCharacter);
    }
}

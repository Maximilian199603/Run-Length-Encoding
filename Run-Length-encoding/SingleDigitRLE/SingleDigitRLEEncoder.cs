using System.Text;
using Utils = RLE.SingleDigit.SIngleDigitRLEUtils;

namespace RLE.SingleDigit;

/// <summary>
/// Provides functionality for encoding a string using fixed-length Run-Length Encoding (RLE).<br/>
/// 
/// This class implements Single Digit RLE, where the count of occurrences for each character<br/>
/// is limited to 9. The encoded output can be returned either as a concatenated string or as<br/>
/// a list of string pairs representing counts and characters.<br/>
/// </summary>
public class SingleDigitRLEEncoder
{

    /// <summary>
    /// Encodes the input string using fixed-length Run-Length Encoding (RLE). <br/>
    /// </summary>
    /// <param name="input">The input string to be encoded.</param>
    /// <returns>
    /// The encoded string, where each character is preceded by its occurrence count (1-9).<br/>
    /// </returns>
    /// <remarks>
    /// Example: <br/>
    ///     Input: "aabbbccde" <br/>
    ///     Output: "2a3b2c1d1e" <br/>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is <c>null</c>.</exception>
    public static string Encode(string input)
    {
        var list = Utils.EncodeAlgorithm(input);
        return Utils.ConvertIntoStringVersion(list);
    }

    /// <summary>
    /// Encodes the input string using fixed-length Run-Length Encoding (RLE).
    /// </summary>
    /// <param name="input">The input string to be encoded.</param>
    /// <returns>
    /// A list of encoded strings, where each element is in the format "count character". <br/>
    /// </returns>
    /// <remarks>
    /// Example: <br/>
    ///     Input: "aabbbccde" <br/>
    ///     Output:  ["2a", "3b", "2c", "1d", "1e"] <br/>
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is <c>null</c>.</exception>
    public static List<string> EncodeAsList(string input)
    {
        return Utils.EncodeAlgorithm(input);
    }
}

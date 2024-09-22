using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utils = RLE.SingleDigit.SIngleDigitRLEUtils;

namespace RLE.SingleDigit;

/// <summary>
/// Provides functionality for decoding a string using fixed-length Run-Length Encoding (RLE).<br/>
/// This class implements Single Digit RLE, where the count of occurrences for each character<br/>
/// is limited to 9.
/// </summary>
public class SingleDigitRLEDecoder
{
    /// <summary>
    /// Decodes an encoded string into its original form.<br/>
    /// </summary>
    /// <param name="input">The encoded string to be decoded.</param>
    /// <returns>
    /// The decoded string.<br/>
    /// </returns>
    /// <remarks>
    /// Example: <br/>
    ///     Input: "2a3b2c1d1e"<br/>
    ///     Output: "aabbbccde"<br/>
    /// </remarks>
    public static string Decode(string input)
    {
        var packets = Utils.SplitIntoPackets(input);
        return Decode(packets);
    }

    /// <summary>
    /// Decodes a list of RLE encoded strings into a single string.
    /// </summary>
    /// <param name="input">A list of encoded strings.</param>
    /// <returns>
    /// The decoded string.<br/>
    /// </returns>
    /// <remarks>
    /// Example: <br/>
    ///     Input: "2a3b2c1d1e"<br/>
    ///     Output: "aabbbccde"<br/>
    /// </remarks>
    public static string Decode(List<string> input)
    {
        return Utils.DecodeAlgorithm(input);
    }
}

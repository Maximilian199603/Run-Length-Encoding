using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils = RLE.SingleByte.SingleByteRLEUtils;

namespace RLE.SingleByte;
/// <summary>
/// Provides functionality for encoding strings using single-byte Run-Length Encoding (RLE).
/// </summary>
public class SingleByteRLEEncoder
{
    /// <summary>
    /// Encodes the specified input string into a byte array using single-byte Run-Length Encoding.
    /// </summary>
    /// <param name="input">The input string to encode.</param>
    /// <returns>A byte array representing the encoded string.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the input string is null.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Example of encoding a string:
    /// </para>
    /// <code>
    /// string input = "AAABB";
    /// byte[] encodedBytes = SingleByteRLEEncoder.Encode(input);
    /// encodedBytes will be { 3, 65, 0, 2, 66, 0 }
    /// </code>
    /// </remarks>
    public static byte[] Encode(string input)
    {
        List<Packet> packets = Utils.GeneratePackets(input);
        return Utils.ConvertPacketsToBytes(packets).ToArray();
    }
}

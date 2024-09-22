using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils = RLE.SingleByte.SingleByteRLEUtils;

namespace RLE.SingleByte;
/// <summary>
/// Provides functionality for decoding byte arrays encoded with single-byte Run-Length Encoding (RLE).
/// </summary>
public class SingleByteRLEDecoder
{
    /// <summary>
    /// Decodes the specified byte list into a string using single-byte Run-Length Encoding.
    /// </summary>
    /// <param name="bytes">The list of bytes to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the byte list length is not a multiple of 3.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Example of decoding a byte list:
    /// </para>
    /// <code>
    /// List&lt;byte&gt; encodedBytes = new List&lt;byte&gt; { 3, 65, 0, 2, 66, 0 };
    /// string decodedString = SingleByteRLEDecoder.Decode(encodedBytes);
    /// decodedString will be "AAABB"
    /// </code>
    /// </remarks>
    public static string Decode(List<byte> bytes)
    {
        List<Packet> packets = Utils.ConvertBytesToPackets(bytes);
        return Utils.GenerateString(packets);
    }

    /// <summary>
    /// Decodes the specified byte array into a string using single-byte Run-Length Encoding.
    /// </summary>
    /// <param name="bytes">The byte array to decode.</param>
    /// <returns>The decoded string.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the byte array length is not a multiple of 3.
    /// </exception>
    /// <remarks>
    /// <para>
    /// Example of decoding a byte array:
    /// </para>
    /// <code>
    /// byte[] encodedBytes = new byte[] { 3, 65, 0, 2, 66, 0 };
    /// string decodedString = SingleByteRLEDecoder.Decode(encodedBytes);
    /// decodedString will be "AAABB"
    /// </code>
    /// </remarks>
    public static string Decode(byte[] bytes)
    {
        List<Packet> packets = Utils.ConvertBytesToPackets(bytes.ToList());
        return Utils.GenerateString(packets);
    }
}

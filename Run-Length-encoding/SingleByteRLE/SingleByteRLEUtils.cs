using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RLE.SingleByte;
/// <summary>
/// Provides utility methods for encoding and decoding strings using single-byte Run-Length Encoding (RLE).
/// </summary>
internal static class SingleByteRLEUtils
{
    /// <summary>
    /// Generates a list of <see cref="Packet"/> objects from the specified input string.
    /// </summary>
    /// <param name="input">The input string to encode.</param>
    /// <returns>A list of <see cref="Packet"/> objects representing the encoded string.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the input string is null.
    /// </exception>
    internal static List<Packet> GeneratePackets(string input)
    {
        ArgumentNullException.ThrowIfNull(nameof(input));
        if (string.Empty.Equals(input))
        {
            return [];
        }

        List<Packet> packets = [];
        byte amount = 1;
        char current = input.First();

        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] == current)
            {
                if (amount >= byte.MaxValue)
                {
                    packets.Add(new Packet(amount, current));
                    amount = 1;
                    continue;
                }
                amount++;
            }
            else
            {
                packets.Add(new Packet(amount, current));
                current = input[i];
                amount = 1;
            }
        }
        packets.Add(new Packet(amount, current));

        return packets;
    }

    /// <summary>
    /// Converts a list of <see cref="Packet"/> objects to a list of bytes.
    /// </summary>
    /// <param name="packets">The list of packets to convert.</param>
    /// <returns>A list of bytes representing the packets.</returns>
    internal static List<byte> ConvertPacketsToBytes(List<Packet> packets)
    {
        List<byte> byteList = [];

        foreach (var packet in packets)
        {
            // Use the ToBytes method of the Packet class
            byteList.AddRange(packet.ToBytes());
        }

        return byteList;
    }

    /// <summary>
    /// Converts a list of bytes back into a list of <see cref="Packet"/> objects.
    /// </summary>
    /// <param name="bytes">The list of bytes to convert.</param>
    /// <returns>A list of <see cref="Packet"/> objects.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the byte list length is not a multiple of 3.
    /// </exception>
    internal static List<Packet> ConvertBytesToPackets(List<byte> bytes)
    {
        List<Packet> packets = new List<Packet>();

        if (bytes.Count % 3 != 0)
        {
            throw new InvalidOperationException("Byte list length must be a multiple of 3.");
        }

        for (int i = 0; i < bytes.Count; i += 3)
        {
            byte amount = bytes[i];
            char character = BitConverter.ToChar(bytes.GetRange(i + 1, 2).ToArray(), 0);

            packets.Add(new Packet(amount, character));
        }

        return packets;
    }

    /// <summary>
    /// Generates a decoded string from a list of <see cref="Packet"/> objects.
    /// </summary>
    /// <param name="packets">The list of packets to decode.</param>
    /// <returns>A string representing the decoded content.</returns>
    internal static string GenerateString(List<Packet> packets)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var packet in packets)
        {
            sb.Append(packet.ToString());
        }
        return sb.ToString();
    }
}

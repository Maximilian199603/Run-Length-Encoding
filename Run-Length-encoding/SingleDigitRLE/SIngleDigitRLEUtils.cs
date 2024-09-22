using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLE.SingleDigit;
internal static class SIngleDigitRLEUtils
{
    /// <summary>
    /// Converts a list of encoded packets into a single concatenated <br/>
    /// string. This method takes the output of <see cref="EncodeAlgorithm"/> <br/>
    /// and converts it to a string.<br/>
    /// </summary>
    /// <param name="input">The list of encoded packets.</param>
    /// <returns>A concatenated string of all encoded packets.</returns>
    internal static string ConvertIntoStringVersion(List<string> input)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string item in input)
        {
            sb.Append(item);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Writes a packet consisting of the count of occurrences and the corresponding character to the output list.<br/>
    /// </summary>
    /// <param name="list">The list to which the packet will be added.</param>
    /// <param name="count">The number of occurrences of the character.</param>
    /// <param name="current">The character being processed.</param>
    internal static void WritePacketToList(List<string> list, int count, char current)
    {
        list.Add($"{count}{current}");
    }

    /// <summary>
    /// Encodes the input string using fixed-length RLE. This is the
    /// core algorithm used by the public methods <br/> <see cref="Encode"/> <br/>
    /// <see cref="EncodeAsList"/>.<br/>
    /// </summary>
    /// <param name="input">The input string to be encoded.</param>
    /// <returns>A list where each element is a two-character string <br/>
    /// representing the count of occurrences and the character itself.<br/></returns>
    /// <exception cref="ArgumentNullException">Thrown if 
    /// <paramref name="input"/> is <c>null</c>.</exception>
    internal static List<string> EncodeAlgorithm(string input)
    {
        if (input is null)
        {
            throw new ArgumentNullException(nameof(input));
        }

        if (string.Empty.Equals(input))
        {
            return [];
        }
        List<string> output = new List<string>();

        //Set initial values
        int counter = 1;
        char currentChar = input.First();
        //Loop over all chars left in the string
        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] == currentChar)
            {
                if (counter >= 9)
                {
                    //Single Digit Limit reached
                    WritePacketToList(output, counter, currentChar);
                    //reset counter value
                    counter = 1;
                    continue;
                }
                counter++;
            }
            else
            {
                WritePacketToList(output, counter, currentChar);
                currentChar = input[i];
                counter = 1;
            }
        }
        WritePacketToList(output, counter, currentChar);
        return output;
    }

    /// <summary>
    /// Splits an encoded string into a list of individual encoded packets (pairs of characters).
    /// </summary>
    /// <param name="input">The encoded string to be split.</param>
    /// <returns>A list of encoded packets.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="input"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="input"/> has an odd number of characters.</exception>
    internal static List<string> SplitIntoPackets(string input)
    {
        ArgumentNullException.ThrowIfNull(input, nameof(input));
        if (string.Empty.Equals(input))
        {
            return [];
        }

        if (input.Length % 2 != 0)
        {
            throw new ArgumentException("Input string must have an even number of characters.");
        }

        string copy = new string(input);
        List<string> packets = new List<string>();

        while (copy.Length > 0)
        {
            // Take the first 2 characters
            string pair = copy.Substring(0, 2);
            packets.Add(pair);

            // Remove the first 2 characters from the copied string
            copy = copy.Substring(2);
        }
        return packets;
    }

    /// <summary>
    /// Expands a single encoded packet into a StringBuilder.
    /// </summary>
    /// <param name="packet">The encoded packet to be expanded (a two-character string).</param>
    /// <param name="temp">The StringBuilder to append the decoded characters to.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="packet"/> or <paramref name="temp"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="packet"/> is empty or has a length other than 2.</exception>
    internal static void ExpandPacketIntoBuilder(string packet, StringBuilder temp)
    {
        ArgumentNullException.ThrowIfNull(temp, nameof(temp));
        ArgumentNullException.ThrowIfNull(packet, nameof(packet));
        ArgumentException.ThrowIfNullOrEmpty(packet, $"The packet: {nameof(packet)} is empty");
        if (2 != packet.Length)
        {
            throw new ArgumentException($"The length of packet: {nameof(packet)} has to be 2");
        }
        int count = int.Parse(packet[0].ToString());
        char target = packet[1];
        _ = temp.Append(target, count);
    }

    /// <summary>
    /// Decodes a list of encoded strings into a single string using single-digit fixed-length RLE.
    /// </summary>
    /// <param name="input">A list of encoded strings, where each element is in the format "count character".</param>
    /// <returns>The decoded string.</returns>
    internal static string DecodeAlgorithm(List<string> input)
    {
        StringBuilder sb = new StringBuilder();
        if (input.Count == 0)
        {
            return string.Empty;
        }
        Console.WriteLine($"Packets to expand = {input.Count}");
        foreach (string packet in input)
        {
            ExpandPacketIntoBuilder(packet, sb);
        }
        Console.WriteLine(sb.ToString());
        return sb.ToString();
    }
}

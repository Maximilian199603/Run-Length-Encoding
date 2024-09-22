using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLE.SingleByte;
/// <summary>
/// Represents a packet containing a count of characters and the character itself.
/// </summary>
internal class Packet
{
    public byte Amount { get; set; }
    private char Character { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Packet"/> class with the specified amount and character.
    /// </summary>
    /// <param name="amount">The number of occurrences of the character.</param>
    /// <param name="character">The character to be represented in the packet.</param>
    public Packet(byte amount, char character)
    {
        Amount = amount;
        Character = character;
    }

    /// <summary>
    /// Converts the packet to a list of bytes representing the amount and character.
    /// </summary>
    /// <returns>A list of bytes, where the first byte is the amount and the next two bytes are the UTF-16 representation of the character.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the resulting byte representation does not contain exactly 3 bytes.
    /// </exception>
    public List<byte> ToBytes()
    {
        List<byte> byteList = [Amount];

        // Convert character to UTF-16 bytes (2 bytes)
        byte[] charBytes = BitConverter.GetBytes(Character);
        byteList.AddRange(charBytes);

        if (byteList.Count != 3)
        {
            throw new InvalidOperationException("Packet byte representation must be exactly 3 bytes.");
        }

        return byteList;
    }

    /// <summary>
    /// Returns a string representation of the packet, consisting of the character repeated by its amount.
    /// </summary>
    /// <returns>A string representation of the packet.</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Character, Amount);
        return sb.ToString();
    }
}

using Burrows_Wheeler_Transform;

namespace Burrows_Wheeler_Transform_Tests;
public class Burrows_Wheeler_UnitTests
{
    private BurrowsWheeler _sut = new BurrowsWheeler();
    [Fact]
    public void BWT_Test()
    {
        // Setup
        string input = "BANANA";
        string expected = "ANNB" + '\a' + "AA";

        // Act
        string transform = new BurrowsWheeler().Transform(input);

        // Assert
        Assert.Equal(expected, transform);
    }

    [Fact]
    public void InverseBWT_Test()
    {
        // Setup
        string input = "ANNB" + '\a' + "AA";
        string expected = "BANANA";

        // Act
        string transform = new BurrowsWheeler().Inverse(input);

        // Assert
        Assert.Equal(expected, transform);
    }
}

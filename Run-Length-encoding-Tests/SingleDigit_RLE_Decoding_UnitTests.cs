using Newtonsoft.Json;
using RLE.SingleDigit;
using Xunit.Abstractions;

namespace Run_Length_Tests;
public class SingleDigit_RLE_Decoding_UnitTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SingleDigit_RLE_Decoding_UnitTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [ClassData(typeof(FixedLength_RLE_Decoding_TestData))]
    public void FixedLength_RLE_Decode_Test(string input, string expected)
    {
        string decoded = SingleDigitRLEDecoder.Decode(input);
        Assert.Equal(expected, decoded);
    }

    private class FixedLength_RLE_Decoding_TestData : TheoryData<string, string>
    {
        private static readonly string testDataPath = 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "FixedLengthRLEDecodingTestData.json");
        public FixedLength_RLE_Decoding_TestData()
        {

            var testdata = DecodingTestDataJsonHelper.GetTestData(testDataPath);
            foreach (DecodingTestData data in testdata)
            {
                Add(data.Input, data.ExpectedOutput);
            }
        }
    }
}

public class DecodingTestData
{
    public string Input { get; set; }
    public string ExpectedOutput { get; set; }
}

public class DecodingTestDataJsonHelper
{
    public static List<DecodingTestData> GetTestData(string filePath)
    {
        // Check if the file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"The file '{filePath}' does not exist.");
        }

        // Check if the file has a .json extension
        if (Path.GetExtension(filePath).ToLower() != ".json")
        {
            throw new InvalidOperationException($"The file '{filePath}' is not a JSON file.");
        }

        // Read and deserialize the JSON file
        string jsonString = File.ReadAllText(filePath);

        if (string.IsNullOrWhiteSpace(jsonString))
        {
            throw new InvalidOperationException("The JSON file is empty.");
        }

        var testDataList = JsonConvert.DeserializeObject<List<DecodingTestData>>(jsonString);

        if (testDataList == null)
        {
            return new List<DecodingTestData>();
        }

        return testDataList;
    }
}

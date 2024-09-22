using System.Text;
using Newtonsoft.Json;
using RLE.SingleDigit;

namespace Run_Length_Tests;
public class SingleDigit_RLE_Encoding_UnitTests
{
    [Theory]
    [ClassData(typeof(FixedLength_RLE_Encoding_TestData))]
    public void FixedLength_RLE_Encode_Test(string input, string expected)
    {
        //Act
        string output = SingleDigitRLEEncoder.Encode(input);

        //Assert
        Assert.Equal(expected, output);
    }

    [Theory]
    [ClassData(typeof(FixedLength_RLE_AsList_Encoding_TestData))]
    public void FixedLength_RLE_AsList_Encode_Test(string input, List<string> expected)
    {
        //Act
        List<string> output = SingleDigitRLEEncoder.EncodeAsList(input);

        //Assert
        Assert.Equal(expected, output);
    }

    //Data Providers
    private class FixedLength_RLE_Encoding_TestData : TheoryData<string, string>
    {
        private static readonly string testDataPath = 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "FixedLengthRLEEncodingTestData.json");
        public FixedLength_RLE_Encoding_TestData()
        {
            var testdata = EncodingTestDataJsonHelper.GetTestData(testDataPath);
            foreach (EncodingTestData data in testdata)
            {
                Add(data.Input, data.ConvertIntoStringVersion());
            }
        }
    }
    private class FixedLength_RLE_AsList_Encoding_TestData : TheoryData<string, List<string>>
    {
        private static readonly string testDataPath = 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "FixedLengthRLEEncodingTestData.json");
        public FixedLength_RLE_AsList_Encoding_TestData()
        {

            var testdata = EncodingTestDataJsonHelper.GetTestData(testDataPath);
            foreach (EncodingTestData data in testdata)
            {
                Add(data.Input, data.ExpectedOutput);
            }
        }
    }
}

public class EncodingTestData
{
    public string Input { get; set; }
    public List<string> ExpectedOutput { get; set; }

    public string ConvertIntoStringVersion()
    {
        StringBuilder sb = new StringBuilder();
        foreach (string item in ExpectedOutput)
        {
            sb.Append(item);
        }
        return sb.ToString();
    }
}
public class EncodingTestDataJsonHelper
{
    public static List<EncodingTestData> GetTestData(string filePath)
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

        var testDataList = JsonConvert.DeserializeObject<List<EncodingTestData>>(jsonString);

        if (testDataList == null)
        {
            return new List<EncodingTestData>();
        }

        return testDataList;
    }
}
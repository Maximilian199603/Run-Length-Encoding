using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLE.SingleByte;

namespace Run_Length_Tests;
public class SingleByte_RLE_Encoding_UnitTests
{
    [Theory]
    [ClassData(typeof(SingleByte_RLE_Encoding_TestData))]
    public void SingleByteEncodingTest(string input, byte[] expected)
    {
        // Act
        byte[] actual = SingleByteRLEEncoder.Encode(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    private class SingleByte_RLE_Encoding_TestData : TheoryData<string, byte[]>
    {
        private static readonly string testDataPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "SingleByteEncodingTestData.json");
        public SingleByte_RLE_Encoding_TestData()
        {

            var testdata = SingleByteEncodingTestDataJsonHelper.GetTestData(testDataPath);
            foreach (SingleByteEncodingTestData data in testdata)
            {
                Add(data.Input, data.ExpectedOutput);
            }
        }
    }
}

public class SingleByteEncodingTestData
{
    public string Input { get; set; }
    public byte[] ExpectedOutput { get; set; }
}

public class SingleByteEncodingTestDataJsonHelper
{
    public static List<SingleByteEncodingTestData> GetTestData(string filePath)
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

        var testDataList = JsonConvert.DeserializeObject<List<SingleByteEncodingTestData>>(jsonString);

        if (testDataList == null)
        {
            return new List<SingleByteEncodingTestData>();
        }

        return testDataList;
    }
}

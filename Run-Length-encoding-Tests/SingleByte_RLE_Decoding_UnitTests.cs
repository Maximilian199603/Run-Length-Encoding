using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLE.SingleByte;

namespace Run_Length_Tests;
public class SingleByte_RLE_Decoding_UnitTests
{
    [Theory]
    [ClassData(typeof(SingleByte_RLE_Decoding_TestData))]
    public void SingleByteDecodingTest(byte[] input, string expected)
    {
        // Act
        string actual = SingleByteRLEDecoder.Decode(input);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SingleByte_RLE_Decoding_TestData))]
    public void SingleByteDecodingFromListTest(byte[] input, string expected)
    {
        // Act
        string actual = SingleByteRLEDecoder.Decode(input.ToList());

        // Assert
        Assert.Equal(expected, actual);
    }

    private class SingleByte_RLE_Decoding_TestData : TheoryData<byte[], string>
    {
        private static readonly string testDataPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "SingleByteDecodingTestData.json");
        public SingleByte_RLE_Decoding_TestData()
        {

            var testdata = SingleByteDecodingTestDataJsonHelper.GetTestData(testDataPath);
            foreach (SingleByteDecodingTestData data in testdata)
            {
                Add(data.Input, data.ExpectedOutput);
            }
        }
    }
}

public class SingleByteDecodingTestData
{
    public byte[] Input { get; set; }
    public string ExpectedOutput { get; set; }
}

public class SingleByteDecodingTestDataJsonHelper
{
    public static List<SingleByteDecodingTestData> GetTestData(string filePath)
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

        var testDataList = JsonConvert.DeserializeObject<List<SingleByteDecodingTestData>>(jsonString);

        if (testDataList == null)
        {
            return new List<SingleByteDecodingTestData>();
        }

        return testDataList;
    }
}

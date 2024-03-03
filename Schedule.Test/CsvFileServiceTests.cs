using FileService;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Schedule.Test;

[TestFixture]
public class CsvFileServiceTests
{
    private string csvFilePath;
    private CsvFileService csvService;

    [SetUp]
    public void Setup()
    {
        csvFilePath = "test.csv";
        csvService = new CsvFileService();
    }

    [TearDown]
    public void Cleanup()
    {
        if (File.Exists(csvFilePath))
        {
            File.Delete(csvFilePath);
        }
    }
    [Test]
    public async Task ReadAsync_ShouldReturnCorrectData()
    {
        // Arrange
        var csvFilePath = "test.csv";
        var csvService = new CsvFileService();
        var testData = new List<Person>
        {
            new Person { Name = "John Doe", Age = 30 },
            new Person { Name = "Jane Doe", Age = 25 }
        };
        await csvService.WriteAsync(csvFilePath, testData);

        // Act
        var result = await csvService.ReadAsync<Person>(csvFilePath);

        // Assert
        Assert.That(result.Count(), Is.EqualTo(testData.Count));
        for (int i = 0; i < testData.Count; i++)
        {
            Assert.That(result.ToList()[i].Name, Is.EqualTo(testData[i].Name));
            Assert.That(result.ToList()[i].Age, Is.EqualTo(testData[i].Age));
        }

        // Cleanup
        //File.Delete(csvFilePath);
    }

    [Test]
    public async Task WriteAsync_ShouldCreateFileWithCorrectData()
    {
        // Arrange
        var csvFilePath = "test.csv";
        var csvService = new CsvFileService();
        var testData = new List<Person>
    {
        new Person { Name = "John Doe", Age = 30 },
        new Person { Name = "Jane Doe", Age = 25 }
    };

        // Act
        await csvService.WriteAsync(csvFilePath, testData);

        // Assert
        Assert.That(File.Exists(csvFilePath), Is.True);

        // Clean up
        File.Delete(csvFilePath);
    }


}
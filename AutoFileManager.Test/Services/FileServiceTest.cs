
using System.Text;
using AutoFileManager.Services;
using AutoFileManager.Services.Interfaces;
using AutoFileManager.Settings;
using Microsoft.Extensions.Logging;
using Moq;

namespace AutoFileManager.Test.Services
{
    public class FileServiceTest
    {
        private readonly Mock<ILogger<FileService>> mockLogger;
        private readonly Mock<IDirectoryService> mockDirectoryService;
        private readonly Mock<IConfigurationsHelper> mockConfigurationsHelper;
        private readonly FileService fileService;

        public FileServiceTest()
        {
            mockLogger = new Mock<ILogger<FileService>>();
            mockDirectoryService = new Mock<IDirectoryService>();
            mockConfigurationsHelper = new Mock<IConfigurationsHelper>();

            mockConfigurationsHelper.Setup(c => c.FileRootOutputDirectory).Returns("testOutputDirectory");
            mockConfigurationsHelper.Setup(c => c.DefaultPositionRegistryType).Returns((0, 5));

            fileService = new FileService(
                mockLogger.Object,
                mockConfigurationsHelper.Object,
                mockDirectoryService.Object
            );
        }

        [Fact]
        public void GetRestryTypeContent_ValidFilePath_ReturnsCorrectDictionary()
        {
            // Arrange
            var filePath = "testFile.txt";
            var fileContent = "12345line1\n12345line2\n67890line3";
            File.WriteAllText(filePath, fileContent);

            // Act
            var result = fileService.GetRestryTypeContent(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains("12345", result.Keys);
            Assert.Contains("67890", result.Keys);
            Assert.Equal(2, result["12345"].Count);
            Assert.Single(result["67890"]);
        }

        [Fact]
        public void WriteFile_ValidInput_CreatesFile()
        {
            // Arrange
            var filePath = "testFile.txt";
            var registryType = "12345";
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Header");
            stringBuilder.AppendLine("Content");

            mockDirectoryService.Setup(ds => ds.GetFileNameWithoutExtension(filePath)).Returns("testFile");
            mockDirectoryService.Setup(ds => ds.CreateFile(It.IsAny<string>(), stringBuilder));

            // Act
            fileService.WriteFile(filePath, registryType, stringBuilder);

            // Assert
            var expectedFilePath = "testOutputDirectory/12345_testFile.csv";
            mockDirectoryService.Verify(ds => ds.CreateFile(expectedFilePath, stringBuilder), Times.Once);
        }

        [Fact]
        public void WriteFile_ExceptionThrown_LogsError()
        {
            // Arrange
            var filePath = "testFile.txt";
            var registryType = "12345";
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Header");
            stringBuilder.AppendLine("Content");

            mockDirectoryService.Setup(ds => ds.GetFileNameWithoutExtension(filePath)).Returns("testFile");
            mockDirectoryService.Setup(ds => ds.CreateFile(It.IsAny<string>(), stringBuilder)).Throws(new Exception("Test exception"));

            // Act & Assert
            Assert.Throws<Exception>(() => fileService.WriteFile(filePath, registryType, stringBuilder));
        }
    }
}
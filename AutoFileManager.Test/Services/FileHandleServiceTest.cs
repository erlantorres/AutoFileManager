
using System.Reflection;
using System.Text;
using AutoFileManager.Data.Entities;
using AutoFileManager.Dtos;
using AutoFileManager.Services;
using AutoFileManager.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace AutoFileManager.Test.Services
{
    public class FileHandleServiceTest
    {
        private readonly Mock<ILogger<FileHandleService>> mockLogger;
        private readonly Mock<IContentInformationService> mockContentInformationService;
        private readonly Mock<IFileService> mockFileService;
        private readonly FileHandleService fileHandleService;

        public FileHandleServiceTest()
        {
            mockLogger = new Mock<ILogger<FileHandleService>>();
            mockContentInformationService = new Mock<IContentInformationService>();
            mockFileService = new Mock<IFileService>();

            fileHandleService = new FileHandleService(
                mockLogger.Object,
                mockContentInformationService.Object,
                mockFileService.Object
            );
        }

        [Fact]
        public void GenerateFile_ValidFilePath_ProcessesFile()
        {
            // Arrange
            var filePath = "validFilePath.txt";
            var registryTypeContent = new Dictionary<string, List<string>>
            {
                { "key1", new List<string> { "12345", "67890" } }
            };

            var contentInformation = new InformationContentDto
            {
                Contents = new List<ContentTypeEntity>
                {
                    new ContentTypeEntity { Order = 1, Description = "Field1", Length = 5 },
                    new ContentTypeEntity { Order = 2, Description = "Field2", Length = 5 }
                }
            };

            mockFileService.Setup(fs => fs.GetRestryTypeContent(filePath)).Returns(registryTypeContent);
            mockContentInformationService.Setup(cis => cis.GetInformationContent("key1")).Returns(contentInformation);

            // Act
            fileHandleService.GenerateFile(filePath);

            // Assert
            mockFileService.Verify(fs => fs.WriteFile(filePath, "key1", It.IsAny<StringBuilder>()), Times.Once);
        }

        [Fact]
        public void TransformLinesToStringBuilder_ValidInput_ReturnsStringBuilder()
        {
            // Arrange
            var keyValuePair = new KeyValuePair<string, List<string>>("key", new List<string> { "12345", "67890" });
            var contents = new List<ContentTypeEntity>
            {
                new ContentTypeEntity { Order = 1, Description = "Field1", Length = 5 },
            };

            MethodInfo methodInfo = typeof(FileHandleService).GetMethod("TransformLinesToStringBuilder", BindingFlags.NonPublic | BindingFlags.Instance);

            // Act
            var result = (StringBuilder)methodInfo.Invoke(fileHandleService, new object[] { keyValuePair, contents });

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Field1\r\n12345\r\n67890", result.ToString());
        }

        [Fact]
        public void GenerateFile_ExceptionThrown_ReturnsThrow()
        {
            // Arrange
            var filePath = "invalidFilePath.txt";
            mockFileService.Setup(fs => fs.GetRestryTypeContent(filePath)).Throws(new Exception(""));

            // Act & Assert
            Assert.Throws<Exception>(() => fileHandleService.GenerateFile(filePath));
        }
    }
}
using AutoFileManager.Services;
using AutoFileManager.Services.Interfaces;
using AutoFileManager.Settings;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace AutoFileManager.Test.Services
{
    public class FileRelayServiceTest
    {
        private readonly Mock<IDirectoryService> mockDirectoryService;
        private readonly Mock<IFileHandleService> mockFileHandleService;
        private readonly Mock<IConfigurationsHelper> mockConfigurationsHelper;
        private readonly FileRelayService fileRelayService;

        public FileRelayServiceTest()
        {
            mockDirectoryService = new Mock<IDirectoryService>();
            mockFileHandleService = new Mock<IFileHandleService>();
            mockConfigurationsHelper = new Mock<IConfigurationsHelper>();

            mockConfigurationsHelper.Setup(c => c.FileRootInputDirectory).Returns("testDirectory");

            fileRelayService = new FileRelayService(
                new NullLogger<FileRelayService>(),
                mockConfigurationsHelper.Object,
                mockDirectoryService.Object,
                mockFileHandleService.Object
            );
        }

        [Fact]
        public async Task ProcessFile_FilePathIsEmpty_DoesNotProcessFile()
        {
            // Arrange
            mockDirectoryService.Setup(ds => ds.GetFilePath(It.IsAny<string>())).Returns(string.Empty);

            // Act
            await fileRelayService.ProcessFile();

            // Assert
            mockFileHandleService.Verify(fhs => fhs.GenerateFile(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ProcessFile_FilePathIsNotEmpty_ProcessesFile()
        {
            // Arrange
            var filePath = "testFile.txt";
            mockDirectoryService.Setup(ds => ds.GetFilePath(It.IsAny<string>())).Returns(filePath);
            mockDirectoryService.Setup(ds => ds.MoveFile(filePath, It.IsAny<string>())).Returns(filePath);

            // Act
            await fileRelayService.ProcessFile();

            // Assert
            mockFileHandleService.Verify(fhs => fhs.GenerateFile(filePath), Times.Once);
        }

        [Fact]
        public void Constructor_FileRootInputDirectoryIsEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            mockConfigurationsHelper.Setup(c => c.FileRootInputDirectory).Returns(string.Empty);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FileRelayService(
                new NullLogger<FileRelayService>(),
                mockConfigurationsHelper.Object,
                mockDirectoryService.Object,
                mockFileHandleService.Object
            ));
        }

        [Fact]
        public async Task ProcessFile_FilePathIsNotEmpty_MovesFileToProcessing()
        {
            // Arrange
            var filePath = "testFile.txt";
            var processingPath = "testDirectory/PROCESSING/testFile.txt";
            mockDirectoryService.Setup(ds => ds.GetFilePath(It.IsAny<string>())).Returns(filePath);
            mockDirectoryService.Setup(ds => ds.MoveFile(filePath, "testDirectory/PROCESSING")).Returns(processingPath);

            // Act
            await fileRelayService.ProcessFile();

            // Assert
            mockDirectoryService.Verify(ds => ds.MoveFile(filePath, "testDirectory/PROCESSING"), Times.Once);
        }

        [Fact]
        public async Task ProcessFile_GenerateFileThrowsException_LogsErrorAndMovesFileToFail()
        {
            // Arrange
            var filePath = "testFile.txt";
            mockDirectoryService.Setup(ds => ds.GetFilePath(It.IsAny<string>())).Returns(filePath);
            mockDirectoryService.Setup(ds => ds.MoveFile(filePath, It.IsAny<string>())).Returns(filePath);
            mockFileHandleService.Setup(fhs => fhs.GenerateFile(filePath)).Throws(new Exception());

            // Act
            await fileRelayService.ProcessFile();

            // Assert
            mockFileHandleService.Verify(fhs => fhs.GenerateFile(filePath), Times.Once);
            mockDirectoryService.Verify(ds => ds.MoveFile(filePath, "testDirectory/FAIL"), Times.Once);
            // Verifique se o log de erro foi chamado
        }
    }
}

using System.Text;
using AutoFileManager.Services;

namespace AutoFileManager.Test.Services
{
    public class DirectoryServiceTest
    {
        private readonly DirectoryService directoryService;

        public DirectoryServiceTest()
        {
            directoryService = new DirectoryService();
        }

        [Fact]
        public void GetFiles_ValidPath_ReturnsFiles()
        {
            // Arrange
            var path = "testDirectory";
            Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "file1.txt"), "content");
            File.WriteAllText(Path.Combine(path, "file2.txt"), "content");

            // Act
            var files = directoryService.GetFiles(path);

            // Assert
            Assert.NotNull(files);
            Assert.Equal(2, files.Length);

            // Cleanup
            Directory.Delete(path, true);
        }

        [Fact]
        public void MoveFile_ValidPaths_MovesFile()
        {
            // Arrange
            var originPath = "testDirectory/file1.txt";
            var destinationPath = "destinationDirectory";
            Directory.CreateDirectory("testDirectory");
            File.WriteAllText(originPath, "content");

            // Act
            var result = directoryService.MoveFile(originPath, destinationPath);

            // Assert
            Assert.True(File.Exists(result));
            Assert.False(File.Exists(originPath));

            // Cleanup
            Directory.Delete("testDirectory", true);
            Directory.Delete(destinationPath, true);
        }

        [Fact]
        public void GetFilePath_ValidPath_ReturnsFirstFile()
        {
            // Arrange
            var path = "testDirectory";
            Directory.CreateDirectory(path);
            var filePath = Path.Combine(path, "file1.txt");
            File.WriteAllText(filePath, "content");

            // Act
            var result = directoryService.GetFilePath(path);

            // Assert
            Assert.Equal(filePath, result);

            // Cleanup
            Directory.Delete(path, true);
        }

        [Fact]
        public void FileExists_ValidPath_ReturnsTrue()
        {
            // Arrange
            var path = "testFile.txt";
            File.WriteAllText(path, "content");

            // Act
            var result = directoryService.FileExists(path);

            // Assert
            Assert.True(result);

            // Cleanup
            File.Delete(path);
        }

        [Fact]
        public void GetFileName_ValidPath_ReturnsFileName()
        {
            // Arrange
            var path = "testDirectory/testFile.txt";

            // Act
            var result = directoryService.GetFileName(path);

            // Assert
            Assert.Equal("testFile.txt", result);
        }

        [Fact]
        public void GetFileNameWithoutExtension_ValidPath_ReturnsFileNameWithoutExtension()
        {
            // Arrange
            var path = "testDirectory/testFile.txt";

            // Act
            var result = directoryService.GetFileNameWithoutExtension(path);

            // Assert
            Assert.Equal("testFile", result);
        }

        [Fact]
        public void CreateFile_ValidPath_CreatesFile()
        {
            // Arrange
            var path = "testDirectory/testFile.txt";
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("content");

            // Act
            directoryService.CreateFile(path, stringBuilder);

            // Assert
            Assert.True(File.Exists(path));

            // Cleanup
            Directory.Delete("testDirectory", true);
        }
    }
}
using AutoFileManager.Data.Entities;
using AutoFileManager.Data.Repositories.Interfaces;
using AutoFileManager.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace AutoFileManager.Test.Services
{
    public class ContentInformationServiceTest
    {
        private readonly Mock<ILogger<ContentInformationService>> mockLogger;
        private readonly Mock<IInformationTypeRepository> mockInformationTypeRepository;
        private readonly Mock<IContentTypeRepository> mockContentTypeRepository;
        private readonly ContentInformationService contentInformationService;

        public ContentInformationServiceTest()
        {
            mockLogger = new Mock<ILogger<ContentInformationService>>();
            mockInformationTypeRepository = new Mock<IInformationTypeRepository>();
            mockContentTypeRepository = new Mock<IContentTypeRepository>();

            contentInformationService = new ContentInformationService(
                mockLogger.Object,
                mockInformationTypeRepository.Object,
                mockContentTypeRepository.Object
            );
        }

        [Fact]
        public void GetInformationContent_ValidRegistryType_ReturnsInformationContent()
        {
            // Arrange
            var registryType = "validType";
            var information = new InformationTypeEntity { RegistryType = registryType, Description = "Test Description" };
            var contents = new List<ContentTypeEntity>
            {
                new ContentTypeEntity { Order = 1, Description = "Field1" },
                new ContentTypeEntity { Order = 2, Description = "Field2" }
            };

            mockInformationTypeRepository.Setup(repo => repo.GetInformationTypeByRegistry(registryType)).Returns(information);
            mockContentTypeRepository.Setup(repo => repo.GetContentTypesByRegistry(registryType)).Returns(contents);

            // Act
            var result = contentInformationService.GetInformationContent(registryType);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Description", result.Name);
            Assert.Equal(2, result.Contents.Count);
        }

        [Fact]
        public void GetInformationContent_InvalidRegistryType_ReturnsDefault()
        {
            // Arrange
            var registryType = "invalidType";
            mockInformationTypeRepository.Setup(repo => repo.GetInformationTypeByRegistry(registryType)).Returns((InformationTypeEntity)null);

            // Act
            var result = contentInformationService.GetInformationContent(registryType);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetInformationContent_NoContents_ReturnsDefault()
        {
            // Arrange
            var registryType = "validType";
            var information = new InformationTypeEntity { RegistryType = registryType, Description = "Test Description" };

            mockInformationTypeRepository.Setup(repo => repo.GetInformationTypeByRegistry(registryType)).Returns(information);
            mockContentTypeRepository.Setup(repo => repo.GetContentTypesByRegistry(registryType)).Returns((IEnumerable<ContentTypeEntity>)null);

            // Act
            var result = contentInformationService.GetInformationContent(registryType);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetInformationContent_ExceptionThrown_LogsErrorAndThrows()
        {
            // Arrange
            var registryType = "validType";
            mockInformationTypeRepository.Setup(repo => repo.GetInformationTypeByRegistry(registryType)).Throws(new Exception());

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => contentInformationService.GetInformationContent(registryType));
            Assert.Equal($"Error getting information content for the registry type {registryType}! Please contact the support for assistance.", exception.Message);
        }
    }
}
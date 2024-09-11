
using AutoFileManager.Data.Repositories.Interfaces;
using AutoFileManager.Dtos;
using AutoFileManager.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoFileManager.Services
{
    public class ContentInformationService : IContentInformationService
    {
        private readonly ILogger<ContentInformationService> logger;
        private readonly IInformationTypeRepository informationTypeRepository;
        private readonly IContentTypeRepository contentTypeRepository;

        public ContentInformationService(
            ILogger<ContentInformationService> logger,
            IInformationTypeRepository informationTypeRepository,
            IContentTypeRepository contentTypeRepository
        )
        {
            this.logger = logger;
            this.informationTypeRepository = informationTypeRepository;
            this.contentTypeRepository = contentTypeRepository;
        }

        public InformationContentDto GetInformationContent(string registryType)
        {
            try
            {
                var information = informationTypeRepository.GetInformationTypeByRegistry(registryType);
                if (information == null || string.IsNullOrWhiteSpace(information.RegistryType))
                {
                    //throw new ArgumentException(nameof(information), "Information not found!");
                    return default;
                }

                var contents = contentTypeRepository.GetContentTypesByRegistry(registryType);
                if (contents == null || !contents.Any())
                {
                    //throw new ArgumentException(nameof(contents), "Content not found");
                    return default;
                }

                return new InformationContentDto
                {
                    Name = information.Description,
                    Contents = contents.ToList()
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GetInformationContent - fail to get information for the registry type {registryType} : Error Message: {ex.Message}");
                throw new Exception($"Error getting information content for the registry type {registryType}! Please contact the support for assistance.");
            }
        }
    }
}
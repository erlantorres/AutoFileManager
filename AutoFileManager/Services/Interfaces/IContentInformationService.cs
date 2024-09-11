
using AutoFileManager.Data.Entities;
using AutoFileManager.Dtos;

namespace AutoFileManager.Services.Interfaces
{
    public interface IContentInformationService
    {
        InformationContentDto GetInformationContent(string registryType);
    }
}

using AutoFileManager.Data.Entities;

namespace AutoFileManager.Data.Repositories.Interfaces
{
    public interface IInformationTypeRepository
    {
        InformationTypeEntity GetInformationTypeByRegistry(string registryType);
    }
}
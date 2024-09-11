
using AutoFileManager.Data.Entities;
using AutoFileManager.Data.Providers.Interfaces;
using AutoFileManager.Data.Repositories.Interfaces;
using System.Linq;

namespace AutoFileManager.Data.Repositories
{
    public class InformationTypeRepository : IInformationTypeRepository
    {
        private readonly IJsonContext jsonContext;

        public InformationTypeRepository(IJsonContext jsonContext)
        {
            this.jsonContext = jsonContext;
        }

        public InformationTypeEntity GetInformationTypeByRegistry(string registryType)
        {
            var informationType = jsonContext.GetData<InformationTypeEntity>();
            return informationType.FirstOrDefault(x => x.RegistryType == registryType);
        }
    }
}
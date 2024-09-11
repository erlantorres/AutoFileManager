
using System.Data;
using System.Collections.Generic;
using System.Linq;
using AutoFileManager.Data.Entities;
using AutoFileManager.Data.Providers.Interfaces;
using AutoFileManager.Data.Repositories.Interfaces;

namespace AutoFileManager.Data.Repositories
{
    public class ContentTypeRepository : IContentTypeRepository
    {
        private readonly IJsonContext jsonContext;

        public ContentTypeRepository(IJsonContext jsonContext)
        {
            this.jsonContext = jsonContext;
        }

        public IEnumerable<ContentTypeEntity> GetContentTypesByRegistry(string registryType)
        {
            var contents = jsonContext.GetData<ContentTypeEntity>();
            return contents.Where(x => x.RegistryType == registryType);
        }
    }
}
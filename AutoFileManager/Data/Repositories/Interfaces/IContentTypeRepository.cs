using AutoFileManager.Data.Entities;
using System.Collections.Generic;

namespace AutoFileManager.Data.Repositories.Interfaces
{
    public interface IContentTypeRepository
    {
        IEnumerable<ContentTypeEntity> GetContentTypesByRegistry(string registryType);
    }
}
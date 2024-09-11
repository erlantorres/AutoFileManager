using System.Collections.Generic;

namespace AutoFileManager.Data.Providers.Interfaces
{
    public interface IJsonContext
    {
        IEnumerable<T> GetData<T>();
    }
}
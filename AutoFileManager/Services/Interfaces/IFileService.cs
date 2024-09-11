using System.Collections.Generic;
using System.Text;

namespace AutoFileManager.Services.Interfaces
{
    public interface IFileService
    {
        Dictionary<string, List<string>> GetRestryTypeContent(string filePathInput);
        void WriteFile(string filePath, string registryType, StringBuilder stringBuilder);
    }
}
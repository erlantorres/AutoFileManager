using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFileManager.Services.Interfaces
{
    public interface IFileService
    {
        void WriteFile(string filePath, string registryType, StringBuilder stringBuilder);
    }
}
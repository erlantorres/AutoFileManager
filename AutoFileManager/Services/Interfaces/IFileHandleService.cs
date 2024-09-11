using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFileManager.Services.Interfaces
{
    public interface IFileHandleService
    {
        void GenerateFile(string filePathInput);
    }
}
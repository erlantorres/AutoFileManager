

using System.IO;
using System.Text;

namespace AutoFileManager.Services.Interfaces
{
    public interface IDirectoryService
    {
        string[] GetFiles(string directory);
        string MoveFile(string originPath, string destinationPath);
        string GetFilePath(string directory);
        bool FileExists(string path);
        string GetFileName(string path);
        string GetFileNameWithoutExtension(string path);
        void CreateFile(string newFilePath, StringBuilder stringBuilder);
    }
}
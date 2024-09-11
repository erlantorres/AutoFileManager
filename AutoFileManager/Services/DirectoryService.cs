
using AutoFileManager.Services.Interfaces;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoFileManager.Services
{
    public class DirectoryService : IDirectoryService
    {
        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public string[] GetFiles(string path)
        {
            CreateDirectory(path);

            return Directory.GetFiles(path);
        }

        public string MoveFile(string originPath, string destinationPath)
        {
            CreateDirectory(destinationPath);

            var fileName = Path.GetFileName(originPath);
            var filePath = $"{destinationPath}/{fileName}";
            if (FileExists(originPath) && !FileExists(destinationPath))
                File.Move(originPath, filePath);

            return filePath;
        }

        public string GetFilePath(string path)
        {
            var files = GetFiles(path).ToList();
            return files.FirstOrDefault();
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public void CreateFile(string newFilePath, StringBuilder stringBuilder)
        {
            var directory = Directory.GetParent(newFilePath);
            CreateDirectory(directory.FullName);
            
            File.WriteAllText(newFilePath, stringBuilder.ToString());
        }
    }
}
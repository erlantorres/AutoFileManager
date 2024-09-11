using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoFileManager.Services.Interfaces;
using AutoFileManager.Settings;
using Microsoft.Extensions.Logging;

namespace AutoFileManager.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> logger;
        private readonly IDirectoryService directoryService;
        private readonly string fileRootOutputDirectory;
        private readonly (int start, int end) defaultPositionRegistryType;

        public FileService(
            ILogger<FileService> logger,
            IConfigurationsHelper configurationsHelper,
            IDirectoryService directoryService
        )
        {
            this.logger = logger;
            this.directoryService = directoryService;

            this.fileRootOutputDirectory = configurationsHelper.FileRootOutputDirectory;
            this.defaultPositionRegistryType = configurationsHelper.DefaultPositionRegistryType;

            if (string.IsNullOrWhiteSpace(fileRootOutputDirectory))
            {
                throw new ArgumentNullException(nameof(fileRootOutputDirectory), "File root directory not found");
            }
        }

        public Dictionary<string, List<string>> GetRestryTypeContent(string filePathInput)
        {
            var registryTypeContent = new Dictionary<string, List<string>>();
            using StreamReader sr = new StreamReader(filePathInput);
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var (start, end) = defaultPositionRegistryType;
                    var type = line.Substring(start, end);

                    if (!registryTypeContent.ContainsKey(type))
                    {
                        registryTypeContent[type] = new List<string>();
                    }

                    registryTypeContent[type].Add(line);
                }
            }

            return registryTypeContent;
        }

        public void WriteFile(string filePath, string registryType, StringBuilder stringBuilder)
        {
            var newFileName = "";

            try
            {
                var nameOrigin = directoryService.GetFileNameWithoutExtension(filePath);
                newFileName = $"{registryType}_{nameOrigin}.csv";
                var newFilePath = $"{fileRootOutputDirectory}/{newFileName}";
                directoryService.CreateFile(newFilePath, stringBuilder);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error to write file [{newFileName}]. Error message: {ex.Message}");
                throw;
            }
        }
    }
}
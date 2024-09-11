using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public FileService(
            ILogger<FileService> logger,
            IConfigurationsHelper configurationsHelper,
            IDirectoryService directoryService
        )
        {
            this.logger = logger;
            this.directoryService = directoryService;

            this.fileRootOutputDirectory = configurationsHelper.FileRootOutputDirectory;

            if (string.IsNullOrWhiteSpace(fileRootOutputDirectory))
            {
                throw new ArgumentNullException(nameof(fileRootOutputDirectory), "File root directory not found");
            }
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
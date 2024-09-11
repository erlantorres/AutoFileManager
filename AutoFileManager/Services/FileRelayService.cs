
using AutoFileManager.Data.Enums;
using AutoFileManager.Services.Interfaces;
using System.Threading.Tasks;
using System;
using AutoFileManager.Settings;
using Microsoft.Extensions.Logging;

namespace AutoFileManager.Services
{
    public class FileRelayService : IFileRelayService
    {
        private readonly ILogger<FileRelayService> logger;
        private readonly IDirectoryService directoryService;
        private readonly IFileHandleService fileHandleService;
        private readonly string fileRootInputDirectory;

        public FileRelayService(
            ILogger<FileRelayService> logger,
            IConfigurationsHelper configurationsHelper,
            IDirectoryService directoryService,
            IFileHandleService fileHandleService
        )
        {
            this.logger = logger;
            this.directoryService = directoryService;
            this.fileHandleService = fileHandleService;
            this.fileRootInputDirectory = configurationsHelper.FileRootInputDirectory;

            if (string.IsNullOrWhiteSpace(fileRootInputDirectory))
            {
                throw new ArgumentNullException(nameof(fileRootInputDirectory), "File root directory not found");
            }
        }

        public Task ProcessFile()
        {
            string filePath = string.Empty;

            try
            {
                filePath = directoryService.GetFilePath(fileRootInputDirectory);
                if (string.IsNullOrEmpty(filePath))
                {
                    return Task.CompletedTask;
                }

                filePath = MoveFile(filePath, FileStatus.PROCESSING);

                fileHandleService.GenerateFile(filePath);

                MoveFile(filePath, FileStatus.PROCESSED);
            }
            catch (Exception ex)
            {
                var fileName = directoryService.GetFileName(filePath);
                if (!string.IsNullOrEmpty(filePath))
                {
                    MoveFile(filePath, FileStatus.FAIL);
                }

                logger.LogError(ex, $"Fail process file {fileName}. Error message: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private string MoveFile(string file, FileStatus status)
        {
            var directory = "";

            try
            {
                directory = $"{fileRootInputDirectory}/{Enum.GetName(typeof(FileStatus), status)}";
                return directoryService.MoveFile(file, directory);
            }
            catch (Exception ex)
            {
                throw new Exception($"Fail moving file [{file}] to directory {directory}", ex);
            }
        }
    }
}
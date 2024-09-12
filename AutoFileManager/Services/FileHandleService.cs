using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFileManager.Data.Entities;
using AutoFileManager.Services.Interfaces;
using AutoFileManager.Settings;
using Microsoft.Extensions.Logging;

namespace AutoFileManager.Services
{
    public class FileHandleService : IFileHandleService
    {
        private readonly ILogger<FileHandleService> logger;
        private readonly IContentInformationService contentInformationService;
        private readonly IFileService fileService;

        public FileHandleService(
            ILogger<FileHandleService> logger,
            IContentInformationService contentInformationService,
            IFileService fileService
        )
        {
            this.logger = logger;
            this.contentInformationService = contentInformationService;
            this.fileService = fileService;
        }

        public void GenerateFile(string filePathInput)
        {
            try
            {
                Dictionary<string, List<string>> registryTypeContent = fileService.GetRestryTypeContent(filePathInput);
                foreach (var keyValuePair in registryTypeContent)
                {
                    string key = keyValuePair.Key;

                    var contInf = contentInformationService.GetInformationContent(key);
                    if (contInf == null || contInf == default)
                    {
                        continue;
                    }

                    // order contents
                    var contents = contInf.Contents.OrderBy(x => x.Order).ToList();

                    var stringBuilder = TransformLinesToStringBuilder(keyValuePair, contents);

                    fileService.WriteFile(filePathInput, key, stringBuilder);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"GenerateFile - Error message: {ex.Message}");
                throw;
            }
        }

        private StringBuilder TransformLinesToStringBuilder(KeyValuePair<string, List<string>> keyValuePair, List<ContentTypeEntity> contents)
        {
            var stringBuilder = new StringBuilder();

            string hearder = GetHeader(contents);
            stringBuilder.AppendLine(hearder);

            List<string> values = keyValuePair.Value;
            foreach (var line in values)
            {
                var index = 0;
                var listAux = new string[contents.Count];
                contents.ForEach(content =>
                {
                    var field = "";
                    if (line.Length >= (index + content.Length))
                    {
                        field = line.Substring(index, content.Length);
                    }

                    listAux[content.Order - 1] = field.Replace(",", " ");
                    index += content.Length;
                });

                stringBuilder.AppendLine(string.Join(", ", listAux));
            }

            return stringBuilder;
        }

        private static string GetHeader(IEnumerable<ContentTypeEntity> contents)
        {
            var descriptions = contents.Select(x => x.Description.Replace(",", "/")).ToList();
            return string.Join(", ", descriptions);
        }
    }
}
using AutoFileManager.Data.Providers.Interfaces;
using AutoFileManager.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutoFileManager.Data.Providers
{
    public class JsonContext : IJsonContext
    {
        private readonly string filePath;

        public JsonContext(IConfigurationsHelper configurationsHelper)
        {
            this.filePath = configurationsHelper.DatabaseRootDirectory;
        }

        private string GetFilePath<T>()
        {
            var entityName = typeof(T).Name;
            if (string.IsNullOrWhiteSpace(entityName))
            {
                throw new ArgumentNullException(nameof(T), "Json Database file not found!");
            }

            var fileName = entityName.Replace("Entity", ".json");
            return $"{filePath}/{fileName}";
        }

        public IEnumerable<T> GetData<T>()
        {
            var filePath = GetFilePath<T>();
            using var reader = new StreamReader(filePath);
            var json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }
    }
}
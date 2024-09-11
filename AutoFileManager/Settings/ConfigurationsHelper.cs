using System;

namespace AutoFileManager.Settings
{

    public interface IConfigurationsHelper
    {
        string DatabaseRootDirectory { get; }
        string FileRootInputDirectory { get; }
        string FileRootOutputDirectory { get; }
        (int start, int end) DefaultPositionRegistryType { get; }
    }

    public class ConfigurationsHelper : IConfigurationsHelper
    {
        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public string DatabaseRootDirectory
        {
            get
            {
                // it could be returned by appSettings.json
                return $"{_baseDirectory}/Data/database";
            }
        }

        public string FileRootInputDirectory
        {
            // it could be returned by appSettings.json or database
            get
            {
                return $"{_baseDirectory}/Public/input";
            }
        }

        public string FileRootOutputDirectory
        {
            // it could be returned by appSettings.json or database
            get { return $"{_baseDirectory}/Public/output"; }
        }

        public (int start, int end) DefaultPositionRegistryType
        {
            get
            {
                return (14, 1);
            }
        }
    }
}
using System;
using System.Threading.Tasks;
using AutoFileManager.Data.Providers;
using AutoFileManager.Data.Providers.Interfaces;
using AutoFileManager.Data.Repositories;
using AutoFileManager.Data.Repositories.Interfaces;
using AutoFileManager.Services;
using AutoFileManager.Services.Interfaces;
using AutoFileManager.Settings;
using AutoFileManager.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AutoFileManeger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuild(args).Build();
            await host.RunAsync();
        }

        static IHostBuilder CreateHostBuild(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddSingleton<IConfigurationsHelper, ConfigurationsHelper>();
            services.AddSingleton<IJsonContext, JsonContext>();
            services.AddTransient<IContentTypeRepository, ContentTypeRepository>();
            services.AddTransient<IInformationTypeRepository, InformationTypeRepository>();
            services.AddTransient<IContentInformationService, ContentInformationService>();
            services.AddTransient<IDirectoryService, DirectoryService>();
            services.AddTransient<IFileHandleService, FileHandleService>();
            services.AddTransient<IFileRelayService, FileRelayService>();
            services.AddTransient<IFileService, FileService>();
            services.AddHostedService<FileRelayWorker>();
        });
    }
}

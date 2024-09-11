
using AutoFileManager.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFileManager.Workers
{
    public class FileRelayWorker : BackgroundService
    {
        private readonly IFileRelayService fileRelayService;

        public FileRelayWorker(
            IFileRelayService fileRelayService
        )
        {
            this.fileRelayService = fileRelayService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await fileRelayService.ProcessFile();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
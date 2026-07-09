using ContextQL.Sync.Clients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContextQL.Sync.Services
{
    public class NotionSyncService : BackgroundService
    {
        private readonly ILogger<NotionSyncService> _logger;
        private readonly INotionClient _notionClient;

        public NotionSyncService(ILogger<NotionSyncService> logger, INotionClient notionClient)
        {
            _logger = logger;
            _notionClient = notionClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notion sync service starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Notion sync heartbeat.");
                await _notionClient.SyncWorkspaceAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogInformation("Notion sync service stopping.");
        }
    }
}

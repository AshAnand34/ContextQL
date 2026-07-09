using ContextQL.Sync.Clients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContextQL.Sync.Services
{
    public class GitHubSyncService : BackgroundService
    {
        private readonly ILogger<GitHubSyncService> _logger;
        private readonly IGitHubClient _gitHubClient;

        public GitHubSyncService(ILogger<GitHubSyncService> logger, IGitHubClient gitHubClient)
        {
            _logger = logger;
            _gitHubClient = gitHubClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("GitHub sync service starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("GitHub sync heartbeat.");
                await _gitHubClient.SyncRepositoriesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogInformation("GitHub sync service stopping.");
        }
    }
}

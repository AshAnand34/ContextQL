using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ContextQL.Sync.Clients
{
    public class GitHubClientAdapter : IGitHubClient
    {
        private readonly ILogger<GitHubClientAdapter> _logger;

        public GitHubClientAdapter(ILogger<GitHubClientAdapter> logger)
        {
            _logger = logger;
        }

        public async Task SyncRepositoriesAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("GitHub sync adapter invoked.");
            await Task.Delay(100, cancellationToken);
            _logger.LogInformation("GitHub sync adapter completed (stub). If you want real sync behavior, implement Octokit integration here.");
        }
    }
}

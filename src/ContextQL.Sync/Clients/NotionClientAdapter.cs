using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ContextQL.Sync.Clients
{
    public class NotionClientAdapter : INotionClient
    {
        private readonly ILogger<NotionClientAdapter> _logger;

        public NotionClientAdapter(ILogger<NotionClientAdapter> logger)
        {
            _logger = logger;
        }

        public async Task SyncWorkspaceAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Notion sync adapter invoked.");
            await Task.Delay(100, cancellationToken);
            _logger.LogInformation("Notion sync adapter completed (stub). If you want real sync behavior, implement Notion.Net integration here.");
        }
    }
}

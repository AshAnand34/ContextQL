using System.Threading;
using System.Threading.Tasks;

namespace ContextQL.Sync.Clients
{
    public interface IGitHubClient
    {
        Task SyncRepositoriesAsync(CancellationToken cancellationToken = default);
    }
}

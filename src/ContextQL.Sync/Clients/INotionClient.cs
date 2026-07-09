using System.Threading;
using System.Threading.Tasks;

namespace ContextQL.Sync.Clients
{
    public interface INotionClient
    {
        Task SyncWorkspaceAsync(CancellationToken cancellationToken = default);
    }
}

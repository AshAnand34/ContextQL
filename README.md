# ContextQL (Phase 1 scaffold)

Minimal scaffold for ContextQL (ASP.NET Core + Hot Chocolate + EF Core + Postgres).

Quick start

1. Use the DotNet CLI to restore packages and build the solution:

```powershell
dotnet restore
dotnet build
```

2. Run Postgres + API via Docker Compose:

```powershell
docker compose up --build
```

3. GraphQL endpoint: `http://localhost:5000/graphql`
5. Run the GraphQL integration tests:

```powershell
dotnet test tests\ContextQL.Tests\ContextQL.Tests.csproj
```

## Sync service behavior

The API project includes background sync host services for GitHub and Notion. These services are registered in production and development runs, but they are disabled when the app is run under the `Testing` environment.

The sync layer currently contains stub adapters and interface wiring:

- `src/ContextQL.Sync/Clients/IGitHubClient.cs`
- `src/ContextQL.Sync/Clients/INotionClient.cs`
- `src/ContextQL.Sync/Clients/GitHubClientAdapter.cs`
- `src/ContextQL.Sync/Clients/NotionClientAdapter.cs`

These adapters are placeholders for future implementation of Octokit and Notion API syncing.
Next steps: add domain entities, implement migrations, flesh out sync services and GraphQL types.

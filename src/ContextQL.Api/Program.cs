using ContextQL.Persistence.Db;
using ContextQL.Sync.Clients;
using ContextQL.Sync.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var isTesting = builder.Environment.IsEnvironment("Testing");

// Configuration: connection string from env or appsettings
var conn = builder.Configuration.GetConnectionString("Default") ?? builder.Configuration["ConnectionStrings:Default"] ?? "Host=localhost;Database=contextql;Username=contextql;Password=Gr4phQL!*";

if (!isTesting)
{
    builder.Services.AddPooledDbContextFactory<ContextQLDbContext>(options =>
        options.UseNpgsql(conn));
}
else
{
    builder.Services.AddDbContextFactory<ContextQLDbContext>(options =>
        options.UseInMemoryDatabase("ContextQLTest"));
}

builder.Services.AddSingleton<IGitHubClient, GitHubClientAdapter>();
builder.Services.AddSingleton<INotionClient, NotionClientAdapter>();

builder.Services.AddGraphQLServer()
    .AddQueryType<ContextQL.Api.GraphQL.Query>()
    .AddType<ContextQL.Api.GraphQL.Types.RepositoryType>()
    .AddType<ContextQL.Api.GraphQL.Types.IssueType>()
    .AddType<ContextQL.Api.GraphQL.Types.DocumentType>()
    .AddType<ContextQL.Api.GraphQL.Types.ProjectType>()
    .AddFiltering()
    .AddSorting()
    .AddProjections();

if (!isTesting)
{
    builder.Services.AddHostedService<GitHubSyncService>();
    builder.Services.AddHostedService<NotionSyncService>();
}

var app = builder.Build();

if (!isTesting)
{
    await WaitForDatabaseAsync(conn, app.Logger);
}

// Ensure migrations applied in dev/startup
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ContextQLDbContext>>();
    using var db = factory.CreateDbContext();
    if (isTesting)
    {
        db.Database.EnsureCreated();
    }
    else
    {
        db.Database.Migrate();
    }
}

app.MapGraphQL();

app.Run();

static async Task WaitForDatabaseAsync(string connectionString, ILogger logger)
{
    var attempt = 0;
    var delay = TimeSpan.FromSeconds(2);
    const int maxAttempts = 12;

    while (true)
    {
        try
        {
            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            await connection.CloseAsync();
            logger.LogInformation("Postgres is available.");
            break;
        }
        catch (Exception ex)
        {
            attempt++;
            if (attempt >= maxAttempts)
            {
                logger.LogError(ex, "Unable to connect to Postgres after {Attempt} attempts.", attempt);
                throw;
            }

            logger.LogWarning(ex, "Postgres is not ready yet (attempt {Attempt}/{MaxAttempts}). Retrying in {Delay}.", attempt, maxAttempts, delay);
            await Task.Delay(delay);
            delay = TimeSpan.FromSeconds(Math.Min(delay.TotalSeconds * 2, 30));
        }
    }
}

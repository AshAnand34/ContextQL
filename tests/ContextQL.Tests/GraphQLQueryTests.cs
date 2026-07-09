using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ContextQL.Api;
using ContextQL.Persistence.Db;
using ContextQL.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContextQL.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(IDbContextFactory<ContextQLDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContextFactory<ContextQLDbContext>(options =>
                    options.UseInMemoryDatabase("ContextQLTest"));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ContextQLDbContext>>().CreateDbContext();
                db.Database.EnsureCreated();

                if (!db.Repositories.Any())
                {
                    var repository = new Repository
                    {
                        Id = Guid.NewGuid(),
                        Name = "TestRepo",
                        Owner = "test-owner",
                        Url = new Uri("https://example.com/testrepo"),
                        Description = "A test repository",
                        IsArchived = false,
                        LastSyncedAt = DateTimeOffset.UtcNow,
                    };

                    db.Repositories.Add(repository);

                    db.Issues.Add(new Issue
                    {
                        Id = Guid.NewGuid(),
                        Number = 1,
                        Title = "Test Issue",
                        Url = "https://example.com/testrepo/issues/1",
                        Body = "A test issue for GraphQL coverage.",
                        State = "open",
                        RepositoryId = repository.Id,
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                        UpdatedAt = DateTimeOffset.UtcNow,
                    });

                    db.Documents.Add(new Document
                    {
                        Id = Guid.NewGuid(),
                        Title = "Test Document",
                        Slug = "test-document",
                        Content = "This is a test document for GraphQL coverage.",
                        SourceUrl = "https://example.com/testrepo/docs/test-document",
                        RepositoryId = repository.Id,
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-2),
                        UpdatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                    });

                    db.Projects.Add(new Project
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test Project",
                        Description = "A test project for GraphQL coverage.",
                        Status = "Active",
                        CreatedAt = DateTimeOffset.UtcNow.AddDays(-3),
                        UpdatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                    });

                    db.SaveChanges();
                }
            });
        }
    }

    public class GraphQLQueryTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GraphQLQueryTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RepositoriesQuery_ReturnsRepositoryWithNestedIssuesAndDocuments()
        {
            var requestBody = new
            {
                query = "query { repositories { id name owner url description isArchived issues { title state } documents { title slug } } }"
            };

            var response = await _client.PostAsJsonAsync("/graphql", requestBody);
            response.EnsureSuccessStatusCode();

            using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var root = document.RootElement;
            Assert.False(root.TryGetProperty("errors", out _));

            var repositories = root.GetProperty("data").GetProperty("repositories");
            Assert.Equal(1, repositories.GetArrayLength());

            var repo = repositories[0];
            Assert.Equal("TestRepo", repo.GetProperty("name").GetString());
            Assert.Equal("test-owner", repo.GetProperty("owner").GetString());
            Assert.Equal("https://example.com/testrepo", repo.GetProperty("url").GetString());
            Assert.Equal("A test repository", repo.GetProperty("description").GetString());
            Assert.False(repo.GetProperty("isArchived").GetBoolean());

            var issues = repo.GetProperty("issues").EnumerateArray().ToArray();
            Assert.Single(issues);
            Assert.Equal("Test Issue", issues[0].GetProperty("title").GetString());
            Assert.Equal("open", issues[0].GetProperty("state").GetString());

            var documents = repo.GetProperty("documents").EnumerateArray().ToArray();
            Assert.Single(documents);
            Assert.Equal("Test Document", documents[0].GetProperty("title").GetString());
            Assert.Equal("test-document", documents[0].GetProperty("slug").GetString());
        }

        [Fact]
        public async Task IssuesQuery_ReturnsSeededIssue()
        {
            var requestBody = new
            {
                query = "query { issues { id number title state repositoryId } }"
            };

            var response = await _client.PostAsJsonAsync("/graphql", requestBody);
            response.EnsureSuccessStatusCode();

            using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var root = document.RootElement;
            Assert.False(root.TryGetProperty("errors", out _));

            var issues = root.GetProperty("data").GetProperty("issues");
            Assert.Equal(1, issues.GetArrayLength());
            var issue = issues[0];
            Assert.Equal("Test Issue", issue.GetProperty("title").GetString());
            Assert.Equal("open", issue.GetProperty("state").GetString());
            Assert.Equal(1, issue.GetProperty("number").GetInt32());
        }

        [Fact]
        public async Task DocumentsQuery_ReturnsSeededDocument()
        {
            var requestBody = new
            {
                query = "query { documents { id title slug content sourceUrl repositoryId } }"
            };

            var response = await _client.PostAsJsonAsync("/graphql", requestBody);
            response.EnsureSuccessStatusCode();

            using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var root = document.RootElement;
            Assert.False(root.TryGetProperty("errors", out _));

            var documents = root.GetProperty("data").GetProperty("documents");
            Assert.Equal(1, documents.GetArrayLength());
            var doc = documents[0];
            Assert.Equal("Test Document", doc.GetProperty("title").GetString());
            Assert.Equal("test-document", doc.GetProperty("slug").GetString());
            Assert.Equal("This is a test document for GraphQL coverage.", doc.GetProperty("content").GetString());
        }

        [Fact]
        public async Task ProjectsQuery_ReturnsSeededProject()
        {
            var requestBody = new
            {
                query = "query { projects { id name description status } }"
            };

            var response = await _client.PostAsJsonAsync("/graphql", requestBody);
            response.EnsureSuccessStatusCode();

            using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var root = document.RootElement;
            Assert.False(root.TryGetProperty("errors", out _));

            var projects = root.GetProperty("data").GetProperty("projects");
            Assert.Equal(1, projects.GetArrayLength());
            var project = projects[0];
            Assert.Equal("Test Project", project.GetProperty("name").GetString());
            Assert.Equal("Active", project.GetProperty("status").GetString());
        }

        [Fact]
        public async Task ProjectsQuery_SmokeTest_ReturnsProjectList()
        {
            var requestBody = new
            {
                query = "query { projects { id name status } }"
            };

            var response = await _client.PostAsJsonAsync("/graphql", requestBody);
            response.EnsureSuccessStatusCode();

            using var document = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            var root = document.RootElement;
            Assert.False(root.TryGetProperty("errors", out _));

            var projects = root.GetProperty("data").GetProperty("projects");
            Assert.NotEmpty(projects.EnumerateArray());
        }
    }
}

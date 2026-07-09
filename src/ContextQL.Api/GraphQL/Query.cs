using ContextQL.Persistence.Db;
using ContextQL.Domain.Entities;
using HotChocolate;
using Microsoft.EntityFrameworkCore;

namespace ContextQL.Api.GraphQL
{
    public class Query
    {
        public IQueryable<Repository> GetRepositories([Service] IDbContextFactory<ContextQLDbContext> dbFactory)
        {
            var db = dbFactory.CreateDbContext();
            return db.Repositories
                .Include(x => x.Issues)
                .Include(x => x.Documents);
        }

        public IQueryable<Issue> GetIssues([Service] IDbContextFactory<ContextQLDbContext> dbFactory)
        {
            var db = dbFactory.CreateDbContext();
            return db.Issues.AsQueryable();
        }

        public IQueryable<Document> GetDocuments([Service] IDbContextFactory<ContextQLDbContext> dbFactory)
        {
            var db = dbFactory.CreateDbContext();
            return db.Documents.AsQueryable();
        }

        public IQueryable<Project> GetProjects([Service] IDbContextFactory<ContextQLDbContext> dbFactory)
        {
            var db = dbFactory.CreateDbContext();
            return db.Projects.AsQueryable();
        }
    }
}

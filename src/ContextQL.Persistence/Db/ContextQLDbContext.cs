using ContextQL.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContextQL.Persistence.Db
{
    public class ContextQLDbContext : DbContext
    {
        public ContextQLDbContext(DbContextOptions<ContextQLDbContext> options) : base(options)
        {
        }

        public DbSet<Repository> Repositories => Set<Repository>();
        public DbSet<Issue> Issues => Set<Issue>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<Project> Projects => Set<Project>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repository>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired();
                b.Property(x => x.Owner).IsRequired();
                b.Property(x => x.Url).IsRequired();
                b.HasMany(x => x.Issues).WithOne(x => x.Repository).HasForeignKey(x => x.RepositoryId);
                b.HasMany(x => x.Documents).WithOne(x => x.Repository).HasForeignKey(x => x.RepositoryId);
            });

            modelBuilder.Entity<Issue>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).IsRequired();
                b.Property(x => x.Url).IsRequired();
                b.Property(x => x.State).IsRequired();
            });

            modelBuilder.Entity<Document>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Title).IsRequired();
                b.Property(x => x.Slug).IsRequired();
                b.Property(x => x.Content).IsRequired();
            });

            modelBuilder.Entity<Project>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired();
                b.Property(x => x.Status).IsRequired();
            });
        }
    }
}

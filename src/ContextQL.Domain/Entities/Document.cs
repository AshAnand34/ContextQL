using System;

namespace ContextQL.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string? SourceUrl { get; set; }
        public Guid? RepositoryId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Repository? Repository { get; set; }
    }
}

using System;

namespace ContextQL.Domain.Entities
{
    public class Issue
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; } = default!;
        public string Url { get; set; } = default!;
        public string? Body { get; set; }
        public string State { get; set; } = default!;
        public Guid RepositoryId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public Repository? Repository { get; set; }
    }
}

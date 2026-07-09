using System;
using System.Collections.Generic;

namespace ContextQL.Domain.Entities
{
    public class Repository
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Owner { get; set; } = default!;
        public Uri Url { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsArchived { get; set; }
        public DateTimeOffset? LastSyncedAt { get; set; }

        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}

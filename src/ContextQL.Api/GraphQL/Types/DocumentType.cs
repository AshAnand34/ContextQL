using ContextQL.Domain.Entities;
using HotChocolate.Types;

namespace ContextQL.Api.GraphQL.Types
{
    public class DocumentType : ObjectType<Document>
    {
        protected override void Configure(IObjectTypeDescriptor<Document> descriptor)
        {
            descriptor.Field(x => x.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(x => x.Title).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Slug).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Content).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.SourceUrl).Type<StringType>();
            descriptor.Field(x => x.RepositoryId).Type<UuidType>();
            descriptor.Field(x => x.CreatedAt).Type<NonNullType<DateTimeType>>();
            descriptor.Field(x => x.UpdatedAt).Type<NonNullType<DateTimeType>>();
        }
    }
}

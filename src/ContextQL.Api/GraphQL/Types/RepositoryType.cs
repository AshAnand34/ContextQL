using ContextQL.Domain.Entities;
using HotChocolate.Types;

namespace ContextQL.Api.GraphQL.Types
{
    public class RepositoryType : ObjectType<Repository>
    {
        protected override void Configure(IObjectTypeDescriptor<Repository> descriptor)
        {
            descriptor.Field(x => x.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Owner).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Url).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.IsArchived).Type<NonNullType<BooleanType>>();
            descriptor.Field(x => x.LastSyncedAt).Type<DateTimeType>();
            descriptor.Field(x => x.Issues).Type<ListType<NonNullType<ObjectType<Issue>>>>();
            descriptor.Field(x => x.Documents).Type<ListType<NonNullType<ObjectType<Document>>>>();
        }
    }
}

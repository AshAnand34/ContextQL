using ContextQL.Domain.Entities;
using HotChocolate.Types;

namespace ContextQL.Api.GraphQL.Types
{
    public class IssueType : ObjectType<Issue>
    {
        protected override void Configure(IObjectTypeDescriptor<Issue> descriptor)
        {
            descriptor.Field(x => x.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(x => x.Number).Type<NonNullType<IntType>>();
            descriptor.Field(x => x.Title).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Url).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Body).Type<StringType>();
            descriptor.Field(x => x.State).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.RepositoryId).Type<NonNullType<UuidType>>();
            descriptor.Field(x => x.CreatedAt).Type<NonNullType<DateTimeType>>();
            descriptor.Field(x => x.UpdatedAt).Type<NonNullType<DateTimeType>>();
        }
    }
}

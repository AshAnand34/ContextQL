using ContextQL.Domain.Entities;
using HotChocolate.Types;

namespace ContextQL.Api.GraphQL.Types
{
    public class ProjectType : ObjectType<Project>
    {
        protected override void Configure(IObjectTypeDescriptor<Project> descriptor)
        {
            descriptor.Field(x => x.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(x => x.Name).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.Description).Type<StringType>();
            descriptor.Field(x => x.Status).Type<NonNullType<StringType>>();
            descriptor.Field(x => x.CreatedAt).Type<NonNullType<DateTimeType>>();
            descriptor.Field(x => x.UpdatedAt).Type<NonNullType<DateTimeType>>();
        }
    }
}

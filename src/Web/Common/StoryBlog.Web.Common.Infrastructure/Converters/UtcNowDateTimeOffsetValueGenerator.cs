using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace StoryBlog.Web.Common.Infrastructure.Converters;

public sealed class UtcNowDateTimeOffsetValueGenerator : ValueGenerator<DateTimeOffset>
{
    public override bool GeneratesTemporaryValues => false;

    public override DateTimeOffset Next(EntityEntry entry) => DateTimeOffset.UtcNow;
}
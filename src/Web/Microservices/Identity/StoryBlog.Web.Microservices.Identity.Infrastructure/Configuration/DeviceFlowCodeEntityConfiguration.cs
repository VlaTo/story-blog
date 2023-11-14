using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Domain;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

internal sealed class DeviceFlowCodeEntityConfiguration : IEntityTypeConfiguration<DeviceFlowCode>
{
    private readonly ConfigurationStoreOptions storeOptions;

    public DeviceFlowCodeEntityConfiguration(ConfigurationStoreOptions storeOptions)
    {
        this.storeOptions = storeOptions;
    }

    public void Configure(EntityTypeBuilder<DeviceFlowCode> builder)
    {
        builder
            .Property(x => x.ClientId)
            .IsRequired(required: true)
            .HasMaxLength(256);
        builder
            .Property(x => x.DeviceCode)
            .IsRequired(required: true)
            .HasMaxLength(256);
        builder
            .Property(x => x.Description)
            .IsRequired(required: false)
            .HasMaxLength(Int16.MaxValue);
        builder
            .Property(x => x.SessionId)
            .IsRequired(required: true)
            .HasMaxLength(128);
        builder
            .Property(x => x.SubjectId)
            .IsRequired(required: true)
            .HasMaxLength(256);
        builder
            .Property(x => x.UserCode)
            .HasMaxLength(256)
            .IsUnicode(unicode: false);
        builder
            .Property(x => x.Data)
            .IsRequired(required: true)
            .HasMaxLength(Int16.MaxValue);
        builder
            .Property(x => x.CreationTime)
            .IsRequired(required: true)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnAdd();
        builder
            .Property(x => x.Expiration)
            .IsRequired(required: true);

        builder
            .HasNoKey();

        builder
            .HasIndex(x => new { x.ClientId, x.DeviceCode });

        builder
            .ToTable(TableNames.DeviceFlowCode, SchemaNames.Identity);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Domain;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

internal sealed class PersistedGrantEntityConfiguration : IEntityTypeConfiguration<Domain.Entities.PersistedGrant>
{
    private readonly ConfigurationStoreOptions storeOptions;

    public PersistedGrantEntityConfiguration(ConfigurationStoreOptions storeOptions)
    {
        this.storeOptions = storeOptions;
    }

    public void Configure(EntityTypeBuilder<Domain.Entities.PersistedGrant> builder)
    {
        builder
            .Property(x => x.Key)
            .IsRequired(required: true)
            .HasMaxLength(256);
        builder
            .Property(x => x.Description)
            .IsRequired(required: false)
            .HasMaxLength(Int16.MaxValue)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.Type)
            .IsRequired(required: true)
            .HasMaxLength(256);
        builder
            .Property(x => x.Data)
            .IsRequired(required: false)
            .HasMaxLength(Int16.MaxValue);
        builder
            .Property(x => x.ClientId)
            .IsRequired(required: true);
        builder
            .Property(x => x.SessionId)
            .IsRequired(required: true);
        builder
            .Property(x => x.SubjectId)
            .IsRequired(required: true);
        builder
            .Property(x => x.CreationTime)
            .IsRequired(required: true);
        builder
            .Property(x => x.Expiration)
            .IsRequired(required: false);
        builder
            .Property(x => x.ConsumedTime)
            .IsRequired(required: false);

        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Key);

        builder
            .ToTable(TableNames.PersistedGrant, SchemaNames.Identity);
    }
}
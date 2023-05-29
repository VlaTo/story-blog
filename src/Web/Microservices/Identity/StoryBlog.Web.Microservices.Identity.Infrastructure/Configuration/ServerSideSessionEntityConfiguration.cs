using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Domain;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

internal sealed class ServerSideSessionEntityConfiguration : IEntityTypeConfiguration<ServerSideSession>
{
    private readonly ConfigurationStoreOptions storeOptions;

    public ServerSideSessionEntityConfiguration(ConfigurationStoreOptions storeOptions)
    {
        this.storeOptions = storeOptions;
    }

    public void Configure(EntityTypeBuilder<ServerSideSession> builder)
    {
        builder
            .Property(x => x.Key)
            .IsRequired(required: true)
            .HasMaxLength(256);
        builder
            .Property(x => x.Scheme)
            .IsRequired(required: true)
            .HasMaxLength(1024);
        builder
            .Property(x => x.DisplayName)
            .IsRequired(required: true)
            .HasMaxLength(Int16.MaxValue)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.SessionId)
            .IsRequired(required: true);
        builder
            .Property(x => x.SubjectId)
            .IsRequired(required: true);
        builder
            .Property(x => x.Data)
            .HasMaxLength(Int16.MaxValue)
            .IsRequired(required: false);
        builder
            .Property(x => x.Created)
            .IsRequired(required: true);
        builder
            .Property(x => x.Expires)
            .IsRequired(required: false);
        builder
            .Property(x => x.Renewed)
            .IsRequired(required: false);

        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Key);

        builder
            .ToTable(TableNames.ServerSideSession, SchemaNames.Identity);
    }
}
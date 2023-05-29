using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Identity.Application.Contexts;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;
using StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Persistence;

public sealed class StoryBlogIdentityDbContext : IdentityDbContext<StoryBlogUser, StoryBlogUserRole, string>, IGenericDbContext, IStoryBlogIdentityDbContext, IConfigurationDbContext, IPersistedGrantDbContext
{
    #region IConfigurationDbContext

    public DbSet<Client> Clients
    {
        get;
        set;
    }

    public DbSet<ClientCorsOrigin> ClientCorsOrigins
    {
        get;
        set;
    }

    public DbSet<IdentityResource> IdentityResources
    {
        get;
        set;
    }

    public DbSet<ApiResource> ApiResources
    {
        get;
        set;
    }

    public DbSet<ApiScope> ApiScopes
    {
        get;
        set;
    }

    public DbSet<IdentityProvider> IdentityProviders
    {
        get;
        set;
    }

    #endregion

    #region IPersistedGrantDbContext

    public DbSet<PersistedGrant> PersistedGrants
    {
        get;
        set;
    }

    public DbSet<DeviceFlowCode> DeviceFlowCodes
    {
        get;
        set;
    }

    public DbSet<Key> Keys
    {
        get;
        set;
    }
    
    #endregion

    public ConfigurationStoreOptions? StoreOptions
    {
        get;
        set;
    }

    public StoryBlogIdentityDbContext()
    {
    }

    public StoryBlogIdentityDbContext(DbContextOptions<StoryBlogIdentityDbContext> options)
        : base(options)
    {
    }

    public bool HasChanges() => ChangeTracker.HasChanges();

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        // do nothing
        return Task.CompletedTask;
    }

    public void Rollback()
    {
        ;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var storeOptions = StoreOptions ?? this.GetService<ConfigurationStoreOptions>();

        if (null == storeOptions)
        {
            throw new ArgumentNullException();
        }

        modelBuilder.ApplyConfiguration(new ApiResourceEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ApiScopeEntityConfiguration(storeOptions));
        modelBuilder.ApplyConfiguration(new ClientEntityConfiguration(storeOptions));
        modelBuilder.ApplyConfiguration(new DeviceFlowCodeEntityConfiguration(storeOptions));
        modelBuilder.ApplyConfiguration(new IdentityResourceEntityConfiguration(storeOptions));
        modelBuilder.ApplyConfiguration(new IdentityProviderEntityConfiguration(storeOptions));
        modelBuilder.ApplyConfiguration(new KeyEntityConfiguration(storeOptions));
        modelBuilder.ApplyConfiguration(new PersistedGrantEntityConfiguration(storeOptions));
        modelBuilder.ApplyConfiguration(new ServerSideSessionEntityConfiguration(storeOptions));

        base.OnModelCreating(modelBuilder);
        
        // User
        modelBuilder.Entity<StoryBlogUser>()
            .Property(x => x.Created)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<StoryBlogUser>()
            .Property(x => x.Modified)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnUpdate();

        // Role
        modelBuilder.Entity<StoryBlogUserRole>()
            .Property(x => x.Description)
            .HasMaxLength(256)
            .IsUnicode(unicode: true);
        modelBuilder.Entity<StoryBlogUserRole>()
            .Property(x => x.Created)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<StoryBlogUser>()
            .Property(x => x.Modified)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnUpdate();

        // Claim
        modelBuilder.Entity<StoryBlogUserRoleClaim>()
            .Property(x => x.Description)
            .HasMaxLength(256)
            .IsUnicode(unicode: true);
        modelBuilder.Entity<StoryBlogUserRoleClaim>()
            .Property(x => x.Group)
            .HasMaxLength(100)
            .IsUnicode(unicode: true);
        modelBuilder.Entity<StoryBlogUserRoleClaim>()
            .Property(x => x.Created)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<StoryBlogUser>()
            .Property(x => x.Modified)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnUpdate();
    }
}
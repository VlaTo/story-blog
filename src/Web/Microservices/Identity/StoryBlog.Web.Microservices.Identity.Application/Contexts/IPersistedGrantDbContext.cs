﻿using Microsoft.EntityFrameworkCore;

namespace StoryBlog.Web.Microservices.Identity.Application.Contexts;

/// <summary>
/// Abstraction for the operational data context.
/// </summary>
/// <seealso cref="System.IDisposable" />
public interface IPersistedGrantDbContext
{
    /// <summary>
    /// Gets or sets the persisted grants.
    /// </summary>
    /// <value>
    /// The persisted grants.
    /// </value>
    DbSet<Domain.Entities.PersistedGrant> PersistedGrants
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the device flow codes.
    /// </summary>
    /// <value>
    /// The device flow codes.
    /// </value>
    DbSet<Domain.Entities.DeviceFlowCode> DeviceFlowCodes
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the keys.
    /// </summary>
    /// <value>
    /// The keys.
    /// </value>
    DbSet<Domain.Entities.Key> Keys
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the user sessions.
    /// </summary>
    /// <value>
    /// The keys.
    /// </value>
    /*DbSet<ServerSideSession> ServerSideSessions
    {
        get;
        set;
    }*/

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    // this is here only because of this: https://github.com/DuendeSoftware/IdentityServer/issues/472
    // and because Microsoft implements the old API explicitly: https://github.com/dotnet/aspnetcore/blob/v6.0.0-rc.2.21480.10/src/Identity/ApiAuthorization.IdentityServer/src/Value/ApiAuthorizationDbContext.cs

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync() => SaveChangesAsync(CancellationToken.None);
}
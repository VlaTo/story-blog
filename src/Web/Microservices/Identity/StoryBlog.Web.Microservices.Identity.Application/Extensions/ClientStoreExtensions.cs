﻿using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Stores;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

public static class ClientStoreExtensions
{
    /// <summary>
    /// Finds the enabled client by identifier.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="clientId">The client identifier.</param>
    /// <returns></returns>
    public static async Task<Client?> FindEnabledClientByIdAsync(this IClientStore store, string? clientId)
    {
        if (false == String.IsNullOrEmpty(clientId))
        {
            var client = await store.FindClientByIdAsync(clientId);

            if (client is { Enabled: true })
            {
                return client;
            }
        }

        return null;
    }
}
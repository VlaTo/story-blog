﻿using Microsoft.IdentityModel.Tokens;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Services.KeyManagement;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Stores;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.Defaults;

/// <summary>
/// The default key material service
/// </summary>
/// <seealso cref="IKeyMaterialService" />
public class DefaultKeyMaterialService : IKeyMaterialService
{
    private readonly IEnumerable<IValidationKeysStore> validationKeysStores;
    private readonly IEnumerable<ISigningCredentialStore> signingCredentialStores;
    private readonly IAutomaticKeyManagerKeyStore keyManagerKeyStore;

    public DefaultKeyMaterialService(
        IEnumerable<IValidationKeysStore> validationKeysStores,
        IEnumerable<ISigningCredentialStore> signingCredentialStores,
        IAutomaticKeyManagerKeyStore keyManagerKeyStore)
    {
        this.validationKeysStores = validationKeysStores;
        this.signingCredentialStores = signingCredentialStores;
        this.keyManagerKeyStore = keyManagerKeyStore;
    }

    public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultKeyMaterialService.GetValidationKeys");

        var keys = new List<SecurityKeyInfo>();

        var automaticSigningKeys = await keyManagerKeyStore.GetValidationKeysAsync();

        if (automaticSigningKeys.Any())
        {
            keys.AddRange(automaticSigningKeys);
        }

        foreach (var store in validationKeysStores)
        {
            var validationKeys = await store.GetValidationKeysAsync();

            if (validationKeys.Any())
            {
                keys.AddRange(validationKeys);
            }
        }

        return keys;
    }

    public async Task<SigningCredentials> GetSigningCredentialsAsync(IEnumerable<string>? allowedAlgorithms = null)
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultKeyMaterialService.GetSigningCredentials");

        if (null == allowedAlgorithms)
        {
            var list = signingCredentialStores.ToList();

            for (var index = 0; index < list.Count; index++)
            {
                var key = await list[index].GetSigningCredentialsAsync();

                if (key != null)
                {
                    return key;
                }
            }

            var automaticKey = await keyManagerKeyStore.GetSigningCredentialsAsync();

            if (automaticKey != null)
            {
                return automaticKey;
            }

            throw new InvalidOperationException($"No signing credential registered.");
        }

        var credentials = await GetAllSigningCredentialsAsync();
        var credential = credentials.FirstOrDefault(signin => allowedAlgorithms.Contains(signin.Algorithm));

        if (credential is null)
        {
            throw new InvalidOperationException($"No signing credential for algorithms ({allowedAlgorithms.ToSpaceSeparatedString()}) registered.");
        }

        return credential;
    }

    public async Task<IEnumerable<SigningCredentials>> GetAllSigningCredentialsAsync()
    {
        using var activity = Tracing.ServiceActivitySource.StartActivity("DefaultKeyMaterialService.GetAllSigningCredentials");

        var credentials = new List<SigningCredentials>();

        foreach (var store in signingCredentialStores)
        {
            var signingKey = await store.GetSigningCredentialsAsync();

            if (null != signingKey)
            {
                credentials.Add(signingKey);
            }
        }

        var automaticSigningKeys = await keyManagerKeyStore.GetAllSigningCredentialsAsync();

        credentials.AddRange(automaticSigningKeys);

        return credentials;
    }
}
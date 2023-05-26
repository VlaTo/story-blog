﻿using IdentityModel;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.Models.Logging;

internal sealed class AuthorizeRequestValidationLog
{
    public string ClientId
    {
        get;
        set;
    }

    public string ClientName
    {
        get;
        set;
    }

    public string RedirectUri
    {
        get;
        set;
    }

    public IEnumerable<string> AllowedRedirectUris
    {
        get;
        set;
    }

    public string SubjectId
    {
        get;
        set;
    }

    public string ResponseType
    {
        get;
        set;
    }

    public string ResponseMode
    {
        get;
        set;
    }

    public string GrantType
    {
        get;
        set;
    }

    public string RequestedScopes
    {
        get;
        set;
    }

    public string State
    {
        get;
        set;
    }

    public string UiLocales
    {
        get;
        set;
    }

    public string Nonce
    {
        get;
        set;
    }

    public IEnumerable<string> AuthenticationContextReferenceClasses
    {
        get;
        set;
    }

    public string DisplayMode
    {
        get;
        set;
    }

    public string PromptMode
    {
        get;
        set;
    }

    public int? MaxAge
    {
        get;
        set;
    }

    public string LoginHint
    {
        get;
        set;
    }

    public string SessionId
    {
        get;
        set;
    }

    public Dictionary<string, string> Raw
    {
        get;
        set;
    }

    public AuthorizeRequestValidationLog(ValidatedAuthorizeRequest request, IEnumerable<string> sensitiveValuesFilter)
    {
        Raw = request.Raw.ToScrubbedDictionary(sensitiveValuesFilter.ToArray());

        if (request.Client != null)
        {
            ClientId = request.Client.ClientId;
            ClientName = request.Client.ClientName;

            AllowedRedirectUris = request.Client.RedirectUris;
        }

        if (request.Subject != null)
        {
            var subjectClaim = request.Subject.FindFirst(JwtClaimTypes.Subject);
            SubjectId = null != subjectClaim ? subjectClaim.Value : "anonymous";
        }

        if (request.AuthenticationContextReferenceClasses.Any())
        {
            AuthenticationContextReferenceClasses = request.AuthenticationContextReferenceClasses;
        }

        RedirectUri = request.RedirectUri;
        ResponseType = request.ResponseType;
        ResponseMode = request.ResponseMode;
        GrantType = request.GrantType;
        RequestedScopes = request.RequestedScopes.ToSpaceSeparatedString();
        State = request.State;
        UiLocales = request.UiLocales;
        Nonce = request.Nonce;

        DisplayMode = request.DisplayMode;
        PromptMode = request.PromptModes.ToSpaceSeparatedString();
        LoginHint = request.LoginHint;
        MaxAge = request.MaxAge;
        SessionId = request.SessionId;
    }

    public override string ToString()
    {
        return LogSerializer.Serialize(this);
    }
}
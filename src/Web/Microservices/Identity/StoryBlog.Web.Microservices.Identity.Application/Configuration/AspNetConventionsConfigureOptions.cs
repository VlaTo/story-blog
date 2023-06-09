﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

internal sealed class AspNetConventionsConfigureOptions : IConfigureOptions<IdentityServerOptions>
{
    public void Configure(IdentityServerOptions options)
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.Authentication.CookieAuthenticationScheme = IdentityConstants.ApplicationScheme;
        options.UserInteraction.ErrorUrl = "/Home";
    }
}
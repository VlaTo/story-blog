using Microsoft.Extensions.Logging;

namespace StoryBlog.Web.Microservices.Identity.Application.Core;

internal static class LoggerEventIds
{
    public static readonly EventId ConfiguringAPIResource = new(1, "ConfiguringAPIResource");
    public static readonly EventId ConfiguringLocalAPIResource = new(2, "ConfiguringLocalAPIResource");
    public static readonly EventId ConfiguringClient = new(3, "ConfiguringClient");
    public static readonly EventId AllowedApplicationNotDefienedForIdentityResource = new(4, "AllowedApplicationNotDefienedForIdentityResource");
    public static readonly EventId AllApplicationsAllowedForIdentityResource = new(5, "AllApplicationsAllowedForIdentityResource");
    public static readonly EventId ApplicationsAllowedForIdentityResource = new(6, "ApplicationsAllowedForIdentityResource");
    public static readonly EventId AllowedApplicationNotDefienedForApiResource = new(7, "AllowedApplicationNotDefienedForApiResource");
    public static readonly EventId AllApplicationsAllowedForApiResource = new(8, "AllApplicationsAllowedForApiResource");
    public static readonly EventId ApplicationsAllowedForApiResource = new(9, "ApplicationsAllowedForApiResource");
    public static readonly EventId DevelopmentKeyLoaded = new(10, "DevelopmentKeyLoaded");
    public static readonly EventId CertificateLoadedFromFile = new(11, "CertificateLoadedFromFile");
    public static readonly EventId CertificateLoadedFromStore = new(12, "CertificateLoadedFromStore");
    public static readonly EventId EndingSessionFailed = new(13, "EndingSessionFailed");
}
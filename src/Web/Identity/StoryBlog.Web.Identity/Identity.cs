﻿using StoryBlog.Web.Common.Identity.Permission;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace StoryBlog.Web.Identity;

public static class Identity
{
    /// <summary>
    /// Creates an anonymous claims identity.
    /// </summary>
    /// <value>
    /// The anonymous.
    /// </value>
    public static ClaimsIdentity Anonymous
    {
        get
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, String.Empty)
            };

            return new ClaimsIdentity(claims);
        }
    }

    /// <summary>
    /// Creates a ClaimsIdentity using the specified authentication type and claims.
    /// </summary>
    /// <param name="authenticationType">Type of the authentication.</param>
    /// <param name="claims">The claims.</param>
    /// <returns></returns>
    public static ClaimsIdentity Create(string authenticationType, params Claim[] claims)
    {
        return new ClaimsIdentity(claims, authenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);
    }

    /// <summary>
    /// Creates a ClaimsIdentity based on information found in an X509 certificate.
    /// </summary>
    /// <param name="certificate">The certificate.</param>
    /// <param name="authenticationType">Type of the authentication.</param>
    /// <param name="includeAllClaims">if set to <c>true</c> [include all claims].</param>
    /// <returns></returns>
    public static ClaimsIdentity CreateFromCertificate(X509Certificate2 certificate, string authenticationType = "X.509", bool includeAllClaims = false)
    {
        var claims = new List<Claim>();
        var issuer = certificate.Issuer;

        claims.Add(new Claim("issuer", issuer));

        var thumbprint = certificate.Thumbprint;
        claims.Add(new Claim(ClaimTypes.Thumbprint, thumbprint, ClaimValueTypes.Base64Binary, issuer));

        var name = certificate.SubjectName.Name;
        if (false == String.IsNullOrEmpty(name))
        {
            claims.Add(new Claim(ClaimTypes.X500DistinguishedName, name, ClaimValueTypes.String, issuer));
        }

        if (includeAllClaims)
        {
            name = certificate.SerialNumber;
            
            if (false == String.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.SerialNumber, name, ClaimValueTypes.String, issuer));
            }

            name = certificate.GetNameInfo(X509NameType.DnsName, false);

            if (false == String.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.Dns, name, ClaimValueTypes.String, issuer));
            }

            name = certificate.GetNameInfo(X509NameType.SimpleName, false);

            if (false == String.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, issuer));
            }

            name = certificate.GetNameInfo(X509NameType.EmailName, false);

            if (false == String.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.Email, name, ClaimValueTypes.String, issuer));
            }

            name = certificate.GetNameInfo(X509NameType.UpnName, false);
            
            if (false == String.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.Upn, name, ClaimValueTypes.String, issuer));
            }

            name = certificate.GetNameInfo(X509NameType.UrlName, false);

            if (false == String.IsNullOrEmpty(name))
            {
                claims.Add(new Claim(ClaimTypes.Uri, name, ClaimValueTypes.String, issuer));
            }
        }

        return new ClaimsIdentity(claims, authenticationType);
    }
}
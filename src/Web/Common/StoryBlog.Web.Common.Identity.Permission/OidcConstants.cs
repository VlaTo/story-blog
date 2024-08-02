namespace StoryBlog.Web.Common.Identity.Permission;

/*public static class OidcConstants
{
    public static class AuthorizeErrors
    {
        // OAuth2 errors
        public const string InvalidRequest = "invalid_request";
        public const string UnauthorizedClient = "unauthorized_client";
        public const string AccessDenied = "access_denied";
        public const string UnsupportedResponseType = "unsupported_response_type";
        public const string InvalidScope = "invalid_scope";
        public const string ServerError = "server_error";
        public const string TemporarilyUnavailable = "temporarily_unavailable";
        public const string UnmetAuthenticationRequirements = "unmet_authentication_requirements";

        // OIDC errors
        public const string InteractionRequired = "interaction_required";
        public const string LoginRequired = "login_required";
        public const string AccountSelectionRequired = "account_selection_required";
        public const string ConsentRequired = "consent_required";
        public const string InvalidRequestUri = "invalid_request_uri";
        public const string InvalidRequestObject = "invalid_request_object";
        public const string RequestNotSupported = "request_not_supported";
        public const string RequestUriNotSupported = "request_uri_not_supported";
        public const string RegistrationNotSupported = "registration_not_supported";

        // resource indicator spec
        public const string InvalidTarget = "invalid_target";
    }

    public static class AuthorizeRequest
    {
        public const string Scope = "scope";
        public const string ResponseType = "response_type";
        public const string ClientId = "client_id";
        public const string RedirectUri = "redirect_uri";
        public const string State = "state";
        public const string ResponseMode = "response_mode";
        public const string Nonce = "nonce";
        public const string Display = "display";
        public const string Prompt = "prompt";
        public const string MaxAge = "max_age";
        public const string UiLocales = "ui_locales";
        public const string IdTokenHint = "id_token_hint";
        public const string LoginHint = "login_hint";
        public const string AcrValues = "acr_values";
        public const string CodeChallenge = "code_challenge";
        public const string CodeChallengeMethod = "code_challenge_method";
        public const string Request = "request";
        public const string RequestUri = "request_uri";
        public const string Resource = "resource";
        public const string DPoPKeyThumbprint = "dpop_jkt";
    }

    public static class CodeChallengeMethods
    {
        public const string Plain = "plain";
        public const string Sha256 = "S256";
    }

    public static class Discovery
    {
        public const string Issuer = "issuer";

        // endpoints
        public const string AuthorizationEndpoint = "authorization_endpoint";
        public const string DeviceAuthorizationEndpoint = "device_authorization_endpoint";
        public const string TokenEndpoint = "token_endpoint";
        public const string UserInfoEndpoint = "userinfo_endpoint";
        public const string IntrospectionEndpoint = "introspection_endpoint";
        public const string RevocationEndpoint = "revocation_endpoint";
        public const string DiscoveryEndpoint = ".well-known/openid-configuration";
        public const string JwksUri = "jwks_uri";
        public const string EndSessionEndpoint = "end_session_endpoint";
        public const string CheckSessionIframe = "check_session_iframe";
        public const string RegistrationEndpoint = "registration_endpoint";
        public const string MtlsEndpointAliases = "mtls_endpoint_aliases";
        public const string PushedAuthorizationRequestEndpoint = "pushed_authorization_request_endpoint";

        // common capabilities
        public const string FrontChannelLogoutSupported = "frontchannel_logout_supported";
        public const string FrontChannelLogoutSessionSupported = "frontchannel_logout_session_supported";
        public const string BackChannelLogoutSupported = "backchannel_logout_supported";
        public const string BackChannelLogoutSessionSupported = "backchannel_logout_session_supported";
        public const string GrantTypesSupported = "grant_types_supported";
        public const string CodeChallengeMethodsSupported = "code_challenge_methods_supported";
        public const string ScopesSupported = "scopes_supported";
        public const string SubjectTypesSupported = "subject_types_supported";
        public const string ResponseModesSupported = "response_modes_supported";
        public const string ResponseTypesSupported = "response_types_supported";
        public const string ClaimsSupported = "claims_supported";
        public const string TokenEndpointAuthenticationMethodsSupported = "token_endpoint_auth_methods_supported";

        // more capabilities
        public const string ClaimsLocalesSupported = "claims_locales_supported";
        public const string ClaimsParameterSupported = "claims_parameter_supported";
        public const string ClaimTypesSupported = "claim_types_supported";
        public const string DisplayValuesSupported = "display_values_supported";
        public const string AcrValuesSupported = "acr_values_supported";
        public const string IdTokenEncryptionAlgorithmsSupported = "id_token_encryption_alg_values_supported";
        public const string IdTokenEncryptionEncValuesSupported = "id_token_encryption_enc_values_supported";
        public const string IdTokenSigningAlgorithmsSupported = "id_token_signing_alg_values_supported";
        public const string OpPolicyUri = "op_policy_uri";
        public const string OpTosUri = "op_tos_uri";
        public const string RequestObjectEncryptionAlgorithmsSupported = "request_object_encryption_alg_values_supported";
        public const string RequestObjectEncryptionEncValuesSupported = "request_object_encryption_enc_values_supported";
        public const string RequestObjectSigningAlgorithmsSupported = "request_object_signing_alg_values_supported";
        public const string RequestParameterSupported = "request_parameter_supported";
        public const string RequestUriParameterSupported = "request_uri_parameter_supported";
        public const string RequireRequestUriRegistration = "require_request_uri_registration";
        public const string ServiceDocumentation = "service_documentation";
        public const string TokenEndpointAuthSigningAlgorithmsSupported = "token_endpoint_auth_signing_alg_values_supported";
        public const string UILocalesSupported = "ui_locales_supported";
        public const string UserInfoEncryptionAlgorithmsSupported = "userinfo_encryption_alg_values_supported";
        public const string UserInfoEncryptionEncValuesSupported = "userinfo_encryption_enc_values_supported";
        public const string UserInfoSigningAlgorithmsSupported = "userinfo_signing_alg_values_supported";
        public const string TlsClientCertificateBoundAccessTokens = "tls_client_certificate_bound_access_tokens";
        public const string AuthorizationResponseIssParameterSupported = "authorization_response_iss_parameter_supported";
        public const string PromptValuesSupported = "prompt_values_supported";

        // CIBA
        public const string BackchannelTokenDeliveryModesSupported = "backchannel_token_delivery_modes_supported";
        public const string BackchannelAuthenticationEndpoint = "backchannel_authentication_endpoint";
        public const string BackchannelAuthenticationRequestSigningAlgValuesSupported = "backchannel_authentication_request_signing_alg_values_supported";
        public const string BackchannelUserCodeParameterSupported = "backchannel_user_code_parameter_supported";

        // DPoP
        public const string DPoPSigningAlgorithmsSupported = "dpop_signing_alg_values_supported";

        // PAR
        public const string RequirePushedAuthorizationRequests = "require_pushed_authorization_requests";
    }

    public static class DisplayModes
    {
        public const string Page = "page";
        public const string Popup = "popup";
        public const string Touch = "touch";
        public const string Wap = "wap";
    }
    
    public static class GrantTypes
    {
        public const string Password = "password";
        public const string AuthorizationCode = "authorization_code";
        public const string ClientCredentials = "client_credentials";
        public const string RefreshToken = "refresh_token";
        public const string Implicit = "implicit";
        public const string Saml2Bearer = "urn:ietf:params:oauth:grant-type:saml2-bearer";
        public const string JwtBearer = "urn:ietf:params:oauth:grant-type:jwt-bearer";
        public const string DeviceCode = "urn:ietf:params:oauth:grant-type:device_code";
        public const string TokenExchange = "urn:ietf:params:oauth:grant-type:token-exchange";
        public const string Ciba = "urn:openid:params:grant-type:ciba";
    }

    public static class HttpHeaders
    {
        public const string DPoP = "DPoP";
        public const string DPoPNonce = "DPoP-Nonce";
    }

    public static class PromptModes
    {
        public const string None = "none";
        public const string Login = "login";
        public const string Consent = "consent";
        public const string SelectAccount = "select_account";
        public const string Create = "create";
    }

    public static class ProtectedResourceErrors
    {
        public const string InvalidToken = "invalid_token";
        public const string ExpiredToken = "expired_token";
        public const string InvalidRequest = "invalid_request";
        public const string InsufficientScope = "insufficient_scope";
    }

    public static class ResponseModes
    {
        public const string FormPost = "form_post";
        public const string Query = "query";
        public const string Fragment = "fragment";
    }

    public static class ResponseTypes
    {
        public const string Code = "code";
        public const string Token = "token";
        public const string IdToken = "id_token";
        public const string IdTokenToken = "id_token token";
        public const string CodeIdToken = "code id_token";
        public const string CodeToken = "code token";
        public const string CodeIdTokenToken = "code id_token token";
    }

    public static class StandardScopes
    {
        /// <summary>REQUIRED. Informs the Authorization Server that the Client is making an OpenID Connect request. If the <c>openid</c> scope value is not present, the behavior is entirely unspecified.</summary>
        public const string OpenId = "openid";

        /// <summary>OPTIONAL. This scope value requests access to the End-User's default profile Claims, which are: <c>name</c>, <c>family_name</c>, <c>given_name</c>, <c>middle_name</c>, <c>nickname</c>, <c>preferred_username</c>, <c>profile</c>, <c>picture</c>, <c>website</c>, <c>gender</c>, <c>birthdate</c>, <c>zoneinfo</c>, <c>locale</c>, and <c>updated_at</c>.</summary>
        public const string Profile = "profile";

        /// <summary>OPTIONAL. This scope value requests access to the <c>email</c> and <c>email_verified</c> Claims.</summary>
        public const string Email = "email";

        /// <summary>OPTIONAL. This scope value requests access to the <c>address</c> Claim.</summary>
        public const string Address = "address";

        /// <summary>OPTIONAL. This scope value requests access to the <c>phone_number</c> and <c>phone_number_verified</c> Claims.</summary>
        public const string Phone = "phone";

        /// <summary>This scope value MUST NOT be used with the OpenID Connect Implicit Client Implementer's Guide 1.0. See the OpenID Connect Basic Client Implementer's Guide 1.0 (http://openid.net/specs/openid-connect-implicit-1_0.html#OpenID.Basic) for its usage in that subset of OpenID Connect.</summary>
        public const string OfflineAccess = "offline_access";
    }
    
    public static class TokenErrors
    {
        public const string InvalidRequest = "invalid_request";
        public const string InvalidClient = "invalid_client";
        public const string InvalidGrant = "invalid_grant";
        public const string UnauthorizedClient = "unauthorized_client";
        public const string UnsupportedGrantType = "unsupported_grant_type";
        public const string UnsupportedResponseType = "unsupported_response_type";
        public const string InvalidScope = "invalid_scope";
        public const string AuthorizationPending = "authorization_pending";
        public const string AccessDenied = "access_denied";
        public const string SlowDown = "slow_down";
        public const string ExpiredToken = "expired_token";
        public const string InvalidTarget = "invalid_target";
        public const string InvalidDPoPProof = "invalid_dpop_proof";
        public const string UseDPoPNonce = "use_dpop_nonce";
    }

    public static class TokenRequest
    {
        public const string GrantType = "grant_type";
        public const string RedirectUri = "redirect_uri";
        public const string ClientId = "client_id";
        public const string ClientSecret = "client_secret";
        public const string ClientAssertion = "client_assertion";
        public const string ClientAssertionType = "client_assertion_type";
        public const string Assertion = "assertion";
        public const string Code = "code";
        public const string RefreshToken = "refresh_token";
        public const string Scope = "scope";
        public const string UserName = "username";
        public const string Password = "password";
        public const string CodeVerifier = "code_verifier";
        public const string TokenType = "token_type";
        public const string Algorithm = "alg";
        public const string Key = "key";
        public const string DeviceCode = "device_code";

        // token exchange
        public const string Resource = "resource";
        public const string Audience = "audience";
        public const string RequestedTokenType = "requested_token_type";
        public const string SubjectToken = "subject_token";
        public const string SubjectTokenType = "subject_token_type";
        public const string ActorToken = "actor_token";
        public const string ActorTokenType = "actor_token_type";

        // ciba
        public const string AuthenticationRequestId = "auth_req_id";
    }

    public static class TokenResponse
    {
        public const string AccessToken = "access_token";
        public const string ExpiresIn = "expires_in";
        public const string TokenType = "token_type";
        public const string RefreshToken = "refresh_token";
        public const string IdentityToken = "id_token";
        public const string Error = "error";
        public const string ErrorDescription = "error_description";
        public const string BearerTokenType = "Bearer";
        public const string DPoPTokenType = "DPoP";
        public const string IssuedTokenType = "issued_token_type";
        public const string Scope = "scope";
    }
}*/
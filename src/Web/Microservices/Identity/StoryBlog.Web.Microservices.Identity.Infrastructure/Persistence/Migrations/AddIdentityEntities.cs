using Microsoft.EntityFrameworkCore.Migrations;
using StoryBlog.Web.Microservices.Identity.Application;
using StoryBlog.Web.Microservices.Identity.Domain;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Persistence.Migrations;

public partial class AddIdentityEntities
{
    private static void SeedIdentityData(MigrationBuilder migrationBuilder)
    {
        #region Api Resources

        migrationBuilder.InsertData(
            table: TableNames.ApiResource,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "Id", "Enabled", "Name", "DisplayName", "Description", "AllowedAccessTokenSigningAlgorithms",
                "ShowInDiscoveryDocument", "RequireResourceIndicator", "Created", "Updated", "LastAccessed",
                "NonEditable"
            },
            values: new object[]
            {
                1, true, "SampleBlog API", "SampleBlog API", "SampleBlog Blogs API", "RS256", true, false,
                new DateTime(2022, 7, 13, 6, 5, 50), null, null, false
            }
        );

        #endregion

        #region Api Resources scopes

        migrationBuilder.InsertData(
            table: TableNames.ApiResourceScope,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "Scope", "ApiResourceId"
            },
            values: new object[]
            {
                "blog", 2
            }
        );

        #endregion

        #region Api Resource Claims

        migrationBuilder.InsertData(
            table: TableNames.ApiResourceClaim,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "ApiResourceId", "Type"
            },
            values: new[]
            {
                new object?[]{ 1, "email" },
                new object?[]{ 1, "email_verified" },
                new object?[]{ 1, "role" },
                new object?[]{ 1, "name" },
                new object?[]{ 1, "preferred_username" },
                new object?[]{ 1, "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor" }
            }
        );

        #endregion

        #region Api scopes

        migrationBuilder.InsertData(
            table: TableNames.ApiScope,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "Enabled", "Name", "DisplayName", "Description", "Required", "Emphasize", "ShowInDiscoveryDocument",
                "Created", "Updated", "LastAccessed", "NonEditable"
            },
            values: new object?[]
            {
                1, "blog", "StoryBlog API scope", "StoryBlog API scope", false, false, true,
                new DateTime(2023, 6, 13, 20, 39, 14), null, null, false
            }
        );

        #endregion

        #region Clients CORS

        migrationBuilder.InsertData(
            table: TableNames.ClientCorsOrigin,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "Origin", "ClientId"
            },
            values: new object[]
            {
                "http://localhost:5030", 1
            }
        );

        #endregion

        #region Clients grant types

        migrationBuilder.InsertData(
            table: TableNames.ClientGrantType,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "GrantType", "ClientId"
            },
            values: new object[][]
            {
                new object[]
                {
                    GrantType.AuthorizationCode, 1
                },
                new object[]
                {
                    GrantType.ClientCredentials, 1
                }
            }
        );

        #endregion

        #region Clients Properties

        migrationBuilder.InsertData(
            table: TableNames.ClientProperty,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "ClientId", "Key", "Value"
            },
            values: new object[]
            {
                1, "Profile", "SPA"
            }
        );

        #endregion

        #region Clients Redirect Uris

        migrationBuilder.InsertData(
            table: TableNames.ClientRedirectUri,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "RedirectUri", "ClientId"
            },
            values: new object[]
            {
                "http://localhost:5035/authentication/login-callback", 1
            }
        );

        #endregion

        #region Clients

        migrationBuilder.InsertData(
            table: TableNames.Client,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "ClientId", "ClientName", "Description", "ClientUri", "Enabled", "FrontChannelLogoutUri",
                "BackChannelLogoutUri", "AllowedIdentityTokenSigningAlgorithms", "ConsentLifetime",
                "UpdateAccessTokenClaimsOnRefresh","ClientClaimsPrefix", "PairWiseSubjectSalt", "UserSsoLifetime",
                "UserCodeType", "RequireClientSecret", "CibaLifetime", "PollingInterval", "Created"
            },
            values: new object[]
            {
                "288849a891664840975fa7992f247947", "Sample Blog", "Sample Blog Client", "http://localhost:5035",
                true, "http://localhost:5035/logout", "http://localhost:5035/logout-callback", "RS256", 150000000,
                false, "sbc_", "k3jr23khk2e4jJKH2hjxbdw_jdTr", 500000000000, "numeric", false, 150000000, 15000000000,
                new DateTime(2023, 05, 26, 11, 43, 39)
            }
        );

        #endregion

        #region Client Scopes

        migrationBuilder.InsertData(
            table: TableNames.ClientScope,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "Scope", "ClientId"
            },
            values: new object[][]
            {
                new object[] { "openid", 1 },
                new object[] { "profile", 1 },
                new object[] { "blog", 1 }
            }
        );

        #endregion

        #region Clients Secrets

        migrationBuilder.InsertData(
            table: TableNames.ClientSecret,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "ClientId", "Description", "Value", "Expiration", "Type", "Created"
            },
            values: new object[]
            {
                1, "StoryBlog basic secret", "l356jtlk3j5l6tk3j6ltk3j5;6tlk345j6l34k",
                new DateTime(2030, 1, 1, 12, 00, 00), SecretTypes.SharedSecret,
                new DateTime(2023, 5, 29, 12, 30, 00)
            }
        );

        #endregion

        #region Identity Resources

        migrationBuilder.InsertData(
            table: TableNames.IdentityResource,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "Id", "Enabled", "Name", "DisplayName", "Description", "Required", "Emphasize", "ShowInDiscoveryDocument",
                "Created", "Updated", "NonEditable"
            },
            values: new object[][]
            {
                new object[]
                {
                    1, 1, "profile", "SimpleBlog OpenID Profile resource", "SimpleBlog OpenID Profile resource", false,
                    false, true, "2022-07-11 09:05:50.7431775", null, false
                },
                new object[]
                {
                    2, 1, "openid", "SimpleBlog OpenID General resource", "SimpleBlog OpenID General resource", false,
                    false, true, "2022-07-11 09:05:50.7731775", null, false
                },
                new object[]
                {
                    3, 1, "email", "SimpleBlog OpenID Email resource", "SimpleBlog OpenID Email resource", false,
                    false, true, "2022-07-11 09:05:50.7931775", null, false
                }
            }
        );

        #endregion

        #region Identity Resources Claims

        migrationBuilder.InsertData(
            table: TableNames.IdentityResourceClaim,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                "IdentityResourceId", "Type"
            },
            values: new object[][]
            {
                new object[]
                {
                    1, "name"
                },
                new object[]
                {
                    1, "given_name"
                },
                new object[]
                {
                    1, "nickname"
                },
                new object[]
                {
                    1, "gender"
                },
                new object[]
                {
                    1, "phone_number"
                },
                new object[]
                {
                    3, "email"
                },
                new object[]
                {
                    3, "email_confirmed"
                }
            }
        );

        #endregion

        #region Keys

        migrationBuilder.InsertData(
            table: TableNames.Keys,
            schema: SchemaNames.Identity,
            columns: new[]
            {
                    "Id", "Version", "Created", "Use", "Algorithm", "IsX509Certificate", "DataProtected", "Data"
            },
            values: new object[]
            {
                    "BD5774334EADCDA65C39FEF2A01AEFB3", 1, "2023-05-26 16:43:42.4669521", "signing", "RS256", false,
                    true,
                    "CfDJ8AoIYmakRP9JrsXlNs9fRz5eom3a0ypQNumXUqONuNrqYQ-tSj7j8uwryY-dQiv7OFYNRmL4Sfq3aL8iPItrTyxEx20"+
                    "amdcUrV5lr969F4xqiqRQsLpXRekg-drsbIndG4nAjnzJ7Wmj6WccpF6fYGs2TawvbCeBrEqp2OLXLqLbN3Vxkbs3bPEfoe"+
                    "tX03OYYRcykdemYKcEfr1WKwR_5EbBdu22jsMcIVRKsbwvMHWSeefTJM7V8L1P24Pw71hFhX86GX9X2l0pwvjYuWr130HII"+
                    "ynXEW1D1ZlhOuV1DftU-Zp2qBHLbHqmjXnYSyceveMSYQt_d3mBpmzmCvsQvqSVqTSR9VciYfb5_A1RskrfUhauoJ9qHcVu"+
                    "PfF8KnXCA_ECZ5MGb0zyoKFh1M02kDBTBJfZ1JUZAF89vPovPJr1fRKPJNjFm6__3vBbz9yF5CZ-GVBPzX-79_8Sy9we04G"+
                    "VWHbQPSI_UcrJEqtjxaE2PmazkshhN83nDTaxxjM7ZbiOzUznEijJfBS2U_OYiOCgOG5u86-N2o-97Q4f5fdBVyXqGBT9H2"+
                    "F2RsQ5ZjV-kWnl-kt2fG8IG3ALSsQBRV0WKk-kIJUUP7q83NFwffyrSapL9dbO1uBSeahKZuHCvaWkeHiJM0mc27OZtuNGf"+
                    "I03SIsK_YSiSdcWC_33PMk0mUdvTucL3G3ZTDdPclbLMKge6b_az06TgfH7LxZajiW32Ja_cA3whoUfHlNGm4UTwgjLwF6n"+
                    "ILmu4zBHw4kWKGMd0JnVGRztLt8v838yyE2OF05Thin7Y4Wuuc8-Mf20yxwz-_HM8TbsKZ-nCbG-FSkLg9uqImkP1izcnFm"+
                    "IO1bL_JrP4U-_ezI4ytYoBk-5FrmZdW1LgL_MKPGA0pnAaXkbpNPMRdpPngMQlXOAVZIBpYSBMi_RlER7EQNsXwwoYsImIa"+
                    "8XoAo-Ltj5_SIUEBZHmNetFnBNsG9iHXoyJwrV-2IxIWTw5YuuZ0qpD-9VG45WOG3_N4oY-bS-fQPL694F-OzAF7B7daaLU"+
                    "hgAC4z9P4IDQh4nQ7gQ5qSplVrAZI6M7g6CVnZ14TNksjrVimNZk3DY4pRFCMBzRYZSv0BnrhKJ2XcRS4b0e1pZLvUImfmh"+
                    "1rdWvMYztusByXP6StMKO8wOvC2DHKHQTsH2aS74OUu3sYpqQWTCuu6hfJXi_uROOevSpwUCESPkQhYvGJIAlefWTnk2yal"+
                    "2ypBa3e_YknzJGh11efTvdFoa6iuKGKK8IZCKUgT5sKsOQ749-0nYJa-2PtCUKDJNXWuX4V5Pg3nJLYr3PXYZFVG5cSnQYr"+
                    "6MDg8iUdBN5fR2YkJ2wRWI9-NJbC1_Qv4eYigoQFZnzHZwy_mqeenO0VHebJY-pbZv0-v3y_qP_3uRD0I9qMVjV4abhJ9Dc"+
                    "t9312oNgsprklxn8vb5ZQpK4Nlw_rwBVTb3M4AX_MjwUB1ne3OcYui3L8QFqINh9ynzqt5wiMYIHF7Am7RHERiU0ZmRigL-"+
                    "8Up_lNsGoBHotiXS9ucQvkezsvCbqTkaJmDMArY34BFud8_Bck9yBWZGjv3Yl93gQDDFc9woGoP2SOEn7_MPi3LsN8uBup6"+
                    "ZJ9jCbWXynnGq072OC2oWRBEfpj2KykCP-KFwQ0Eza3xjF4CKtB5CBA1ZwA2m5HB0hzOY726KHI3LuIUuc5rrZGMubiuiJX"+
                    "aiZiaibsO9TQSbktfxvDQ5cbV3C3vYf8w6bEgL-rbCLgATieIIfdtjR236qBEA5YEIYmt2A3saUjMIbSoabhwMygWTrydt_"+
                    "DEIF3-Wdo5R2O8z6N6erUY4BiY3KoJa_3axsIi31146NxjnUxd1ntNrJHgxlqFVovw39q_JjjVoLFc6gVJkoNihGRiJeuVQ"+
                    "g_WfypMiDXUwP1gq-mPBwHUlPjB1JN9TrDL1JMJBtC3tZzYwDo72cd1FWoenBYXLqAQx7LBAHz0UGNGDovENJ5AqmEDdMij"+
                    "G9OKVP3k7v74TRh43eGAHdbF-vDpRzpJcdFC5wJP976-DJBNB_F6432uv-p-CUUJwPX0lzeth6Zap08PyM4xvfYJksOicFr"+
                    "HyNfIaukPZX8xWqPxFb7tOWBwqcMo8oJsl_Rbgx4ThXFNprlaUvXwNdbGGf99yBivjxWAoI352v5AxufjgoL7uli05BDnhE"+
                    "S1BzeEcVHJ6TlN2a0X2xfi37UBy86BQupYstG95BGcQxC7tKFv_-0EaNCvlDI7kMT82f-2HuCSun6wqHdU4qsfK7LlMiggF"+
                    "N5J_T1v8TkV23nTJpHmoMvz47e9swx1h0NY680ku5ynZwMTmpwsMiG_ujrNpltrVw1yY3w6VKevv0bDjUYhpVzZrduwteea"+
                    "fsLHRyIZHTO6cHRCFaNNdshvYHyLsoH1GlYlzh0-Ur3kQS_TpvVqAFtFMw1Jjz1JBQ3PUE4YTcxgv__YSJ2E4-YF4e7rKB0"+
                    "Lxw8rO2fuY"
            }
        );

        #endregion
        
        #region Users

        migrationBuilder.InsertData(
            table: "AspNetUsers",
            columns: new[]
            {
                    "Id", "IsActive", "Created", "Modified", "UserName", "NormalizedUserName", "Email",
                    "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
                    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled",
                    "AccessFailedCount"
            },
            values: new object[]
            {
                    "1b2696b7-7605-49be-b59c-ab577f6c1cc0", true, "2023-06-08 08:40:34.0273908",
                    "2023-06-08 08:40:34.0348363", "guest@storyblog.net", "GUEST@STORYBLOG.NET", "guest@storyblog.net",
                    "GUEST@STORYBLOG.NET", true,
                    "AQAAAAIAAYagAAAAEP2omrptRd+Y6ldZrmgtyGyiS5lCKnBNVSZFOxOioe798g708HWi818ueedmCfjfKg==",
                    "JUBOLHW6CITA22TQG4QV4FCUDQDYZCKA", "e96c8397-8d72-4ffb-9cf4-246d5fa2f36f", "8-800-444-55-66", true,
                    false, null, true, 0
            }
        );

        migrationBuilder.InsertData(
            table: "AspNetRoles",
            columns: new[]
            {
                    "Id", "Description", "Created", "Modified", "Name", "NormalizedName", "ConcurrencyStamp"
            },
            values: new object[]
            {
                    "9e63463a-67ab-4bb7-a019-c151ea74a05b", "Blog Viewer", "2023-06-08 12:34:00.7029025", null,
                    "Permissions.Blogs.View", "PERMISSIONS.BLOGS.VIEW", "7389d70d-c988-42f3-9712-5d1c607d60a7"
            }
        );

        migrationBuilder.InsertData(
            table: "AspNetUserRoles",
            columns: new[]
            {
                    "UserId", "RoleId"
            },
            values: new object[]
            {
                    "1b2696b7-7605-49be-b59c-ab577f6c1cc0", "9e63463a-67ab-4bb7-a019-c151ea74a05b"
            }
        );

        migrationBuilder.InsertData(
            table: "AspNetUserClaims",
            columns: new[]
            {
                    "UserId", "ClaimType", "ClaimValue"
            },
            values: new object[]
            {
                    "1b2696b7-7605-49be-b59c-ab577f6c1cc0",
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "Guest"
            }
        );

        migrationBuilder.InsertData(
            table: "AspNetRoleClaims",
            columns: new[]
            {
                    "RoleId", "ClaimType", "ClaimValue", "Discriminator", "Description", "Group", "Created",
                    "Modified"
            },
            values: new object[]
            {
                    "9e63463a-67ab-4bb7-a019-c151ea74a05b",
                    "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor", "Viewer",
                    "IdentityRoleClaim<string>", null, null, null, null
            }
        );

        #endregion
    }
}
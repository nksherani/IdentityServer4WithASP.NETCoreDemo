// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer.Data.Entities;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")//,
                //new ApiScope(IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.OpenId),
                //new ApiScope(IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Profile),
                //new ApiScope(IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.StandardScopes.OfflineAccess)

            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AlwaysIncludeUserClaimsInIdToken=true,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1"
                    }
                },
                
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AlwaysIncludeUserClaimsInIdToken=true,

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1"
                    }
                },
                 new Client
                {
                    ClientId = "authapi",
                    ClientSecrets = { new Secret("authapi".Sha256()) },
                    AlwaysIncludeUserClaimsInIdToken=true,

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:5001/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:5001/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1"
                    }
                }
            };

        public static void CreateUsers(Microsoft.Extensions.DependencyInjection.IServiceScope serviceScope)
        {
            var manager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            ApplicationUser user = new ApplicationUser();
            user.UserName = "alice";
            user.FirstName = "alice";
            user.LastName = "alice";
            user.IsEnabled = true;
            user.Email = "alice@wonderland.com";
            var res = manager.CreateAsync(user, "alice").GetAwaiter().GetResult();
            manager.AddClaimAsync(user, new Claim(JwtClaimTypes.Name, "Alice")).GetAwaiter().GetResult();
            manager.AddClaimAsync(user, new Claim(JwtClaimTypes.GivenName, "Alice")).GetAwaiter().GetResult();
            manager.AddClaimAsync(user, new Claim(JwtClaimTypes.FamilyName, "Alice")).GetAwaiter().GetResult();
            manager.AddClaimAsync(user, new Claim(JwtClaimTypes.Email, "Alice@hotmail.com")).GetAwaiter().GetResult();
            manager.AddClaimAsync(user, new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)).GetAwaiter().GetResult();
            //manager.AddClaimAsync(user, new Claim(JwtClaimTypes.Email, "Alice@hotmail.com")).GetAwaiter().GetResult();
            manager.AddClaimAsync(user, new Claim(JwtClaimTypes.Scope, "api1")).GetAwaiter().GetResult();
            manager.AddClaimAsync(user, new Claim(JwtClaimTypes.Role, "admin")).GetAwaiter().GetResult();
            //IdentityUserClaim<string> claim = new IdentityUserClaim<string>();
            //claim.ClaimType = JwtClaimTypes.Name;
            //claim.ClaimValue = "Alice";
            //claim.UserId = user.Id;
            //userContext.UserClaims.Add(claim);
            //userContext.SaveChanges();
        }
    }
}
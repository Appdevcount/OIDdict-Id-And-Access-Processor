using Id_And_Access_Processor;
//using Identity_And_Access_Management.AOpenIdConnectInfra;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity_And_Access_Management.SeedData
{


    public class OpenIdConfigSeeding : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public OpenIdConfigSeeding(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var Context = scope.ServiceProvider.GetRequiredService<OpenIddictContext>();
            //await Context.Database.MigrateAsync();
            await Context.Database.EnsureCreatedAsync(cancellationToken);

            await RegisterApplicationsAsync(scope.ServiceProvider, cancellationToken);
            await RegisterScopesAsync(scope.ServiceProvider, cancellationToken);

            //await EnsureAdministratorRole(scope.ServiceProvider);
            //await EnsureAdministratorUser(scope.ServiceProvider);



        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;




        private static async Task RegisterApplicationsAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {
            var manager = provider.GetRequiredService<IOpenIddictApplicationManager>();
            
            if (await manager.FindByClientIdAsync("spa_client", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "spa_client",
                    // ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                    ConsentType = ConsentTypes.Implicit,
                    DisplayName = "SPA Client Application",
                    PostLogoutRedirectUris =
                        {
                          new Uri("https://localhost:7108"),
                          new Uri("http://localhost:4200")
                        },
                    RedirectUris =
                        {
                          new Uri("https://localhost:7108"),
                          new Uri("http://localhost:4200")
                        },
                    Permissions =
                        {
                          Permissions.Endpoints.Authorization,
                          Permissions.Endpoints.Logout,
                          Permissions.Endpoints.Token,
                          Permissions.Endpoints.Revocation,//added
                          Permissions.GrantTypes.AuthorizationCode,
                          Permissions.GrantTypes.RefreshToken,
                          Permissions.ResponseTypes.Code,
                          Permissions.Scopes.Email,
                          Permissions.Scopes.Profile,
                          Permissions.Scopes.Roles,
                          Permissions.Prefixes.Scope + "server_scope",
                          Permissions.Prefixes.Scope + "api_scope"
                        },
                    Requirements =
                        {
                          Requirements.Features.ProofKeyForCodeExchange
                        }
                },cancellationToken);
            }

            if (await manager.FindByClientIdAsync("api_service", cancellationToken) == null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "api_service",
                    DisplayName = "API Service",
                    ClientSecret = "my-api-secret",
                            Permissions =
                    {
                      Permissions.Endpoints.Introspection
                    }
                };

                await manager.CreateAsync(descriptor, cancellationToken);
            }
            if (await manager.FindByClientIdAsync("mvc") == null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "mvc",
                    ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "MVC client application",
                    PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7286/signout-callback-oidc")
                },
                    RedirectUris =
                {
                    new Uri("https://localhost:7286/signin-oidc")
                },
                    Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    //Permissions.Endpoints.Introspection,//added
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "demo_api"//"api_service"// "demo_api"
                    ,Permissions.Prefixes.Scope + "api_scope"
                },
                    Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
                });
              
            }
            else
            {
                await manager.UpdateAsync(await manager.FindByClientIdAsync("mvc"), new OpenIddictApplicationDescriptor
                {
                    ClientId = "mvc",
                    ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                    Type= "confidential",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "MVC client application",
                    PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7286/signout-callback-oidc")
                },
                    RedirectUris =
                {
                    new Uri("https://localhost:7286/signin-oidc")
                },
                    Permissions =
                {
                    Permissions.Endpoints.Authorization,
                    Permissions.Endpoints.Logout,
                    Permissions.Endpoints.Token,
                    Permissions.GrantTypes.AuthorizationCode,
                    Permissions.GrantTypes.RefreshToken,
                    //Permissions.Endpoints.Introspection,//added
                    Permissions.ResponseTypes.Code,
                    Permissions.Scopes.Email,
                    Permissions.Scopes.Profile,
                    Permissions.Scopes.Roles,
                    Permissions.Prefixes.Scope + "demo_api"//"api_service"// "demo_api"
                    ,Permissions.Prefixes.Scope + "api_scope"
                },
                    Requirements =
                {
                    Requirements.Features.ProofKeyForCodeExchange
                }
                });
            }
            if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    //ClientId = "postman",
                    //ClientSecret = "postman-secret",
                    //DisplayName = "Postman",
                    //Permissions =
                    //{
                    //    OpenIddictConstants.Permissions.Endpoints.Token,
                    //    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,

                    //    OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                    //}

                    ClientId = "postman",
                    ClientSecret = "postman-secret",
                    DisplayName = "Postman",
                    RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
                    Permissions =
                                {
                                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                                    OpenIddictConstants.Permissions.Endpoints.Token,

                                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,

                                    OpenIddictConstants.Permissions.Prefixes.Scope + "api",

                                    OpenIddictConstants.Permissions.ResponseTypes.Code
                                }
                }, cancellationToken);
            }
        }

        private static async Task RegisterScopesAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {
            var manager = provider.GetRequiredService<IOpenIddictScopeManager>();

            if (await manager.FindByNameAsync("server_scope",cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = "server_scope",
                    DisplayName = "Server scope access",
                    Resources =
                    {
                      "server"
                    }
                });
            }

            if (await manager.FindByNameAsync("api_scope", cancellationToken) == null)
            {
                var descriptor = new OpenIddictScopeDescriptor
                {
                    Name = "api_scope",
                    DisplayName = "API Scope access",
                    Resources =
                    {
                      "api_service"
                    }
                };

                await manager.CreateAsync(descriptor,cancellationToken);
            }
        }

        //private static async Task EnsureAdministratorRole(IServiceProvider provider)
        //{
        //    var manager = provider.GetRequiredService<RoleManager<IdentityRole>>();

        //    var role = Roles.ADMINISTRATOR_ROLE;
        //    var roleExists = await manager.RoleExistsAsync(role);
        //    if (!roleExists)
        //    {
        //        var newRole = new IdentityRole(role);
        //        await manager.CreateAsync(newRole);
        //    }
        //}

        //private static async Task EnsureAdministratorUser(IServiceProvider provider)
        //{
        //    var manager = provider.GetRequiredService<UserManager<ApplicationUser>>();

        //    var user = await manager.FindByNameAsync(Constants.ADMIN_MAILADDRESS);
        //    if (user != null) return;

        //    var applicationUser = new ApplicationUser
        //    {
        //        UserName = Constants.ADMIN_MAILADDRESS,
        //        Email = Constants.ADMIN_MAILADDRESS
        //    };

        //    var userResult = await manager.CreateAsync(applicationUser, "Pass123$");
        //    if (!userResult.Succeeded) return;

        //    await manager.SetLockoutEnabledAsync(applicationUser, false);
        //    await manager.AddToRoleAsync(applicationUser, Roles.ADMINISTRATOR_ROLE);
        //}
    }
}

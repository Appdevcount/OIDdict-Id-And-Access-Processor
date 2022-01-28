//using Identity_And_Access_Management.AOpenIdConnectInfra;
using Id_And_Access_Processor;
using Identity_And_Access_Management.Areas.Identity.Data;
using Identity_And_Access_Management.IdentityManagement;
using Microsoft.AspNetCore.Authorization;
//using Identity_And_Access_Management.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
//using Identity_And_Access_Management.IdentityManagement.Infra;
using Microsoft.EntityFrameworkCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity_And_Access_Management
{
    public static class RegisterServicesToContainer
    {
       
        public static IServiceCollection AddOpenIddictRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<,>), typeof(EfRepository<,>));

            services.AddTransient<IScopeRepository, ScopeRepository<OpenIddictContext>>();
            services.AddTransient<IApplicationRepository, ApplicationRepository<OpenIddictContext>>();

            services.AddTransient<IScopeService, ScopeService>();
            services.AddTransient<IApplicationService, ApplicationService>();


            //api service
            services.AddTransient<IScopeApiService, ScopeApiService>();
            services.AddTransient<IApplicationApiService, ApplicationApiService>();


            return services;
        }
        public static IServiceCollection AddOpenIddictServerConfigs(this IServiceCollection services, IConfiguration config,IWebHostEnvironment environment)
        {
            services.AddDbContext<OpenIddictContext>(options =>
            {
                // Configure the context to use an in-memory store.
                options.UseSqlServer(config.GetConnectionString("IdentityAndAccessManagementDB"));
                //options.UseInMemoryDatabase(nameof(DbContext));

                // Register the entity sets needed by OpenIddict.
                options.UseOpenIddict();
            })
             .AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the EF Core stores/models.
                options.UseEntityFrameworkCore()
                .UseDbContext<OpenIddictContext>();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                options.SetIssuer(new Uri("https://localhost:7108/"));

                options
                    .AllowClientCredentialsFlow()
                    .AllowAuthorizationCodeFlow()
                    .RequireProofKeyForCodeExchange()
                    .AllowRefreshTokenFlow();

                options
                        .SetAuthorizationEndpointUris("/connect/authorize")
                        .SetTokenEndpointUris("/connect/token")
                        .SetUserinfoEndpointUris("/connect/userinfo")

                         .SetLogoutEndpointUris("/connect/logout")

                         .SetIntrospectionEndpointUris("/connect/introspect");

                // Encryption and signing of tokens
                if (environment.IsDevelopment())//!Helpers.Constants.IsTestingEnvironment(Environment.EnvironmentName))
                {
                    // Register the signing and encryption credentials.
                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();
                }
                else
                {
                    options.AddEphemeralEncryptionKey()
                           .AddEphemeralSigningKey()
                           .DisableAccessTokenEncryption(); ;
                }
                
                //options
                //.AddEphemeralEncryptionKey()
                //.AddEphemeralSigningKey()
                //.DisableAccessTokenEncryption();

                // Register scopes (permissions)
                // Mark the "email", "profile", "roles" and "demo_api" scopes as supported scopes.
                options.RegisterScopes(
                  Scopes.OpenId,
                  Scopes.Email,
                  Scopes.Profile,
                  Scopes.Roles,
                  "server_scope",
                  "api_scope",
                  "api"
                );


                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options
                       .UseAspNetCore()
                       .EnableTokenEndpointPassthrough()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableUserinfoEndpointPassthrough()

                         .EnableLogoutEndpointPassthrough()
                         .EnableStatusCodePagesIntegration();
                // .DisableTransportSecurityRequirement(); // During development, you can disable the HTTPS requirement.

                // options.DisableAccessTokenEncryption();

                
            })
            // Register the OpenIddict validation components./bcz here its both OpenIdDict server and api server
                .AddValidation(options =>
                {
                    // Note: the validation handler uses OpenID Connect discovery
                    // to retrieve the address of the introspection endpoint.
                    options.SetIssuer("https://localhost:7108/");
                    options.AddAudiences("api_service");

                    // Configure the validation handler to use introspection and register the client
                    // credentials used when communicating with the remote introspection endpoint.
                    options.UseIntrospection()
                           .SetClientId("api_service")
                           .SetClientSecret("my-api-secret");

                    // Register the System.Net.Http integration.
                    options.UseSystemNetHttp();


                    // Import the configuration from the local OpenIddict server instance.
                    //options.UseLocalServer();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                }); 


            return services;
        }

        public static IServiceCollection AddAuthorizationServices(      this IServiceCollection services    )
        {
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(
            //      Policies.ADMIN_POLICY,
            //      policy => policy
            //        .RequireAuthenticatedUser()
            //        .RequireRole(Roles.ADMINISTRATOR_ROLE)
            //    );
            //});

            return services;
        }
        public static IServiceCollection AddIdentityConfigs(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                // Configure the context to use an in-memory store.
                options.UseSqlServer(config.GetConnectionString("IdentityAndAccessManagementDB"));
            });

            //services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultUI()
            .AddDefaultTokenProviders();

            //services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            //services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            return services;
        }

    }
}

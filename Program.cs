using Communicator;
using Communicator.Services;
using EmployeeManagement.Security;
using Id_And_Access_Processor;
using Id_And_Access_Processor.IdentityConfig.Repository;
using Identity_And_Access_Management;
using Identity_And_Access_Management.IdentityManagement;
using Identity_And_Access_Management.SeedData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);


ConfigurationManager config = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

IServiceCollection services = builder.Services;

// Add services to the container.    services.AddControllersWithViews();
services.AddRazorPages();

services.AddControllersWithViews();
services.AddDbContext<OpenIddictContext>(options =>
{
    // Configure the context to use Microsoft SQL Server.
    options.UseSqlServer(config.GetConnectionString("IdAndAccessProcessorConnection"));

    // Register the entity sets needed by OpenIddict.
    // Note: use the generic overload if you need
    // to replace the default OpenIddict entities.
    options.UseOpenIddict();
});

//services.AddDatabaseDeveloperPageExceptionFilter();

services.AddDbContext<AppIdentityDbContext>(options =>
{
    // Configure the context to use an in-memory store.
    options.UseSqlServer(config.GetConnectionString("IdAndAccessProcessorConnection"));
});
services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 5  ;
        options.Password.RequiredUniqueChars = 3;

        options.SignIn.RequireConfirmedEmail = true;
        options.SignIn.RequireConfirmedAccount = true;

        options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    }
    )
    .AddEntityFrameworkStores<AppIdentityDbContext>()
    .AddDefaultTokenProviders()
    .AddTokenProvider<CustomEmailConfirmationTokenProvider
                <ApplicationUser>>("CustomEmailConfirmation")
    .AddDefaultUI();

services.Configure<DataProtectionTokenProviderOptions>(o =>
                       o.TokenLifespan = TimeSpan.FromHours(5));

services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromDays(3));


services.AddAuthentication()
    .AddGoogle(options =>
{
    options.ClientId = "497881634266-1a0q6n4pn3unoajmeoqclrglgjo3hm1v.apps.googleusercontent.com";
    options.ClientSecret = "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";
})
    .AddFacebook(options =>
{
    options.ClientId = "1578738295830880";
    options.ClientSecret = "bca1687d3ebd6464c4213d76c7f6450b";
});
services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

services.Configure<IdentityOptions>(options =>
{
    // Configure Identity to use the same JWT claims as OpenIddict instead
    // of the legacy WS-Federation claims it uses by default (ClaimTypes),
    // which saves you from doing the mapping in your authorization controller.
    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = Claims.Role;
    options.ClaimsIdentity.EmailClaimType = Claims.Email;

    // Note: to require account confirmation before login,
    // register an email sender service (IEmailSender) and
    // set options.SignIn.RequireConfirmedAccount to true.
    //
    // For more information, visit https://aka.ms/aspaccountconf.
    options.SignIn.RequireConfirmedAccount = false;

    //options.SignIn.RequireConfirmedEmail = true;
});

services.AddScoped<IIdentityRepository, IdentityRepository>();

// OpenIddict offers native integration with Quartz.NET to perform scheduled tasks
// (like pruning orphaned authorizations/tokens from the database) at regular intervals.
services.AddQuartz(options =>
{
    options.UseMicrosoftDependencyInjectionJobFactory();
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

// Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

services.AddOpenIddict()

    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        // Configure OpenIddict to use the Entity Framework Core stores and models.
        // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
        options.UseEntityFrameworkCore()
       .UseDbContext<OpenIddictContext>();

        // Enable Quartz.NET integration.
        options.UseQuartz();
    })

    // Register the OpenIddict server components.
    .AddServer(options =>
    {

        // Enable the authorization, device, logout, token, userinfo and verification endpoints.
        options.SetAuthorizationEndpointUris("/connect/authorize")
                   //.SetDeviceEndpointUris("/connect/device")
                   .SetLogoutEndpointUris("/connect/logout")
                   .SetIntrospectionEndpointUris("/connect/introspect")
                   .SetTokenEndpointUris("/connect/token")
                   .SetUserinfoEndpointUris("/connect/userinfo")
                   .SetVerificationEndpointUris("/connect/verify")
                   .SetRevocationEndpointUris("/connect/revoke");//sdded

        // Mark the "email", "profile" and "roles" scopes as supported scopes.
        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);


        // Note: this sample uses the code, device code, password and refresh token flows, but you
        // can enable the other flows if you need to support implicit or client credentials.
        options.AllowAuthorizationCodeFlow()
               //.AllowDeviceCodeFlow()
               .AllowHybridFlow()
               .AllowRefreshTokenFlow();

       

        // Register the signing and encryption credentials.
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        // Force client applications to use Proof Key for Code Exchange (PKCE).
        options.RequireProofKeyForCodeExchange();
              

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options.UseAspNetCore()
               .EnableStatusCodePagesIntegration()
               .EnableAuthorizationEndpointPassthrough()
               .EnableLogoutEndpointPassthrough()
               .EnableTokenEndpointPassthrough()
               .EnableUserinfoEndpointPassthrough()
               //.EnableVerificationEndpointPassthrough()
               //.DisableTransportSecurityRequirement() // During development, you can disable the HTTPS requirement.
               ;

        // Note: when issuing access tokens used by third-party APIs
        // you don't own, you can disable access token encryption:
        //The JWT access tokens produced by the latest OpenIddict 3.0 nightly builds are now encrypted, to match the behavior we had in 1.x and 2.x. JWE is not something jwt.io supports, so it's not surprising if you get an invalid error there.
        //https://documentation.openiddict.com/configuration/token-formats.html
        options.DisableAccessTokenEncryption();


        options.SetAccessTokenLifetime(TimeSpan.FromHours(10));
        options.SetRefreshTokenLifetime(TimeSpan.FromHours(20));
        options.SetIdentityTokenLifetime(TimeSpan.FromHours(10));
        options.SetAuthorizationCodeLifetime(TimeSpan.FromHours(20));
        

        // Note: if you don't want to specify a client_id when sending
        // a token or revocation request, uncomment the following line:
        //
        // options.AcceptAnonymousClients();

        // Note: if you want to process authorization and token requests
        // that specify non-registered scopes, uncomment the following line:
        //
        // options.DisableScopeValidation();

        // Note: if you don't want to use permissions, you can disable
        // permission enforcement by uncommenting the following lines:
        //
        // options.IgnoreEndpointPermissions()
        //        .IgnoreGrantTypePermissions()
        //        .IgnoreResponseTypePermissions()
        //        .IgnoreScopePermissions();
    })

    // Register the OpenIddict validation components.
    .AddValidation(options =>
    {
        // Configure the audience accepted by this resource server.
        // The value MUST match the audience associated with the
        // "demo_api" scope, which is used by ResourceController.
        //options.AddAudiences("rs_dataEventRecordsApi");

        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();

        // For applications that need immediate access token or authorization
        // revocation, the database entry of the received tokens and their
        // associated authorizations can be validated for each API call.
        // Enabling these options may have a negative impact on performance.
        //
        // options.EnableAuthorizationEntryValidation();
        // options.EnableTokenEntryValidation();
    });

//To acess openiddict repository services for db store data access
services.AddOpenIddictRepositoryServices();

// Register the worker responsible of seeding the database.
// Note: in a real world application, this step should be part of a setup script.
services.AddHostedService<OpenIdConfigSeeding>();
services.AddHostedService<UserDetailsSeeding>();

////To be implemented in Client app side with policies
//services.AddSingleton<IAuthorizationHandler, AccessHandler>();
//services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
//services.AddSingleton<DataProtectionPurposeStrings>();
//services.AddMvc(options =>
//{
//    var policy = new AuthorizationPolicyBuilder()
//                    .RequireAuthenticatedUser()
//                    .Build();
//    options.Filters.Add(new AuthorizeFilter(policy));
//}).AddXmlSerializerFormatters();
//services.AddAuthorization(options =>
//{
//    options.AddPolicy("DeleteRolePolicy",
//        policy => policy.RequireClaim("Delete Role"));

//    options.AddPolicy("EditRolePolicy",
//        policy => policy.AddRequirements(new ManageRolesAndClaimsRequirement()));

//    options.AddPolicy("AdminRolePolicy",
//        policy => policy.RequireRole("Admin"));
//});

//Install - Package MailKit
//  Install-Package MimeKit
services.Configure<MailSettings>(config.GetSection("MailSettings"));
services.AddTransient<IMailService, MailService>();

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
}
else
{
    app.UseStatusCodePagesWithReExecute("~/error");
    //app.UseExceptionHandler("~/error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}
app.UseCors(builder =>
{
    //builder.WithOrigins("https://localhost:7286/", "https://localhost:4200/");
    //builder.WithMethods("GET");
    //builder.WithMethods("POST");
    builder.WithHeaders("Authorization");
    ////.AllowCredentials()
    ////.SetIsOriginAllowedToAllowWildcardSubdomains()
    builder.AllowAnyOrigin();
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapDefaultControllerRoute();
    endpoints.MapRazorPages();
});

app.Run();
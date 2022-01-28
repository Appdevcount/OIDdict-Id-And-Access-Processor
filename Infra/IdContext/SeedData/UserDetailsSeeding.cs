//using Identity_And_Access_Management.AOpenIdConnectInfra;
using Id_And_Access_Processor;
using Identity_And_Access_Management.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenIddict.Abstractions;

namespace Identity_And_Access_Management.SeedData
{
    public class UserDetailsSeeding : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public UserDetailsSeeding(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await ContextSeed.SeedRolesAsync(userManager, roleManager);
            await ContextSeed.SeedBasicUserAsync(userManager, roleManager);
            await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);

            var jsonData = System.IO.File.ReadAllText(@"Infra\IdContext\SeedData\Claims.json");
            List<ClaimsStore> ClaimsStoreList =
                          JsonConvert.DeserializeObject<List<ClaimsStore>>(
                            jsonData);

            foreach (ClaimsStore cs in ClaimsStoreList)

            {
                if(!context.ClaimsStore.Any(i=>i.ClaimType==cs.ClaimType&& i.ClaimValue == cs.ClaimValue))
                {
                    context.Add(cs);
                    context.SaveChanges();
                }
            }
            }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

       
    }

}

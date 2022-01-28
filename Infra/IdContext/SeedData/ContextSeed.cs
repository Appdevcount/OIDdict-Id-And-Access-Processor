using Id_And_Access_Processor;
using Identity_And_Access_Management.IdentityManagement;
//using Identity_And_Access_Management.IdentityManagement;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity_And_Access_Management.Areas.Identity.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(RoleNames.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleNames.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleNames.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleNames.Basic.ToString()));
        }
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "Siraj",
                LastName = "R",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Test@12345");
                    await userManager.AddToRoleAsync(defaultUser, RoleNames.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RoleNames.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RoleNames.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RoleNames.SuperAdmin.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }

        //public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    await roleManager.CreateAsync(new IdentityRole(RoleNames.SuperAdmin.ToString()));
        //    await roleManager.CreateAsync(new IdentityRole(RoleNames.Admin.ToString()));
        //    await roleManager.CreateAsync(new IdentityRole(RoleNames.Basic.ToString()));
        //}
        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "basicuser@gmail.com",
                Email = "basicuser@gmail.com",
                FirstName="test",
                LastName = "test",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Test@12345");
                    await userManager.AddToRoleAsync(defaultUser, RoleNames.Basic.ToString());
                }
            }
        }

        //public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    //Seed Default User
        //    var defaultUser = new ApplicationUser
        //    {
        //        UserName = "superadmin@gmail.com",
        //        Email = "superadmin@gmail.com",
        //        EmailConfirmed = true,
        //        PhoneNumberConfirmed = true,
        //    };
        //    if (userManager.Users.All(u => u.Id != defaultUser.Id))
        //    {
        //        var user = await userManager.FindByEmailAsync(defaultUser.Email);
        //        if (user == null)
        //        {
        //            await userManager.CreateAsync(defaultUser, "123Pa$$word!");
        //            await userManager.AddToRoleAsync(defaultUser, RoleNames.Basic.ToString());
        //            await userManager.AddToRoleAsync(defaultUser, RoleNames.Admin.ToString());
        //            await userManager.AddToRoleAsync(defaultUser, RoleNames.SuperAdmin.ToString());
        //        }
        //        await roleManager.SeedClaimsForSuperAdmin();
        //    }
        //}

        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            await roleManager.AddPermissionClaim(adminRole, "Products");
        }

        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }

    }
}

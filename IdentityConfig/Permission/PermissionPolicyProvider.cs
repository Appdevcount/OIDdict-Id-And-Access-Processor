using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Identity_And_Access_Management.IdentityManagement
{


    //https://www.zehntec.com/blog/permission-based-authorization-in-asp-net-core/
    //As we add more permissions, we also need to create policies for those permissions.
    //We can automate this process with IAuthorizationPolicyProvider.
    //We will create a custom policy provider that will dynamically create a policy with
    //the appropriate requirement as it's needed during runtime.
    internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // There can only be one policy provider in ASP.NET Core.
            // We only handle permissions related policies, for the rest
            /// we will use the default provider.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        // Dynamically creates a policy with a requirement that contains the permission.
        // The policy name must match the permission that is needed.
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Permissions", StringComparison.OrdinalIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            }

            // Policy is not for permissions, try the default provider.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();
    }
    //internal class PermissionPolicyProvider : IAuthorizationPolicyProvider
    //{
    //    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    //    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    //    {
    //        // There can only be one policy provider in ASP.NET Core.
    //        // We only handle permissions related policies, for the rest
    //        /// we will use the default provider.
    //        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    //    }
    //    //For the search engines: IAuthorizationPolicyProvider AllowAnonymous not working when using this in Razor Pages. Since you cannot tag a handler method with AllowAnonymous you can’t use the Authorize attribute on the class and the AllowAnonymous attribute on the method.

    //    //Changing this:
    //    //public Task GetFallbackPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
    //    //    to this:
    //    //public Task GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

    //    //Fixes the issue and allows classes to be tagged with[AllowAnonymous]


    //    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
        
    //    // Dynamically creates a policy with a requirement that contains the permission.
    //    // The policy name must match the permission that is needed.
    //    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    //    {
    //        if (policyName.StartsWith("Permission", StringComparison.OrdinalIgnoreCase))
    //        {
    //            var policy = new AuthorizationPolicyBuilder();
    //            policy.AddRequirements(new PermissionRequirement(policyName));
    //            return Task.FromResult(policy.Build());
    //        }

    //        // Policy is not for permissions, try the default provider.
    //        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    //    }

     

    //    //public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();
    //}
}
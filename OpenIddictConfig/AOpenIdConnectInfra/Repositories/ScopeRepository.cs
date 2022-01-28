using Id_And_Access_Processor;
//using Identity_And_Access_Management.AOpenIdConnectInfra;
using OpenIddict.EntityFrameworkCore.Models;

namespace Identity_And_Access_Management
{
  public class ScopeRepository<TContext>
    : EfRepository<OpenIddictEntityFrameworkCoreScope, string>, IScopeRepository
    where TContext : OpenIddictContext
    {
    public ScopeRepository(TContext dbContext) : base(dbContext)
    {
    }
  }
}
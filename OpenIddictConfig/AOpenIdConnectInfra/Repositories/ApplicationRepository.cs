using Id_And_Access_Processor;
//using Identity_And_Access_Management.AOpenIdConnectInfra;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace Identity_And_Access_Management
{
  public class ApplicationRepository<TContext>
        : EfRepository<OpenIddictEntityFrameworkCoreApplication, string>, IApplicationRepository
    where TContext : OpenIddictContext
  {
    public ApplicationRepository(TContext dbContext) : base(dbContext)
    {
    }
  }
}
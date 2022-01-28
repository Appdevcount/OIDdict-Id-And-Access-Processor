using OpenIddict.EntityFrameworkCore.Models;
using Identity_And_Access_Management;

namespace Identity_And_Access_Management
{
  public interface IApplicationRepository     : IAsyncRepository<OpenIddictEntityFrameworkCoreApplication, string>
  { 
    

    }
}
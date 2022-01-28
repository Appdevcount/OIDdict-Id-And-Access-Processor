using OpenIddict.EntityFrameworkCore.Models;
using Identity_And_Access_Management;

namespace Identity_And_Access_Management
{
  public sealed class AllApplications : BaseSpecification<OpenIddictEntityFrameworkCoreApplication>
  {
    public AllApplications()
    {
      ApplyOrderBy(x => x.DisplayName);
      ApplyNoTracking();
    }
  }
}

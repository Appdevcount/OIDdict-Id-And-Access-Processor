using OpenIddict.EntityFrameworkCore.Models;
using Identity_And_Access_Management;

namespace Identity_And_Access_Management
{
  public sealed class AllScopes : BaseSpecification<OpenIddictEntityFrameworkCoreScope>
  {
    public AllScopes()
    {
      ApplyOrderBy(x => x.Name);
      ApplyNoTracking();
    }
  }
}

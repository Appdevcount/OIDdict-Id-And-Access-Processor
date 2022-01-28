using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity_And_Access_Management
{
  public interface IApplicationService
  {
    Task<IEnumerable<ApplicationInfo>> GetApplicationsAsync();

    Task<ApplicationInfo> GetAsync(string id);

    Task<string> CreateAsync(ApplicationParam model);

    Task UpdateAsync(ApplicationParam model);

    Task DeleteAsync(string id);
        Task<ApplicationInfo> GetbyClientId(string clientid);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity_And_Access_Management
{
  public interface IScopeService
  {
    Task<IEnumerable<ScopeInfo>> GetScopesAsync();

    Task<ScopeInfo> GetAsync(string id);

    Task<string> CreateAsync(ScopeParam model);

    Task UpdateAsync(ScopeParam model);

    Task DeleteAsync(string id);
  }
}
using System.Collections.Generic;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity_And_Access_Management
{
  public class ApplicationOptionsViewModel
  {
    public List<string> Permissions { get; set; } = new List<string>();
    
    public List<string> Requirements { get; set; } = new List<string>();
    
    public List<string> Types { get; set; } = new List<string>{
      ClientTypes.Public,
      ClientTypes.Confidential
    };
  }
}

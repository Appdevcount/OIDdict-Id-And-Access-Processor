using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Identity_And_Access_Management
{
  public class ScopeViewModel
  {
    public string Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public List<string> Resources { get; set; } = new List<string>();
  }
}

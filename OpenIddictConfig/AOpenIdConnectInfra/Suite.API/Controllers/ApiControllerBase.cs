using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Net.Mime;
using Identity_And_Access_Management;

namespace Identity_And_Access_Management
{
  [Authorize(
    Policies.ADMIN_POLICY,
    AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme
  )]
  [Produces(MediaTypeNames.Application.Json)]
  public class ApiControllerBase : ControllerBase
  {
  }
}
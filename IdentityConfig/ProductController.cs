using Microsoft.AspNetCore.Mvc;

namespace Identity_And_Access_Management.IdentityManagement
{
public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
}
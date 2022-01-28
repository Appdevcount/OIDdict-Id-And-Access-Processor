using Id_And_Access_Processor;
using Id_And_Access_Processor.IdentityConfig.Repository;
using Id_And_Access_Processor.IdentityConfig.ViewModels;
using Identity_And_Access_Management;
using Identity_And_Access_Management.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using PermissionManagement.MVC.Constants;
//using PermissionManagement.MVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity_And_Access_Management.IdentityManagement
{
    [Authorize(Roles = "SuperAdmin")]
    public class ClaimsManagerController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IIdentityRepository _identityRepository { get; }

        public ClaimsManagerController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager,
            IIdentityRepository identityRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _identityRepository = identityRepository;
        }
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                //thisViewModel.Roles = await GetUserClaims(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        public async Task<IActionResult> Roles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        public  IActionResult Claims()
        {
          return View(_identityRepository.GetClaims());
        }
        public IActionResult UpdateClaim(Int64 Id)
        {
            return View(_identityRepository.GetClaim(Id));
        }
        [HttpPost]
        public IActionResult UpdateClaim(ClaimsStore claimsStore)
        {
            if (!ModelState.IsValid)
            {
                return View(claimsStore);
            }
            if (_identityRepository.UpdateClaim(claimsStore))
                return RedirectToAction("Claims");
            else
                return View(claimsStore);
        }
        public IActionResult CreateClaim()
        {
            ClaimsStore claimsStore = new ClaimsStore();
            return View(claimsStore);
        }
        [HttpPost]
        public IActionResult CreateClaim(ClaimsStore claimsStore)
        {
            if (!ModelState.IsValid)
            {
                return View(claimsStore);
            }
            if (_identityRepository.CreateClaim(claimsStore))
                return RedirectToAction("Claims");
            else
                return View(claimsStore);
        }

        private async Task<List<Claim>> GetUserClaims(ApplicationUser user)
        {
            return new List<Claim>(await _userManager.GetClaimsAsync(user));

        }
        private async Task<List<Claim>> GetRoleClaims(IdentityRole role)
        {
            return new List<Claim>(await _roleManager.GetClaimsAsync(role));

        }


        //    public async Task<IActionResult> Index(string userId)
        //{
        //    var viewModel = new List<UserRolesViewModel>();
        //    var user = await _userManager.FindByIdAsync(userId);
        //    foreach (var role in _roleManager.Roles.ToList())
        //    {
        //        var userRolesViewModel = new UserRolesViewModel
        //        {
        //            RoleName = role.Name
        //        };
        //        if (await _userManager.IsInRoleAsync(user, role.Name))
        //        {
        //            userRolesViewModel.Selected = true;
        //        }
        //        else
        //        {
        //            userRolesViewModel.Selected = false;
        //        }
        //        viewModel.Add(userRolesViewModel);
        //    }
        //    var model = new ManageUserRolesViewModel()
        //    {
        //        UserId = userId,
        //        UserRoles = viewModel
        //    };

        //    return View(model);
        //}

        public async Task<IActionResult> Update(string id, ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            var currentUser = await _userManager.GetUserAsync(User);
            await _signInManager.RefreshSignInAsync(currentUser);
            //await Seeds.DefaultUsers.SeedSuperAdminAsync(_userManager, _roleManager);
            return RedirectToAction("Index", new { userId = id });
        }
        public async Task<IActionResult> ManageUsers(string userId)
        {
            ViewBag.userId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ClaimsViewModel>();
            List<Claim> UserClaims= GetUserClaims(user).Result;
            List<ClaimsStore> totalclaims = _identityRepository.GetClaims().ToList();
            foreach (ClaimsStore claim in totalclaims)
            {
                var ClaimsViewModel = new ClaimsViewModel
                {
                    ClaimType =claim.ClaimType,
                    ClaimValue = claim.ClaimValue
                };
                if ( UserClaims.FirstOrDefault(I=>I.Type==claim.ClaimType)!=null)
                {
                    ClaimsViewModel.Selected = true;
                }
                else
                {
                    ClaimsViewModel.Selected = false;
                }
                model.Add(ClaimsViewModel);
            }
           
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManageUsers(List<ClaimsViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing claims");
                return View(model);
            }
            var modelOnlySelected = model.Where(i => i.Selected == true).ToList();
            result = await _userManager.AddClaimsAsync(user, modelOnlySelected.Select(s => new Claim(s.ClaimType,s.ClaimValue )));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected claims to user");
                return View(model);
            }
            return RedirectToAction("Users");
        }
        public async Task<IActionResult> ManageRoles(string roleId)
        {
            ViewBag.userId = roleId;
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = role.Name;
            var model = new List<ClaimsViewModel>();
            List<Claim> RoleClaims = GetRoleClaims(role).Result;
            List<ClaimsStore> totalclaims = _identityRepository.GetClaims().ToList();
            foreach (ClaimsStore claim in totalclaims)
            {
                var ClaimsViewModel = new ClaimsViewModel
                {
                    ClaimType = claim.ClaimType,
                    ClaimValue = claim.ClaimValue
                };
                if (RoleClaims.FirstOrDefault(I => I.Type == claim.ClaimType) != null)
                {
                    ClaimsViewModel.Selected = true;
                }
                else
                {
                    ClaimsViewModel.Selected = false;
                }
                model.Add(ClaimsViewModel);
            }

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ManageRoles(List<ClaimsViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return View();
            }
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove role existing claims");
                    return View(model);
                }
            }
            var modelOnlySelected = model.Where(i => i.Selected == true).Select(s => new Claim(s.ClaimType, s.ClaimValue)).ToList();
            foreach (var claim in modelOnlySelected)
            {
                var result2 = await _roleManager.AddClaimAsync(role,  claim);
                if (!result2.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected claims to role");
                    return View(model);
                }
            }
            return RedirectToAction("Roles");
        }
    }
}
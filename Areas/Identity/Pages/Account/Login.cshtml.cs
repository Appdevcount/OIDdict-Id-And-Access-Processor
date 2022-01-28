// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Id_And_Access_Processor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Web;

namespace Identity_And_Access_Management.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger
            ,UserManager<ApplicationUser> userManager,
            IApplicationService applicationService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            ApplicationService = applicationService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }
        [TempData]
        public string ClientIdImg { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }
        public IApplicationService ApplicationService { get; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [Display(Name = "Email / Username")]
            //[EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        //https://localhost:7236/Identity/Account/Login?ReturnUrl=%2Fconnect%2Fauthorize%3Fclient_id%3Dmvc%26redirect_uri%3Dhttps%253A%252F%252Flocalhost%253A7286%252Fsignin-oidc%26response_type%3Dcode%26scope%3Dopenid%2520profile%2520email%2520roles%26code_challenge%3DJf9i1gM_eA4a90EdWobT1Pm7-BsKBY6ViG7sz2eCd18%26code_challenge_method%3DS256%26response_mode%3Dform_post%26nonce%3D637768266729182683.OTFmYTI2MjktZGJmYy00MTE1LTlmOWEtNjI1N2M5YWM5YmViOGZkMzZhNzEtMGYxNC00YjliLTk4MWYtZWJiYmEwOTk0NzY5%26state%3DCfDJ8DA6YiBrIlFIiW17sGpFHouEbZc9o2BN_7zj4NABkRaNAd4R6_itJ-k1dx7_mlzftdyiiQpAJ847SoFLR59MyXO_i_ZkkbOZ5-VTVr-NaDPffrRgiEEl1nF7JqltQ7oEqPLbzEjdQfLnxdb5x3SdZM2umEgq0iheSIIaUUH5Sltga6SB00itCrleL0vR2WhCvhbVLG1nYe67kHRBwWWx6Cryk9DiicFe93CAByAxAZB1bZko3T2hejrAwDb0IzOfQKexb_AZlbPjtt6z13npmYuikAOF4pKfpZO4GOYl02SChym2OfjSrE6u7S280wHPGIhnE7b3jxiBlpRbiO9cNopp3Qxzp1VrkUgb9D9FUSbKx7ib23Dv2kbgSrh5en3itA%26x-client-SKU%3DID_NETSTANDARD2_0%26x-client-ver%3D6.11.1.0
       

        public async Task OnGetAsync(string returnUrl = null)

        {
            //Uri myUri = new Uri(returnUrl);
            //string client_id = HttpUtility.ParseQueryString(myUri.Query).Get("client_id");
            string client_id = string.IsNullOrEmpty( returnUrl)?"": HttpUtility.ParseQueryString(returnUrl.Split('?').Last()).Get("client_id");
            ClientIdImg = client_id;
            if (!string.IsNullOrEmpty(ClientIdImg))
            {
                ApplicationInfo clientinfo = ApplicationService.GetbyClientId(ClientIdImg).Result;
                ClientIdImg = clientinfo.RedirectUris.FirstOrDefault();
            }


            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;


        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //return RedirectToPage("./Lockout");
            returnUrl ??= Url.Content("~/");
            var userName = Input.Email;
            if (IsValidEmail(Input.Email))
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null)
                {
                    userName = user.UserName;
                }
            }
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

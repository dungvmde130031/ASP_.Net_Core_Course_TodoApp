using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace TodoApp.WebMvc.Controllers
{
    [AllowAnonymous]
    //public class AccountController : Controller
    //{
    //    public IActionResult Login()
    //    {
    //        return View();
    //    }

    //    // Login Action
    //    [AllowAnonymous]
    //    [HttpPost]
    //    public async Task<IActionResult> Login(
    //        [FromForm(Name = "Username")] string username,
    //        [FromForm(Name = "Password")] string password,
    //        [FromQuery(Name = "ReturnUrl")] string returnUrl)
    //    {

    //        // Validate Username & Password
    //        if (username == "Admin" && password == "123")
    //        {
    //            // Setting identification information for current user
    //            // To validate role/right later
    //            var claims = new List<Claim>()
    //            {
    //                new Claim(ClaimTypes.Name, username),
    //                new Claim(ClaimTypes.NameIdentifier, username),
    //                new Claim("Username", username)
    //            };

    //            var claimsIdentity = new ClaimsIdentity(claims,
    //                CookieAuthenticationDefaults.AuthenticationScheme);

    //            await HttpContext.SignInAsync(
    //                CookieAuthenticationDefaults.AuthenticationScheme,
    //                new ClaimsPrincipal(claimsIdentity),
    //                new AuthenticationProperties()
    //                {
    //                    AllowRefresh = true,
    //                    ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(10),
    //                    IssuedUtc = DateTimeOffset.UtcNow,
    //                    IsPersistent = true
    //                });

    //            if (!string.IsNullOrEmpty(returnUrl))
    //            {
    //                return Redirect(returnUrl);
    //            }

    //            return RedirectToAction("Index", "Home");
    //        }

    //        return RedirectToAction("Login");
    //    }

    //    // Logout Action
    //    public async Task<IActionResult> Logout()
    //    {
    //        await HttpContext.SignOutAsync();

    //        return RedirectToAction("Login");
    //    }
    //}

    public class AccountController : Controller
    {

        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
         
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(
            [FromForm(Name = "Username")] string username,
            [FromForm(Name = "Password")] string password,
            [FromQuery(Name = "ReturnUrl")] string? returnUrl)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return Redirect("/");
                }

            }

            ModelState.AddModelError("Login Failed", "Your username or password is not correct!");

            return View();
        }


        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUp(
            [FromForm(Name = "Username")] string username,
            [FromForm(Name = "Password")] string password,
            [FromQuery(Name = "Email")] string email)
        {
            var user = new IdentityUser(username)
            {
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    // Confirm email before login
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // Send email
                    var verifyLink = Url.ActionLink(action: "ComfirmEmail", controller: "Account",
                        values: new { userId = user.Id, code });

                    TempData["verifyLink"] = verifyLink;

                    return View();
                }
                else
                {
                    await _signInManager.SignInAsync(user, false);

                    return Redirect("/");
                }
            }

            foreach(var item in result.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);

            }

            return View();
        }

        // Confirm Email
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var codeDecoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ConfirmEmailAsync(user, codeDecoded);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

                return Redirect("/");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }

            return View("SignUp");
        }

        // Logout
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}


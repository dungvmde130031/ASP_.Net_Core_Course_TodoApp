using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TodoApp.WebMvc.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        // Login Action
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(
            [FromForm(Name = "Username")] string username,
            [FromForm(Name = "Password")] string password,
            [FromQuery(Name = "ReturnUrl")] string returnUrl)
        {

            // Validate Username & Password
            if (username == "Admin" && password == "123")
            {
                // Setting identification information for current user
                // To validate role/right later
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim("Username", username)
                };

                var claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(10),
                        IssuedUtc = DateTimeOffset.UtcNow,
                        IsPersistent = true
                    });

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }

        // Logout Action
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}


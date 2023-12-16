using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PeopleManager.Dto.Requests;
using PeopleManager.Sdk;
using PeopleManager.Ui.Mvc.Models;
using PeopleManager.Ui.Mvc.Stores;
using Vives.Security.Jwt;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class IdentityController(
        IdentityApiService identityApiService,
        TokenStore tokenStore)
        : Controller
    {
        [HttpGet]
        public async Task<IActionResult> SignIn(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;

            await InternalSignOut();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInRequest request, string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await identityApiService.SignInAsync(request);
            if (result.Errors.Any() || string.IsNullOrWhiteSpace(result.Token))
            {
                ModelState.AddModelError("", "Something went wrong");
                return View();
            }

            await InternalSignIn(result.Token);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut(string returnUrl = "/")
        {
            await InternalSignOut();
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Register(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;

            await InternalSignOut();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel, string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var registerRequest = new RegisterRequest
            {
                Username = viewModel.Username,
                Password = viewModel.Password
            };

            var result = await identityApiService.RegisterAsync(registerRequest);
            if (result.Errors.Any() || string.IsNullOrWhiteSpace(result.Token))
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(viewModel);
            }

            await InternalSignIn(result.Token);
            return LocalRedirect(returnUrl);
        }

        private async Task InternalSignOut()
        {
            await HttpContext.SignOutAsync();
            await tokenStore.ClearToken();
        }

        private async Task InternalSignIn(string token)
        {
            await tokenStore.SaveToken(token);
            var claimsPrincipal = JwtSecurityHelper.GetClaimsPrincipal(token, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(claimsPrincipal);
        }

        
    }
}

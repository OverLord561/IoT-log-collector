using IoTWebClient.Models;
using IoTWebClient.Services;
using IoTWebClient.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IoTWebClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly _2FAuthService _2FAuthService;


        public AccountController(UserManager<AppUser> userManager
            , SignInManager<AppUser> signInManager
            , _2FAuthService twoFAuthService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _2FAuthService = twoFAuthService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { Email = model.Email, UserName = model.Email };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // set cookies
                    await _signInManager.SignInAsync(user, false);

                    return new JsonResult(new { StatusCode = StatusCodes.Status201Created, User = user });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = GetModelStateErrors()
                });
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return new JsonResult(
                        new
                        {
                            StatusCode = StatusCodes.Status200OK,
                            User = new AppUser { Email = model.Email, UserName = model.Email, TwoFactorEnabled = false }
                        });
                }

                if (result.RequiresTwoFactor)
                {
                    return new JsonResult(
                        new
                        {
                            StatusCode = StatusCodes.Status100Continue,
                            User = new AppUser { Email = model.Email, UserName = model.Email, TwoFactorEnabled = true }
                        });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect Login or Password");
                }
            }

            return BadRequest(
               new
               {
                   StatusCode = StatusCodes.Status409Conflict,
                   errors = GetModelStateErrors()
               });
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK });

        }

        [HttpPost]
        public async Task<IActionResult> LoginWith2fa([FromBody] LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = GetModelStateErrors()
                });
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = new List<string>() { $"Unable to load user. Check browser cache" }
                });
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                return new JsonResult(
                        new
                        {
                            StatusCode = StatusCodes.Status200OK,
                            User = new AppUser { Email = user.Email, UserName = user.Email, TwoFactorEnabled = false }
                        });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return BadRequest(
                    new
                    {
                        StatusCode = StatusCodes.Status409Conflict,
                        errors = GetModelStateErrors()
                    });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [EnableCors("AllowGoogleLoginProvider")]

        public IActionResult ExternalLogin([FromBody] ExternalLoginViewModel model)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", null);
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(model.Provider, redirectUrl);
            return Challenge(properties, model.Provider);
        }

        [HttpGet]
        [EnableCors("AllowGoogleLoginProvider")]

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return BadRequest(
                     new
                     {
                         StatusCode = StatusCodes.Status409Conflict,
                         errors = SetSpecificError($"Error from external provider: {remoteError}")
                     });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return new JsonResult(
                        new
                        {
                            StatusCode = StatusCodes.Status200OK,
                            //TODO add user info
                        });
            }

            // If the user does not have an account, then ask the user to create an account.
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            return new JsonResult(
                       new
                       {
                           StatusCode = StatusCodes.Status200OK,
                           //TODO add user info
                       });

            //return View("ExternalLogin", new ExternalLoginViewModel { Email = email });

        }

        private IEnumerable<string> GetModelStateErrors()
        {
            return ModelState.Values.SelectMany(x => x.Errors)
                                           .Select(e => e.ErrorMessage);
        }
        private List<string> SetSpecificError(string message)
        {
            return new List<string>(1) { message };
        }

    }
}
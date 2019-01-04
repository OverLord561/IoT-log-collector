using IoTWebClient.Models;
using IoTWebClient.Services;
using IoTWebClient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IoTWebClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly _2FAuthService _2FAuthService;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, _2FAuthService TwoFAuthService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _2FAuthService = TwoFAuthService;
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
                    errors = ModelState.Values.SelectMany(x => x.Errors)
                                            .Select(e => e.ErrorMessage)
                                                .ToList()
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
                   errors = ModelState.Values.SelectMany(x => x.Errors)
                                           .Select(e => e.ErrorMessage)
                                               .ToList()
               });
        }


        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK });

        }

        [HttpGet]
        public async Task<IActionResult> QrCodeURI()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = "User does not exists" // TODO add error to model state
                });

            }

            var model = new EnableAuthenticatorViewModel();
            await _2FAuthService.LoadSharedKeyAndQrCodeUriAsync(user, model);

            return new JsonResult(new
            {
                StatusCode = StatusCodes.Status200OK,
                QrCodeURI = model.AuthenticatorUri,
                SharedKey = model.SharedKey
            });
        }


        [HttpPost]
        public async Task<IActionResult> EnableAuthenticator([FromBody] EnableAuthenticatorViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = ModelState.Values.SelectMany(x => x.Errors)
                                            .Select(e => e.ErrorMessage)
                                                .ToList()
                });
            }

            if (!ModelState.IsValid)
            {
                await _2FAuthService.LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            // Strip spaces and hypens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            try
            {
                var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                    user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

                if (!is2faTokenValid)
                {
                    ModelState.AddModelError("Code", "Verification code is invalid.");
                    await _2FAuthService.LoadSharedKeyAndQrCodeUriAsync(user, model);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            //_logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK, _2faverified = true });

        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = new List<string>() { $"Unable to load user. Check browser cache" }
                });
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            };

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK, _2faData = model });
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
                    errors = ModelState.Values.SelectMany(x => x.Errors)
                                            .Select(e => e.ErrorMessage)
                                                .ToList()
                });
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                //throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
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
                            //User = new AppUser { Email = model.Email, UserName = model.Email, TwoFactorEnabled = false }
                        });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = ModelState.Values.SelectMany(x => x.Errors)
                                            .Select(e => e.ErrorMessage)
                                                .ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = new List<string>() { $"Unable to load user. Check browser cache" }
                });
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = new List<string>() { $"Unexpected error occured disabling 2FA for user with ID '{user.Id}'." }
                });
            }

            return new JsonResult(
                        new
                        {
                            StatusCode = StatusCodes.Status200OK,
                        });
        }
    }
}
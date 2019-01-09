using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IoTWebClient.Models;
using IoTWebClient.Services;
using IoTWebClient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IoTWebClient.Controllers
{
    public class ManageController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly _2FAuthService _2FAuthService;


        public ManageController(UserManager<AppUser> userManager
            , _2FAuthService twoFAuthService)
        {
            _userManager = userManager;
            _2FAuthService = twoFAuthService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQrCodeURI()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = SetSpecificError("User does not exists") // TODO add error to model state
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
                    errors = SetSpecificError($"Unable to load user. Check browser cache")
                });
            }

            if (!ModelState.IsValid)
            {
                await _2FAuthService.LoadSharedKeyAndQrCodeUriAsync(user, model);
                return BadRequest(
                            new
                            {
                                StatusCode = StatusCodes.Status409Conflict,
                                errors = GetModelStateErrors()
                            });
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

                    return BadRequest(
                            new
                            {
                                StatusCode = StatusCodes.Status409Conflict,
                                errors = GetModelStateErrors()
                            });

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
        public async Task<IActionResult> GetTwoFactorAuthenticationData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = SetSpecificError($"Unable to load user. Check browser cache")
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

        public async Task<IActionResult> Disable2fa()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = SetSpecificError($"Unable to load user. Check browser cache")
                });
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                return BadRequest(
                new
                {
                    StatusCode = StatusCodes.Status409Conflict,
                    errors = SetSpecificError($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.")
                });
            }

            return new JsonResult(
                        new
                        {
                            StatusCode = StatusCodes.Status200OK,
                        });
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoTWebClient.Models;
using IoTWebClient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IoTWebClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
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
            return new JsonResult(new
            {
                StatusCode = StatusCodes.Status409Conflict,
                erros = ModelState.Values.SelectMany(x => x.Errors)
                                            .Select(e => e.ErrorMessage)
                                                .ToList()
            });
        }
    }
}
using IoTWebClient.Models;
using IoTWebClient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

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
                            StatusCode = StatusCodes.Status201Created,
                            User = new AppUser { Email = model.Email, UserName = model.Email }
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
        public async Task<IActionResult> LogOff()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();

            return new JsonResult(new { StatusCode = StatusCodes.Status200OK });

        }
    }
}
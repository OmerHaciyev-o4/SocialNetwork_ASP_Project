using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Business.Abstract;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.Entities;
using SocialNetwork.WebUI.Models;

namespace SocialNetwork.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<CustomIdentityUser> _userManager;
        private RoleManager<CustomIdentityRole> _roleManager;
        private SignInManager<CustomIdentityUser> _signInManager;
        private IUserService _userService;

        public AccountController(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult LogOff()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Login");
        }

        public IActionResult Forgot()
        {
            return View();
        }

        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            return Json(claims);
        }
        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });

            return Json(claims);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _signInManager.PasswordSignInAsync(model.Username,
                    model.Password, model.RememberMe, false).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                TempData["noLogErr"] = "Please enter Username or Password.";
            }
            TempData["noLogErr"] = "Please enter Username or Password.";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            var checkUser = _userService.GetByUsername(model.Username);
            if (checkUser != null)
            {
                TempData.Add("regErrMsg", "This username already exists. \nPlease enter other username.");
                return View(model);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (model.ConfirmPassword != model.Password)
                    {
                        TempData.Add("regErrMsg", "Passwords are not the same.\nPlease enter correct password");
                        return View(model);
                    }

                    CustomIdentityUser userIdentity = new CustomIdentityUser
                    { UserName = model.Username, Email = model.Email };

                    IdentityResult result = _userManager.CreateAsync(userIdentity, model.Password).Result;
                    if (result.Succeeded)
                    {
                        if (!_roleManager.RoleExistsAsync("User").Result)
                        {
                            CustomIdentityRole role = new CustomIdentityRole { Name = "User" };

                            IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
                            if (!roleResult.Succeeded)
                            {
                                ModelState.AddModelError("", "We can not add the role");
                                return View(model);
                            }
                        }

                        _userManager.AddToRoleAsync(userIdentity, "User").Wait();

                        User user = new User
                        {
                            FirstName = "",
                            Lastname = "",
                            Username = model.Username, 
                            Password = model.Password, 
                            Email = model.Email
                        };

                        try { _userService.Add(user); }
                        catch (Exception e)
                        {
                            _userManager.DeleteAsync(userIdentity);
                            //TODO: exception delete.
                            TempData.Add($"regErrMsg", $"We encountered an unexpected error. Please try again a little later. _. {e.Message}");
                            return View(model);
                        }

                        TempData["succReg"] = "Your account has been successfully created.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["passErr"] = "Please enter:";
                        return View(model);
                    }
                }

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Forgot(ForgotViewModel model)
        {
            if (ModelState.IsValid)
            {
                CustomIdentityUser user = null;
                foreach (var customIdentityUser in _userManager.Users)
                {
                    if (customIdentityUser.UserName == model.Username)
                    {
                        user = customIdentityUser;
                        break;
                    }
                }

                if (user != null)
                {
                    User userDb = _userService.GetByUsername(user.UserName);

                    if (model.OldPassword != userDb.Password)
                    {
                        TempData["forErrMsg"] = $"Please enter current password.";
                        return View(model);
                    }

                    try
                    {
                        var result = _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).Result;
                        if (result.Succeeded)
                        {
                            TempData["succReg"] = $"The {model.Username} account password has already been updated.";
                            userDb.Password = model.NewPassword;
                            _userService.Update(userDb);
                            return RedirectToAction("Login");
                        }

                        TempData["forErrMsg"] = $"Please enter new password.";
                    }
                    catch (Exception)
                    {
                        TempData["forErrMsg"] = $"Please enter new password.";
                    }
                }
            }
            return View(model);
        }
    }
}
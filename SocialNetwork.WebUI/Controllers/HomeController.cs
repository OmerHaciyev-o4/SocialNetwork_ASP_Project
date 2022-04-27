using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Business.Abstract;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.Entities;
using SocialNetwork.WebUI.Helpers;
using SocialNetwork.WebUI.Models;

namespace SocialNetwork.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static User User { get; set; }
        private UserManager<CustomIdentityUser> _userManager;
        private RoleManager<CustomIdentityRole> _roleManager;
        private SignInManager<CustomIdentityUser> _signInManager;
        private IUserService _userService;
        private IWebHostEnvironment _webHost;


        public HomeController(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager, IUserService userService, IWebHostEnvironment webHost)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
            _webHost = webHost;

            User = _userService.GetAll().FirstOrDefault(u => u.IsLogined == true);
        }



        public IActionResult Index()
        {
            

            return View();
        }
        public IActionResult Badges()
        {
            return View();
        }
        public IActionResult Stories()
        {
            return View();
        }
        public IActionResult Videos()
        {
            return View();
        }
        public IActionResult Group()
        {
            return View();
        }
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult Settings()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccountInformation()
        {
            var model = new AccountViewModel
            {
                User = HomeController.User
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult AccountInformation(AccountViewModel model, User user)
        {
            User currentUserDb = User;

            if (currentUserDb != null)
            {
                CustomIdentityUser currentUser = null;
                foreach (var customIdentityUser in _userManager.Users)
                {
                    if (customIdentityUser.UserName == currentUserDb.Username)
                    {
                        currentUser = customIdentityUser;
                        break;
                    }
                }

                if (currentUser != null)
                {
                    if (model.User.Email != currentUserDb.Email)
                        currentUser.Email = model.User.Email;

                    _signInManager.UserManager.UpdateAsync(currentUser);

                    if (model.User.FirstName != currentUserDb.FirstName)
                        currentUserDb.FirstName = model.User.FirstName;

                    if (model.User.Lastname != currentUserDb.Lastname)
                        currentUserDb.Lastname = model.User.Lastname;

                    if (model.User.Email != currentUserDb.Email)
                        currentUserDb.Email = model.User.Email;

                    if (model.User.Address != currentUserDb.Address)
                        currentUserDb.Address = model.User.Address;

                    if (model.User.Country != currentUserDb.Country)
                        currentUserDb.Country = model.User.Country;

                    if (model.User.Description != currentUserDb.Description)
                        currentUserDb.Description = model.User.Description;

                    if (model.User.PhoneNumber != currentUserDb.PhoneNumber)
                        currentUserDb.PhoneNumber = model.User.PhoneNumber;

                    if (model.User.TownOrCity != currentUserDb.TownOrCity)
                        currentUserDb.TownOrCity = model.User.TownOrCity;

                    if (model.User.PostCode != currentUserDb.PostCode)
                        currentUserDb.PostCode = model.User.PostCode;

                    if (model.File != null)
                        currentUserDb.ImageUrl = ImageHelper.GetURL(_webHost, model.File, currentUserDb.Id, "front");

                    _userService.Update(currentUserDb);
                    User = currentUserDb;

                    TempData["setSuccMsg"] = "Your information has been successfully updated.";

                    return RedirectToAction("settings");
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.ChangePassword != model.ConfirmChangePassword)
                {
                    TempData.Add("resErrMsg", "Passwords are not the same.\nPlease enter correct password");
                    return View(model);
                }
                CustomIdentityUser user = null;
                foreach (var customIdentityUser in _userManager.Users)
                {
                    if (customIdentityUser.UserName == User.Username)
                    {
                        user = customIdentityUser;
                        break;
                    }
                }

                if (user != null)
                {
                    try
                    {
                        _userManager.ChangePasswordAsync(user, model.CurrentPassowrd, model.ChangePassword);
                        TempData["setSuccMsg"] = "Your password has been changed successfully.";
                        return RedirectToAction("Settings");
                    }
                    catch (Exception)
                    {
                        TempData["passErr"] = "Please enter:";
                        return View(model);
                    }
                }
            }

            return View(model);
        }
        
        public IActionResult Notification()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SearchResult()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchResult(string data)
        {
            return RedirectToAction("SearchResult");
        }
        public IActionResult GroupView()
        {
            return View();
        }
        public IActionResult LiveConversation()
        {
            return View();
        }
        public IActionResult Help()
        {
            return View();
        }
    }
}
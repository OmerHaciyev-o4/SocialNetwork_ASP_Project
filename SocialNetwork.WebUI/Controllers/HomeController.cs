using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Business.Abstract;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.Entities;
using SocialNetwork.WebUI.Models;

namespace SocialNetwork.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserManager<CustomIdentityUser> _userManager;
        private RoleManager<CustomIdentityRole> _roleManager;
        private SignInManager<CustomIdentityUser> _signInManager;
        private IUserService _userService;


        public HomeController(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
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
                User = _userService.GetByUsername("omer.04")
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult AccountInformation(AccountViewModel model, User user)
        {
            CustomIdentityUser currentUser = null;
            foreach (var customIdentityUser in _userManager.Users)
            {
                if (customIdentityUser.UserName == user.Username)
                {
                    currentUser = customIdentityUser;
                    break;
                }
            }

            if (currentUser != null)
            {
                if (model.User.Email != user.Email)
                    currentUser.Email = model.User.Email;

                _signInManager.UserManager.UpdateAsync(currentUser);

                var currentUserDB = _userService.GetByUsername(user.Username);

                if (currentUserDB != null)
                {
                    if (model.User.FirstName != user.FirstName)
                        currentUserDB.FirstName = model.User.FirstName;
                    
                    if (model.User.Lastname != user.Lastname)
                        currentUserDB.Lastname = model.User.Lastname;

                    if (model.User.Email != user.Email)
                        currentUserDB.Email = model.User.Email;

                    if (model.User.Address != user.Address)
                        currentUserDB.Address = model.User.Address;

                    if (model.User.Country != user.Country)
                        currentUserDB.Country = model.User.Country;

                    if (model.User.Description != user.Description)
                        currentUserDB.Description = model.User.Description;

                    if (model.User.PhoneNumber != user.PhoneNumber)
                        currentUserDB.PhoneNumber = model.User.PhoneNumber;
                    
                    if (model.User.TownOrCity != user.TownOrCity)
                        currentUserDB.TownOrCity = model.User.TownOrCity;
                    
                    if (model.User.PostCode != user.PostCode)
                        currentUserDB.PostCode = model.User.PostCode;
                    
                    _userService.Update(currentUserDB);

                    TempData["setSuccMsg"] = "All your information has changed.";
                    RedirectToAction("settings");
                }
            }
            return View(model);
        }


        public IActionResult ResetPassword()
        {
            return View();
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
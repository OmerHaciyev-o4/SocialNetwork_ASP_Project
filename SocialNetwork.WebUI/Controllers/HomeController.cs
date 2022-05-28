using System;
using System.Linq;
using System.Threading;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialNetwork.Business.Abstract;
using SocialNetwork.DataAccess.Abstract;
using SocialNetwork.Social.Entities.Concrete;
using SocialNetwork.WebUI.DTOS;
using SocialNetwork.WebUI.Entities;
using SocialNetwork.WebUI.Helpers;
using SocialNetwork.WebUI.Models;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace SocialNetwork.WebUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public static User User { get; set; }
        public static HttpContext Context { get; set; }
        private UserManager<CustomIdentityUser> _userManager;
        private RoleManager<CustomIdentityRole> _roleManager;
        private SignInManager<CustomIdentityUser> _signInManager;
        private IUserService _userService;
        private IFriendService _friendService;
        private IMapper _mapper;
        private IWebHostEnvironment _webHost;
        private Cloudinary _cloudinary;


        public HomeController(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager, IUserService userService, IWebHostEnvironment webHost, IMapper mapper, IFriendService friendService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
            _webHost = webHost;
            _mapper = mapper;
            _friendService = friendService;

            var userId = _userManager.GetUserId(Context.User);
            var username = _userManager.Users.FirstOrDefault(u => u.Id == userId).UserName;

            User = _userService.GetByUsername(username);

            var myAccount = new Account(apiKey: "392371254347452", apiSecret: "7qwJgIJnuMdrYhtOVgj4TxQ2yNQ", cloud: "social-network-web");
            _cloudinary = new Cloudinary(myAccount);
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
                    {
                        if (!string.IsNullOrEmpty(currentUserDb.ImageUrl))
                        {
                            var result = _cloudinary.DestroyAsync(new DeletionParams(currentUserDb.ImageUrl.Split('/')[7].Split('.')[0])
                            {
                                ResourceType = ResourceType.Image
                            }).Result;
                        }
                        //TODO: Before default time button hiden and if image path default img selected Cloud not upload.
                        string imgPath = ImageHelper.GetURL(_webHost, model.File, currentUserDb.Id, "front");

                        var uploadImage = new ImageUploadParams()
                        {
                            File = new FileDescription(imgPath)
                        };

                        currentUserDb.ImageUrl = _cloudinary.Upload(uploadImage).Url.ToString();
                        System.IO.File.Delete(imgPath);
                    }

                    _userService.Update(currentUserDb);
                    User = currentUserDb;

                    TempData["setSuccMsg"] = "Your information has been successfully updated.";

                    return RedirectToAction("settings");
                }
            }

            return View(model);
        }


        public IActionResult Chat()
        {
            return View();
        }
        

        public IActionResult Group()
        {
            return View();
        }


        public IActionResult GroupView()
        {
            return View();
        }


        public IActionResult Help()
        {
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult LiveConversation()
        {
            return View();
        }


        public IActionResult Notification()
        {
            return View();
        }


        public IActionResult Profile()
        {
            return View();
        }


        [Microsoft.AspNetCore.Mvc.HttpGet]
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


        [HttpGet]
        public IActionResult SearchResult()
        {
            string data = HttpContext.Request.Cookies["SearchModelJsonData"];
            var resultModel = new SearchResultResultViewModel();
            var model = JsonConvert.DeserializeObject<SearchResultViewModel>(HttpContext.Request.Cookies["SearchModelJsonData"]);

            string[] RateSigns = model.RateSigns.Split(',');
            string[] Hashs = model.Hashs.Split(',');


            for (int i = 0; i < RateSigns.Length - 1; i++)
            {
                var user = _userService.GetByUsername(RateSigns[i]);
                if (user != null)
                {
                    var userMapper = _mapper.Map<UserForDetailDto>(user);
                    userMapper.IsFriend = _friendService.CheckFriend(User.Id, userMapper.Id);
                    resultModel.Users.Add(userMapper);
                }
            }

            int o = 0;

            return View(resultModel);
        }
        [HttpPost]
        public IActionResult SearchResult(string searchedData)
        {
            HttpContext.Response.Cookies.Append("SearchModelJsonData", searchedData);
            return RedirectToAction("SearchResult");
        }


        public IActionResult Settings()
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
                return View(loginModel);
            var result = _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, false,false);
            if (!result.Result.Succeeded)
            {
               ModelState.AddModelError("string.Empty", "Invalid login attempt.");
            }         
            return RedirectToAction("Index", "Item");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout(LoginViewModel loginModel)
        {
            _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
               
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel regModel)
        {
            if (!ModelState.IsValid)
                return View(regModel);
          
                var user = new ApplicationUser()
                {
                    UserName = regModel.Email,
                    Email = regModel.Email
                };
                var result = _userManager.CreateAsync(user, regModel.Password).Result;
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("Password", error.Description);
                    }
                    return View(regModel);
                }
                return RedirectToAction("Index", "Home");                       
        }
    }
}

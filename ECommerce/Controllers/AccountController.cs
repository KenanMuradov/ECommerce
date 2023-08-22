using ECommerce.Models;
using ECommerce.Models.ViewModels;
using ECommerce.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using static ECommerce.Models.ViewModels.ResetPasswordViewModel;

namespace ECommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMailService _mailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMailService mailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }


        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public IActionResult ForgotPassword() => View();

        public IActionResult ForgotPasswordConfirmed() => View();

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Fullname = model.FullName,
                    Year = model.Year
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url = Url.Action(nameof(ConfirmEmail), "Account", new { email = user.Email, token = confirmToken }, Request.Scheme);
                    if (url is not null)
                        _mailService.SendConfirmationMessage(user.Email, url, model.FullName);

                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    if (await _userManager.IsEmailConfirmedAsync(user))
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrWhiteSpace(returnUrl))
                                return Redirect(returnUrl);
                            return Redirect("/");
                        }
                        else if (result.IsLockedOut)
                            ModelState.AddModelError("All", "Lockout");
                    }
                    else
                        ModelState.AddModelError("All", "Mail has not been confirmed");
                }
                else
                    ModelState.AddModelError("login", "Incorrect username or password");

            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var url = Url.Action(nameof(ResetPassword), "Account", new { email = user.Email, token }, Request.Scheme);
                    if (url is not null)
                        _mailService.SendResetPasswordMessage(model.Email, url);

                    return RedirectToAction(nameof(ForgotPasswordConfirmed));
                }
            }
            return View(model);
        }

        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description); ;
                        }
                        return View();
                    }
                    return RedirectToAction(nameof(ResetPasswordCompleted));
                }
            }
            return View(model);
        }

        public IActionResult ResetPasswordCompleted() => View();

        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is not null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }
    }
}

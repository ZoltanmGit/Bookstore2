using Bookstore.Persistence.Models;
using Bookstore.Models;
using Bookstore.Persistence.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly IBookStoreService _service;
        private readonly UserManager<BaseUser> _visitorManager;
        private readonly SignInManager<BaseUser> _visitorSingInManager;
        public VisitorsController(UserManager<BaseUser> visitorManager, SignInManager<BaseUser> signInManager, IBookStoreService service)
        {
            _visitorManager = visitorManager;
            _visitorSingInManager = signInManager;
            _service = service;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(VisitorLoginViewModel model, string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _visitorSingInManager.PasswordSignInAsync(model.visitorUserName, model.visitorPassword, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(BooksController.Index), "Books");
                }
                ModelState.AddModelError("", "Login Failed");
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _visitorSingInManager.SignOutAsync();
            return RedirectToAction(nameof(BooksController.Index), "Books");
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(VisitorRegistrationViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                Visitor user = new Visitor
                {
                    UserName = model.visitorUserName,
                    Name = model.visitorName,
                    Address = model.visitorAddress,
                    PhoneNumber = model.visitorPhoneNumber
                };
                var result = await _visitorManager.CreateAsync(user, model.visitorPassword);
                if (result.Succeeded)
                {
                    await _visitorSingInManager.SignInAsync(user, false);
                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError("ErrorKey", "Registration failed");
            }
            return View(model);
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(BooksController.Index), "Books");
            }
        }
        [Authorize]
        [HttpGet]
        public IActionResult CreateLending()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateLending(LendingViewModel model)
        {
            if (ModelState.IsValid && _service.DoesOverLap(model.startDate,model.endDate,model.volumeID) == false)
            {
                Lending newLending = new Lending
                {
                    Volume = _service.GetVolumeById(model.volumeID),
                    StartDate = model.startDate,
                    EndDate = model.endDate,
                };
                bool result = _service.CreateLending(newLending, Int32.Parse(_visitorManager.GetUserId(User)));

                if (result)
                {
                    return RedirectToAction(nameof(Index), "Books");
                }
            }
            if(_service.DoesOverLap(model.startDate, model.endDate, model.volumeID))
            {
                ModelState.AddModelError("Errorkey", "Dates Overlap, choose another interval");
            }
            else
            {
                ModelState.AddModelError("Errorkey", "Undefined Error");
            }
            return View();
        }
    }
}

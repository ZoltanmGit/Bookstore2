using Bookstore.Persistence.DTO;
using Bookstore.Persistence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LibrarianController : ControllerBase
    {
        private readonly SignInManager<BaseUser> _signInManager;
        private readonly UserManager<BaseUser> _userManager;
        public LibrarianController(SignInManager<BaseUser> signInManager, UserManager<BaseUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        // POST: api/Librarian/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.UserName);
            bool bInRoleResult = await _userManager.IsInRoleAsync(user, "Librarian");
            if(bInRoleResult)
            {
                var result = await _signInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            return Unauthorized("Login failed.");
        }
        // POST: api/Librarian/Logout
        [HttpPost]
        [Authorize(Roles ="Librarian")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    } 
}

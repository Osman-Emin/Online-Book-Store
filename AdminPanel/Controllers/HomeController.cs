using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AdminPanel.Infrastructure;
using AdminPanel.Infrastructure.ErrorHandling;
using AdminPanel.Models;
using AdminPanel.Data;
using AdminPanel.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdminPanel.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("/Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpGet("/Team")]
        public IActionResult Team()
        {
            return View();
        }
        [HttpGet("/Preview")]
        public IActionResult Preview()
        {
            return View();
        }

        [HttpGet("/AddToCart")]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            var book = await _context.Books.FindAsync(bookId);
            var client = _context.Clients.Include(o=>o.Carts).FirstOrDefault(c => c.ApplicationUserId == user.Id);
            if (client == null)
            {
                client = new Client() { ApplicationUser = user };
                _context.Clients.Add(client);
            }
            var cart = client.Carts?.FirstOrDefault();
            if (cart == null)
            {
                cart = new Cart() { Client = client };
                _context.Carts.Add(cart);
            }
            _context.CartItems.Add(new CartItem(){BookId = bookId,Price = book.Price,Cart = cart});
            await _context.SaveChangesAsync();
            return Ok("success");
        }

        [ImportModelState]
        [HttpGet("/profile")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return View(new ProfileViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                FullName = user.FullName
            });
        }

        [ExportModelState]
        [HttpPost("/profile")]
        public async Task<IActionResult> UpdateProfile(
            [FromForm]
            ProfileViewModel input)
        {
            if (!ModelState.IsValid)
            { 
                return RedirectToAction(nameof(Profile));
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            
            var email = await _userManager.GetEmailAsync(user);
            if (input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, input.Email);
                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            // Model state might not be valid anymore if we weren't able to change the e-mail address
            // so we need to check for that before proceeding
            if (ModelState.IsValid)
            {
                if (input.FullName != user.FullName)
                {
                    // If we receive an empty string, set a null full name instead
                    user.FullName = string.IsNullOrWhiteSpace(input.FullName) ? null : input.FullName;
                }

                await _userManager.UpdateAsync(user);

                await _signInManager.RefreshSignInAsync(user);

                StatusMessage = "Your profile has been updated";
            }

            return RedirectToAction(nameof(Profile));
        }

        
        
        [HttpGet("/privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet("/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/status-code")]
        public IActionResult StatusCodeHandler(int code)
        {
            ViewBag.StatusCode = code;
            ViewBag.StatusCodeDescription = ReasonPhrases.GetReasonPhrase(code);
            ViewBag.OriginalUrl = null;


            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusCodeReExecuteFeature != null)
            {
                ViewBag.OriginalUrl =
                    statusCodeReExecuteFeature.OriginalPathBase
                    + statusCodeReExecuteFeature.OriginalPath
                    + statusCodeReExecuteFeature.OriginalQueryString;
            }

            if (code == 404)
            {
                return View("Status404");
            }

            return View("Status4xx");
        }
    }
}

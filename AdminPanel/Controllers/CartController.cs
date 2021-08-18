using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Data;
using AdminPanel.Models;
using AdminPanel.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public CartController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)

        {
            _context = context;
            _userManager = userManager;
        }

        // GET
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = PrepareCart( user);
            cart = _context.Carts
                .Include(c=>c.CartItems).ThenInclude(b => b.Book)
                .Include(c=>c.Client.ApplicationUser)
                .First(c=>c.Id ==cart.Id);
            return View(cart);
        }
        [HttpGet("/AddToCart")]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            var book = await _context.Books.FindAsync(bookId);
            var cart = PrepareCart( user);
            _context.CartItems.Add(new CartItem(){BookId = bookId,Price = book.Price,Cart = cart});
            await _context.SaveChangesAsync();
            return Ok("success");
        }

        private  Cart PrepareCart(ApplicationUser user)
        {
            var client = _context.Clients.Include(o => o.Carts).FirstOrDefault(c => c.ApplicationUserId == user.Id);
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

            return cart;
        }
    }
}

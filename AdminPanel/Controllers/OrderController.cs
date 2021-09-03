using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Data;
using AdminPanel.Models;
using AdminPanel.Models.Identity;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class OrderController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public OrderController(UserManager<ApplicationUser> userManager,
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
            return View(_context.Orders
                .Include(o=>o.CartItems).ThenInclude(i=>i.Book)
                .Include(o=>o.Client).ThenInclude(c=>c.ApplicationUser)
                .Where(o=>o.Client.ApplicationUserId == user.Id).OrderByDescending(b=>b.Id).ToList());
        }
        [AllowAnonymous]
        [HttpPost("/payment")]
        public async Task<IActionResult> payment(string token)
        {
            RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
            request.ConversationId = "123456789";
            request.Token = token;
            CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, new Options(){ApiKey = "key",BaseUrl = "https://sandbox-api.iyzipay.com"});
            var orderId = int.Parse(checkoutForm.BasketId);
            var order = _context.Orders.FirstOrDefault(o=>o.Id==orderId);
            if (order == null)
                return NotFound();
            if (checkoutForm.Status == "success")
            {
                order.PaymentReferenceNo = checkoutForm.PaymentId;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GoToPaymentPage(int cartId)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = GetCart(cartId, user);
            if (cart == null)
                return NotFound();
            var order=CreateOrder(cart);
            CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = order.Total.ToString("F");
            request.PaidPrice = order.Total.ToString("F");
            request.Currency = Currency.TRY.ToString();
            request.BasketId = order.Id.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "https://www.onlinebookstoreurl.com/payment";

            List<int> enabledInstallments = new List<int>();
            enabledInstallments.Add(1);
            request.EnabledInstallments = enabledInstallments;

            Buyer buyer = new Buyer();
            buyer.Id = order.Client.Id.ToString();
            buyer.Name = order.Client.ApplicationUser.FullName;
            buyer.Surname = "";
            buyer.GsmNumber = order.Client.ApplicationUser.PhoneNumber;
            buyer.Email = order.Client.ApplicationUser.Email;
            buyer.IdentityNumber = "";
            buyer.LastLoginDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            buyer.RegistrationDate = order.Client.CreationDate.ToString(CultureInfo.InvariantCulture);
            buyer.RegistrationAddress = order.Client.ApplicationUser.Address;
            buyer.Ip = HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            buyer.City = "";
            buyer.Country = "Turkey";
            buyer.ZipCode = "";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "";
            shippingAddress.City = "";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description =order.Client.ApplicationUser.Address;
            shippingAddress.ZipCode = "";
            request.ShippingAddress = shippingAddress;
            request.BillingAddress = shippingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            foreach (var cartItem in order.CartItems)
            {
                basketItems.Add(new BasketItem()
                {
                    Id = cartItem.Book.Id.ToString(),
                    Name = cartItem.Book.Title,
                    Category1 = "Book",
                    Category2 = cartItem.Book.BookCategory.Name,
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = cartItem.Price.ToString("F"),
                });
            }
            request.BasketItems = basketItems;

            CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, new Options(){ApiKey = "key",BaseUrl = "https://sandbox-api.iyzipay.com"});
            if(checkoutFormInitialize.Status== "success" )
                return Redirect(checkoutFormInitialize.PaymentPageUrl);
            return new StatusCodeResult(500);
        }

        private Cart GetCart(int cartId, ApplicationUser user=null)
        {
            return _context.Carts.Include(c => c.CartItems).ThenInclude(i=>i.Book).ThenInclude(b=>b.BookCategory)
                .Include(c=>c.Client).ThenInclude(cl=>cl.ApplicationUser).FirstOrDefault(c => c.Id == cartId &&( user == null || c.Client.ApplicationUserId == user.Id));
        }

        private Order CreateOrder(Cart cart)
        {
            var order = new Order() { CartItems = cart.CartItems, Client = cart.Client};
            _context.Orders.Add(order);
            _context.Carts.Remove(cart);
            _context.SaveChanges();
            return order;
        }

    }
}

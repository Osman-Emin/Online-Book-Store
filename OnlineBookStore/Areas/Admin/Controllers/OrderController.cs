using System;
using System.Linq;
using Faker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Administrator")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _context;  
  
        public OrderController(ApplicationDbContext context)  
        {  
            _context = context;  
        }  
        // GET: /<controller>/  
        public IActionResult Index()  
        {  
            return View();  
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult ToggleProcessed(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                order.IsProcessed = !order.IsProcessed;
                _context.SaveChanges();
            }
            return Ok(true);
        }
        
        [HttpGet]
        public IActionResult Details(int id)
        {
            ViewBag.IsDetail = true;
            return View(_context.Orders.Include(o => o.CartItems).ThenInclude(ci => ci.Book).Include(o => o.Client)
                .ThenInclude(oo => oo.ApplicationUser).FirstOrDefault(o => o.Id == id));
        }
        
        [HttpPost]
        public IActionResult LoadData()  
        {  
            // try  
            // {  
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();  
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();  
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();  
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();  
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();  
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();  
  
                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;  
                int skip = start != null ? Convert.ToInt32(start) : 0;  
                int recordsTotal = 0;  
  
                // Getting all Order data  
                IQueryable<Order> orderData = _context.Orders;
  
                //Sorting  
                // if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))  
                // {  
                //     orderData = orderData.OrderBy(sortColumn , sortColumnDirection);  
                // }  
                //Search  
                if (!string.IsNullOrEmpty(searchValue))  
                {  
                    orderData = orderData.Where(m => m.Client.ApplicationUser.FullName.Contains(searchValue));  
                }  
  
                //total number of rows count   
                recordsTotal = orderData.Count();  
                //Paging   
                var data = orderData.Skip(skip).Take(pageSize).Include(o => o.CartItems).Include(o => o.Client)
                    .ThenInclude(oo => oo.ApplicationUser).Select(cc=>new
                    {
                        cc.Id, Client = cc.Client.ApplicationUser.FullName,Phone = cc.Client.ApplicationUser.PhoneNumber,
                        cc.PaymentReferenceNo,cc.IsProcessed,
                        CreationDate = cc.CreationDate.ToString("f"),Total=cc.Total.ToString("F")
                    }).ToList();  
                //Returning Json Data  
                var x =Content(Newtonsoft.Json.JsonConvert.SerializeObject(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }),"application/json");
                return x;

                // }  
                // catch (Exception)  
                // {  
                //     throw;  
                // }  

        }  
    }  
    
}

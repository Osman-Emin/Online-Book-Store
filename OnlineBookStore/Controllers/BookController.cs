using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using OnlineBookStore.Infrastructure;
using OnlineBookStore.Infrastructure.ErrorHandling;
using OnlineBookStore.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Controllers
{
    public class BookController : Controller
    {
        private ApplicationDbContext _context;  
  
        public BookController(ApplicationDbContext context)  
        {  
            _context = context;  
        } 
        
        // [HttpGet("/Book/GetBooks")]
        public IActionResult GetBooks(int length = 10,int page=1,bool random = false,string term = null)
        {
            IQueryable<Book> data = _context.Books;
            if (random)
            {
                data = data.OrderBy(o => Guid.NewGuid());
            }
            if (!string.IsNullOrWhiteSpace(term))
            {
                data = data.Where(o =>
                    o.Title.Contains(term) || o.Author.Name.Contains(term) || o.BookCategory.Name.Contains(term) ||
                    o.Publisher.Name.Contains(term));
            }

            var noPages = Math.Ceiling(data.Count()/(double)length);
            var result = data.Skip(length * (page-1)).Take(length).ToList();
            var x =Content(Newtonsoft.Json.JsonConvert.SerializeObject(new {length = noPages , data = result }),"application/json");
            return x;
        }
        public IActionResult Details(int bookId)
        {
            return View(_context.Books
                .Include(o=>o.Author)
                .Include(o=>o.Publisher)
                .Include(o=>o.BookCategory)
                .First(b=>b.Id==bookId));
        }
       
    }
}

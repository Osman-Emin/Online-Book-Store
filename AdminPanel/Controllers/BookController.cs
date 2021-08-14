using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdminPanel.Data;
using AdminPanel.Infrastructure;
using AdminPanel.Infrastructure.ErrorHandling;
using AdminPanel.Models;
using AdminPanel.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AdminPanel.Controllers
{
    public class BookController : Controller
    {
        private ApplicationDbContext _context;  
  
        public BookController(ApplicationDbContext context)  
        {  
            _context = context;  
        } 
        
        // [HttpGet("/Book/GetBooks")]
        public IActionResult GetBooks(int length = 10,int page=1,bool random = false)
        {
            IQueryable<Book> data = _context.Books;
            if (random)
            {
                data = data.OrderBy(o => Guid.NewGuid());
            }
            var result = data.Skip(length * page).Take(length).ToList();
            var x =Content(Newtonsoft.Json.JsonConvert.SerializeObject(new {  data = result }),"application/json");
            return x;
        }
       
    }
}

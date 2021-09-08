using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBookStore.Data;
using OnlineBookStore.Models;

namespace OnlineBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Administrator")]
    public class BookController : Controller
    {
        private ApplicationDbContext _context;  
  
        public BookController(ApplicationDbContext context)  
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
        public IActionResult Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(o => o.Id == id);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
            return Ok(true);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Details(Book book)
        {
            try
            {
                _context.Update(book); 
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "There is something wrong while saving item.");
                return View();
            }
            
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            ViewBag.IsDetail = true;
            SetSelectLists();
            return View(GetBookWithData(id));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.IsDetail = false;
            SetSelectLists();
            return View("Details",GetBookWithData(id));
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.IsDetail = false;
            SetSelectLists();
            return View("Details",new Book());
        }

        private void SetSelectLists()
        {
            ViewBag.BookCategories = _context.BookCategories.ToList();
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Publishers = _context.Publishers.ToList();
        }

        private Book GetBookWithData(int bookId)
        {

            return _context.Books
                .Include(o => o.Author)
                .Include(o => o.Publisher)
                .Include(o => o.BookCategory)
                .First(b => b.Id == bookId);
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
  
                // Getting all Book data  
                IQueryable<Book> bookData = _context.Books;

                //Sorting  
                // if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))  
                // {  
                //     bookData = bookData.OrderBy(sortColumn , sortColumnDirection);  
                // }  
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    bookData=bookData.Where(m => m.Title.Contains(searchValue));
                }  
  
                //total number of rows count   
                recordsTotal = bookData.Count();  
                //Paging   
                var data = from d in bookData.Skip(skip).Take(pageSize)
                        .Include(o => o.Author)
                        .Include(o => o.Publisher)
                        .Include(o => o.BookCategory).AsQueryable()  
                    select new { 
                        d.Id,
                        d.Title,
                        d.ImageUrl,
                        d.ISBN,
                        Author=d.Author.Name,
                        Publisher=d.Publisher.Name,
                        d.PublishYear,
                        d.CopyNumber,
                        BookCategory=d.BookCategory.Name,
                        d.Price,
                    };
                // data = data.Select(tempBook => new { Id = tempBook.Id.ToString(),
                //     Title = tempBook.Title})
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

using System;
using System.Globalization;
using System.Linq;
using AdminPanel.Data;
using AdminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize]
    public class BookCategoryController : Controller
    {
        private ApplicationDbContext _context;  
  
        public BookCategoryController(ApplicationDbContext context)  
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
            var bookCategory = _context.BookCategories.FirstOrDefault(o => o.Id == id);
            if (bookCategory != null)
            {
                _context.BookCategories.Remove(bookCategory);
                _context.SaveChanges();
            }
            return Ok(true);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Details(BookCategory bookCategory)
        {
            try
            {
                _context.Update(bookCategory); 
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
            return View(_context.BookCategories.Find(id));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.IsDetail = false;
            return View("Details",_context.BookCategories.Find(id));
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.IsDetail = false;
            return View("Details",new BookCategory());
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
  
                // Getting all BookCategory data  
                var bookCategoryData = (from tempaBookCategory in _context.BookCategories
                    select new{Id=tempaBookCategory.Id.ToString(),Name=tempaBookCategory.Name,CreationDate=tempaBookCategory.CreationDate.ToString(CultureInfo.InvariantCulture)} );  
  
                //Sorting  
                // if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))  
                // {  
                //     bookCategoryData = bookCategoryData.OrderBy(sortColumn , sortColumnDirection);  
                // }  
                //Search  
                if (!string.IsNullOrEmpty(searchValue))  
                {  
                    bookCategoryData = bookCategoryData.Where(m => m.Name.Contains(searchValue));  
                }  
  
                //total number of rows count   
                recordsTotal = bookCategoryData.Count();  
                //Paging   
                var data = bookCategoryData.Skip(skip).Take(pageSize).ToList();  
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

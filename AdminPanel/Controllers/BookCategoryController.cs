using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class BookCategoryController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}

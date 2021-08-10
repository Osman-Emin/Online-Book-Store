using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class BookController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}

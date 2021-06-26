using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Infrastructure
{
    [Route("[controller]/[action]", Name = "[controller]_[action]")]
    public abstract class BaseController : Controller
    {
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

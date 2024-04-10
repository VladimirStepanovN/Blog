using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

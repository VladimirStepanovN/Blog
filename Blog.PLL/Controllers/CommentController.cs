using Microsoft.AspNetCore.Mvc;

namespace Blog.PLL.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Blog.PLL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.PLL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Index action called");
			return View();
        }

        public IActionResult Privacy()
        {
			_logger.LogInformation($"{User.Identity.Name} :: [HttpGet] Privacy action called");
			return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
			_logger.LogError($"{User.Identity.Name} :: [HttpGet] Error action called");
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            _logger.LogError($"{User.Identity.Name} :: [HttpGet] AccessDenied action called");
            return View();
        }

        public IActionResult PageNotFound()
        {
            _logger.LogError($"{User.Identity.Name} :: [HttpGet] PageNotFound action called");
            return View();
        }
    }
}

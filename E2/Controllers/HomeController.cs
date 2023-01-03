using Microsoft.AspNetCore.Mvc;

namespace E2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
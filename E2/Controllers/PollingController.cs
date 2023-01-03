using Microsoft.AspNetCore.Mvc;

namespace E2.Controllers
{
    public class PollingController : Controller
    {
        /// <summary>
        /// Loads the initial page
        /// </summary>
        public IActionResult Index() => View();

        /// <summary>
        /// Called with each client-side polling calls
        /// </summary>
        public async Task<IActionResult> Poll()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            return PartialView(DateTime.UtcNow);
        }
    }
}
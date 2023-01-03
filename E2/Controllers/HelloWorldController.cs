using Microsoft.AspNetCore.Mvc;

namespace E2.Controllers
{
    /// <summary>
    /// Some basic examples as a warmup
    /// </summary>
    public class HelloWorldController : Controller
    {
        /// <summary>
        /// Loads the initial page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View();

        /// <summary>
        /// Given  GET /helloworld/basicclick this returns a partial as a response
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> BasicClick()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            return PartialView();
        }

        private static int _ticker = 0;
        /// <summary>
        /// Given POST /helloworld/increment this returns a partial containing the value of the static server-side variable
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Increment()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            return PartialView(++_ticker);
        }
    }
}
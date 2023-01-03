using Microsoft.AspNetCore.Mvc;

namespace E2.Controllers
{
    public class SwapOobController : Controller
    {
        /// <summary>
        /// Loads the initial page
        /// </summary>
        public IActionResult Index() => View(new SwapOobViewModel());
        
        /// <summary>
        /// Call to GET /swapoob/additem loads the additem menu as a partial
        /// </summary>
        public IActionResult AddItem() => PartialView(new SwapOobViewModel());
        
        /// <summary>
        /// Call to POST /swapoob/additem adds to the items selected state
        /// </summary>
        /// <param name="viewModel"></param>
        [HttpPost]
        public async Task<IActionResult> AddItem(SwapOobViewModel viewModel)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            viewModel.ItemsSelected++;
            return PartialView(viewModel);
        }
    }

    public class SwapOobViewModel
    {
        [BindProperty]
        public int ItemsSelected { get; set; }
    }
}
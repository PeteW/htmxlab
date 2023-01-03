using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace E2.Controllers
{
    public class ServerSideModalController: Controller
    {
        /// <summary>
        /// Loads the initial page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View();

        /// <summary>
        /// GET /serversidemodal/form returns a blank form
        /// </summary>
        public IActionResult Form() => PartialView(new ModalReqViewModel());

        /// <summary>
        /// POST /serversidemodal/form validates the post and returns actionable responses
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Form(ModalReqViewModel reqViewModel)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            reqViewModel.IsValid = ModelState.IsValid;
            return PartialView(reqViewModel);
        }
        
        /// <summary>
        /// Returns the modal dialog as a partial
        /// </summary>
        public async Task<IActionResult> Modal()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            return PartialView();
        }
    }

    public class ModalReqViewModel
    {
        [BindProperty, EmailAddress, Required]
        public string Email { get; set; }
        public bool IsValid { get; set; }
    }
}
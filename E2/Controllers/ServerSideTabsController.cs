using Microsoft.AspNetCore.Mvc;

namespace E2.Controllers
{
    public class ServerSideTabsController : Controller
    {
        /// <summary>
        /// Loads the initial page
        /// </summary>
        public IActionResult Index() => View();

        // just pretend this is some stuff in a database somewhere
        private static readonly TabItemCollectionViewModel _tabItemCollection = new()
        {
            Items = new List<TabItemViewModel>
            {
                new() {Id = 1, Name = "Tab1", IsActive = true,},
                new() {Id = 2, Name = "Tab2", IsActive = false,},
                new() {Id = 3, Name = "Tab3", IsActive = false,},
            }
        };

        /// <summary>
        /// Given a GET /serversidetabs/<id> manipulate the server-side tabs to show the proper active tab
        /// </summary>
        public async Task<IActionResult> Tabs(int id = 1)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            _tabItemCollection.Items.ForEach(x => x.IsActive = x.Id == id);
            return PartialView(_tabItemCollection);
        }
    }

    public class TabItemCollectionViewModel
    {
        public List<TabItemViewModel> Items { get; set; } = new();
    }

    public class TabItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}

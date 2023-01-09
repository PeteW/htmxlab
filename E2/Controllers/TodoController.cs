using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace E2.Controllers
{
    public class TodoController : Controller
    {
        public TodoController() { Db.EnsureDatabase(); }
        public IActionResult Index() => View();
        public async Task<IActionResult> List(TodoStatus? status) => PartialView(Db.GetByStatus(status));
        public async Task<IActionResult> Edit(int id = 0) => PartialView(id == 0 ? new TodoViewModel() : Db.GetById(id));

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            Db.DeleteById(id);
            Response.Headers.Add("HX-Trigger-After-Swap", "itemSaved");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TodoViewModel todoViewModel)
        {
            if (ModelState.IsValid)
            {
                await Db.InsertOrUpdateTodo(todoViewModel);
                // HTMX looks for headers like this in the response. they become programmable JS event hooks
                // https://htmx.org/headers/hx-trigger
                Response.Headers.Add("HX-Trigger-After-Swap", "itemSaved");
            }
            
            return PartialView(todoViewModel);
        }
    }

    public enum TodoStatus { New, Active, Done, }
    
    public class TodoViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public TodoStatus Status { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Demo of custom server-side validation logic
        /// </summary>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                return new[] {new ValidationResult("You need to fill out the description field", new[] {nameof(Description)})};
            }
            else if (!Description.ToLower().StartsWith("please"))
            {
                return new[] {new ValidationResult("Todo must start with 'please'", new[] {nameof(Description)})};
            }
            else if (Description.Length > 10)
            {
                return new[] {new ValidationResult("Todo max length is 10", new[] {nameof(Description)})};
            }
            else if (Db.IsDescriptionInUse(Description))
            {
                return new[] {new ValidationResult("This description is a duplicate", new[] {nameof(Description)})};
            }
            else
            {
                return Array.Empty<ValidationResult>();
            }
        }
    }
    
    /// <summary>
    /// Let's pretend this is an interface to some database
    /// </summary>
    public class Db
    {
        private static List<TodoViewModel> _db;
        public static async Task EnsureDatabase()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            if (_db == null)
            {
                _db = new List<TodoViewModel>();
                for (var i = 1; i < 5; i++)
                {
                    _db.Add(new TodoViewModel
                    {
                        Id = i,
                        Description = $"Please do item #{i}",
                        Status = Enum.GetValues<TodoStatus>()[i % Enum.GetValues<TodoStatus>().Length],
                    });
                }
            }
        }

        public static TodoViewModel GetById(int id) => _db.Single(x => x.Id == id);

        public static bool IsDescriptionInUse(string description) =>
        _db.Any(x => x.Description.Equals(description, StringComparison.OrdinalIgnoreCase));
        
        public static List<TodoViewModel> GetByStatus(TodoStatus? status) =>
        _db.Where(x => !status.HasValue || x.Status == status).ToList();

        public static void DeleteById(int id) => _db = _db.Where(x => x.Id != id).ToList();
        
        public static async Task InsertOrUpdateTodo(TodoViewModel todoViewModel)
        {
            await EnsureDatabase();
            if (todoViewModel.Id == 0)
            {
                todoViewModel.Id = _db.Max(x => x.Id) + 1;
                _db.Add(todoViewModel);
            }

            _db.Single(x => x.Id == todoViewModel.Id).Description = todoViewModel.Description;
            _db.Single(x => x.Id == todoViewModel.Id).Status = todoViewModel.Status;
        }
    }
}
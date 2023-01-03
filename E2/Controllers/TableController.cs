using Microsoft.AspNetCore.Mvc;

namespace E2.Controllers
{
    public class TableController : Controller
    {
        private static List<TableRowViewModel> _db;
        
        /// <summary>
        /// Returns the index view
        /// </summary>
        public IActionResult Index() => View();

        /// <summary>
        /// Call GET /table/table returns the table with default criteria as a partial
        /// </summary>
        public async Task<IActionResult> Table() => PartialView(await GetTableViewModel(TableCriteria.Default));
        
        /// <summary>
        /// Call POST /table/table returns the table using customized criteria as a partial
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Table(TableCriteria criteria) => PartialView(await GetTableViewModel(criteria));
        
        /// <summary>
        /// Think of this as database lookup logic
        /// </summary>
        private async Task<TableViewModel> GetTableViewModel(TableCriteria criteria)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(250));
            EnsureDatabase();
            var result = new TableViewModel() {Rows = _db.ToList()};
            if (criteria.FilterText != null)
            {
                result.Rows = result.Rows.Where(x => x.Name.ToLower().Contains(criteria.FilterText.ToLower())).ToList();
            }

            if (criteria.SortColumn == "Id")
            {
                if(criteria.SortDescending)
                {
                    result.Rows = result.Rows.OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    result.Rows = result.Rows.OrderBy(x => x.Id).ToList();
                }
            }
            
            if (criteria.SortColumn == "Name")
            {
                if(criteria.SortDescending)
                {
                    result.Rows = result.Rows.OrderByDescending(x => x.Name).ToList();
                }
                else
                {
                    result.Rows = result.Rows.OrderBy(x => x.Name).ToList();
                }
            }

            result.Criteria = criteria;
            result.TotalItems = result.Rows.Count;
            result.Rows = result.Rows.Skip(criteria.Offset).Take(criteria.Count).ToList();
            return result;
        }

        /// <summary>
        /// Create some static data if not exists
        /// </summary>
        private void EnsureDatabase()
        {
            if (_db == null)
            {
                _db = new List<TableRowViewModel>();
                for(var i = 0; i < 1000; i++)
                {
                    _db.Add(new TableRowViewModel {Id = i, Name = $"Item-{i}-{Guid.NewGuid().ToString()}"});
                }
            }
        }
    }

    /// <summary>
    /// Representation of table searching/sorting/filtering criteria
    /// </summary>
    public class TableCriteria
    {
        /// <summary>
        /// Default criteria (AKA null object pattern)
        /// </summary>
        public static TableCriteria Default => new()
        {
            Offset = 0,
            Count = 10,
            SortColumn = "Id"
        };
        
        public int Count { get; set; }
        public int Offset { get; set; }
        public string? FilterText { get; set; }
        public string? SortColumn { get; set; }
        public bool SortDescending { get; set; }
        
        /// <summary>
        /// Just a helper method which returns the up/down arrow icon on the table for a given column based on criteria
        /// </summary>
        public string GetSortArrowForColumn(string columnName)
        {
            if (SortColumn == columnName && SortDescending)
            {
                return "<i class='fa fa-arrow-down'></i>";
            }
            else if (SortColumn == columnName && !SortDescending)
            {
                return "<i class='fa fa-arrow-up'></i>";
            }
            else
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// The view model for the current state of the table including the current rows displayed and current criteria
    /// </summary>
    public class TableViewModel
    {
        public List<TableRowViewModel> Rows { get; set; } = new();
        public int TotalItems { get; set; }
        public TableCriteria Criteria { get; set; }
        public bool IsFirstPage() => Criteria.Offset == 0;
        public bool IsLastPage() => Criteria.Offset + Criteria.Count >= TotalItems;
    }

    public class TableRowViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Extension;

namespace Repositories.Helpers
{
    public class QueryObject
    {
        public string? Symbol {  get; set; } = null;
        public string? CompanyName { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        [PositiveNumberValidation(ErrorMessage = "PageNumber must be a positive number.")]
        public int PageNumber { get; set; } = 1;
        [PositiveNumberValidation(ErrorMessage = "PageSize must be a positive number.")]
        public int PageSize { get; set; } = 20;
    }
}

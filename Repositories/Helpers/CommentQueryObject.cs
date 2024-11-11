using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Helpers
{
    public class CommentQueryObject
    {
        public string Symbol {  get; set; } = String.Empty;
        public bool IsDecsending { get; set; } = true;
    }
}

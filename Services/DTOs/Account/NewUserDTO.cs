using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs.Account
{
    public class NewUserDTO
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}

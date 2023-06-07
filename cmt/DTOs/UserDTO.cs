using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.DTOs
{
    public partial class UserDTO
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
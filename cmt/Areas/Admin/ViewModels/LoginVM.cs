using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cmt.Areas.Admin.ViewModels
{
    public class LoginVM
    {
        [Display(Name = "帳號")]
        [Required]
        public string account { get; set; }

        [Display(Name = "密碼")]
        [Required]
        public string password { get; set; }
    }
}
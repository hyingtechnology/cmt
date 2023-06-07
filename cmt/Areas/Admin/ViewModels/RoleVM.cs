using cmt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Areas.Admin.ViewModels
{
    public class RoleVM
    {
        public List<SelectListItem> codeTables { get; set; }
        public UserLoginVM Users { get; set; }
    }
}
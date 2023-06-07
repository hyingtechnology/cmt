using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Areas.Admin.ViewModels
{
    public class assign_RoleModel
    {
       public string  assign_Guid { get; set; }
        public string  assign_UserId { get; set; }
        public string  assign_Role { get; set; }
        public DateTime assign_CreateTime { get; set; }
        public DateTime assign_EditTime { get; set; }
        public string  assign_CreateUserId { get; set; }
        public string  assign_EditUserId { get; set; }

        public string roleName { get; set; }
    }
}
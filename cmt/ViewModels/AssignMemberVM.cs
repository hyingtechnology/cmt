using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.ViewModels
{
    public class AssignMemberVM
    {
        public string UserGuid { get; set; }
        public string UserAccount { get; set; }

        public string UserNickname { get; set; }
        public string CreateTime { get; set; }
        public string Company { get; set; }
        public string Type { get; set; }
        public string UserEmail { get; set; }
        public DateTime EditTime { get; set; }
        public bool IsEnabled { get; set; }

        public string RoleName { get; set; }

        public string Role { get; set; }
    }
}
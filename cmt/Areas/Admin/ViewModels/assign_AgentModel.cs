using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Areas.Admin.ViewModels
{
    public class assign_AgentModel
    {

        public string assign_Agent_Guid { get; set; }
        public string assign_UserId { get; set; }
        public string assign_AgentUserId { get; set; }
        public DateTime assign_CreateTime { get; set; }
        public string assign_CreateUserId { get; set; }
        public DateTime assign_EditTime { get; set; }
        public string assign_EditUserId { get; set; }

        public string Agent_userName { get; set; }
    }
}
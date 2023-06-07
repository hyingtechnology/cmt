using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.DTOs
{
    public class CodeTableDTO
    {
        public string CT_type { get; set; }
        public string CT_typedesc { get; set; }
        public string CT_value { get; set; }
        public string CT_desc { get; set; }
        public string CT_relationtype { get; set; }
        public string CT_relationvalue { get; set; }
        public string CT_order { get; set; }
        public string CT_enabled { get; set; }
        public string CT_mod_user { get; set; }
        public string CT_mod_date { get; set; }
        public string CT_remark { get; set; }
    }
}
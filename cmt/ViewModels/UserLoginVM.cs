using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cmt.ViewModels
{
    public class UserLoginVM
    {
        public string UserGuid { get; set; }
        [Required(ErrorMessage = "請填寫 欄位")]
        
        public string UserAccount { get; set; }
        public string UserPassWord { get; set; }

        [Required(ErrorMessage = "請填寫 欄位")]
        public string UserNickname { get; set; }


        public string CreateTime { get; set; }

        [Required(ErrorMessage = "請填寫 欄位")]
        public string Company { get; set; }
        public string Type { get; set; }

        [Required(ErrorMessage = "請填寫 欄位")]
        public string UserEmail { get; set; }

        public string HttpPostResult { get; set; }


        public bool IsEnabled { get; set; }


        public string Error { get; set; }

        public string mode { get; set; }


        public string Role { get; set; }

        public string RoleName { get; set; }

        public string Agent { get; set; }

        public string AgentName { get; set; }
    }
}
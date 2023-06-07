using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Areas.Admin.ViewModels
{
    public class ForeignEditVM
    {
        public ForeignEditVM()
        {
            this.Attaches = new List<Attach>();
        }
        public int Id { get; set; }
        public string TempId { get; set; }
        public int Type { get; set; }
        [Display(Name = "標題")]
        [Required(ErrorMessage = "請填寫 欄位 {0} ")]
        [StringLength(100, ErrorMessage = "欄位 {0} 不可超過{1}碼")]
        public string Title { get; set; }
        [Display(Name = "簡介")]
        [StringLength(100, ErrorMessage = "欄位 {0} 不可超過{1}碼")]
        public string Brief { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Picture { get; set; }
        public HttpPostedFileBase PictureFile { get; set; }
        public List<Attach> Attaches { get; set; }
        public IEnumerable<HttpPostedFileBase> AttachFiles { get; set; }
        [Display(Name = "開始公開日期")]
        [Required(ErrorMessage = "請填寫 欄位 {0} ")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<DateTime> Start_date { get; set; }
        [Display(Name = "結束公開日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<DateTime> End_date { get; set; }
        public bool Is_public { get; set; }
        public string Creator { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<DateTime> Create_date { get; set; }
        public string Modifier { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<DateTime> Modified_date { get; set; }
        public class Attach
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.ViewModels
{
    public class DomesticDetailVM
    {
        public int Id { get; set; }
        public string TempId { get; set; }
        public int Type { get; set; }
        [Display(Name = "標題")]
        public string Title { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string Picture { get; set; }
        public HttpPostedFileBase PictureFile { get; set; }
        public List<Attach> Attaches { get; set; }
        public IEnumerable<HttpPostedFileBase> AttachFiles { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<DateTime> Start_date { get; set; }
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
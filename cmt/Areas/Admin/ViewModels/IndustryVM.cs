using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using X.PagedList;

namespace cmt.Areas.Admin.ViewModels
{
    public class IndustryVM
    {
        public IPagedList<Industry> Industries { get; set; }
        public string Keywords { get; set; }
        public string NewSort { get; set; }
        public string LastSort { get; set; }
        public bool IsSortAsc { get; set; }
        public partial class Industry
        {
            public int Id { get; set; }
            public int Type { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Picture { get; set; }
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
        }
    }
}
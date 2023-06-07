using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.DTOs
{
    public class NewsDTO
    {
        public int Id { get; set; }
        public string TempId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Brief { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public string Url { get; set; }
        public HttpPostedFileBase PictureFile { get; set; }
        public List<NewsFileDTO> Attaches { get; set; }
        public IEnumerable<HttpPostedFileBase> AttachFiles { get; set; }
        public Nullable<DateTime> Start_date { get; set; }
        public Nullable<DateTime> End_date { get; set; } 
        public bool Is_public { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> Create_date { get; set; }
        public string Modifier { get; set; }

        public string parent_type { get; set; }
        public Nullable<DateTime> Modified_date { get; set; }
    }
}
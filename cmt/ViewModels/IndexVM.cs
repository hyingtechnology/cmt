using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.ViewModels
{
    public class IndexVM
    {
        public List<News> Sliders { get; set; }
        public List<News> Industrys { get; set; }
        public List<News> Knowledges { get; set; }
        public List<News> Events { get; set; }
        public class News
        {
            public int Id { get; set; }
            public int Type { get; set; }
            public string Title { get; set; }
            public string Brief { get; set; }
            public string Content { get; set; }
            public string Picture { get; set; }
            public string Url { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.DTOs
{
    public class KnowledgeSearchDTO
    {
        public int Type { get; set; }
        public string Keywords { get; set; }
        public string SortKey { get; set; }
        public bool IsSortAsc { get; set; }
    }
}
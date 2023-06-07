using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Helper
{
    public static class SortHelper
    {
        public static MvcHtmlString SortIcon(this HtmlHelper helper, string newSort,string lastSort, bool isSortAsc)
        {
            var className = "fa fa-fw fa-sort";
            if (lastSort == newSort)
            {
                className = isSortAsc ? "fa fa-fw fa-sort-up" : "fa fa-fw fa-sort-down";
            }

            return new MvcHtmlString($"<i class=\"{className}\"></i>");
        }
    }
}
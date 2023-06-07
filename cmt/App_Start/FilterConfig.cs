using cmt.Extensions.CustomFilter;
using System.Web;
using System.Web.Mvc;

namespace cmt
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionAttribute());
        }
    }
}

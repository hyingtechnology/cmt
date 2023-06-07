using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Extensions.CustomFilter
{
    public class ExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public  void OnException(ExceptionContext filterContext)
        {
            if (filterContext != null && filterContext.Exception != null)
            {
                string controller = filterContext.RouteData.Values["controller"].ToString();
                string action = filterContext.RouteData.Values["action"].ToString();
                string loggerName = string.Format("{0}Controller.{1}", controller, action);

                LogManager.GetLogger(loggerName).Error(filterContext.Exception);
            }
        }
    }
}
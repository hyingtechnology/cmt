using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Services
{
    public class Common
    {
        public static string ApplicationName = "CmvmSystem";
        public static string CookieName = ".Cmvm.SharedCookie";
        public static string SessionName = ".Cmvm.SharedSession";
        public static string AuthenticationScheme = "CmvmSystemCookie";

        public static bool DateTimeCompareByToday(DateTime? dateTime)
        {
            return DateTime.Compare(DateTime.Now, (DateTime)dateTime) >= 0;
        }
        public static bool DateTimeCompareByEndTime(DateTime? dateTime)
        {
            if (dateTime != null)
            {
                DateTime endTime = DateTime.Parse(((DateTime)dateTime).ToString("yyyy-MM-dd 23:59:00"));
                return DateTime.Compare(endTime, DateTime.Now) >= 0;
            }
            else return true;
        }

    }
}
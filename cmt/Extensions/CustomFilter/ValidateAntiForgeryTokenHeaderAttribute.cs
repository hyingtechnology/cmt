using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace cmt.Extensions.CustomFilter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateAntiForgeryTokenHeaderAttribute : FilterAttribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            ValidateRequestHeader(filterContext.HttpContext.Request);
        }

		private void ValidateRequestHeader(HttpRequestBase request)
		{
			String cookieToken = String.Empty;
			String formToken = String.Empty;
			String TokenValue = request.Headers["RequestVerificationToken"];

			if (!String.IsNullOrWhiteSpace(TokenValue))
			{
				String[] Tokens = TokenValue.Split(':');
				if (Tokens.Length == 2)
				{
					cookieToken = Tokens[0];
					formToken = Tokens[1];
				}
				AntiForgery.Validate(cookieToken, formToken);
			}
		}
	}
}
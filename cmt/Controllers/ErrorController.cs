using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace cmt.Controllers
{
    public class ErrorController : Controller
    {
        // GET api/<controller>
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;

            return View();
        }

        public ActionResult NotFound403()
        {
            Response.StatusCode = 403;

            return View();
        }


    }
}
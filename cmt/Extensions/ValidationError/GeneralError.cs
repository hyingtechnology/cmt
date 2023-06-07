using cmt.Extensions.ValidationError.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Extensions.ValidationError
{
    public class GeneralError : IBaseError
    {
        public string PropertyName { get { return string.Empty; } }
        public string PropertyExceptionMessage { get; set; }
        public GeneralError(string errorMessage)
        {
            PropertyExceptionMessage = errorMessage;
        }
    }
}
using cmt.Extensions.ValidationError.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Extensions.ValidationError
{
    public class PropertyError : IBaseError
    {
        public string PropertyName { get; set; }
        public string PropertyExceptionMessage { get; set; }
        public PropertyError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            PropertyExceptionMessage = errorMessage;
        }
    }
}
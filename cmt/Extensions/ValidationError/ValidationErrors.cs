using cmt.Extensions.ValidationError.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Extensions.ValidationError
{
    public class ValidationErrors : Exception, IValidationErrors
    {
        public List<IBaseError> Errors { get; set; }
        public ValidationErrors()
        {
            Errors = new List<IBaseError>();
        }

        public ValidationErrors(IBaseError error): this()
        {
            Errors.Add(error);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace cmt.Extensions.ValidationError
{
    public class DatabaseValidationErrors : ValidationErrors
    {
        public DatabaseValidationErrors(IEnumerable<DbEntityValidationResult> errors) : base()
        {
            foreach (var err in errors.SelectMany(DbEntityValidationResult => DbEntityValidationResult.ValidationErrors))
            {
                Errors.Add(new PropertyError(err.PropertyName, err.ErrorMessage));
            }
        }
    }
}
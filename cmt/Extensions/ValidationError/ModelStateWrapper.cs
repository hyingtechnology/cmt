using cmt.Extensions.ValidationError.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Extensions.ValidationError
{
    public class ModelStateWrapper : IValidationDictionary
    {
        public bool IsValid
        {
            get { return _modelState.IsValid; }
        }

        private ModelStateDictionary _modelState;

        public ModelStateWrapper(ModelStateDictionary modelState)
        {
            _modelState = modelState;
        }

        public void AddError(string key, string message)
        {
            _modelState.AddModelError(key, message);
        }

        public void AddValidationErrors(ValidationErrors validationErrors)
        {
            foreach (var err in validationErrors.Errors)
            {
                AddError(err.PropertyName, err.PropertyExceptionMessage);
            }
        }
    }
}
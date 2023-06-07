using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmt.Extensions.ValidationError.Interface
{
    public interface IValidationDictionary
    {
        bool IsValid { get; }
        void AddError(string key, string message);
        void AddValidationErrors(ValidationErrors validationErrors);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmt.Extensions.ValidationError.Interface
{
    public interface IValidationErrors
    {
        List<IBaseError> Errors { get; set; }
    }
}

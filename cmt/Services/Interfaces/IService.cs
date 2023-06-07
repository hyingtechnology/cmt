using cmt.Extensions.ValidationError.Interface;
using cmt.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmt.Services.Interfaces
{
    public interface IService
    {
        IValidationDictionary ValidationDictionary { get; }

        void InitialiseIValidationDictionary(IValidationDictionary inValidationDictionary);

        void SaveChanges(IUnitOfWork uow);
    }
}

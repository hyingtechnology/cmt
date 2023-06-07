using cmt.Extensions.ValidationError;
using cmt.Extensions.ValidationError.Interface;
using cmt.Services.Interfaces;
using cmt.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace cmt.Services
{
    public class GenericService : IService
    {
        public readonly string _uploadFileDir = WebConfigurationManager.AppSettings["UploadFileDir"];

        public IValidationDictionary ValidationDictionary { get; private set; }

        public void InitialiseIValidationDictionary(IValidationDictionary validationDictionary)
        {
            ValidationDictionary = validationDictionary;
        }

        public void SaveChanges(IUnitOfWork uow)
        {
            try
            {
                uow.SaveChanges();
            }
            catch (ValidationErrors propertyErrors)
            {
                ValidationDictionary.AddValidationErrors(propertyErrors);
            }
        }
    }
}
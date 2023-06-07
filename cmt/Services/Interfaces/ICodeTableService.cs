using cmt.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Services.Interfaces
{
    public interface ICodeTableService : IService
    {
        List<SelectListItem> GetCodeTableByType(string Type);

        string Get_Desc(string Type, string value);
    }
}
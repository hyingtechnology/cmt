using AutoMapper;
using cmt.DTOs;
using cmt.Models;
using cmt.Repositories.Interfaces;
using cmt.Services.Interfaces;
using cmt.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace cmt.Services
{
    public class CodeTableService: GenericService, ICodeTableService
    {
        private readonly ICmtUow _cmtUow;
        private readonly ICodeTableRepo _codeTable;
        public CodeTableService(ICmtUow cmtUow, ICodeTableRepo codeTable)
        {
            _cmtUow = cmtUow;
            _codeTable = codeTable;

        }

        public List<SelectListItem> GetCodeTableByType(string Type)
        {
            var m = _codeTable.GetAll().Where(x => x.CT_type == Type).ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("n_");
                cfg.CreateMap<CodeTable, CodeTableDTO>();
            });
            var mapper = config.CreateMapper();
            var codeTableModel = mapper.Map<List<CodeTable>, List<CodeTableDTO>>(m);

            return codeTableModel.Select(x => new SelectListItem
            {
                Text = x.CT_desc,
                Value = x.CT_value
            }).ToList();
        }

        public string Get_Desc(string Type,string value)
        {
            var m = _codeTable.GetAll().Where(x => x.CT_type == Type && x.CT_value == value).ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("n_");
                cfg.CreateMap<CodeTable, CodeTableDTO>();
            });
            var mapper = config.CreateMapper();
            var codeTableModel = mapper.Map<List<CodeTable>, List<CodeTableDTO>>(m);

             if (codeTableModel.Count() > 0)
            {
                return codeTableModel.First().CT_desc;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
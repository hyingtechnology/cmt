using cmt.Repositories.Interfaces;
using cmt.Services.Interfaces;
using cmt.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace cmt.Services
{
    public class LogService : GenericService, ILogService
    {
        public readonly string _StartPageViewCountDate = WebConfigurationManager.AppSettings["StartPageViewCountDate"];
        private readonly ICmtUow _cmtUow;
        private readonly ILogRepo _logRepo;

        public LogService(ICmtUow cmtUow, ILogRepo logRepo)
        {
            _cmtUow = cmtUow;
            _logRepo = logRepo;
        }

        public int GetPageViewCount()
        {
            DateTime startDate = Convert.ToDateTime(_StartPageViewCountDate);
            var pvCount = _logRepo.GetAll().Where(x => x.l_Method== "GET" && x.l_message == "瀏覽" && !x.l_url.Contains("/Admin/") && DbFunctions.TruncateTime(x.l_create_date) >= startDate).Count();
            return pvCount;
        }
    }
}
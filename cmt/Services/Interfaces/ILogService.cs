using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmt.Services.Interfaces
{
    public interface ILogService : IService
    {
        int GetPageViewCount();
    }
}

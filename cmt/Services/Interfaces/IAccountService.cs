using cmt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmt.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// 取得所有使用者
        /// </summary>
        /// <returns></returns>
        List<UserLoginVM> GetUserAll();


        /// <summary>
        /// 取得個別使用者
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        UserLoginVM GetUser(string guid);



        /// <summary>
        /// 使用者新增或更新
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        string UpdateUser(UserLoginVM models);


        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        string DeleteUser(string guid);
    }
}

using cmt.DTOs;
using cmt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmt.Services.Interfaces
{
    public interface IAuthService : IService
    {
        bool IsValidLogin(LoginDTO loginDTO);
        User GetUserByAccount(UserDTO userDTO);
    }
}

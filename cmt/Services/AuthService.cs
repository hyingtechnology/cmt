using cmt.DTOs;
using cmt.Extensions.Cryptography;
using cmt.Models;
using cmt.Repositories.Interfaces;
using cmt.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace cmt.Services
{
    public class AuthService : GenericService, IAuthService
    {
        private readonly IUserRepo _userRepo;

        public AuthService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public bool IsValidLogin(LoginDTO loginDTO)
        {
            loginDTO.Password = SHA.Encode(loginDTO.Password);
            var isUserExists = _userRepo.Get(x => x.u_account == loginDTO.Account && x.u_password == loginDTO.Password);

            if(isUserExists == null)
            {
                ValidationDictionary.AddError("message", $"請確認使用者/密碼是否正確。");
                return false;
            }

            return true;
        }

        public User GetUserByAccount(UserDTO userDTO)
        {
            return _userRepo.Get(x => x.u_account == userDTO.Account);
        }
    }
}
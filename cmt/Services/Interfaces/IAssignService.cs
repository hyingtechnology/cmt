using cmt.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmt.Services.Interfaces
{
    public interface IAssignService
    {

        string setRole(RoleEditVM models, string CreateUserName);


        string setAgent(AgentEditVM models, string CreateUserName);

        List<assign_RoleModel> getRoleAll();

        List<assign_AgentModel> getAgentAll();


        List<assign_AgentModel> getAgent(string assign_UserId);

        List<assign_RoleModel> getRole(string assign_UserId);
    }
}

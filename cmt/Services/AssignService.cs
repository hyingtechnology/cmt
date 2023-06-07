using cmt.Areas.Admin.ViewModels;
using cmt.Services.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace cmt.Services
{
    public class AssignService : IAssignService
    {
        string api = ConfigurationManager.AppSettings["MemberAPI"];
        public string setRole(RoleEditVM models,string CreateUserName)
        {
            assign_RoleModel assign_RoleModel = new assign_RoleModel();
            assign_RoleModel.assign_UserId = models.UserGuid;
            assign_RoleModel.assign_Role = models.roles ?? string.Empty;
            assign_RoleModel.assign_CreateTime = DateTime.Now;
            assign_RoleModel.assign_Guid = Guid.NewGuid().ToString();
            assign_RoleModel.assign_CreateUserId = CreateUserName;
            assign_RoleModel.assign_EditTime = DateTime.Now;
            assign_RoleModel.assign_EditUserId = CreateUserName;

            var client = new RestClient(api + "setRole");
            var request = new RestRequest();
            request.Method = Method.Post;

            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(assign_RoleModel);
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            return Content;


        }

        public string setAgent(AgentEditVM models, string CreateUserName)
        {
            assign_AgentModel assign_RoleModel = new assign_AgentModel();
            assign_RoleModel.assign_UserId = models.UserGuid;
            assign_RoleModel.assign_AgentUserId = models.AgentUserId ?? string.Empty;
            assign_RoleModel.assign_CreateTime = DateTime.Now;
            assign_RoleModel.assign_Agent_Guid = Guid.NewGuid().ToString();
            assign_RoleModel.assign_CreateUserId = CreateUserName;
            assign_RoleModel.assign_EditTime = DateTime.Now;
            assign_RoleModel.assign_EditUserId = CreateUserName;

            var client = new RestClient(api + "setAgent");
            var request = new RestRequest();
            request.Method = Method.Post;

            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(assign_RoleModel);
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            return Content;


        }

        public List<assign_RoleModel> getRoleAll()
        {

            var client = new RestClient(api + "getRoleAll");
            var request = new RestRequest();
            request.Method = Method.Post;
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            var jresult = JsonConvert.DeserializeObject<List<assign_RoleModel>>(Content.ToString());
            return jresult;
        }

        

        public List<assign_AgentModel> getAgentAll()
        {

            var client = new RestClient(api + "getAgentAll");
            var request = new RestRequest();
            request.Method = Method.Post;
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            var jresult = JsonConvert.DeserializeObject<List<assign_AgentModel>>(Content.ToString());
            return jresult;
        }

        public List<assign_AgentModel> getAgent(string assign_UserId)
        {

            var client = new RestClient(api + $"getAgent/{assign_UserId}");
            var request = new RestRequest();
            request.Method = Method.Post;
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            var jresult = JsonConvert.DeserializeObject<List<assign_AgentModel>>(Content.ToString());
            return jresult;
        }

        public List<assign_RoleModel> getRole(string assign_UserId)
        {

            var client = new RestClient(api + $"getRole/{assign_UserId}");
            var request = new RestRequest();
            request.Method = Method.Post;
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            var jresult = JsonConvert.DeserializeObject<List<assign_RoleModel>>(Content.ToString());
            return jresult;
        }
    }
}
using cmt.Services.Interfaces;
using cmt.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace cmt.Services
{
    public class AccountService: IAccountService
    {
        string api = ConfigurationManager.AppSettings["MemberAPI"];
         public List<UserLoginVM> GetUserAll()
        {

            var client = new RestClient(api+"UserAll");
            var request = new RestRequest();
            request.Method = Method.Post;
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            var jresult = JsonConvert.DeserializeObject<List<UserLoginVM>>(Content.ToString());
            return jresult;
        }

        public List<AssignMemberVM> GetAssignRole(string UsersId)
        {

            var client = new RestClient(api + "getAssign/"+ UsersId);
            var request = new RestRequest();
            request.Method = Method.Post;
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            var jresult = JsonConvert.DeserializeObject<List<AssignMemberVM>>(Content.ToString());
            return jresult;
        }
        public List<string> GetAuthorityUserPage(string Authority_UserId,string Authority_WebSitePageUrl)
        {
            IdentityUserVM identityUser = new IdentityUserVM();
            identityUser.Authority_UserId = Authority_UserId;
            identityUser.Authority_WebSitePageUrl = Authority_WebSitePageUrl;

            var client = new RestClient(api + "getAuthorityUserPage");
            var request = new RestRequest();
            request.Method = Method.Post;

            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(identityUser);

            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            var jresult = JsonConvert.DeserializeObject<List<string>>(Content.ToString());
            return jresult;
        }
        public UserLoginVM GetUser(string guid)
        {

            var client = new RestClient(api + "GetUser/" + guid);
            var request = new RestRequest();
            request.Method = Method.Post;
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            var jresult = JsonConvert.DeserializeObject<UserLoginVM>(Content.ToString());
            return jresult;
        }

        public string DeleteUser(string guid)
        {

            var client = new RestClient(api + "DeleteUser/" + guid);
            var request = new RestRequest();
            request.Method = Method.Post;
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            return Content;
        }

        public string UpdateUser(UserLoginVM models)
        {
            
            
            var json = JsonConvert.SerializeObject(models);
            var client = new RestClient(api + "UpdateUser");
            var request = new RestRequest();
            request.Method = Method.Post;
            
            request.AddHeader("content-type", "application/json");
            request.AddJsonBody(models);
            var response = client.ExecuteAsync(request);
            var result = response.Result;
            var Content = result.Content;
            return Content;
        }
    }
}
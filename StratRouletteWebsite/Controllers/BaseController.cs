using RestSharp;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace StratRouletteWebsite.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            if (!string.IsNullOrEmpty(Session?["Token"]?.ToString()))
            {
                ViewBag.Token = Session["Token"];
            }

            ViewBag.Games = GetGames();
        }

        public string RestCall(string requestFunction, Method method)
        {
            return Rest(requestFunction, method, null, null);
        }

        public string RestCall(string requestFunction, Method method, List<Parameter> parameters)
        {
            return Rest(requestFunction, method, parameters);
        }

        public string RestCall(string requestFunction, Method method, object body)
        {
            return Rest(requestFunction, method, null, body);
        }

        public string Rest(string requestFunction, Method method, List<Parameter> parameters = null, object body = null)
        {
            var api_url = ConfigurationSettings.AppSettings["api_url"];
            var api_key = Session?["Token"]?.ToString();

            var client = new RestClient(api_url);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            if (parameters == null)
            {
                parameters = new List<Parameter>();
            }

            var request = new RestRequest(requestFunction, method);

            foreach (var parameter in parameters)
            {
                request.AddParameter(parameter.Name, parameter.Value);
            }

            if (body != null)
            {
                request.AddJsonBody(body);
            }


            // easily add HTTP Headers
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer " + api_key);

            // add files to upload (works with compatible verbs)
            // request.AddFile(path);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            return content;
        }

        public List<SelectListItem> GetGames()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Siege", Value = "Tom Clancy's Rainbow Six: Siege"},
                new SelectListItem { Text = "CS:GO", Value = "Counter-Strike: Global Offensive"}
            };
        }

        public List<SelectListItem> GetSides()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Attack", Value = "Attack"},
                new SelectListItem { Text = "Defend", Value = "Defend"},
                new SelectListItem { Text = "Both", Value = "Both"}
            };
        }

        public List<SelectListItem> GetDifficulties()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Easy", Value = "Easy"},
                new SelectListItem { Text = "Medium", Value = "Medium"},
                new SelectListItem { Text = "Hard", Value = "Hard"}
            };
        }

        public List<SelectListItem> GetLoadoutType()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Primary", Value = "Primary"},
                new SelectListItem { Text = "Secondary", Value = "Secondary"},
                new SelectListItem { Text = "Gadget", Value = "Gadget"}
            };
        }

        public List<SelectListItem> GetVideoTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Youtube", Value = "Youtube"},
                new SelectListItem { Text = "Twitch", Value = "Twitch"}
            };
        }
    }
}
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ModelLibrary;
using Business;

namespace RestAPI.Controllers
{
	[Authorize]
    public class OperatorController : ApiController
    {
        // GET api/operator
        [Authorize(Roles = "Developer")]
        public HttpResponseMessage Get(string side, string excludedOperators = "", int numberOfOperators = 1)
		{
			side = side?.ToLower();

			//Define aliases for side
			switch (side)
			{
				case "def":
				case "defend":
					side = "defend";
					break;
				case "attack":
				case "atk":
					side = "attack";
					break;
				default:
					return Request.CreateResponse(HttpStatusCode.BadRequest, "Please pick a side."); 
			} 

			var operators = Operators.GetStoredProcedure(side, excludedOperators, numberOfOperators);
			return Request.CreateResponse(HttpStatusCode.OK, operators);
        }
        // GET api/operator
        [Authorize(Roles = "AdminDeveloper")]
        public HttpResponseMessage Post(string side, string excludedOperators = "", int numberOfOperators = 1)
        {
            side = side?.ToLower();

            //Define aliases for side
            switch (side)
            {
                case "def":
                case "defend":
                    side = "defend";
                    break;
                case "attack":
                case "atk":
                    side = "attack";
                    break;
                default:
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Please pick a side.");
            }

            var operators = Operators.GetStoredProcedure(side, excludedOperators, numberOfOperators);
            return Request.CreateResponse(HttpStatusCode.OK, operators);
        }
    }
}

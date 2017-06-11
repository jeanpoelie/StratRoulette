using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using ModelLibrary;
using ModelLibrary.Models;
using Business;

namespace RestAPI.Controllers
{
    [Authorize]
    public class ChallengeController : ApiController
    {
        // GET api/challenge
        [Authorize(Roles = "AdminDeveloper, Developer")]
        public HttpResponseMessage Get(string gameName, string side = null, string difficulty = null)

		{
			gameName = gameName?.ToLower();
			side = side?.ToLower();
			difficulty = difficulty?.ToLower();

			//define aliases for gameName
			switch (gameName)
			{
				case "siege":
				case "r6":
					gameName = "Tom Clancy\'s Rainbow Six: Siege";
					break;
				case "csgo":
				case "cs:go":
					gameName = "Counter-Strike: Global Offensive";
					break;
				default:
					return Request.CreateResponse(HttpStatusCode.BadRequest, "Please use a known game, for example: cs:go or siege");
			}

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
					side = null;
					break;
			}

			//Define aliases for difficulty
			switch (difficulty)
			{
				case "ez":
				case "easy":
					difficulty = "easy";
					break;
				case "med":
				case "medium":
					difficulty = "medium";
					break;
				case "hard":
					difficulty = "hard";
					break;
				default:
					difficulty = null;
					break;
			}

			var challenge = Challenges.GetStoredProcedure(gameName, side, difficulty);
			return Request.CreateResponse(HttpStatusCode.OK, challenge);
		}

        // POST api/challenge
        [Authorize(Roles = "AdminDeveloper")]
        public HttpResponseMessage Post([FromBody]string value)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "not made yet");
        }

        // POST api/challenge
        [Authorize(Roles = "AdminDeveloper")]
        public HttpResponseMessage Patch([FromBody]string value)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "not made yet");
        }
    }
}

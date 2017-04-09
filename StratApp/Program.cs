using Discord;	
using System;
using System.Threading.Tasks;	  
using RestSharp;
using ModelLibrary.Models;
using Newtonsoft.Json;
using System.Configuration;	  
using Discord.WebSocket;
using System.Collections.Generic;
using Discord.Net.Providers.WS4Net;
using Discord.Net.Providers.UDPClient;

namespace StratApp
{
	class Program
    {
		public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{		
			var botToken = ConfigurationSettings.AppSettings["bot_token"].ToString();

			var client = new DiscordSocketClient(new DiscordSocketConfig
			{
				WebSocketProvider = WS4NetProvider.Instance,
				UdpSocketProvider = UDPClientProvider.Instance
			});

			client.MessageReceived += MessageReceived;

			await client.LoginAsync(TokenType.Bot, botToken);
			await client.StartAsync();
			//await client.SetGameAsync("StratRoulette", "http://stratroulette.dakpangames.com");

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}

		private async Task MessageReceived(SocketMessage message)
		{
			var botClientId = ConfigurationSettings.AppSettings["bot_clientid"].ToString();
			var defaultCommand = ConfigurationSettings.AppSettings["bot_prefixmessage"].ToString();

			if(message.Author.IsBot)
			{
				return;
			}		

			var msgList = message.Content.Split(' ');

			if (msgList[0] == defaultCommand || message.Content == defaultCommand)
			{						  
				var gameName = "";
				var side = "";
				var difficulty = "";

				if (msgList.Length == 1)
				{
					await message.Channel.SendMessageAsync("I do not recognize this command, please use `!strat help`");
					return;
				}

				if (msgList[1] == "invite")
				{
					await message.Channel.SendMessageAsync("You can invite me here: https://discordapp.com/oauth2/authorize?client_id=" + botClientId + "&scope=bot&permissions=0");
					return;
				}

				if (msgList[1] == "help")
				{
					await message.Channel.SendMessageAsync("The Commands: " + Environment.NewLine +
											"`" + defaultCommand + " [GameName] [Side] [Difficulty]` example: `" + defaultCommand + "  r6 atk ez`" + Environment.NewLine +
											"`" + defaultCommand + "  invite` for a bot invite link.");
					return;
				}

				switch (msgList[1])
				{
					case "siege":
					case "r6":
					case "rainbow6":
						gameName = "siege";
						break;
					case "cs:go":
					case "csgo":
						gameName = "cs:go";
						break;
					default:
						await message.Channel.SendMessageAsync("Please pick a known game like; siege, cs:go. example: `" + defaultCommand + " [gamename] [side] [difficulty]`");
						return;
				}

				if (!string.IsNullOrEmpty(gameName))
				{
					if (msgList.Length > 2)
					{
						switch (msgList[2])
						{
							case "a":
							case "att":
							case "atk":
							case "attack":
							case "t"://terrorist cs:go
								side = "attack";
								break;
							case "d":
							case "def":
							case "defend":
							case "ct"://counter terrorist cs:go
								side = "defend";
								break;
							default:			  
								await message.Channel.SendMessageAsync("Please pick a known side like; def, atk. example: `" + defaultCommand + " [gamename] [side] [difficulty]`");
								return;
						}
					}

					if (msgList.Length > 3)
					{
						switch (msgList[3])
						{
							case "e":
							case "ez":
							case "easy":
								difficulty = "easy";
								break;
							case "m":
							case "med":
							case "medium":
								difficulty = "medium";
								break;
							case "h":
							case "hard":
								difficulty = "hard";
								break;
							default:				
								await message.Channel.SendMessageAsync("Please pick a known difficulty like; ez, med, hard. example: `" + defaultCommand + " [gamename] [side] [difficulty]`");
								return;
						}
					}  

					var stratModel = GetStrat(gameName, side, difficulty);
					var embed = new EmbedBuilder
					{
						Author = new EmbedAuthorBuilder()
						{
							Url = message.Author.GetAvatarUrl(),
							Name = message.Author.Username + "#" + message.Author.Discriminator.ToString().PadLeft(4, '0')
						},

						// Integer color for our Embed. We'll use a nice dark red color here.
						Color = (side.ToLower() == "attack" ? new Color(0x0000ff) : new Color(0xff8000)),

						// Our embed's title
						Title = stratModel.Title,

						// Our embed's description
						Description = stratModel.Description,  

						// Our embed's footer.
						Footer = new EmbedFooterBuilder()
						{
							//TODO get ICON from database?
							Text = "Credits: " + (!string.IsNullOrEmpty(stratModel.Credits) ? stratModel.Credits : "Dakpan"),
							//IconUrl = "https://s-media-cache-ak0.pinimg.com/564x/01/25/dc/0125dc547e4f43d6493aca279b464895.jpg"
						},

						// Our embed's image.
						//Image = new DiscordEmbedImage()
						//{
						//	Url = "https://s-media-cache-ak0.pinimg.com/564x/01/25/dc/0125dc547e4f43d6493aca279b464895.jpg"
						//},

						// Our embed's thumbnail
						//Thumbnail = new DiscordEmbedThumbnail()
						//{
						//	Url = "https://s-media-cache-ak0.pinimg.com/564x/01/25/dc/0125dc547e4f43d6493aca279b464895.jpg"
						//},

						// A timestamp you want to add
						//Timestamp = DateTime.UtcNow,

						// Link URL for our embed
						//TODO get URL from database.
						Url = "http://stratroulette.dakpangames.com/" + stratModel.GameName.Replace(" ", "").Replace("-", "").Replace(":", "") + "/ChallengeInformation/" + stratModel.Id,
					};

					embed.AddField(
						new EmbedFieldBuilder()
						{
							Name = "Strat Legend",
							Value = "Side: " + stratModel.Side + Environment.NewLine +
											"Difficulty: " + stratModel.Difficulty + Environment.NewLine +
											"Likes: " + stratModel.Likes + Environment.NewLine +
											"Reports: " + stratModel.Reports + Environment.NewLine +
											"Keyboard & Mouse: " + (stratModel.KeyboardAndMouse == 1 ? "yes" : "no") + Environment.NewLine +
											"Controller: " + (stratModel.Controller == 1 ? "yes" : "no")
						});

					await message.Channel.SendMessageAsync("Thank you for using the StratBot, here is your strat, have fun!", false, embed);
					return;
				} 

				await message.Channel.SendMessageAsync("I do not recognize this command, please use `" + defaultCommand + " help`");
			}
		}

		public static ChallengeModel GetStrat(string gameName, string side = "", string difficulty = "")
        {
            var parameters = new List<Parameter>();

            if(string.IsNullOrEmpty(gameName))
            {
                return null;
            }

            parameters.Add(new Parameter() { Name = "gameName", Value = gameName });


            if(!string.IsNullOrEmpty(side))
            {
                parameters.Add(new Parameter() { Name = "side", Value = side });
            }

            if (!string.IsNullOrEmpty(difficulty))
            {
                parameters.Add(new Parameter() { Name = "difficulty", Value = difficulty });
            }

            var result = RestCall("Challenge", Method.GET, parameters);	
			var model = JsonConvert.DeserializeObject<ChallengeModel>(result);
            return model;
        }

        public static string RestCall(string requestFunction, Method method, List<Parameter> parameters)
        {
            var api_key = ConfigurationSettings.AppSettings["api_key"].ToString();
            var api_url = ConfigurationSettings.AppSettings["api_url"].ToString();
            var client = new RestClient(api_url);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            var request = new RestRequest(requestFunction, method);
            
            foreach(var parameter in parameters)
            {
                request.AddParameter(parameter.Name, parameter.Value);
            }
            //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
            //request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

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
	}
}

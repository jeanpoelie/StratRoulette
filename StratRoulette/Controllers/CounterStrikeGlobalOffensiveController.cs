// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CounterStrikeGlobalOffensiveController.cs" company="">
//   
// </copyright>
// <summary>
//   The counter strike global offensive controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace StratRoulette.Controllers
{
	using System;
	using System.Collections.Generic;

	using AutoMapper;

    using Business;
    using ModelLibrary;

	using Models;

	/// <summary>
	/// The counter strike global offensive controller.
	/// </summary>
	public class CounterStrikeGlobalOffensiveController : BaseController
	{
		/// <summary>
		/// The name of the game (controller) this is used to find stuff like statistics, challenges etc.
		/// </summary>
		private const string GameName = "Counter-Strike: Global Offensive";

		/// <summary>
		/// Initialization of the controller
		/// </summary>
		public CounterStrikeGlobalOffensiveController()
		{
			
		}

		/// <summary>
		/// The homepage view
		/// </summary>
		/// <returns>The view</returns>
		public ActionResult Index()
		{
			var numberOfChallengesToday = Statistics.GetNumberOfChallengesSinceDate(1, 0, GameName);
			var lineChartModel = new LineChartModel
			{
				PlaysToday = numberOfChallengesToday,
				PlaysEachWeek = Statistics.GetNumberOfWeeklyChallengesSinceStart(0, GameName),
				HeaderText = "The chart below is for the challenges that have been played each week.",
				FooterText = "The total number of challenges played today is: " + numberOfChallengesToday
			};
			lineChartModel.DefineLabels();

			ViewBag.LineChartModel = lineChartModel;
			ViewBag.TopPlayedChallenges = Mapper.Map<List<StatisticModel>>(Statistics.GetTopPlayedChallenges(5, GameName));
			ViewBag.TopLikeChallenges = Mapper.Map<List<StatisticModel>>(Statistics.GetTopLiked(5, GameName));
			ViewBag.TopPlayDates = Mapper.Map<List<StatisticModel>>(Statistics.GetTopPlayDays(5, GameName));
			return View();
		}

		/// <summary>
		/// Initial selection of a random challenge based on no filters.
		/// </summary>
		/// <returns>The View with the model</returns>
		public ActionResult RandomChallenge()
		{
			var model = new ChallengeModel();

			try
			{
				model = Mapper.Map<ChallengeModel>(Challenges.GetRandom(null, null, null, null, new List<int>(), GameName));
				Statistics.AddRandomChallenge(null, null, null, model.Id);
			}
			catch (Exception ex)
			{
				General.AddLog(ex);
			}

			if (model != null)
			{
				var numberOfChallengesToday = Statistics.GetNumberOfChallengesSinceDate(1, model.Id, GameName);
				var lineChartModel = new LineChartModel
				{
					PlaysToday = numberOfChallengesToday,
					PlaysEachWeek = Statistics.GetNumberOfWeeklyChallengesSinceStart(model.Id, GameName),
					HeaderText = "The chart below is for the number of times this challenge has been played, each week.",
					FooterText = "This challenge has been played " + numberOfChallengesToday + " times today."
				};
				lineChartModel.DefineLabels();
				ViewBag.LineChartModel = lineChartModel;
			}

			ViewBag.SelectedGameModeId = 0;
			ViewBag.SelectedMapIds = new List<int>();
			ViewBag.Maps = Mapper.Map<List<MapModel>>(General.GetAllMaps(GameName));
			ViewBag.GameModes = Mapper.Map<List<GameModeModel>>(General.GetAllGameModes(GameName));
			ViewBag.Difficulty = "All";
			ViewBag.Peripheral = "Both";

			return View(model);
		}

		/// <summary>
		/// When a players select to randomize the challenges this function will be called with filters etc.
		/// </summary>
		/// <param name="difficulty">The difficulty of the challenge, Easy, Medium, Hard or All.</param>
		/// <param name="side">Which side the challenge is for, attacking or defending.</param>
		/// <param name="peripheral">Which peripherals can be used, keyboard/controller</param>
		/// <param name="gameModeId">The game mode of the game (depending on the game itself)</param>
		/// <param name="maps">The maps of the game (Depending on the game itself)</param>
		/// <returns>The view with the model</returns>
		[HttpPost]
		public ActionResult RandomChallenge(string difficulty, string side, string peripheral, int? gameModeId, List<int> maps)
		{
			var model = new ChallengeModel();

			maps = maps ?? new List<int>();

			try
			{
				model = Mapper.Map<ChallengeModel>(Challenges.GetRandom(side, difficulty, peripheral, gameModeId, maps, GameName));
				Statistics.AddRandomChallenge(difficulty, side, peripheral, model.Id);
			}
			catch (Exception ex)
			{
				General.AddLog(ex);
			}

			var numberOfChallengesToday = Statistics.GetNumberOfChallengesSinceDate(1, model.Id, GameName);
			var lineChartModel = new LineChartModel
			{
				PlaysToday = numberOfChallengesToday,
				PlaysEachWeek = Statistics.GetNumberOfWeeklyChallengesSinceStart(model.Id, GameName),
				HeaderText = "The chart below is for the number of times this challenge has been played, each week.",
				FooterText = "This challenge has been played " + numberOfChallengesToday + " times today."
			};
			lineChartModel.DefineLabels();

			ViewBag.LineChartModel = lineChartModel;
			ViewBag.SelectedMapIds = maps;
			ViewBag.SelectedGameModeId = gameModeId;
			ViewBag.Maps = Mapper.Map<List<MapModel>>(General.GetAllMaps(GameName));
			ViewBag.GameModes = Mapper.Map<List<GameModeModel>>(General.GetAllGameModes(GameName));
			ViewBag.Difficulty = difficulty;
			ViewBag.Peripheral = peripheral;

			return View(model);
		}

		/// <summary>
		/// This function will return a list of all the challenges with only the game as filter to the view!
		/// </summary>
		/// <returns>Return the challenge list view to the client</returns>
		public ActionResult ChallengeList()
		{
			var model = Mapper.Map<List<ChallengeModel>>(Challenges.GetAll(GameName));

			var numberOfChallengesToday = Statistics.GetNumberOfChallengesSinceDate(1, 0, GameName);
			var lineChartModel = new LineChartModel
			{
				PlaysToday = numberOfChallengesToday,
				PlaysEachWeek = Statistics.GetNumberOfWeeklyChallengesSinceStart(0, GameName),
				HeaderText = "The chart below is for the challenges that have been played each week.",
				FooterText = "The total number of challenges played today is: " + numberOfChallengesToday
			};

			ViewBag.LineChartModel = lineChartModel;
			ViewBag.TopPlayedChallenges = Mapper.Map<List<StatisticModel>>(Statistics.GetTopPlayedChallenges(5, GameName));
			ViewBag.TopLikeChallenges = Mapper.Map<List<StatisticModel>>(Statistics.GetTopLiked(5, GameName));
			ViewBag.TopPlayDates = Mapper.Map<List<StatisticModel>>(Statistics.GetTopPlayDays(5, GameName));

			return View(model);
		}

		/// <summary>
		/// This will get information for the challenge with a certain ID and the stats of a certain game.
		/// </summary>
		/// <param name="id">The ID of the challenge</param>
		/// <returns>Returns the challenge information view with the model</returns>
		public ActionResult ChallengeInformation(int id = 0)
		{
			var model = new ChallengeModel();

			try
			{
				model = Mapper.Map<ChallengeModel>(Challenges.Get(id));
			}
			catch (Exception ex)
			{
				General.AddLog(ex);
			}

			if (model != null)
			{ 
				var numberOfChallengesToday = Statistics.GetNumberOfChallengesSinceDate(1, model.Id, GameName);
				var lineChartModel = new LineChartModel
				{
					PlaysToday = numberOfChallengesToday,
					PlaysEachWeek = Statistics.GetNumberOfWeeklyChallengesSinceStart(model.Id, GameName),
					HeaderText = "The chart below is for the number of times this challenge has been played, each week.",
					FooterText = "This challenge has been played " + numberOfChallengesToday + " times today."
				};
				lineChartModel.DefineLabels();
				ViewBag.LineChartModel = lineChartModel;
				ViewBag.Maps = Mapper.Map<List<MapModel>>(Challenges.GetMaps(model.Title));
				ViewBag.GameModes = Mapper.Map<List<GameModeModel>>(Challenges.GetGameModes(model.Title));
			} 

			return View(model);
		}

		/// <summary>
		/// When someone presses the Thumbs up or Report button this will add feedback.
		/// </summary>
		/// <param name="ip">The IP of the person who voted.</param>
		/// <param name="challengeId">The ID of the challenge</param>
		/// <param name="liked">Whether its a vote</param>
		/// <param name="reported">or a report</param>
		/// <returns>Return the response message</returns>
		[HttpGet] 
		public string AddFeedBack(string ip, int challengeId, string liked, string reported)
		{
			var responseMessage = string.Empty;

			if (ip == "127.0.0.1" || ip == "::1")
			{
				return "Local IPS are not allowed to vote.";
			}

			if (!string.IsNullOrEmpty(liked))
			{
				responseMessage = Challenges.AddFeedback(ip, challengeId, FeedbackType.Like, GameName);
			}

			if (!string.IsNullOrEmpty(reported))
			{
				responseMessage = Challenges.AddFeedback(ip, challengeId, FeedbackType.Report, GameName);
			}

			return responseMessage;
		}
	}
}
using System.Web.Mvc;

namespace StratRoulette.Controllers
{
	using System.Collections.Generic;

	using AutoMapper;

	using ModelLibrary;

	using Models;

	public class HomeController : BaseController
	{
		public ActionResult Index()
		{
			//var numberOfChallengesToday = Statistics.GetNumberOfChallengesSinceDate(1);
			//var lineChartModel = new LineChartModel
			//{
			//	PlaysToday = numberOfChallengesToday,
			//	PlaysEachWeek = Statistics.GetNumberOfWeeklyChallengesSinceStart(),
			//	HeaderText = "The chart below is for the challenges that have been played each week.",
			//	FooterText = "The total number of challenges played today is: " + numberOfChallengesToday
			//};
			//lineChartModel.DefineLabels();

			//ViewBag.LineChartModel = lineChartModel;
			ViewBag.TopPlayedChallenges = Mapper.Map<List<StatisticModel>>(Statistics.GetTopPlayedChallenges(5));
			ViewBag.TopLikeChallenges = Mapper.Map<List<StatisticModel>>(Statistics.GetTopLiked(5));
			ViewBag.TopPlayDates = Mapper.Map<List<StatisticModel>>(Statistics.GetTopPlayDays(5));

			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Contribute()
		{
			return View();
		}

		public ActionResult InAction()
		{
			var model = Mapper.Map<List<VideoModel>>(General.GetAllVideos());
			return View(model);
		}

		public ActionResult Contact()
		{
			return View();
		}

		public ActionResult FAQ()
		{
			return View();
		}

		public ActionResult News()
		{
			return View();
		}

		public ActionResult Guestbook()
		{
			return View();
		}
	}
}
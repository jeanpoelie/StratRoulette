using System.Web.Mvc;

namespace MvcAreasApplication.Areas.Manage.Controllers
{
	using System.Collections.Generic;

	using AutoMapper;

	using global::StratRoulette.Controllers;
	using global::StratRoulette.Models;

	/// <summary>
	/// The management controller.
	/// </summary>
	public class ManagementController : BaseController
	{
		/// <summary>
		/// The index.
		/// </summary>
		/// <returns>
		/// The <see cref="ActionResult"/>.
		/// </returns>
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Login(string username, string password)
		{
			

			return View();
		}

		public ActionResult Challenges(string gameName = "")
		{
			var model = new List<ChallengeModel>();
			if (!string.IsNullOrEmpty(gameName))
			{
				model = Mapper.Map<List<ChallengeModel>>(Business.Challenges.GetAll(gameName));
			}

			return View(model);
		}

		public ActionResult Challenge(int modelId)
		{
			var model = Mapper.Map<ChallengeModel>(Business.Challenges.Get(modelId));
			ViewBag.Maps = Mapper.Map<List<MapModel>>(Business.Challenges.GetMaps(model.Title));
			ViewBag.GameModes = Mapper.Map<List<GameModeModel>>(Business.Challenges.GetGameModes(model.Title));
			return View(model);
		}

		[HttpPost]
		public ActionResult Challenge(ChallengeModel model)
		{

			ViewBag.Maps = Mapper.Map<List<MapModel>>(Business.Challenges.GetMaps(model.Title));
			ViewBag.GameModes = Mapper.Map<List<GameModeModel>>(Business.Challenges.GetGameModes(model.Title));
			return View(model);
		}
	}
}
using System.Web.Mvc;

namespace StratRoulette.Controllers
{
	using System.Collections.Generic;

	using AutoMapper;

	using ModelLibrary;

	using Models;

	public class ErrorController : BaseController
	{
		public ActionResult UnHandled()
		{
			return View();
		}

		public ActionResult Maintenance()
		{
			return View();
		}
	}
}
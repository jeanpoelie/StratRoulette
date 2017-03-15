using System.Web.Mvc;

namespace StratRoulette.Controllers
{
	using System.Collections.Generic;

	using AutoMapper;

	using Business;

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelLibrary.Models;

namespace StratRouletteCMS.Controllers
{
	public class ChallengeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Add()
		{
			return View();
		}

        [HttpPost]
        public ActionResult Add(ChallengeModel model)
        {


            return View(model);
        }

        public ActionResult Edit()
		{
			return View();
        }

        [HttpPost]
        public ActionResult Edit(ChallengeModel model)
        {


            return View(model);
        }
    }
}
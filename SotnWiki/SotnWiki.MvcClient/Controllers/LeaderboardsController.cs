﻿using Bytes2you.Validation;
using SotnWiki.DataServices.Contracts;
using SotnWiki.Models;
using SotnWiki.MvcClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SotnWiki.MvcClient.Controllers
{
    public class LeaderboardsController : Controller
    {
        private readonly IRunService runService;

        public LeaderboardsController(IRunService runService)
        {
            Guard.WhenArgument(runService, nameof(IRunService)).IsNull().Throw();

            this.runService = runService;
        }

        public ActionResult CvSpeedrunsArchive()
        {
            var allCvsRuns = this.runService.GetCvsRuns();
            var alucardAny = allCvsRuns.Where(r => r.Category == SotnWiki.Models.Category.CvsAlucardAnyNSC).ToList();

            var model = new LeaderboardViewModel();
            model.AlucardAnyNSC = alucardAny;

            return View(model);
        }

        public ActionResult Current()
        {
            var allSrcRuns = this.runService.GetSrComRuns();
            var alucardAny = allSrcRuns.Where(r => r.Category == SotnWiki.Models.Category.AlucardAnyNSC).ToList();

            var model = new LeaderboardViewModel();
            model.AlucardAnyNSC = alucardAny;

            return View(model);
        }
    }
}
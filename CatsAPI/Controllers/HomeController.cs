using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Models;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using AdminUnitType = Cats.Rest.Models.AdminUnitType;

namespace Cats.Rest.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using BorderCrossInfo.Services;
using BorderCrossInfo.Models;

namespace BorderCrossInfo.Controllers
{
    public class HomeController : Controller
    {
        private GranicniPrelazService _service = new GranicniPrelazService();
        public ActionResult Index()
        {
            var podaci = _service.GetAll()
                             .OrderByDescending(x => x.VremeCekanjaMinuta)
                             .Take(3)
                             .ToList();

            return View(podaci);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Statistika()
        {
            var podaci = _service.GetAll();

            ViewBag.Ukupno = podaci.Count;
            ViewBag.Prosek = podaci.Any() ? podaci.Average(p => p.VremeCekanjaMinuta) : 0;
            ViewBag.Najduze = podaci.Any() ? podaci.Max(p => p.VremeCekanjaMinuta) : 0;

            var poDrzavi = podaci.GroupBy(p => p.Drzava)
                                 .Select(g => new { Drzava = g.Key, Broj = g.Count() })
                                 .ToDictionary(x => x.Drzava, x => x.Broj);

            return View(poDrzavi);
        }
    }
}
using BorderCrossInfo.Models;
using BorderCrossInfo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BorderCrossInfo.Controllers
{
    public class KatalogController : Controller
    {
        private GranicniPrelazService _service = new GranicniPrelazService();

        public ActionResult Index(string searchString, string drzavaFilter, string sortOrder)
        {
            var prelazi = _service.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                prelazi = prelazi.Where(p => p.Naziv.ToLower().Contains(searchString.ToLower())).ToList();
            }

            if (!String.IsNullOrEmpty(drzavaFilter))
            {
                prelazi = prelazi.Where(p => p.Drzava == drzavaFilter).ToList();
            }

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TimeSortParm = sortOrder == "Time" ? "time_desc" : "Time";

            switch (sortOrder)
            {
                case "name_desc":
                    prelazi = prelazi.OrderByDescending(p => p.Naziv).ToList();
                    break;
                case "Time":
                    prelazi = prelazi.OrderBy(p => p.VremeCekanjaMinuta).ToList();
                    break;
                case "time_desc":
                    prelazi = prelazi.OrderByDescending(p => p.VremeCekanjaMinuta).ToList();
                    break;
                default:
                    prelazi = prelazi.OrderBy(p => p.Naziv).ToList();
                    break;
            }

            ViewBag.Drzave = new SelectList(prelazi.Select(p => p.Drzava).Distinct());

            return View(prelazi);
        }
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GranicniPrelaz prelaz)
        {
            if (ModelState.IsValid) 
            {
                _service.Add(prelaz);
                return RedirectToAction("Index");
            }

            return View(prelaz); 
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var prelaz = _service.GetById(id);
            if (prelaz == null) return HttpNotFound();

            return View(prelaz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GranicniPrelaz prelaz)
        {
            if (ModelState.IsValid)
            {
                _service.Update(prelaz);
                return RedirectToAction("Index");
            }
            return View(prelaz);
        }
        public JsonResult ExportJson()
        {
            var podaci = _service.GetAll();
            return Json(podaci, JsonRequestBehavior.AllowGet);
        }

        public FileResult ExportXml()
        {
            string putanja = Server.MapPath("~/App_Data/granice.xml");
            return File(putanja, "text/xml", "granicni_prelazi_export.xml");
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            var prelaz = _service.GetById(id);
            if (prelaz == null) return HttpNotFound();
            return View(prelaz);
        }
    }
}
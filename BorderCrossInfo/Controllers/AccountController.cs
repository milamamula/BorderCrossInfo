using BorderCrossInfo.Models;
using BorderCrossInfo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BorderCrossInfo.Controllers
{
    public class AccountController : Controller
    {
        private KorisnikService _service = new KorisnikService();

        public ActionResult Register() => View();

        [HttpPost]
        public ActionResult Register(Korisnik model)
        {
            if (ModelState.IsValid)
            {
                if (_service.Registruj(model))
                    return RedirectToAction("Login");

                ModelState.AddModelError("", "Korisničko ime je zauzeto.");
            }
            return View(model);
        }

        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var korisnik = _service.Prijava(username, password);
            if (korisnik != null)
            {
                FormsAuthentication.SetAuthCookie(korisnik.KorisnickoIme, false);
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "Pogrešno korisničko ime ili lozinka.";
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
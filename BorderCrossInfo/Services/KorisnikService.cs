using BorderCrossInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BorderCrossInfo.Services
{
    public class KorisnikService
    {
        private readonly string _putanja = HttpContext.Current.Server.MapPath("~/App_Data/korisnici.xml");

        public bool Registruj(Korisnik k)
        {
            XDocument doc = XDocument.Load(_putanja);

            if (doc.Descendants("Korisnik").Any(x => x.Element("KorisnickoIme").Value == k.KorisnickoIme))
                return false;

            int noviId = doc.Descendants("Korisnik").Any()
                ? doc.Descendants("Korisnik").Max(x => (int)x.Element("Id")) + 1 : 1;

            XElement noviKorisnik = new XElement("Korisnik",
                new XElement("Id", noviId),
                new XElement("KorisnickoIme", k.KorisnickoIme),
                new XElement("Lozinka", k.Lozinka), 
                new XElement("Uloga", "Admin")
            );

            doc.Root.Add(noviKorisnik);
            doc.Save(_putanja);
            return true;
        }

        public Korisnik Prijava(string username, string password)
        {
            XDocument doc = XDocument.Load(_putanja);
            var k = doc.Descendants("Korisnik")
                .FirstOrDefault(x => x.Element("KorisnickoIme").Value == username &&
                                     x.Element("Lozinka").Value == password);

            if (k == null) return null;

            return new Korisnik
            {
                KorisnickoIme = k.Element("KorisnickoIme").Value,
                Uloga = k.Element("Uloga").Value
            };
        }
    }
}

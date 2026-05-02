using BorderCrossInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BorderCrossInfo.Services
{
    public class GranicniPrelazService
    {
        private readonly string _putanja = HttpContext.Current.Server.MapPath("~/App_Data/granice.xml");

        public List<GranicniPrelaz> GetAll()
        {
            XDocument doc = XDocument.Load(_putanja);

            var lista = from p in doc.Descendants("Prelaz")
                        select new GranicniPrelaz
                        {
                            Id = (int)p.Element("Id"),
                            Naziv = (string)p.Element("Naziv"),
                            Drzava = (string)p.Element("Drzava"),
                            VremeCekanjaMinuta = (int)p.Element("VremeCekanjaMinuta"),
                            TipVozila = (string)p.Element("TipVozila"),
                            PoslednjeAzuriranje = DateTime.Parse(p.Element("PoslednjeAzuriranje").Value)
                        };

            return lista.ToList();
        }

        public bool Add(GranicniPrelaz prelaz)
        {
            XDocument doc = XDocument.Load(_putanja);

            bool postoji = doc.Descendants("Prelaz")
                              .Any(x => x.Element("Naziv").Value.ToLower() == prelaz.Naziv.ToLower());

            if (postoji) return false; 

            int noviId = doc.Descendants("Prelaz").Any()
                ? doc.Descendants("Prelaz").Max(x => (int)x.Element("Id")) + 1
                : 1;

            XElement noviElement = new XElement("Prelaz",
                new XElement("Id", noviId),
                new XElement("Naziv", prelaz.Naziv),
                new XElement("Drzava", prelaz.Drzava),
                new XElement("VremeCekanjaMinuta", prelaz.VremeCekanjaMinuta),
                new XElement("TipVozila", prelaz.TipVozila),
                new XElement("PoslednjeAzuriranje", DateTime.Now.ToString("s"))
            );

            doc.Root.Add(noviElement);
            doc.Save(_putanja);
            return true; 
        }
        public GranicniPrelaz GetById(int id)
        {
            XDocument doc = XDocument.Load(_putanja);
            var p = doc.Descendants("Prelaz").FirstOrDefault(x => (int)x.Element("Id") == id);

            if (p == null) return null;

            return new GranicniPrelaz
            {
                Id = (int)p.Element("Id"),
                Naziv = (string)p.Element("Naziv"),
                Drzava = (string)p.Element("Drzava"),
                VremeCekanjaMinuta = (int)p.Element("VremeCekanjaMinuta"),
                TipVozila = (string)p.Element("TipVozila"),
                PoslednjeAzuriranje = DateTime.Parse(p.Element("PoslednjeAzuriranje").Value)
            };
        }

        public void Update(GranicniPrelaz model)
        {
            XDocument doc = XDocument.Load(_putanja);
            XElement prelaz = doc.Descendants("Prelaz").FirstOrDefault(x => (int)x.Element("Id") == model.Id);

            if (prelaz != null)
            {
                prelaz.Element("Naziv").Value = model.Naziv;
                prelaz.Element("Drzava").Value = model.Drzava;
                prelaz.Element("VremeCekanjaMinuta").Value = model.VremeCekanjaMinuta.ToString();
                prelaz.Element("TipVozila").Value = model.TipVozila;
                prelaz.Element("PoslednjeAzuriranje").Value = DateTime.Now.ToString("s"); 

                doc.Save(_putanja);
            }
        }
        public void Delete(int id)
        {
            XDocument doc = XDocument.Load(_putanja);
            XElement prelaz = doc.Descendants("Prelaz").FirstOrDefault(x => (int)x.Element("Id") == id);
            if (prelaz != null)
            {
                prelaz.Remove();
                doc.Save(_putanja);
            }
        }
    }
}
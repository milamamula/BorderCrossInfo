using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BorderCrossInfo.Models
{
    public class GranicniPrelaz
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv graničnog prelaza je obavezan.")]
        [StringLength(50, ErrorMessage = "Naziv ne može biti duži od 50 karaktera.")]
        [Display(Name = "Naziv prelaza")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Morate uneti državu u kojoj se prelaz nalazi.")]
        [Display(Name = "Država")]
        public string Drzava { get; set; }

        [Required(ErrorMessage = "Unesite procenjeno vreme čekanja.")]
        [Range(0, 1000, ErrorMessage = "Vreme čekanja mora biti između 0 i 1000 minuta.")]
        [Display(Name = "Čekanje (min)")]
        public int VremeCekanjaMinuta { get; set; }

        [Required(ErrorMessage = "Odaberite tip saobraćaja.")]
        [Display(Name = "Tip vozila")]
        public string TipVozila { get; set; } 

        [Display(Name = "GPS Lokacija")]
        public string LokacijaUrl { get; set; } 

        [Display(Name = "Poslednje ažuriranje")]
        [DataType(DataType.DateTime)]
        public DateTime PoslednjeAzuriranje { get; set; }
        public string GetStatusColor()
        {
            if (VremeCekanjaMinuta < 15) return "success"; 
            if (VremeCekanjaMinuta < 60) return "warning"; 
            return "danger"; 
        }
    }
}
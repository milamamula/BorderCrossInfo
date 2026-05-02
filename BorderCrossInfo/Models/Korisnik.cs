using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BorderCrossInfo.Models
{
    public class Korisnik
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Korisničko ime je obavezno")]
        [Display(Name = "Korisničko ime")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Lozinka mora imati bar 6 karaktera")]
        public string Lozinka { get; set; }

        [Compare("Lozinka", ErrorMessage = "Lozinke se ne podudaraju")]
        [DataType(DataType.Password)]
        [Display(Name = "Potvrdi lozinku")]
        public string PotvrdaLozinke { get; set; }

        public string Uloga { get; set; } 
    }
}
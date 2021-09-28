using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsenProekt.Models
{
    public class TuristickiVodic
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Display(Name = "Ime")]
        public string Ime { get; set; }
        [StringLength(50)]
        [Display(Name = "Prezime")]
        public string Prezime { get; set; }
        public string FullName
        {
            get { return String.Format("{0} {1}", Ime, Prezime); }
        }
        public string Obrazovanie { get; set; }
        [Display(Name = "Poseti")]
        public ICollection<Poseta> Poseta { get; set; }
        [NotMapped]
        public string ProfilePicture { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsenProekt.Models
{
    public class Poseta
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Znamenitost { get; set; }
        public int Price { get; set; }
        public string Komentar { get; set; }
        [Display(Name = "Datum na Poseta")]
        [DataType(DataType.Date)]
        public DateTime? DatumPoseta { get; set; }
        [Display(Name = "Turisticki Vodic")]
        public int TuristickiVodicId { get; set; }

        [Display(Name = "Turisticki Vodic")]
        public TuristickiVodic TuristickiVodic { get; set; }
        public ICollection<VkluciSe> Turisti { get; set; }
    }
}

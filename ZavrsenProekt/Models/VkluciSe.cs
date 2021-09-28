using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsenProekt.Models
{
    public class VkluciSe
    {
        public int Id { get; set; }
        public int PosetaId { get; set; }
        public Poseta Poseta { get; set; }
        public int TouristId { get; set; }
        public Turist Turist { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Data na Plakjanje")]
        public DateTime PaymentDate { get; set; }
        public string PosetaUrl { get; set; }
        public string ZnamenitostUrl { get; set; }
        public string BrojKartica { get; set; }
        public string CVC { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data Validnost")]
        public DateTime ValidnostData { get; set; }
    }
}

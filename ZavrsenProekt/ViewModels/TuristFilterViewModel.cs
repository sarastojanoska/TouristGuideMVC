using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZavrsenProekt.Models;

namespace ZavrsenProekt.ViewModels
{
    public class TuristFilterViewModel
    {
        public IList<Turist> Turist { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string SearchString { get; set; }
    }
}

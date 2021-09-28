using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZavrsenProekt.Models;

namespace ZavrsenProekt.ViewModels
{
    public class VodicFilterViewModel
    {
        public IList<TuristickiVodic> TuristickiVodic { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string SearchString { get; set; }
        public SelectList Obrazovanie { get; set; }
        public string VodicObrazovanie { get; set; }
    }
}

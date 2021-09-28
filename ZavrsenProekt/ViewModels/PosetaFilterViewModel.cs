using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZavrsenProekt.Models;

namespace ZavrsenProekt.ViewModels
{
    public class PosetaFilterViewModel
    {
        public IList<Poseta> Poseti { get; set; }
        public SelectList Iminja { get; set; }
        public string ImePoseta { get; set; }
        public SelectList Znamenitosti { get; set; }
        public string ZnamenitostPoseta { get; set; }
        public string SearchString { get; set; }
        public string SearchString1 { get; set; }
    }
}

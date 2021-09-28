using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZavrsenProekt.Models;

namespace ZavrsenProekt.ViewModels
{
    public class PosetaTuristiEditViewModel
    {
        public Poseta Poseta { get; set; }
        public IEnumerable<int> SelectedTuristi { get; set; }
        public IEnumerable<SelectListItem> TuristiList { get; set; }
    }
}

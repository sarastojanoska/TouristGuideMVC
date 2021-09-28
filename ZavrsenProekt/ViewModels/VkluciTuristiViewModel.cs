using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZavrsenProekt.Models;

namespace ZavrsenProekt.ViewModels
{
    public class VkluciTuristiViewModel
    {
        public VkluciSe VkluciSe { get; set; }
        public IEnumerable<int> SelectedTourists { get; set; }
        public IEnumerable<SelectListItem> TouristList { get; set; }
    }
}

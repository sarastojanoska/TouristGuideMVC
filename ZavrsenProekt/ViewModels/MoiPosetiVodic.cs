using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZavrsenProekt.Models;

namespace ZavrsenProekt.ViewModels
{
    public class MoiPosetiVodic
    {
        public TuristickiVodic Vodic { get; set; }
        public IEnumerable<int> SelectedPoseti { get; set; }
        public IEnumerable<SelectListItem> PosetiList { get; set; }
    }
}

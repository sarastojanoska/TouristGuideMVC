using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZavrsenProekt.Models;

namespace ZavrsenProekt.ViewModels
{
    public class ProfilePicTurist
    {
        public Turist Turist { get; set; }
        public IFormFile ProfilePic { get; set; }
    }
}

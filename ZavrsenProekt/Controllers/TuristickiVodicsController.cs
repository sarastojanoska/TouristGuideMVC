using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZavrsenProekt.Data;
using ZavrsenProekt.Models;
using ZavrsenProekt.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ZavrsenProekt.Controllers
{
    public class TuristickiVodicsController : Controller
    {
        private readonly ZavrsenProektContext _context;
        private object webHostEnvironment;
        private IWebHostEnvironment WebHostEnvironment { get; }

        public TuristickiVodicsController(ZavrsenProektContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        // GET: TuristickiVodics
        public async Task<IActionResult> Index(string obrazovanie,string ime,string prezime)
        {
            IQueryable<TuristickiVodic> vodici = _context.TuristickiVodic.AsQueryable();
            IQueryable<string> obrazovanieQuery = _context.TuristickiVodic.OrderBy(m => m.Obrazovanie).Select(m => m.Obrazovanie).Distinct();
            if (!string.IsNullOrEmpty(ime))
            {
                vodici = vodici.Where(s => s.Ime.Contains(ime));
            }
            if (!string.IsNullOrEmpty(prezime))
            {
                vodici = vodici.Where(s => s.Prezime.Contains(prezime));
            }
            if (!string.IsNullOrEmpty(obrazovanie))
            {
                vodici = vodici.Where(x => x.Obrazovanie.Contains(obrazovanie));
            }
            vodici = vodici.Include(m => m.Poseta);
            var vodicFilter = new VodicFilterViewModel
            {
                Obrazovanie = new SelectList(await obrazovanieQuery.ToListAsync()),
                TuristickiVodic = await vodici.ToListAsync()
            };
            //var zavrsenProektContext = _context.TuristickiVodic.Include(m => m.Poseta);
            return View(vodicFilter);
        }

        // GET: TuristickiVodics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turistickiVodic = await _context.TuristickiVodic
                .Include(m => m.Poseta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turistickiVodic == null)
            {
                return NotFound();
            }

            return View(turistickiVodic);
        }

        // GET: TuristickiVodics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TuristickiVodics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ime,Prezime,Obrazovanie")] TuristickiVodic turistickiVodic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turistickiVodic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(turistickiVodic);
        }

        // GET: TuristickiVodics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var turistickiVodic =_context.TuristickiVodic.Include(m => m.Poseta).First();
           
            if (turistickiVodic == null)
            {
                return NotFound();
            }

            MoiPosetiVodic viewmodel = new MoiPosetiVodic
            {
                Vodic = turistickiVodic,
                PosetiList = new MultiSelectList(_context.Poseta.OrderBy(s => s.Ime), "Id", "Ime"),
                SelectedPoseti = turistickiVodic.Poseta.Select(sa => sa.Id)
            };
            return View(viewmodel);
        }

        // POST: TuristickiVodics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MoiPosetiVodic moiPoseti)
        {
            if (id != moiPoseti.Vodic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moiPoseti.Vodic);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> listPoseti = moiPoseti.SelectedPoseti;
                    IQueryable<Poseta> toBeRemoved = _context.Poseta.Where(s => !listPoseti.Contains(s.Id) && s.Id == id);
                    _context.Poseta.RemoveRange(toBeRemoved);

                    IEnumerable<int> existPoseti = _context.Poseta.Where(s => listPoseti.Contains(s.Id) && s.TuristickiVodicId == id).Select(s => s.Id);
                    IEnumerable<int> newPoseti = listPoseti.Where(s => !existPoseti.Contains(s));
                    foreach (int posetaId in newPoseti)
                        _context.Poseta.Add(new Poseta { Id = posetaId, TuristickiVodicId = id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TuristickiVodicExists(moiPoseti.Vodic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(moiPoseti);
        }

        // GET: TuristickiVodics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turistickiVodic = await _context.TuristickiVodic
                .Include(m => m.Poseta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turistickiVodic == null)
            {
                return NotFound();
            }

            return View(turistickiVodic);
        }

        // POST: TuristickiVodics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turistickiVodic = await _context.TuristickiVodic.FindAsync(id);
            _context.TuristickiVodic.Remove(turistickiVodic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private string UploadedFile(IFormFile model)
        {
            string uniqueFileName = null;
            if (model != null)
            {
                string uploadsFolder = Path.Combine(WebHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + " _ " + Path.GetFileName(model.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        public IActionResult UploadPic(double? id)
        {
            var vm = new ProfilePicVodic
            {
                TuristickiVodic = null,
                ProfilePic = null
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPic(double? id, IFormFile iffff)
        {
            var vm = new ProfilePicVodic
            {
                TuristickiVodic = await _context.TuristickiVodic.FindAsync(id),
                ProfilePic = iffff
            };
            string uniqueFileName = UploadedFile(vm.ProfilePic);
            vm.TuristickiVodic.ProfilePicture = uniqueFileName;
            _context.Update(vm.TuristickiVodic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TuristickiVodicExists(int id)
        {
            return _context.TuristickiVodic.Any(e => e.Id == id);
        }
    }
}

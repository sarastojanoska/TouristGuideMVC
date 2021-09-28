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
    public class TuristsController : Controller
    {
        private readonly ZavrsenProektContext _context;
        private object webHostEnvironment;
        private IWebHostEnvironment WebHostEnvironment { get; }

        public TuristsController(ZavrsenProektContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        // GET: Turists
        public async Task<IActionResult> Index(string ime,string prezime,string searchString)
        {
            IQueryable<Turist> turisti = _context.Turist.AsQueryable();
            if (!string.IsNullOrEmpty(ime))
            {
                turisti = turisti.Where(s => s.Ime.Contains(ime));
            }
            if (!string.IsNullOrEmpty(prezime))
            {
                turisti = turisti.Where(s => s.Prezime.Contains(prezime));
            }
            turisti = turisti.Include(m => m.Poseti).ThenInclude(m => m.Poseta);
            var turistFilter = new TuristFilterViewModel
            {
                Turist = await turisti.ToListAsync()
            };
            //var zavrsenProektContext = _context.Turist.Include(m => m.Poseti).ThenInclude(m => m.Poseta);
            return View(turistFilter);
        }

        // GET: Turists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turist = await _context.Turist
                .Include(m => m.Poseti).ThenInclude(m => m.Poseta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turist == null)
            {
                return NotFound();
            }

            return View(turist);
        }

        // GET: Turists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Turists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PassportId,Ime,Prezime,DatumPrijava")] Turist turist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(turist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(turist);
        }

        // GET: Turists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var turist = _context.Turist.Where(m => m.Id == id).Include(m => m.Poseti).ThenInclude(m => m.Poseta).First();
            if (turist == null)
            {
                return NotFound();
            }

            MoiPosetiTurist viewmodel = new MoiPosetiTurist
            {
                Turist = turist,
                PosetiList = new MultiSelectList(_context.Poseta.OrderBy(s => s.Ime), "Id", "Ime"),
                SelectedPoseti = turist.Poseti.Select(sa => sa.PosetaId)
            };
            return View(viewmodel);
        }

        // POST: Turists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MoiPosetiTurist moiPoseti)
        {
            if (id != moiPoseti.Turist.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moiPoseti.Turist);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> listPoseti = moiPoseti.SelectedPoseti;
                    IQueryable<VkluciSe> toBeRemoved = _context.VkluciSe.Where(s => !listPoseti.Contains(s.PosetaId) && s.PosetaId == id);
                    _context.VkluciSe.RemoveRange(toBeRemoved);

                    IEnumerable<int> existPoseti = _context.VkluciSe.Where(s => listPoseti.Contains(s.PosetaId) && s.TouristId == id).Select(s => s.PosetaId);
                    IEnumerable<int> newPoseti = listPoseti.Where(s => !existPoseti.Contains(s));
                    foreach (int posetaId in newPoseti)
                        _context.VkluciSe.Add(new VkluciSe { PosetaId = posetaId, TouristId = id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TuristExists(moiPoseti.Turist.Id))
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

        // GET: Turists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turist = await _context.Turist
                .Include(m => m.Poseti).ThenInclude(m => m.Poseta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (turist == null)
            {
                return NotFound();
            }

            return View(turist);
        }

        // POST: Turists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turist = await _context.Turist.FindAsync(id);
            _context.Turist.Remove(turist);
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
            var vm = new ProfilePicTurist
            {
                Turist = null,
                ProfilePic = null
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPic(double? id, IFormFile iffff)
        {
            var vm = new ProfilePicTurist
            {
                Turist = await _context.Turist.FindAsync(id),
                ProfilePic = iffff
            };
            string uniqueFileName = UploadedFile(vm.ProfilePic);
            vm.Turist.ProfilePicture = uniqueFileName;
            _context.Update(vm.Turist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    
    private bool TuristExists(int id)
        {
            return _context.Turist.Any(e => e.Id == id);
        }
    }
}

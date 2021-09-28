using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZavrsenProekt.Data;
using ZavrsenProekt.Models;
using ZavrsenProekt.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ZavrsenProekt.Controllers
{
    public class VkluciSesController : Controller
    {
        private readonly ZavrsenProektContext _context;
        private IWebHostEnvironment WebHostEnvironment { get; }

        public VkluciSesController(ZavrsenProektContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            WebHostEnvironment = webHostEnvironment;
        }

        // GET: VkluciSes
        public async Task<IActionResult> Index()
        {
            var zavrsenProektContext = _context.VkluciSe.Include(v => v.Poseta);
            return View(await zavrsenProektContext.ToListAsync());
        }

        // GET: VkluciSes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vkluciSe = await _context.VkluciSe
                .Include(v => v.Poseta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vkluciSe == null)
            {
                return NotFound();
            }

            return View(vkluciSe);
        }

        // GET: VkluciSes/Create
        public IActionResult Create()
        {
            ViewData["PosetaId"] = new SelectList(_context.Poseta, "Id", "Id");
            return View();
        }

        // POST: VkluciSes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PosetaId,TouristId,PaymentDate,PosetaUrl,ZnamenitostUrl,BrojKartica,CVC,ValidnostData")] VkluciSe vkluciSe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vkluciSe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PosetaId"] = new SelectList(_context.Poseta, "Id", "Id", vkluciSe.PosetaId);
            return View(vkluciSe);
        }

        // GET: VkluciSes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vkluciSe = await _context.VkluciSe.Include(s => s.Poseta).Include(s => s.Turist).FirstOrDefaultAsync(m => m.Id == id);
            if (vkluciSe == null)
            {
                return NotFound();
            }
            ViewData["PosetaId"] = new SelectList(_context.Poseta, "Id", "Id", vkluciSe.PosetaId);
            return View(vkluciSe);
        }

        // POST: VkluciSes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PosetaId,TouristId,PaymentDate,PosetaUrl,ZnamenitostUrl,BrojKartica,CVC,ValidnostData")] VkluciSe vkluciSe, IFormFile posUrl, IFormFile znamUrl)
        {
            if (id != vkluciSe.Id)
            {
                return NotFound();
            }

            var enrollmentToUpdate = await _context.VkluciSe.FirstOrDefaultAsync(s => s.Id == id);
            enrollmentToUpdate.PosetaUrl = UploadedFile(posUrl);
            enrollmentToUpdate.ZnamenitostUrl = UploadedFile(znamUrl);
            await TryUpdateModelAsync<VkluciSe>(enrollmentToUpdate, "", s => s.PosetaUrl);
            await TryUpdateModelAsync<VkluciSe>(enrollmentToUpdate, "", s => s.ZnamenitostUrl);
            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(vkluciSe);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VkluciSeExists(vkluciSe.Id))
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
            ViewData["PosetaId"] = new SelectList(_context.Poseta, "Id", "Id", vkluciSe.PosetaId);
            return View(enrollmentToUpdate);
        }

        // GET: VkluciSes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vkluciSe = await _context.VkluciSe
                .Include(v => v.Poseta)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vkluciSe == null)
            {
                return NotFound();
            }

            return View(vkluciSe);
        }

        // POST: VkluciSes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vkluciSe = await _context.VkluciSe.FindAsync(id);
            _context.VkluciSe.Remove(vkluciSe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: VkluciSes/EnrollTourists
        public async Task<IActionResult> ListAllPoseti()
        {
            return View(await _context.Poseta.ToListAsync());
        }
        public async Task<IActionResult> VkluciTuristi(int? id)
        {
            var poseta = await _context.Poseta.Where(m => m.Id == id).Include(m => m.Turisti).FirstOrDefaultAsync();
            var viewmodel = new VkluciTuristiViewModel
            {
                TouristList = new MultiSelectList(_context.Turist.OrderBy(s => s.Ime), "Id","FullName"),
                SelectedTourists = poseta.Turisti.Select(sa => sa.TouristId)
            };
            ViewData["Poseta"] = _context.Poseta.Where(c => c.Id == id).Select(c => c.Ime).FirstOrDefault();
            ViewData["chosenId"] = id;
            return View(viewmodel);
        }
        //POST: VkluciSes/EnrollTourists
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VkluciTuristi(int id, VkluciTuristiViewModel viewmodel)
        {
            if(id != viewmodel.VkluciSe.PosetaId)
            {
                return NotFound();
            }
            IEnumerable<int> listTourists = viewmodel.SelectedTourists;
            IEnumerable<int> existTourists = _context.VkluciSe.Where(s => listTourists.Contains(s.TouristId) && s.PosetaId == id).Select(s => s.TouristId);
            IEnumerable<int> newTourists = listTourists.Where(s => !existTourists.Contains(s));
            foreach (int tId in newTourists)
            {
                _context.VkluciSe.Add(new VkluciSe { TouristId = tId, PosetaId = id, ZnamenitostUrl = viewmodel.VkluciSe.ZnamenitostUrl, PosetaUrl = viewmodel.VkluciSe.PosetaUrl });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //GET: VkluciSes/UnenrollStudent
        public async Task<IActionResult> OdjaviTurist(int? id)
        {
            var poseta = await _context.Poseta.Where(m => m.Id == id).Include(m => m.Turisti).FirstOrDefaultAsync();
            var viewmodel = new VkluciTuristiViewModel
            {
                TouristList = new MultiSelectList(_context.Turist.OrderBy(s => s.Ime), "Id", "FullName"),
                SelectedTourists = poseta.Turisti.Select(sa => sa.TouristId)
            };
            ViewData["Poseta"] = _context.Poseta.Where(c => c.Id == id).Select(c => c.Ime).FirstOrDefault();
            ViewData["chosenId"] = id;
            return View(viewmodel);
        }
        //POST: VkluciSes/UnenrollStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OdjaviTurist(int id,VkluciTuristiViewModel viewmodel)
        {
            if(id != viewmodel.VkluciSe.PosetaId)
            {
                return NotFound();
            }
            IEnumerable<int> listTourists = viewmodel.SelectedTourists;
            IEnumerable<VkluciSe> existTourist = _context.VkluciSe.Where((s => listTourists.Contains(s.TouristId) && s.PosetaId == id));
            foreach (var enr in existTourist)
            {
                _context.VkluciSe.Update(viewmodel.VkluciSe);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        private string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(WebHostEnvironment.WebRootPath, "seminals");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        public async Task<IActionResult> MoiTuristiPoPoseta(int id)
        {
            var poseta = _context.Poseta.Where(l => l.Id == id).FirstOrDefault();
            var vklucise = _context.VkluciSe.Where(s => s.PosetaId == id);
            vklucise = vklucise.Include(s => s.Turist);
            
            return View(vklucise);
        }
        public async Task<IActionResult> StatusPoPoseta(int id)
        {
            var turist = _context.Turist.Where(s => s.Id == id);
            var vklucise = _context.VkluciSe.Where(s => s.TouristId == id);
            vklucise = vklucise.Include(c => c.Poseta);

            return View(vklucise);
        }
        public async Task<IActionResult> EditStatusPoPoseta(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vklucise = await _context.VkluciSe.Include(s => s.Poseta).Include(s => s.Turist).FirstOrDefaultAsync(m => m.Id == id);
            if (vklucise == null)
            {
                return NotFound();
            }

            return View(vklucise);
        }
        [HttpPost, ActionName("EditStatusPoPoseta")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatusPoPoseta(int id, DateTime payment,DateTime validate, string cvc, string cardnum)
        {
            if(id == null)
            {
                return NotFound();
            }
            int stId = 1;
            string st = null;
            if (TempData["selectedStudent"] != null)
            {
                st = TempData["selectedStudent"].ToString();
                stId = Int32.Parse(st);
            }
            var enrollmentToUpdate = await _context.VkluciSe.FirstOrDefaultAsync(s => s.Id == id);
            enrollmentToUpdate.PaymentDate = payment;
            enrollmentToUpdate.ValidnostData = validate;
            enrollmentToUpdate.BrojKartica = cardnum;
            enrollmentToUpdate.CVC = cvc;
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("StatusPoPoseta", "VkluciSes", new { id = stId });

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VkluciSeExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(enrollmentToUpdate);
        }
        private bool VkluciSeExists(int id)
        {
            return _context.VkluciSe.Any(e => e.Id == id);
        }
    }
}

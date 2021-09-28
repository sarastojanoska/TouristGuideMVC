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

namespace ZavrsenProekt.Controllers
{
    public class PosetasController : Controller
    {
        private readonly ZavrsenProektContext _context;

        public PosetasController(ZavrsenProektContext context)
        {
            _context = context;
        }

        // GET: Posetas
        public async Task<IActionResult> Index(string imePoseta, string znamenitostPoseta,string searchString,string searchString1)
        {
            IQueryable<Poseta> poseti = _context.Poseta.AsQueryable();
            IQueryable<string> iminja = _context.Poseta.OrderBy(m => m.Ime).Select(m => m.Ime).Distinct();
            IQueryable<string> znamenitosti = _context.Poseta.OrderBy(m => m.Znamenitost).Select(m => m.Znamenitost).Distinct();
            if (!string.IsNullOrEmpty(searchString))
            {
                poseti = poseti.Where(s => s.Ime.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(searchString1))
            {
                poseti = poseti.Where(s => s.Ime.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(imePoseta))
            {
                poseti = poseti.Where(x => x.Ime == imePoseta);
            }
            if (!string.IsNullOrEmpty(znamenitostPoseta))
            {
                poseti = poseti.Where(x => x.Znamenitost == znamenitostPoseta);
            }
            poseti = poseti.Include(p => p.TuristickiVodic).Include(m => m.Turisti).ThenInclude(m => m.Turist);
            var posetiFilter = new PosetaFilterViewModel
            {
                Iminja = new SelectList(await iminja.ToListAsync()),
                Znamenitosti = new SelectList(await znamenitosti.ToListAsync()),
                Poseti = await poseti.ToListAsync()
            };
            var zavrsenProektContext = _context.Poseta.Include(p => p.TuristickiVodic).Include(m => m.Turisti).ThenInclude(m => m.Turist);
            return View(posetiFilter);
        }

        // GET: Posetas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poseta = await _context.Poseta
                .Include(p => p.TuristickiVodic)
                .Include(p => p.Turisti).ThenInclude(m => m.Turist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poseta == null)
            {
                return NotFound();
            }

            return View(poseta);
        }

        // GET: Posetas/Create
        public IActionResult Create()
        {
            ViewData["TuristickiVodicId"] = new SelectList(_context.Set<TuristickiVodic>(), "Id", "FullName");
            return View();
        }

        // POST: Posetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ime,Znamenitost,Price,Komentar,DatumPoseta,TuristickiVodicId")] Poseta poseta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(poseta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TuristickiVodicId"] = new SelectList(_context.Set<TuristickiVodic>(), "Id", "FullName", poseta.TuristickiVodicId);
            return View(poseta);
        }

        // GET: Posetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poseta = _context.Poseta.Where(m => m.Id == id).Include(m => m.Turisti).First();
            if (poseta == null)
            {
                return NotFound();
            }
            
            PosetaTuristiEditViewModel viewmodel = new PosetaTuristiEditViewModel
            {
                Poseta = poseta,
                TuristiList = new MultiSelectList(_context.Turist.OrderBy(s => s.PassportId), "Id", "FullName"),
                SelectedTuristi = poseta.Turisti.Select(sa => sa.TouristId)
            };

            ViewData["TuristickiVodicId"] = new SelectList(_context.Set<TuristickiVodic>(), "Id", "FullName", poseta.TuristickiVodicId);
            return View(viewmodel);
        }

        // POST: Posetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PosetaTuristiEditViewModel viewmodel)
        {
            if (id != viewmodel.Poseta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Poseta);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> listTuristi = viewmodel.SelectedTuristi;
                    IQueryable<VkluciSe> toBeRemoved = _context.VkluciSe.Where(s => !listTuristi.Contains(s.TouristId) && s.TouristId == id);
                    _context.VkluciSe.RemoveRange(toBeRemoved);

                    IEnumerable<int> existTuristi = _context.VkluciSe.Where(s => listTuristi.Contains(s.TouristId) && s.PosetaId == id).Select(s => s.TouristId);
                    IEnumerable<int> newTuristi = listTuristi.Where(s => !existTuristi.Contains(s));
                    foreach (int turistId in newTuristi)
                        _context.VkluciSe.Add(new VkluciSe { TouristId = turistId, PosetaId = id});
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosetaExists(viewmodel.Poseta.Id))
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
            ViewData["TuristickiVodicId"] = new SelectList(_context.Set<TuristickiVodic>(), "Id", "FullName", viewmodel.Poseta.TuristickiVodicId);
            return View(viewmodel);
        }

        // GET: Posetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poseta = await _context.Poseta
                .Include(p => p.TuristickiVodic)
                .Include(m => m.Turisti).ThenInclude(m => m.Turist)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poseta == null)
            {
                return NotFound();
            }

            return View(poseta);
        }

        // POST: Posetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var poseta = await _context.Poseta.FindAsync(id);
            _context.Poseta.Remove(poseta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        private bool PosetaExists(int id)
        {
            return _context.Poseta.Any(e => e.Id == id);
        }
    }
}

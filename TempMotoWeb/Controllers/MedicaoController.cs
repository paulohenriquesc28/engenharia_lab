using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TempMotoWeb.Data;
using TempMotoWeb.Models;
using System.Text.Json;


namespace TempMotoWeb.Controllers
{
    public class MedicaoController : Controller
    {
        private readonly AquaContext _context;

        public MedicaoController(AquaContext context)
        {
            _context = context;
        }

        // GET: Medicao
        public async Task<IActionResult> Index()
        {
              return _context.Medicao != null ? 
                          View(await _context.Medicao.ToListAsync()) :
                          Problem("Entity set 'TempMotoWebContext.Medicao'  is null.");
        }

        // GET: Medicao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Medicao == null)
            {
                return NotFound();
            }

            var medicao = await _context.Medicao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicao == null)
            {
                return NotFound();
            }

            return View(medicao);
        }

        // GET: Medicao/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Latitude,Longitude,Altitude,Temperatura,Umidade,Num_Satelites,Velocidade,Data_Medicao")] Medicao medicao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medicao);
        }

        // GET: Medicao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medicao == null)
            {
                return NotFound();
            }

            var medicao = await _context.Medicao.FindAsync(id);
            if (medicao == null)
            {
                return NotFound();
            }
            return View(medicao);
        }

        // POST: Medicao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Latitude,Longitude,Altitude,Temperatura,Umidade,Num_Satelites,Velocidade,Data_Medicao")] Medicao medicao)
        {
            if (id != medicao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicaoExists(medicao.Id))
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
            return View(medicao);
        }

        // GET: Medicao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Medicao == null)
            {
                return NotFound();
            }

            var medicao = await _context.Medicao
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medicao == null)
            {
                return NotFound();
            }

            return View(medicao);
        }

        // POST: Medicao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medicao == null)
            {
                return Problem("Entity set 'TempMotoWebContext.Medicao'  is null.");
            }
            var medicao = await _context.Medicao.FindAsync(id);
            if (medicao != null)
            {
                _context.Medicao.Remove(medicao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicaoExists(int id)
        {
          return (_context.Medicao?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Mapa()
        {
            return View(await _context.Medicao.ToListAsync());
        }
        public async Task<IActionResult> Grafico()
        {
            return View(await _context.Medicao.ToListAsync());
        }

        public async Task<IActionResult> Docs()
        {
            return View(await _context.Medicao.ToListAsync());
        }
        public async Task<IActionResult> Historico()
        {
            return View(await _context.Medicao.ToListAsync());
        }
    }
}

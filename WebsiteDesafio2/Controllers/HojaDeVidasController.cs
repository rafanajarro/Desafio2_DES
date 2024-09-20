using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteDesafio2.Models;

namespace WebsiteDesafio2.Controllers
{
    public class HojaDeVidasController : Controller
    {
        private readonly ProyectoDbContext _context;

        public HojaDeVidasController(ProyectoDbContext context)
        {
            _context = context;
        }

        // GET: HojaDeVidas
        public async Task<IActionResult> Index()
        {
            return View(await _context.HojaDeVida.ToListAsync());
        }

        // GET: HojaDeVidas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hojaDeVida = await _context.HojaDeVida
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hojaDeVida == null)
            {
                return NotFound();
            }

            return View(hojaDeVida);
        }

        // GET: HojaDeVidas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HojaDeVidas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NombreCompleto,FechaNacimiento")] HojaDeVida hojaDeVida)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hojaDeVida);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hojaDeVida);
        }

        // GET: HojaDeVidas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hojaDeVida = await _context.HojaDeVida.FindAsync(id);
            if (hojaDeVida == null)
            {
                return NotFound();
            }
            return View(hojaDeVida);
        }

        // POST: HojaDeVidas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NombreCompleto,FechaNacimiento")] HojaDeVida hojaDeVida)
        {
            if (id != hojaDeVida.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hojaDeVida);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HojaDeVidaExists(hojaDeVida.Id))
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
            return View(hojaDeVida);
        }

        // GET: HojaDeVidas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hojaDeVida = await _context.HojaDeVida
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hojaDeVida == null)
            {
                return NotFound();
            }

            return View(hojaDeVida);
        }

        // POST: HojaDeVidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hojaDeVida = await _context.HojaDeVida.FindAsync(id);
            if (hojaDeVida != null)
            {
                _context.HojaDeVida.Remove(hojaDeVida);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HojaDeVidaExists(int id)
        {
            return _context.HojaDeVida.Any(e => e.Id == id);
        }
    }
}

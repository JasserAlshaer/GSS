using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GSS.Models;

namespace GSS.Controllers
{
    public class ProcuduresController : Controller
    {
        private readonly GssContext _context;

        public ProcuduresController(GssContext context)
        {
            _context = context;
        }

        // GET: Procudures
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            var gssContext = _context.Procudures.Include(p => p.Department);
            return View(await gssContext.ToListAsync());
        }

        // GET: Procudures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.Procudures == null)
            {
                return NotFound();
            }

            var procudure = await _context.Procudures
                .Include(p => p.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procudure == null)
            {
                return NotFound();
            }

            return View(procudure);
        }

        // GET: Procudures/Create
        public IActionResult Create()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Procudures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,MinAmount,MaxAmount,DefaultAmount,DepartmentId")] Procudure procudure)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                _context.Add(procudure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", procudure.DepartmentId);
            return View(procudure);
        }

        // GET: Procudures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.Procudures == null)
            {
                return NotFound();
            }

            var procudure = await _context.Procudures.FindAsync(id);
            if (procudure == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", procudure.DepartmentId);
            return View(procudure);
        }

        // POST: Procudures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,MinAmount,MaxAmount,DefaultAmount,DepartmentId")] Procudure procudure)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id != procudure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(procudure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcudureExists(procudure.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", procudure.DepartmentId);
            return View(procudure);
        }

        // GET: Procudures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.Procudures == null)
            {
                return NotFound();
            }

            var procudure = await _context.Procudures
                .Include(p => p.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (procudure == null)
            {
                return NotFound();
            }

            return View(procudure);
        }

        // POST: Procudures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (_context.Procudures == null)
            {
                return Problem("Entity set 'GssContext.Procudures'  is null.");
            }
            var procudure = await _context.Procudures.FindAsync(id);
            if (procudure != null)
            {
                _context.Procudures.Remove(procudure);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcudureExists(int id)
        {
          return (_context.Procudures?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

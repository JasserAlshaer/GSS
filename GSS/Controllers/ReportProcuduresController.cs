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
    public class ReportProcuduresController : Controller
    {
        private readonly GssContext _context;

        public ReportProcuduresController(GssContext context)
        {
            _context = context;
        }

        // GET: ReportProcudures
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            var gssContext = _context.ReportProcudures.Include(r => r.Procudure).Include(r => r.Report);
            return View(await gssContext.ToListAsync());
        }

        // GET: ReportProcudures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.ReportProcudures == null)
            {
                return NotFound();
            }

            var reportProcudure = await _context.ReportProcudures
                .Include(r => r.Procudure)
                .Include(r => r.Report)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reportProcudure == null)
            {
                return NotFound();
            }

            return View(reportProcudure);
        }

        // GET: ReportProcudures/Create
        public IActionResult Create()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            ViewData["ProcudureId"] = new SelectList(_context.Procudures, "Id", "Id");
            ViewData["ReportId"] = new SelectList(_context.Reports, "Id", "Id");
            return View();
        }

        // POST: ReportProcudures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProcudureId,ReportId,ActualAmount")] ReportProcudure reportProcudure)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                _context.Add(reportProcudure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProcudureId"] = new SelectList(_context.Procudures, "Id", "Id", reportProcudure.ProcudureId);
            ViewData["ReportId"] = new SelectList(_context.Reports, "Id", "Id", reportProcudure.ReportId);
            return View(reportProcudure);
        }

        // GET: ReportProcudures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.ReportProcudures == null)
            {
                return NotFound();
            }

            var reportProcudure = await _context.ReportProcudures.FindAsync(id);
            if (reportProcudure == null)
            {
                return NotFound();
            }
            ViewData["ProcudureId"] = new SelectList(_context.Procudures, "Id", "Id", reportProcudure.ProcudureId);
            ViewData["ReportId"] = new SelectList(_context.Reports, "Id", "Id", reportProcudure.ReportId);
            return View(reportProcudure);
        }

        // POST: ReportProcudures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProcudureId,ReportId,ActualAmount")] ReportProcudure reportProcudure)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id != reportProcudure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportProcudure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportProcudureExists(reportProcudure.Id))
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
            ViewData["ProcudureId"] = new SelectList(_context.Procudures, "Id", "Id", reportProcudure.ProcudureId);
            ViewData["ReportId"] = new SelectList(_context.Reports, "Id", "Id", reportProcudure.ReportId);
            return View(reportProcudure);
        }

        // GET: ReportProcudures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.ReportProcudures == null)
            {
                return NotFound();
            }

            var reportProcudure = await _context.ReportProcudures
                .Include(r => r.Procudure)
                .Include(r => r.Report)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reportProcudure == null)
            {
                return NotFound();
            }

            return View(reportProcudure);
        }

        // POST: ReportProcudures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (_context.ReportProcudures == null)
            {
                return Problem("Entity set 'GssContext.ReportProcudures'  is null.");
            }
            var reportProcudure = await _context.ReportProcudures.FindAsync(id);
            if (reportProcudure != null)
            {
                _context.ReportProcudures.Remove(reportProcudure);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportProcudureExists(int id)
        {
          return (_context.ReportProcudures?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

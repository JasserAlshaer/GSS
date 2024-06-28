using GSS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GSS.Controllers
{
    public class UEmployeeController : Controller
    {
        private readonly GssContext _context;
        public UEmployeeController(GssContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.Where(x => x.Id == id).Include(d => d.UserType).FirstOrDefault();
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Login", "Main");
        }
        public IActionResult Reports()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var gssContext = _context.Reports.Where(x => x.IsComplete == false).Include(r => r.User);
            return View(gssContext.ToList());
        }


        public IActionResult ReportDetails(int id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }

            var report = _context.Reports
                .Include(r => r.User)
                .FirstOrDefault(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        public IActionResult Procudures(int id)
        {
            var gssContext = _context.ReportProcudures
                .Where(x=>x.ReportId == id)
                .Include(r => r.Procudure).Include(r => r.Report);
            return View( gssContext.ToList());
        }

        public IActionResult ProcudureCreate(int reportId)
        {
            return View();
        }


        public async Task<IActionResult> Details(int? id)
        {
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

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login", "Main");
        }
    }
}

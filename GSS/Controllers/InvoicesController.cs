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
    public class InvoicesController : Controller
    {
        private readonly GssContext _context;

        public InvoicesController(GssContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            var gssContext = _context.Invoices.Include(i => i.Report);
            return View(await gssContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Report)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            ViewData["ReportId"] = new SelectList(_context.Reports, "Id", "Id");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IssuedDate,Amount,ReportId")] Invoice invoice)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReportId"] = new SelectList(_context.Reports, "Id", "Id", invoice.ReportId);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["ReportId"] = new SelectList(_context.Reports, "Id", "Id", invoice.ReportId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IssuedDate,Amount,ReportId")] Invoice invoice)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            ViewData["ReportId"] = new SelectList(_context.Reports, "Id", "Id", invoice.ReportId);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Report)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'GssContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
          return (_context.Invoices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

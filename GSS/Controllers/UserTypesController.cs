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
    public class UserTypesController : Controller
    {
        private readonly GssContext _context;

        public UserTypesController(GssContext context)
        {
            _context = context;
        }

        // GET: UserTypes
        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            return _context.UserTypes != null ? 
                          View(await _context.UserTypes.ToListAsync()) :
                          Problem("Entity set 'GssContext.UserTypes'  is null.");
        }

        // GET: UserTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.UserTypes == null)
            {
                return NotFound();
            }

            var userType = await _context.UserTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // GET: UserTypes/Create
        public IActionResult Create()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            return View();
        }

        // POST: UserTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] UserType userType)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                _context.Add(userType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userType);
        }

        // GET: UserTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.UserTypes == null)
            {
                return NotFound();
            }

            var userType = await _context.UserTypes.FindAsync(id);
            if (userType == null)
            {
                return NotFound();
            }
            return View(userType);
        }

        // POST: UserTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] UserType userType)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id != userType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTypeExists(userType.Id))
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
            return View(userType);
        }

        // GET: UserTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.UserTypes == null)
            {
                return NotFound();
            }

            var userType = await _context.UserTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userType == null)
            {
                return NotFound();
            }

            return View(userType);
        }

        // POST: UserTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (_context.UserTypes == null)
            {
                return Problem("Entity set 'GssContext.UserTypes'  is null.");
            }
            var userType = await _context.UserTypes.FindAsync(id);
            if (userType != null)
            {
                _context.UserTypes.Remove(userType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTypeExists(int id)
        {
          return (_context.UserTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

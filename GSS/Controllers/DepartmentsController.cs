﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GSS.Models;

namespace GSS.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly GssContext _context;

        public DepartmentsController(GssContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.Where(x => x.Id == id).Single();
            if (user == null)
                return RedirectToAction("Login");
            return _context.Departments != null ? 
                          View(await _context.Departments.ToListAsync()) :
                          Problem("Entity set 'GssContext.Departments'  is null.");
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Department department)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (_context.Departments == null)
            {
                return Problem("Entity set 'GssContext.Departments'  is null.");
            }
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {

          return (_context.Departments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

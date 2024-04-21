using GSS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GSS.Controllers
{

    public class MainController : Controller
    {
        private readonly GssContext _context;
        public MainController(GssContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync
                (x => x.Email == email && x.Password == password);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                // Employee Login 

                //Student Login

                //Admin Login

                return View();
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Students()
        {
            return View();
        }
        public IActionResult Employees()
        {
            return View();
        }
        public IActionResult Reports()
        {
            return View();
        }
        public IActionResult Invoices()
        {
            return View();
        }
    }
}

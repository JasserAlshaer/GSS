using GSS.DTO;
using GSS.Models;
using Microsoft.AspNetCore.Http;
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
                HttpContext.Session.SetInt32("UserId", user.Id);
                // Employee Login 
                if (user.UserTypeId == 2)
                {

                    HttpContext.Session.SetString("IsAdmin", "false");
                    return RedirectToAction("Index", "UEmployee");
                }

                //Student Login
                if (user.UserTypeId == 3)
                {

                    HttpContext.Session.SetString("IsStudent", "true");
                    return RedirectToAction("Index", "GraduateStudent");
                }

                //Admin Login
                if (user.UserTypeId == 1)
                {

                    HttpContext.Session.SetString("IsAdmin", "true");
                    return RedirectToAction("Index");
                }

                return View();

            }
        }
        
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPassword(string email ,string password, string newPassword)
        {
            var user = _context.Users.Where(x => x.Email == email).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                if (password.Equals(newPassword))
                {
                    user.Password = newPassword;
                    _context.Update(user);
                    _context.SaveChanges();
                }
                return View("Login");
            }
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.Where(x => x.Id == id).Single();
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Login");

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login");
        }
    }
}

using GSS.DTO;
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
                if(user.UserTypeId == 1)
                {
                    HttpContext.Session.SetString("IsAdmin", "true");
                    return RedirectToAction("Index");
                }

                return View();
               
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Students()
        {

            return View(_context.Users.Where(x => x.UserTypeId == 3).ToList());
        }
        public IActionResult Employees()
        {
            var response = from emp in _context.Users
                           where emp.UserTypeId == 2
                           select new EmployeeCardDTO
                           {
                               Id = emp.Id,
                               Name = emp.Name,
                               Email = emp.Email,
                               Phone = emp.Phone,
                               DepartmentName = emp.Department.Name
                           };
            var res1 = _context.Users.Where(x => x.UserTypeId == 2).Include
                (x => x.Department).ToList();
            return View(response.ToList());
        }
        public IActionResult Reports()
        {
            string isadminloggedIn = HttpContext.Session.GetString("IsAdmin");
            if (isadminloggedIn == "true")
            {
                return View(_context.Reports.ToList());
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }
        public IActionResult Invoices()
        {
            return View(_context.Invoices.ToList());
        }

        public IActionResult Edit(int id)
        {

            var report = _context.Reports.FirstOrDefault(x => x.Id == id);
            return View(report);
        }
        [HttpPost]
        public IActionResult Edit(int id,string title,DateTime requestdate,float DueAmount)
        {
            var report = _context.Reports.FirstOrDefault(x => x.Id == id);
            report.Title = title;
            report.DueAmount = DueAmount;
            //report.RequetDate = requestdate;
            _context.Update(report);
            _context.SaveChanges();
            return RedirectToAction("Reports");

        }
        public IActionResult Delete(int id,string test,bool isxc)
        {
            var report = _context.Reports.FirstOrDefault(x => x.Id == id);
            _context.Remove(report);
            _context.SaveChanges();
            return RedirectToAction("Reports");
        }
    }
}

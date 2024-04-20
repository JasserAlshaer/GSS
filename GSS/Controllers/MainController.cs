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
                return View("Index");
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Departments() {
            var deps = _context.Departments.ToList();
            return View(deps); 
        }
    }
}

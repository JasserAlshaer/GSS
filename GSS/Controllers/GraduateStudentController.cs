using GSS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Composition;

namespace GSS.Controllers
{
    public class GraduateStudentController : Controller
    {
        private readonly GssContext _context;
        public GraduateStudentController(GssContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var user = _context.Users.Where(x => x.Id == id).Include(d=>d.UserType).FirstOrDefault();
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Login","Main");

        }

        public IActionResult Reports() {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            var id = HttpContext.Session.GetInt32("UserId");
            var gssContext = _context.Reports.Where(x=>x.UserId == id).Include(r => r.User);
            return View( gssContext.ToList());
        }

        public IActionResult Invoices(int Id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            var gssContext = _context.Invoices.Where(x => x.ReportId == Id).Include(i => i.Report);
            return View(gssContext.ToList());
        }

        public IActionResult ReportDetails(int id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
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

        public IActionResult InvoiceDetails(int Id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            if (Id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice =  _context.Invoices
                .Include(i => i.Report)
                .FirstOrDefault(m => m.Id == Id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }
        public IActionResult PayNow(int Id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            var report = _context.Reports.Where(x=>x.Id == Id).FirstOrDefault();
            ViewBag.ReportId = Id;
            ViewBag.Final = report.DueAmount;
            return View();
        }

        [HttpPost]
        public IActionResult PayByVisa(String visaNumber, int cvvCode,int reportId)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            int? areThereId = HttpContext.Session.GetInt32("Id");
            if (areThereId != null && HttpContext.Session.GetString("Role") == "1")
            {
                var report = _context.Reports.Where(x => x.Id == reportId).FirstOrDefault();
                double finalPrice = (double)report.DueAmount;
                var findMyCard = _context.PaymentMethods.Where(card => card.VisaNumber == visaNumber
                  && card.Code == cvvCode && card.Balance >= finalPrice
                ).SingleOrDefault();
                if (findMyCard != null)
                {
                    Invoice sailing = new Invoice();
                    sailing.ReportId = reportId;
                    sailing.Title = report.Title + "Invoice";
                    sailing.Amount = finalPrice;
                    sailing.IssuedDate = DateTime.Now;
                    _context.Add(sailing);
                    _context.SaveChanges();

                    findMyCard.Balance -= finalPrice;

                    _context.Update(findMyCard);
                    _context.SaveChanges();

                    return RedirectToAction("Reports");

                }
                else
                {
                    return RedirectToAction("EnterPayment");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        [HttpPost]
        public IActionResult PayByPayPal(String email, string passwod, int reportId)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            int? areThereId = HttpContext.Session.GetInt32("Id");
            if (areThereId != null && HttpContext.Session.GetString("Role") == "1")
            {
                var report = _context.Reports.Where(x => x.Id == reportId).FirstOrDefault();
                double finalPrice = (double)report.DueAmount;
                var findMyCard = _context.PaymentMethods.Where(card => card.Email == email
                  && card.Password == passwod && card.Balance >= finalPrice
                ).SingleOrDefault();
                if (findMyCard != null)
                {

                    Invoice sailing = new Invoice();
                    sailing.ReportId = reportId;
                    sailing.Title = report.Title + "Invoice";
                    sailing.Amount = finalPrice;
                    sailing.IssuedDate = DateTime.Now;
                    _context.Add(sailing);
                    _context.SaveChanges();

                    findMyCard.Balance -= finalPrice;

                    _context.Update(findMyCard);
                    _context.SaveChanges();

                    return RedirectToAction("Reports");

                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }


        }

        public IActionResult PrintReport(int id)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var userLogin = _context.Users.Where(x => x.Id == userid).Single();
            if (userLogin == null)
                return RedirectToAction("Login");
            // Fetch the report by Id
            var report = _context.Reports.Where(x=>x.Id == id).FirstOrDefault();

            if (report == null)
            {
                return NotFound(); // Handle if report with given Id is not found
            }

            ViewBag.List = _context.ReportProcudures.Where(x => x.ReportId == id)
                .Include(x=>x.Report).Include(x=>x.Procudure).ToList();

            return View(report);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Login", "Main");
        }
    }
}

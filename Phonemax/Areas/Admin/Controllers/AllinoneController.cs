using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using Phonemax.uitility;
using System.Linq;
using System.Text;

namespace Phonemax.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AllinoneController : Controller
    {
        private readonly Iunitofwork _unitofwork;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailsender;
        private readonly UserManager<IdentityUser> _userManager;

        private static bool isemailconfirm = false;

        public AllinoneController(Iunitofwork unitofwork, ApplicationDbContext context,IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unitofwork = unitofwork;
            _context = context;
            _emailsender = emailSender;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult all()
        {
            //  var products = _context.orderdetails.OrderByDescending(p => p.count).ToList();
            var products = _context.orderdetails.Include(p => p.Product).Include(p=>p.OrderHeader).OrderByDescending(p => p.count)
            .ToList();
              //var products=_unitofwork.orderdetail.GetAll(includeproperties: "Product,OrderHeader").OrderByDescending(p => p.count).ToList();


            return View(products);
         
        }
        public IActionResult ByDateTime(DateTime? datetime1, DateTime? datetime2)
        {
            if (datetime1 == null && datetime2 == null)
            {
                return RedirectToAction("Index");
            }
            if (datetime2 == null)
            {
                var date = _unitofwork.orderheader.GetAll(u => u.OrderDate.Date == datetime1);
                return View(date);
            }
            if (datetime1 == null)
            {
                var date = _unitofwork.orderheader.GetAll(u => u.OrderDate.Date == datetime2);
                return View(date);
            }
            IQueryable<Orderheader> query = from o in _context.orderheaders select o;
            if (datetime1 != null && datetime2 != null)
            {
                query = from o in _context.orderheaders where o.OrderDate.Date > datetime1 && o.OrderDate.Date <= datetime2 select o;
            }
            return View(query);
        }

        public IActionResult StatusApproved()
        {
            var approved = _unitofwork.orderheader.GetAll().Where(os => os.OrderStatus == "Approved");
            return View(approved);
        }
        public IActionResult StatusPending()
        {
           
            var approved = _unitofwork.orderheader.GetAll().Where(os => os.OrderStatus == "pending");
            return View(approved);
        }
        public IActionResult StatusCancelled()
        {
            var approved = _unitofwork.orderheader.GetAll().Where(os => os.OrderStatus == "Cancelled");
            return View(approved);

        }
        public IActionResult StatusRefunded()
        {
            var approved = _unitofwork.orderheader.GetAll().Where(os => os.OrderStatus == "Refunded");
            return View(approved);
        }
        public IActionResult StatusProcessing()
        {
            var approved = _unitofwork.orderheader.GetAll().Where(os => os.OrderStatus == "Processing");
            return View(approved);
        }
        public IActionResult StatusShipped()
        {
            var approved = _unitofwork.orderheader.GetAll().Where(os => os.OrderStatus == "Shipped");
            return View(approved);
        }
      
       



        public IActionResult Upsert(int id)
        {
            var productlist = _unitofwork.orderdetail.FirstOrDefault(includeproperties: "OrderHeader,OrderHeader.applicationuser,Product,Product.Category,Product.Covertype,Product.Processor");
            return View(productlist);
        }
        public async Task<IActionResult> CancelOrder(int id)
        {
            var email = _unitofwork.orderheader.Get(id);
            email.OrderStatus = "Cancelled";
            var user = _unitofwork.applicationuser.FirstOrDefault(u => u.Id == email.ApplicationUserId);

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await _emailsender.SendEmailAsync(user.Email, $"Your Order's Has Been Cancelled due to some reason. the Order Id no is:{email.Id}",
                "Please Contact Us For More Information!!!");
           
            _unitofwork.orderheader.Update(email);
            _unitofwork.save();
            return View("Index");
        }

        #region apis
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitofwork.orderheader.GetAll() });
        }
        #endregion
    }
}

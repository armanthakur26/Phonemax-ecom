using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using Phonemax.uitility;
using System.Diagnostics;
using System.Security.Claims;

namespace Phonemax.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Iunitofwork _unitofwork;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,Iunitofwork unitofwork,ApplicationDbContext context)
        {
            _logger = logger;
            _unitofwork = unitofwork;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitofwork.shop.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(Sd.Ss_CartSessionCount, count);
            }
            if ( _context.products== null)
            {
                return Problem("entity set is null");
            }
            var productss = from p in _context.products select p;
            if (!string.IsNullOrEmpty(searchString))
            {
                productss = productss.Where(s => s.Title!.Contains(searchString) || s.Discription.Contains(searchString));
            }
            var productlist = _unitofwork.Product.GetAll(includeproperties: "category,covertype,processor");
            return View(await productss.ToListAsync());
        }
       
       
        public IActionResult Details(int id)
        {

            var productindb = _unitofwork.Product.FirstOrDefault(p => p.Id == id, includeproperties: "category,covertype,processor");
            if (productindb == null) return NotFound();
            var shoppingcart = new Shoppingcart()
            {
                Product = productindb,
                ProductId = productindb.Id
            };
            //session
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitofwork.shop.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(Sd.Ss_CartSessionCount, count);
            }
            return View(shoppingcart);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public IActionResult details(Shoppingcart shoppingCart)
        {
            shoppingCart.Id = 0;
            if (!ModelState.IsValid)
            {
                var claimsidentity = (ClaimsIdentity)User.Identity;
                var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claim == null) return NotFound();
                shoppingCart.ApplicationUserId = claim.Value;
                var shoppingcartfromdb = _unitofwork.shop.FirstOrDefault(sc => sc.ApplicationUserId == claim.Value && sc.ProductId == shoppingCart.ProductId);
                if (shoppingcartfromdb == null)
                    _unitofwork.shop.Add(shoppingCart);
                else
                    shoppingcartfromdb.count += shoppingCart.count;
                _unitofwork.save();
                return RedirectToAction("Index");
            }
            else
            {
                var productindb = _unitofwork.Product.FirstOrDefault(p => p.Id == shoppingCart.ProductId, includeproperties: "category,covertype");
                if (productindb == null) return NotFound();
                var shoppingcartedit = new  Shoppingcart()
                {
                    Product = productindb,
                    ProductId = productindb.Id
                };
                return View(shoppingcartedit);

            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult od()
        {
            var projects = _context.orderdetails.ToList();
            projects = projects.OrderByDescending(p => p.count).ToList();
            return View(projects);
        }
    }
}
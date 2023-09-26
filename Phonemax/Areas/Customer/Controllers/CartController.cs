using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using Phonemax.Models.Viewmodel;
using Phonemax.uitility;
using Stripe;
using System.Security.Claims;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Phonemax.dataaccess.Repository;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Phonemax.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly Iunitofwork _unitofwork;
        public readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailsender;
       


        private static bool isemailconfirm = false;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(Iunitofwork unitofwork,ApplicationDbContext context, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unitofwork = unitofwork;
            _context = context;
            _userManager = userManager;
            _emailsender= emailSender;
          
           
        }
        [BindProperty]
        public ShoppingcartVm shoppingcartVm { get; set; }
        public IActionResult Index()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                shoppingcartVm = new ShoppingcartVm()
                {
                    listcart = new List<Shoppingcart>()
                };
                return View(shoppingcartVm);
            }
            shoppingcartVm= new ShoppingcartVm()
            {
                listcart = _unitofwork.shop.GetAll(sc => sc.ApplicationUserId == claim.Value, includeproperties: "Product"),
                orderheader=new Orderheader()                         
            };
            shoppingcartVm.orderheader.OrderTotal = 0;
        shoppingcartVm.orderheader.applicationuser = _unitofwork.applicationuser.FirstOrDefault(u => u.Id == claim.Value);
            foreach (var list in shoppingcartVm.listcart)
            {
                list.price = Sd.GetpricebasedonQuantity(list.count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                shoppingcartVm.orderheader.OrderTotal += (list.price * list.count);
                if (list.Product.Discription.Length > 100)
                {
                    list.Product.Discription = list.Product.Discription.Substring(0, 99) + "..........";
                }
            }
            //*******
            if (!isemailconfirm)
            {
                ViewBag.EmailMessage = "Email has been sent kindly verify your email!!!";
                ViewBag.EmailCSS = "text-success";
                isemailconfirm = false;
            }
            else
            {
                ViewBag.EmailMessage = "Email must be confirm for authorize customer!!!";
                ViewBag.EmailCSS = "text-danger";
            }

            return View(shoppingcartVm);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Index")]
        public async Task<IActionResult> Indexpost()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _unitofwork.applicationuser.FirstOrDefault(u => u.Id == claim.Value);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email is empty");
            }
            else
            {
                //email
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code },
                    protocol: Request.Scheme);

                await _emailsender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                isemailconfirm = true;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult plus(int id)
        {
            var cart = _unitofwork.shop.FirstOrDefault(sc => sc.Id == id);
            cart.count += 1;
            _unitofwork.save();
            return RedirectToAction("Index");
        }
        public IActionResult minus(int id)
        {
            var cart = _unitofwork.shop.FirstOrDefault(sc => sc.Id == id);
            if (cart.count == 1)
                cart.count = 1;
            else
                cart.count -= 1;
            _unitofwork.save();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var cart = _unitofwork.shop.FirstOrDefault(sc => sc.Id == id);
            _unitofwork.shop.Remove(cart);
            _unitofwork.save();
            //session
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitofwork.shop.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(Sd.Ss_CartSessionCount, count);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Summary()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingcartVm = new ShoppingcartVm()
            {
                orderheader = new Orderheader(),
                listcart = _unitofwork.shop.GetAll(sc => sc.ApplicationUserId == claim.Value, includeproperties: "Product")
            };
            shoppingcartVm.orderheader.applicationuser = _unitofwork.applicationuser.FirstOrDefault(u => u.Id == claim.Value);
            foreach (var list in shoppingcartVm.listcart)
            {
                list.price = Sd.GetpricebasedonQuantity(list.count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                shoppingcartVm.orderheader.OrderTotal += (list.count * list.price);
                list.Product.Discription = Sd.ConvertToRawHtml(list.Product.Discription);
            }
            shoppingcartVm.orderheader.Name = shoppingcartVm.orderheader.applicationuser.Name;
            shoppingcartVm.orderheader.StreetAddress = shoppingcartVm.orderheader.applicationuser.StreetAddress;
            shoppingcartVm.orderheader.State = shoppingcartVm.orderheader.applicationuser.State;
            shoppingcartVm.orderheader.City = shoppingcartVm.orderheader.applicationuser.City;
            shoppingcartVm.orderheader.PostalCode = shoppingcartVm.orderheader.applicationuser.PostalCode;
            shoppingcartVm.orderheader.PhoneNumber = shoppingcartVm.orderheader.applicationuser.PhoneNumber;


            return View(shoppingcartVm);


        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(string stripetoken)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingcartVm.orderheader.applicationuser = _unitofwork.applicationuser.FirstOrDefault(u => u.Id == claim.Value);
            shoppingcartVm.listcart = _unitofwork.shop.GetAll(sc => sc.ApplicationUserId == claim.Value, includeproperties: "Product");

            shoppingcartVm.orderheader.Carrier = "";
            shoppingcartVm.orderheader.TransactionId = "";
            shoppingcartVm.orderheader.TrackingNumber = "";
            shoppingcartVm.orderheader.PaymentStatus = Sd.paymentstatusPending;
            shoppingcartVm.orderheader.OrderStatus = Sd.orderstatuspending;
            shoppingcartVm.orderheader.OrderDate = DateTime.Now;
            shoppingcartVm.orderheader.ApplicationUserId = claim.Value;
            _unitofwork.orderheader.Add(shoppingcartVm.orderheader);
            _unitofwork.save();
            foreach (var list in shoppingcartVm.listcart)
            {
                list.price = Sd.GetpricebasedonQuantity(list.count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                orderdetail Orderdetail = new orderdetail()
                {

                    ProductId = list.ProductId,
                    OrderHeaderId = shoppingcartVm.orderheader.Id,
                    price = list.price,
                    count = list.count
                };
                shoppingcartVm.orderheader.OrderTotal += (list.price * list.count);
                _unitofwork.orderdetail.Add(Orderdetail);
                _unitofwork.save();
            }
            _unitofwork.shop.RemoveRange(shoppingcartVm.listcart);
            _unitofwork.save();
            //session
            HttpContext.Session.SetInt32(Sd.Ss_CartSessionCount, 0);
            #region stripeprocess
            if (stripetoken == null)
            {
                shoppingcartVm.orderheader.PaymentDueDate = DateTime.Today.AddDays(30);
                shoppingcartVm.orderheader.PaymentStatus = Sd.paymentstatusdelayPayment;
                shoppingcartVm.orderheader.OrderStatus = Sd.orderstatusApproved;
            }
            else
            {
                //payment process
                var option = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(shoppingcartVm.orderheader.OrderTotal),
                    Currency = "usd",
                    Description = "order id:" + shoppingcartVm.orderheader.Id,
                    Source = stripetoken
                };
                //payment
                var service = new ChargeService();
                Charge charge = service.Create(option);
                if (charge.BalanceTransactionId == null)
                {
                    shoppingcartVm.orderheader.PaymentStatus = Sd.paymentstatusRejected;
                }
                else
                {
                    shoppingcartVm.orderheader.TransactionId = charge.BalanceTransactionId;
                }
                if (charge.Status.ToLower() == "succeeded")
                {
                    shoppingcartVm.orderheader.PaymentStatus = Sd.paymentstatusApproved;
                    shoppingcartVm.orderheader.OrderStatus = Sd.orderstatusApproved;
                    shoppingcartVm.orderheader.OrderDate = DateTime.Now;
                }
                _unitofwork.save();

            }
            #endregion

            return RedirectToAction("orderconfirmation", "Cart", new { id = shoppingcartVm.orderheader.Id });
        }
        public async Task <IActionResult> orderconfirmation(int id)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);

            //Twilio
            string accountSid = "AC4fd2f3ab6436384cfc34025e5fd0b07d";
            string authToken = "9f74d9fecfb9836cea001ff9d54a6af1";
            var phoneNumber1 = _unitofwork.applicationuser.FirstOrDefault(x => x.Id == claim.Value);
            string phoneNumber = phoneNumber1.PhoneNumber;
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Your Order is Confirmed And Your Order's id is:" + id,
                from: new Twilio.Types.PhoneNumber("+14302305478"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
                );


            //var user = _unitofwork.applicationuser.FirstOrDefault(u => u.Id == claim.Value);
            //if (user == null)
            //{
            //    ModelState.AddModelError(string.Empty, "Email is empty");
            //}
            //else
            //{
            //    var confirmid = _unitofwork.orderheader.Get(id);
            //    confirmid.OrderStatus=Sd.orderstatusApproved;
            //    var userid=await _userManager.GetUserIdAsync(user);
            //    var code=await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //    await _emailsender.SendEmailAsync(user.Email, "Order Status", $"Order has  been cancel due to some reason"+ $"Product id:{id}");
            //    try
            //    {
            //        var modelMessage = "order confirm";

            //        var result = await _sMSService.SendAsync(modelMessage, user.PhoneNumber);
            //    }
            //    catch (Exception ex)
            //    {
            //        return BadRequest(ex.Message);
            //    }
            //}
            return View(id);
        }
    }
}

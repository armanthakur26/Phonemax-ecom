using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using Phonemax.uitility;
using Company = DocumentFormat.OpenXml.ExtendedProperties.Company;

namespace Phonemax.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Sd.Role_Admin + "," + Sd.Role_Employee)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Iunitofwork _unitofwork;
        public UserController(ApplicationDbContext context,Iunitofwork unitofwork)
        {
            _context = context;
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var userlist = _context.applicationusers.ToList();
            var roles = _context.Roles.ToList();
            var userroles = _context.UserRoles.ToList();
            foreach (var user in userlist)
            {
                var roleid = userroles.FirstOrDefault(r => r.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleid).Name;
                if (user.CompanyId != null)
                {
                    user.company = new Models.Company()
                    {
                        Name = _unitofwork.company.Get(Convert.ToInt32(user.CompanyId)).Name
                    };
                }
                if (user.company == null)
                {
                    user.company = new Models.Company()
                    {
                       Name= ""
                    };
                }
            }
            //remove admin user data from user table;
            var adminuser = userlist.FirstOrDefault(u => u.Role == Sd.Role_Admin);
            userlist.Remove(adminuser);
            return Json(new { data = userlist });
        }
        [HttpPost]
        public IActionResult lockunlock([FromBody] string id)
        {
            bool islocked = false;
            var userindb = _context.applicationusers.FirstOrDefault(u => u.Id == id);
            if (userindb == null)
                return Json(new { success = false, message = "something went wrong while lock and unlock user" });
            if (userindb != null && userindb.LockoutEnd > DateTime.Now)
            {
                userindb.LockoutEnd = DateTime.Now;
                islocked = false;
            }
            else
            {
                userindb.LockoutEnd = DateTime.Now.AddYears(100);
                islocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message = islocked == true ? "User Successfully Locked" : "User successfully Unlock" });
        }
        #endregion
    }
}

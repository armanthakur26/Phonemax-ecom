using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using Phonemax.uitility;

namespace Phonemax.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Sd.Role_Admin + "," + Sd.Role_Employee)]
    public class CompanyController : Controller
    {
        private readonly Iunitofwork _unitofwork;
        public CompanyController(Iunitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Company company=new Company();
            if (id == null) return View(company);
            company = _unitofwork.company.Get(id.GetValueOrDefault());
            if (company == null) return NotFound();
            return View(company);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (company == null) return NotFound();
            if (!ModelState.IsValid) return View(company);
            if (company.Id == 0) _unitofwork.company.Add(company);
            else
                _unitofwork.company.Update(company);
            _unitofwork.save();
            return RedirectToAction(nameof(Index));

        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitofwork.company.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var del = _unitofwork.company.Get(id);
            if (del ==
                null)
                return Json(new { success = false, Message = "Something went wrong....." });
            _unitofwork.company.Remove(del);
            _unitofwork.save();
            return Json(new { success = true, Message = "Data delete successfully " });
        }
#endregion

    }
}

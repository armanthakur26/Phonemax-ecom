using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using Phonemax.uitility;

namespace Phonemax.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Sd.Role_Admin)]
    public class CovertypeController : Controller
    {
        private readonly Iunitofwork _unitofwork;
        public CovertypeController(Iunitofwork Unitofwork)
        {
            _unitofwork = Unitofwork;
        }
        public IActionResult Upsert(int? id)
        {
            Covertype Covertype = new Covertype();
            if (id == null) return View(Covertype);
            Covertype = _unitofwork.Covertype.Get(id.GetValueOrDefault());
            if (Covertype == null) return NotFound();
            return View(Covertype);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(Covertype Covertype)
        {
            if (Covertype == null) return NotFound();
            if (!ModelState.IsValid) return View(Covertype);
            if (Covertype.Id == 0)
                _unitofwork.Covertype.Add(Covertype);
            else
                _unitofwork.Covertype.Update(Covertype);
            _unitofwork.save();
            return RedirectToAction(nameof(Index));


        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitofwork.Covertype.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var covertypeindb = _unitofwork.Covertype.Get(id);
            if (covertypeindb == null) return Json(new { success = false, Message = "Something went wrong" });
            _unitofwork.Covertype.Remove(covertypeindb);
            _unitofwork.save();
            return Json(new { success = true, Message = "Data delete successfully!!!" });
        }
        #endregion
    }
}

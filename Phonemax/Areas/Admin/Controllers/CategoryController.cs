using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phonemax.dataaccess.Repository;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using Phonemax.uitility;

namespace Phonemax.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Sd.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly Iunitofwork _unitofwork;
        public CategoryController(Iunitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null) return View(category);
            category = _unitofwork.Category.Get(id.GetValueOrDefault());
            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (category == null) return NotFound();
            if (!ModelState.IsValid) return View();
            if (category.Id == 0)
                _unitofwork.Category.Add(category);
            else
                _unitofwork.Category.Update(category);
            _unitofwork.save();
            return RedirectToAction(nameof(Index));

        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var category = _unitofwork.Category.GetAll();
            return Json(new { data = category });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryInDb = _unitofwork.Category.Get(id);
            if (categoryInDb == null) return Json(new { success = false, Message = "Something went wrong while delete data!!!" });
            _unitofwork.Category.Remove(categoryInDb);
            _unitofwork.save();
            return Json(new { success = true, Message = "Data delete Successfully!!!" });
        }
        #endregion
    }
}

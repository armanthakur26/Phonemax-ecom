using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using Phonemax.uitility;

namespace Phonemax.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Sd.Role_Admin)]
    public class ProcessorController : Controller
    {
        private readonly Iunitofwork _unitofwork;
        public ProcessorController(Iunitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Processor processor = new Processor();
            if (id == null) return View(processor);
            processor = _unitofwork.Processor.Get(id.GetValueOrDefault());
            if (processor == null) return NotFound();
            return View(processor);

        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(Processor processor)
        {
            if (processor == null) return NotFound();
            if (!ModelState.IsValid) return View(   processor);
            if (processor.Id == 0)
                _unitofwork.Processor.Add(processor);
            else
                _unitofwork.Processor.Update(processor);
            _unitofwork.save();
            return RedirectToAction(nameof(Index));


        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var processor = _unitofwork.Processor.GetAll();
            return Json(new { data = processor });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var processorInDb = _unitofwork.Processor.Get(id);
            if (processorInDb == null) return Json(new { success = false, Message = "Something went wrong while delete data!!!" });
            _unitofwork.Processor.Remove(processorInDb);
            _unitofwork.save();
            return Json(new { success = true, Message = "Data delete Successfully!!!" });
        }
        #endregion
    }
}

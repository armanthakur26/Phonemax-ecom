using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using Phonemax.Models.Viewmodel;
using Phonemax.uitility;
using Product = Phonemax.Models.Product;

namespace Phonemax.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Sd.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly Iunitofwork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(Iunitofwork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;

            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }
      public IActionResult Upsert(int? id)
        {
            ProductVm productVm = new ProductVm()
            {
                product = new Product(),
                categorylist = _unitofwork.Category.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                }),
                covertypelist = _unitofwork.Covertype.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                }),
                processorlist = _unitofwork.Processor.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                })
            };
            if (id == null) return View(productVm);  //Create

            productVm.product = _unitofwork.Product.Get(id.GetValueOrDefault());
            return View(productVm);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(ProductVm productVm)
        {
            if (!ModelState.IsValid)
            {
                var webrootpath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    var Filename = Guid.NewGuid().ToString();
                    var extension = Path.GetExtension(files[0].FileName);
                    var upload = Path.Combine(webrootpath, @"Images\Product");
                    //case 1
                    if (productVm.product.Id != 0)
                    {
                        var imageexists = _unitofwork.Product.Get(productVm.product.Id).ImageUrl;
                        productVm.product.ImageUrl = imageexists;
                    }
                    //case 2
                    if (productVm.product.ImageUrl != null)
                    {
                        var imagepath = Path.Combine(webrootpath, productVm.product.ImageUrl.Trim('\\'));
                        if (System.IO.File.Exists(imagepath))
                        {
                            System.IO.File.Delete(imagepath);
                        }
                    }
                    using (var filestrem = new FileStream(Path.Combine(upload, Filename + extension), FileMode.Create))
                    {
                        files[0].CopyTo(filestrem);
                    }
                    productVm.product.ImageUrl = @"\Images\Product\" + Filename + extension;
                }
                else
                {
                    if (productVm.product.Id != 0)
                    {
                        var imageexists = _unitofwork.Product.Get(productVm.product.Id).ImageUrl;
                        productVm.product.ImageUrl = imageexists;
                    }
                }
                if (productVm.product.Id == 0)
                {
                    _unitofwork.Product.Add(productVm.product);
                }
                else
                    _unitofwork.Product.Update(productVm.product);
                _unitofwork.save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVm = new ProductVm()
                {
                    product = new Product(),
                    categorylist = _unitofwork.Category.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString()
                    }),
                    covertypelist = _unitofwork.Covertype.GetAll().Select(ct => new SelectListItem()
                    {
                        Text = ct.Name,
                        Value = ct.Id.ToString()
                    }),
                    processorlist = _unitofwork.Processor.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString()
                    })
                };
                if (productVm.product.Id != 0)
                {
                    productVm.product = _unitofwork.Product.Get(productVm.product.Id);
                }
                return View(productVm);
            }

        }
        #region apis

        [HttpGet]
        public IActionResult GetAll()
        {
            var productlist = _unitofwork.Product.GetAll(includeproperties: "category,covertype,processor");
            return Json(new {Data= productlist});

        }
        public IActionResult Delete(int id)
        {
            var productindb = _unitofwork.Product.Get(id);
            if (productindb == null)
                return Json(new { success = false, message = "something went wrong while delete data" });
            var webrootpath = _webHostEnvironment.WebRootPath;
            var imagepath = Path.Combine(webrootpath, productindb.ImageUrl.Trim('\\'));
            if (System.IO.File.Exists(imagepath))
            {
                System.IO.File.Delete(imagepath);
            }
            _unitofwork.Product.Remove(productindb);
            _unitofwork.save();
            return Json(new { success = true, message = "Data delete successfully" });
        }
        #endregion
    }
}

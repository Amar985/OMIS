using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineMusic.DataAccess.Data;
using OnlineMusic.DataAccess.Repository;
using OnlineMusic.DataAccess.Repository.IRepository;
using OnlineMusic.Models;
using OnlineMusic.Models.ViewModels;
using OnlineMusicStore.Utility;
using System.Collections.Generic;
using System.Drawing;

namespace OnlineMusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductlist = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            
            
            return View(objProductlist);
        }
        public IActionResult Upsert(int? id)
        {
            // Initialize the view model
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if(id==null ||id==0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file!=null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var filename= new FileStream(Path.Combine(productPath,fileName),FileMode.Create))
                    {
                        file.CopyTo(filename);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;

                }
                bool flag = true;
                if(productVM.Product.Id==0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    flag = false;
                }
                _unitOfWork.Save();
                if (flag)
                { 
                    TempData["Success"] = "Product created successfully!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Success"] = "Product Updated successfully!!!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                    return View(productVM);
            }

        }
       
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productfromDb = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (productfromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productfromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product Deleted Successfully";
        //    return RedirectToAction("Index");

        //}
        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductlist = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductlist });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToDeleted = _unitOfWork.Product.Get(u => u.Id==id);
            if(productToDeleted==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productToDeleted);
            _unitOfWork.Save();
            List<Product> objProductlist = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { success = false, message = "Delete Success" });
        }
        #endregion
    }
}

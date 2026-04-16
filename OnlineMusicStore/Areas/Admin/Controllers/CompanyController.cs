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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }
        public IActionResult Index()
        {
            List<Company> objCompanylist = _unitOfWork.Company.GetAll().ToList();
            
            
            return View(objCompanylist);
        }
        public IActionResult Upsert(int? id)
        {
            // Initialize the view model
            if(id==null ||id==0)
            {
                return View(new Company());
            }
            else
            {
                Company companyobj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyobj);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                bool flag = true;
                if(companyObj.Id==0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                    flag = false;
                }
                _unitOfWork.Save();
                if (flag)
                { 
                    TempData["Success"] = "Company created successfully!!!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Success"] = "Company Updated successfully!!!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                 return View(companyObj);
            }

        }
       
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? productfromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (productfromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productfromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Company? obj = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Company Deleted Successfully";
        //    return RedirectToAction("Index");

        //}
        #region APICALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanylist = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanylist });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToDeleted = _unitOfWork.Company.Get(u => u.Id==id);
            if(productToDeleted==null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Company.Remove(productToDeleted);
            _unitOfWork.Save();
            return Json(new { success = false, message = "Delete Success" });
        }
        #endregion
    }
}

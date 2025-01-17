using tieuthiminhthuong.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace tieuthiminhthuong.Areas.Admin.Controllers
{
    public class AdminBrandController : Controller
    {
        BanHangEntities objBanHangEntities = new BanHangEntities();

        // GET: Admin/AdminBrand
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstBrand = new List<Brand>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                // Lấy danh sách thương hiệu theo từ khóa tìm kiếm
                lstBrand = objBanHangEntities.Brands.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                // Lấy tất cả thương hiệu trong bảng Brand
                lstBrand = objBanHangEntities.Brands.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            lstBrand = lstBrand.OrderByDescending(n => n.Id).ToList();
            return View(lstBrand.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Brand objBrand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    objBrand.CreatedOnUtc = DateTime.Now;
                    objBanHangEntities.Brands.Add(objBrand);
                    objBanHangEntities.SaveChanges();
                    ViewBag.Message = "Brand created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objBrand = objBanHangEntities.Brands.FirstOrDefault(n => n.Id == id);
            if (objBrand == null)
            {
                TempData["ErrorMessage"] = "Brand not found.";
                return RedirectToAction("Index");
            }
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = objBanHangEntities.Brands.FirstOrDefault(n => n.Id == id);
            if (objBrand == null)
            {
                TempData["ErrorMessage"] = "Brand not found.";
                return RedirectToAction("Index");
            }
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Delete(Brand objBrand)
        {
            var brand = objBanHangEntities.Brands.FirstOrDefault(n => n.Id == objBrand.Id);
            if (brand != null)
            {
                objBanHangEntities.Brands.Remove(brand);
                objBanHangEntities.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objBrand = objBanHangEntities.Brands.FirstOrDefault(n => n.Id == id);
            if (objBrand == null)
            {
                TempData["ErrorMessage"] = "Brand not found.";
                return RedirectToAction("Index");
            }
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Edit(Brand objBrand)
        {
            if (ModelState.IsValid)
            {
                objBrand.UpdatedOnUtc = DateTime.Now;
                objBanHangEntities.Entry(objBrand).State = EntityState.Modified;
                objBanHangEntities.SaveChanges();
                ViewBag.Message = "Brand updated successfully.";
                return RedirectToAction("Index");
            }
            return View(objBrand);
        }
    }
}
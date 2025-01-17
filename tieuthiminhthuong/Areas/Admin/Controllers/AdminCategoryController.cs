using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Collections.Generic;
using PagedList;
using tieuthiminhthuong.Context;

namespace tieuthiminhthuong.Areas.Admin.Controllers
{
    public class AdminCategoryController : Controller
    {
        BanHangEntities objBanHangEntities = new BanHangEntities();

        // GET: Admin/AdminCategory
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstCategory = new List<Category>();
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
                // Lấy danh sách danh mục theo từ khóa tìm kiếm
                lstCategory = objBanHangEntities.Categories.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                // Lấy tất cả danh mục trong bảng Category
                lstCategory = objBanHangEntities.Categories.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            lstCategory = lstCategory.OrderByDescending(n => n.Id).ToList();
            return View(lstCategory.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category objCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    objCategory.CreatedOnUtc = DateTime.Now;
                    objBanHangEntities.Categories.Add(objCategory);
                    objBanHangEntities.SaveChanges();
                    ViewBag.Message = "Category created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objCategory = objBanHangEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objCategory = objBanHangEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Delete(Category objCategory)
        {
            var category = objBanHangEntities.Categories.Where(n => n.Id == objCategory.Id).FirstOrDefault();
            objBanHangEntities.Categories.Remove(category);
            objBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objCategory = objBanHangEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Edit(Category objCategory)
        {
            if (ModelState.IsValid)
            {
                objCategory.UpdatedOnUtc = DateTime.Now;
                objBanHangEntities.Entry(objCategory).State = EntityState.Modified;
                objBanHangEntities.SaveChanges();
                ViewBag.Message = "Category updated successfully.";
                return RedirectToAction("Index");
            }
            return View(objCategory);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tieuthiminhthuong.Context;

namespace tieuthiminhthuong.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        BanHangEntities objBanHangEntities = new BanHangEntities();
        public ActionResult Category()
        {
            var ListCategory = objBanHangEntities.Categories.ToList();
            return View(ListCategory);
        }
        public ActionResult ProductCategory(int Id)
        {
            var ListProduct = objBanHangEntities.Products.Where(n => n.CategoryId == Id).ToList();
            return View(ListProduct);
        }
    }
}
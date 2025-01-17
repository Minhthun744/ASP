using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tieuthiminhthuong.Context;

namespace tieuthiminhthuong.Controllers
{
    public class ProductController : Controller
    {
        BanHangEntities objBanHangEntities = new BanHangEntities();
        // GET: Product
        public ActionResult ProductDetail(int Id)
        {
            var objProduct = objBanHangEntities.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(objProduct);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static tieuthiminhthuong.Common;
using tieuthiminhthuong.Context;
using PagedList;

namespace tieuthiminhthuong.Areas.Admin.Controllers
{
    public class AdminProductController : Controller
    {
        BanHangEntities objBanHangEntities = new BanHangEntities();

        // GET: Admin/AdminProduct
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Product>();
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
                // Lấy danh sách sản phẩm theo từ khóa tìm kiếm
                lstProduct = objBanHangEntities.Products.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                // Lấy tất cả sản phẩm trong bảng Product
                lstProduct = objBanHangEntities.Products.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            // Số lượng item của 1 trang = 4
            int pageSize = 3;
            // Số trang = (page ?? 1);
            int pageNumber = (page ?? 1);
            // Sắp xếp theo id sản phẩm, sản phẩm mới đưa lên đầu
            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objProduct.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(objProduct.ImageUpLoad.FileName);
                        fileName = fileName + extension;
                        objProduct.Avatar = fileName;
                        objProduct.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
                    }
                    objProduct.CreatedOnUtc = DateTime.Now;
                    objBanHangEntities.Products.Add(objProduct);
                    objBanHangEntities.SaveChanges();
                    ViewBag.Message = "Product created successfully.";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objProduct);
        }
        void LoadData()
        {
            Common objCommon = new Common();

            //lấy dữ liệu danh mục dưới DB
            var lstCat = objBanHangEntities.Categories.ToList();
            //Convert sang select list dạng value, text
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            //lấy dữ liệu thương hiệu dưới DB
            var lstBrand = objBanHangEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            //Convert sang select list dạng value, text
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //Loai san pham
            List<ProductType> LstproductType = new List<ProductType>();
            ProductType objproductType = new ProductType();
            objproductType.Id = 01;
            objproductType.Name = "Giam gia soc";
            LstproductType.Add(objproductType);

            objproductType = new ProductType();
            objproductType.Id = 02;
            objproductType.Name = "De xuat";
            LstproductType.Add(objproductType);

            DataTable dtProductType = converter.ToDataTable(LstproductType);
            //Convert sang select list dạng value, text
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = objBanHangEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = objBanHangEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objBanHangEntities.Products.Where(n => n.Id == objPro.Id).FirstOrDefault();
            objBanHangEntities.Products.Remove(objProduct);
            objBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var objProduct = objBanHangEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Edit(Product objProduct)
        {
            this.LoadData();
            if (objProduct.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.Avatar = fileName;
                objProduct.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
            }
            objBanHangEntities.Entry(objProduct).State = EntityState.Modified;
            objBanHangEntities.SaveChanges();
            return View(objProduct);
        }
    }
}
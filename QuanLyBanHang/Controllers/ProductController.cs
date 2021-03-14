using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyBanHang.Models;

namespace QuanLyBanHang.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {  
            return View();
        }
        public ActionResult SanPhamPatial()
        {
             var listsp = db.SanPhams.Take(4);
            return PartialView(listsp);
        }
        public ActionResult ChiTietSanPham(int? id)
        {
            if(id==null)
            {
                return HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n=>n.MaSP==id);
            if(sp==null)
            {
                return HttpNotFound();
            }
            return View(sp);
            
        }

        private ActionResult HttpStatusCodeResult(HttpStatusCode badRequest)
        {
            throw new NotImplementedException();
        }
    }
}
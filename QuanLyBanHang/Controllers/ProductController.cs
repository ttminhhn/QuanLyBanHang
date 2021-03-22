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
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX)
        {
            if(MaLoaiSP== null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Load sar pham theo 2 tieu chi ma loai san pham va ma nha san xuat
            var lstSp = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if(lstSp.Count()==0)
            {
                return HttpNotFound();
            }
            return View(lstSp);
        }

        private ActionResult HttpStatusCodeResult(HttpStatusCode badRequest)
        {
            throw new NotImplementedException();
        }
    }
}
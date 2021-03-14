using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyBanHang.Models;

namespace QuanLyBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Brand()
        {
            ;

            var lstBr = from sp in db.SanPhams
                        join br in db.NhaSanXuats on sp.MaNSX equals br.MaNSX
                        where sp.MaNSX == br.MaNSX
                        select new BrandsSl{ TennNhaSanXuat = br.TennNhaSanXuat, SoluongTon = sp.SoluongTon };
                        ViewBag.lstBr = lstBr;

            return PartialView();
        }
        public ActionResult LoaiSanPhamPatial()
        {
            var lstLSP = db.LoaiSanPhams;
            return PartialView(lstLSP);
        }
        public ActionResult NewItem()
        {
            var lstnewItem = db.SanPhams.Where(n => n.Moi == 1).ToList();
            return PartialView(lstnewItem);
        }
        public ActionResult Category()
        {
            var listLSP = db.LoaiSanPhams;
            return PartialView(listLSP);
        }
    }
}
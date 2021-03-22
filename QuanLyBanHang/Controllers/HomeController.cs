using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
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
                        select new BrandsSl { TennNhaSanXuat = br.TennNhaSanXuat, SoluongTon = sp.SoluongTon };
            ViewBag.lstBr = lstBr;

            return PartialView();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
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
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.Cauhoi = new SelectList(LoadCauhoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))// check capcha hợp lệ
            {
                ViewBag.Cauhoi = new SelectList(LoadCauhoi());
                ViewBag.thongbao = "Thêm thành công";
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                return View();
            }
            ViewBag.thongbao = "Sai mã Capcha";
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string tk = f["TaiKhoan"].ToString();
            string mk = f["MatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.Where(x => x.TaiKhoan == tk && x.MatKhau == mk).FirstOrDefault();
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                ViewBag.thongbao = "Đăng nhập thành công";
            }
            else
            {
                ViewBag.thongbao = "Đăng nhập thất bại";
            }
            return View();
        }

        public List<string> LoadCauhoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Bố của bạn tên gì");
            lstCauHoi.Add("Con vật mà bạn yêu thích");
            lstCauHoi.Add("Cầu thủ mà bạn yêu thích");
            return lstCauHoi;
        }
        

    }
}
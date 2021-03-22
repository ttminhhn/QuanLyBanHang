using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyBanHang.Models;

namespace QuanLyBanHang.Controllers
{
   
    public class GioHangController : Controller
    {
        // GET: GioHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // lấy giỏ hàng
        public List<ItemGioHang> LayGioHang()
        {
            //Giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;// gán cho 1 session 
            if(lstGioHang==null)//giỏ hàng chưa tồn tại
            {
                lstGioHang = new List<ItemGioHang>();// khỏi tạo giỏ hàng mới 
                Session["GioHang"] = lstGioHang; // gán session bằng giỏ hàng 
                return lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int MaSP, string strUrl)
        {
            // kiểm tra sp có trong csdl hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if(sp==null)// không thì báo lỗi 
            {
                Response.StatusCode = 404;
                return null;
            }
            //Còn có thì lấy giỏ hàng ra
            List<ItemGioHang> lstGioHang = LayGioHang();// lấy đc giỏ hàng đã đc gán session
            // Kiểm tra Sp đã tồn tại trong list giỏ hàng k 
            ItemGioHang ItemDaCoGioHang = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (ItemDaCoGioHang != null)// Nếu có thì tính các sản phẩm 
            {
                if(sp.SoluongTon< ItemDaCoGioHang.SoLuong)
                {
                    return View("ThongBao");
                }
                ItemDaCoGioHang.SoLuong++;
                ItemDaCoGioHang.ThanhTien = ItemDaCoGioHang.DonGia * ItemDaCoGioHang.SoLuong;
                return Redirect(strUrl);
            }
            else
            {// chưa tồn tại thì tạo
                ItemGioHang itemGH = new ItemGioHang(MaSP);
                if (sp.SoluongTon < itemGH.SoLuong)
                {
                    return View("ThongBao");
                }
                {
                    lstGioHang.Add(itemGH);
                    return Redirect(strUrl);
                }
            }
        }
        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGH = Session["GioHang"] as List<ItemGioHang>;
            if(lstGH==null)
            {
                return 0;
            }
            else
            {
                return lstGH.Sum(n => n.SoLuong);
            }    
        }
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGH = Session["GioHang"] as List<ItemGioHang>;
            if (lstGH == null)
            {
                return 0;
            }
            else
            {
                return lstGH.Sum(n => n.ThanhTien);
            }
        }
        public ActionResult GioHangPatial()
        {
            if(TinhTongSoLuong()==0)
            {
                ViewBag.TongSoTien = 0;
                ViewBag.TongSoLuong = 0;
                return PartialView();
            }
  
                ViewBag.TongSoTien = TinhTongTien();
                ViewBag.TongSoLuong = TinhTongSoLuong();
            return PartialView();

        }
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();

            return View(lstGioHang);
        }
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
          //Kiểm tra session Giỏ hàng
          if(Session["GioHang"]==null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)// không thì báo lỗi 
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();// Lấy giỏ hàng ra
             // Kiểm tra Sp đã tồn tại trong list giỏ hàng k 
            ItemGioHang ItemDaCoGioHang = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (ItemDaCoGioHang == null)// Nếu có thì tính các sản phẩm 
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.GioHang = lstGioHang;
                return View(ItemDaCoGioHang);
        }
        [HttpPost]
        public ActionResult CapNhatGioHang( ItemGioHang itemGH)
        {
            //kiểm tra số lượng tồn tiếp
            SanPham spCheck = db.SanPhams.SingleOrDefault(n => n.MaSP == itemGH.MaSP);
            if(spCheck.SoluongTon<itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            // Cập nhật Session giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang itemUpdate = lstGH.SingleOrDefault(n => n.MaSP == itemGH.MaSP);
            itemUpdate.SoLuong = itemGH.SoLuong;
            //if(itemUpdate.SoLuong==itemGH.SoLuong)
            //{
            //   itemUpdate.SoLuong
            //}    
            itemUpdate.ThanhTien = itemUpdate.SoLuong*itemUpdate.DonGia;

            return RedirectToAction("SuaGioHang",new {@MaSP=itemGH.MaSP});
        }
       
        public ActionResult XoaGioHang(int MaSP)
        {
            //Kiểm tra session Giỏ hàng
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)// không thì báo lỗi 
            {
                Response.StatusCode = 404;
                return null;
            }
            List<ItemGioHang> lstGioHang = LayGioHang();// Lấy giỏ hàng ra
                                                        // Kiểm tra Sp đã tồn tại trong list giỏ hàng k 
            ItemGioHang ItemDaCoGioHang = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (ItemDaCoGioHang == null)// Nếu có thì tính các sản phẩm 
            {
                return RedirectToAction("Index", "Home");
            }
            lstGioHang.Remove(ItemDaCoGioHang);
            return RedirectToAction("XemGioHang");
        }
        public ActionResult DatHang(KhachHang kh)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang KhachHang = new KhachHang();
            if (Session["TaiKhoan"]==null)
            {
              //Them khach hàng vào bảng khách hàng 
                KhachHang = kh;
                db.KhachHangs.Add(KhachHang);
                db.SaveChanges();
            }
            else
            {
                // khách hàng là thành viên
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                KhachHang.TenKH = tv.Hoten;
                KhachHang.DiaChi = tv.DiaChi;
                KhachHang.Email = tv.Email;
                KhachHang.SoDienThoai = tv.SoDienThoai;
                db.KhachHangs.Add(KhachHang);
            }
            // thêm đơn đặt 
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = KhachHang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
            // Lấy giỏ hàng
            List<ItemGioHang> lshGH = LayGioHang();
            foreach(var item in lshGH)
            {
                ChiTietDonDatHang ctddh = new ChiTietDonDatHang();
                ctddh.MaDDH = ddh.MaDDH;
                ctddh.MaSP = item.MaSP;
                ctddh.TenSP = item.TenSP;
                ctddh.SoLuong = item.SoLuong;
                ctddh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctddh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
    }
}
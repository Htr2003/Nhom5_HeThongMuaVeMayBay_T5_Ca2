using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using DoAnCNPMNC.Models;
using System.Data.Entity;

namespace DoAnCNPMNC.Controllers
{
    public class TrangChuController : Controller
    {
        private DBRedNitEntities db = new DBRedNitEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(KhachHang kh)
        {
            if (string.IsNullOrEmpty(kh.HoTen))
                ModelState.AddModelError(string.Empty, " khong duoc de trong ho va ten ");

            if (string.IsNullOrEmpty(kh.GioiTinh))
                ModelState.AddModelError(string.Empty, "gioi tinh kh dc de trong ");

            if (string.IsNullOrEmpty(kh.Sdt))
                ModelState.AddModelError(string.Empty, " khong duoc de trong sdt");

            if(kh.NgaySinh <= DateTime.Now.Date)
            {
                ViewBag.LoiNgaySinh = "Ngày sinh phải trước ngày hiện tại";
            }

            if (string.IsNullOrEmpty(kh.Email))
                ModelState.AddModelError(string.Empty, "khong dc de email trong");

            if (string.IsNullOrEmpty(kh.TenDangNhap))
                ModelState.AddModelError(string.Empty, " khong duoc de trong ten dang nhap");

            if (string.IsNullOrEmpty(kh.MatKhau))
                ModelState.AddModelError(string.Empty, " khong duoc de trong mat khau ");

            if (ModelState.IsValid)
            {
                db.KhachHang.Add(kh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var checkKh = db.KhachHang.FirstOrDefault(k => k.TenDangNhap == kh.TenDangNhap);

            if (checkKh != null)
            {
                ModelState.AddModelError(string.Empty, "Ten dang nhap da duoc dung, vui long chon ten khac!");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(KhachHang acc)
        {
            if (string.IsNullOrEmpty(acc.TenDangNhap))
                ModelState.AddModelError(string.Empty, "Ten dang nhap kh dc de trong");
            if (string.IsNullOrEmpty(acc.MatKhau))
                ModelState.AddModelError(string.Empty, "Mat khau kh dc de trong ");

            var checkAdmin = db.AdminUser.FirstOrDefault(t => t.TenDangNhap == acc.TenDangNhap && t.MatKhau == acc.MatKhau);
            if (checkAdmin != null)
            {
                Session["Admin"] = checkAdmin.AdminID;
                Session["HoTenAdmin"] = checkAdmin.HoTen;
                Session["AdminAccount"] = checkAdmin.TenDangNhap;

                return RedirectToAction("Index", "Users");
            }

            var check = db.KhachHang.Where(k => k.TenDangNhap.Equals(acc.TenDangNhap) && k.MatKhau.Equals(acc.MatKhau)).FirstOrDefault();
            if (check != null)
            {
                Session["KhID"] = check.KhID;
                Session["NameKH"] = check.HoTen;
                Session["TenDangNhap"] = check.TenDangNhap;
                Session["MatKhau"] = check.MatKhau;
                Session["TaiKhoan"] = check;

                return RedirectToAction("Index", "TrangChu");
            }
            else
            {
                ViewBag.ThongBao = "Ten dang nhap hoac mat khau khong dung";
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        public ActionResult DanhSachChuyenBay()
        {
            var chuyenBay = db.ChuyenBay.Include(c => c.SanBay).Include(c => c.SanBay1);
            return View(chuyenBay.ToList());
        }

        [HttpGet]
        public ActionResult ChiTiet(int chuyenBayId)
        {
            var chuyenBay = db.ChuyenBay.Find(chuyenBayId);

            if (chuyenBay == null)
            {
                return HttpNotFound();
            }

            return PartialView("ChiTiet", chuyenBay);
        }

    }
}
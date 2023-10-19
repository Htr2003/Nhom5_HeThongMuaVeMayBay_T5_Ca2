using DoAnCNPMNC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DoAnCNPMNC.Controllers
{
    public class UsersController : Controller
    {
        private DBRedNitEntities db = new DBRedNitEntities();
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DanhSachNguoiDung(string sortOder)
        {
            var Kh = db.KhachHang.OrderByDescending(x => x.HoTen).ToList();
            
            if(sortOder == "HoTen")
            {
                var kh = db.KhachHang.OrderBy(k => k.HoTen).ToList();
            }

            if(sortOder == "NgaySinh")
            {
                var kh = db.KhachHang.OrderBy(k => k.NgaySinh).ToList();
            }

            return View(Kh);
        }


    }
}
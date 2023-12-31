﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnCNPMNC.Models;
using Hangfire;

namespace DoAnCNPMNC.Controllers
{
    public class ChuyenBaysController : Controller
    {
        private DBRedNitEntities db = new DBRedNitEntities();

        // GET: ChuyenBays
        [AutomaticRetry(Attempts = 3)]
        public void UpdateChuyenBayStatusJob()
        {
            using (var context = new DBRedNitEntities())
            {
                context.UpdateChuyenBayStatus();
            }
        }
        public ActionResult Index()
        {
            var chuyenBay = db.ChuyenBay.Include(c => c.SanBay).Include(c => c.SanBay1);
            return View(chuyenBay.ToList());
        }

        // GET: ChuyenBays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenBay chuyenBay = db.ChuyenBay.Find(id);
            if (chuyenBay == null)
            {
                return HttpNotFound();
            }
            return View(chuyenBay);
        }

        // GET: ChuyenBays/Create
        public ActionResult Create()
        {
            ViewBag.SanKhoiHanhID = new SelectList(db.SanBay, "SanBayID", "TenSanBay");
            ViewBag.SanDenID = new SelectList(db.SanBay, "SanBayID", "TenSanBay");
            return View();
        }

        // POST: ChuyenBays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChuyenBayID,SanKhoiHanhID,SanDenID,GioKhoiHanh,GioKetThuc,NgayKhoiHanh,NgayKetThuc,SoGheTrong,SoGheVipTrong,TrangThai")] ChuyenBay chuyenBay)
        {
            if (ModelState.IsValid)
            {
                if (chuyenBay.NgayKhoiHanh < DateTime.Now.Date || chuyenBay.NgayKetThuc < DateTime.Now.Date)
                {
                    ModelState.AddModelError(string.Empty, "Ngày khởi hành và ngày kết thúc không được trước ngày hiện tại");
                }
                else
                {
                    chuyenBay.TrangThai = "Chưa bay";
                    db.ChuyenBay.Add(chuyenBay);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.SanKhoiHanhID = new SelectList(db.SanBay, "SanBayID", "TenSanBay", chuyenBay.SanKhoiHanhID);
            ViewBag.SanDenID = new SelectList(db.SanBay, "SanBayID", "TenSanBay", chuyenBay.SanDenID);
            return View(chuyenBay);
        }

        // GET: ChuyenBays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenBay chuyenBay = db.ChuyenBay.Find(id);
            if (chuyenBay == null)
            {
                return HttpNotFound();
            }
            ViewBag.SanKhoiHanhID = new SelectList(db.SanBay, "SanBayID", "TenSanBay", chuyenBay.SanKhoiHanhID);
            ViewBag.SanDenID = new SelectList(db.SanBay, "SanBayID", "TenSanBay", chuyenBay.SanDenID);
            return View(chuyenBay);
        }

        // POST: ChuyenBays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChuyenBayID,SanKhoiHanhID,SanDenID,GioKhoiHanh,GioKetThuc,NgayKhoiHanh,NgayKetThuc,SoGheTrong,SoGheVipTrong,TrangThai")] ChuyenBay chuyenBay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuyenBay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SanKhoiHanhID = new SelectList(db.SanBay, "SanBayID", "TenSanBay", chuyenBay.SanKhoiHanhID);
            ViewBag.SanDenID = new SelectList(db.SanBay, "SanBayID", "TenSanBay", chuyenBay.SanDenID);
            return View(chuyenBay);
        }

        // GET: ChuyenBays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChuyenBay chuyenBay = db.ChuyenBay.Find(id);
            if (chuyenBay == null)
            {
                return HttpNotFound();
            }
            return View(chuyenBay);
        }

        // POST: ChuyenBays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChuyenBay chuyenBay = db.ChuyenBay.Find(id);
            db.ChuyenBay.Remove(chuyenBay);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet, ActionName("Sort")]
        public ActionResult Sort()
        {
            var sort = db.ChuyenBay.OrderByDescending(x => x.GioKhoiHanh).ToList();
            return View(sort.ToList());
        }

        public ActionResult Search(string query)
        {
            // Add logic to query the ChuyenBay model based on the search query
            // For example, using Entity Framework to query the database
            var searchResults = db.ChuyenBay
                .Where(cb => cb.SanKhoiHanhID.Contains(query) 
                          || cb.SanDenID.Contains(query) 
                          || cb.NgayKhoiHanh.ToString().Contains(query))
                .ToList();

            return View("SearchResults", searchResults);
        }

        [HttpGet]
        public ActionResult DatVe(int chuyenBayId)
        {
            var chuyenbay = db.ChuyenBay.Find(chuyenBayId);
            return View(chuyenbay);
        }

        [HttpPost]
        public ActionResult DatVe(Ve ve, KhachHang kh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(kh.HoTen))
                    ModelState.AddModelError(string.Empty, "Khong duoc de trong ho ten");

                if (string.IsNullOrEmpty(kh.NgaySinh.ToString()))
                    ModelState.AddModelError(string.Empty, "khong duoc de trong ngay sinh");

                if (string.IsNullOrEmpty(kh.Sdt))
                    ModelState.AddModelError(string.Empty, "khong duoc de trong sdt");

                if (Session["NameKH"] != null)
                {
                    var currentKH = (int)Session["KhID"];
                    ve.KhID = currentKH;
                }

                else
                {
                    db.KhachHang.Add(kh);
                    db.SaveChanges();
                    ve.KhID = kh.KhID;
                }

                var chuyenbayID = (int)Session["chuyenbayId"];
                var c = db.ChuyenBay.Find(chuyenbayID);
                ve.ChuyenBayID = c.ChuyenBayID;
                ve.TrangThaiVe = "Đang Chờ";

                db.Ve.Add(ve);
                db.SaveChanges();
                Session["Ve"] = ve.VeID;

                return View("ThanhToan");
            }

            return View();
        }
        public ActionResult ThanhToan()
        {
            return View();
        }

        public ActionResult LichSuVe_Admin()
        {
            var ve = db.Ve.Include("ChuyenBay").Include("KhachHang").ToList();
            return View(ve);
        }

        [HttpPost]
        public ActionResult HoanTatChuyenBay(int veId)
        {
            try
            {
                var ve = db.Ve.Find(veId);

                if (ve == null)
                {
                    return HttpNotFound();
                }
                ve.ChuyenBay.TrangThai = "Hoàn tất";
                if(ve.TrangThaiVe == "Đang chờ")
                {
                    ve.TrangThaiVe = "Hoàn tất";
                    db.SaveChanges();
                }
                db.SaveChanges();

                return View("LichSuVe_Admin");
            }
            catch (Exception ex)
            {
                // Handle exception
                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult LichSuVe_KhachHang()
        {
            var lichsu = db.Ve.Include("ChuyenBay").ToList();
            return View(lichsu);
        }

        [HttpPost]
        public ActionResult HuyDatVe(int veId)
        {
            try
            {
                var ve = db.Ve.Find(veId);

                if (ve == null)
                {
                    // Log the veId to understand what's being passed
                    Console.WriteLine($"Failed to find Ve with ID: {veId}");
                    return Json(new { success = false, error = "Ticket not found." });
                }

                ve.TrangThaiVe = "Hủy";
                db.SaveChanges();

                // Return updated ticket information
                return Json(new { success = true, updatedStatus = ve.TrangThaiVe });
            }
            catch (Exception ex)
            {
                // Log the exception for further investigation
                Console.WriteLine($"Exception while canceling ticket: {ex.Message}");
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}

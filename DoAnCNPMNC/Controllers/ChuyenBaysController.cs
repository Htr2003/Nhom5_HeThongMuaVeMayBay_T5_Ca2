using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnCNPMNC.Models;

namespace DoAnCNPMNC.Controllers
{
    public class ChuyenBaysController : Controller
    {
        private DBRedNitEntities db = new DBRedNitEntities();

        // GET: ChuyenBays
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
                db.ChuyenBay.Add(chuyenBay);
                db.SaveChanges();
                return RedirectToAction("Index");
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
    }
}

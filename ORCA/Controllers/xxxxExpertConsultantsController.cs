using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ORCA.DAL;
using ORCA.Models.OrcaDB;

namespace ORCA.Controllers
{
    public class xxxxExpertConsultantsController : Controller
    {
        private OrcaContext db = new OrcaContext();

        // GET: xxxxExpertConsultants
        public ActionResult Index()
        {
            var expertConsultants = db.ExpertConsultants.Include(e => e.OrcaUser);
            return View(expertConsultants.ToList());
        }

        // GET: xxxxExpertConsultants/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpertConsultant expertConsultant = db.ExpertConsultants.Find(id);
            if (expertConsultant == null)
            {
                return HttpNotFound();
            }
            return View(expertConsultant);
        }

        // GET: xxxxExpertConsultants/Create
        public ActionResult Create()
        {
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName");
            return View();
        }

        // POST: xxxxExpertConsultants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrcaUserID,ExpertStatus,IsActive,TitleDegree,KeyWordList")] ExpertConsultant expertConsultant)
        {
            if (ModelState.IsValid)
            {
                db.ExpertConsultants.Add(expertConsultant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", expertConsultant.OrcaUserID);
            return View(expertConsultant);
        }

        // GET: xxxxExpertConsultants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpertConsultant expertConsultant = db.ExpertConsultants.Find(id);
            if (expertConsultant == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", expertConsultant.OrcaUserID);
            return View(expertConsultant);
        }

        // POST: xxxxExpertConsultants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrcaUserID,ExpertStatus,IsActive,TitleDegree,KeyWordList")] ExpertConsultant expertConsultant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expertConsultant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", expertConsultant.OrcaUserID);
            return View(expertConsultant);
        }

        // GET: xxxxExpertConsultants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpertConsultant expertConsultant = db.ExpertConsultants.Find(id);
            if (expertConsultant == null)
            {
                return HttpNotFound();
            }
            return View(expertConsultant);
        }

        // POST: xxxxExpertConsultants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExpertConsultant expertConsultant = db.ExpertConsultants.Find(id);
            db.ExpertConsultants.Remove(expertConsultant);
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
    }
}

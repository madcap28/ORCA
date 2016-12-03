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

namespace ORCA.Controllers.TESTinCASE
{
    public class zzzTESTinCASEOrcaPasswordsController : Controller
    {
        private OrcaContext db = new OrcaContext();

        // GET: zzzTESTinCASEOrcaPasswords
        public ActionResult Index()
        {
            var orcaPasswords = db.OrcaPasswords.Include(o => o.OrcaUser);
            return View(orcaPasswords.ToList());
        }

        // GET: zzzTESTinCASEOrcaPasswords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrcaPassword orcaPassword = db.OrcaPasswords.Find(id);
            if (orcaPassword == null)
            {
                return HttpNotFound();
            }
            return View(orcaPassword);
        }

        // GET: zzzTESTinCASEOrcaPasswords/Create
        public ActionResult Create()
        {
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName");
            return View();
        }

        // POST: zzzTESTinCASEOrcaPasswords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrcaUserID,Password")] OrcaPassword orcaPassword)
        {
            if (ModelState.IsValid)
            {
                db.OrcaPasswords.Add(orcaPassword);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", orcaPassword.OrcaUserID);
            return View(orcaPassword);
        }

        // GET: zzzTESTinCASEOrcaPasswords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrcaPassword orcaPassword = db.OrcaPasswords.Find(id);
            if (orcaPassword == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", orcaPassword.OrcaUserID);
            return View(orcaPassword);
        }

        // POST: zzzTESTinCASEOrcaPasswords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrcaUserID,Password")] OrcaPassword orcaPassword)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orcaPassword).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", orcaPassword.OrcaUserID);
            return View(orcaPassword);
        }

        // GET: zzzTESTinCASEOrcaPasswords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrcaPassword orcaPassword = db.OrcaPasswords.Find(id);
            if (orcaPassword == null)
            {
                return HttpNotFound();
            }
            return View(orcaPassword);
        }

        // POST: zzzTESTinCASEOrcaPasswords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrcaPassword orcaPassword = db.OrcaPasswords.Find(id);
            db.OrcaPasswords.Remove(orcaPassword);
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

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
    public class zzzTESTinCASEOrcaUsersController : Controller
    {
        private OrcaContext db = new OrcaContext();

        // GET: zzzTESTinCASEOrcaUsers
        public ActionResult Index()
        {
            return View(db.OrcaUsers.ToList());
        }

        // GET: zzzTESTinCASEOrcaUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrcaUser orcaUser = db.OrcaUsers.Find(id);
            if (orcaUser == null)
            {
                return HttpNotFound();
            }
            return View(orcaUser);
        }

        // GET: zzzTESTinCASEOrcaUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: zzzTESTinCASEOrcaUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrcaUserID,OrcaUserName,FirstName,LastName,Email,PhoneNumber,IsAccountDeactivated,UserType")] OrcaUser orcaUser)
        {
            if (ModelState.IsValid)
            {
                db.OrcaUsers.Add(orcaUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orcaUser);
        }

        // GET: zzzTESTinCASEOrcaUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrcaUser orcaUser = db.OrcaUsers.Find(id);
            if (orcaUser == null)
            {
                return HttpNotFound();
            }
            return View(orcaUser);
        }

        // POST: zzzTESTinCASEOrcaUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrcaUserID,OrcaUserName,FirstName,LastName,Email,PhoneNumber,IsAccountDeactivated,UserType")] OrcaUser orcaUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orcaUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orcaUser);
        }

        // GET: zzzTESTinCASEOrcaUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrcaUser orcaUser = db.OrcaUsers.Find(id);
            if (orcaUser == null)
            {
                return HttpNotFound();
            }
            return View(orcaUser);
        }

        // POST: zzzTESTinCASEOrcaUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrcaUser orcaUser = db.OrcaUsers.Find(id);
            db.OrcaUsers.Remove(orcaUser);
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

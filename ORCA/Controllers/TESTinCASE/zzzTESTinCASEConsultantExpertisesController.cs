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
    public class zzzTESTinCASEConsultantExpertisesController : Controller
    {
        private OrcaContext db = new OrcaContext();

        // GET: zzzTESTinCASEConsultantExpertises
        public ActionResult Index()
        {
            var consultantExpertises = db.ConsultantExpertises.Include(c => c.ExpertConsultant).Include(c => c.OrcaUser);
            return View(consultantExpertises.ToList());
        }

        // GET: zzzTESTinCASEConsultantExpertises/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantExpertise consultantExpertise = db.ConsultantExpertises.Find(id);
            if (consultantExpertise == null)
            {
                return HttpNotFound();
            }
            return View(consultantExpertise);
        }

        // GET: zzzTESTinCASEConsultantExpertises/Create
        public ActionResult Create()
        {
            ViewBag.OrcaUserID = new SelectList(db.ExpertConsultants, "OrcaUserID", "TitleDegree");
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName");
            return View();
        }

        // POST: zzzTESTinCASEConsultantExpertises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConsultantExpertiseID,OrcaUserID,FieldOfExpertise")] ConsultantExpertise consultantExpertise)
        {
            if (ModelState.IsValid)
            {
                db.ConsultantExpertises.Add(consultantExpertise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrcaUserID = new SelectList(db.ExpertConsultants, "OrcaUserID", "TitleDegree", consultantExpertise.OrcaUserID);
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", consultantExpertise.OrcaUserID);
            return View(consultantExpertise);
        }

        // GET: zzzTESTinCASEConsultantExpertises/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantExpertise consultantExpertise = db.ConsultantExpertises.Find(id);
            if (consultantExpertise == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrcaUserID = new SelectList(db.ExpertConsultants, "OrcaUserID", "TitleDegree", consultantExpertise.OrcaUserID);
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", consultantExpertise.OrcaUserID);
            return View(consultantExpertise);
        }

        // POST: zzzTESTinCASEConsultantExpertises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConsultantExpertiseID,OrcaUserID,FieldOfExpertise")] ConsultantExpertise consultantExpertise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultantExpertise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrcaUserID = new SelectList(db.ExpertConsultants, "OrcaUserID", "TitleDegree", consultantExpertise.OrcaUserID);
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", consultantExpertise.OrcaUserID);
            return View(consultantExpertise);
        }

        // GET: zzzTESTinCASEConsultantExpertises/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantExpertise consultantExpertise = db.ConsultantExpertises.Find(id);
            if (consultantExpertise == null)
            {
                return HttpNotFound();
            }
            return View(consultantExpertise);
        }

        // POST: zzzTESTinCASEConsultantExpertises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConsultantExpertise consultantExpertise = db.ConsultantExpertises.Find(id);
            db.ConsultantExpertises.Remove(consultantExpertise);
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

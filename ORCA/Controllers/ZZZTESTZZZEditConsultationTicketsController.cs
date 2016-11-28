using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ORCA.DAL;
using ORCA.Models;

namespace ORCA.Controllers
{
    public class ZZZTESTZZZEditConsultationTicketsController : Controller
    {
        private OrcaContext db = new OrcaContext();

        // GET: ZZZTESTZZZEditConsultationTickets
        public ActionResult Index()
        {
            return View(db.TEMPTEST.ToList());
        }

        // GET: ZZZTESTZZZEditConsultationTickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditConsultationTicket editConsultationTicket = db.TEMPTEST.Find(id);
            if (editConsultationTicket == null)
            {
                return HttpNotFound();
            }
            return View(editConsultationTicket);
        }

        // GET: ZZZTESTZZZEditConsultationTickets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZZZTESTZZZEditConsultationTickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,TicketID,OrcaUserName,DTStamp,DescriptionName,IsTicketOpen")] EditConsultationTicket editConsultationTicket)
        {
            if (ModelState.IsValid)
            {
                db.TEMPTEST.Add(editConsultationTicket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(editConsultationTicket);
        }

        // GET: ZZZTESTZZZEditConsultationTickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditConsultationTicket editConsultationTicket = db.TEMPTEST.Find(id);
            if (editConsultationTicket == null)
            {
                return HttpNotFound();
            }
            return View(editConsultationTicket);
        }

        // POST: ZZZTESTZZZEditConsultationTickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,TicketID,OrcaUserName,DTStamp,DescriptionName,IsTicketOpen")] EditConsultationTicket editConsultationTicket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(editConsultationTicket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(editConsultationTicket);
        }

        // GET: ZZZTESTZZZEditConsultationTickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditConsultationTicket editConsultationTicket = db.TEMPTEST.Find(id);
            if (editConsultationTicket == null)
            {
                return HttpNotFound();
            }
            return View(editConsultationTicket);
        }

        // POST: ZZZTESTZZZEditConsultationTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EditConsultationTicket editConsultationTicket = db.TEMPTEST.Find(id);
            db.TEMPTEST.Remove(editConsultationTicket);
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

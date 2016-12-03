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
    public class zzzTESTinCASETicketExpertsController : Controller
    {
        private OrcaContext db = new OrcaContext();

        // GET: zzzTESTinCASETicketExperts
        public ActionResult Index()
        {
            var ticketExperts = db.TicketExperts.Include(t => t.ExpertConsultant).Include(t => t.OrcaUser).Include(t => t.Ticket);
            return View(ticketExperts.ToList());
        }

        // GET: zzzTESTinCASETicketExperts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketExpert ticketExpert = db.TicketExperts.Find(id);
            if (ticketExpert == null)
            {
                return HttpNotFound();
            }
            return View(ticketExpert);
        }

        // GET: zzzTESTinCASETicketExperts/Create
        public ActionResult Create()
        {
            ViewBag.ExpertForThisTicket = new SelectList(db.ExpertConsultants, "OrcaUserID", "TitleDegree");
            ViewBag.ExpertForThisTicket = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName");
            ViewBag.TicketID = new SelectList(db.Tickets, "TicketID", "DescriptionName");
            return View();
        }

        // POST: zzzTESTinCASETicketExperts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketExpertID,TicketID,ExpertForThisTicket,TicketActivityState")] TicketExpert ticketExpert)
        {
            if (ModelState.IsValid)
            {
                db.TicketExperts.Add(ticketExpert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExpertForThisTicket = new SelectList(db.ExpertConsultants, "OrcaUserID", "TitleDegree", ticketExpert.ExpertForThisTicket);
            ViewBag.ExpertForThisTicket = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticketExpert.ExpertForThisTicket);
            ViewBag.TicketID = new SelectList(db.Tickets, "TicketID", "DescriptionName", ticketExpert.TicketID);
            return View(ticketExpert);
        }

        // GET: zzzTESTinCASETicketExperts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketExpert ticketExpert = db.TicketExperts.Find(id);
            if (ticketExpert == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExpertForThisTicket = new SelectList(db.ExpertConsultants, "OrcaUserID", "TitleDegree", ticketExpert.ExpertForThisTicket);
            ViewBag.ExpertForThisTicket = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticketExpert.ExpertForThisTicket);
            ViewBag.TicketID = new SelectList(db.Tickets, "TicketID", "DescriptionName", ticketExpert.TicketID);
            return View(ticketExpert);
        }

        // POST: zzzTESTinCASETicketExperts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketExpertID,TicketID,ExpertForThisTicket,TicketActivityState")] TicketExpert ticketExpert)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketExpert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExpertForThisTicket = new SelectList(db.ExpertConsultants, "OrcaUserID", "TitleDegree", ticketExpert.ExpertForThisTicket);
            ViewBag.ExpertForThisTicket = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticketExpert.ExpertForThisTicket);
            ViewBag.TicketID = new SelectList(db.Tickets, "TicketID", "DescriptionName", ticketExpert.TicketID);
            return View(ticketExpert);
        }

        // GET: zzzTESTinCASETicketExperts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketExpert ticketExpert = db.TicketExperts.Find(id);
            if (ticketExpert == null)
            {
                return HttpNotFound();
            }
            return View(ticketExpert);
        }

        // POST: zzzTESTinCASETicketExperts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketExpert ticketExpert = db.TicketExperts.Find(id);
            db.TicketExperts.Remove(ticketExpert);
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

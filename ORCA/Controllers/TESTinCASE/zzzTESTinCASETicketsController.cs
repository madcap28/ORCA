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
    public class zzzTESTinCASETicketsController : Controller
    {
        private OrcaContext db = new OrcaContext();

        // GET: zzzTESTinCASETickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.OrcaUserCreator).Include(t => t.OrcaUserLastReplied);
            return View(tickets.ToList());
        }

        // GET: zzzTESTinCASETickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: zzzTESTinCASETickets/Create
        public ActionResult Create()
        {
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName");
            ViewBag.OrcaUserIDLastReplied = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName");
            return View();
        }

        // POST: zzzTESTinCASETickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketID,OrcaUserID,OrcaUserIDLastReplied,DTStamp,DescriptionName,IsTicketOpen")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticket.OrcaUserID);
            ViewBag.OrcaUserIDLastReplied = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticket.OrcaUserIDLastReplied);
            return View(ticket);
        }

        // GET: zzzTESTinCASETickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticket.OrcaUserID);
            ViewBag.OrcaUserIDLastReplied = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticket.OrcaUserIDLastReplied);
            return View(ticket);
        }

        // POST: zzzTESTinCASETickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketID,OrcaUserID,OrcaUserIDLastReplied,DTStamp,DescriptionName,IsTicketOpen")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticket.OrcaUserID);
            ViewBag.OrcaUserIDLastReplied = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticket.OrcaUserIDLastReplied);
            return View(ticket);
        }

        // GET: zzzTESTinCASETickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: zzzTESTinCASETickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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

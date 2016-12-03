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
    public class zzzTESTinCASETicketEntriesController : Controller
    {
        private OrcaContext db = new OrcaContext();

        // GET: zzzTESTinCASETicketEntries
        public ActionResult Index()
        {
            var ticketEntries = db.TicketEntries.Include(t => t.OrcaUser).Include(t => t.Ticket);
            return View(ticketEntries.ToList());
        }

        // GET: zzzTESTinCASETicketEntries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketEntry ticketEntry = db.TicketEntries.Find(id);
            if (ticketEntry == null)
            {
                return HttpNotFound();
            }
            return View(ticketEntry);
        }

        // GET: zzzTESTinCASETicketEntries/Create
        public ActionResult Create()
        {
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName");
            ViewBag.TicketID = new SelectList(db.Tickets, "TicketID", "DescriptionName");
            return View();
        }

        // POST: zzzTESTinCASETicketEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketEntryID,TicketID,OrcaUserID,EntryDTStamp,EntryText")] TicketEntry ticketEntry)
        {
            if (ModelState.IsValid)
            {
                db.TicketEntries.Add(ticketEntry);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticketEntry.OrcaUserID);
            ViewBag.TicketID = new SelectList(db.Tickets, "TicketID", "DescriptionName", ticketEntry.TicketID);
            return View(ticketEntry);
        }

        // GET: zzzTESTinCASETicketEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketEntry ticketEntry = db.TicketEntries.Find(id);
            if (ticketEntry == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticketEntry.OrcaUserID);
            ViewBag.TicketID = new SelectList(db.Tickets, "TicketID", "DescriptionName", ticketEntry.TicketID);
            return View(ticketEntry);
        }

        // POST: zzzTESTinCASETicketEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketEntryID,TicketID,OrcaUserID,EntryDTStamp,EntryText")] TicketEntry ticketEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrcaUserID = new SelectList(db.OrcaUsers, "OrcaUserID", "OrcaUserName", ticketEntry.OrcaUserID);
            ViewBag.TicketID = new SelectList(db.Tickets, "TicketID", "DescriptionName", ticketEntry.TicketID);
            return View(ticketEntry);
        }

        // GET: zzzTESTinCASETicketEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketEntry ticketEntry = db.TicketEntries.Find(id);
            if (ticketEntry == null)
            {
                return HttpNotFound();
            }
            return View(ticketEntry);
        }

        // POST: zzzTESTinCASETicketEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketEntry ticketEntry = db.TicketEntries.Find(id);
            db.TicketEntries.Remove(ticketEntry);
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

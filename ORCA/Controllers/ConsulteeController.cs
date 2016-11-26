using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace ORCA.Controllers
{
    public class ConsulteeController : Controller
    {
        public ActionResult Index([Bind]string sortOrder, [Bind]string searchString)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            if (String.IsNullOrEmpty(sortOrder))
                if (TempData["SortOrder"] != null)
                    sortOrder = TempData["SortOrder"].ToString();
            if (String.IsNullOrEmpty(searchString))
                if (TempData["SearchString"] != null)
                    searchString = TempData["SearchString"].ToString();




            if (String.IsNullOrEmpty(sortOrder)) sortOrder = SortBy.FieldOfExpertise.ToString();

            ViewBag.FieldOfExpertiseSortParam = sortOrder == SortBy.FieldOfExpertise.ToString() ? "FieldOfExpertise_desc" : SortBy.FieldOfExpertise.ToString();
            ViewBag.TitleDegreeSortParam = sortOrder == SortBy.TitleDegree.ToString() ? "TitleDegree_desc" : SortBy.TitleDegree.ToString();
            ViewBag.OrcaUserNameSortParam = sortOrder == SortBy.OrcaUserName.ToString() ? "OrcaUserName_desc" : SortBy.OrcaUserName.ToString();
            ViewBag.FirstNameSortParam = sortOrder == SortBy.FirstName.ToString() ? "FirstName_desc" : SortBy.FirstName.ToString();
            ViewBag.LastNameSortParam = sortOrder == SortBy.LastName.ToString() ? "LastName_desc" : SortBy.LastName.ToString();

            ActiveExperts activeExperts = new ActiveExperts();

            switch (sortOrder)
            {
                case "OrcaUserName":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.OrcaUserName, SortMethod.Ascending);
                    break;
                case "TitleDegree":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.TitleDegree, SortMethod.Ascending);
                    break;
                case "FirstName":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.FirstName, SortMethod.Ascending);
                    break;
                case "LastName":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.LastName, SortMethod.Ascending);
                    break;
                case "FieldOfExpertise_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.FieldOfExpertise, SortMethod.Descending);
                    break;
                case "OrcaUserName_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.OrcaUserName, SortMethod.Descending);
                    break;
                case "TitleDegree_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.TitleDegree, SortMethod.Descending);
                    break;
                case "FirstName_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.FirstName, SortMethod.Descending);
                    break;
                case "LastName_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.LastName, SortMethod.Descending);
                    break;
                default: // case "FieldOfExpertise":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.FieldOfExpertise, SortMethod.Ascending);
                    break;
            }

            ViewBag.SortOrder = sortOrder;
            //ViewBag.SearchString = searchString;

            return View(activeExperts);
        }

        public ActionResult About()
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            return View();
        }




        public ActionResult UserProfile()
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            // pre-populate the profile info for the vew, THIS ASSUMES THE SESSION INFO IS SAVED
            UserProfile userProfile = new UserProfile((int)Session["OrcaUserID"]);

            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult UserProfile([Bind(Include = "OrcaUserID,OrcaUserName,FirstName,LastName,Email,PhoneNumber")] UserProfile profileInfo)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            if (ModelState.IsValid)
            {
                // save the profile changes
                if (OrcaHelper.ChangeUserProfileInfo(profileInfo) != null)
                {
                    // update the session variables that may have changed (really need to get the time to find out how to do this correctly but this clunky work-around will have to suffice for now)
                    //Session["OrcaUserID"] = userQuery.OrcaUserID;
                    //Session["OrcaUserName"] = userQuery.OrcaUserName;
                    Session["FirstName"] = profileInfo.FirstName;
                    Session["LastName"] = profileInfo.LastName;
                    //Session["UserType"] = userQuery.UserType;

                    ViewBag.Message += " Changes have been saved.";
                    TempData["Message"] = " Changes have been saved.";

                    return RedirectToAction("UserProfile");
                }
                else
                {
                    ViewBag.Message += " A problem occured and changes were not saved.";
                }
            }
            else
            {
                ViewBag.Message += " Unable to save changes. Please review your changes.";
            }

            return View(profileInfo);
        }

        
        public ActionResult Consultations(string sortOrder, string selectionFilter)
        {
            /*
             * 
             * NOTE: There is a bug in that when repeatedly clicking on the filter buttons
             *          the sort order will be toggled between ascending and descending. I'm
             *          not going to worry about it until I get finished.
             * 
             */

            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            string rcvSortOrder, rcvOldSortOrder, sndSortOrder, sndOldSortOrder;

            // catch selectionFilter to pass between View and Controller
            if (TempData["SelectionFilter"] != null) { selectionFilter = TempData["SelectionFilter"].ToString(); }
            selectionFilter = String.IsNullOrEmpty(selectionFilter) ? ConsultationTicketSelection.All.ToString() : selectionFilter;// check for first time and set selectionFilter

            // catch sortOrder, get it through method arguments, if not get it through TempData, if stil not then set default
            rcvSortOrder = String.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            if (String.IsNullOrEmpty(rcvSortOrder) && TempData["SortOrder"] != null) { rcvSortOrder = TempData["SortOrder"].ToString(); }
            rcvSortOrder = String.IsNullOrEmpty(rcvSortOrder) ? SortBy.TicketID.ToString() : rcvSortOrder;

            if (TempData["OldSortOrder"] != null) { rcvOldSortOrder = TempData["OldSortOrder"].ToString(); }
            else { rcvOldSortOrder = SortBy.IsTicketOpen.ToString(); }// check for first time and set oldSortOrder

            
            // convert the string representations back to the enum representation
            ConsultationTicketSelection selectFilt = (ConsultationTicketSelection)Enum.Parse(typeof(ConsultationTicketSelection), selectionFilter, true);

            UserConsultations userConsultations;

            int userId = Convert.ToInt32(Session["OrcaUserID"].ToString());

            sndSortOrder = rcvSortOrder;

            switch (rcvSortOrder)
            {
                case "TicketID":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "TicketID_desc"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.TicketID, SortMethod.Ascending);
                    break;
                case "TicketID_desc":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "TicketID"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.TicketID, SortMethod.Descending);
                    break;
                case "DescriptionName":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "DescriptionName_desc"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.DescriptionName, SortMethod.Ascending);
                    break;
                case "DescriptionName_desc":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "DescriptionName"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.DescriptionName, SortMethod.Descending);
                    break;
                case "DTStamp":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "DTStamp_desc"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.DTStamp, SortMethod.Ascending);
                    break;
                case "DTStamp_desc":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "DTStamp"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.DTStamp, SortMethod.Descending);
                    break;
                case "OrcaUserIDLastReplied":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "OrcaUserIDLastReplied_desc"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.OrcaUserIDLastReplied, SortMethod.Ascending);
                    break;
                case "OrcaUserIDLastReplied_desc":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "OrcaUserIDLastReplied"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.OrcaUserIDLastReplied, SortMethod.Descending);
                    break;
                case "IsTicketOpen":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "IsTicketOpen_desc"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.IsTicketOpen, SortMethod.Ascending);
                    break;
                case "IsTicketOpen_desc":
                    if (rcvSortOrder == rcvOldSortOrder) { sndSortOrder = "IsTicketOpen"; }
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.IsTicketOpen, SortMethod.Descending);
                    break;
                default:
                    sndSortOrder = "DTStamp_desc";
                    userConsultations = new UserConsultations(userId, selectFilt).SortListBy(SortBy.TicketID, SortMethod.Ascending);
                    break;
            }

            sndOldSortOrder = rcvSortOrder;

            ViewBag.SortOrder = sndSortOrder;
            ViewBag.OldSortOrder = sndOldSortOrder;
            ViewBag.SelectionFilter = selectionFilter;

            return View(userConsultations);
        }



        public ActionResult CreateConsultationTicket()
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }
            CreateConsultationTicket ticket = new CreateConsultationTicket();

            return View(ticket);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConsultationTicket([Bind(Include = "DescriptionName,DescriptionDetails")] CreateConsultationTicket ticket)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }
            
            if (ModelState.IsValid)
            {
                OrcaContext db = new OrcaContext();

                int oid = Convert.ToInt32(Session["OrcaUserID"].ToString());

                // make sure the user hasn't already created a ticket with the same name
                if (!db.Tickets.Any(tic => (tic.OrcaUserID == oid && tic.DescriptionName == ticket.DescriptionName)))
                {
                    Ticket newTicket = new Ticket();
                    TicketEntry newTicketEntry = new TicketEntry();
                    DateTime dtStamp = DateTime.Now;
                    
                    // enter the values for the ticket
                    newTicket.OrcaUserID = newTicket.OrcaUserIDLastReplied = Convert.ToInt32(Session["OrcaUserID"].ToString());
                    newTicket.DTStamp = dtStamp;
                    newTicket.DescriptionName = ticket.DescriptionName;
                    newTicket.IsTicketOpen = true;
                    
                    db.Tickets.Add(newTicket);
                    db.SaveChanges();

                    // enter the values for the first ticket entry
                    newTicketEntry.TicketID = newTicket.TicketID;
                    newTicketEntry.OrcaUserID = newTicket.OrcaUserID;
                    newTicketEntry.EntryDTStamp = dtStamp;
                    newTicketEntry.EntryText = ticket.DescriptionDetails;

                    db.TicketEntries.Add(newTicketEntry);
                    db.SaveChanges();
                    

                    return RedirectToAction("EditConsultationTicket", new { newTicket.TicketID, id = newTicket.TicketID });

                    // DOUBLE NOTE: Leaving this here for now, this was annoying and still doesn't work as I would like it, or maybe it does. 
                    // NOTE: The following RedirectToAction takes me to the right url but the id value is null so I will pass it in TempData and pick it up in the EditConsultationTicket
                    // NOTE: Need to do this differently, but will have to do this workaround for now. I think the RoutConfig.cs needs to be edited in App_Start but I'm not sure.

                    //TempData["TicketID"] = newTicket.TicketID;

                    //return RedirectToAction("EditConsultationTicket", new { id = newTicket.TicketID } );
                    //("EditConsultationTicket", new RouteValueDictionary( new { Controller = "Consultee", Action = "EditConsultationTicket", Id = newTicket.TicketID }));//

                    //// the following seems to mostly work as needed, but there is something odd
                    //int id = newTicket.TicketID;
                    //return RedirectToAction("EditConsultationTicket", new { id = id , newTicket.TicketID} );
                    //// the above seems to mostly work as needed, but there is something odd
                    //int id = newTicket.TicketID;
                    //return RedirectToAction("EditConsultationTicket", new { newTicket.TicketID, id = id });
                }
                else
                {
                    ViewBag.Message += "You have previously created a ticket with the same Short Description/Name.  Please, either edit that ticket or change the Short Discription/Name of the new consultation ticket.";
                }
            }
            
            return View(ticket);
        }


        [HttpGet]
        public ActionResult EditConsultationTicket(int ticketId)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }
            //if (ticketId == null) ViewBag.Message = "ticketid = null";
            //else ViewBag.Message = "ticketId = " + ticketId;
            //ViewBag.Message = "ticketId = " + ticketId;


            return View();
        }



        public ActionResult ChangePassword()
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "CurrentPassword,Password,ConfirmPassword")] PasswordChange passwordChange)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            OrcaContext db = new OrcaContext();

            if (ModelState.IsValid)
            {
                // change password for logged in user and get the success status
                PasswordChangeStatus status = OrcaHelper.ChangePassword(Convert.ToInt32(Session["OrcaUserID"].ToString()), passwordChange);

                switch (status)
                {
                    case PasswordChangeStatus.SUCCESS:
                        ViewBag.Message += " Your password has been changed.";
                        break;
                    case PasswordChangeStatus.INVALID_PASSWORD:
                        ViewBag.Message += " The Current Password you entered was incorrect. Please try again";
                        break;
                    case PasswordChangeStatus.INVALID_USER:
                    default:
                        ViewBag.Message += " Something went wrong. This may suggest an Invalid User login.";
                        break;
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["OrcaUserID"] = null;
            Session["OrcaUserName"] = null;
            Session["FirstName"] = null;
            Session["LastName"] = null;
            Session["UserType"] = null;

            TempData["Message"] = "You have successfully Logged Out.";
            return RedirectToAction("Index", "Home");
        }




        public ActionResult ExpertRequest()
        {
            OrcaContext db = new OrcaContext();

            int userId = (int)Convert.ToInt32(Session["OrcaUserID"].ToString());

            ExpertConsultant requestingUser = db.ExpertConsultants.Find(userId);

            if (requestingUser == null)
            {
                requestingUser = new ExpertConsultant();
                requestingUser.OrcaUserID = userId;
                requestingUser.IsActive = true;
                requestingUser.ExpertStatus = ExpertStatus.Requested;

                db.ExpertConsultants.Add(requestingUser);
                db.SaveChanges();

                ViewBag.Message = "A request to become a Marshall Expert Consultant has been submitted.  Please make sure that your name and contact information under Profile is accurate. Your request will be reviewed, and an administrator may contact you for further information.  If your request is approved then your account will be promoted.";
            }
            else
            {
                switch (requestingUser.ExpertStatus)
                {
                    case ExpertStatus.Requested:
                        ViewBag.Message = "A previous request to become a Marshall Expert Consultant has already been submitted.  Please be patient as the request must be reviewed.";
                        break;
                    case ExpertStatus.Declined:
                        ViewBag.Message = "Your previous request to become a Marshall Expert Consultant has been declined.";
                        break;
                    case ExpertStatus.Approved:
                        ViewBag.Message = "You have been approved as a Marshall Expert Consultant.  You will need to log out and log back in to access additional features of your new account status.";
                        break;
                    default:
                        ViewBag.Message = "An unknown problem has occured and your request cannot be processed at this time.";
                        break;
                }
            }
            
            return View();
        }
        
    }
}

using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


/* 
 *  
 *  These are all the actions methods that are common to Consultee, Consultant, and ConsultantAdmin
 *  
 *  Inherits all action methods from BaseController that includes actions that doesn't require login
 *  
 */



namespace ORCA.Controllers
{
    public class BaseConsultController : BaseController
    {

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
            return RedirectToAction("Login", "Home");
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

                ConsultantStatusRequest request = db.ConsultantStatusRequests.Find(userId);

                if (request == null)
                {
                    request = new ConsultantStatusRequest();
                    request.OrcaUserID = userId;
                    request.RequestingStatus = true;

                    db.ConsultantStatusRequests.Add(request);
                    db.SaveChanges();
                }
                else
                {
                    request.OrcaUserID = userId;
                    request.RequestingStatus = true;

                    db.Entry(request).State = EntityState.Modified;
                    db.SaveChanges();
                }

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








        /*
         * 
         * THIS BEGINS THE CRAP 
         * 
         */




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



        //public ActionResult Consults()
        //{

        //    return View();
        //}








        /*
         * 
         * THIS BEGINS THE A MOUNTAIN OF EXPLODING CRAP THAT NEEDS TO BE REDONE ASAP
         * 
         */


        [HttpGet]
        public ActionResult EditConsultationTicket(int ticketId)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            OrcaContext db = new OrcaContext();

            EditConsultationTicket ticketToEdit = new Models.EditConsultationTicket(ticketId);// this is the ticket we will edit

            //System.Diagnostics.Debug.WriteLine("THIS IS IN EditConsultationTicket(int ticketiD)");
            //System.Diagnostics.Debug.WriteLine(ticketToEdit.TicketID.ToString());
            //System.Diagnostics.Debug.WriteLine(ticketToEdit.OrcaUserName);
            //System.Diagnostics.Debug.WriteLine(ticketToEdit.DTStamp.ToString());
            //System.Diagnostics.Debug.WriteLine(ticketToEdit.DescriptionName);
            //System.Diagnostics.Debug.WriteLine(ticketToEdit.NewTicketEntry);
            //System.Diagnostics.Debug.WriteLine(ticketToEdit.CurrentTicketExperts.ToString());
            //System.Diagnostics.Debug.WriteLine(ticketToEdit.ExpertsToAdd.ToString());
            //System.Diagnostics.Debug.WriteLine(ticketToEdit.TicketEntries.ToString());
            //System.Diagnostics.Debug.WriteLine("THIS IS IN EditConsultationTicket(int ticketiD)");

            //System.Diagnostics.Debug.WriteLine("THIS IS IN BEFORE View(ticketToEdit)");


            return View(ticketToEdit);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConsultationTicket([Bind(Exclude = "TicketID,OrcaUserName,DTStamp,DescriptionName,IsTicketOpen,NewTicketEntry,CurrentTicketExperts,ExpertsToAdd,TicketEntries")] EditConsultationTicket editConsultationTicket)
        {
            //System.Diagnostics.Debug.WriteLine("THIS IS IN EditConsultationTicket(EditConsultationTicket)");
            //System.Diagnostics.Debug.WriteLine(editConsultationTicket.TicketID.ToString());
            //System.Diagnostics.Debug.WriteLine(editConsultationTicket.OrcaUserName);
            //System.Diagnostics.Debug.WriteLine(editConsultationTicket.DTStamp.ToString());
            //System.Diagnostics.Debug.WriteLine(editConsultationTicket.DescriptionName);
            //System.Diagnostics.Debug.WriteLine(editConsultationTicket.NewTicketEntry);
            //System.Diagnostics.Debug.WriteLine(editConsultationTicket.CurrentTicketExperts.ToString());
            //System.Diagnostics.Debug.WriteLine(editConsultationTicket.ExpertsToAdd.ToString());
            //System.Diagnostics.Debug.WriteLine(editConsultationTicket.TicketEntries.ToString());
            //System.Diagnostics.Debug.WriteLine("THIS IS IN EditConsultationTicket(EditConsultationTicket)");
            System.Diagnostics.Debug.WriteLine("THIS IS IN BEFORE if (MedelState.IsValid");

            

            if (ModelState.IsValid)
            {
                //System.Diagnostics.Debug.WriteLine("THIS IS IN BEFORE return RedirectToAction");

                return RedirectToAction("Index");
            }

            //System.Diagnostics.Debug.WriteLine("THIS IS IN BEFORE View(ticketeditConsultationTicket)");

            return View(editConsultationTicket);
        }


        /*
         * 
         * THIS BEGINS THE CRAP THAT NEEDS TO BE REDONE ASAP
         * 
         */

        public ActionResult AddConsultant(int ticketId, string sortOrder, string searchString)
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


            System.Diagnostics.Debug.WriteLine("public ActionResult AddConsultant(int ticketId, string sortOrder, string searchString)");

            if (String.IsNullOrEmpty(sortOrder)) sortOrder = SortBy.FieldOfExpertise.ToString();

            ViewBag.FieldOfExpertiseSortParam = sortOrder == SortBy.FieldOfExpertise.ToString() ? "FieldOfExpertise_desc" : SortBy.FieldOfExpertise.ToString();
            ViewBag.TitleDegreeSortParam = sortOrder == SortBy.TitleDegree.ToString() ? "TitleDegree_desc" : SortBy.TitleDegree.ToString();
            ViewBag.OrcaUserNameSortParam = sortOrder == SortBy.OrcaUserName.ToString() ? "OrcaUserName_desc" : SortBy.OrcaUserName.ToString();
            ViewBag.FirstNameSortParam = sortOrder == SortBy.FirstName.ToString() ? "FirstName_desc" : SortBy.FirstName.ToString();
            ViewBag.LastNameSortParam = sortOrder == SortBy.LastName.ToString() ? "LastName_desc" : SortBy.LastName.ToString();

            ActiveExperts activeExperts = new ActiveExperts();



            System.Diagnostics.Debug.WriteLine("*  1  **********IMPORTANT********");
            System.Diagnostics.Debug.WriteLine("      Inside    BaseConsultController.cs -> AddConsultant");
            System.Diagnostics.Debug.WriteLine("activeExperts was just created");
            System.Diagnostics.Debug.WriteLine("       activeExperts.Experts.Count = " + activeExperts.Experts.Count);
            System.Diagnostics.Debug.WriteLine("       searchString = " + searchString);


            if (String.IsNullOrWhiteSpace(searchString)) activeExperts.PopulateList();

            System.Diagnostics.Debug.WriteLine("*  2  **********IMPORTANT********");
            System.Diagnostics.Debug.WriteLine("      Inside    BaseConsultController.cs -> AddConsultant");
            System.Diagnostics.Debug.WriteLine("after if (String.IsNullOrWhiteSpace(searchString)) activeExperts.PopulateList();");
            System.Diagnostics.Debug.WriteLine("       activeExperts.Experts.Count = " + activeExperts.Experts.Count);



            //activeExperts.AddInactiveExpertsThatAreStillActiveOnTicket(ticketId);
            activeExperts = activeExperts.RemoveExpertsNotActiveOnTicket(ticketId);

            // IS THIS THE CULPRIT???????
            //if (String.IsNullOrWhiteSpace(searchString)) activeExperts.PopulateList();

            System.Diagnostics.Debug.WriteLine("*  3  **********IMPORTANT********");
            System.Diagnostics.Debug.WriteLine("      Inside    BaseConsultController.cs -> AddConsultant");
            System.Diagnostics.Debug.WriteLine("activeExperts was just updated");
            System.Diagnostics.Debug.WriteLine("       activeExperts.Experts.Count = " + activeExperts.Experts.Count);


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
            ViewBag.TicketID = ticketId;


            System.Diagnostics.Debug.WriteLine("*  4  **********IMPORTANT********");
            System.Diagnostics.Debug.WriteLine("      Inside    BaseConsultController.cs -> AddConsultant");
            System.Diagnostics.Debug.WriteLine("going to View(activeExperts)");
            System.Diagnostics.Debug.WriteLine("       activeExperts.Experts.Count = " + activeExperts.Experts.Count);

            return View(activeExperts);
        }


        public ActionResult AddConsultantToTicket(int consultantId, int ticketId, string sortOrder, string searchString)
        {
            OrcaContext db = new OrcaContext();
            
            List<TicketExpert> ticketExperts = db.TicketExperts.Where(ticEx => ticEx.TicketID == ticketId).ToList();




            System.Diagnostics.Debug.WriteLine("AddConsultantToTicket ticketExperts.count = " + ticketExperts.Count);




            if (ticketExperts.Any(tic => tic.ExpertForThisTicket == consultantId))
            {
                TicketExpert expToEdit = new TicketExpert();
                int ticExpId = ticketExperts.First(ticExp => ticExp.ExpertForThisTicket == consultantId).TicketExpertID;

                expToEdit = db.TicketExperts.Find(ticExpId);

                expToEdit.TicketActivityState = ActivityState.Active;

                db.Entry(expToEdit).State = EntityState.Modified;
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine("AddConsultantToTicket UDATED expToEdit.TicketActivityState ");

            }
            else
            {
                TicketExpert ticketExpert = new TicketExpert();
                ticketExpert.TicketID = ticketId;
                ticketExpert.ExpertForThisTicket = consultantId;
                ticketExpert.TicketActivityState = ActivityState.Active;// need to change

                db.TicketExperts.Add(ticketExpert);
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine("AddConsultantToTicket ADDED ticketExpert");

            }

            return RedirectToAction("EditConsulTationTicket", new { ticketId = ticketId });
        }


        public ActionResult RemoveConsultantFromTicket(int consultantId, int ticketId)
        {
            //System.Diagnostics.Debug.WriteLine("consultantId = " + consultantId + " | ticketId = " + ticketId);

            OrcaContext db = new OrcaContext();
            
            // find the ticket expert associated with the ticket
            TicketExpert gettingId = (from exp in db.TicketExperts
                                      where exp.ExpertForThisTicket == consultantId && exp.TicketID == ticketId
                                      select exp).First();
            int ticexpid = gettingId.TicketExpertID;

            TicketExpert expertToRemove = db.TicketExperts.Find(ticexpid);
            // disable expert on this ticket
            if (expertToRemove != null)
            {
                expertToRemove.TicketActivityState = ActivityState.Inactive;

                // update database update db
                db.Entry(expertToRemove).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return RedirectToAction("EditConsultationTicket", new { ticketId, id = ticketId });
        }








        

    }
}
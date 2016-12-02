using ORCA;
using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.Consultation;
using ORCA.Models.OrcaDB;
//using ORCA.OrcaHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ORCA.Models.Consultation.Consultation;

namespace ORCA.Controllers
{
    public class ConsultantController : BaseConsultController
    {
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
        public ActionResult UserProfile([Bind(Include = "OrcaUserID,OrcaUserName,FirstName,LastName,Email,PhoneNumber,IsActive,TitleDegree,FieldToAdd,KeyWordList")] UserProfile profileInfo)
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
        
        public ActionResult DeleteExpertise(int id)
        {
            OrcaContext db = new OrcaContext();

            ConsultantExpertise expertise = db.ConsultantExpertises.Find(id);
            db.ConsultantExpertises.Remove(expertise);
            db.SaveChanges();

            return RedirectToAction("UserProfile");
        }









        public ActionResult OrcaConsults() { return View(); } // used for testing and in future maybe redirect, or remove it

        [HttpGet]
        public ActionResult Consults()
        {
            Consultation consultsModel=null;

            if (TempData["ConsultationModel"] != null)
            {
                //System.Diagnostics.Debug.WriteLine("************#######************");
                consultsModel = TempData["ConsultationModel"] as Consultation;
            }
            else
            {
                int userId = int.Parse(Session["OrcaUserID"].ToString());

                consultsModel = new Consultation(userId, Consultation.UserAccessType.Consultant);
            }
            
            return View(consultsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Consults([Bind(Include = "OrcaUserID_or_TicketExpertID,UserViewAccess,FilterTicketsOption,SortTicketsOption,TicketSearchString,ConsultationTickets")]Consultation viewModel)
        {

            return View(viewModel);
        }

        public ActionResult ConsultsUpdate(int OrcaUserID_or_TicketExpertID, UserAccessType UserViewAccess, TicketFilterOption FilterTicketsOption, TicketSortOption SortTicketsOption, string TicketSearchString)
        {
            Consultation consultationModel = new Consultation();
            
            consultationModel.OrcaUserID_or_TicketExpertID = OrcaUserID_or_TicketExpertID;
            consultationModel.FilterTicketsOption = FilterTicketsOption;
            consultationModel.SortTicketsOption = SortTicketsOption;
            consultationModel.TicketSearchString = TicketSearchString;

            return RedirectToAction("Consults", new { viewModel = consultationModel });

            //return View();
        }




















        //[HttpGet]
        //public ActionResult Consults(Consultation consultationModel, TicketFilterOption? FilterTicketsOption, TicketSortOption? SortTicketsOption, string TicketSearchString)
        //{
        //if (FilterTicketsOption != null) consultationModel.FilterTicketsOption = (TicketFilterOption)FilterTicketsOption;
        //if (SortTicketsOption != null) consultationModel.SortTicketsOption = (TicketSortOption)SortTicketsOption;
        //if (!String.IsNullOrEmpty(TicketSearchString)) consultationModel.TicketSearchString = TicketSearchString;


        //if (FilterTicketsOption != null) consultationModel.FilterConsultationTickets(FilterTicketsOption);
        //if (SortTicketsOption != null) consultationModel.SortConsultationTickets(SortTicketsOption, true);
        //if (!String.IsNullOrEmpty(TicketSearchString)) consultationModel.SearchConsultationTickets(TicketSearchString);

        //    return View(consultationModel);
        //}




        //public ActionResult Consults(consultationModel)
        //{
        //    if (consultationModel != null)
        //    {

        //        consultationModel.FilterConsultationTickets(filterTicketOption);
        //        return View(consultationModel);

        //    }

        //    OrcaUserType userType;

        //    // check user type 
        //    if (Session["UserType"].ToString() == OrcaUserType.Consultant.ToString())
        //        userType = OrcaUserType.Consultant;
        //    else if (Session["UserType"].ToString() == OrcaUserType.ConsultantAdmin.ToString())
        //        userType = OrcaUserType.ConsultantAdmin;
        //    else
        //        userType = OrcaUserType.Consultee;
        //    // check the user id
        //    int orcaUserId = int.Parse(Session["OrcaUserID"].ToString());

        //    Consultation consults = new Consultation();
        //    consults.Init(orcaUserId, userType);




        //    return View(consults);

        //}



        //[HttpGet]
        //public ActionResult OrcaConsults()
        //{
        //    OrcaContext db = new OrcaContext();


        //    Ticket ticket = db.Tickets.Find(1);

        //    ticket.IsTicketOpen = false;




        //    int stop = 2;

        //    int go = stop = 3;
        //    //System.Diagnostics.Debug.WriteLine("****** stop *****" + stop);

        //    EnumTest test = new EnumTest();


        //    //System.Diagnostics.Debug.WriteLine("     test.EnumTest_TestPublicEnum = " + test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.Value2;
        //    //System.Diagnostics.Debug.WriteLine("     test.EnumTest_TestPublicEnum = " + test.EnumTest_TestPublicEnum);

        //    test.EnumTest_TestPublicEnum = TestPublicEnum.Value2;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.Value2;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.Value1;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.Value1;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.Value1;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);


        //    test.EnumTest_TestPublicEnum = TestPublicEnum.descValue3;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.descValue3;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.descValue3;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.descValue4;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.descValue4;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);
        //    test.EnumTest_TestPublicEnum = TestPublicEnum.descValue4;
        //    //System.Diagnostics.Debug.WriteLine(test.EnumTest_TestPublicEnum);







        //    int userId = int.Parse(Session["OrcaUserID"].ToString());

        //    Consultation consult = new Consultation(userId, OrcaUserType.Consultant);

        //    ////////System.Diagnostics.Debug.WriteLine("*****************BEGIN TEST******************************");

        //    //////TicketFilterOption testVar;

        //    ////////System.Diagnostics.Debug.WriteLine("   BEFORE GET");
        //    ////////System.Diagnostics.Debug.WriteLine("      consult.FilterTicketsOption = " + consult.FilterTicketsOption);
        //    //////testVar = consult.FilterTicketsOption;

        //    ////////System.Diagnostics.Debug.WriteLine("   AFTER GET / BEFORE GET");
        //    ////////System.Diagnostics.Debug.WriteLine("      consult.FilterTicketsOption = " + consult.FilterTicketsOption);
        //    //////testVar = consult.FilterTicketsOption;

        //    ////////System.Diagnostics.Debug.WriteLine("   AFTER GET / before set");
        //    ////////System.Diagnostics.Debug.WriteLine("      consult.FilterTicketsOption = " + consult.FilterTicketsOption);
        //    //////consult.FilterTicketsOption = testVar;

        //    ////////System.Diagnostics.Debug.WriteLine("   after set / before set");
        //    ////////System.Diagnostics.Debug.WriteLine("      consult.FilterTicketsOption = " + consult.FilterTicketsOption);
        //    //////consult.FilterTicketsOption = testVar;

        //    ////////System.Diagnostics.Debug.WriteLine("   after set / before set");
        //    ////////System.Diagnostics.Debug.WriteLine("      consult.FilterTicketsOption = " + consult.FilterTicketsOption);
        //    //////consult.FilterTicketsOption = testVar;

        //    ////////System.Diagnostics.Debug.WriteLine("   after set");

        //    ////////System.Diagnostics.Debug.WriteLine("********************END TEST***************************");


        //    //////consult.TicketSearchString = "FIRST SEARCH";
        //    //////consult.TicketSearchString = "second search";
        //    //////consult.TicketSearchString = "THIRD SEARCH";


        //    //////consult.SortTicketsOption = TicketSortOption.DescriptionName;
        //    //////consult.SortTicketsOption = TicketSortOption.OrcaUserName;
        //    //////consult.SortTicketsOption = TicketSortOption.OrcaUserName;
        //    //////consult.SortTicketsOption = TicketSortOption.DescriptionName;
        //    //////consult.SortTicketsOption = TicketSortOption.DescriptionName;


        //    return View(consult);
        //}



    }
}












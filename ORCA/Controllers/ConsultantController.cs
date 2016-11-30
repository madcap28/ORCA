using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.Consultation;
using ORCA.Models.OrcaDB;
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

        [HttpGet]
        public ActionResult Consults()
        {
            int userId = int.Parse(Session["OrcaUserID"].ToString());
            string orcaUserType = Session["UserType"].ToString();

            OrcaUserType userType;

            switch (orcaUserType)
            {
                case "Consultee":
                    userType = OrcaUserType.Consultee;
                    break;
                case "Consultant":
                    userType = OrcaUserType.Consultant;
                    break;
                case "ConsultantAdmin":
                    userType = OrcaUserType.ConsultantAdmin;
                    break;
                default:
                    userType = OrcaUserType.Consultee;
                    break;
            }
            Consultation consult = new Consultation(userId, userType);
            return View(consult);

        }
        [HttpPost]
        public ActionResult Consults(Consultation consultationModel)
        {
            return View("consultationModel");
        }




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


    }
}
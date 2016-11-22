using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.OrcaDB;
using ORCA.OrcaHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ORCA.Controllers
{
    public class ConsultantController : Controller
    {
        public ActionResult Index()
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            return View();
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



        //////        //db.Entry(userQuery).State = EntityState.Modified;
        //////        //db.SaveChanges();

        public ActionResult UserProfile()
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            // pre-populate the profile info for the vew, THIS ASSUMES THE SESSION INFO IS SAVED
            // must do a stupid work-around because populating the userProfile will fill in the editable fields but will eliminate any fields that are not editable, i.e. OrcaUserID and OrcaUserName
            ConsultantProfile consultantProfile = new ConsultantProfile((int)Session["OrcaUserID"]);



            //ViewBag.Message += "OrcaUserID = " + consultantProfile.OrcaUserID + " OrcaUserName = " + consultantProfile.OrcaUserName + " FirstName = " + consultantProfile.FirstName + " LastName = " + consultantProfile.LastName + " Email = " + consultantProfile.Email + " PhoneNumber = " + consultantProfile.PhoneNumber + " IsActive = " + consultantProfile.IsActive + " TitleDegree = " + consultantProfile.TitleDegree + " KeyWordList = " + consultantProfile.KeyWordList;


            return View(consultantProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile([Bind(Include = "OrcaUserID,OrcaUserName,FirstName,LastName,Email,PhoneNumber,IsActive,TitleDegree,KeyWordList")] ConsultantProfile consultantInfo)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }


            //ViewBag.Message += "OrcaUserID = " + consultantInfo.OrcaUserID + " OrcaUserName = " + consultantInfo.OrcaUserName + " FirstName = " + consultantInfo.FirstName + " LastName = " + consultantInfo.LastName + " Email = " + consultantInfo.Email + " PhoneNumber = " + consultantInfo.PhoneNumber + " IsActive = " + consultantInfo.IsActive + " TitleDegree = " + consultantInfo.TitleDegree + " KeyWordList = " + consultantInfo.KeyWordList;


            // the next two lines are necissary as the values get nullified when passing between UserProfile()->View(userProfile) and UserProfile(UserProfile)
            // it's not the prefered method but the work-around is necissary at the moment

            //consultantInfo.OrcaUserID = (int)Convert.ToInt32(Session["OrcaUserID"].ToString());
            //consultantInfo.OrcaUserName = (string)Session["OrcaUserName"];

            ////int ouid = (int)Convert.ToInt32(Session["OrcaUserID"].ToString());
            ////consultantInfo.UserProfile.OrcaUserID = ouid;
            ////consultantInfo.UserProfile.OrcaUserName = (string)Session["OrcaUserName"];
            ////consultantInfo = new ConsultantProfile((int)Convert.ToInt32(Session["OrcaUserID"].ToString()));
            //// THIS IS BUGGERED i need to learn more and figure out what and how it is dictated what will be retained and when
            //////// FINAL NOTE it seems that the view has to have @Html.HiddenFor(model => model.OrcaUserID) if it isn't being altered by the view, this just doesn't seem to work for the model.FieldsOfExpertise so it seems i'll have to use the dumb work-around after all, at least till i figure out how to do it properly

            //OrcaContext db = new OrcaContext();
            //consultantInfo.FieldsOfExpertise = (from expertise in db.ConsultantExpertises
            //                                    where expertise.OrcaUserID == consultantInfo.OrcaUserID
            //                                    select expertise).ToList();
            //// this didn't do it

            OrcaContext db = new OrcaContext();
            //consultantInfo.FieldsOfExpertise = new List<ConsultantExpertise>();
            consultantInfo.FieldsOfExpertise = new List<ConsultantExpertise>();
            consultantInfo.FieldsOfExpertise = (from expertise in db.ConsultantExpertises
                                                where expertise.OrcaUserID == consultantInfo.OrcaUserID
                                                select expertise).ToList();

            if (ModelState.IsValid)
            {
                //OrcaContext db = new OrcaContext();
                //consultantInfo.FieldsOfExpertise = new List<ConsultantExpertise>();
                //consultantInfo.FieldsOfExpertise = (from expertise in db.ConsultantExpertises
                //                                    where expertise.OrcaUserID == consultantInfo.OrcaUserID
                //                                    select expertise).ToList();

                // save the profile changes
                if (UserProfileInfoChanger.ChangeConsultantProfileInfo(consultantInfo) != null)
                {
                    // update the session variables that may have changed (really need to get the time to find out how to do this correctly but this clunky work-around will have to suffice for now)
                    //Session["OrcaUserID"] = userQuery.OrcaUserID;
                    //Session["OrcaUserName"] = userQuery.OrcaUserName;
                    Session["FirstName"] = consultantInfo.FirstName;
                    Session["LastName"] = consultantInfo.LastName;
                    //Session["UserType"] = userQuery.UserType;

                    ViewBag.Message += " Changes have been saved.";
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

            return View(consultantInfo);
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
        public ActionResult ChangePassword([Bind(Include = "CurrentPassword,Password,ConfirmPassword")] ChangePasswordModel passwordChange)
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
                PasswordChangeStatus status = PasswordChanger.ChangePassword(Convert.ToInt32(Session["OrcaUserID"].ToString()), passwordChange);

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


    }
}
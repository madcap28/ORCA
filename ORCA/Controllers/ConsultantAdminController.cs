using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ORCA.Controllers
{
    public class ConsultantAdminController : Controller
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

    }
}
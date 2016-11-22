using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.OrcaDB;
using ORCA.OrcaHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ORCA.Controllers
{
    public class ConsulteeController : Controller
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
            // must do a stupid work-around because populating the userProfile will fill in the editable fields but will eliminate any fields that are not editable, i.e. OrcaUserID and OrcaUserName
            UserProfile userProfile = new UserProfile((int)Session["OrcaUserID"]);
            
            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult UserProfile([Bind(Exclude = "OrcaUserID,OrcaUserName", Include = "FirstName,LastName,Email,PhoneNumber")] UserProfile profileInfo)
        public ActionResult UserProfile([Bind(Include = "OrcaUserID,OrcaUserName,FirstName,LastName,Email,PhoneNumber")] UserProfile profileInfo)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            
            // the next two lines are necissary as the values get nullified when passing between UserProfile()->View(userProfile) and UserProfile(UserProfile)
            // it's not the prefered method but the work-around is necissary at the moment
            profileInfo.OrcaUserID = (int)Convert.ToInt32(Session["OrcaUserID"].ToString());
            profileInfo.OrcaUserName = (string)Session["OrcaUserName"];
            
            if (ModelState.IsValid)
            {
                // save the profile changes
                if (UserProfileInfoChanger.ChangeUserProfileInfo(profileInfo) != null)
                {
                    // update the session variables that may have changed (really need to get the time to find out how to do this correctly but this clunky work-around will have to suffice for now)
                    //Session["OrcaUserID"] = userQuery.OrcaUserID;
                    //Session["OrcaUserName"] = userQuery.OrcaUserName;
                    Session["FirstName"] = profileInfo.FirstName;
                    Session["LastName"] = profileInfo.LastName;
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
                ViewBag.Message += " Unable to save changes.";
            }
            
            return View(profileInfo);
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
                        ViewBag.Message = "A request to become a Marshall Expert Consultant has previously been submitted and is being reviewed.  Please make sure that your name and contact information under Profile is accurate. Your request will be reviewed, and an administrator may contact you for further information.  If your request is approved then your account will be promoted.";
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

        //public ActionResult UserProfile()
        //{
        //    // pre-populate the profile info for the vew, THIS ASSUMES THE SESSION INFO IS SAVED
        //    UserProfile userProfile = new UserProfile((int)Session["OrcaUserID"]);
        //    //UserProfile userProfile = new UserProfile(Convert.ToInt32(Session["OrcaUserID"].ToString()));

        //    //ViewBag.Message = "Session test " + (int)Convert.ToInt32(Session["OrcaUserID"].ToString());
        //    //ViewBag.Message = "Session test " + (int)Session["OrcaUserID"];
        //    //ViewBag.Message = userProfile.OrcaUserName + " OrcaUserName " + userProfile.OrcaUserID + " OrcaUserID " + userProfile.FirstName + " FirstName " + userProfile.LastName + " LastName " + userProfile.Email + " Email " + userProfile.PhoneNumber + "PhoneNumber";
        //    ViewBag.Message = "OrcaUserID = " + userProfile.OrcaUserID;

        //    return View(userProfile);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////public ActionResult UserProfile([Bind(Exclude = "OrcaUserID,OrcaUserName", Include = "FirstName,LastName,Email,PhoneNumber")] UserProfile profileInfo)
        //public ActionResult UserProfile([Bind(Include = "OrcaUserID,OrcaUserName,FirstName,LastName,Email,PhoneNumber")] UserProfile profileInfo)
        //{
        //    //int id = profileInfo.OrcaUserID;
        //    //ViewBag.Message = profileInfo.OrcaUserName + " = OrcaUserName " + profileInfo.OrcaUserID + " = OrcaUserID " + profileInfo.FirstName + " = FirstName " + profileInfo.LastName + " = LastName " + profileInfo.Email + " = Email " + profileInfo.PhoneNumber + " = PhoneNumber";
        //    OrcaContext db = new OrcaContext();

        //    if (ModelState.IsValid)
        //    {
        //        //int test = Convert.ToInt32(Session["OrcaUserID"].ToString());
        //        //int id2 = profileInfo.OrcaUserID;
        //        //int test = (int)Session["OrcaUserID"];

        //        OrcaUser userQuery = (from user in db.OrcaUsers
        //                              where user.OrcaUserID == test //Convert.ToInt32(Session["OrcaUserID"].ToString())//1// profileInfo.OrcaUserID
        //                              select user).First();

        //        userQuery.OrcaUserName = profileInfo.OrcaUserName;
        //        userQuery.FirstName = profileInfo.FirstName;
        //        userQuery.LastName = profileInfo.LastName;
        //        userQuery.Email = profileInfo.Email;
        //        userQuery.PhoneNumber = profileInfo.PhoneNumber;

        //        //db.Entry(userQuery).State = EntityState.Modified;
        //        //db.SaveChanges();

        //        //ViewBag.Message = "Changes have been saved. " + profileInfo.OrcaUserName + " OrcaUserName " + profileInfo.OrcaUserID + " OrcaUserID " + profileInfo.FirstName + " FirstName " + profileInfo.LastName + " LastName " + profileInfo.Email + " Email " + profileInfo.PhoneNumber + "PhoneNumber";

        //        //ViewBag.Message = "Changes have been saved." + " id = " + id + " id2 = " + id2;
        //        // not sure exactly why, but the profileInfo loses the OrcaUserName and OrcaUserID, so reinitialize it with the proper info from the database
        //        profileInfo = new UserProfile(userQuery.OrcaUserID);

        //        return View(profileInfo);
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Unable to save changes.";
        //    }

        //    return View(profileInfo);
        //}
    }
}

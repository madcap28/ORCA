using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.OrcaDB;
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
            return View();
        }

        public ActionResult About()
        {
            return View();
        }




        public ActionResult UserProfile()
        {
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
            OrcaContext db = new OrcaContext();

            // the next two lines are necissary as the values get nullified when passing between UserProfile()->View(userProfile) and UserProfile(UserProfile)
            // it's not the prefered method but the work-around is necissary at the moment
            profileInfo.OrcaUserID = (int)Session["OrcaUserID"];
            profileInfo.OrcaUserName = (string)Session["OrcaUserName"];
            
            if (ModelState.IsValid)
            {
                // get the user from the database
                OrcaUser userQuery = (from user in db.OrcaUsers
                                      where user.OrcaUserID == profileInfo.OrcaUserID
                                      select user).First();

                // update any allowed changes that may have been made
                userQuery.FirstName = profileInfo.FirstName;
                userQuery.LastName = profileInfo.LastName;
                userQuery.Email = profileInfo.Email;
                userQuery.PhoneNumber = profileInfo.PhoneNumber;

                // update the database
                db.Entry(userQuery).State = EntityState.Modified;
                db.SaveChanges();

                // update the session variables that may have changed (really need to get the time to find out how to do this correctly but this clunky work-around will have to suffice for now)
                //Session["OrcaUserID"] = userQuery.OrcaUserID;
                //Session["OrcaUserName"] = userQuery.OrcaUserName;
                Session["FirstName"] = userQuery.FirstName;
                Session["LastName"] = userQuery.LastName;
                //Session["UserType"] = userQuery.UserType;

                ViewBag.Message = "Changes have been saved.";
            }
            else
            {
                ViewBag.Message = "Unable to save changes.";
            }
            
            return View(profileInfo);
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

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
    public class HomeController : Controller
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




        public ActionResult Login()
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            Login loginInfo = new Login();

            return View(loginInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "OrcaUserName,Password")] Login loginInfo)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            OrcaContext db = new OrcaContext();

            if (ModelState.IsValid)
            {
                OrcaUser userQuery = null;

                // get the OrcaUser, if it doesn't exist this will throw an exception and userQuery will remain null
                try
                {
                    userQuery = (from user in db.OrcaUsers
                                 where user.OrcaUserName == loginInfo.OrcaUserName
                                 select user).First();
                }
                catch (Exception)
                {
                }

                if (userQuery != null)// check to see if the user account exists
                {
                    userQuery.OrcaPassword = (from user in db.OrcaPasswords
                                              where user.OrcaUserID == userQuery.OrcaUserID
                                              select user).First();
                    // NOTE:
                    // NOTE: Need to hash password later.
                    // NOTE:
                    if (userQuery.OrcaPassword.Password == loginInfo.Password)// check the password
                    {
                        // this is not the best way to do it, but knowledge of the language and api, or lack thereof dictates a sloppy work-around for now
                        Session["OrcaUserID"] = userQuery.OrcaUserID;
                        Session["OrcaUserName"] = userQuery.OrcaUserName;
                        Session["FirstName"] = userQuery.FirstName;
                        Session["LastName"] = userQuery.LastName;
                        Session["UserType"] = userQuery.UserType;

                        // following might be somewhat more proper but it is a pain in the arse to figure out when you don't know the language well enough and what i have below doesn't seem to work when trying to get the info out of it.  i will note that using tempdata has no problem but then you have to keep saving the tempdata when going between controllers because otherwise it is cleared
                        //Session["UserProfile"] = (new UserProfile(userQuery.OrcaUserID)) as UserProfile;

                        TempData["Message"] = "You have successfully logged into your account.";

                        switch (userQuery.UserType)
                        {
                            case OrcaUserType.Consultee:
                                return RedirectToAction("Index", "Consultee");
                            case OrcaUserType.Consultant:
                                return RedirectToAction("Index", "Consultant");
                            case OrcaUserType.ConsultantAdmin:
                                return RedirectToAction("Index", "ConsultantAdmin");
                            default:
                                break;
                        }
                    }
                }
            }

            loginInfo.Password = "";// clear the password field so they know something was entered incorrectly
            ViewBag.Message = "The User Name or Password is incorrect.";

            return View(loginInfo);
        }




        public ActionResult Register()
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            Registration registrationInfo = new Registration();

            return View(registrationInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "OrcaUserName,FirstName,LastName,Email,PhoneNumber,Password,ConfirmPassword")] Registration registrationInfo)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            OrcaContext db = new OrcaContext();

            if (ModelState.IsValid)
            {
                var userExistQuery = from user in db.OrcaUsers
                                     where user.OrcaUserName == registrationInfo.OrcaUserName
                                     select user;

                if (userExistQuery.Any() == false)// make sure the username has not been taken
                {
                    OrcaUser newOrcaUser = new OrcaUser();
                    newOrcaUser.OrcaPassword = new OrcaPassword();


                    // newOrcaUser.OrcaUserID = AUTOGENERATED
                    newOrcaUser.OrcaUserName = registrationInfo.OrcaUserName;
                    newOrcaUser.FirstName = registrationInfo.FirstName;
                    newOrcaUser.LastName = registrationInfo.LastName;
                    newOrcaUser.OrcaPassword.Password = registrationInfo.Password;
                    newOrcaUser.Email = registrationInfo.Email;
                    newOrcaUser.PhoneNumber = registrationInfo.PhoneNumber;
                    newOrcaUser.IsAccountDeactivated = false;
                    newOrcaUser.UserType = OrcaUserType.Consultee;

                    db.OrcaUsers.Add(newOrcaUser);
                    db.SaveChanges();

                    // not the best way to do it, but lets get the ID that was created. i havent tested it but i don't think that the OrcaUserID is coppied from the database to the newOrcaUser.OrcaUserID object.
                    newOrcaUser = (from user in db.OrcaUsers
                                   where user.OrcaUserName == registrationInfo.OrcaUserName
                                   select user).First();

                    Session["OrcaUserID"] = newOrcaUser.OrcaUserID;
                    Session["OrcaUserName"] = newOrcaUser.OrcaUserName;
                    Session["FirstName"] = newOrcaUser.FirstName;
                    Session["LastName"] = newOrcaUser.LastName;
                    Session["UserType"] = newOrcaUser.UserType;

                    TempData["Message"] = "You have successfully created a new account.";

                    return RedirectToAction("Index", "Consultee");
                }
                else// if the username has been taken, instruct to choose different username
                {
                    ViewBag.Message = "This User Name has already been taken. Please choose a different User Name for your account.";
                }
            }
            return View(registrationInfo);
        }
    }
}
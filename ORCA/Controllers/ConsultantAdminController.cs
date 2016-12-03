using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.Admin;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ORCA.Controllers
{
    public class ConsultantAdminController : ConsultantController
    {
        public ActionResult Admin()
        {

            return View();
        }


        public ActionResult PendingExpertRequests()
        {
            OrcaContext db = new OrcaContext();

            //   List<PendingExpertRequest> pendingRequests
            List<ExpertConsultant> pendingRequests = (from pen in db.ExpertConsultants
                                                      where pen.ExpertStatus == ExpertStatus.Requested
                                                      select pen).ToList();

            List<PendingExpertRequest> pendingExpertRequests = new List<PendingExpertRequest>();

            foreach (ExpertConsultant per in pendingRequests)
            {
                PendingExpertRequest userRequesting = new PendingExpertRequest();

                int oid = per.OrcaUserID;

                OrcaUser ou = db.OrcaUsers.Find(oid);

                userRequesting.OrcaUserID = oid;
                userRequesting.OrcaUserName = ou.OrcaUserName;
                userRequesting.FirstName = ou.FirstName;
                userRequesting.LastName = ou.LastName;
                userRequesting.Email = ou.Email;
                userRequesting.PhoneNumber = ou.PhoneNumber;

                pendingExpertRequests.Add(userRequesting);
            }

            return View(pendingExpertRequests);
        }


        public ActionResult PendingExpertRequestDetails(int? OrcaUserID)
        {
            OrcaContext db = new OrcaContext();

            PendingExpertRequest pendingRequester = new PendingExpertRequest();

            int oid = (int)OrcaUserID;
            OrcaUser ou = db.OrcaUsers.Find(oid);
            ExpertConsultant exp = db.ExpertConsultants.Find(oid);
            ou = db.OrcaUsers.Find(oid);

            pendingRequester.OrcaUserID = ou.OrcaUserID;
            pendingRequester.ExpertStatus = exp.ExpertStatus;
            pendingRequester.OrcaUserName = ou.OrcaUserName;
            pendingRequester.FirstName = ou.FirstName;
            pendingRequester.LastName = ou.LastName;
            pendingRequester.Email = ou.Email;
            pendingRequester.PhoneNumber = ou.PhoneNumber;

            return View(pendingRequester);
        }

        //"OrcaUserID,ExpertStatus,OrcaUserName,FirstName,LastName,Email,PhoneNumber"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PendingExpertRequestDetails([Bind(Exclude = "OrcaUserName,FirstName,LastName,Email,PhoneNumber", Include = "OrcaUserID, ExpertStatus")]PendingExpertRequest pendingExpertRequest)
        {
            int oid = pendingExpertRequest.OrcaUserID;

            if (ModelState.IsValid)
            {

                OrcaContext db = new OrcaContext();

                ExpertConsultant expToUpdate = db.ExpertConsultants.Find(pendingExpertRequest.OrcaUserID);

                expToUpdate.ExpertStatus = pendingExpertRequest.ExpertStatus;

                db.Entry(expToUpdate).State = EntityState.Modified;
                db.SaveChanges();

                
                //OrcaUserType.Consultant
                if (pendingExpertRequest.ExpertStatus == ExpertStatus.Approved)
                {
                    OrcaUser requestingUser = db.OrcaUsers.Find(pendingExpertRequest.OrcaUserID);

                    requestingUser.UserType = OrcaUserType.Consultant;

                    db.Entry(requestingUser).State = EntityState.Modified;
                    db.SaveChanges();
                }

                
                return RedirectToAction("PendingExpertRequests");
            }


            return View(pendingExpertRequest);
        }



        
        public ActionResult CurrentExperts()
        {
            OrcaContext db = new OrcaContext();

            List<ChangeExpert> experts = new List<ChangeExpert>();

            
            List<OrcaUser> expertUsers = db.OrcaUsers.Where(x => x.UserType == OrcaUserType.Consultant).ToList();

            foreach (OrcaUser exp in expertUsers)
            {
                int oid = exp.OrcaUserID;
                ExpertConsultant expCon = db.ExpertConsultants.Find(oid);
                
                ChangeExpert expToChange = new ChangeExpert();
                
                expToChange.OrcaUserID = oid;
                expToChange.OrcaUserName = exp.OrcaUserName;
                

                expToChange.IsActive = expCon.IsActive;

                expToChange.IsAccountDeactivated = exp.IsAccountDeactivated;


                expToChange.FirstName = exp.FirstName;
                expToChange.LastName = exp.LastName;
                expToChange.Email = exp.Email;
                expToChange.PhoneNumber = exp.PhoneNumber;

                experts.Add(expToChange);
            }
            
            return View(experts);
        }

        
        public ActionResult ChangeExpert(int OrcaUserID)
        {
            OrcaContext db = new OrcaContext();

            OrcaUser exp = db.OrcaUsers.Find(OrcaUserID);

            ChangeExpert expToChange = new ChangeExpert();
            int oid = (int)OrcaUserID;

            expToChange.OrcaUserID = oid;
            expToChange.OrcaUserName = exp.OrcaUserName;

            expToChange.UserType = exp.UserType;

            string boolStrng = db.ExpertConsultants.Find(oid).IsActive.ToString().ToLower().Trim();



            if (boolStrng == "true")
                expToChange.IsActive = true;
            else expToChange.IsActive = false;

            boolStrng = exp.IsAccountDeactivated.ToString().ToLower().Trim();



            if (boolStrng == "true")
                expToChange.IsAccountDeactivated = true;
            else expToChange.IsAccountDeactivated = false;



            expToChange.FirstName = exp.FirstName;
            expToChange.LastName = exp.LastName;
            expToChange.Email = exp.Email;
            expToChange.PhoneNumber = exp.PhoneNumber;


            return View(expToChange);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeExpert([Bind(Exclude = "OrcaUserName,FirstName,LastName,Email,PhoneNumber", Include = "OrcaUserID,UserType,IsActive,IsAccountDeactivated")] ChangeExpert expToChange)
        {
            if (ModelState.IsValid)
            {
                OrcaContext db = new OrcaContext();
                int oid = expToChange.OrcaUserID;

                OrcaUser changeUser = db.OrcaUsers.Find(oid);

                changeUser.UserType = expToChange.UserType;
                changeUser.IsAccountDeactivated = expToChange.IsAccountDeactivated;

                db.Entry(changeUser).State = EntityState.Modified;
                db.SaveChanges();

                ExpertConsultant expCon = db.ExpertConsultants.Find(oid);

                expCon.IsActive = expToChange.IsActive;

                db.Entry(expCon).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("CurrentExperts");

            }

            return View();
        }


            

    }
}
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ORCA.DAL
{
    public class OrcaInitializer : DropCreateDatabaseIfModelChanges<OrcaContext>
    {
        protected override void Seed(OrcaContext context)
        {
            base.Seed(context);

            OrcaUser generalUser = new OrcaUser();

            generalUser.OrcaUserName = "TestUser";
            generalUser.FirstName = "Test";
            generalUser.LastName = "User";
            generalUser.Email = "testuser@marshall.edu";
            generalUser.PhoneNumber = "3045432101";
            generalUser.IsAccountDeactivated = false;
            generalUser.UserType = OrcaUserType.Consultee;

            context.OrcaUsers.Add(generalUser);

            OrcaUser expertUser = new OrcaUser();

            expertUser.OrcaUserName = "TestExpert";
            expertUser.FirstName = "Test";
            expertUser.LastName = "Expert";
            expertUser.Email = "testexpert@marahsll.edu";
            expertUser.PhoneNumber = "1234567890";
            expertUser.IsAccountDeactivated = false;
            expertUser.UserType = OrcaUserType.Consultant;

            ExpertConsultant expertUserConsultant = new ExpertConsultant();

            expertUserConsultant.ExpertConsultantID = expertUser.OrcaUserID;
            expertUserConsultant.IsActive = true;
            expertUserConsultant.IsApproved = true;
            expertUserConsultant.IsAdmin = true;
        }
    }
}
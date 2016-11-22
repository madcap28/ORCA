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
            //base.Seed(context);


            // add a regular user for testing initialization
            OrcaUser generalUser = new OrcaUser();
            generalUser.OrcaPassword = new OrcaPassword();

            generalUser.OrcaUserName = "TestUser";
            generalUser.FirstName = "Test";
            generalUser.LastName = "User";
            generalUser.OrcaPassword.Password = "password";
            generalUser.Email = "testuser@marshall.edu";
            generalUser.PhoneNumber = "3045432101";
            generalUser.IsAccountDeactivated = false;
            generalUser.UserType = OrcaUserType.Consultee;

            context.OrcaUsers.Add(generalUser);
            context.SaveChanges();



            // add a regular expert consultant for testing initialization
            OrcaUser expertUser = new OrcaUser();
            expertUser.OrcaPassword = new OrcaPassword();

            expertUser.OrcaUserName = "TestExpert";
            expertUser.FirstName = "Test";
            expertUser.LastName = "Expert";
            expertUser.OrcaPassword.Password = "password";
            expertUser.Email = "testexpert@marahsll.edu";
            expertUser.PhoneNumber = "1234567890";
            expertUser.IsAccountDeactivated = false;
            expertUser.UserType = OrcaUserType.Consultant;

            context.OrcaUsers.Add(expertUser);
            context.SaveChanges();

            ExpertConsultant expertUserConsultant = new ExpertConsultant();

            expertUserConsultant.OrcaUserID = expertUser.OrcaUserID;
            expertUserConsultant.ExpertStatus = ExpertStatus.Approved;
            expertUserConsultant.IsActive = true;

            context.ExpertConsultants.Add(expertUserConsultant);
            context.SaveChanges();

            ConsultantExpertise consultantExpertise = new ConsultantExpertise();

            consultantExpertise.OrcaUserID = expertUserConsultant.OrcaUserID;
            consultantExpertise.FieldOfExpertise = "Computer Science";

            context.ConsultantExpertises.Add(consultantExpertise);
            context.SaveChanges();



            // add an admin expert consultant for testing initialization
            expertUser = new OrcaUser();
            expertUser.OrcaPassword = new OrcaPassword();

            expertUser.OrcaUserName = "AdminExpert";
            expertUser.FirstName = "Admin";
            expertUser.LastName = "Expert";
            expertUser.OrcaPassword.Password = "password";
            expertUser.Email = "adminexpert@marahsll.edu";
            expertUser.PhoneNumber = "1234567890";
            expertUser.IsAccountDeactivated = false;
            expertUser.UserType = OrcaUserType.ConsultantAdmin;

            context.OrcaUsers.Add(expertUser);
            context.SaveChanges();

            expertUserConsultant = new ExpertConsultant();

            expertUserConsultant.OrcaUserID = expertUser.OrcaUserID;
            expertUserConsultant.ExpertStatus = ExpertStatus.Approved;
            expertUserConsultant.IsActive = true;

            context.ExpertConsultants.Add(expertUserConsultant);
            context.SaveChanges();

            consultantExpertise = new ConsultantExpertise();

            consultantExpertise.OrcaUserID = expertUserConsultant.OrcaUserID;
            consultantExpertise.FieldOfExpertise = "Software Engineering";

            context.ConsultantExpertises.Add(consultantExpertise);
            context.SaveChanges();

        }
    }
}
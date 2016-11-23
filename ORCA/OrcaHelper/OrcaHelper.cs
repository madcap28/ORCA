using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ORCA
{
    public enum PasswordChangeStatus { SUCCESS, INVALID_PASSWORD, INVALID_USER }

    
    public class OrcaHelper
    {
        
        public static UserProfile ChangeUserProfileInfo(UserProfile profileChanges)
        {
            OrcaContext db = new OrcaContext();

            OrcaUser orcaUser = db.OrcaUsers.Find(profileChanges.OrcaUserID);

            if (orcaUser != null)
            {
                // update any allowed changes that may have been made
                orcaUser.FirstName = profileChanges.FirstName;
                orcaUser.LastName = profileChanges.LastName;
                orcaUser.Email = profileChanges.Email;
                orcaUser.PhoneNumber = profileChanges.PhoneNumber;

                // update the database
                db.Entry(orcaUser).State = EntityState.Modified;
                db.SaveChanges();

                if (orcaUser.UserType == OrcaUserType.Consultant || orcaUser.UserType == OrcaUserType.ConsultantAdmin)
                {
                    ExpertConsultant expertConsultant = db.ExpertConsultants.Find(orcaUser.OrcaUserID);

                    if (expertConsultant != null)
                    {
                        // update any allowed changes that may have been made
                        expertConsultant.TitleDegree = profileChanges.TitleDegree;
                        expertConsultant.KeyWordList = profileChanges.KeyWordList;

                        // IsActive is done this way to allow a drop down list for the profile view
                        if (profileChanges.IsActive == ActiveStatus.Yes)
                        {
                            expertConsultant.IsActive = true;
                        }
                        else
                        {
                            expertConsultant.IsActive = false;
                        }
                        
                        // update the database
                        db.Entry(expertConsultant).State = EntityState.Modified;
                        db.SaveChanges();


                        // this isn't pretty, but i was doing it this way while trying to figure out where a problem was occuring.  since its done, i'll leave it alone for now, after all, it is a prototype
                        // add the new field of expertise if it is entered
                        if (!(String.IsNullOrEmpty(profileChanges.FieldToAdd) || String.IsNullOrWhiteSpace(profileChanges.FieldToAdd)))// make sure something was entered in FieldToAdd
                        {
                            // get a list of current expertises
                            List<ConsultantExpertise> userExpertises = (from expertise in db.ConsultantExpertises
                                                                        where expertise.OrcaUserID == profileChanges.OrcaUserID
                                                                        select expertise).ToList();

                            if (userExpertises.Count() <= 0 || userExpertises.All(ex => ex.FieldOfExpertise != profileChanges.FieldToAdd))// make sure that we aren't duplicating any expertises
                            {
                                ConsultantExpertise newField = new ConsultantExpertise();
                                newField.OrcaUserID = profileChanges.OrcaUserID;
                                newField.FieldOfExpertise = profileChanges.FieldToAdd;

                                db.ConsultantExpertises.Add(newField);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }

            return profileChanges;
        }

        
        public static PasswordChangeStatus ChangePassword(int OrcaUserID, PasswordChange passwordChange)
        {
            OrcaContext db = new OrcaContext();
            OrcaPassword userPasswordQuery = null;

            // get the password from Passwords db table
            try
            {
                //OrcaPassword userPasswordQuery = (from user in db.OrcaPasswords
                //                                  where user.OrcaUserID == OrcaUserID
                //                                  select user).First();
                userPasswordQuery = db.OrcaPasswords.AsQueryable().First(user => user.OrcaUserID == OrcaUserID);
            }
            catch (Exception)// if the OrcaUserID could not be found, return INVALID_USER
            {
                return PasswordChangeStatus.INVALID_USER;
            }

            // make sure the proper original password was entered
            if (userPasswordQuery.Password == passwordChange.CurrentPassword)
            {
                // change the password
                userPasswordQuery.Password = passwordChange.Password;

                // update the database
                db.Entry(userPasswordQuery).State = EntityState.Modified;
                db.SaveChanges();

                return PasswordChangeStatus.SUCCESS;
            }

            return PasswordChangeStatus.INVALID_PASSWORD;
        }
    }
}
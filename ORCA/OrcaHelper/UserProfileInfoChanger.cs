using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ORCA.OrcaHelper
{
    public class UserProfileInfoChanger
    {
        public static ConsultantProfile ChangeConsultantProfileInfo(ConsultantProfile profileChanges)
        {
            OrcaContext db = new OrcaContext();

            // get user from database and change settings
            //if (ChangeUserProfileInfo(profileChanges.UserProfile) != null)
            //{
                try
                {
                    // definately not the way i want to do this, but i don't have time to figure out how to do it the way it should be done
                    // get the user from the database
                    OrcaUser userQuery = (from user in db.OrcaUsers
                                          where user.OrcaUserID == profileChanges.OrcaUserID
                                          select user).First();

                    // update any allowed changes that may have been made
                    userQuery.FirstName = profileChanges.FirstName;
                    userQuery.LastName = profileChanges.LastName;
                    userQuery.Email = profileChanges.Email;
                    userQuery.PhoneNumber = profileChanges.PhoneNumber;

                    // update the database
                    db.Entry(userQuery).State = EntityState.Modified;
                    db.SaveChanges();




                    // get expert from database
                    ExpertConsultant expertQuery = (from expert in db.ExpertConsultants
                                                    where expert.OrcaUserID == profileChanges.OrcaUserID
                                                    select expert).First();

                    // update any allowed changes that may have been made
                    expertQuery.TitleDegree = profileChanges.TitleDegree;
                    expertQuery.KeyWordList = profileChanges.KeyWordList;
                    if (profileChanges.IsActive == ActiveStatus.Yes)
                    {
                        expertQuery.IsActive = true;
                    }
                    else
                    {
                        expertQuery.IsActive = false;
                    }

// NOTE: MUST STILL IMPLEMENT THE ConsultantExpertises FOR THE ExpertConsultant

                    // update the database
                    db.Entry(expertQuery).State = EntityState.Modified;
                    db.SaveChanges();

                    return profileChanges;
                }
                catch (Exception)
                {
                }
            //}
            return null;
        }





        public static UserProfile ChangeUserProfileInfo(UserProfile profileChanges)
        {
            OrcaContext db = new OrcaContext();

            try
            {
                // get the user from the database
                OrcaUser userQuery = (from user in db.OrcaUsers
                                      where user.OrcaUserID == profileChanges.OrcaUserID
                                      select user).First();

                // update any allowed changes that may have been made
                userQuery.FirstName = profileChanges.FirstName;
                userQuery.LastName = profileChanges.LastName;
                userQuery.Email = profileChanges.Email;
                userQuery.PhoneNumber = profileChanges.PhoneNumber;

                // update the database
                db.Entry(userQuery).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }
            return profileChanges;
        }
    }
}
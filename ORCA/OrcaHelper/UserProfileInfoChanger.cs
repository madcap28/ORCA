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
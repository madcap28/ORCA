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
    public enum PasswordChangeStatus { SUCCESS, INVALID_PASSWORD, INVALID_USER }

    public class PasswordChanger
    {
        public static PasswordChangeStatus ChangePassword(int OrcaUserID, ChangePasswordModel passwordChange)
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
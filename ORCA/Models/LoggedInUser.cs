using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public class LoggedInUser
    {
        int OrcaUserID { get; set; }
        string OrcaUserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        OrcaUserType UserType { get; set; }

        public LoggedInUser()
        {

        }
        public LoggedInUser(int OrcaUserID, string OrcaUserName, string FirstName, string LastName, OrcaUserType UserType)
        {
            this.OrcaUserID = OrcaUserID;
            this.OrcaUserName = OrcaUserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.UserType = UserType;
        }
    }
}
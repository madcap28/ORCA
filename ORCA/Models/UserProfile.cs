using ORCA.DAL;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    [BindableType(IsBindable = true)]
    public class UserProfile
    {
        
        public int OrcaUserID { get; set; }

        [Display(Name = "User Name")]
        public string OrcaUserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [EmailAddress, /*DataType(DataType.EmailAddress),*/ Display(Name = "Email Address")]
        public string Email { get; set; }

        // NOTE:
        // NOTE: Must take care of proper phone number formatting later
        // NOTE:
        [Phone, /*DataType(DataType.PhoneNumber),*/ Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        

        
        public UserProfile() { }
        public UserProfile(int OrcaUserID)
        {
            OrcaContext db = new OrcaContext();

            OrcaUser userInfo = db.OrcaUsers.Find(OrcaUserID);

            if (userInfo != null)
            {
                // fill in the info for the user
                this.OrcaUserID = OrcaUserID;
                this.OrcaUserName = userInfo.OrcaUserName;
                this.FirstName = userInfo.FirstName;
                this.LastName = userInfo.LastName;
                this.Email = userInfo.Email;
                this.PhoneNumber = userInfo.PhoneNumber;
            }

            //try
            //{
            //    OrcaUser userInfo = (from user in db.OrcaUsers
            //                         where user.OrcaUserID == OrcaUserID
            //                         select user).First();
            //    //db.OrcaUsers.AsQueryable().First(user => user.OrcaUserID == OrcaUserID);

            //    // fill in the info for the user
            //    this.OrcaUserID = OrcaUserID;
            //    this.OrcaUserName = userInfo.OrcaUserName;
            //    this.FirstName = userInfo.FirstName;
            //    this.LastName = userInfo.LastName;
            //    this.Email = userInfo.Email;
            //    this.PhoneNumber = userInfo.PhoneNumber;
            //}
            //catch (Exception)
            //{
            //    //this.OrcaUserID = -1;
            //    //this.OrcaUserName = null;
            //    //this.FirstName = null;
            //    //this.LastName = null;
            //    //this.Email = null;
            //    //this.PhoneNumber = null;
            //}
        }
        
    }
}
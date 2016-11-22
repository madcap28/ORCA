using ORCA.DAL;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public enum ActiveStatus { Yes, No }

    [BindableType(IsBindable = true)]
    public class ConsultantProfile
    {
        [Required]
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




        [Display(Name = "Actively Consulting?"), Required]
        public ActiveStatus IsActive { get; set; }

        [Display(Name = "Professional Degree")]
        public string TitleDegree { get; set; }
        
        [Display(Name = "Fields of Expertise")]
        public ICollection<ConsultantExpertise> FieldsOfExpertise { get; set; }
        //public List<ConsultantExpertise> FieldsOfExpertise { get; set; }
        //public IEnumerable<ConsultantExpertise> FildsOfExpertise { get; set; }

        [Display(Name = "Key Word List")]
        public string KeyWordList { get; set; }




        public ConsultantProfile() { }
        public ConsultantProfile(int OrcaUserID)
        {
            OrcaContext db = new OrcaContext();

            OrcaUser userInfo = db.OrcaUsers.Find(OrcaUserID);
            ExpertConsultant consultantInfo = db.ExpertConsultants.Find(OrcaUserID);

            if (consultantInfo != null && userInfo != null)
            {
                // THIS GETS NULLIFIED this.UserProfile = new UserProfile(OrcaUserID);
                // NOTE: I think i may have figured out why, but now i won't be fixing it as its a little late
                // not the way i want to do this but i don't have time to figure out how to do it correctly, there are issues when passing models between views and controlers that cause some variables to be retained while other variables are nullified

                // fill in the info for the user
                this.OrcaUserID = OrcaUserID;
                this.OrcaUserName = userInfo.OrcaUserName;
                this.FirstName = userInfo.FirstName;
                this.LastName = userInfo.LastName;
                this.Email = userInfo.Email;
                this.PhoneNumber = userInfo.PhoneNumber;

                
                // using a dropdown but the db value is a bool, so set appropriately
                if (consultantInfo.IsActive)
                {
                    this.IsActive = ActiveStatus.Yes;
                }
                else
                {
                    this.IsActive = ActiveStatus.No;
                }

                this.TitleDegree = consultantInfo.TitleDegree;

                // this.FieldsOfExpertise = consultantInfo.ConsultantExpertises;

                this.FieldsOfExpertise = (from expertise in db.ConsultantExpertises
                                          where expertise.OrcaUserID == OrcaUserID
                                          select expertise).ToList();

                this.KeyWordList = consultantInfo.KeyWordList;
            }
        }
    }
    
}
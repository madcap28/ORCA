using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORCA.Models.OrcaDB
{
    public enum ExpertStatus { Requested, Declined, Approved }

    public class ExpertConsultant
    {
        [Key, ForeignKey("OrcaUser")/*, Required*/]
        public int OrcaUserID { get; set; }// This is the ExpertConsultantID of the OrcaUser that is an ExpertConsultant.   ExpertConsultantID = OrcaUser.OrcaUserID
        
        //[Required]
        public ExpertStatus ExpertStatus { get; set; }// This indicates if the OrcaUser ExpertConsultant has been approved to provide expert advice.  This is set by an admin that confirms and approves the account.

        //[Required]
        public bool IsActive { get; set; }// This indicates if the ExpertConsultant is currently active and will be available for response.  The OrcaUser ExpertConsultant can change this, i.e. setting this to False (inactive) when he/she goes on vacation.

        public string TitleDegree { get; set; }
        public string KeyWordList { get; set; }



        [ForeignKey("OrcaUserID")]
        public virtual OrcaUser OrcaUser { get; set; }// This is the OrcaUser that is an ExpertConsultant

        public virtual ICollection<ConsultantExpertise> ConsultantExpertises { get; set; }// This is a list of ConsultantExpertises that this OrcaUser ExpertConsultant has claimed as fields of expertise
    }
}
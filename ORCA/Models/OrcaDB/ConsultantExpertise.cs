using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORCA.Models.OrcaDB
{
    public class ConsultantExpertise
    {
        [Key]
        public int ConsultantExpertiseID { get; set; }

        [Required]// [ForeignKey("ExpertConsultant")] [ForeignKey("OrcaUser")]
        public int OrcaUserID { get; set; }// This is the OrcaUser ExpertConsultant that has this ConsultantExpertise in his/her list of ConsultantExpertises.   ExpertConsultantID = ExpertConsultant.ExpertConsultantID = OrcaUser.OrcaUserID

        //[Required]
        public string FieldOfExpertise { get; set; }// This is FieldOfExpertise of an OrcaUser that is an ExpertConsultant





        // NOTE: ExpertConsultantID = ExpertConsultant.ExpertConsultantID = OrcaUser.OrcaUserID
        [ForeignKey("OrcaUserID")]
        public virtual ExpertConsultant ExpertConsultant { get; set; }// This points to the ExpertConsultant class that contains the info about users that are experts

        [ForeignKey("OrcaUserID")]
        public virtual OrcaUser OrcaUser { get; set; }// This points to the OrcaUser class that is an ExpertConsultant
    }
}
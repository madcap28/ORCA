using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORCA.Models.OrcaDB
{
    public class ConsultantStatusRequest
    {
        [Key, ForeignKey("OrcaUser")]
        public int OrcaUserID { get; set; }

        public bool RequestingStatus { get; set; }

        [ForeignKey("OrcaUserID")]
        public virtual OrcaUser OrcaUser { get; set; }
    }
}
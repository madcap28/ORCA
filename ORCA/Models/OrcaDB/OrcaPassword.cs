using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ORCA.Models.OrcaDB
{
    public class OrcaPassword
    {
        [Key, ForeignKey("OrcaUser"), Required]
        public int OrcaUserID { get; set; }
        
        [Required,
            Display(Name = "Password"),
            DataType(DataType.Password),
            MinLength(8, ErrorMessage = "Please ensure that your password is at least 8 characters.")]
        public string Password { get; set; }// NEED TO HASH AND SECURE THIS



        [ForeignKey("OrcaUserID")]
        public virtual OrcaUser OrcaUser { get; set; }
    }
}
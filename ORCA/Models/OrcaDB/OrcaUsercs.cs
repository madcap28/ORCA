﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models.OrcaDB
{
    public enum OrcaUserType { Consultee, Consultant }

    public class OrcaUser
    {
        [Key, Required]
        public int OrcaUserID { get; set; }

        [Required]
        public string Password { get; set; }// NEED TO HASH AND SECURE THIS

        [Required]
        public string OrcaUserName { get; set; }// This is the OrcaUserName that the account creator chooses.  This must be unique but we are not using it as the OrcaUserID in order to avoid problems with special characters.

        public string FirstName { get; set; }// Optional field but will be required for OrcaUser ExpertConsultant in order to get approved as an ExpertConsultant.
        public string LastName { get; set; }// Optional field but will be required for OrcaUser ExpertConsultant in order to get approved as an ExpertConsultant.

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }// Optional field but will be required for OrcaUser ExpertConsultant in order to get approved as an ExpertConsultant.  May be required for password reset if the user forgets his/her password.

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }// Optional field

        [Required]
        public bool IsAccountDeactivated { get; set; }// This is initially False when the account is created but an admin can disable this account by setting this to True.

        public OrcaUserType UserType { get; set; }// This indicates the UserType as either a Consultee seeking expert advice or a Consultant providing expert advice.
    }
}
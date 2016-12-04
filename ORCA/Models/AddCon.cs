using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static ORCA.OrcaHelper;

namespace ORCA.Models
{
    public class AddCon
    {
        public int TicketID { get; set; }
        public string SortOrder { get; set; }
        
        [Display(Name = "")]
        public string SearchString { get; set; }
        List<ActiveExpert> Experts { get; set; }



        //public SortExpert StringToSortExpert(string sortExpert)
        //{
        //    switch (sortExpert)
        //    {
        //        case "OrcaUserName":
        //            return SortExpert.OrcaUserName;
        //        case "TitleDegree":
        //            return SortExpert.TitleDegree;
        //        case "FirstName":
        //            return SortExpert.FirstName;
        //        case "LastName":
        //            return SortExpert.LastName;
        //        case "FieldOfExpertise":
        //            return SortExpert.FieldOfExpertise;
        //        case "OrcaUserName_desc":
        //            return SortExpert.OrcaUserName_desc;
        //        case "TitleDegree_desc":
        //            return SortExpert.TitleDegree_desc;
        //        case "FirstName_desc":
        //            return SortExpert.FirstName_desc;
        //        case "LastName_desc":
        //            return SortExpert.LastName_desc;
        //        case "FieldOfExpertise_desc":
        //            return SortExpert.FieldOfExpertise_desc;
        //        default:
        //            return SortExpert.OrcaUserName;
        //    }
        //}

        //public SortExpert OppositeOrder(SortExpert sortExpert)
        //{
        //    SortExpert newSort;

        //    if (sortExpert < SortExpert.OrcaUserName_desc) newSort = (sortExpert + 5);
        //    else newSort = (sortExpert - 5);

        //    return newSort;
            
        //}
    }

    //public enum SortExpert { OrcaUserName, TitleDegree, FirstName, LastName, FieldOfExpertise, OrcaUserName_desc, TitleDegree_desc, FirstName_desc, LastName_desc, FieldOfExpertise_desc }



}
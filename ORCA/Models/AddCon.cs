using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ORCA.Models
{
    public class AddCon
    {
        public int TicketID { get; set; }
        public SortBy SortOrder { get; set; }
        public string SearchString { get; set; }
        List<ActiveExpert> Experts { get; set; }
        
    }
}
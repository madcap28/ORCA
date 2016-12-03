using ORCA.DAL;
using ORCA.Models;
using ORCA.Models.OrcaDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ORCA.Controllers
{
    public class ConsultantAdminController : ConsultantController
    {
        public ActionResult PendingExpertRequests()
        {

            return View();
        }
        
    }
}
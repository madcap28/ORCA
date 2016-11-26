using ORCA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ORCA.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult Index([Bind]string sortOrder, [Bind]string searchString)
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            if (String.IsNullOrEmpty(sortOrder))
                if (TempData["SortOrder"] != null)
                    sortOrder = TempData["SortOrder"].ToString();
            if (String.IsNullOrEmpty(searchString))
                if (TempData["SearchString"] != null)
                    searchString = TempData["SearchString"].ToString();




            if (String.IsNullOrEmpty(sortOrder)) sortOrder = SortBy.FieldOfExpertise.ToString();

            ViewBag.FieldOfExpertiseSortParam = sortOrder == SortBy.FieldOfExpertise.ToString() ? "FieldOfExpertise_desc" : SortBy.FieldOfExpertise.ToString();
            ViewBag.TitleDegreeSortParam = sortOrder == SortBy.TitleDegree.ToString() ? "TitleDegree_desc" : SortBy.TitleDegree.ToString();
            ViewBag.OrcaUserNameSortParam = sortOrder == SortBy.OrcaUserName.ToString() ? "OrcaUserName_desc" : SortBy.OrcaUserName.ToString();
            ViewBag.FirstNameSortParam = sortOrder == SortBy.FirstName.ToString() ? "FirstName_desc" : SortBy.FirstName.ToString();
            ViewBag.LastNameSortParam = sortOrder == SortBy.LastName.ToString() ? "LastName_desc" : SortBy.LastName.ToString();

            ActiveExperts activeExperts = new ActiveExperts();

            switch (sortOrder)
            {
                case "OrcaUserName":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.OrcaUserName, SortMethod.Ascending);
                    break;
                case "TitleDegree":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.TitleDegree, SortMethod.Ascending);
                    break;
                case "FirstName":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.FirstName, SortMethod.Ascending);
                    break;
                case "LastName":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.LastName, SortMethod.Ascending);
                    break;
                case "FieldOfExpertise_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.FieldOfExpertise, SortMethod.Descending);
                    break;
                case "OrcaUserName_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.OrcaUserName, SortMethod.Descending);
                    break;
                case "TitleDegree_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.TitleDegree, SortMethod.Descending);
                    break;
                case "FirstName_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.FirstName, SortMethod.Descending);
                    break;
                case "LastName_desc":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.LastName, SortMethod.Descending);
                    break;
                default: // case "FieldOfExpertise":
                    activeExperts.FilterList(searchString).SortListBy(SortBy.FieldOfExpertise, SortMethod.Ascending);
                    break;
            }

            ViewBag.SortOrder = sortOrder;
            //ViewBag.SearchString = searchString;

            return View(activeExperts);
        }




        public ActionResult About()
        {
            // convnention for making it easier to pass messages between controllers
            if (TempData["Message"] != null)
            {
                ViewBag.Message += (" " + TempData["Message"].ToString());
            }

            return View();
        }




    }
}
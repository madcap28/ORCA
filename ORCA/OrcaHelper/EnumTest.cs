using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORCA
{
    public enum TestPublicEnum
    {
        Value1, Value2, Value3, Value4, Value5,
        descValue1, descValue2, descValue3, descValue4, descValue5
    }

    public class EnumTest
    {
        private TestPublicEnum _EnumTest_TestPublicEnum;
        public TestPublicEnum EnumTest_TestPublicEnum
        {
            get
            {
                return _EnumTest_TestPublicEnum;
            }
            set
            {
                if (value == _EnumTest_TestPublicEnum)
                {
                    if (_EnumTest_TestPublicEnum <= TestPublicEnum.Value5)
                        _EnumTest_TestPublicEnum += 5;
                    else _EnumTest_TestPublicEnum -= TestPublicEnum.Value5 + 1;
                }
                else
                    _EnumTest_TestPublicEnum = value;
            }
        }
    }

}









//    Session["OrcaUserID"] = newOrcaUser.OrcaUserID;
//    Session["OrcaUserName"] = newOrcaUser.OrcaUserName;
//    Session["FirstName"] = newOrcaUser.FirstName;
//    Session["LastName"] = newOrcaUser.LastName;
//    Session["UserType"] = newOrcaUser.UserType;





//  db.Entry(orcaUser).State = EntityState.Modified;
//  db.SaveChanges();




//  db.OrcaUsers.Add(newOrcaUser);
//  db.SaveChanges();








/*

        <div class="form-group">
            @Html.LabelFor(model => model.Password, htmlAttributes: new { @class = "control-label col-md-2" })
            <div class="col-md-10">
                @Html.EditorFor(model => model.Password, new { htmlAttributes = new { @class = "form-control" } })
                @Html.ValidationMessageFor(model => model.Password, "", new { @class = "text-danger" })
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <input type="submit" value="Login" class="btn btn-default" />
            </div>
        </div>

*/



/*
@using(Html.BeginForm())

{
    <p>
        Search Expert Consultants:  @Html.TextBox("searchString")
        <input type = "submit" value="Search" onclick=@{ TempData["SearchString"] = null; } />
    </p>
}
*/


/*


@using (Html.BeginForm())
        {
            @Html.ActionLink("CLICK THIS", "Index");
            
            <input type="hidden" onclick=@{ Model.FilterTicketsOption = Consultation.TicketFilterOption.Open; TempData["ConsultationModel"] = Model; TempData["Message"] = "HOLLY FUCKER ME"; } />
        }


*/


/*
    /// <summary>
    /// This just accidently popped up, but i think it is somthing i would like
    /// </summary>
    /// <returns></returns>
*/

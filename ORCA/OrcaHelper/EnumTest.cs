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













    //  db.Entry(orcaUser).State = EntityState.Modified;
    //  db.SaveChanges();

    //  db.OrcaUsers.Add(newOrcaUser);
    //  db.SaveChanges();
    
    /*
    @using(Html.BeginForm())


    {
        <p>
            Search Expert Consultants:  @Html.TextBox("searchString")
            <input type = "submit" value="Search" onclick=@{ TempData["SearchString"] = null; } />
        </p>
    }

    */

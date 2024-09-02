using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public interface IAspxDropdownEditWindowTemplateBase : ITemplate
    {
        Control Control { get; set; }
        string DisplayName { get; set; }
        string ModuleName { get; set; }
        bool IsMulti { get; set; }
        string GetValues();
        void SetValues(string values);
        string GetText();

        string Value { get; set; }
        object CustomParameters { get; set; }
        string JsCallBackEvent { get; set; }
        ASPxDropDownEdit DropDownEdit { get; set; }
       
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
namespace uGovernIT.Web
{
    public class RequestTypeDropdownTemplate : IAspxDropdownEditWindowTemplateBase
    {
        RequestTypeCustom ctrl = null;

        public Control Control { get; set; }
        public string DisplayName { get; set; }
        public bool IsMulti { get; set; }

        public string ModuleName { get; set; }
        public object CustomParameters { get; set; }
        public string Value { get; set; }
        public ASPxDropDownEdit DropDownEdit { get; set; }
        public string JsCallBackEvent { get; set; }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            ctrl = (RequestTypeCustom)container.Page.LoadControl("~/ControlTemplates/Shared/RequestTypeCustom.ascx");
            ctrl.ID = "RequestTypeCustom";
            ctrl.DisplayName = DisplayName;
            ctrl.ModuleName = ModuleName;
            ctrl.SetValueCheck = Value;
            ctrl.DropDownEdit = DropDownEdit;
            ctrl.CustomProperties = (RequestTypeCustomProperties)CustomParameters;
            container.Controls.Add(ctrl);
        }
        public string GetValues()
        {
            return ctrl.GetValues();
        }
        public void SetValues(string value)
        {
            if(ctrl != null)
                ctrl.SetValues(value);
            Value = value;
            DropDownEdit = DropDownEdit;
        }

        public string GetText()
        {
            return string.Empty;
        }
    }
}
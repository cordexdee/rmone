using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web;
namespace uGovernIT.Web
{
    public class DepartmentDropDownTemplate:IAspxDropdownEditWindowTemplateBase
    {
        DepartmentDropdownList ctrl = null;
        public Control Control { get; set; }
        public string DisplayName { get; set; }
        public string ModuleName { get; set; }
        public bool IsMulti { get; set; }
        public object CustomParameters { get ; set ; }
        public string Value { get; set; }

        public ASPxDropDownEdit DropDownEdit { get; set; }
        public string JsCallBackEvent { get; set; }

        public void InstantiateIn(System.Web.UI.Control container)
        {
            ctrl = (DepartmentDropdownList)container.Page.LoadControl("~/ControlTemplates/uGovernIT/DepartmentDropdownList.ascx");
            //ctrl.DisplayName = DisplayName;
            ctrl.IsMulti = IsMulti;
            ctrl.ID = "department";
            ctrl.SetValueCheck = Value;
            ctrl.DropDownEdit = DropDownEdit;
            ctrl.CallBackJSEvent = JsCallBackEvent;
            container.Controls.Add(ctrl);
        }
        public string GetValues()
        {
            return ctrl.GetValues();
        }
        public void SetValues(string value)
        {
            if (ctrl != null)
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
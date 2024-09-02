using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web;

namespace uGovernIT.Web
{ 
    public class AssetLookupDropDownTemplate: IAspxDropdownEditWindowTemplateBase
    {
        AssetLookupDropDownList ctrl = null;
        public Control Control { get; set; }
        public string DisplayName { get; set; }
        public string ModuleName { get; set; }
        public bool IsMulti { get; set; }
        public object CustomParameters { get; set; }
        public string Value { get; set; }
        public ASPxDropDownEdit DropDownEdit { get; set; }
        public string JsCallBackEvent { get; set; }

        public AssetLookupDropDownTemplate(ASPxDropDownEdit DropDown)
        {
            DropDownEdit = DropDown;
        }
        public void InstantiateIn(System.Web.UI.Control container)
        {
            ctrl = (AssetLookupDropDownList)container.Page.LoadControl("~/ControlTemplates/uGovernIT/AssetLookupDropDownList.ascx");
            //ctrl.DisplayName = DisplayName;
            ctrl.IsMulti = IsMulti;
            ctrl.ID = "AssetLookup";
            ctrl.SetValueCheck = Value;
            ctrl.DropDown = DropDownEdit;
            ctrl.CustomProperties = (AssetTypeCustomProperties)CustomParameters;
           // ctrl.currentModuleName = ModuleName;
            container.Controls.Add(ctrl);
        }

        public string GetValues()
        {
            return string.Empty;
            //return ctrl.GetValues();
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
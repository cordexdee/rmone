using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Text;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.Specialized;
using DevExpress.Web.Data;
using System.Globalization;
using System.Threading;

namespace uGovernIT.Web
{
    [ToolboxData("<{0}:NumberValueBox runat=server></{0}:NumberValueBox>")]
    public class NumberValueBox:UGITControl
    {
        public DevExpress.Web.ASPxSpinEdit NumberSpinEdit { get; set; }
        public bool DecimalNotRequired { get; set; }
        public ModuleFormLayout FormLayout { get; set; }
        FieldConfigurationManager fieldManager;
        FieldConfiguration field;
        private string dataType { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        public NumberValueBox()
        {
            NumberSpinEdit = new DevExpress.Web.ASPxSpinEdit();

        }
        protected override void OnInit(System.EventArgs e)
        {
            CultureInfo culture = null;
            if (FieldName != null)
            {
                fieldManager = new FieldConfigurationManager(context);
                field = fieldManager.GetFieldByFieldName(FieldName);
                if (field != null)
                {
                    dataType = Convert.ToString(field.Datatype);
                }
                else if (FormLayout != null && !string.IsNullOrEmpty(FormLayout.ColumnType) && (FormLayout.ColumnType.Equals("Currency") || FormLayout.ColumnType.Equals("Percentage") || FormLayout.ColumnType.Equals("Integer") || FormLayout.ColumnType.Equals("Double")))
                   dataType = FormLayout.ColumnType;


            }
            if (dataType == "Currency")
            {
                culture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
                NumberSpinEdit.EnableViewState = true;
                NumberSpinEdit.AutoPostBack = false;
                NumberSpinEdit.DecimalPlaces = 2;
                NumberSpinEdit.DisplayFormatString = "{0:C}";


            }
            else if (dataType == "Percentage")
            {
                NumberSpinEdit.EnableViewState = true;
                NumberSpinEdit.AutoPostBack = false;
                NumberSpinEdit.MinValue = 1;
                NumberSpinEdit.MaxValue = 100;
                NumberSpinEdit.DecimalPlaces = 2;
                NumberSpinEdit.NullText = "From 1 to 100";
                NumberSpinEdit.NumberType = SpinEditNumberType.Float;
                NumberSpinEdit.ToolTip = "Percentage Value Should be from 1 to 100";
                NumberSpinEdit.DisplayFormatString = "{0:0.##}%";
                NumberSpinEdit.ShowOutOfRangeWarning = false;
            }
            else if (dataType == "Integer")
            {
                NumberSpinEdit.EnableViewState = true;
                NumberSpinEdit.AutoPostBack = false;
                NumberSpinEdit.DecimalPlaces = 0;
                NumberSpinEdit.NumberType = SpinEditNumberType.Integer;
                NumberSpinEdit.DisplayFormatString = "{0:0}";
                NumberSpinEdit.ShowOutOfRangeWarning = false;
            }
            else if (dataType == "Double")
            {
                NumberSpinEdit.EnableViewState = true;
                NumberSpinEdit.AutoPostBack = false;
                NumberSpinEdit.DecimalPlaces = 2;
                NumberSpinEdit.NumberType = SpinEditNumberType.Float;
                NumberSpinEdit.DisplayFormatString = "{0:0.##}";
                NumberSpinEdit.ShowOutOfRangeWarning = false;
            }
            else
            {
                if (DecimalNotRequired)
                {
                    NumberSpinEdit.EnableViewState = true;
                    NumberSpinEdit.AutoPostBack = false;
                    NumberSpinEdit.NumberType = SpinEditNumberType.Integer;
                }
                else
                {
                    NumberSpinEdit.EnableViewState = true;
                    NumberSpinEdit.AutoPostBack = false;
                    NumberSpinEdit.DecimalPlaces = 2;
                }
            }

            NumberSpinEdit.ID = this.ID + "_number";
            Controls.Add(NumberSpinEdit);
            base.OnInit(e);

        }

        public string GetValue()
        {
            string value = "";
            if (!string.IsNullOrEmpty(Convert.ToString(NumberSpinEdit.Value)))
            {
                value = Convert.ToString(NumberSpinEdit.Value);
            }
            return value.Trim();
        }
        public void SetValue(string value)
        {
            NumberSpinEdit.Value = value;

        }
    }
}
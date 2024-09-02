using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Text;
using uGovernIT.Utility;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public class UGITDropDown : UGITControl    {
        TextBox textBox;
        HiddenField hiddenBox;
       // public SPList SPListObj { get; set; }
        public string ChoiceFieldName { get; set; }
        public bool FillInChoice { get; set; }
        string addIconUrl = "/Content/images/uGovernIT/add_icon.png";
        string cancelIconUrl = "/Content/images/uGovernIT/cancel-icon.png";
        Label lb1;
        Label lb2;
        string uniqueID = string.Empty;
        Panel panel;
        Image addIcon;
        Image cancelIcon;

        public ASPxComboBox DropDown { get; set; }

        public int RequestTypeId { get; set; }

        public UGITDropDown()
        {
            DropDown = new ASPxComboBox();
            hiddenBox = new HiddenField();
            textBox = new TextBox();
            lb1 = new Label();
            lb2 = new Label();
            panel = new Panel();
            addIcon = new Image();
            cancelIcon = new Image();
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.DropDown.Width.Value <= 0)
            {
                this.DropDown.Width = new Unit(170);
            }

            DropDown.ClientInstanceName = "ugitdropdown";
            if (FillInChoice)
            {
                uniqueID = Guid.NewGuid().ToString().Split('-').LastOrDefault();
                this.DropDown.CssClass += string.Format(" {0} ugit-dropdown", ChoiceFieldName);
                Control ctr = this.DropDown;

                panel.CssClass += string.Format(" fleft {0}", uniqueID);
                panel.Controls.Add(hiddenBox);
                this.Controls.Add(panel);

                panel.Controls.Add(lb1);
                panel.Controls.Add(lb2);
                Label lb3 = new Label();
                lb1.Controls.Add(lb3);
                lb3.Controls.Add(ctr);
                textBox.CssClass = lb3.CssClass = "fleft";
                textBox.CssClass = "fleft";
                lb1.Width = new Unit(this.DropDown.Width.Value + 20);

                addIcon.CssClass = "fleft";
                addIcon.Style.Add("padding-left", "5px");
                addIcon.Style.Add("padding-top", "5px");
                addIcon.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                addIcon.ToolTip = "Add New Choice";
                addIcon.Attributes.Add("onclick", string.Format("addNewChoice('{0}')", uniqueID));
                addIcon.ImageUrl = addIconUrl;
                lb1.Controls.Add(addIcon);

                lb2.Controls.Add(textBox);
                cancelIcon.CssClass = "fleft";
                cancelIcon.ToolTip = "Cancel";
                cancelIcon.Style.Add("padding-left", "4px");
                addIcon.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                cancelIcon.Attributes.Add("onclick", string.Format("cancelNewChoice('{0}')", uniqueID));
                cancelIcon.ImageUrl = cancelIconUrl;
                lb2.Controls.Add(cancelIcon);
                lb2.Width = new Unit(this.DropDown.Width.Value + 20);
                textBox.Width = new Unit(this.DropDown.Width.Value - 12);


                lb2.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            else
            {
                DropDown.CssClass += string.Format(" {0} ugit-dropdown", ChoiceFieldName);
                this.Controls.Add(this.DropDown);
            }

            this.DropDown.TextField = ChoiceFieldName;
            this.DropDown.ValueField = ChoiceFieldName;
            this.DropDown.DataSource = GetChoiceData();
            this.DataBind();
            this.DropDown.Items.Insert(0, new ListEditItem(""));

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.FillInChoice)
            {
                if (hiddenBox.Value == "1")
                {
                    this.DropDown.Value = textBox.Text.Trim();
                    lb1.Style.Add(HtmlTextWriterStyle.Display, "none");
                    lb2.Style.Add(HtmlTextWriterStyle.Display, "block");
                }
                else
                {
                    lb1.Style.Add(HtmlTextWriterStyle.Display, "block");
                    lb2.Style.Add(HtmlTextWriterStyle.Display, "none");
                }
            }

            base.OnLoad(e);
        }

        private string GetJavascript(string uniqueID)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(" function addNewChoice(uniqueID) {0}", "{");
            sb.Append("$('.'+uniqueID+'ddldiv').hide();");
            sb.Append("$('.'+uniqueID+'txtdiv').show();");
            sb.Append("$('.'+uniqueID+'>input:hidden').val('1');");
            sb.Append("}");

            sb.AppendFormat(" function cancelNewChoice(uniqueID) {0}", "{");
            sb.Append("$('.' + uniqueID + 'ddldiv').show();");
            sb.Append("$('.' + uniqueID + 'txtdiv').hide();");
            sb.Append("$('.'+uniqueID+'>input:hidden').val('');");
            sb.Append("}");

            return string.Format("{0}", sb.ToString());
        }

        private DataTable GetChoiceData()
        {
            DataTable table = new DataTable();

            string strType = string.Empty;
            if (ChoiceFieldName == DatabaseObjects.Columns.TicketResolutionType)
            {
                //string viewFields = string.Format("<FieldRef Name='{0}' Nullable='True'/><FieldRef Name='{1}' Nullable='True'/>", DatabaseObjects.Columns.ResolutionTypes, DatabaseObjects.Columns.IssueTypeOptions);
                //SPListItem item = SPListHelper.GetSPListItem(SPListHelper.GetSPList(DatabaseObjects.Lists.RequestType), RequestTypeId, viewFields, true);
                //if (item != null)
                //    strType = Convert.ToString(item[DatabaseObjects.Columns.ResolutionTypes]).Trim();

            }

            if (!string.IsNullOrEmpty(strType))
            {
                if (table == null)
                    table = new DataTable();

                if (!table.Columns.Contains(ChoiceFieldName))
                    table.Columns.Add(ChoiceFieldName);

                //string[] arrayTypes = uHelper.SplitString(strType, new string[] { Constants.Separator, Constants.NewLineSeperator }, StringSplitOptions.None);
                //foreach (string rtitem in arrayTypes)
                //{
                //    if (!string.IsNullOrEmpty(rtitem))
                //        table.Rows.Add(rtitem);
                //}
            }
            else
            {
                //if (FillInChoice)
                //{
                //    SPQuery query = new SPQuery();
                //    query.ViewFields = string.Format("<FieldRef Name='{0}'/>", ChoiceFieldName);
                //    query.ViewFieldsOnly = true;
                //    query.Query = string.Format("<Where><And><IsNotNull><FieldRef Name='{0}'/></IsNotNull><Neq><FieldRef Name='{0}'/><Value Type='Text'></Value></Neq></And></Where>", ChoiceFieldName);
                //    table = SPListHelper.GetDataTable(SPListObj, query);
                //}


                if (table == null)
                    table = new DataTable();

                if (!table.Columns.Contains(ChoiceFieldName))
                    table.Columns.Add(ChoiceFieldName);

                //SPFieldChoice choiceField = SPListObj.Fields.GetFieldByInternalName(ChoiceFieldName) as SPFieldChoice;
                //if (choiceField != null)
                //{
                //    foreach (string choice in choiceField.Choices)
                //    {
                //        table.Rows.Add(choice);
                //    }
                //}

                if (table != null)
                {
                    if (this.FillInChoice)
                        table.DefaultView.Sort = string.Format("{0} asc", ChoiceFieldName);

                    table = table.DefaultView.ToTable(true, ChoiceFieldName);
                }
            }
            return table;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (this.FillInChoice)
            {
                lb1.CssClass = string.Format("fleft {0}ddldiv", uniqueID);
                lb2.CssClass = string.Format("fleft {0}txtdiv", uniqueID);
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "dropdown", GetJavascript(uniqueID), true);
            }

            base.OnPreRender(e);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }
    }
}
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class EmailContactsDropDown : System.Web.UI.UserControl
    {
        ApplicationContext context = HttpContext.Current.GetManagerContext();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();                
            }
        }

        private void BindGrid()
        {
            DataTable dtInternalUser = new DataTable();
            dtInternalUser = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AspNetUsers, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'", "ID,Name,Email", "");

            DataTable dtGroups = new DataTable();
            dtGroups = GetTableDataManager.GetTableData(DatabaseObjects.Tables.AspNetRoles, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'", "ID,Name,Title", "");
            DataRow row = null;

            foreach (DataRow item in dtGroups.Rows)
            {
                row = dtInternalUser.NewRow();
                row["Id"] = UGITUtility.ObjectToString(item["Id"]);
                row["Name"] = UGITUtility.ObjectToString(item["Title"]);
                row["Email"] = UGITUtility.ObjectToString(item["Name"]);

                dtInternalUser.Rows.Add(row);
            }

            bool AllowAllUserEmails = context.ConfigManager.GetValueAsBool(ConfigConstants.AllowAllUserEmails);
            if (AllowAllUserEmails)
            {
                DataTable dtContacts = new DataTable();
                dtContacts = GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMContact, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.EmailAddress} is not null and {DatabaseObjects.Columns.EmailAddress} != ''", "ID,Title,EmailAddress", "");
                foreach (DataRow item in dtContacts.Rows)
                {
                    row = dtInternalUser.NewRow();
                    row["Id"] = UGITUtility.ObjectToString(item["Id"]);
                    row["Name"] = UGITUtility.ObjectToString(item["Title"]);
                    row["Email"] = UGITUtility.ObjectToString(item["EmailAddress"]);

                    dtInternalUser.Rows.Add(row);
                }
            }
            //dtInternalUser.Merge(dtGroups, false, MissingSchemaAction.Ignore);
            ddlUsers.ItemTemplate = new ToDropDownTemplate(ddlUsers.ClientID);
            ddlUsers.ValueField = DatabaseObjects.Columns.ID;
            ddlUsers.TextField = DatabaseObjects.Columns.Name;
            ddlUsers.DataSource = dtInternalUser;
            ddlUsers.DataBind();

            ddlUsers.ItemTemplate = new ToDropDownTemplate(ddlUsers.ClientID);
            ddlUsers.ValueField = DatabaseObjects.Columns.ID;
            ddlUsers.TextField = DatabaseObjects.Columns.Name;
            ddlUsers.DataSource = dtInternalUser;
            ddlUsers.DataBind();
        }

        public string GetValues()
        {
            return Convert.ToString(ddlUsers.Value);
        }

        public List<string> GetValuesAsList()
        {
            List<string> values = new List<string>();
            if (!string.IsNullOrEmpty(Convert.ToString(ddlUsers.Value)))
            {
                values = UGITUtility.ConvertStringToList(Convert.ToString(ddlUsers.Value), Constants.Separator6);
                return values;
            }
            else
            {
                return values;
            }
        }
    }

    public class ToDropDownTemplate : IBindableTemplate
    {
        private string gridLookupID = string.Empty;
        private readonly string _tenantID = TenantHelper.GetTanantID();

        public ToDropDownTemplate(string id)
        {
            gridLookupID = id;
        }

        public void InstantiateIn(Control container)
        {

            ListEditItemTemplateContainer tContainer = container as ListEditItemTemplateContainer;

            if (tContainer.DataItem.GetType().FullName != "System.Data.DataRowView")
            {
                return;
            }

            DataRow rowView = ((System.Data.DataRowView)tContainer.DataItem).Row;

            List<string> listValue = new List<string>();
            listValue.Add(Convert.ToString(rowView[DatabaseObjects.Columns.Name] + " [" + rowView["Email"] + "]"));
            
            

            string data = string.Format(@"<table style='width:200px; overflow:hidden; float:left;'>
                                       <tr><td style='' valign='middle'><div style='padding:7px 3px; '>{0}</div>
                                         <div style='display:none'>{1}</div>
                                        </td></tr>
                                       </table>", string.Join(",", listValue.ToArray()), rowView["ID"]);

            LiteralControl ctd = new LiteralControl(data);
            ctd.EnableViewState = false;
            LiteralControl ct = new LiteralControl("<hr style='margin:2px; padding:0px; display:none;'/>");
            ct.EnableViewState = false;
            container.Controls.Add(ctd);
            container.Controls.Add(ct);
        }

        public IOrderedDictionary ExtractValues(Control container)
        {
            return new OrderedDictionary(); //throw new NotImplementedException();
        }
    }
}

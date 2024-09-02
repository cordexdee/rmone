using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using uGovernIT.Utility;
using uGovernIT.Manager.Managers;
using System.Web;
using uGovernIT.Manager;
using DevExpress.Web;
using uGovernIT.Helpers;
using uGovernIT.Utility.Entities;
//using Microsoft.AspNet.Identity;
//using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class QueryParameters : UserControl
    {
        public delegate void OnSubmitHandler(object sender,EventArgsSubmitEventHandler e);
        
        public event OnSubmitHandler OnSubmit = delegate { };

        public long Id { get; set; }
        public long QueryId { get; set; }
        public string Where { get; set; }
        public List<WhereInfo> WhereInfoList { get; set; }
        DashboardManager dManager = new DashboardManager(HttpContext.Current.GetManagerContext());
        
        protected override void OnInit(EventArgs e)
        {
            Dashboard uDashboard = dManager.LoadPanelById(QueryId);
            var query = uDashboard.panel as DashboardQuery;
            if (query.QueryInfo.WhereClauses.Exists(x => x.Valuetype == qValueType.Parameter))
            {
                WhereInfoList = query.QueryInfo.WhereClauses.FindAll(x => x.Valuetype == qValueType.Parameter);
            }

            if (WhereInfoList != null && WhereInfoList.Count > 0) CreateDynamicContols();

            tdvalues.InnerHtml = getQueryParameterHTML(QueryId, Where);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void ClearQueryParameterHTML(long queryId)
        {
             Dashboard uDB = dManager.LoadPanelById(queryId);
            var query = (DashboardQuery)uDB.panel;
            if (query.QueryInfo.WhereClauses.Exists(x => x.Valuetype == qValueType.Parameter))
            {
                tdvalues.InnerHtml = string.Empty;
                aEditbutton.Attributes.Add("style", "display:block;");
            }
            else
            {
                tdvalues.InnerHtml = "No parameter fields.";
                aEditbutton.Attributes.Add("style", "display:none;");
                uHelper.ReportScheduleDict = new Dictionary<string, object>();
                uHelper.ReportScheduleDict.Add(ReportScheduleConstant.Where, string.Empty);
            }
        }

        /// <summary>
        /// This function creates the dynamic textboxes and a button to get value for parameters.
        /// </summary>
        /// <param name="whereList"></param>
        public void CreateDynamicContols()
        {
            // Now iterate through the table and add controls 
            int i = 0;
            foreach (var item in WhereInfoList.Where(x => x.Valuetype == qValueType.Parameter))
            {
                TableRow row = new TableRow();
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                TextBox tb = new TextBox();
                tb.Width = Unit.Pixel(240);
                Label lb = new Label();
                cell1.Controls.Add(lb);
                cell1.CssClass = "ms-formlabel";
                cell2.CssClass = "ms-formbody";
                ASPxDateEdit date = new ASPxDateEdit();
               // date.DatePickerFrameUrl = uHelper.GetAbsoluteURL("/_layouts/15/iframe.aspx");
                String colName = item.ParameterName;
                lb.Text = colName + ": <span style='color:#FF0000;'>*</span>";
                //PeopleEditor peopleEditor = new PeopleEditor();
                UserValueBox peopleEditor = new UserValueBox();
               // peopleEditor.Rows = 1;
                peopleEditor.Width = Unit.Pixel(200);

                switch (item.ParameterType)
                {
                    case "TextBox":
                        // Add the control to the TableCell
                        cell2.Controls.Add(tb);
                        // Set a unique ID for each control added
                        tb.ID = "Ctrl_" + i;
                        tb.EnableViewState = true;
                        tb.Text = item.Value;
                        break;

                    case "DateTime":
                       // Add the control to the TableCell
                         //Set a unique ID for each control added
                          date.ID = "Ctrl_" + i; 
                        cell1.Controls.Add(lb);
                         cell2.Controls.Add(date);
                       // date.Date = true; 
                        DateTime dt = DateTime.Now.Date; ; 
                        DateTime.TryParse(item.Value, out dt);
                        date.Date = dt; 
                       
                       
                        break;

                    case "DropDown":

                        DropDownList ddlOptions = new DropDownList();
                        ddlOptions.ID = "Ctrl_" + i;
                        ddlOptions.CssClass = "param-value";
                        string[] vals = item.DrpOptions.OptionsDropdown.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        if (vals != null && vals.Length > 0)
                        {
                            for (int k = 0; k < vals.Length; k++)
                            {
                                ddlOptions.Items.Add(vals[k]);
                            }
                        }

                        ddlOptions.SelectedValue = item.DrpOptions.DropdownDefaultValue;
                        cell2.Controls.Add(ddlOptions);
                        // Set a unique ID for each control added
                        break;
                    case "Lookup":
                        DropDownList drpLookup = new DropDownList();
                        drpLookup.CssClass = "param-value";
                        drpLookup.ID = "Ctrl_" + i;
                        cell2.Controls.Add(drpLookup);

                        DataTable list = GetTableDataManager.GetTableData(item.LookupList.LookupListName);
                        DataColumn field = null;
                        if (list != null && list.Columns.Contains(item.LookupList.LookupField))
                        {
                            field = list.Columns[item.LookupList.LookupField];
                        }

                        if (field != null)
                        {
                            DataTable tb1 =list;
                            DataTable columnVals = new DataTable();
                            if (tb1 != null)
                            {
                                DataView dView = tb1.DefaultView;
                                dView.Sort = string.Format("{0} asc", field.ColumnName);
                                if (tb1.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                                {
                                    dView.RowFilter = string.Format("{0} = '{1}'", DatabaseObjects.Columns.ModuleNameLookup, item.LookupList.LookupModuleName);
                                }

                                columnVals = dView.ToTable(true, field.ColumnName, DatabaseObjects.Columns.Id);
                                drpLookup.DataValueField = field.ColumnName;
                                //drpLookup.ValueType = typeof(string);
                                drpLookup.DataTextField = field.ColumnName;
                                drpLookup.DataSource = columnVals;
                                drpLookup.DataBind();
                            }
                        }
                        break;
                }

                // Add the TableCell to the TableRow
                row.Cells.Add(cell1);
                row.Cells.Add(cell2);

                // Add the TableRow to the Table
                tParameter.Rows.AddAt(tParameter.Rows.Count-2, row);
                i++;
            }
        }
        
        protected void GetValueFromControls()
        {
            string where = string.Empty;
            int counter = 0;
            foreach (var item in WhereInfoList.Where(x => x.Valuetype == qValueType.Parameter))
            {
                
                if (tParameter.FindControl(string.Format("Ctrl_{0}", counter)) is ASPxDateEdit)
                {
                    ASPxDateEdit date = tParameter.FindControl(string.Format("Ctrl_{0}", counter)) as ASPxDateEdit;
                    if (date != null)
                    {
                        DateTime dt = date.Date;
                        item.Value = dt.ToString("M/dd/yyyy");
                    }
                }

                else if (tParameter.FindControl(string.Format("Ctrl_{0}", counter)) is TextBox)
                {
                    TextBox textBox = tParameter.FindControl(string.Format("Ctrl_{0}", counter)) as TextBox;
                    if (textBox != null)
                    {
                        item.Value = textBox.Text;
                    }
                }

                else if (tParameter.FindControl(string.Format("Ctrl_{0}", counter)) is DropDownList)
                {
                    DropDownList ddl = tParameter.FindControl(string.Format("Ctrl_{0}", counter)) as DropDownList;
                    if (ddl != null)
                    {
                        item.Value = ddl.SelectedValue;
                    }
                }
              
                counter++;

            }
            if (!WhereInfoList.Exists(x => string.IsNullOrEmpty(x.Value)))
            {
                Where = GetWhereExpression(WhereInfoList);
                tdvalues.InnerHtml = getQueryParameterHTML(QueryId, Where);
                pcParameter.ShowOnPageLoad = false;
                uHelper.ReportScheduleDict = new Dictionary<string, object>();
                uHelper.ReportScheduleDict.Add(ReportScheduleConstant.Where, Where);
                
                EventArgsSubmitEventHandler e = new EventArgsSubmitEventHandler();
                e.Value = Where;
                OnSubmit(this,e);
            }
            else
            {
                lblMsg.Text = "(*) Marked fields are required.";
            }
        }
       
        private string GetWhereExpression(List<WhereInfo> WhereInfoList)
        {
            string where = string.Empty;
            if (WhereInfoList.Count > 0)
            {
                var whereFilter = WhereInfoList.Where(w => w.Valuetype == qValueType.Parameter)
                                            .Select(w => w.Value).ToArray();
                where = string.Join(",", whereFilter);
            }
            return where;
        }

        private string getQueryParameterHTML(long queryId, string wherevalue)
        {
            StringBuilder sb = new StringBuilder();
            Dashboard uDB = dManager.LoadPanelById(queryId);
            var query = (DashboardQuery)uDB.panel;
            if (query.QueryInfo.WhereClauses.Exists(x => x.Valuetype == qValueType.Parameter))
            {
                aEditbutton.Attributes.Add("style", "display:block;");
                var whereInfo = query.QueryInfo.WhereClauses.Where(x => x.Valuetype == qValueType.Parameter).ToList();
                if(!string.IsNullOrEmpty(wherevalue))
                {
                    string[] wherevalues = wherevalue.Split(',');
                    if (whereInfo.Count == wherevalues.Length)
                    {
                        for (int i = 0; i < wherevalues.Length; i++)
                        {
                            sb.AppendFormat("<b>{0}</b>:", whereInfo[i].ParameterName);
                            sb.AppendFormat("{0};<br> ", wherevalues[i]);
                        }
                    }
                }
                
            }
            else
            {
                sb.Append("No parameter fields.");
                aEditbutton.Attributes.Add("style", "display:none;");
                uHelper.ReportScheduleDict = new Dictionary<string, object>();
                uHelper.ReportScheduleDict.Add(ReportScheduleConstant.Where, string.Empty);
            }
            if (!string.IsNullOrEmpty(Where) && !uHelper.ReportScheduleDict.ContainsKey(ReportScheduleConstant.Where))
            {
                uHelper.ReportScheduleDict = new Dictionary<string, object>();               
                uHelper.ReportScheduleDict.Add(ReportScheduleConstant.Where, Where);
               
            }
            return sb.ToString();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            GetValueFromControls();
        }
        
    }
}

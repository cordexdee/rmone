using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using System.Linq;
using System.Drawing;
using System.Web.UI.HtmlControls;
using uGovernIT.Core;
using System.Web;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.DxReport
{
    public partial class BusinessStrategy_Viewer : UserControl
    {
        #region Properties & Variable

        protected DataTable resultentTable;
        protected DataTable dtTasks;
        protected DataTable dtCompletedTasks;

        protected DataTable dtIssues;
        DataTable dataSource;
        int ticketsIds;
        BusinessStrategyDashboard obj;
        int initiativeCount;
        int strategiesCount;
        public string FilterExpressionInitiative
        {
            get;
            set;
        }
        public string FilterExpressionProject { get; set; }
        public string FilterExpressionBusinessStrategy { get; set; }
        public string pmmUrl;
        public string nprUrl;
        public string bsTitle;
        public string iniTitle;
        protected DataTable dtInitiative;
        UGITModule nprModule;
        UGITModule pmmModule;
        public string DelegateURl = "/layouts/ugovernit/delegatecontrol.aspx?control=businessstrategygroupeddata";
        public bool IsBusinessStrategyExist { get; set; }
        //SPList bsList;
        private string addNewItem = string.Empty;
        public bool isPMO;
        DataTable dtBusinessStrategy;
        private ProjectInitiative _SPListItem;
        List<BusinessStrategy> spBSList;
        ApplicationContext _Context = HttpContext.Current.GetManagerContext();
        ConfigurationVariableManager ConfigManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        BusinessStrategyManager BusinessStrategyMGR = new BusinessStrategyManager(HttpContext.Current.GetManagerContext());
        ProjectInitiativeViewManager ProjectInitiativeMGR = new ProjectInitiativeViewManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ModuleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        public string filterUrl = string.Empty;
        #endregion

        #region Methods & Events

        public BusinessStrategy_Viewer()
        {
            obj = new BusinessStrategyDashboard(_Context);
            dtBusinessStrategy = new DataTable();
            dtInitiative = new DataTable();
            dtBusinessStrategy = obj.GetAllBusinessStrategy();
            dtInitiative = obj.GetIntiative();
            User = HttpContext.Current.CurrentUser();
        }

        protected override void OnInit(EventArgs e)
        {
            filterUrl = _Context.SiteUrl + DelegateURl;
            User = HttpContext.Current.CurrentUser();
            bsTitle = string.IsNullOrEmpty(ConfigManager.GetValue(ConfigConstants.InitiativeLevel1Name)) ? "Business Initiative" : ConfigManager.GetValue(ConfigConstants.InitiativeLevel1Name);
            iniTitle = string.IsNullOrEmpty(ConfigManager.GetValue(ConfigConstants.InitiativeLevel2Name)) ? "Program" : ConfigManager.GetValue(ConfigConstants.InitiativeLevel2Name);
            addnewstrategy.HeaderText = string.Format("Add New {0}", bsTitle);
            addnewinitiativepopup.HeaderText = string.Format("Add New {0}", iniTitle);

            crdBusinessStrategy.SettingsPager.AlwaysShowPager = false;
            crdInitiative.SettingsPager.AlwaysShowPager = false;
            crdBusinessStrategy.SettingsPager.Visible = false;
            crdInitiative.SettingsPager.Visible = false;
            _SPListItem = new ProjectInitiative();   // SPListHelper.GetSPList(DatabaseObjects.Tables.ProjectInitiative).AddItem();
            spBSList = new List<BusinessStrategy>();  // spWeb.Lists[DatabaseObjects.Lists.BusinessStrategy];
            dataSource = obj.CreateSchema();
            if (dtBusinessStrategy != null && dtBusinessStrategy.Rows.Count > 0)   //spBSList changed with dtBusinessStrategy
                IsBusinessStrategyExist = true;
            BindBSDropDown();

            FilterListView(FilterCheckBox_cp, new EventArgs());

            dtTasks = obj.GetTasks();
            dtIssues = obj.GetIssues();
            LoadGroupData();
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ddlBusinessStrategy.SelectedIndex != -1)
                addnewinitiativepopup.JSProperties.Add("cpSelectedbs", ddlBusinessStrategy.SelectedValue);

            //uGITCache Module
            string group = ConfigManager.GetValue(ConfigConstants.PMOGroup);   // uGITCache.GetConfigVariableValue(ConfigConstants.PMOGroup);
            if (!string.IsNullOrEmpty(group))
                isPMO = _Context.UserManager.CheckUserIsInGroup(group, User);

            if (!isPMO)
            {
                crdBusinessStrategyChilds.SettingsCommandButton.EditButton.Styles.Style.CssClass = "showhideimage";
                crdInitiativeChilds.SettingsCommandButton.EditButton.Styles.Style.CssClass = "showhideimage";
                crdBusinessStrategyChilds.SettingsCommandButton.DeleteButton.Styles.Style.CssClass = "showhideimage";
                crdInitiativeChilds.SettingsCommandButton.DeleteButton.Styles.Style.CssClass = "showhideimage";
            }

            nprModule = ModuleManager.LoadByName(ModuleNames.NPR);
            pmmModule = ModuleManager.LoadByName(ModuleNames.PMM);
            //Details Url
            nprUrl = _Context.SiteUrl + nprModule.StaticModulePagePath;
            pmmUrl = _Context.SiteUrl + pmmModule.StaticModulePagePath;
        }

        protected void LoadGroupData()
        {
            imgback.ClientVisible = false;
            dataSource.Clear();

            bool flipview = false;
            if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "BI")
                flipview = true;

            if (IsBusinessStrategyExist)
            {

                DataTable resultdt = obj.UpdateEmptyOrNull(resultentTable, DatabaseObjects.Columns.BusinessStrategy, DatabaseObjects.Columns.BusinessStrategy);

                List<string> existInProj = resultdt.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessId])).Distinct().ToList();
                List<string> parentBsCount = dtBusinessStrategy.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Id])).ToList();
                strategiesCount = parentBsCount.AsEnumerable().Where(x => !existInProj.Contains(x)).Count() + existInProj.Count;
                strategiesCount = existInProj.Count;
                dataSource = obj.GenerateData(dataSource, resultdt, flipview, "Business Initiative");

                flipview = false;
                crdBusinessStrategy.ClientVisible = true;
                if (!crdBusinessStrategy.JSProperties.ContainsKey("cpHidechild"))
                    crdBusinessStrategy.JSProperties.Add("cpHidechild", false);

                if (dataSource.Rows.Count == 0)
                {
                    crdBusinessStrategy.JSProperties["cpHidechild"] = true;
                }
                else if (dataSource != null)
                {
                    DataRow[] drSource = dataSource.Select(string.Format("{0} <> ''", DatabaseObjects.Columns.Title));

                    if (drSource != null && drSource.Length > 0)
                        dataSource = drSource.CopyToDataTable();
                }

                crdBusinessStrategy.DataSource = dataSource;
                crdBusinessStrategy.DataBind();
                crdBusinessStrategyChilds.ClientVisible = false;
                if (dataSource.Rows.Count > 0)
                {
                    crdBusinessStrategyChilds.ClientVisible = true;
                    crdBusinessStrategyChilds_CustomCallback(null, null);
                }

                if (hdninitiativevalues.Contains("currentLevel") && Convert.ToString(hdninitiativevalues["currentLevel"]) == "INI")
                {
                    string callBackParam = Convert.ToString(Request.Params["__CALLBACKPARAM"]);
                    if (!string.IsNullOrEmpty(callBackParam) &&
                        (callBackParam.Contains("STARTEDIT") || callBackParam.Contains("CANCELEDIT") || callBackParam.Contains("UPDATEEDIT")))
                    {
                        crdInitiative_CustomCallback(crdInitiative, null);
                        crdInitiativeChilds_CustomCallback(crdInitiativeChilds, null);
                    }
                }
            }
            else if (!IsBusinessStrategyExist)
            {
                crdInitiative_CustomCallback(crdInitiative, new ASPxCardViewCustomCallbackEventArgs(string.Empty));
                flipview = false;

                if (dataSource.Rows.Count == 0)
                    crdInitiative.ClientVisible = false;

                if (!crdInitiative.JSProperties.ContainsKey("cpHidechild"))
                    crdInitiative.JSProperties.Add("cpHidechild", false);

                if (dataSource.Rows.Count == 0)
                {
                    crdInitiative.JSProperties["cpHidechild"] = true;
                }
                crdInitiativeChilds.ClientVisible = false;
                if (dataSource.Rows.Count > 0)
                {
                    crdInitiativeChilds.ClientVisible = true;
                    crdInitiativeChilds_CustomCallback(crdInitiativeChilds, new ASPxCardViewCustomCallbackEventArgs(string.Empty));
                }
            }
        }

        protected void crdProjectGroup_CustomCallback(object sender, ASPxCardViewCustomCallbackEventArgs e)
        {
            dataSource.Clear();
            if (resultentTable != null && resultentTable.Rows.Count > 0)
            {
                string filter = string.Empty;
                if (e != null && !string.IsNullOrEmpty(e.Parameters))
                {
                    filter = e.Parameters;
                }
                else if (!string.IsNullOrEmpty(hdnInitiative.Value) || !string.IsNullOrEmpty(hdnlabeltext.Value))
                {
                    if (IsBusinessStrategyExist)
                    {
                        filter = UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8).Length == 2 ? UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8)[1] : string.Empty;
                        if (string.IsNullOrEmpty(filter))
                            filter = UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8).Length == 2 ? UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8)[1] : string.Empty;
                    }
                    else
                    {
                        filter = UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8).Length == 1 ? UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8)[0] : string.Empty;
                        if (string.IsNullOrEmpty(filter))
                            filter = UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8).Length == 1 ? UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8)[0] : string.Empty;
                    }

                    if (filter.ToLower() == "unassigned")
                        filter = string.Empty;
                }
                filter = filter.Trim();

                bool flipview = false;
                if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "PROJ")
                    flipview = true;

                DataRow[] projects = resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && Convert.ToString(x[DatabaseObjects.Columns.InitiativeId]) == filter).ToArray(); //resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && Convert.ToString(x[DatabaseObjects.Columns.ProjectInitiativeLookup]) == filter).ToArray();

                crdProjectGroup.JSProperties.Add("cpCommonhas", false);

                if (projects.Length > 0)
                {
                    ticketsIds = projects.Length;
                    dataSource = obj.GenerateData(dataSource, projects.CopyToDataTable(), flipview, e.Parameters);
                }
                else
                {
                    crdProjectGroup.SettingsText.EmptyCard = "No Projects";
                    if (crdProjectGroup.JSProperties.ContainsKey("cpCommonhas"))
                        crdProjectGroup.JSProperties["cpCommonhas"] = true;
                }

                if (dataSource != null)
                {
                    DataRow[] drSource = dataSource.Select(string.Format("{0} <> ''", DatabaseObjects.Columns.Title));

                    if (drSource != null && drSource.Length > 0)
                        dataSource = drSource.CopyToDataTable();
                }

                crdProjectGroup.DataSource = dataSource;
                crdProjectGroup.DataBind();

                if (dataSource.Rows.Count > 0)
                {
                    FilterExpressionProject = e.Parameters;
                }
            }
        }

        protected void crdProjectGroup_CustomColumnDisplayText(object sender, ASPxCardViewColumnDisplayTextEventArgs e)
        {
            bool flipview = false;
            if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "PROJ")
                flipview = true;

            if (!flipview)
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    e.DisplayText = string.Format("<div class='green' title='Risk Level'>Risk Level</div>");

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "R")
                        e.DisplayText = string.Format("<div class='red' title='Risk Level' >Risk Level</div>");
                    else if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "Y")
                        e.DisplayText = string.Format("<div class='yellow' title='Risk Level' >Risk Level</div>");
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(e.Value), out total);
                    e.DisplayText = string.Format("<div class='gray' title='Total Budget' ><span>{0:C} Budget</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(total, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    double amoleft = 0;
                    double.TryParse(Convert.ToString(e.Value), out amoleft);
                    e.DisplayText = string.Format("<div class='gray' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(amoleft, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    double allm = 0;
                    double.TryParse(Convert.ToString(e.Value), out allm);
                    e.DisplayText = string.Format("<div class='gray' title='Total Duration'><span>{0} Months</span></div>", Convert.ToString(Math.Round(allm, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    double mleft = 0;
                    double.TryParse(Convert.ToString(e.Value), out mleft);
                    e.DisplayText = string.Format("<div class='gray' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(mleft, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    string txtissues = string.Empty;
                    if (string.IsNullOrEmpty(Convert.ToString(e.Value)))
                        txtissues = "No Issues";
                    else if (Convert.ToInt32(e.Value) == 0)
                        txtissues = "No Issues";
                    else
                        txtissues = string.Format("{0} Issues", e.Value);

                    e.DisplayText = string.Format("<div class='gray' title='Issues'><span>{0}</span></div>", txtissues);

                    int issues = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Convert.ToInt32(e.Value);
                    if (issues >= obj.IssuesYellowThreshold && issues < obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Issues'><span>{0}</span></div>", txtissues);
                    else if (issues >= obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='red' title='Issues'><span>{0}</span></div>", txtissues);
                }
            }
            else
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double totalpercentage = 0;
                    double.TryParse(Convert.ToString(e.GetFieldValue("TopNPro")), out totalpercentage);
                    totalpercentage = Math.Round(totalpercentage, 1);
                    e.DisplayText = string.Format("<div class='flipgray' title='Top 3 - % of Budget' ><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", totalpercentage > 100 ? 100 : totalpercentage));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='Cost Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    double longestDuration = 0;
                    double.TryParse(Convert.ToString(e.GetFieldValue("LongestInstanceProj")), out longestDuration);
                    longestDuration = Math.Round(longestDuration, 1);
                    e.DisplayText = string.Format("<div class='flipgray' title='Longest Project - % of Schedule'><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", longestDuration > 100 ? 100 : longestDuration));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='Schedule Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}<span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='# of Issues'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='# of Risks'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
            }
        }

        protected void innerProjectData_CustomColumnDisplayText(object sender, ASPxCardViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
            {
                e.DisplayText = string.Format("<div class='green' title='Risk Level'>Risk Level</div>");

                if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "R")
                    e.DisplayText = string.Format("<div class='red' title='Risk Level'>Risk Level</div>");
                else if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "Y")
                    e.DisplayText = string.Format("<div class='yellow' title='Risk Level'>Risk Level</div>");
            }
            else if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
            {
                double totalm = 0;
                double.TryParse(Convert.ToString(e.Value), out totalm);
                e.DisplayText = string.Format("<div class='gray' title='Total Budget'><span>{0:C} Budget</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(totalm, 2))));
            }
            else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
            {
                double budgetamount = string.IsNullOrEmpty(Convert.ToString(e.GetFieldValue("TotalAmount"))) ? 0 : Math.Round(Convert.ToDouble(e.GetFieldValue("TotalAmount")), 2);
                double redthreshold = 0;
                double yellowthreshold = 0;
                if (budgetamount > 0)
                {
                    //Get RedThresholdIn % of budget
                    redthreshold = (budgetamount * obj.AmountLeftRedThreshold) / 100;
                    //Get yellow threshold in % of budget
                    yellowthreshold = (budgetamount * obj.AmountLeftYellowThreshold) / 100;
                }
                double amountleft = 0;
                if (!string.IsNullOrEmpty(Convert.ToString(e.Value)))
                {
                    amountleft = Math.Round(Convert.ToDouble(e.Value), 2);
                }

                double leftamo = 0;
                double.TryParse(Convert.ToString(e.Value), out leftamo);
                e.DisplayText = string.Format("<div class='green' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamo, 2))));

                if (budgetamount == amountleft)
                    e.DisplayText = string.Format("<div class='green' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamo, 2))));
                else if (amountleft < 0 && budgetamount >= 0)
                    e.DisplayText = string.Format("<div class='red' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamo, 2))));
                else if (amountleft <= redthreshold)
                    e.DisplayText = string.Format("<div class='red' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamo, 2))));
                else if (amountleft > redthreshold && amountleft <= yellowthreshold)
                    e.DisplayText = string.Format("<div class='yellow' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamo, 2))));
            }
            else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
            {
                double allm = 0;
                double.TryParse(Convert.ToString(e.Value), out allm);
                e.DisplayText = string.Format("<div class='gray' title='Total Duration' ><span>{0} Months</span></div>", Convert.ToString(Math.Round(allm, 1)));
            }
            else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
            {
                double allmonths = string.IsNullOrEmpty(Convert.ToString(e.GetFieldValue("MonthLeft"))) ? 0 : Math.Round(Convert.ToDouble(e.GetFieldValue("AllMonth")), 1);
                double leftmonths = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Math.Round(Convert.ToDouble(e.Value), 1);

                double leftm = 0;
                double.TryParse(Convert.ToString(e.Value), out leftm);
                e.DisplayText = string.Format("<div class='green' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));

                if (leftmonths == allmonths)
                    e.DisplayText = string.Format("<div class='green' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));
                else if (leftmonths < 0 && allmonths >= 0)
                    e.DisplayText = string.Format("<div class='red' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));
                else if (leftmonths <= obj.MonthsLeftRedThreshold)
                    e.DisplayText = string.Format("<div class='red' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));
                else if (leftmonths > obj.MonthsLeftRedThreshold && leftmonths <= obj.MonthsLeftYellowThreshold)
                    e.DisplayText = string.Format("<div class='yellow' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));
            }
            else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
            {
                string txtissues = string.Empty;
                if (string.IsNullOrEmpty(Convert.ToString(e.Value)))
                    txtissues = "No Issues";
                else if (Convert.ToInt32(e.Value) == 0)
                    txtissues = "No Issues";
                else
                    txtissues = string.Format("{0} Issues", e.Value);

                e.DisplayText = string.Format("<div class='green' title='Issues'><span>{0}</span></div>", txtissues);

                int issues = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Convert.ToInt32(e.Value);
                if (issues >= obj.IssuesYellowThreshold && issues < obj.IssuesRedThreshold)
                    e.DisplayText = string.Format("<div class='yellow' title='Issues'><span>{0}</span></div>", txtissues);
                else if (issues >= obj.IssuesRedThreshold)
                    e.DisplayText = string.Format("<div class='red' title='Issues'><span>{0}</span></div>", txtissues);
            }
        }

        protected void innerProjectData_PageIndexChanged(object sender, EventArgs e)
        {
            innerProjectData_CustomCallback(null, null);
        }

        protected void pnlTotalProCount_Load(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            HtmlGenericControl txttotalproject = pnl.FindControl("txtProjectTotal") as HtmlGenericControl;
            if (txttotalproject != null)
                txttotalproject.InnerText = Convert.ToString(ticketsIds);
        }

        protected void crdBusinessStrategy_CustomColumnDisplayText(object sender, ASPxCardViewColumnDisplayTextEventArgs e)
        {
            bool flipview = false;
            if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "BI")
                flipview = true;

            if (!flipview)
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    e.DisplayText = string.Format("<div class='green' title='Risk Level'>Risk Level</div>");

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "R")
                        e.DisplayText = string.Format("<div class='red' title='Risk Level' >Risk Level</div>");
                    else if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "Y")
                        e.DisplayText = string.Format("<div class='yellow' title='Risk Level' >Risk Level</div>");

                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double total = 0;
                    double.TryParse(Convert.ToString(e.Value), out total);
                    e.DisplayText = string.Format("<div class='gray' title='Total Budget' ><span>{0:C} Budget</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(total, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    double amoleft = 0;
                    double.TryParse(Convert.ToString(e.Value), out amoleft);
                    e.DisplayText = string.Format("<div class='gray' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(amoleft, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    double allm = 0;
                    double.TryParse(Convert.ToString(e.Value), out allm);
                    e.DisplayText = string.Format("<div class='gray' title='Total Duration'><span>{0} Months</span></div>", Convert.ToString(Math.Round(allm, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    double mleft = 0;
                    double.TryParse(Convert.ToString(e.Value), out mleft);
                    e.DisplayText = string.Format("<div class='gray' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(mleft, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    string txtissues = string.Empty;
                    if (string.IsNullOrEmpty(Convert.ToString(e.Value)))
                        txtissues = "No Issues";
                    else if (Convert.ToInt32(e.Value) == 0)
                        txtissues = "No Issues";
                    else
                        txtissues = string.Format("{0} Issues", e.Value);

                    e.DisplayText = string.Format("<div class='gray' title='Issues'><span>{0}</span></div>", txtissues);
                }
            }
            else
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {

                    double totalamount = 0;
                    double.TryParse(Convert.ToString(e.GetFieldValue("TopNBS")), out totalamount);
                    totalamount = Math.Round(totalamount, 1);
                    e.DisplayText = string.Format("<div class='flipgray' title='Top 3 - % of Budget' ><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", totalamount > 100 ? 100 : totalamount));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftR")), out redcount);

                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftY")), out yellowcount);

                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='Cost Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    double longestDuration = 0;
                    double.TryParse(Convert.ToString(e.GetFieldValue("LongestInstanceBS")), out longestDuration);

                    longestDuration = Math.Round(longestDuration, 1);
                    e.DisplayText = string.Format("<div class='flipgray' title='Longest Project - % of Schedule'><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", longestDuration > 100 ? 100 : longestDuration));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftG")), out greencount);


                    e.DisplayText = string.Format("<div class='flipgray' title='Schedule Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}<span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesG")), out greencount);
                    e.DisplayText = string.Format("<div class='flipgray' title='# of Issues'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='# of Risks'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
            }
        }

        protected void pnlBusinessStrategiesCount_Load(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            HtmlGenericControl txttotalproject = pnl.FindControl("txtBusinessStrategiesTotal") as HtmlGenericControl;
            if (txttotalproject != null)
                txttotalproject.InnerText = Convert.ToString(strategiesCount);
        }

        protected void crdInitiative_CustomColumnDisplayText(object sender, ASPxCardViewColumnDisplayTextEventArgs e)
        {
            bool flipview = false;
            if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "PRO")
                flipview = true;
            if (!flipview)
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    e.DisplayText = string.Format("<div class='green' title='Risk Level'>Risk Level</div>");

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "R")
                        e.DisplayText = string.Format("<div class='red' title='Risk Level' >Risk Level</div>");
                    else if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "Y")
                        e.DisplayText = string.Format("<div class='yellow' title='Risk Level' >Risk Level</div>");

                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double totalamount = 0;
                    double.TryParse(Convert.ToString(e.Value), out totalamount);
                    e.DisplayText = string.Format("<div class='gray' title='Total Budget' ><span>{0:C} Budget</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(totalamount, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    double amountleft = 0;
                    double.TryParse(Convert.ToString(e.Value), out amountleft);
                    e.DisplayText = string.Format("<div class='gray' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(amountleft, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    decimal allmonth = 0;
                    decimal.TryParse(Convert.ToString(e.Value), out allmonth);
                    e.DisplayText = string.Format("<div class='gray' title='Total Duration'><span>{0} Months</span></div>", Convert.ToString(Math.Round(allmonth, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    decimal monthleft = 0;
                    decimal.TryParse(Convert.ToString(e.Value), out monthleft);
                    e.DisplayText = string.Format("<div class='gray' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(monthleft, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    string txtissues = string.Empty;
                    if (string.IsNullOrEmpty(Convert.ToString(e.Value)))
                        txtissues = "No Issues";
                    else if (Convert.ToInt32(e.Value) == 0)
                        txtissues = "No Issues";
                    else
                        txtissues = string.Format("{0} Issues", e.Value);

                    e.DisplayText = string.Format("<div class='gray' title='Issues'><span>{0}</span></div>", txtissues);

                    int issues = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Convert.ToInt32(e.Value);
                    if (issues >= obj.IssuesYellowThreshold && issues < obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Issues'><span>{0}</span></div>", txtissues);
                    else if (issues >= obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='red' title='Issues'><span>{0}</span></div>", txtissues);
                }
            }
            else
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double totalamountpercentage = 0;
                    double.TryParse(Convert.ToString(e.GetFieldValue("TopNIni")), out totalamountpercentage);
                    totalamountpercentage = Math.Round(totalamountpercentage, 1);
                    e.DisplayText = string.Format("<div class='flipgray' title='Top 3 - % of Budget' ><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", totalamountpercentage > 100 ? 100 : totalamountpercentage));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='Cost Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    double longestDuration = 0;
                    double.TryParse(Convert.ToString(e.GetFieldValue("LongestInstanceI")), out longestDuration);
                    longestDuration = Math.Round(longestDuration, 1);
                    e.DisplayText = string.Format("<div class='flipgray' title='Longest Project - % of Schedule'><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", longestDuration > 100 ? 100 : longestDuration));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftG")), out greencount);
                    e.DisplayText = string.Format("<div class='flipgray' title='Schedule Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}<span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='# of Issues'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelG")), out greencount);
                    e.DisplayText = string.Format("<div class='flipgray' title='# of Risks'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
            }
        }

        protected void pnlInitiative_Load(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            HtmlGenericControl txttotalproject = pnl.FindControl("txtInitiative") as HtmlGenericControl;

            if (txttotalproject != null)
                txttotalproject.InnerText = Convert.ToString(initiativeCount);
        }

        protected void crdBusinessStrategyChilds_CustomColumnDisplayText(object sender, ASPxCardViewColumnDisplayTextEventArgs e)
        {
            bool flipview = false;
            if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "BI")
                flipview = true;
            if (!flipview)
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    e.DisplayText = string.Format("<div class='green' title='Risk Level'>Risk Level</div>");

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "R")
                        e.DisplayText = string.Format("<div class='red' title='Risk Level'>Risk Level</div>");
                    else if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "Y")
                        e.DisplayText = string.Format("<div class='yellow' title='Risk Level'>Risk Level</div>");
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double totalamount = 0;
                    double.TryParse(Convert.ToString(e.Value), out totalamount);
                    e.DisplayText = string.Format("<div class='gray' title='Total Budget'><span>{0:C} Budget</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(totalamount, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    double budgetamount = string.IsNullOrEmpty(Convert.ToString(e.GetFieldValue("TotalAmount"))) ? 0 : Math.Round(Convert.ToDouble(e.GetFieldValue("TotalAmount")), 2);
                    double redthreshold = 0;
                    double yellowthreshold = 0;
                    if (budgetamount > 0)
                    {
                        //Get RedThresholdIn % of budget
                        redthreshold = (budgetamount * obj.AmountLeftRedThreshold) / 100;
                        //Get yellow threshold in % of budget
                        yellowthreshold = (budgetamount * obj.AmountLeftYellowThreshold) / 100;
                    }
                    double amountleft = 0;
                    if (!string.IsNullOrEmpty(Convert.ToString(e.Value)))
                    {
                        amountleft = Math.Round(Convert.ToDouble(e.Value), 2);
                    }
                    double leftamval = 0;
                    double.TryParse(Convert.ToString(e.Value), out leftamval);
                    e.DisplayText = string.Format("<div class='green' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamval, 2))));

                    if (budgetamount == amountleft)
                        e.DisplayText = string.Format("<div class='green' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamval, 2))));
                    else if (amountleft < 0 && budgetamount >= 0)
                        e.DisplayText = string.Format("<div class='red' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamval, 2))));
                    else if (amountleft <= redthreshold)
                        e.DisplayText = string.Format("<div class='red' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamval, 2))));
                    else if (amountleft > redthreshold && amountleft <= yellowthreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(leftamval, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    decimal allmonth = 0;
                    decimal.TryParse(Convert.ToString(e.Value), out allmonth);
                    e.DisplayText = string.Format("<div class='gray' title='Total Duration' ><span>{0} Months</span></div>", Convert.ToString(Math.Round(allmonth, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    double allmonths = string.IsNullOrEmpty(Convert.ToString(e.GetFieldValue("MonthLeft"))) ? 0 : Math.Round(Convert.ToDouble(e.GetFieldValue("AllMonth")), 1);
                    double leftmonths = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Math.Round(Convert.ToDouble(e.Value), 1);

                    e.DisplayText = string.Format("<div class='green' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftmonths, 1)));

                    if (leftmonths == allmonths)
                        e.DisplayText = string.Format("<div class='green' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftmonths, 1)));
                    else if (leftmonths < 0 && allmonths >= 0)
                        e.DisplayText = string.Format("<div class='red' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftmonths, 1)));
                    else if (leftmonths <= obj.MonthsLeftRedThreshold)
                        e.DisplayText = string.Format("<div class='red' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftmonths, 1)));
                    else if (leftmonths > obj.MonthsLeftRedThreshold && leftmonths <= obj.MonthsLeftYellowThreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftmonths, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    string txtissues = string.Empty;
                    if (string.IsNullOrEmpty(Convert.ToString(e.Value)))
                        txtissues = "No Issues";
                    else if (Convert.ToInt32(e.Value) == 0)
                        txtissues = "No Issues";
                    else
                        txtissues = string.Format("{0} Issues", e.Value);

                    e.DisplayText = string.Format("<div class='green' title='Issues'><span>{0}</span></div>", txtissues);

                    int issues = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Convert.ToInt32(e.Value);
                    if (issues >= obj.IssuesYellowThreshold && issues < obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Issues'><span>{0}</span></div>", txtissues);
                    else if (issues >= obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='red' title='Issues'><span>{0}</span></div>", txtissues);
                }
            }
            else
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double topnbs = 0;
                    double.TryParse(Convert.ToString(e.GetFieldValue("TopNBS")), out topnbs);

                    double totalPercentage = Math.Round(topnbs);
                    e.DisplayText = string.Format("<div class='flipgray' title='Top 3 - % of Budget' ><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", totalPercentage > 100 ? 100 : totalPercentage));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftR")), out redcount);

                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='Cost Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    double longestDuration = 0;
                    double.TryParse(Convert.ToString(e.GetFieldValue("LongestInstanceBS")), out longestDuration);
                    longestDuration = Math.Round(longestDuration, 1);
                    e.DisplayText = string.Format("<div class='flipgray' title='Longest Project - % of Schedule'><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", longestDuration > 100 ? 100 : longestDuration));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftG")), out greencount);
                    e.DisplayText = string.Format("<div class='flipgray' title='Schedule Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}<span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesG")), out greencount);
                    e.DisplayText = string.Format("<div class='flipgray' title='# of Issues'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelG")), out greencount);
                    e.DisplayText = string.Format("<div class='flipgray' title='# of Risks'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
            }
        }

        protected void crdBusinessStrategyChilds_PageIndexChanged(object sender, EventArgs e)
        {
            crdBusinessStrategyChilds_CustomCallback(null, null);
        }

        protected void crdBusinessStrategyChilds_CustomCallback(object sender, ASPxCardViewCustomCallbackEventArgs e)
        {
            dataSource.Clear();
            if (!dataSource.Columns.Contains(DatabaseObjects.Columns.TicketId))
                dataSource.Columns.Add(DatabaseObjects.Columns.TicketId);
            if (resultentTable != null && resultentTable.Rows.Count > 0)
            {
                string filter = FilterExpressionBusinessStrategy;
                if (e != null)
                {
                    FilterExpressionBusinessStrategy = e.Parameters;
                    filter = e.Parameters;
                }

                DataTable dtBSUnique = obj.UpdateEmptyOrNull(resultentTable, DatabaseObjects.Columns.BusinessStrategy, DatabaseObjects.Columns.BusinessStrategy);
                var businessStrat = dtBSUnique.AsEnumerable().GroupBy(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessId]));
                List<string> lstExistInProj = businessStrat.AsEnumerable().Select(x => x.Key.ToString()).Distinct().ToList();
                List<string> lstparent = dtBusinessStrategy.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Id])).ToList();
                if (businessStrat.Count() > 0)
                    bsTitle = string.IsNullOrEmpty(ConfigManager.GetValue(ConfigConstants.InitiativeLevel1Name)) ? "Business Initiative(s)" : string.Format("{0}(s)", ConfigManager.GetValue(ConfigConstants.InitiativeLevel1Name));

                crdBusinessStrategyChilds.SettingsCommandButton.EditButton.Image.ToolTip = string.Format("Edit {0}", bsTitle);
                crdBusinessStrategyChilds.SettingsCommandButton.DeleteButton.Image.ToolTip = string.Format("Delete {0} ", bsTitle);
                bool flipview = false;
                if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "BI")
                    flipview = true;
                foreach (var current in businessStrat)
                {
                    DataRow[] drColl = current.ToArray();
                    if (drColl.Length > 0)
                    {
                        string key = Convert.ToString(current.Key);
                        string title = string.Empty;
                        title = Convert.ToString(drColl[0][DatabaseObjects.Columns.BusinessStrategy]);
                        if (string.IsNullOrEmpty(key))
                        {
                            key = "<Unassigned>";
                            title = "<Unassigned>";
                        }

                        dataSource = obj.GenerateData(dataSource, drColl.CopyToDataTable(), flipview, title);
                    }
                }

                if (dataSource != null)
                {
                    //DataRow[] drSource = dataSource.Select(string.Format("{0} <> ''", DatabaseObjects.Columns.BusinessId));

                    //if (drSource != null && drSource.Length > 0)
                    //    dataSource = drSource.CopyToDataTable();
                }

                crdBusinessStrategyChilds.DataSource = dataSource;
                crdBusinessStrategyChilds.DataBind();
            }
        }

        protected void crdInitiativeChilds_CustomColumnDisplayText(object sender, ASPxCardViewColumnDisplayTextEventArgs e)
        {
            bool flipview = false;
            if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "PRO")
                flipview = true;
            if (!flipview)
            {

                if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    e.DisplayText = string.Format("<div class='green' title='Risk Level'>Risk Level</div>");

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "R")
                        e.DisplayText = string.Format("<div class='red' title='Risk Level'>Risk Level</div>");
                    else if (!string.IsNullOrWhiteSpace(Convert.ToString(e.Value)) && Convert.ToString(e.Value) == "Y")
                        e.DisplayText = string.Format("<div class='yellow' title='Risk Level'>Risk Level</div>");
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double totalamount = 0;
                    double.TryParse(Convert.ToString(e.Value), out totalamount);
                    e.DisplayText = string.Format("<div class='gray' title='Total Budget'><span>{0:C} Budget</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(totalamount, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    double budgetamount = string.IsNullOrEmpty(Convert.ToString(e.GetFieldValue("TotalAmount"))) ? 0 : Math.Round(Convert.ToDouble(e.GetFieldValue("TotalAmount")), 2);
                    double redthreshold = 0;
                    double yellowthreshold = 0;
                    if (budgetamount > 0)
                    {
                        //Get RedThresholdIn % of budget
                        redthreshold = (budgetamount * obj.AmountLeftRedThreshold) / 100;
                        //Get yellow threshold in % of budget
                        yellowthreshold = (budgetamount * obj.AmountLeftYellowThreshold) / 100;
                    }
                    double amountleft = 0;
                    if (!string.IsNullOrEmpty(Convert.ToString(e.Value)))
                    {
                        amountleft = Math.Round(Convert.ToDouble(e.Value), 2);
                    }
                    double left = 0;
                    double.TryParse(Convert.ToString(e.Value), out left);
                    e.DisplayText = string.Format("<div class='green' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(left, 2))));

                    if (budgetamount == amountleft)
                        e.DisplayText = string.Format("<div class='green' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(left, 2))));
                    else if (amountleft < 0 && budgetamount >= 0)
                        e.DisplayText = string.Format("<div class='red' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(left, 2))));
                    else if (amountleft <= redthreshold)
                        e.DisplayText = string.Format("<div class='red' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(left, 2))));
                    else if (amountleft > redthreshold && amountleft <= yellowthreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Amount Left' ><span>{0:C} Left</span></div>", UGITUtility.StringToDouble(Convert.ToString(Math.Round(left, 2))));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    decimal allm = 0;
                    decimal.TryParse(Convert.ToString(e.Value), out allm);
                    e.DisplayText = string.Format("<div class='gray' title='Total Duration' ><span>{0} Months</span></div>", Convert.ToString(Math.Round(allm, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    double allmonths = string.IsNullOrEmpty(Convert.ToString(e.GetFieldValue("MonthLeft"))) ? 0 : Math.Round(Convert.ToDouble(e.GetFieldValue("AllMonth")), 1);
                    double leftmonths = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Math.Round(Convert.ToDouble(e.Value), 1);
                    double leftm = 0;
                    double.TryParse(Convert.ToString(e.Value), out leftm);
                    e.DisplayText = string.Format("<div class='green' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));

                    if (leftmonths == allmonths)
                        e.DisplayText = string.Format("<div class='green' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));
                    else if (leftmonths < 0 && allmonths >= 0)
                        e.DisplayText = string.Format("<div class='red' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));
                    else if (leftmonths <= obj.MonthsLeftRedThreshold)
                        e.DisplayText = string.Format("<div class='red' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));
                    else if (leftmonths > obj.MonthsLeftRedThreshold && leftmonths <= obj.MonthsLeftYellowThreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Duration Left'><span>{0} Months Left</span></div>", Convert.ToString(Math.Round(leftm, 1)));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    string txtissues = string.Empty;
                    if (string.IsNullOrEmpty(Convert.ToString(e.Value)))
                        txtissues = "No Issues";
                    else if (Convert.ToInt32(e.Value) == 0)
                        txtissues = "No Issues";
                    else
                        txtissues = string.Format("{0} Issues", e.Value);

                    e.DisplayText = string.Format("<div class='green' title='Issues'><span>{0}</span></div>", txtissues);

                    int issues = string.IsNullOrEmpty(Convert.ToString(e.Value)) ? 0 : Convert.ToInt32(e.Value);
                    if (issues >= obj.IssuesYellowThreshold && issues < obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='yellow' title='Issues'><span>{0}</span></div>", txtissues);
                    else if (issues >= obj.IssuesRedThreshold)
                        e.DisplayText = string.Format("<div class='red' title='Issues'><span>{0}</span></div>", txtissues);
                }

            }
            else
            {
                if (e.Column.FieldName == DatabaseObjects.Columns.TotalAmount)
                {
                    double totalamountpercentage = 0;
                    bool isnull = e.GetFieldValue("TopNIni") is DBNull;
                    if (!isnull)
                        totalamountpercentage = Math.Round(Convert.ToDouble(e.GetFieldValue("TopNIni")), 1);

                    e.DisplayText = string.Format("<div class='flipgray' title='Top 3 - % of Budget' ><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", totalamountpercentage > 100 ? 100 : totalamountpercentage));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AmountLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("AmountLeftG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='Cost Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.AllMonth)
                {
                    double longestDuration = 0;
                    bool isnull = e.GetFieldValue("LongestInstanceI") is DBNull;
                    if (!isnull)
                        longestDuration = Math.Round(Convert.ToDouble(e.GetFieldValue("LongestInstanceI")), 1);

                    e.DisplayText = string.Format("<div class='flipgray' title='Longest Project - % of Schedule'><div class='flipblue' style='width:{0};'><span>{0}</span></div></div>", string.Format("{0}%", longestDuration > 100 ? 100 : longestDuration));
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.MonthLeft)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("MonthLeftG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='Schedule Variance - # of Projects'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}<span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.Issues)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("IssuesG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='# of Issues'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }
                else if (e.Column.FieldName == DatabaseObjects.Columns.RiskLevel)
                {
                    int redcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelR")), out redcount);
                    int yellowcount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelY")), out yellowcount);
                    int greencount = 0;
                    int.TryParse(Convert.ToString(e.GetFieldValue("RiskLevelG")), out greencount);

                    e.DisplayText = string.Format("<div class='flipgray' title='# of Risks'><div class='flipgreen' style='width:33%;float:left;'><span>{0}</span></div><div class='flipyellow' style='width:33%;float:left;'><span>{1}</span></div><div class='flipred' style='width:34%;float:left;'><span>{2}</span></div></div>", greencount, yellowcount, redcount);
                }

            }

        }

        protected void crdInitiativeChilds_CustomCallback(object sender, ASPxCardViewCustomCallbackEventArgs e)
        {
            dataSource.Clear();
            if (!dataSource.Columns.Contains(DatabaseObjects.Columns.TicketId))
                dataSource.Columns.Add(DatabaseObjects.Columns.TicketId);

            //Exclude IsPrivate marked projects
            //resultentTable = uHelper.ExcludeIsPrivateMarked(_Context, resultentTable, User);

            if (resultentTable != null && resultentTable.Rows.Count > 0)
            {
                string filter = string.Empty;
                if (e != null && !string.IsNullOrEmpty(e.Parameters))
                {
                    filter = e.Parameters;
                }
                else if (!string.IsNullOrEmpty(hdnInitiative.Value) || !string.IsNullOrEmpty(hdnlabeltext.Value))
                {
                    filter = UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8).Length >= 1 ? UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8)[0] : string.Empty;
                    if (string.IsNullOrWhiteSpace(filter))
                        filter = UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8).Length >= 1 ? UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8)[0] : string.Empty;
                    if (filter.ToLower() == "unassigned")
                        filter = string.Empty;
                }
                else if (hdninitiativevalues.Contains("currentselectedkey") && !string.IsNullOrEmpty(Convert.ToString(hdninitiativevalues["currentselectedkey"])))
                {
                    filter = Convert.ToString(hdninitiativevalues["currentselectedkey"]);
                }
                else
                    filter = "all";

                filter = filter.Trim();
                List<string> mapiniid = dtInitiative.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessId]) == filter).Select(x => Convert.ToString(x[DatabaseObjects.Columns.Id])).ToList();
                if (string.IsNullOrEmpty(filter))
                {
                    DataRow[] unmappedPro = resultentTable.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessId]) == filter && Convert.ToString(x[DatabaseObjects.Columns.InitiativeId]) == filter).ToArray();
                    if (unmappedPro != null && unmappedPro.Length > 0 && !mapiniid.Contains(string.Empty))
                        mapiniid.Add(string.Empty);

                }
                var initiativeChild = resultentTable.AsEnumerable().Where(x => mapiniid.Contains(Convert.ToString(x[DatabaseObjects.Columns.InitiativeId]))).GroupBy(x => Convert.ToString(x[DatabaseObjects.Columns.InitiativeId]));//resultentTable.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessStrategy]) == filter).ToArray();
                if (filter.ToLower() == "all")
                {
                    initiativeChild = resultentTable.AsEnumerable().GroupBy(x => Convert.ToString(x[DatabaseObjects.Columns.InitiativeId]));
                }

                List<string> existMapids = initiativeChild.AsEnumerable().Select(x => Convert.ToString(x.Key)).ToList();
                List<string> allInitIds = dtInitiative.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.Id])).ToList();

                bool flipview = false;
                if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "PRO")
                    flipview = true;
                foreach (var child in initiativeChild)
                {
                    DataRow[] dr = new DataRow[0];
                    dr = child.ToArray();
                    if (dr.Length > 0)
                    {
                        string key = Convert.ToString(child.Key);
                        string title = string.Empty;

                        if (!string.IsNullOrEmpty(Convert.ToString(dr[0][DatabaseObjects.Columns.ProjectInitiativeLookup])))
                            title = ProjectInitiativeMGR.LoadByID(Convert.ToInt64(dr[0][DatabaseObjects.Columns.ProjectInitiativeLookup])).Title;

                        if (string.IsNullOrEmpty(key))
                        {
                            key = "<Unassigned>";
                            title = "<Unassigned>";
                        }
                        dataSource = obj.GenerateData(dataSource, dr.CopyToDataTable(), flipview, title);
                    }
                }

                iniTitle = "Program";
                if (dataSource.Rows.Count > 0)
                    iniTitle = string.IsNullOrEmpty(ConfigManager.GetValue(ConfigConstants.InitiativeLevel2Name)) ? "Program(s)" : string.Format("{0}(s)", ConfigManager.GetValue(ConfigConstants.InitiativeLevel2Name));

                crdInitiativeChilds.SettingsCommandButton.EditButton.Image.ToolTip = string.Format("Edit {0}", iniTitle);
                crdInitiativeChilds.SettingsCommandButton.DeleteButton.Image.ToolTip = string.Format("Delete {0} ", iniTitle);

                if (dataSource != null)
                {
                    DataRow[] drSource = dataSource.Select(string.Format("{0} <> ''", DatabaseObjects.Columns.InitiativeId));

                    if (drSource != null && drSource.Length > 0)
                        dataSource = drSource.CopyToDataTable();
                }

                crdInitiativeChilds.DataSource = dataSource;
                crdInitiativeChilds.DataBind();
            }
        }

        protected void crdInitiativeChilds_PageIndexChanged(object sender, EventArgs e)
        {
            crdInitiativeChilds_CustomCallback(null, null);
        }
        protected void crdInitiative_CustomCallback(object sender, ASPxCardViewCustomCallbackEventArgs e)
        {
            dataSource.Clear();
            if (resultentTable != null && resultentTable.Rows.Count > 0)
            {
                string filter = string.Empty;
                if (e != null && !string.IsNullOrEmpty(e.Parameters))
                {
                    filter = e.Parameters;
                }
                else if (!string.IsNullOrEmpty(hdnInitiative.Value) || !string.IsNullOrEmpty(hdnlabeltext.Value))
                {
                    filter = UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8).Length == 1 ? UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8)[0] : string.Empty;
                    if (string.IsNullOrWhiteSpace(filter))
                        filter = UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8).Length == 1 ? UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8)[0] : string.Empty;
                    if (filter.ToLower() == "unassigned")
                        filter = string.Empty;
                }
                else if (e != null && string.IsNullOrEmpty(e.Parameters))
                    filter = "all";
                else if (hdninitiativevalues.Contains("currentselectedkey") && !string.IsNullOrEmpty(Convert.ToString(hdninitiativevalues["currentselectedkey"])))
                {
                    filter = Convert.ToString(hdninitiativevalues["currentselectedkey"]);
                }

                filter = filter.Trim();

                var groupCount = dtInitiative.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessId]) == filter).GroupBy(x => x[DatabaseObjects.Columns.ID]);
                initiativeCount = groupCount.AsEnumerable().Select(x => x.Key).Count();

                if (filter.ToLower() == "all")
                {
                    List<string> mppids = resultentTable.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.InitiativeId])).Distinct().ToList();
                    List<string> allids = dtInitiative.AsEnumerable().Select(x => Convert.ToString(x[DatabaseObjects.Columns.ID])).ToList();
                    initiativeCount = allids.AsEnumerable().Where(x => !mppids.Contains(x)).ToList().Count + mppids.Count;
                }

                iniTitle = "Program";
                if (initiativeCount > 0)
                    iniTitle = string.IsNullOrEmpty(ConfigManager.GetValue(ConfigConstants.InitiativeLevel2Name)) ? "Program(s)" : string.Format("{0}(s)", ConfigManager.GetValue(ConfigConstants.InitiativeLevel2Name));

                bool flipview = false;
                if (hndkeepflipviewtrack.Contains("currentkey") && Convert.ToString(hndkeepflipviewtrack.Get("currentkey")) == "PRO")
                    flipview = true;
                DataRow[] initiative;
                if (filter != "all")
                {
                    List<string> mapiniid = dtInitiative.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessId]) == filter).Select(x => Convert.ToString(x[DatabaseObjects.Columns.Id])).ToList();
                    if (string.IsNullOrEmpty(filter))
                    {
                        DataRow[] unmappedPro = resultentTable.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessId]) == filter && Convert.ToString(x[DatabaseObjects.Columns.InitiativeId]) == filter).ToArray();
                        if (unmappedPro != null && unmappedPro.Length > 0 && !mapiniid.Contains(string.Empty))
                            mapiniid.Add(string.Empty);

                    }

                    initiative = resultentTable.AsEnumerable().Where(x => mapiniid.Contains(Convert.ToString(x[DatabaseObjects.Columns.InitiativeId]))).ToArray();
                    if (initiative.Length == 0)
                    {
                        DataTable dtclone = resultentTable.Clone();
                        DataRow[] driniColl = dtInitiative.AsEnumerable().Where(x => Convert.ToString(x[DatabaseObjects.Columns.BusinessId]) == filter).ToArray();
                        if (driniColl != null && driniColl.Length > 0)
                        {
                            foreach (DataRow cRow in driniColl)
                            {
                                DataRow rRow = dtclone.NewRow();
                                dtclone.Rows.Add(rRow);
                                rRow[DatabaseObjects.Columns.BusinessId] = cRow[DatabaseObjects.Columns.BusinessId];
                                rRow[DatabaseObjects.Columns.InitiativeId] = cRow[DatabaseObjects.Columns.Id];
                                rRow[DatabaseObjects.Columns.InitiativeDescription] = cRow[DatabaseObjects.Columns.Title];
                                rRow[DatabaseObjects.Columns.BusinessStrategyDescription] = cRow[DatabaseObjects.Columns.BusinessStrategyDescription];
                                rRow[DatabaseObjects.Columns.ProjectInitiativeLookup] = cRow[DatabaseObjects.Columns.Title];
                            }
                            initiative = dtclone.AsEnumerable().Select(x => x).ToArray();
                        }
                    }
                }
                else
                {
                    initiative = resultentTable.AsEnumerable().ToArray();
                }

                if (!crdInitiative.JSProperties.ContainsKey("cpCommonhas"))
                    crdInitiative.JSProperties.Add("cpCommonhas", false);
                else
                    crdInitiative.JSProperties["cpCommonhas"] = false;

                if (initiative.Length > 0)
                {
                    groupCount = initiative.GroupBy(x => x[DatabaseObjects.Columns.InitiativeId]);
                    initiativeCount = groupCount.Count();
                    if (string.IsNullOrEmpty(filter))
                        filter = "<Unassigned>";

                    dataSource = obj.GenerateData(dataSource, initiative.CopyToDataTable(), flipview, filter);
                }
                else
                {
                    crdInitiative.SettingsText.EmptyCard = "No Initiatives";
                    if (crdInitiative.JSProperties.ContainsKey("cpCommonhas"))
                        crdInitiative.JSProperties["cpCommonhas"] = true;

                }

                if (dataSource != null)
                {
                    DataRow[] drSource = dataSource.Select(string.Format("{0} <> ''", DatabaseObjects.Columns.Title));

                    if (drSource != null && drSource.Length > 0)
                        dataSource = drSource.CopyToDataTable();
                }

                crdInitiative.DataSource = dataSource;
                crdInitiative.DataBind();
            }
        }

        protected void innerProjectData_CustomCallback(object sender, ASPxCardViewCustomCallbackEventArgs e)
        {
            ASPxCardView view = sender as ASPxCardView;
            dataSource.Clear();
            //Exclude IsPrivate marked for spcific user
            //uHelper.ExcludeIsPrivateMarked(_Context, resultentTable, User);

            if (resultentTable != null && resultentTable.Rows.Count > 0)
            {
                string filter = string.Empty;
                if (e != null && !string.IsNullOrEmpty(e.Parameters))
                {
                    filter = e.Parameters;
                }
                else if (!string.IsNullOrEmpty(hdnInitiative.Value) || !string.IsNullOrEmpty(hdnlabeltext.Value))
                {
                    filter = UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8).Length == 2 ? UGITUtility.SplitString(Convert.ToString(hdnInitiative.Value), Constants.Separator8)[1] : hdnInitiative.Value;
                    if (string.IsNullOrWhiteSpace(filter))
                        filter = UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8).Length == 2 ? UGITUtility.SplitString(Convert.ToString(hdnlabeltext.Value), Constants.Separator8)[1] : hdnlabeltext.Value;

                    if (filter.ToLower() == "unassigned")
                        filter = string.Empty;
                }
                filter = filter.Trim();
                DataRow[] projectChilds = resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && Convert.ToString(x[DatabaseObjects.Columns.InitiativeId]) == filter).ToArray();//resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.TicketId) && Convert.ToString(x[DatabaseObjects.Columns.ProjectInitiativeLookup]) == filter).ToArray();
                if (projectChilds.Length > 0)
                {
                    foreach (DataRow dr in projectChilds)
                    {
                        DataRow[] rows = new DataRow[1] { dr };
                        string title = string.Format("<a class='cssprojectlink'  href='javascript:openrespectiveproject(\"{0}\",\"{1}\")'><b>{1}</b></a>", Convert.ToString(dr[DatabaseObjects.Columns.TicketId]), Convert.ToString(dr[DatabaseObjects.Columns.Title]));

                        dataSource = obj.GenerateData(dataSource, rows.CopyToDataTable(), false, title);
                    }
                }

                if (dataSource.Rows.Count == 0)
                    innerProjectData.Visible = false;
                else
                    innerProjectData.Visible = true;

                if (dataSource != null)
                {
                    DataRow[] drSource = dataSource.Select(string.Format("{0} <> ''", DatabaseObjects.Columns.Title));

                    if (drSource != null && drSource.Length > 0)
                        dataSource = drSource.CopyToDataTable();
                }

                innerProjectData.DataSource = dataSource;
                innerProjectData.DataBind();

                if (view != null)
                    view.PageIndex = 0;
            }
        }

        protected void crdBusinessStrategy_CustomCallback(object sender, ASPxCardViewCustomCallbackEventArgs e)
        {
            LoadGroupData();
        }

        protected void pnltoprow_Load(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            HtmlGenericControl txtinitiativetitle = pnl.FindControl("spnInitiative") as HtmlGenericControl;

            if (txtinitiativetitle != null)
                txtinitiativetitle.InnerText = iniTitle;
        }

        protected void FilterListView(object sender, EventArgs e)
        {
            List<string> filterIdPost = new List<string>();
            if (hdnkeepprojectstate.Contains("FilterCheckBox_cp"))
                FilterCheckBox_cp.Checked = UGITUtility.StringToBoolean(hdnkeepprojectstate.Get("FilterCheckBox_cp"));

            if (hdnkeepprojectstate.Contains("FilterCheckBox_apr"))
                FilterCheckBox_apr.Checked = UGITUtility.StringToBoolean(hdnkeepprojectstate.Get("FilterCheckBox_apr"));

            if (hdnkeepprojectstate.Contains("FilterCheckBox_pa"))
                FilterCheckBox_pa.Checked = UGITUtility.StringToBoolean(hdnkeepprojectstate.Get("FilterCheckBox_pa"));

            if (FilterCheckBox_cp.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.CurrentProjects.ToString());
            }
            if (FilterCheckBox_apr.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.ApprovedNPRs.ToString());
            }
            if (FilterCheckBox_pa.Checked)
            {
                filterIdPost.Add(Constants.ProjectType.PendingApprovalNPRs.ToString());
            }

            resultentTable = obj.FilterById(filterIdPost);
            //Exclude IsPrivate marked projects
            //resultentTable = uHelper.ExcludeIsPrivateMarked(_Context, resultentTable, _Context.CurrentUser);
            hdnkeepFileters.Value = string.Join(",", filterIdPost);
        }
        protected void btnSaveBS_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBSTitle.Text))
                return;

            BusinessStrategy item = new BusinessStrategy();
            item.Title = txtBSTitle.Text.Trim();
            item.Description = txtBSDescription.Text.Trim();
            BusinessStrategyMGR.Insert(item);
            dtBusinessStrategy = obj.GetAllBusinessStrategy();
            BindBSDropDown();

            addnewstrategy.ShowOnPageLoad = false;
            LoadGroupData();

        }

        #endregion

        protected void crdBusinessStrategyChilds_CardUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string key = Convert.ToString(e.Keys[0]);
            if (!string.IsNullOrWhiteSpace(key))
            {
                //BusinessStrategy item = spBSList.FirstOrDefault(x=>x.ID == (Convert.ToInt32(key)));
                BusinessStrategy item = BusinessStrategyMGR.LoadByID(Convert.ToInt64(key));
                if (item == null)
                    return;
                Panel pnl = crdBusinessStrategyChilds.FindEditFormLayoutItemTemplateControl("pnlStartegy") as Panel;
                if (pnl != null)
                {
                    ASPxMemo memo = pnl.FindControl("aspxBsDescription") as ASPxMemo;
                    if (memo != null)
                        item.Description = memo.Text;
                }

                pnl = crdBusinessStrategyChilds.FindEditFormLayoutItemTemplateControl("pnlStartegyTitle") as Panel;
                if (pnl != null)
                {
                    ASPxTextBox memo = pnl.FindControl("txtbstitle") as ASPxTextBox;
                    if (memo != null && !string.IsNullOrEmpty(memo.Text))
                    {
                        item.Title = memo.Text;
                    }
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                BusinessStrategyMGR.Update(item);
                e.Cancel = true;
                crdBusinessStrategyChilds.CancelEdit();
            }
        }

        protected void btAddBusinessStrategy_Click(object sender, EventArgs e)
        {
            addnewstrategy.ShowOnPageLoad = true;
        }

        protected void crdBusinessStrategyChilds_CardLayoutCreated(object sender, ASPxCardViewCardLayoutCreatedEventArgs e)
        {

        }

        protected void pnlStartegy_Load(object sender, EventArgs e)
        {
            if (crdBusinessStrategyChilds.EditingCardVisibleIndex == -1)
                return;

            Panel pnl = sender as Panel;
            var key = crdBusinessStrategyChilds.GetCardValues(crdBusinessStrategyChilds.EditingCardVisibleIndex, DatabaseObjects.Columns.BusinessId);

            if (resultentTable != null)
            {
                //int id = 0;
                //int.TryParse(Convert.ToString(key), out id);

                DataRow row = dtBusinessStrategy.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ID) == Convert.ToString(key)).FirstOrDefault();//resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.BusinessId) && x.Field<string>(DatabaseObjects.Columns.BusinessId) == Convert.ToString(key)).FirstOrDefault();//resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.BusinessStrategy) && x.Field<string>(DatabaseObjects.Columns.BusinessStrategy) == Convert.ToString(key)).FirstOrDefault();
                if (row != null)
                {
                    string description = Convert.ToString(row[DatabaseObjects.Columns.TicketDescription]);
                    ASPxMemo memo = pnl.FindControl("aspxBsDescription") as ASPxMemo;
                    if (memo != null)
                        memo.Text = description;
                }
            }
        }

        protected void pnlinitiative_Load1(object sender, EventArgs e)
        {
            if (crdInitiativeChilds.EditingCardVisibleIndex == -1)
                return;

            Panel pnl = sender as Panel;
            var key = crdInitiativeChilds.GetCardValues(crdInitiativeChilds.EditingCardVisibleIndex, DatabaseObjects.Columns.InitiativeId);

            if (resultentTable != null)
            {
                int keyid = 0;
                int.TryParse(Convert.ToString(key), out keyid);
                DataRow row = dtInitiative.AsEnumerable().Where(x => Convert.ToInt32(x[DatabaseObjects.Columns.Id]) == keyid).FirstOrDefault();
                if (row != null)
                {
                    string description = Convert.ToString(row[DatabaseObjects.Columns.InitiativeDescription]);
                    ASPxMemo memo = pnl.FindControl("aspxinitiativeDescription") as ASPxMemo;
                    if (memo != null)
                        memo.Text = description;
                }
            }
        }

        protected void crdInitiativeChilds_CardUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            string key = Convert.ToString(e.Keys[0]);
            if (!string.IsNullOrWhiteSpace(key))
            {
                ProjectInitiative item = ProjectInitiativeMGR.LoadByID(Convert.ToInt32(key));
                if (item == null)
                    return;
                Panel pnl = crdInitiativeChilds.FindEditFormLayoutItemTemplateControl("pnlinitiative") as Panel;
                if (pnl != null)
                {
                    ASPxMemo memo = pnl.FindControl("aspxinitiativeDescription") as ASPxMemo;
                    if (memo != null)
                        item.ProjectNote = memo.Text;
                }

                pnl = crdInitiativeChilds.FindEditFormLayoutItemTemplateControl("pnlinitiativetitle") as Panel;

                if (pnl != null)
                {
                    ASPxTextBox memo = pnl.FindControl("aspxinitiativetitle") as ASPxTextBox;
                    if (memo != null && !string.IsNullOrEmpty(memo.Text))
                        item.Title = memo.Text;
                    else
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                ProjectInitiativeMGR.Update(item);
                e.Cancel = true;
                crdInitiativeChilds.CancelEdit();
            }
        }

        protected void pnlStartegyTitle_Load(object sender, EventArgs e)
        {
            if (crdBusinessStrategyChilds.EditingCardVisibleIndex == -1)
                return;

            Panel pnl = sender as Panel;
            var key = crdBusinessStrategyChilds.GetCardValues(crdBusinessStrategyChilds.EditingCardVisibleIndex, DatabaseObjects.Columns.BusinessId);

            if (resultentTable != null)
            {
                DataRow row = dtBusinessStrategy.AsEnumerable().Where(x => x.Field<string>(DatabaseObjects.Columns.ID) == Convert.ToString(key)).FirstOrDefault();
                if (row != null)
                {
                    string description = Convert.ToString(row[DatabaseObjects.Columns.Title]);
                    ASPxTextBox memo = pnl.FindControl("txtbstitle") as ASPxTextBox;
                    if (memo != null)
                        memo.Text = description;
                }
            }
        }

        protected void pnlinitiativetitle_Load(object sender, EventArgs e)
        {
            if (crdInitiativeChilds.EditingCardVisibleIndex == -1)
                return;

            Panel pnl = sender as Panel;
            var key = crdInitiativeChilds.GetCardValues(crdInitiativeChilds.EditingCardVisibleIndex, DatabaseObjects.Columns.InitiativeId);

            if (resultentTable != null)
            {
                int keyid = 0;
                int.TryParse(Convert.ToString(key), out keyid);
                DataRow row = dtInitiative.AsEnumerable().Where(x => Convert.ToInt32(x[DatabaseObjects.Columns.Id]) == keyid).FirstOrDefault();//resultentTable.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.ProjectInitiativeLookup) && x.Field<string>(DatabaseObjects.Columns.ProjectInitiativeLookup) == Convert.ToString(key)).FirstOrDefault();
                if (row != null)
                {
                    string description = Convert.ToString(row[DatabaseObjects.Columns.Title]);
                    ASPxTextBox memo = pnl.FindControl("aspxinitiativetitle") as ASPxTextBox;
                    if (memo != null)
                        memo.Text = description;
                }
            }
        }

        protected void crdBusinessStrategyChilds_AfterPerformCallback(object sender, ASPxCardViewAfterPerformCallbackEventArgs e)
        {
            if (e.CallbackName == "UPDATEEDIT")
            {
                crdBusinessStrategyChilds.JSProperties.Add("cpPerformCallback", true);
            }
            else if (e.CallbackName == "DELETEROW")
            {
                crdBusinessStrategyChilds.JSProperties.Add("cpPerformDeleteCallback", true);
            }
        }

        protected void crdInitiativeChilds_AfterPerformCallback(object sender, ASPxCardViewAfterPerformCallbackEventArgs e)
        {
            if (e.CallbackName == "UPDATEEDIT")
            {
                crdInitiativeChilds.JSProperties.Add("cpPerformCallback", true);
            }
            else if (e.CallbackName == "DELETEROW")
            {
                crdInitiativeChilds.JSProperties.Add("cpPerformDeleteCallback", true);
            }
            else if (e.CallbackName == "CANCELEDIT")
            {
                crdInitiativeChilds.JSProperties.Add("cpPerformCancelEditCallback", true);
            }
            else if (e.CallbackName == "STARTEDIT")
            {
                crdInitiativeChilds.JSProperties.Add("cpPerformStartEditCallback", true);
            }
        }

        protected void crdBusinessStrategyChilds_CardDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int id = 0;
            int.TryParse(Convert.ToString(e.Keys[0]), out id);

            if (id > 0)
            {
                BusinessStrategy item = BusinessStrategyMGR.LoadByID(id);  // SPListHelper.GetSPListItem(list, id);
                BusinessStrategyMGR.Delete(item);
                e.Cancel = true;
                crdBusinessStrategyChilds.CancelEdit();
            }

        }

        protected void crdInitiativeChilds_CardDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int id = 0;
            int.TryParse(Convert.ToString(e.Keys[0]), out id);

            if (id > 0)
            {
                Manager.Managers.TicketManager ticketManager = new Manager.Managers.TicketManager(HttpContext.Current.GetManagerContext());
                ModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                UGITModule pmmMoudle = moduleManager.LoadByName(ModuleNames.PMM);

                ProjectInitiative item = ProjectInitiativeMGR.LoadByID(id);
                DataRow[] pmTickets = ticketManager.GetAllTickets(pmmModule).Select(string.Format("{0} = {1}", DatabaseObjects.Columns.ProjectInitiativeLookup, id));
                foreach (DataRow row in pmTickets)
                {
                    ticketManager.Delete(pmmModule, row);
                }
                ProjectInitiativeMGR.Delete(item);
                e.Cancel = true;
                crdInitiativeChilds.CancelEdit();
            }
        }

        protected void BindBSDropDown()
        {
            if (spBSList == null)
                return;

            DataTable dtBS = BusinessStrategyMGR.GetDataTable();
            if (dtBS != null && dtBS.Rows.Count > 0)
            {
                foreach (DataRow row in dtBS.Rows)
                {
                    ddlBusinessStrategy.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.Title]).Trim(), Convert.ToString(row[DatabaseObjects.Columns.ID])));
                }
            }

            if (ddlBusinessStrategy.Items.FindByText("None") == null)
                ddlBusinessStrategy.Items.Insert(0, new ListItem("None", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Int64 bsvalue = Convert.ToInt64(hdninitiativevalues.Get("bsvalue"));
            string title = Convert.ToString(hdninitiativevalues.Get("inititle"));
            string note = Convert.ToString(hdninitiativevalues.Get("projectnote"));
            bool chkdele = false;
            bool.TryParse(Convert.ToString(hdninitiativevalues.Get("chkdelete")), out chkdele);
            if (string.IsNullOrEmpty(title))
                return;
            _SPListItem.Title = title.Trim();
            _SPListItem.ProjectNote = note;
            _SPListItem.Deleted = chkdele;
            _SPListItem.BusinessStrategyLookup = bsvalue;

            ProjectInitiativeMGR.Insert(_SPListItem);
            dtInitiative = obj.GetIntiative();
            if (!aspxclientcallback.JSProperties.ContainsKey("cptitle"))
                aspxclientcallback.JSProperties.Add("cptitle", title);
            else
                aspxclientcallback.JSProperties["cptitle"] = title;
            addnewinitiativepopup.ShowOnPageLoad = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void lnkaddinitiative_Click(object sender, EventArgs e)
        {
            addnewinitiativepopup.ShowOnPageLoad = true;
        }

        protected void aspxclientcallback_Callback(object source, CallbackEventArgs e)
        {
            aspxclientcallback.JSProperties.Add("cpForInitiativeRefresh", true);
            btnSave_Click(null, null);
        }

        protected void crdBusinessStrategyChilds_CommandButtonInitialize(object sender, ASPxCardViewCommandButtonEventArgs e)
        {
            if (e.VisibleIndex == -1)
                return;

            switch (e.ButtonType)
            {
                case CardViewCommandButtonType.Edit:
                    e.Visible = EditButtonVisibleCriteria((ASPxCardView)sender, e.VisibleIndex);
                    break;
                case CardViewCommandButtonType.Delete:
                    e.Visible = DeleteButtonVisibleCriteria((ASPxCardView)sender, e.VisibleIndex, e);
                    break;
            }
        }

        protected bool EditButtonVisibleCriteria(ASPxCardView sender, int visibleindex)
        {
            object row = sender.GetDataRow(visibleindex);
            if (row == null) return true;

            string title = ((DataRow)row)[DatabaseObjects.Columns.Title].ToString();

            if (title.ToLower() == "<unassigned>" || title.ToLower() == "unassigned")
                return false;

            return true;
        }

        protected bool DeleteButtonVisibleCriteria(ASPxCardView sender, int visibleindex, ASPxCardViewCommandButtonEventArgs e)
        {
            object row = sender.GetDataRow(visibleindex);
            if (row == null) return true;

            string title = ((DataRow)row)[DatabaseObjects.Columns.Title].ToString();
            if (title.ToLower() == "<unassigned>" || title.ToLower() == "unassigned")
                return false;

            return true;

        }

        protected void crdInitiativeChilds_CommandButtonInitialize(object sender, ASPxCardViewCommandButtonEventArgs e)
        {
            if (!e.IsEditingCard)
            {
                ASPxCardView cardview = sender as ASPxCardView;
                if (e.VisibleIndex == -1)
                    return;

                switch (e.ButtonType)
                {
                    case CardViewCommandButtonType.Edit:
                        e.Visible = EditButtonVisibleCriteria((ASPxCardView)sender, e.VisibleIndex);
                        break;
                    case CardViewCommandButtonType.Delete:
                        e.Visible = DeleteButtonVisibleCriteria((ASPxCardView)sender, e.VisibleIndex, e);
                        break;

                }
            }

        }

        protected void addnewstrategy_WindowCallback(object source, PopupWindowCallbackArgs e)
        {
            if (string.IsNullOrEmpty(txtBSTitle.Text))
                return;

            BusinessStrategy item = new BusinessStrategy();
            item.Title = txtBSTitle.Text.Trim();
            item.Description = txtBSDescription.Text.Trim();

            BusinessStrategyMGR.Insert(item);

            dtBusinessStrategy = obj.GetAllBusinessStrategy();
            BindBSDropDown();
            if (!addnewstrategy.JSProperties.ContainsKey(""))
                addnewstrategy.JSProperties.Add("cpBsCreatedMessage", txtBSTitle.Text.Trim());
            else
                addnewstrategy.JSProperties["cpBsCreatedMessage"] = txtBSTitle.Text.Trim();

            addnewstrategy.ShowOnPageLoad = false;
            LoadGroupData();
        }
    }
}

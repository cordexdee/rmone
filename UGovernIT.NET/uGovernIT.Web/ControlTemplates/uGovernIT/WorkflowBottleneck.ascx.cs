using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using DevExpress.Web;

namespace uGovernIT.Web
{
    public partial class WorkflowBottleneck : UserControl
    {
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }

        public LifeCycle ModuleLifeCycle { get; set; }
        //Added by mudassir 5 feb 2020
        public LifeCycle ModuleLifeCycleFinal { get; set; }
        public string LifeCycleId;
        
        public string moduleNames;
        
       
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager moduleViewManager = null;
        TicketManager ticketManager = null;
        UGITModule moduleDetail = new UGITModule();
        LifeCycleManager objLifeCycleHelper = null;
        double totalModuleWeight;
        DataTable dtmoduleTicketList = new DataTable();
        const string grayColor = "gray";
        const string greenColor = "#4fca66";
        const string orangeColor = "#FFDD20";
        const string redColor = "#ff5d5d";
        public string Width { get; set; }
        public string Height { get; set; }
        public bool DisplayOnDashboard { get; set; }
        public bool IsDialog { get; set; }
        //Added 31 jan 2020
       
        private string absoluteUrlStage = "/layouts/ugovernit/DelegateControl.aspx?control={0}&StageStep={1}&ModuleName={2}&StageName={3}&LifeCycle={4}";
        //
        //Added by mudassir 29 jan 2020
        List<LifeCycle> lifeCycles;
        //
        //Added by mudassir 5 feb 2020
       
        //string cmbLifeCycleItems = "";
        //
        protected override void OnInit(EventArgs e)
        {
           
            IsDialog = Convert.ToString(Request.QueryString["isudlg"]) == "1" ? true : false;
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            moduleViewManager = new ModuleViewManager(context);
            ticketManager = new TicketManager(context);
            objLifeCycleHelper = new LifeCycleManager(context);

            moduleDetail = moduleViewManager.LoadByName(ModuleName,true); 
            ModuleTitle = Convert.ToString(moduleDetail.Title);

            if (DisplayOnDashboard)
            {
               
                dvContainer.Style.Add("width", Width);
                dvContainer.Style.Add("height", Height);
                dvContainer.Style.Add("overflow", "auto");
            }

            BindLifeCycle();
            if (!IsPostBack && ModuleName == ModuleNames.PMM)
            {
                if (cmbLifeCycle.Items.Count >= 1)
                    cmbLifeCycle.Items[0].Selected = true;
            }
            BindData(ModuleName);
            base.OnLoad(e);
        }
        

        private void BindData(string moduleName)
        {

            #region migrate code from SP to .Net
            dtmoduleTicketList = ticketManager.GetAllTickets(moduleDetail);
            int numTickets = 0;
            if (dtmoduleTicketList != null && dtmoduleTicketList.Rows.Count > 0)
            {
                if (ModuleName == ModuleNames.PMM && dtmoduleTicketList.Columns.Contains(DatabaseObjects.Columns.ProjectLifeCycleLookup))
                {
                    DataRow[] dtRowColl = dtmoduleTicketList.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.ProjectLifeCycleLookup) && x.Field<long>(DatabaseObjects.Columns.ProjectLifeCycleLookup) ==UGITUtility.StringToLong(cmbLifeCycle.SelectedItem.Value)).ToArray();
                    if (dtRowColl != null && dtRowColl.Length > 0)
                        dtmoduleTicketList = dtRowColl.CopyToDataTable();
                    else
                        dtmoduleTicketList = dtmoduleTicketList.Clone();
                }

                numTickets = dtmoduleTicketList.Rows.Count;
            }

            //Call to set value out side callbackpanel
            hdnkeepcount.Set("ticketCountTitle", string.Format("{0}: {1} total {2}s", ModuleTitle, numTickets, UGITUtility.moduleTypeName(moduleName).ToLower()));
            LifeCycle selLifeCycle = null;
            if (moduleName != ModuleNames.PMM)
                selLifeCycle = lifeCycles.Where(m => m.ID == 0).FirstOrDefault();
            else
                selLifeCycle = lifeCycles.Where(m => m.ID == UGITUtility.StringToInt(cmbLifeCycle.Value)).FirstOrDefault();

            //rptSummary.DataSource = null;
            //rptSummary.DataBind();

            if (selLifeCycle != null && selLifeCycle.Stages.Count > 0)
            {
                ModuleLifeCycle = selLifeCycle;

                totalModuleWeight = ModuleLifeCycle.Stages.Sum(x => x.StageWeight);
                if (totalModuleWeight <= 0)
                    totalModuleWeight = ModuleLifeCycle.Stages.Count();

                ParentRepeater.DataSource = new List<LifeCycle>() { selLifeCycle };
                ParentRepeater.DataBind();

                rptSummary.DataSource = ModuleLifeCycle.Stages.Where(w => Convert.ToString(w.StageCapacityNormal) != null
                                                                       && Convert.ToString(w.StageCapacityMax) != null
                                                                       && w.StageCapacityNormal > 0
                                                                       && w.StageCapacityMax > 0).ToList();
                rptSummary.DataBind();

                
            }

            if (rptSummary.DataSource == null || rptSummary.Items.Count == 0)
            {
                summaryContainer.Visible = false;
            }

            #endregion


           
        }
        protected void Parentrepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int numberTickets = 0;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LifeCycleId = ((uGovernIT.Utility.LifeCycle)e.Item.DataItem).ID.ToString();
                Repeater StageRptr = (Repeater)e.Item.FindControl("stageRepeater");
                StageRptr.DataSource = lifeCycles.Where(m => m.ID == ((uGovernIT.Utility.LifeCycle)e.Item.DataItem).ID).FirstOrDefault().Stages;
                StageRptr.DataBind();
                
                dtmoduleTicketList = ticketManager.GetAllTickets(moduleDetail);
                if (dtmoduleTicketList != null && dtmoduleTicketList.Rows.Count > 0)
                {
                    if (ModuleName == ModuleNames.PMM && dtmoduleTicketList.Columns.Contains(DatabaseObjects.Columns.ProjectLifeCycleLookup))
                    {
                        DataRow[] dtRowColl = dtmoduleTicketList.AsEnumerable().Where(x => !x.IsNull(DatabaseObjects.Columns.ProjectLifeCycleLookup) && x.Field<Int64>(DatabaseObjects.Columns.ProjectLifeCycleLookup).ToString() == LifeCycleId && x.Field<Int32>(DatabaseObjects.Columns.StageStep) != 0).ToArray();

                        if (dtRowColl != null && dtRowColl.Length > 0)
                            dtmoduleTicketList = dtRowColl.CopyToDataTable();
                        else
                            dtmoduleTicketList = dtmoduleTicketList.Clone();
                    }


                }
               
                numberTickets = dtmoduleTicketList.Rows.Count;

                ASPxLabel lblTotalText = e.Item.FindControl("lblTotalText") as ASPxLabel;
                if(lblTotalText != null)
                    lblTotalText.Text = string.Format("{0} total {1}s", numberTickets, UGITUtility.moduleTypeName(ModuleName).ToLower());
                //lblTotalText.Text = string.Format("{0}: {1} total {2}s", ModuleTitle, numberTickets, UGITUtility.moduleTypeName(ModuleName).ToLower());
            }



        }

        protected void StageRptr_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            Repeater StageRptr = (Repeater)e.Item.FindControl("stageRepeater");
            LinkButton btShowSummary = (LinkButton)e.Item.FindControl("btShowSummary");

            //
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                LifeCycleStage stage = (LifeCycleStage)e.Item.DataItem;
                Repeater repeator = (Repeater)sender;
                List<LifeCycleStage> allStages = (List<LifeCycleStage>)repeator.DataSource;
                int stageIdex = allStages.IndexOf(stage);


                HtmlTableCell tdStage = (HtmlTableCell)e.Item.FindControl("tdStage");
                Literal lbStageNumber = (Literal)e.Item.FindControl("lbStageNumber");
                HtmlGenericControl stageTitleContainer = (HtmlGenericControl)e.Item.FindControl("stageTitleContainer");
                Literal stageTitle = (Literal)e.Item.FindControl("stageTitle");
                Literal lbStageCount = (Literal)e.Item.FindControl("lbStageCount");
                HtmlGenericControl activeStageArrow = (HtmlGenericControl)e.Item.FindControl("activeStageArrow");
                HtmlTableCell tdStepLine = (HtmlTableCell)e.Item.FindControl("tdStepLine");
                HtmlGenericControl spdigit = (HtmlGenericControl)e.Item.FindControl("spdigit");
                HtmlGenericControl divOuterCircle = (HtmlGenericControl)e.Item.FindControl("divOuterCircle");

                //tdStage.Attributes.Add("title", stage.Name);
                tdStage.Attributes.Add("aStage", stage.ApprovedStage != null ? stage.ApprovedStage.StageStep.ToString() : "0");
                tdStage.Attributes.Add("rStage", stage.ReturnStage != null ? stage.ReturnStage.StageStep.ToString() : "0");
                tdStage.Attributes.Add("rjStage", stage.RejectStage != null ? stage.RejectStage.StageStep.ToString() : "0");

                lbStageNumber.Text = Convert.ToString(stage.StageStep);




                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlStage, "stagetickets", lbStageNumber.Text, ModuleName, stage.Name, 0));
                //Get data as per lifecycle filter too
                if (ModuleName == ModuleNames.PMM)
                    //Added by mudassir 5 feb 2020
                    editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlStage, "stagetickets", lbStageNumber.Text, ModuleName, stage.Name, LifeCycleId));
                //
                string jsFunc = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}','90','90',0,'{2}','')", editItem, stage.Name, Server.UrlEncode(Request.Url.AbsolutePath));
                divOuterCircle.Attributes.Add("onclick", jsFunc);

                int cnt;
                if (dtmoduleTicketList != null && dtmoduleTicketList.Rows.Count > 0)
                {
                    string expression = string.Format("{0} = '{1}'", DatabaseObjects.Columns.StageStep, stage.StageStep);
                    cnt = dtmoduleTicketList.Select(expression).Count();
                }
                else
                    cnt = 0;

                if (cnt < 10)
                    spdigit.Attributes.Add("class", "pos_rel stageCount");
                else if (cnt < 100)
                    spdigit.Attributes.Add("class", "pos_rel stageCount");
                else if (cnt < 1000)
                    spdigit.Attributes.Add("class", "pos_rel stageCount");
                else if (cnt < 10000)
                    spdigit.Attributes.Add("class", "pos_rel  digit-4");

                lbStageCount.Text = Convert.ToString(cnt);

                if (Convert.ToString(stage.StageCapacityNormal) != null && Convert.ToString(stage.StageCapacityMax) != null &&
                    stage.StageCapacityNormal != 0 && stage.StageCapacityMax != 0)
                {
                    if (cnt < stage.StageCapacityNormal)
                    { divOuterCircle.Style.Add("background", greenColor); }
                    else if (cnt < stage.StageCapacityMax)
                    { divOuterCircle.Style.Add("background", orangeColor); }
                    else
                    { divOuterCircle.Style.Add("background", redColor); }
                }
                else
                { divOuterCircle.Style.Add("background", grayColor); }


                if (stage.StageStep % 2 != 0)
                {
                    stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "-28px");

                }
                else
                {
                    stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "40px");

                }



                stageTitle.Text = stage.Name;
                //Added by mudassir 29 jan 2020
                if (ModuleName == ModuleNames.PMM)
                    tdStepLine.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepline.gif");

                if (totalModuleWeight > 0)
                    tdStepLine.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", (((Convert.ToInt32(stage.StageWeight) / totalModuleWeight) * 25000))));
                else
                    tdStepLine.Style.Add(HtmlTextWriterStyle.Width, "100px");

                if (stageIdex == allStages.Count - 1)
                    tdStepLine.Visible = false;

                //start
                HtmlGenericControl divBar = (HtmlGenericControl)e.Item.FindControl("divBar");
                Literal lbLine = (Literal)e.Item.FindControl("lbLine");

                divBar.Visible = false;
                if (stage.StageCapacityNormal != 0 && stage.StageCapacityMax != 0)
                {
                    divBar.Visible = true;
                    decimal max = Convert.ToDecimal(cnt < stage.StageCapacityMax ? stage.StageCapacityMax : cnt) / 90;
                    string backGround = "";

                    if (cnt < stage.StageCapacityNormal) backGround = greenColor;
                    else if (cnt < stage.StageCapacityMax) backGround = orangeColor;
                    else backGround = redColor;

                    lbLine.Text = @"<div style='text-align:left;position: relative; left:" + (stage.StageCapacityNormal / max).ToString() + @"%; font-size:10px;'>
                                        Normal (" + stage.StageCapacityNormal.ToString() + @")</div>
                                    <div class='parallelogram'>
                                        <span style='background: " + backGround + " ;width:" + (cnt / max).ToString() + @"%;'></span>
                                    </div>
                                    <div style='text-align:left;position: relative;left:" + (stage.StageCapacityMax / max).ToString() +
                                    @"%; font-size:10px;'>Max (" + stage.StageCapacityMax.ToString() + @")</div>


                              <style> #" + divBar.ClientID + @" > .parallelogram:before {
                                    content: '';
                                    position: absolute;
                                    z-index: 10;
                                    top: 25.1px;
                                    bottom: 0;
                                    left: " + (stage.StageCapacityNormal / max).ToString() + @"%;
                                    border-left: 3.5px solid navy;
                                    color: navy;
                                    height: 17px;
                                }
                                #" + divBar.ClientID + @" > .parallelogram:after {
                                    content: '';
                                    position: absolute;
                                    z-index: 10;
                                    top: 25.1px;
                                    bottom: 0;
                                    left: " + (stage.StageCapacityMax / max).ToString() + @"%;
                                    border-left: 3.5px solid blue;
                                    color: blue;
                                    height: 17px;
                                }</style>";
                }
                //end
            }

        }
        //
        protected void rptSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LifeCycleStage stage = (LifeCycleStage)e.Item.DataItem;
                Repeater repeator = (Repeater)sender;
                List<LifeCycleStage> allStages = (List<LifeCycleStage>)repeator.DataSource;
                int stageIdex = allStages.IndexOf(stage);


                Literal lbLine = (Literal)e.Item.FindControl("lbLine");
                Literal lbStageCount = (Literal)e.Item.FindControl("lbStageCount");
                Literal lbStageTitle = (Literal)e.Item.FindControl("lbStageTitle");
                //HtmlGenericControl spBar = (HtmlGenericControl)e.Item.FindControl("spBar");
                HtmlGenericControl divBar = (HtmlGenericControl)e.Item.FindControl("divBar");
                //Added 11 feb 2020

                //

                int cnt;
                if (dtmoduleTicketList != null && dtmoduleTicketList.Rows.Count > 0)
                {
                   string expression = string.Format("{0} = '{1}'", DatabaseObjects.Columns.StageStep, stage.StageStep);
                    cnt = dtmoduleTicketList.Select(expression).Count();
                }
                else
                    cnt = 0;

                lbStageTitle.Text = stage.Name + " (" + cnt.ToString() + ")";
                lbStageCount.Text = Convert.ToString(stage.StageCapacityMax);
                if (stage.StageCapacityNormal != 0 && stage.StageCapacityMax != 0)
                {
                    decimal max = Convert.ToDecimal(cnt < stage.StageCapacityMax ? stage.StageCapacityMax : cnt) / 90;
                    string backGround = "";

                    if (cnt < stage.StageCapacityNormal) backGround = greenColor;
                    else if (cnt < stage.StageCapacityMax) backGround = orangeColor;
                    else backGround = redColor;

                    lbLine.Text = @"<div style='position: relative; left:" + (stage.StageCapacityNormal / max).ToString() + @"%; font-size:10px;'>
                                        Normal (" + stage.StageCapacityNormal.ToString() + @")</div>
                                    <div class='parallelogram'>
                                        <span style='background: " + backGround + " ;width:" + (cnt / max).ToString() + @"%;'></span>
                                    </div>
                                    <div style='position: relative;left:" + (stage.StageCapacityMax / max).ToString() +
                                    @"%; font-size:10px;'>Max (" + stage.StageCapacityMax.ToString() + @")</div>


                              <style> #" + divBar.ClientID + @" > .parallelogram:before {
                                    content: '';
                                    position: absolute;
                                    z-index: 10;
                                    top: 15.1px;
                                    bottom: 0;
                                    left: " + (stage.StageCapacityNormal / max).ToString() + @"%;
                                    border-left: 3.5px solid #d7d7d7;
                                    color: navy;
                                    height: 17px;
                                }
                                #" + divBar.ClientID + @" > .parallelogram:after {
                                    content: '';
                                    position: absolute;
                                    z-index: 10;
                                    top: 15.1px;
                                    bottom: 0;
                                    left: " + (stage.StageCapacityMax / max).ToString() + @"%;
                                    border-left: 3.5px solid #d7d7d7;
                                    color: blue;
                                    height: 17px;
                                }</style>";
                }
            }
        }

        private void BindLifeCycle()
        {
            cmbLifeCycle.Items.Clear();
            if (cmbLifeCycle.Items.Count == 0)
            {
                lifeCycles = objLifeCycleHelper.LoadLifeCycleByModule(ModuleName.Trim()).Where(x => !x.Deleted).ToList();
                if (lifeCycles != null || lifeCycles.Count > 0)
                {
                    foreach (LifeCycle lifeCycle in lifeCycles)
                    {
                        cmbLifeCycle.Items.Add(lifeCycle.Name, lifeCycle.ID);
                    }
                }
            }
        }
    }
}


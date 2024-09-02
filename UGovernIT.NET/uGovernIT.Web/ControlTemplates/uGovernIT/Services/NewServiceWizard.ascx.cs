using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Collections;
using uGovernIT.Manager;
using System.Web;
using System.Collections.Specialized;
using System.IO;
using DevExpress.Web;
using uGovernIT.Core;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Manager.Managers;
using uGovernIT.Web.Helpers;
using System.Xml;
using System.Configuration;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class NewServiceWizard : UserControl
    {
        private class MappedControlInfo
        {
            public string FieldName { get; set; }
            public Control Control { get; set; }
            public string CtrType { get; set; }
            public object Value { get; set; }
        }

        ServiceQuestionWorkFlow serviceQuestionWorkFlow = new ServiceQuestionWorkFlow();
        public int ServiceId { get; set; }
        public string Category { get; set; }
        protected Services service;
        bool newService;
        protected string editTaskFormUrl;
        protected int defaultTabNumber = 0;
        protected string editServiceQuestionUrl;
        protected string editServiceSectionUrl;
        protected string editservicetaskBranchUrl;
        protected string editservicequestionbranchUrl;
        protected int totalServiceSection;
        private bool createParentServiceRequest = true;
        protected int reorderActionColumn = 0;
        private int previousSTicketID = -1;
        private int previousSModuleID = 0;
        private DataTable fieldList = null;
        protected string newServiceCategoryUrl;
        private List<ServiceQuestion> serviceVariables;
        private List<MappedControlInfo> mappedInfo = new List<MappedControlInfo>();
        private const string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private const string absoluteUrlViewHelpCard = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private const string absoluteUrlView1 = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";

        private string newParam = "listpicker";
        private string formTitle = "Picker List";
        private string absPath = string.Empty;
        protected string newServiceURL;
        string selectedMappingTaskID;
        UserProfile User;
        //DataTable modules = null;
        ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        TicketManager ObjTicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        protected string editServiceRatingQuestionUrl;
        protected string sendSurveyURL;
        public string selectedModule;
        ApplicationContext context;
        List<LifeCycleStage> lstStages;

        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            User = HttpContext.Current.CurrentUser();
            //editTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl");
            editTaskFormUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=taskedit");
            editServiceQuestionUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=servicequestioneditor");
            editServiceSectionUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=servicesectioneditor");
            editservicetaskBranchUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=editservicetaskbranch");
            editservicequestionbranchUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=editservicequestionbranch");

            editServiceRatingQuestionUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=surveyratingquestioneditor");
            newServiceCategoryUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=servicecategoryeditor&categorytype=");
            newServiceCategoryUrl = string.Format("{0}{1}", newServiceCategoryUrl, Category);
            //CategoryDD_Load(categoryDD, new EventArgs());
            btVariables.Visible = false;

            absPath = UGITUtility.GetAbsoluteURL(DelegateControlsUrl.PickFromAsset);
            //made Icon Tr visible  to both service and agent
            trIcon.Visible = true;


            string urlHelpCard = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlViewHelpCard, newParam, "Help Card Picker", "HLP", string.Empty, "HelpCardList", txtHelpCard.ClientID)); //"TicketWiki"
            aAddHelpCard.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", urlHelpCard, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView1, newParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtWiki.ClientID)); //"TicketWiki"
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));


            int sID = GetServiceID();

            UserProfileManager userManager;
            userManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            userManager.IsUGITSuperAdmin(User);
            if (User != null && userManager.IsUGITSuperAdmin(User))
            {
                divIncludeInDefaultData.Visible = true;
            }
            if (sID > 0)
            {
                lbBtDeleteService.Visible = true;
                btDeleteService.ClientVisible = true;
                lnkFullDeleteService.Visible = true;
                service = serviceManager.LoadByServiceID(sID);
                if (service != null && service.Sections != null)
                {
                    sendSurveyURL = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=sendsurvey&sendsurvey=true&ServiceID=" + service.ID + "");
                    totalServiceSection = service.Sections.Count;
                }


                if (service.ID > 0 && service.Deleted)
                {
                    lbBtDeleteService.ClientSideEvents.Click = "function(){ unArchiveService(); }"; // .Attributes.Add("onclick", "return unArchiveService();");
                    lbBtDeleteService.Text = "Unarchive";
                    btDeleteService.Text = "Unarchive";
                }


                string mapVariableEditUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=serviceqmapvariableEditor");
                btVariables.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', 'svcConfigID={1}', '{2}', '80', '80', 0, '{3}');", mapVariableEditUrl, sID, "Map Variables", Uri.EscapeUriString(Request.Url.AbsolutePath)));
                btVariables.Visible = true;
                serviceQuestionWorkFlow = (ServiceQuestionWorkFlow)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceQuestionWorkFlow.ascx");
                serviceQuestionWorkFlow.service = service;
                lWorkFlow.Controls.Add(serviceQuestionWorkFlow);


            }

            //Disable button and tab when new service form is opened
            if (sID == 0)
            {
                tabMenu.Tabs[1].Enabled = false;
                tabMenu.Tabs[2].Enabled = false;
                tabMenu.Tabs[3].Enabled = false;
                tabMenu.Tabs[4].Enabled = false;

                btNext1stTab.Visible = false;
                btSaveLabel.Text = "Create";
                btSaveAndClose1stTab.Visible = false;

                String moduleName = ObjModuleViewManager.Load().Where(x => x.EnableModuleAgent == true && x.EnableModule == true).OrderBy(x => x.ModuleName).Select(x => x.ModuleName).FirstOrDefault();
                //if (moduleName != null)
                //    ddlModule.SetValues(Convert.ToString(uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), moduleName)));

                if (!string.IsNullOrEmpty(categoryDD.FilterExpression))
                    categoryDD.FilterExpression = "Deleted <> 1 and " + categoryDD.FilterExpression;
                else
                    categoryDD.FilterExpression = "Deleted <> 1";
                categoryDD.SetValues("0");
            }

            if (Category.ToLower() == Constants.ModuleAgent.ToLower())
            {
                tabMenu.Tabs[0].Text = "Agent";
                tabMenu.Tabs[1].Visible = false;
                tabMenu.Tabs[2].Visible = true;
                tabMenu.Tabs[3].Visible = false;
                tabMenu.Tabs[4].Visible = true;
                divMappingTickets.Visible = false;
                trCategory.Visible = false;
                trAuthorizedToRun.Visible = false;
                trOwner.Visible = false;
                trParentService.Visible = false;
                trHideSummary.Visible = true;
                trHideThankuScreen.Visible = true;
                trOrder.Visible = false;
                trAttachmentReqd.Visible = false;
                //trServiceHelp.Visible = false;             //Need to check, if required   
                trNavigationType.Visible = false;
                //trHelpCard.Visible = false;
                //trWiki.Visible = false;
                //trLink.Visible = false;
                //trFileUpload.Visible = false;
                trModule.Visible = true;
                trAgentStages.Visible = true;
                trOrder.Visible = true;
                trShowDefaultvalChkAgent.Visible = true;
                ddlModule.Paging = false;
                ddlModule.FilterExpression = "EnableModuleAgent='True'";
                FillModuleDropDown(ddlModule);
                txtServiceName.ValidationSettings.RequiredField.ErrorText = "Please enter agent name";
                btRunService.Text = "Test Agent";
                trShowStageTransitionButtons.Visible = true;
                lblServiceName.Text = "Agent Name";
                lblSeviceDescription.Text = "Agent Description";
                if (sID == 0 && !string.IsNullOrEmpty(ddlModule.GetValues()))
                {
                    string moduleString = uHelper.getModuleNameByModuleId(HttpContext.Current.GetManagerContext(), Convert.ToInt32(ddlModule.GetValues()));
                    BindAgentStages(moduleString);
                }
                trShowDefaultvalChkAgent.Visible = true;
                btSendSurvey.Visible = false;
                trSurveyType.Visible = false;
            }
            else if (Category.ToLower() == Constants.ModuleFeedback.ToLower())
            {
                tabMenu.Tabs[0].Text = "Survey";
                tabMenu.Tabs[1].Visible = true;
                tabMenu.Tabs[2].Visible = true;
                tabMenu.Tabs[3].Visible = false;
                tabMenu.Tabs[4].Visible = false;
                trServiceName.Visible = true;
                trServiceDescription.Visible = true;
                trCategory.Visible = false;
                trAuthorizedToRun.Visible = false;
                trOwner.Visible = false;
                trParentService.Visible = false;
                trHideSummary.Visible = false;
                trHideThankuScreen.Visible = false;
                trOrder.Visible = true;
                trAttachmentReqd.Visible = false;
                //txtCompletionMessage.Text = "Thank you for your valuable feedback! This helps us improve our quality of service.";
                //trServiceHelp.Visible = false;
                trNavigationType.Visible = false;
                //trHelpCard.Visible = false;
                //trWiki.Visible = false;
                //trLink.Visible = false;
                //trFileUpload.Visible = false;
                trModule.Visible = false;
                trAgentStages.Visible = false;
                txtServiceName.ValidationSettings.RequiredField.ErrorText = "Please enter Survey name";
                trShowStageTransitionButtons.Visible = false;
                lblServiceName.Text = "Survery Name";
                lblSeviceDescription.Text = "Survey Description";
                trSurveyType.Visible = true;
                //btSendSurvey.Visible = false;
                if (ddlModule.GetValues() != "Generic" && !string.IsNullOrEmpty(ddlModule.GetValues()) && ddlModule.GetValues() != "ALL")
                {
                    int moduleid = Convert.ToInt32(ddlModule.GetValues());
                    if (!string.IsNullOrEmpty(Convert.ToString(moduleid)))
                    {
                        DataRow dr = ObjModuleViewManager.GetDataTable().Select(string.Format("{0} = {1}", DatabaseObjects.Columns.ID, moduleid))[0]; // uGITCache.GetModuleDetails(moduleid);
                        selectedModule = string.Empty;
                        if (dr != null)
                        {
                            string modulesele = Convert.ToString(dr[DatabaseObjects.Columns.ModuleName]);
                            selectedModule = modulesele;
                            btSendSurvey.Visible = false;
                        }
                    }
                }
                else if (ddlModule.GetValues() == "Generic")
                {
                    selectedModule = "";
                    btSendSurvey.Visible = true;
                }
                else
                    btSendSurvey.Visible = false;

                btActivateService.Visible = false;
                btDeactivateService.Visible = false;
                btRunService.Visible = true;
                btRunService.Text = "Test Survey";
                btSendSurvey.Visible = true;
                btDeleteService.Visible = false;
                btCopyServiceLink.Visible = false;
                lWorkFlow.Visible = false;
                btNextTab.Visible = false;
            }
            else
            {
                trCategory.Visible = true;
                tabMenu.Tabs[0].Text = "Service";
                tabMenu.Tabs[1].Visible = false;
                tabMenu.Tabs[2].Visible = true;
                tabMenu.Tabs[3].Visible = true;
                tabMenu.Tabs[4].Visible = true;
                divMappingTickets.Visible = true;
                trAuthorizedToRun.Visible = true;
                trOwner.Visible = true;
                trParentService.Visible = true;
                trHideSummary.Visible = true;
                trHideThankuScreen.Visible = true;
                trOrder.Visible = true;
                trAttachmentReqd.Visible = true;
                //trServiceHelp.Visible = true;     //Need to check, if required   
                trNavigationType.Visible = true;
                //trHelpCard.Visible = true;
                //trWiki.Visible = true;
                //trLink.Visible = true;
                //trFileUpload.Visible = false;
                trModule.Visible = false;
                trAgentStages.Visible = false;
                btRunService.Text = "Run Service";
                txtServiceName.ValidationSettings.RequiredField.ErrorText = "Please enter service name";
                trShowStageTransitionButtons.Visible = false;
                btSendSurvey.Visible = false;
                lWorkFlow.Visible = true;
                trSurveyType.Visible = false;
            }


            if (!IsPostBack)
            {
                tabMenu.ActiveTab = tabMenu.Tabs[0];

                object cacheVal = Context.Cache.Get(string.Format("SVCConfigQuestion-{0}", User.Id));
                if (cacheVal != null)
                {
                    Context.Cache.Remove(string.Format("SVCConfigQuestion-{0}", User.Id));
                    tabMenu.ActiveTab = tabMenu.Tabs[2];
                }

                cacheVal = Context.Cache.Get(string.Format("SVCConfigTask-{0}", User.Id));
                if (cacheVal != null)
                {
                    Context.Cache.Remove(string.Format("SVCConfigTask-{0}", User.Id));
                    tabMenu.ActiveTab = tabMenu.Tabs[3];
                }

                cacheVal = Context.Cache.Get(string.Format("SVCConfigMapVariable-{0}", User.Id));
                if (cacheVal != null)
                {
                    Context.Cache.Remove(string.Format("SVCConfigMapVariable-{0}", User.Id));
                    tabMenu.ActiveTab = tabMenu.Tabs[4];
                }

                cacheVal = Context.Cache.Get(string.Format("SVCConfigMapRefresh-{0}", User.Id));
                if (cacheVal != null && !IsPostBack)
                {
                    selectedMappingTaskID = Convert.ToString(cacheVal);
                    Context.Cache.Remove(string.Format("SVCConfigMapRefresh-{0}", User.Id));
                    tabMenu.ActiveTab = tabMenu.Tabs[4];
                }

                cacheVal = Context.Cache.Get(string.Format("SVCConfigRatingQuestion-{0}", User.Id));
                if (cacheVal != null)
                {
                    Context.Cache.Remove(string.Format("SVCConfigRatingQuestion-{0}", User.Id));
                    tabMenu.ActiveTab = tabMenu.Tabs[1];
                    defaultTabNumber = 1;
                }

                defaultTabNumber = tabMenu.Tabs.IndexOfName(tabMenu.ActiveTab.Name);
            }
            else if (!string.IsNullOrWhiteSpace(Request[hfPreTab.UniqueID]))
            {
                int.TryParse(Request[hfPreTab.UniqueID], out defaultTabNumber);
                if (defaultTabNumber >= 0 && tabMenu.Tabs.Count > defaultTabNumber)
                {
                    tabMenu.ActiveTab = tabMenu.Tabs[defaultTabNumber];
                }
            }
            BindTargetTypeCategories();

            if (tabMenu.ActiveTab != null)
                tabMenuActiveTabChanged(tabMenu, new TabControlEventArgs(tabMenu.ActiveTab));


            FillModuleDropDown(ddlModule);

            string newTaskUrl = editTaskFormUrl + "&ticketId=" + hfServiceID.Value + "&projectID=" + hfServiceID.Value + "&taskID=0" + "&moduleName=SVCConfig" + "&taskType=";
            btNewTask.Attributes.Add("onclick", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','New Task','1000','800',0,'{1}','true')", newTaskUrl + "task", Server.UrlEncode(Request.Url.AbsolutePath)));
            btNewTicket.Attributes.Add("onclick", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','New Ticket','1000','600',0,'{1}','true')", newTaskUrl + "ticket", Server.UrlEncode(Request.Url.AbsolutePath)));
            base.OnInit(e);
        }
        private void FillModuleDropDown(LookUpValueBox dropDown)
        {
            dropDown.devexListBox.AutoPostBack = false;
            //dropDown.devexListBox.
            dropDown.devexListBox.ClientSideEvents.ValueChanged = "BindModuleStageGridLookup";
        }
        private void DevexListBox_ValueChanged(object sender, EventArgs e)
        {
            if (ddlModule.devexListBox.Value != null)
            {
                int selectedModuleID = Convert.ToInt32(ddlModule.devexListBox.Value);
                string moduleName = ObjModuleViewManager.LoadByID(selectedModuleID).ModuleName;
                BindAgentStages(moduleName);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btActivateService.Visible = false;
            btDeactivateService.Visible = false;
            btRunService.Visible = false;
            btCopyServiceLink.Visible = false;
            if (glModuleStages.DataSource == null)
            {
                if (lstStages != null)
                {
                    glModuleStages.DataSource = lstStages;
                    glModuleStages.DataBind();
                }
                else
                {
                    string moduleName;
                    if (!string.IsNullOrEmpty(ddlModule.GetValues()))
                    {
                        moduleName = ObjModuleViewManager.LoadByID(Convert.ToInt32(ddlModule.GetValues())).ModuleName;
                        BindAgentStages(moduleName);
                    }
                }
            }

            // string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtWiki.ClientID)); //"TicketWiki"
            //aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            if (service != null)
            {
                btRunService.Visible = true;
                btDeactivateService.Visible = false;
                btActivateService.Visible = true;
                if (service.IsActivated)
                {
                    btDeactivateService.Visible = true;
                    btActivateService.Visible = false;
                }
                newServiceURL = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=ServicesWizard&serviceID={0}", service.ID));

                //memoServiceLinkBox.Text = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration?control=ServicesWizard&serviceID={0}", service.ID));
                memoServiceLinkBox.Text = UGITUtility.GetAbsoluteURL(string.Format(UGITUtility.ToAbsoluteUrl(Constants.HomePagePath) + "?control=ServicesWizard&serviceID={0}", service.ID));

                if (Category.ToLower() == Constants.ModuleAgent.ToLower())
                    btRunService.ClientSideEvents.Click = "function(s, e){ window.parent.UgitOpenPopupDialog('" + newServiceURL + "', '' , 'Agent: " + uHelper.ReplaceInvalidCharsInURL(Convert.ToString(service.Title)) + "', 90, 90, 0,'" + Server.UrlEncode(Request.Url.AbsolutePath) + "') }";
                else
                {
                    btRunService.ClientSideEvents.Click = "function(s, e){ window.parent.UgitOpenPopupDialog('" + newServiceURL + "','' , 'Service:" + uHelper.ReplaceInvalidCharsInURL(Convert.ToString(service.Title)) + "', 90, 90, 0,'" + Server.UrlEncode(Request.Url.AbsolutePath) + "') }";
                    btCopyServiceLink.Visible = true;
                }
                if (!IsPostBack)
                {
                    if (service.ID > 0)
                    {
                        if (service.CategoryId == null)
                            categoryDD.SetValues("0");
                    }
                }
                //btRunService.ClientSideEvents.Click = "function(s, e){ window.parent.UgitOpenPopupDialog('" + newServiceURL + "', '', '"+ uHelper.ReplaceInvalidCharsInURL(Convert.ToString(service.Title)) + "', 90, 90, 0,'"+ Server.UrlEncode(Request.Url.AbsolutePath) + "') }";

            }
        }

        #region Global Events
        protected override void OnPreRender(EventArgs e)
        {
            int previousTab = defaultTabNumber;
            if (IsPostBack && hfCurrentTab.Value != "")
            {
                int.TryParse(hfCurrentTab.Value, out defaultTabNumber);
                if (defaultTabNumber >= 0 && tabMenu.Tabs.Count > defaultTabNumber)
                {
                    tabMenu.ActiveTab = tabMenu.Tabs[defaultTabNumber];
                }

            }

            if (tabMenu.ActiveTab != null && previousTab != tabMenu.ActiveTab.Index)
            {
                tabMenuActiveTabChanged(tabMenu, new TabControlEventArgs(tabMenu.ActiveTab));
            }

            hfPreTab.Value = hfCurrentTab.Value = defaultTabNumber.ToString();
            hfMapPreviousTaskID.Value = selectedMappingTaskID;



            foreach (MappedControlInfo m in mappedInfo)
            {
                if (m.CtrType == "Choice")
                {
                    //DropDown ddl = (UGITDropDown)m.Control;
                    //if (ddl != null)
                    //{
                    //    ddl.DropDown.SelectedIndex = ddl.DropDown.Items.IndexOf(ddl.DropDown.Items.FindByValue(Convert.ToString(m.Value)));
                    //}
                }
                //else if (m.CtrType == SPFieldType.Boolean)
                //{
                //    CheckBox chb = (CheckBox)m.Control.Controls[0].Controls[1];
                //    if (chb != null)
                //    {
                //        chb.Checked = uHelper.StringToBoolean(m.Value);
                //    }
                //}
                else if (m.CtrType == "Lookup")
                {
                    if (m.Control.Controls.Count > 0)
                    {
                        if (m.Control.Controls[0] is LookUpValueBox)
                        {
                            LookUpValueBox ctrObj = (LookUpValueBox)m.Control.Controls[0];
                            ctrObj.SetValues(Convert.ToString(m.Value));
                        }
                        else if (m.Control.Controls[0] is DepartmentDropdownList)
                        {
                            DepartmentDropdownList ctrObj = m.Control.Controls[0] as DepartmentDropdownList;
                            ctrObj.SetValue(Convert.ToString(m.Value));
                        }
                    }
                }

            }


            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {

        }
        protected void BtActivateService_Click(object sender, EventArgs e)
        {
            ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
            if (service != null)
            {
                service.IsActivated = true;
                serviceManager.Save(service);
                btActivateService.Visible = false;
                btDeactivateService.Visible = true;
            }
        }
        protected void BtDeactivateService_Click(object sender, EventArgs e)
        {
            ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
            if (service != null)
            {
                service.IsActivated = false;
                serviceManager.Save(service);

                btActivateService.Visible = true;
                btDeactivateService.Visible = false;
            }

        }
        #endregion

        #region Comman Methods
        private int GetServiceID()
        {
            int sID = ServiceId;
            if (sID <= 0)
            {
                int.TryParse(hfServiceID.Value, out sID);
                if (sID <= 0)
                {
                    int.TryParse(Request[hfServiceID.UniqueID.ToString()], out sID);
                }
                ServiceId = sID;
            }
            hfServiceID.Value = sID.ToString();
            return sID;
        }


        #endregion

        #region Tab 1 Servcie
        //protected void CategoryDD_Load(object sender, EventArgs e)
        //{
        //    if (categoryDD.Items.Count <= 0)
        //    {
        //        BindCategoriesDropDown();
        //    }
        //}

        protected void BtSaveServiceInitials_Click(object sender, EventArgs e)
        {
            ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
            string moduleName = string.Empty;
            if (!Page.IsValid || !ASPxEdit.ValidateEditorsInContainer(tabServiceDetails, "serviceIntitial"))
                return;
            //Page.Validate();
            //if (!Page.IsValid)
            //    return;
            lbTaskDisclaimer.Visible = false;
            int sID = GetServiceID();
            if (service == null && sID > 0)
            {
                service = serviceManager.LoadByServiceID(sID);
            }
            if (service == null)
            {
                service = new Services();
                newService = true;
            }
            if (Category.ToLower() == Constants.ModuleAgent.ToLower())
            {
                service.ServiceType = "ModuleAgent";
            }
            else if (Category.ToLower() == Constants.ModuleFeedback.ToLower())
            {
                service.ServiceType = "ModuleFeedback";
            }
            else
            {
                service.ServiceType = "Service";
            }
            string uploadFileURL = string.Empty;
            //if (fileupload.HasFile)
            //{
            //    uploadFileURL = string.Format("/Content/images/uploadedfiles/{0}", fileupload.FileName);
            //    string path = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), fileupload.FileName);
            //    fileupload.PostedFile.SaveAs(path);
            //    txtWiki.Text = uploadFileURL;
            //}

            // Need to check code, which part, from above & below , to remove.
            switch (ddlTargetType.SelectedValue)
            {
                case "File":
                    if (fileUploadControl.HasFile)
                    {
                        string AssetFolder = ConfigurationManager.AppSettings["AssetFolder"];
                        string finalPath = AssetFolder + "/" + service.TenantID;
                        string folderPath = Server.MapPath(finalPath);

                        if (!Directory.Exists(folderPath))
                        {
                            //If Directory (Folder) does not exists. Create it.
                            Directory.CreateDirectory(folderPath);
                        }

                        //Save the File to the Directory (Folder).
                        fileUploadControl.SaveAs(folderPath + "/" + Path.GetFileName(fileUploadControl.FileName));

                        service.NavigationUrl = finalPath + "/" + Path.GetFileName(fileUploadControl.FileName);
                        service.NavigationType = ddlTargetType.SelectedValue;
                        lblUploadedFile.Text = fileUploadControl.FileName;
                    }
                    else
                    {

                        service.NavigationUrl = "";
                        service.NavigationType = "";
                        lblUploadedFile.Text = fileUploadControl.FileName;
                    }
                    break;
                case "Link":
                    service.NavigationUrl = txtFileLink.Text.Trim();
                    service.NavigationType = ddlTargetType.SelectedValue;
                    break;
                case "Wiki":
                    {
                        if (txtWiki.Visible)
                        {
                            service.NavigationUrl = txtWiki.Text.Trim();
                        }
                        service.NavigationType = ddlTargetType.SelectedValue;
                    }
                    break;
                case "HelpCard":
                    {
                        if (txtHelpCard.Visible)
                        {
                            service.NavigationUrl = txtHelpCard.Text.Trim();
                        }
                        service.NavigationType = ddlTargetType.SelectedValue;
                    }
                    break;
                default:
                    break;
            }
            //service.NavigationUrl = txtWiki.Text.Trim();


            service.Title = txtServiceName.Text.Trim();
            service.ServiceDescription = txtServiceDescription.Text.Trim();
            service.LoadDefaultValue = chkbxShowDefaultvalAgent.Checked;
            long moduleId = 0;
            if (!string.IsNullOrWhiteSpace(ddlModule.GetValues()))
            {
                long.TryParse(ddlModule.GetValues(), out moduleId);
                if ((Category.ToLower() == Constants.ModuleFeedback.ToLower() && rdbSurveyType.SelectedIndex != 0) || (Category.ToLower() != Constants.ModuleFeedback.ToLower()))
                {
                    moduleName = ObjModuleViewManager.LoadByID(moduleId).ModuleName;
                }
                //SPDelta 155(Start:-Survey complete functionality)
                if (Category.ToLower() != Constants.ModuleFeedback.ToLower())
                {
                    moduleName = ObjModuleViewManager.LoadByID(moduleId).ModuleName;
                }
                //moduleName = ObjModuleViewManager.LoadByID(moduleId).ModuleName; //SPDelta 155(Commented:-Survey complete functionality)
                //SPDelta 155(End:-Survey complete functionality)
            }
            else
            {
                moduleName = "SVCConfig";
                if (Category.ToLower() == Constants.ModuleFeedback.ToLower())
                {
                    moduleName = "";
                }
            }
            // service.ModuleId = moduleId;
            service.ModuleNameLookup = moduleName;
            long categoryID = 1;

            if (Category.ToLower() == Constants.ModuleAgent.ToLower())
            {
                ServiceCategoryManager serviceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
                ServiceCategory serviceCategory = serviceCategoryManager.LoadAllCategories().FirstOrDefault(x => x.CategoryName.ToLower() == Constants.ModuleAgent.ToLower());
                if (serviceCategory == null)//if category of type service agent does not exist then create it first
                {
                    serviceCategory = new ServiceCategory();
                    serviceCategory.CategoryName = Constants.ModuleAgent;
                    serviceCategory.ItemOrder = -1;
                    serviceCategoryManager.Save(serviceCategory);
                }
                categoryID = serviceCategory.ID;
                List<string> moduleStages = new List<string>();
                List<object> selectedModuleStages = glModuleStages.GridView.GetSelectedFieldValues(DatabaseObjects.Columns.ID);
                LifeCycleManager objLifeCycleHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
                if (moduleName != "SVCConfig")
                {
                    LifeCycle obj = objLifeCycleHelper.LoadLifeCycleByModule(moduleName)[0];
                    foreach (object lifecycle in selectedModuleStages)
                    {
                        LifeCycleStage sstage = obj.Stages.FirstOrDefault(x => x.ID == Convert.ToInt32(lifecycle));
                        if (sstage != null)
                            moduleStages.Add(Convert.ToString(sstage.StageStep));

                    }

                    //foreach (object val in selectedModuleStages)
                    //{
                    //    moduleStages.Add(Convert.ToString(val));
                    //}
                    service.ModuleStage = string.Join(Constants.Separator, moduleStages);
                    service.ShowStageTransitionButtons = chkbxShowStageTranBtns.Checked;
                    chkTaskReminders.Checked = service.EnableTaskReminder;
                }
            }
            else
                long.TryParse(categoryDD.GetValues(), out categoryID);
            if (categoryDD.GetValues() == "" || categoryDD.GetValues() == "0")
            {
                service.CategoryId = null;
            }
            else
            {
                service.CategoryId = categoryID;
            }

            ServiceCategoryManager categoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
            ServiceCategory serviceCategory1 = categoryManager.LoadByID(Convert.ToInt64(service.CategoryId));
            if (serviceCategory1 != null)
                service.ServiceCategoryType = serviceCategory1.CategoryName; // categoryDD.GetValues();

            service.CreateParentServiceRequest = cbCreateParentServiceRequest.Checked;
            service.IncludeInDefaultData = chkIncludeInDefaultData.Checked;

            service.OwnerApprovalRequired = cbOwnerApprovalRequired.Checked;
            service.AllowServiceTasksInBackground = cbOwnerTasksInBackground.Checked;
            service.AttachmentsInChildTickets = chkbxAttachInChild.Checked;
            service.AttachmentRequired = ddlAttachmentRequired.SelectedValue;
            service.HideSummary = chkhidesummary.Checked;
            service.HideThankYouScreen = chkHideThankYouScreen.Checked;
            service.SLADisabled = chkDisableSLA.Checked;
            service.Use24x7Calendar = chkDisableSLA.Checked ? false : chkUse24x7.Checked;
            service.ResolutionSLA = GetDurationInMinutes(txtResolutionSLA.Text.Trim(), ddlResolutionSLAType.SelectedValue, service.Use24x7Calendar);
            service.StartResolutionSLAFromAssigned = cbStartResolutionSLAFromStart.Checked;

            List<string> userList = peServiceOwner.GetTextsAsList();
            service.OwnerUser = peServiceOwner.GetValues();
            //foreach (var userEntity in userList)
            //{
            //    int userID = 0;
            //    string userAcc = string.Empty;
            //    PickerEntity entity = (PickerEntity)userEntity;
            //    if (Convert.ToString(entity.EntityData["PrincipalType"]) == "User")
            //    {
            //        if (int.TryParse(entity.EntityData["SPUserID"].ToString(), out userID))
            //        {
            //            SPFieldUserValue userVal = new SPFieldUserValue();
            //            userVal.LookupId = userID;
            //            owners.Add(userVal);
            //        }
            //    }
            //    else
            //    {
            //        if (int.TryParse(Convert.ToString(entity.EntityData["SPGroupID"]), out userID))
            //        {
            //            SPFieldUserValue userVal = new SPFieldUserValue();
            //            userVal.LookupId = userID;
            //            owners.Add(userVal);
            //        }
            //    }
            //}

            ////Set current user as owner if not assigned
            //if (owners.Count <= 0)
            //{
            //    SPFieldUserValue userVal = new SPFieldUserValue();
            //    userVal.LookupId = User.Id;
            //    owners.Add(userVal);
            //}

            ////Authorized to view
            //SPFieldUserValueCollection authToView = new SPFieldUserValueCollection();
            //ArrayList authList = peAuthorizedToRun.ResolvedEntities;
            //foreach (var userEntity in authList)
            //{
            //    int userID = 0;
            //    string userAcc = string.Empty;
            //    PickerEntity entity = (PickerEntity)userEntity;
            //    if (Convert.ToString(entity.EntityData["PrincipalType"]) == "User")
            //    {
            //        if (int.TryParse(entity.EntityData["SPUserID"].ToString(), out userID))
            //        {
            //            SPFieldUserValue userVal = new SPFieldUserValue();
            //            userVal.LookupId = userID;
            //            authToView.Add(userVal);
            //        }
            //    }
            //    else
            //    {
            //        if (int.TryParse(Convert.ToString(entity.EntityData["SPGroupID"]), out userID))
            //        {
            //            SPFieldUserValue userVal = new SPFieldUserValue();
            //            userVal.LookupId = userID;
            //            authToView.Add(userVal);
            //        }
            //    }
            //}

            service.AuthorizedToView = peAuthorizedToRun.GetValues();
            //service.Owners = owners;
            int sOrder = 0;
            int.TryParse(txtOrder.Text.Trim(), out sOrder);
            service.ItemOrder = sOrder;
            service.CompletionMessage = txtCompletionMessage.Text.Trim();

            createParentServiceRequest = cbCreateParentServiceRequest.Checked;

            if (createParentServiceRequest)
            {
                //btNewTask.Visible = true;
                ListItem serviceRequestItem = ddlTaskTickets.Items.FindByValue("0");
                if (serviceRequestItem == null)
                {
                    ddlTaskTickets.ClearSelection();
                    ddlTaskTickets.Items.Insert(0, new ListItem("SVC: Service Instance", "0"));
                }

                hfMapPreviousTaskID.Value = string.Empty;
                BindQuestionMapping(hfMapPreviousTaskID.Value);
                //service.AttachmentsInChildTickets = chkbxAttachInChild.Checked;
            }
            else
            {
                dvTaskDisclaimer.Visible = true;
                lbTaskDisclaimer.Visible = true;
                //btNewTask.Visible = false;
                ListItem serviceRequestItem = ddlTaskTickets.Items.FindByValue("0");
                if (serviceRequestItem != null)
                    ddlTaskTickets.Items.Remove(serviceRequestItem);
            }

            service.ImageUrl = fileUploadIcon.GetImageUrl(); // txtServiceIcon.Text.Trim();
            service.EnableTaskReminder = chkTaskReminders.Checked;

            TaskReminderProperties reminderProperties = new TaskReminderProperties();
            if (chkTaskReminders.Checked)
            {
                if (chkReminder1.Checked)
                {
                    reminderProperties.Reminder1 = chkReminder1.Checked;
                    reminderProperties.Reminder1Duration = GetDurationInMinutes(txtReminder1Duration.Text.Trim(), ddlReminder1Unit.SelectedValue, service.Use24x7Calendar);
                    reminderProperties.Reminder1Frequency = ddlReminder1Frequency.SelectedValue;
                }
                else
                {
                    chkReminder1.Checked = false;
                    reminderProperties.Reminder1 = chkReminder1.Checked;
                    ddlReminder1Unit.SelectedValue = "Days";
                    txtReminder1Duration.Text = "0";
                    ddlReminder1Frequency.SelectedValue = "After";
                    reminderProperties.Reminder1Duration = GetDurationInMinutes(txtReminder1Duration.Text.Trim(), ddlReminder1Unit.SelectedValue, service.Use24x7Calendar);
                    reminderProperties.Reminder1Frequency = ddlReminder1Frequency.SelectedValue;
                }
                if (chkReminder2.Checked)
                {
                    reminderProperties.Reminder2 = chkReminder2.Checked;
                    reminderProperties.Reminder2Duration = GetDurationInMinutes(txtReminder2Duration.Text.Trim(), ddlReminder2Unit.SelectedValue, service.Use24x7Calendar);
                    reminderProperties.Reminder2Frequency = ddlReminder2Frequency.SelectedValue;
                }
                else
                {
                    chkReminder2.Checked = false;
                    reminderProperties.Reminder1 = chkReminder2.Checked;
                    ddlReminder2Unit.SelectedValue = "Days";
                    txtReminder2Duration.Text = "0";
                    ddlReminder2Frequency.SelectedValue = "After";
                    reminderProperties.Reminder1Duration = GetDurationInMinutes(txtReminder1Duration.Text.Trim(), ddlReminder2Unit.SelectedValue, service.Use24x7Calendar);
                    reminderProperties.Reminder1Frequency = ddlReminder2Frequency.SelectedValue;
                }
            }
            else
            {
                chkReminder1.Checked = false;
                ddlReminder1Unit.SelectedValue = "Days";
                txtReminder1Duration.Text = "0";
                ddlReminder1Frequency.SelectedValue = "After";
                chkReminder2.Checked = false;
                ddlReminder2Unit.SelectedValue = "Days";
                txtReminder2Duration.Text = "0";
                ddlReminder2Frequency.SelectedValue = "After";
                reminderProperties.Reminder1 = chkReminder1.Checked;
                reminderProperties.Reminder1Duration = GetDurationInMinutes(txtReminder1Duration.Text.Trim(), ddlReminder1Unit.SelectedValue, service.Use24x7Calendar);
                reminderProperties.Reminder1Frequency = ddlReminder1Frequency.SelectedValue;
                reminderProperties.Reminder2 = chkReminder2.Checked;
                reminderProperties.Reminder2Duration = GetDurationInMinutes(txtReminder2Duration.Text.Trim(), ddlReminder2Unit.SelectedValue, service.Use24x7Calendar);
                reminderProperties.Reminder2Frequency = ddlReminder2Frequency.SelectedValue;
            }
            XmlDocument doc = uHelper.SerializeObject(reminderProperties);
            service.Reminders = doc.OuterXml;

            service.SLAConfig = new SLAConfiguration();
            service.SLAConfig.EnableEscalation = chkEnableEscalation.Checked;
            service.SLAConfig.EscalationUnit = ddlEscaltionUnit.SelectedValue;
            string escalationUnitVal = service.SLAConfig.EscalationUnit;
            double escalationAfterduration = GetDurationInMinutes(txtEscalationAfter.Text.Trim(), ddlEscalationAfter.SelectedValue, service.Use24x7Calendar);
            service.SLAConfig.EscalationAfter = string.IsNullOrEmpty(escalationUnitVal) || escalationUnitVal == "0" ? escalationAfterduration : -escalationAfterduration;
            service.SLAConfig.EscalationFrequency = GetDurationInMinutes(txtEsclationFrequency.Text.Trim(), ddlEsclationFrequency.SelectedValue, service.Use24x7Calendar);
            service.SLAConfig.EscalationEmailTo = txtEmail.Text.ToString();
            service.SLAConfig.EscalationTo = pplEscalationTo.GetValues();


            //Owner we need to save
            serviceManager.Save(service);
            hfServiceID.Value = service.ID.ToString();

            if (!UGITUtility.StringToBoolean(hfSaveAndClose.Value))
            {
                if (newService)
                {
                    string serviceDetailUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=service&serviceid=");
                    StringBuilder qParams = new StringBuilder();
                    foreach (string key in Request.QueryString.Keys)
                    {
                        if (key != null && key.ToLower() != "serviceid" && key.ToLower() != "control")
                        {
                            qParams.AppendFormat("&{0}={1}", key, Request.QueryString[key]);
                        }
                    }
                    Response.Redirect(string.Format("{0}{1}{2}", serviceDetailUrl, service.ID, qParams.ToString()));
                }
            }
            else
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }


        }

        protected void BtDeleteService_Click(object sender, EventArgs e)
        {
            ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
            if (service != null)
            {
                if (service.Deleted)
                {
                    service.Deleted = false;
                    serviceManager.UnArchive(service.ID);

                }
                else
                {
                    serviceManager.Archive(service.ID);


                }
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        #endregion

        #region Tab 2 Rating
        private void BindRatingQuestionGrid()
        {
            List<ServiceQuestion> ratingQuestions = service.Questions.Where(x => x.QuestionType == "Rating").OrderBy(x => x.ID).ToList();
            rRatingQuestions.DataSource = ratingQuestions;
            rRatingQuestions.DataBind();

            object cacheVal = Context.Cache.Get(string.Format("SVCConfigRatingQuestion-{0}", User.Id));
            if (cacheVal != null)
            {
                Context.Cache.Remove(string.Format("SVCConfigRatingQuestion-{0}", User.Id));
                defaultTabNumber = 2;
            }

        }
        protected void RRatingQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                ServiceQuestion question = (ServiceQuestion)e.Item.DataItem;
                List<ServiceQuestion> questions = (List<ServiceQuestion>)((Repeater)sender).DataSource;

                Label lbRatingTitle = (Label)e.Item.FindControl("lbQuestionTitle");
                lbRatingTitle.Text = question.QuestionTitle;
                if (question.FieldMandatory && lbRatingTitle != null)
                {
                    lbRatingTitle.Text = string.Format("{0}<b style='color:red;'>*</b>", lbRatingTitle.Text);
                }

                //Stores question id in hidden variable
                HiddenField hfServiceQuestionID = (HiddenField)e.Item.FindControl("hfServiceQuestionID");
                hfServiceQuestionID.Value = question.ID.ToString();

                //Fills order dropdown list
                DropDownList ddlOrders = (DropDownList)e.Item.FindControl("ddlRatingQuestionOrderBy");
                ddlOrders.Attributes.Add("onChange", string.Format("reOrderRatingQuestion(this, {1}, {0}, {2})", questions.Count, e.Item.ItemIndex, question.ServiceSectionID));
                GenerateOrderDropDown(ddlOrders, questions.Count);
                ddlOrders.SelectedIndex = e.Item.ItemIndex;
                ddlOrders.CssClass = string.Format("ratingquestion-order-{0}", question.ServiceSectionID);
            }
        }
        protected void BtSaveRatingQuestionOrder_Click(object sender, EventArgs e)
        {
            //Iterate Repeator to save order of each question

            foreach (RepeaterItem rQuestionItem in rRatingQuestions.Items)
            {
                HiddenField hfServiceQuestionID = (HiddenField)rQuestionItem.FindControl("hfServiceQuestionID");
                int serviceQuestionID = 0;
                int.TryParse(hfServiceQuestionID.Value, out serviceQuestionID);
                DropDownList ddlQuestionOrders = (DropDownList)rQuestionItem.FindControl("ddlRatingQuestionOrderBy");
                ServiceQuestion question = service.Questions.FirstOrDefault(x => x.ID == serviceQuestionID);
                question.ItemOrder = Convert.ToInt32(ddlQuestionOrders.SelectedValue) - 9;
            }


            //Saves order changes in database
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
            serviceQuestionManager.SaveOrder(service.Questions);

            //Close popup when save and close button click
            if (UGITUtility.StringToBoolean(hfSaveAndClose.Value))
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }

            BindRatingQuestionGrid();
        }
        #endregion

        #region  Tab 2 Questions

        private void BindQuestionSectionGrid()
        {
            rSections.DataSource = service.Sections.Where(x => x.SectionName != null).OrderBy(x => x.ItemOrder).ToList();
            rSections.DataBind();

            List<ServiceQuestion> nonSectionQuestions = service.Questions.Where(x => x.ServiceSectionID == 0).ToList();

            if (nonSectionQuestions.Count > 0)
            {
                noUnCategorizedDataMsg.Visible = true;
                rNonSectionQuestions.DataSource = nonSectionQuestions;
                rNonSectionQuestions.DataBind();
            }
            else
            {
                noUnCategorizedDataMsg.Visible = false;
                lblunCategorized.Visible = false;
            }
            //rSections.DataSource = service.Sections.Where(x => x.SectionName != null).OrderBy(x => x.ItemOrder).ToList();
            //rSections.DataBind();

            //List<ServiceQuestion> nonSectionQuestions = service.Questions.Where(x => x.ServiceSectionID == 0).ToList();

            //unCategoriezedQuestion.Visible = false;
            //noUnCategorizedDataMsg.Visible = true;
            //if (nonSectionQuestions.Count > 0)
            //{
            //    unCategoriezedQuestion.Visible = true;
            //    noUnCategorizedDataMsg.Visible = false;
            //}
            //rNonSectionQuestions.DataSource = nonSectionQuestions;
            //rNonSectionQuestions.DataBind();



        }

        protected void RSections_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                ServiceSection section = (ServiceSection)e.Item.DataItem;
                List<ServiceSection> sections = (List<ServiceSection>)rSections.DataSource;

                //Stores section id in hidden variable
                HiddenField hfServiceSectionID = (HiddenField)e.Item.FindControl("hfServiceSectionID");
                hfServiceSectionID.Value = section.ID.ToString();

                //Fills order dropdown list
                DropDownList ddlOrders = (DropDownList)e.Item.FindControl("ddlSectionOrderBy");
                ddlOrders.Attributes.Add("onChange", string.Format("reOrderSection(this, {1}, {0})", sections.Count, e.Item.ItemIndex));
                GenerateOrderDropDown(ddlOrders, sections.Count);
                ddlOrders.CssClass = "section-order itsmDropDownList grid-aspxDropDownList";
                ddlOrders.SelectedIndex = e.Item.ItemIndex;

                //Binds internal repeater to show question
                Repeater questionRepeater = (Repeater)e.Item.FindControl("rSectionQuestions");
                if (questionRepeater != null)
                {
                    questionRepeater.DataSource = service.Questions.Where(x => x.ServiceSectionID == section.ID && x.QuestionType != "Rating").OrderBy(x => x.ItemOrder).ToList();
                    questionRepeater.DataBind();
                }
            }
        }

        protected void RSectionQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                ServiceQuestion question = (ServiceQuestion)e.Item.DataItem;
                List<ServiceQuestion> questions = (List<ServiceQuestion>)((Repeater)sender).DataSource;

                Label lbQuestionTitle = (Label)e.Item.FindControl("lbQuestionTitle");
                lbQuestionTitle.Text = question.QuestionTitle;
                if (question.FieldMandatory)
                {
                    lbQuestionTitle.Text = string.Format("{0}<b style='color:red;'>*</b>", question.QuestionTitle);
                }

                //Stores question id in hidden variable
                HiddenField hfServiceQuestionID = (HiddenField)e.Item.FindControl("hfServiceQuestionID");
                hfServiceQuestionID.Value = question.ID.ToString();

                //Fills order dropdown list
                DropDownList ddlOrders = (DropDownList)e.Item.FindControl("ddlSectionOrderBy");
                ddlOrders.Attributes.Add("onChange", string.Format("reOrderQuestion(this, {1}, {0}, {2})", questions.Count, e.Item.ItemIndex, question.ServiceSectionID));
                GenerateOrderDropDown(ddlOrders, questions.Count);
                ddlOrders.SelectedIndex = e.Item.ItemIndex;
                ddlOrders.CssClass = string.Format("itsmDropDownList grid-aspxDropDownList question-order-{0}", question.ServiceSectionID);
            }
        }

        protected void GenerateOrderDropDown(DropDownList ddlOrders, int totalItem)
        {
            if (totalItem > 0)
            {
                for (int i = 1; i <= totalItem; i++)
                {
                    ddlOrders.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
        }

        protected void BtSaveSectionQuestionOrder_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || !ASPxEdit.ValidateEditorsInContainer(tabServiceDetails, "serviceIntitial"))
                return;

            foreach (RepeaterItem rItem in rSections.Items)
            {
                HiddenField hfServiceSectionID = (HiddenField)rItem.FindControl("hfServiceSectionID");
                //Save order of each section
                int serviceSectionID = 0;
                int.TryParse(hfServiceSectionID.Value, out serviceSectionID);
                DropDownList ddlOrders = (DropDownList)rItem.FindControl("ddlSectionOrderBy");
                ServiceSection section = service.Sections.FirstOrDefault(x => x.ID == serviceSectionID);
                section.ItemOrder = UGITUtility.StringToInt(ddlOrders.SelectedValue);

                //Iterate Repeator to save order of each question
                Repeater questionRepeater = (Repeater)rItem.FindControl("rSectionQuestions");
                foreach (RepeaterItem rQuestionItem in questionRepeater.Items)
                {
                    HiddenField hfServiceQuestionID = (HiddenField)rQuestionItem.FindControl("hfServiceQuestionID");
                    int serviceQuestionID = 0;
                    int.TryParse(hfServiceQuestionID.Value, out serviceQuestionID);
                    DropDownList ddlQuestionOrders = (DropDownList)rQuestionItem.FindControl("ddlSectionOrderBy");
                    ServiceQuestion question = service.Questions.FirstOrDefault(x => x.ID == serviceQuestionID);
                    question.ItemOrder = UGITUtility.StringToInt(ddlQuestionOrders.SelectedValue);
                }
            }

            //Saves order changes in database
            ServiceSectionManager serviceSectionManager = new ServiceSectionManager(HttpContext.Current.GetManagerContext());
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
            serviceSectionManager.SaveOrder(service.Sections);
            serviceQuestionManager.SaveOrder(service.Questions);

            //Close popup when save and close button click
            if (UGITUtility.StringToBoolean(hfSaveAndClose.Value))
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }

            BindQuestionSectionGrid();
        }

        protected void rNonSectionQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {

                //rNonSectionQuestionstable.Visible = true;
                ServiceQuestion question = (ServiceQuestion)e.Item.DataItem;

                Label lbQuestionTitle = (Label)e.Item.FindControl("lbQuestionTitle");
                lbQuestionTitle.Text = question.QuestionTitle;
                if (question.FieldMandatory)
                {
                    lbQuestionTitle.Text = string.Format("{0}<b style='color:red;'>*</b>", question.QuestionTitle);
                }
            }

        }
        #endregion

        #region  Tab 3 Ticket/Task
        private void GenerateSubTicketsList()
        {
            TaskList taskList = (TaskList)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Task/TaskList.ascx");
            taskList.ModuleName = "SVCConfig";
            taskList.TicketID = ServiceId;
            taskList.DisableBatchEdit = true;
            taskList.DisableMarkAsComplete = true;
            taskList.DisableNewRecuringTask = true;
            taskList.TicketPublicId = Convert.ToString(ServiceId); // Request["ticketID"];
            pnlSubTickets.Controls.Add(taskList);

        }

        #endregion

        #region Tab 4 Mapping
        //private void BindQuestionMapping(string taskIDString)
        //{
        //    mappedInfo = new List<MappedControlInfo>();
        //    rItemMapping.ID = "rItemMapping" + taskIDString;

        //    ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
        //    serviceVariables = serviceQuestionManager.GetBuiltInQuestions();
        //    ServiceQuestion variable = null;
        //    if (service.QMapVariables != null && service.QMapVariables.Count > 0)
        //    {
        //        foreach (QuestionMapVariable mVar in service.QMapVariables)
        //        {
        //            variable = new ServiceQuestion();
        //            variable.TokenName = mVar.ShortName;
        //            variable.QuestionTitle = mVar.Title;
        //            variable.QuestionType = mVar.Type;
        //            serviceVariables.Add(variable);
        //        }
        //    }
        //    serviceVariables = serviceVariables.OrderBy(x => x.TokenName).ToList();

        //    if (service.QuestionsMapping != null)
        //    {
        //        List<UGITTask> tasks = service.Tasks;
        //        if (service.CreateParentServiceRequest)
        //        {
        //            UGITTask svcTask = new UGITTask();
        //            tasks.Insert(0, svcTask);
        //            UGITModule moduleDetail = ObjModuleViewManager.LoadByName("SVC");
        //            svcTask.ModuleNameLookup = string.Format("{0}{1}{2}", moduleDetail.ID, Constants.Separator, "SVC");
        //            svcTask.Title = "Service Instance";
        //            svcTask.RelatedModule = ModuleNames.SVC;
        //        }

        //        int taskID = 0;
        //        int.TryParse(taskIDString, out taskID);

        //        if (taskID <= 0 && ddlTaskTickets.Items.Count > 0)
        //        {
        //            int.TryParse(ddlTaskTickets.Items[0].Value, out taskID);
        //        }

        //        UGITTask selectedTask = tasks.FirstOrDefault(x => x.ID == taskID);
        //        List<UGITTask> selectedTasks = new List<UGITTask>();
        //        if (selectedTask != null)
        //            selectedTasks.Add(selectedTask);

        //        DataTable ticketFields = new DataTable();
        //        if (Category != Constants.ModuleAgent)
        //        {
        //            ServiceQuestionMappingManager mappingManager = new ServiceQuestionMappingManager(HttpContext.Current.GetManagerContext());
        //            ticketFields = mappingManager.GetFieldsRequiredToCreateTickets(selectedTasks);
        //            if (ticketFields != null && ticketFields.Rows.Count > 0)
        //            {
        //                DataRow[] extraRows = new DataRow[0];
        //                //If requestcategory of task is not null then exclude request type from the mapping.
        //                if (selectedTask != null && selectedTask.ModuleNameLookup != null && selectedTask.RequestTypeCategory != null)
        //                {
        //                    extraRows = ticketFields.AsEnumerable().Where(x => x.Field<string>("FieldName") == DatabaseObjects.Columns.TicketRequestTypeLookup).ToArray();
        //                }

        //                foreach (DataRow row in extraRows)
        //                {
        //                    ticketFields.Rows.Remove(row);
        //                }

        //                ticketFields.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
        //                ticketFields = ticketFields.DefaultView.ToTable();
        //            }
        //        }
        //        else
        //        {
        //            UGITModule dr = ObjModuleViewManager.LoadByName(service.ModuleNameLookup);
        //            if (dr != null)
        //            {
        //                UGITModule module = dr;
        //                ServiceQuestionMappingManager serviceQuestionMappingManager = new ServiceQuestionMappingManager(HttpContext.Current.GetManagerContext());
        //                ticketFields = serviceQuestionMappingManager.GetFieldsRequiredToCreateAgents(module);
        //                int i = ticketFields.Rows.Count;
        //                ticketFields.Columns.Add(DatabaseObjects.Columns.ItemOrder);
        //                foreach (DataRow drs in ticketFields.Rows)
        //                {
        //                    drs[DatabaseObjects.Columns.ItemOrder] = 0;
        //                    ServiceQuestionMapping serviceQuestionMapping = service.QuestionsMapping.FirstOrDefault(x => x.ServiceTaskID.ToString() == Convert.ToString(drs["ServiceTicketID"]) && x.ColumnName == Convert.ToString(drs[DatabaseObjects.Columns.FieldName]));
        //                    if (serviceQuestionMapping != null)
        //                    {
        //                        drs[DatabaseObjects.Columns.ItemOrder] = i;
        //                        i = i - 1;
        //                    }
        //                }
        //                if (ticketFields.Rows.Count > 0)
        //                {
        //                    DataView dataView = ticketFields.DefaultView;
        //                    dataView.Sort = DatabaseObjects.Columns.ItemOrder + " DESC," + DatabaseObjects.Columns.FieldDisplayName + " ASC";
        //                    ticketFields = dataView.ToTable();
        //                }
        //            }
        //        }
        //        rItemMapping.DataSource = ticketFields;
        //        rItemMapping.DataBind();
        //    }
        //}
        private void BindQuestionMapping(string taskIDString)
        {
            mappedInfo = new List<MappedControlInfo>();
            rItemMapping.ID = "rItemMapping" + taskIDString;
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
            //list of variables(Custom and built-in)
            serviceVariables = serviceQuestionManager.GetBuiltInQuestions();
            ServiceQuestion variable = null;
            if (service.QMapVariables != null && service.QMapVariables.Count > 0)
            {
                foreach (QuestionMapVariable mVar in service.QMapVariables)
                {
                    variable = new ServiceQuestion();
                    variable.TokenName = mVar.ShortName;
                    variable.QuestionTitle = mVar.Title;
                    variable.QuestionType = mVar.Type;
                    serviceVariables.Add(variable);
                }
            }
            serviceVariables = serviceVariables.OrderBy(x => x.TokenName).ToList();

            if (service.QuestionsMapping != null)
            {
                List<UGITTask> tasks = service.Tasks;
                if (service.CreateParentServiceRequest)
                {
                    UGITTask svcTask = new UGITTask();
                    tasks.Insert(0, svcTask);
                    UGITModule moduleDetail = ObjModuleViewManager.LoadByName("SVC");
                    svcTask.ModuleNameLookup = string.Format("{0}{1}{2}", moduleDetail.ID, Constants.Separator, "SVC");
                    svcTask.Title = "Service Instance";
                    svcTask.RelatedModule = ModuleNames.SVC;
                }

                int taskID = 0;
                int.TryParse(taskIDString, out taskID);

                if (taskID <= 0 && ddlTaskTickets.Items.Count > 0)
                {
                    int.TryParse(ddlTaskTickets.Items[0].Value, out taskID);
                }

                UGITTask selectedTask = tasks.FirstOrDefault(x => x.ID == taskID);
                List<UGITTask> selectedTasks = new List<UGITTask>();
                if (selectedTask != null)
                    selectedTasks.Add(selectedTask);

                DataTable ticketFields = new DataTable();
                if (Category.ToLower() != Constants.ModuleAgent.ToLower())
                {
                    ServiceQuestionMappingManager mappingManager = new ServiceQuestionMappingManager(HttpContext.Current.GetManagerContext());
                    ticketFields = mappingManager.GetFieldsRequiredToCreateTickets(selectedTasks);
                    if (ticketFields != null && ticketFields.Rows.Count > 0)
                    {
                        DataRow[] extraRows = new DataRow[0];

                        //If requestcategory of task is not null then exclude request type from the mapping.
                        if (selectedTask != null && selectedTask.ModuleNameLookup != null && selectedTask.RequestTypeCategory != null && selectedTask.RequestTypeCategory != "")
                        {
                            extraRows = ticketFields.AsEnumerable().Where(x => x.Field<string>("FieldName") == DatabaseObjects.Columns.TicketRequestTypeLookup).ToArray();
                        }

                        foreach (DataRow row in extraRows)
                        {
                            ticketFields.Rows.Remove(row);
                        }

                        ticketFields.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
                        ticketFields = ticketFields.DefaultView.ToTable();
                    }
                }
                else
                {
                    UGITModule dr = ObjModuleViewManager.LoadByName(service.ModuleNameLookup);
                    if (dr != null)
                    {
                        UGITModule module = dr;
                        ServiceQuestionMappingManager serviceQuestionMappingManager = new ServiceQuestionMappingManager(HttpContext.Current.GetManagerContext());
                        ticketFields = serviceQuestionMappingManager.GetFieldsRequiredToCreateAgents(module);
                        int i = ticketFields.Rows.Count;
                        ticketFields.Columns.Add(DatabaseObjects.Columns.ItemOrder);

                        foreach (DataRow drs in ticketFields.Rows)
                        {
                            drs[DatabaseObjects.Columns.ItemOrder] = 0;
                            ServiceQuestionMapping serviceQuestionMapping = service.QuestionsMapping.FirstOrDefault(x => x.ColumnName == Convert.ToString(drs[DatabaseObjects.Columns.FieldName]));
                            if (serviceQuestionMapping != null)
                            {
                                drs[DatabaseObjects.Columns.ItemOrder] = i;
                                i = i - 1;
                            }
                        }
                        if (ticketFields.Rows.Count > 0)
                        {
                            DataView dataView = ticketFields.DefaultView;
                            dataView.Sort = DatabaseObjects.Columns.ItemOrder + " DESC," + DatabaseObjects.Columns.FieldDisplayName + " ASC";
                            ticketFields = dataView.ToTable();
                        }
                    }
                }
                rItemMapping.DataSource = ticketFields;
                rItemMapping.DataBind();
            }
        }
        //protected void RItemMapping_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    string moduleName = service.ModuleNameLookup;
        //    DataRowView row = (DataRowView)e.Item.DataItem;
        //    if (e.Item.ItemType == ListItemType.Header)
        //    {
        //        System.Web.UI.HtmlControls.HtmlTableCell thEnableMapping = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thEnableMapping");
        //        if (Category == Constants.ModuleAgent)
        //            thEnableMapping.Visible = true;
        //        else
        //            thEnableMapping.Visible = false;
        //    }
        //    if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        //    {
        //        System.Web.UI.HtmlControls.HtmlTableCell tdEnableMapping = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdEnableMapping");
        //        if (Category == Constants.ModuleAgent)
        //            tdEnableMapping.Visible = true;
        //        else
        //            tdEnableMapping.Visible = false;

        //        System.Web.UI.HtmlControls.HtmlTableRow groupRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("ticketGroupTr");
        //        Image imgQuestionMapArrow = (Image)e.Item.FindControl("imgQuestionMapArrow");
        //        Panel groupPanel = (Panel)e.Item.FindControl("ticketGroupPanel");
        //        groupRow.Visible = false;
        //        int selectedTaskID = 0;
        //        int.TryParse(ddlTaskTickets.SelectedValue, out selectedTaskID);
        //        UGITTask taskInfo = service.Tasks.FirstOrDefault(x => x.ID == selectedTaskID);
        //        if (previousSTicketID != UGITUtility.StringToInt(row["ServiceTicketID"]) || (taskInfo != null && taskInfo.SubTaskType == ServiceSubTaskType.AccountTask))
        //        {
        //            previousSTicketID = UGITUtility.StringToInt(row["ServiceTicketID"]);
        //            previousSModuleID = UGITUtility.StringToInt(row["ModuleID"]);
        //            //If the module is task then load list ServiceTicketRelationships otherwise load module ticket detail list
        //            if (Convert.ToString(row["ModuleName"]) == "Task")
        //            {
        //                UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
        //                fieldList = UGITUtility.ObjectToData(taskManager.LoadByID(selectedTaskID));
        //            }
        //            else
        //            {
        //                fieldList = ObjTicketManager.GetAllTickets(ObjModuleViewManager.LoadByName(Convert.ToString(row["ModuleName"])));
        //            }
        //        }
        //        //Creates spfield for each field
        //        string fieldName = Convert.ToString(row[DatabaseObjects.Columns.FieldName]);

        //        if (!fieldList.Columns.Contains(Convert.ToString(row[DatabaseObjects.Columns.FieldName])))
        //        {
        //            e.Item.Visible = false;
        //        }
        //        else
        //        {
        //            DataRow newItem = fieldList.NewRow();
        //            DataColumn configField = fieldList.Columns[Convert.ToString(row[DatabaseObjects.Columns.FieldName])];
        //            FieldConfigurationManager configFieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
        //            FieldConfiguration fieldForControl = configFieldManager.GetFieldByFieldName(configField.ColumnName);
        //            string fieldColumnType = string.Empty;
        //            HiddenField hfServieTaskID = (HiddenField)(e.Item.FindControl("hfServieTaskID"));
        //            hfServieTaskID.Value = Convert.ToString(row["ServiceTicketID"]);
        //            ServiceQuestionMapping serviceQuestionMapping = service.QuestionsMapping.FirstOrDefault(x => x.ServiceTaskID.ToString() == Convert.ToString(row["ServiceTicketID"]) && x.ColumnName == configField.ColumnName);
        //            CheckBox chkbxMapping = (CheckBox)(e.Item.FindControl("chkbxMapping"));
        //            if (serviceQuestionMapping != null && chkbxMapping != null)
        //                chkbxMapping.Checked = true;

        //            if (fieldColumnType == "System.String" || fieldColumnType == "NoteField")
        //            {
        //                imgQuestionMapArrow.Visible = true;
        //            }

        //            Control control = null;
        //            if (fieldColumnType == "Date" || fieldColumnType == "System.DateTime")
        //            {
        //                Panel pnlDateCtr = new Panel();
        //                DropDownList ddlPlusMinus = new DropDownList();
        //                ddlPlusMinus.ID = "datetimeTxt";
        //                ddlPlusMinus.CssClass = "txtDateMapping";
        //                ddlPlusMinus.Items.Insert(0, new ListItem("+", "0"));
        //                ddlPlusMinus.Items.Insert(1, new ListItem("-", "1"));
        //                // txtBoxCtr.Text = "+";
        //                ASPxSpinEdit dxNoOfDays = new ASPxSpinEdit();

        //                dxNoOfDays.ID = "datetimeSpEdit";
        //                dxNoOfDays.Width = new Unit("80px");
        //                dxNoOfDays.HelpTextSettings.VerticalAlign = HelpTextVerticalAlign.Middle;
        //                dxNoOfDays.HelpText = "Day(s)";
        //                dxNoOfDays.HelpTextSettings.Position = HelpTextPosition.Right;
        //                dxNoOfDays.MinValue = 0;
        //                dxNoOfDays.MaxValue = 365;
        //                if (serviceQuestionMapping != null)
        //                {
        //                    if (serviceQuestionMapping.ColumnValue.Contains(","))
        //                    {
        //                        int noOfDays = UGITUtility.StringToInt((serviceQuestionMapping.ColumnValue.Split(',')[1]).Split(')')[0]);
        //                        if (noOfDays == 0)
        //                        {
        //                            dxNoOfDays.Text = string.Empty;
        //                            ddlPlusMinus.SelectedIndex = 0;
        //                        }
        //                        else if (noOfDays > 0)
        //                        {
        //                            dxNoOfDays.Text = Convert.ToString(noOfDays);
        //                            ddlPlusMinus.SelectedIndex = 0;
        //                        }
        //                        else
        //                        {
        //                            ddlPlusMinus.SelectedIndex = 1;
        //                            dxNoOfDays.Text = Convert.ToString(Math.Abs(noOfDays));
        //                        }
        //                    }
        //                    HiddenField hfQuestionMapID = (HiddenField)(e.Item.FindControl("hfQuestionMapID"));
        //                    hfQuestionMapID.Value = serviceQuestionMapping.ID.ToString();
        //                }
        //                pnlDateCtr.Controls.Add(ddlPlusMinus);
        //                pnlDateCtr.Controls.Add(dxNoOfDays);
        //                control = pnlDateCtr;
        //            }
        //            else
        //            {
        //                if (serviceQuestionMapping != null)
        //                {
        //                    if (!string.IsNullOrEmpty(serviceQuestionMapping.ColumnValue) && serviceQuestionMapping.ColumnValue.ToLower() != "[$current$]")
        //                    {
        //                        newItem[configField.ColumnName] = serviceQuestionMapping.ColumnValue;
        //                    }
        //                    else
        //                    {
        //                        newItem[configField.ColumnName] = DBNull.Value;
        //                    }
        //                    HiddenField hfQuestionMapID = (HiddenField)(e.Item.FindControl("hfQuestionMapID"));
        //                    hfQuestionMapID.Value = serviceQuestionMapping.ID.ToString();
        //                }

        //                bool callDefaultCtrGenerator = true;
        //                if (fieldColumnType == "Lookup")
        //                {
        //                    LookUpValueBox lookupField = new LookUpValueBox();
        //                    //lookupField.devexListBox.CssClass = editcss;
        //                    //lookupField.devexListBox.ClientSideEvents.ValueChanged = "rowClick";
        //                    lookupField.FieldName = fieldForControl.FieldName;
        //                    if (taskInfo != null)
        //                    {
        //                        lookupField.ModuleName = taskInfo.RelatedModule;
        //                    }
        //                    else
        //                    {
        //                        lookupField.ModuleName = moduleName;
        //                    }
        //                    //lookupField.ID = GenerateID(field, tabId);
        //                    if (lookupField.IsMulti &&
        //                        (fieldForControl.FieldName != DatabaseObjects.Columns.DepartmentLookup && fieldForControl.FieldName != DatabaseObjects.Columns.TicketBeneficiaries))
        //                    {
        //                        control = GetMultiLookupControl(this.Context, configField, fieldList, newItem);
        //                        callDefaultCtrGenerator = false;
        //                    }
        //                }
        //                if (callDefaultCtrGenerator)
        //                {
        //                    UGITModule aModule = null;
        //                    if (taskInfo != null)
        //                    {
        //                        //SPControlMode mode = SPControlMode.New;  
        //                        aModule = ObjModuleViewManager.LoadByName(Convert.ToString(taskInfo.RelatedModule));
        //                    }
        //                    else
        //                    {
        //                        aModule = ObjModuleViewManager.LoadByName(service.ModuleNameLookup);
        //                    }
        //                    TicketControls ticketControl = new TicketControls(aModule, newItem);
        //                    ticketControl.SourceItem = newItem;
        //                    control = ticketControl.GetControls(configField, ControlMode.New, FieldDesignMode.Normal, string.Empty, null);
        //                }

        //                #region Some controls are not able to pre select value. this code is for those control
        //                if (serviceQuestionMapping != null && (fieldColumnType == "Choice" || fieldColumnType == "Boolean" || fieldColumnType == "Lookup"))
        //                {
        //                    MappedControlInfo mappedData = new MappedControlInfo();
        //                    mappedData.FieldName = fieldName;
        //                    mappedData.Control = control;
        //                    mappedData.Value = serviceQuestionMapping.ColumnValue;
        //                    mappedData.CtrType = fieldColumnType;
        //                    mappedInfo.Add(mappedData);
        //                }

        //                if (serviceQuestionMapping == null && fieldColumnType == "UserField")
        //                {
        //                    MappedControlInfo mappedData = new MappedControlInfo();
        //                    mappedData.FieldName = fieldName;
        //                    mappedData.Control = control;
        //                    if (taskInfo != null)
        //                        mappedData.Value = taskInfo.AssignedTo;
        //                    mappedData.CtrType = fieldColumnType;
        //                    mappedInfo.Add(mappedData);
        //                }
        //                #endregion
        //            }


        //            if (serviceQuestionMapping != null && control != null && !(control is LookUpValueBox))
        //            {
        //                // Need to do control.Controls.Count otherwise control.HasControls() statement does not work sometimes
        //                // But need try-catch in case no controls 
        //                int controlCount = 0;
        //                try
        //                {
        //                    if (control.Controls != null)
        //                        controlCount = control.Controls.Count;
        //                }
        //                catch { }

        //                if ((controlCount > 0 || control.HasControls()) && control.Controls[0] is DropDownList)
        //                {
        //                    DropDownList ddlList = control.Controls[0] as DropDownList;
        //                    ASPxSpinEdit dxNoDays = control.Controls[1] as ASPxSpinEdit;
        //                    if (dxNoDays != null && ddlList != null && serviceQuestionMapping.ColumnValue.Contains(","))
        //                    {
        //                        int noOfDays = UGITUtility.StringToInt((serviceQuestionMapping.ColumnValue.Split(',')[1]).Split(')')[0]);
        //                        if (noOfDays == 0)
        //                        {
        //                            dxNoDays.Text = string.Empty;
        //                            ddlList.SelectedIndex = 0;
        //                        }
        //                        else if (noOfDays > 0)
        //                        {
        //                            dxNoDays.Text = Convert.ToString(noOfDays);
        //                            ddlList.SelectedIndex = 0;
        //                        }
        //                        else
        //                        {
        //                            ddlList.SelectedIndex = 1;
        //                            dxNoDays.Text = Convert.ToString(Math.Abs(noOfDays));
        //                        }
        //                        if (string.IsNullOrEmpty(serviceQuestionMapping.PickValueFrom) && !string.IsNullOrWhiteSpace(dxNoDays.Text))
        //                            serviceQuestionMapping.PickValueFrom = "Today";
        //                    }
        //                    else if (ddlList != null)
        //                        ddlList.SelectedIndex = ddlList.Items.IndexOf(ddlList.Items.FindByText(serviceQuestionMapping.ColumnValue));
        //                }
        //                else if (control is DropDownList)
        //                {
        //                    DropDownList ddlList = (DropDownList)control;
        //                    ddlList.SelectedIndex = ddlList.Items.IndexOf(ddlList.Items.FindByText(serviceQuestionMapping.ColumnValue));
        //                }
        //                else if (control is ASPxComboBox)
        //                {
        //                    ASPxComboBox combobox = control as ASPxComboBox;
        //                    //SPFieldLookupValue lLook = new SPFieldLookupValue(serviceQuestionMapping.ColumnValue);
        //                    //if (lLook != null && lLook.LookupId > 0)
        //                    //    combobox.SelectedItem = combobox.Items.FindByValue(lLook.LookupId);
        //                }
        //            }

        //            Panel controlPanel = (Panel)(e.Item.FindControl("controlPanel"));
        //            controlPanel.Controls.Add(control);

        //            //Shows display name of field
        //            System.Web.UI.HtmlControls.HtmlTableCell ticketFieldNameObj = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("ticketFieldName");
        //            ticketFieldNameObj.Controls.Add(new LiteralControl(Convert.ToString(row[DatabaseObjects.Columns.FieldDisplayName])));
        //            bool mandatory = false;
        //            if (uHelper.IfColumnExists(DatabaseObjects.Columns.FieldMandatory, row.DataView.Table))
        //                mandatory = UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.FieldMandatory]);
        //            if (mandatory)
        //                ticketFieldNameObj.Controls.Add(new LiteralControl("<b style='color:red;padding-left:2px;vertical-align: middle;'>*</b>"));

        //            if (configField.ColumnName == DatabaseObjects.Columns.Approver)
        //            {
        //                ticketFieldNameObj.Controls.Clear();
        //                if (!mandatory)
        //                    ticketFieldNameObj.Controls.Add(new LiteralControl(string.Format("{0}<br /><b style='color:Gray;float:left;padding-top:3px;'>Skips approval if left blank</b>", Convert.ToString(row[DatabaseObjects.Columns.FieldDisplayName]))));
        //                else
        //                    ticketFieldNameObj.Controls.Add(new LiteralControl(string.Format("{0}<b style='color:red;padding-left:2px;vertical-align: middle;'>*</b><br /><b style='color:Gray;float:left;padding-top:3px;'>Skips approval if left blank</b>", Convert.ToString(row[DatabaseObjects.Columns.FieldDisplayName]))));
        //            }

        //            HiddenField hfFieldInternalName = (HiddenField)(e.Item.FindControl("hfFieldInternalName"));
        //            hfFieldInternalName.Value = configField.ColumnName;

        //            #region fill question dropdown for each mapped field
        //            List<ServiceQuestion> questions = service.Questions.OrderBy(x => x.TokenName).ToList();
        //            List<ServiceQuestion> sVariables = serviceVariables;

        //            Panel ucontrolPanel = (Panel)(e.Item.FindControl("ctpUserCollection"));
        //            TextBox txtUserCollection = new TextBox();

        //            #region Picks questions and variable based on types
        //            if (configField.ColumnName == DatabaseObjects.Columns.TicketRequestTypeLookup)
        //            {
        //                questions = questions.Where(x => x.QuestionType.ToLower() == "requesttype").ToList();

        //                List<ServiceQuestion> qs = new List<ServiceQuestion>();
        //                if (questions.Count > 0)
        //                {
        //                    //Filter out only related request type questions
        //                    //UGITModule module = ObjModuleViewManager.GetByID(previousSTicketID);
        //                    moduleName = string.Empty;
        //                    string tModuleName = string.Empty;
        //                    if (taskInfo != null && taskInfo.RelatedModule != null)
        //                    {
        //                        moduleName = taskInfo.RelatedModule;
        //                    }
        //                    else if (!string.IsNullOrEmpty(service.ModuleNameLookup))
        //                    {
        //                        UGITModule moduleRow = ObjModuleViewManager.GetByName(service.ModuleNameLookup);
        //                        if (moduleRow != null)
        //                        {
        //                            moduleName = moduleRow.ModuleName;
        //                        }
        //                    }
        //                    foreach (ServiceQuestion q in questions)
        //                    {
        //                        q.QuestionTypePropertiesDicObj.TryGetValue("module", out tModuleName);
        //                        if (tModuleName == moduleName)
        //                        {
        //                            qs.Add(q);
        //                        }
        //                    }
        //                    questions = qs;
        //                }

        //            }
        //            else if (fieldColumnType == "System.DateTime")
        //            {
        //                questions = questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME).ToList();
        //                sVariables = sVariables.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME).ToList();
        //            }
        //            else if (fieldColumnType == "UserField")
        //            {
        //                questions = questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD).ToList();
        //                sVariables = sVariables.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD).ToList();
        //                //for textbox if question mapped value is not defalut only for userfield type question
        //                txtUserCollection.ID = "txtUserCollection";
        //                txtUserCollection.CssClass = "ccsusercollection doubleWidth";
        //                ucontrolPanel.Controls.Add(txtUserCollection);
        //            }
        //            #endregion

        //            #region Binds questions and variables to dropdown
        //            DropDownList questionDropDown = (DropDownList)(e.Item.FindControl("ddlQuestionMapped"));
        //            string dText = string.Empty;
        //            foreach (ServiceQuestion question in questions)
        //            {
        //                dText = string.Format("{0}", question.QuestionTitle);
        //                if (!string.IsNullOrEmpty(question.TokenName))
        //                    dText = string.Format("[${1}$] {0}", question.QuestionTitle, question.TokenName);

        //                questionDropDown.Items.Add(new ListItem(dText, question.ID.ToString()));
        //            }
        //            #endregion

        //            #region Add variables(Custom or inbuilt) in dropdown
        //            if (sVariables != null && sVariables.Count > 0)
        //            {
        //                ListItem variableItem = new ListItem("--Variables--", string.Empty);
        //                variableItem.Attributes.Add("disabled", "disabled");
        //                questionDropDown.Items.Add(variableItem);
        //                foreach (ServiceQuestion question in questions)
        //                {
        //                    dText = string.Format("{0} Manager", question.QuestionTitle);
        //                    if (!string.IsNullOrEmpty(question.TokenName))
        //                        dText = string.Format("{0}[${2}~Manager$] {1}", HttpUtility.HtmlDecode("&nbsp;&nbsp;"), dText, question.TokenName);

        //                    questionDropDown.Items.Add(new ListItem(dText, question.TokenName + "~Manager"));
        //                }
        //                foreach (ServiceQuestion qVariable in sVariables)
        //                {
        //                    variableItem = new ListItem(string.Format("{0}[${1}$] {2}", HttpUtility.HtmlDecode("&nbsp;&nbsp;"), qVariable.TokenName, qVariable.QuestionTitle), qVariable.TokenName);
        //                    questionDropDown.Items.Add(variableItem);
        //                }
        //            }
        //            questionDropDown.Items.Insert(0, new ListItem("--None or Default--", string.Empty));
        //            #endregion

        //            #endregion

        //            if (serviceQuestionMapping != null)
        //            {
        //                questionDropDown.SelectedIndex = questionDropDown.Items.IndexOf(questionDropDown.Items.FindByValue(serviceQuestionMapping.PickValueFrom));
        //            }

        //            questionDropDown.Attributes.Add("onchange", "fieldMapToQuestion(this)");
        //            questionDropDown.CssClass = "donthide ddlquestionmapped";
        //            if (fieldColumnType != "System.String" && fieldColumnType != "Date" && fieldColumnType != "DateTime" && fieldColumnType != "System.DateTime" && fieldColumnType != "NoteField")
        //            {
        //                questionDropDown.CssClass = string.Empty;
        //                if (questionDropDown.SelectedValue != string.Empty && questionDropDown.SelectedIndex > 0)
        //                {
        //                    if (serviceQuestionMapping != null)
        //                    {
        //                        txtUserCollection.Text = serviceQuestionMapping.ColumnValue;
        //                        HiddenField hfQuestionMapID = (HiddenField)(e.Item.FindControl("hfQuestionMapID"));
        //                        hfQuestionMapID.Value = serviceQuestionMapping.ID.ToString();
        //                    }
        //                    controlPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
        //                    ucontrolPanel.Style.Add(HtmlTextWriterStyle.Display, "block");

        //                }
        //                else
        //                {
        //                    txtUserCollection.Text = string.Empty;
        //                    controlPanel.Style.Add(HtmlTextWriterStyle.Display, "block");
        //                    ucontrolPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
        //                }
        //            }
        //        }
        //    }
        //}


        protected void RItemMapping_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string moduleName = service.ModuleNameLookup;
            DataRowView row = (DataRowView)e.Item.DataItem;
            if (e.Item.ItemType == ListItemType.Header)
            {
                System.Web.UI.HtmlControls.HtmlTableCell thEnableMapping = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("thEnableMapping");
                if (Category.ToLower() == Constants.ModuleAgent.ToLower())
                    thEnableMapping.Visible = true;

                else
                    thEnableMapping.Visible = false;
            }
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                System.Web.UI.HtmlControls.HtmlTableCell tdEnableMapping = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdEnableMapping");
                //if (Category.ToLower() == Constants.ModuleAgent.ToLower())
                //    tdEnableMapping.Visible = true;
                //else
                //    tdEnableMapping.Visible = false;

                System.Web.UI.HtmlControls.HtmlTableRow groupRow = (System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("ticketGroupTr");
                Image imgQuestionMapArrow = (Image)e.Item.FindControl("imgQuestionMapArrow");
                Panel groupPanel = (Panel)e.Item.FindControl("ticketGroupPanel");
                groupRow.Visible = false;
                int selectedTaskID = 0;
                int.TryParse(ddlTaskTickets.SelectedValue, out selectedTaskID);

                if (previousSTicketID != UGITUtility.StringToInt(row["ServiceTicketID"]))
                {
                    previousSTicketID = UGITUtility.StringToInt(row["ServiceTicketID"]);
                    previousSModuleID = UGITUtility.StringToInt(row["ModuleID"]);
                    //If the module is task then load list ServiceTicketRelationships otherwise load module ticket detail list:old comment
                    // load task from moduletasks bcz serviceticketrelationship table merged with moduletask: new comment
                    if (Convert.ToString(row["ModuleName"]) == "Task")
                    {
                        UGITTaskManager taskManager = new UGITTaskManager(HttpContext.Current.GetManagerContext());
                        fieldList = UGITUtility.ObjectToData(taskManager.LoadByID(selectedTaskID));
                    }
                    else
                    {
                        fieldList = ObjTicketManager.GetAllTickets(ObjModuleViewManager.LoadByName(Convert.ToString(row["ModuleName"])));
                    }
                }

                UGITTask taskInfo = service.Tasks.FirstOrDefault(x => x.ID == selectedTaskID);

                //Creates spfield for each field
                string fieldName = Convert.ToString(row[DatabaseObjects.Columns.FieldName]);
                if (!fieldList.Columns.Contains(Convert.ToString(row[DatabaseObjects.Columns.FieldName])))
                {
                    e.Item.Visible = false;
                }
                else
                {
                    DataRow newItem = fieldList.NewRow();
                    DataColumn configField = fieldList.Columns[Convert.ToString(row[DatabaseObjects.Columns.FieldName])];
                    FieldConfigurationManager configFieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                    FieldConfiguration fieldForControl = configFieldManager.GetFieldByFieldName(configField.ColumnName);
                    string fieldColumnType = string.Empty;
                    if (fieldForControl != null)
                    {
                        fieldColumnType = Convert.ToString(fieldForControl.Datatype);
                    }
                    else
                        fieldColumnType = Convert.ToString(configField.DataType);

                    HiddenField hfServieTaskID = (HiddenField)(e.Item.FindControl("hfServieTaskID"));
                    hfServieTaskID.Value = Convert.ToString(row["ServiceTicketID"]);
                    ServiceQuestionMapping serviceQuestionMapping = service.QuestionsMapping.FirstOrDefault(x => Convert.ToInt32(x.ServiceTaskID) == Convert.ToInt32(row["ServiceTicketID"]) && x.ColumnName == configField.ColumnName);

                    CheckBox chkbxMapping = (CheckBox)(e.Item.FindControl("chkbxMapping"));
                    if (serviceQuestionMapping != null && chkbxMapping != null)
                        chkbxMapping.Checked = true;

                    if (fieldColumnType == "System.String" || fieldColumnType == "NoteField")
                    {
                        imgQuestionMapArrow.Visible = true;
                    }

                    Control control = null;
                    #region codeToGenerateColumns
                    if (fieldColumnType == "Date" || fieldColumnType == "System.DateTime")
                    {
                        Panel pnlDateCtr = new Panel();
                        DropDownList ddlPlusMinus = new DropDownList();
                        ddlPlusMinus.ID = "datetimeTxt";
                        ddlPlusMinus.CssClass = "txtDateMapping";
                        ddlPlusMinus.Items.Insert(0, new ListItem("+", "0"));
                        ddlPlusMinus.Items.Insert(1, new ListItem("-", "1"));
                        // txtBoxCtr.Text = "+";
                        ASPxSpinEdit dxNoOfDays = new ASPxSpinEdit();

                        dxNoOfDays.ID = "datetimeSpEdit";
                        dxNoOfDays.Width = new Unit("80px");
                        dxNoOfDays.HelpTextSettings.VerticalAlign = HelpTextVerticalAlign.Middle;
                        dxNoOfDays.HelpText = "Day(s)";
                        dxNoOfDays.HelpTextSettings.Position = HelpTextPosition.Right;
                        dxNoOfDays.MinValue = 0;
                        dxNoOfDays.MaxValue = 365;
                        if (serviceQuestionMapping != null)
                        {
                            if (serviceQuestionMapping.ColumnValue.Contains(","))
                            {
                                int noOfDays = UGITUtility.StringToInt(serviceQuestionMapping.ColumnValue);   // UGITUtility.StringToInt((serviceQuestionMapping.ColumnValue.Split(',')[1]).Split(')')[0]);
                                if (noOfDays == 0)
                                {
                                    dxNoOfDays.Text = string.Empty;
                                    ddlPlusMinus.SelectedIndex = 0;
                                }
                                else if (noOfDays > 0)
                                {
                                    dxNoOfDays.Text = Convert.ToString(noOfDays);
                                    ddlPlusMinus.SelectedIndex = 0;
                                }
                                else
                                {
                                    ddlPlusMinus.SelectedIndex = 1;
                                    dxNoOfDays.Text = Convert.ToString(Math.Abs(noOfDays));
                                }
                            }
                            HiddenField hfQuestionMapID = (HiddenField)(e.Item.FindControl("hfQuestionMapID"));
                            hfQuestionMapID.Value = serviceQuestionMapping.ID.ToString();
                            dxNoOfDays.Text = serviceQuestionMapping.ColumnValue;
                        }
                        pnlDateCtr.Controls.Add(ddlPlusMinus);
                        pnlDateCtr.Controls.Add(dxNoOfDays);
                        control = pnlDateCtr;
                    }
                    else
                    {
                        if (serviceQuestionMapping != null)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(serviceQuestionMapping.ColumnValue))
                                {
                                    if (UGITUtility.IsNumber(serviceQuestionMapping.ColumnValue, out long result))
                                    {
                                        newItem[configField.ColumnName] = serviceQuestionMapping.ColumnValue;
                                    }
                                    else
                                    {
                                        string output = configFieldManager.GetFieldConfigurationIdByName(serviceQuestionMapping.ColumnName, serviceQuestionMapping.ColumnValue, Convert.ToString(row["ModuleName"]));
                                        newItem[configField.ColumnName] = output.Replace("'%", "").Replace("%'", "");
                                        if (string.IsNullOrEmpty(output))
                                        {
                                            newItem[configField.ColumnName] = serviceQuestionMapping.ColumnValue;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex);
                                newItem[configField.ColumnName] = DBNull.Value;
                            }

                            HiddenField hfQuestionMapID = (HiddenField)(e.Item.FindControl("hfQuestionMapID"));
                            hfQuestionMapID.Value = serviceQuestionMapping.ID.ToString();
                        }

                        bool callDefaultCtrGenerator = true;
                        if (fieldColumnType == "Lookup")
                        {
                            LookUpValueBox lookupField = new LookUpValueBox();
                            //lookupField.devexListBox.CssClass = editcss;
                            //lookupField.devexListBox.ClientSideEvents.ValueChanged = "rowClick";
                            lookupField.FieldName = fieldForControl.FieldName;
                            if (taskInfo != null)
                            {
                                lookupField.ModuleName = taskInfo.RelatedModule;
                            }
                            else
                            {
                                lookupField.ModuleName = moduleName;
                            }
                            //lookupField.ID = GenerateID(field, tabId);
                            if (lookupField.IsMulti &&
                                (fieldForControl.FieldName != DatabaseObjects.Columns.DepartmentLookup && fieldForControl.FieldName != DatabaseObjects.Columns.TicketBeneficiaries))
                            {
                                control = GetMultiLookupControl(this.Context, configField, fieldList, newItem);
                                callDefaultCtrGenerator = false;
                            }
                        }

                        if (callDefaultCtrGenerator)
                        {
                            UGITModule aModule = null;
                            if (taskInfo != null)
                            {
                                //SPControlMode mode = SPControlMode.New;  
                                aModule = ObjModuleViewManager.LoadByName(Convert.ToString(taskInfo.RelatedModule));
                            }
                            else
                            {
                                aModule = ObjModuleViewManager.LoadByName(service.ModuleNameLookup);
                            }
                            TicketControls ticketControl = new TicketControls(aModule, newItem);
                            ticketControl.SourceItem = newItem;
                            ModuleFormLayout moduleFormLayout = null;
                            bool isNoteField = configField.ColumnName.EqualsIgnoreCase(DatabaseObjects.Columns.TicketResolutionComments) || configField.ColumnName.EqualsIgnoreCase(DatabaseObjects.Columns.Comment)
                                || configField.ColumnName.EqualsIgnoreCase(DatabaseObjects.Columns.TicketDescription);
                            if (isNoteField)
                            {
                                moduleFormLayout = new ModuleFormLayout();
                                moduleFormLayout.FieldName = configField.ColumnName;
                                moduleFormLayout.TabId = 0;
                                moduleFormLayout.FieldDisplayName = configField.Caption;
                                moduleFormLayout.ColumnType = "NoteField";
                            }
                            control = ticketControl.GetControls(configField, ControlMode.New, FieldDesignMode.Normal, string.Empty, moduleFormLayout);
                        }

                        #region Some controls are not able to pre select value. this code is for those control
                        if (serviceQuestionMapping != null && (fieldColumnType == "Choice" || fieldColumnType == "Boolean" || fieldColumnType == "Lookup"))
                        {
                            MappedControlInfo mappedData = new MappedControlInfo();
                            mappedData.FieldName = fieldName;
                            mappedData.Control = control;
                            mappedData.Value = serviceQuestionMapping.ColumnValue;
                            mappedData.CtrType = fieldColumnType;
                            mappedInfo.Add(mappedData);
                        }
                        if (serviceQuestionMapping == null && fieldColumnType == "UserField")
                        {
                            MappedControlInfo mappedData = new MappedControlInfo();
                            mappedData.FieldName = fieldName;
                            mappedData.Control = control;
                            if (taskInfo != null)
                                mappedData.Value = taskInfo.AssignedTo;
                            mappedData.CtrType = fieldColumnType;
                            mappedInfo.Add(mappedData);
                        }
                        #endregion
                    }
                    #endregion codeToGenerateColumns

                    if (serviceQuestionMapping != null && control != null)
                    {
                        // Need to do control.Controls.Count otherwise control.HasControls() statement does not work sometimes
                        // But need try-catch in case no controls 
                        int controlCount = 0;
                        try
                        {
                            if (control.Controls != null)
                                controlCount = control.Controls.Count;
                        }
                        catch { }

                        if (controlCount > 1 && control.Controls[0] is DropDownList)
                        {
                            DropDownList ddlList = control.Controls[0] as DropDownList;
                            ASPxSpinEdit dxNoDays = control.Controls[1] as ASPxSpinEdit;
                            if (dxNoDays != null && ddlList != null && serviceQuestionMapping.ColumnValue.Contains(","))
                            {
                                int noOfDays = UGITUtility.StringToInt((serviceQuestionMapping.ColumnValue.Split(',')[1]).Split(')')[0]);
                                if (noOfDays == 0)
                                {
                                    dxNoDays.Text = string.Empty;
                                    ddlList.SelectedIndex = 0;
                                }
                                else if (noOfDays > 0)
                                {
                                    dxNoDays.Text = Convert.ToString(noOfDays);
                                    ddlList.SelectedIndex = 0;
                                }
                                else
                                {
                                    ddlList.SelectedIndex = 1;
                                    dxNoDays.Text = Convert.ToString(Math.Abs(noOfDays));
                                }
                                if (string.IsNullOrEmpty(serviceQuestionMapping.PickValueFrom) && !string.IsNullOrWhiteSpace(dxNoDays.Text))
                                    serviceQuestionMapping.PickValueFrom = "Today";
                            }
                            else if (ddlList != null)
                                ddlList.SelectedIndex = ddlList.Items.IndexOf(ddlList.Items.FindByText(serviceQuestionMapping.ColumnValue));
                        }
                        else if ((controlCount > 0 || control.HasControls()) && (control.Controls[0] is LookUpValueBox || control.Controls[0] is UserValueBox))
                        {
                            control.Controls[0].ID += serviceQuestionMapping.ServiceTaskID + "_" + serviceQuestionMapping.ColumnName;

                        }
                        else if (control is DropDownList)
                        {
                            DropDownList ddlList = (DropDownList)control;
                            ddlList.SelectedIndex = ddlList.Items.IndexOf(ddlList.Items.FindByText(serviceQuestionMapping.ColumnValue));
                        }
                        else if (control is ASPxComboBox)
                        {
                            ASPxComboBox combobox = control as ASPxComboBox;
                            //SPFieldLookupValue lLook = new SPFieldLookupValue(serviceQuestionMapping.ColumnValue);
                            //if (lLook != null && lLook.LookupId > 0)
                            //    combobox.SelectedItem = combobox.Items.FindByValue(lLook.LookupId);
                        }
                        HiddenField hfQuestionMapID = (HiddenField)(e.Item.FindControl("hfQuestionMapID"));
                        hfQuestionMapID.Value = serviceQuestionMapping.ID.ToString();
                    }

                    Panel controlPanel = (Panel)(e.Item.FindControl("controlPanel"));
                    if (control != null)
                        controlPanel.Controls.Add(control);

                    //Shows display name of field
                    System.Web.UI.HtmlControls.HtmlTableCell ticketFieldNameObj = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("ticketFieldName");
                    ticketFieldNameObj.Controls.Add(new LiteralControl(Convert.ToString(row[DatabaseObjects.Columns.FieldDisplayName])));
                    bool mandatory = false;
                    if (uHelper.IfColumnExists(DatabaseObjects.Columns.FieldMandatory, row.DataView.Table))
                        mandatory = UGITUtility.StringToBoolean(row[DatabaseObjects.Columns.FieldMandatory]);
                    if (mandatory)
                        ticketFieldNameObj.Controls.Add(new LiteralControl("<b style='color:red;padding-left:2px;vertical-align: middle;'>*</b>"));
                    if (configField.ColumnName == DatabaseObjects.Columns.Approver)
                    {
                        ticketFieldNameObj.Controls.Clear();
                        ticketFieldNameObj.Controls.Add(new LiteralControl(string.Format("{0}<br /><b style='color:Gray;float:left;padding-top:3px;'>Skips approval if left blank</b>", Convert.ToString(row[DatabaseObjects.Columns.FieldDisplayName]))));
                    }

                    HiddenField hfFieldInternalName = (HiddenField)(e.Item.FindControl("hfFieldInternalName"));
                    hfFieldInternalName.Value = configField.ColumnName;

                    #region fill question dropdown for each mapped field
                    List<ServiceQuestion> questions = service.Questions.OrderBy(x => x.TokenName).ToList();
                    List<ServiceQuestion> sVariables = serviceVariables;

                    Panel ucontrolPanel = (Panel)(e.Item.FindControl("ctpUserCollection"));
                    TextBox txtUserCollection = new TextBox();

                    #region Picks questions and variable based on types
                    if (configField.ColumnName == DatabaseObjects.Columns.TicketRequestTypeLookup)
                    {
                        questions = questions.Where(x => x.QuestionType.ToLower() == "requesttype").ToList();

                        List<ServiceQuestion> qs = new List<ServiceQuestion>();
                        if (questions.Count > 0)
                        {
                            //Filter out only related request type questions
                            //UGITModule module = ObjModuleViewManager.GetByID(previousSTicketID);
                            moduleName = string.Empty;
                            string tModuleName = string.Empty;
                            if (taskInfo != null && taskInfo.RelatedModule != null)
                            {
                                moduleName = taskInfo.RelatedModule;
                            }
                            else if (!string.IsNullOrEmpty(service.ModuleNameLookup))
                            {
                                UGITModule moduleRow = ObjModuleViewManager.GetByName(service.ModuleNameLookup);
                                if (moduleRow != null)
                                {
                                    moduleName = moduleRow.ModuleName;
                                }
                            }
                            foreach (ServiceQuestion q in questions)
                            {
                                q.QuestionTypePropertiesDicObj.TryGetValue("module", out tModuleName);
                                if (tModuleName == moduleName)
                                {
                                    qs.Add(q);
                                }
                            }
                            questions = qs;
                        }

                    }
                    else if (fieldColumnType == "System.DateTime")
                    {
                        questions = questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME).ToList();
                        sVariables = sVariables.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME).ToList();
                    }
                    else if (fieldColumnType == "UserField")
                    {
                        questions = questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD).ToList();
                        sVariables = sVariables.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD).ToList();
                        //for textbox if question mapped value is not defalut only for userfield type question
                        txtUserCollection.ID = "txtUserCollection";
                        txtUserCollection.CssClass = "ccsusercollection doubleWidth";
                        ucontrolPanel.Controls.Add(txtUserCollection);
                    }
                    #endregion

                    #region Binds questions and variables to dropdown
                    DropDownList questionDropDown = (DropDownList)(e.Item.FindControl("ddlQuestionMapped"));
                    string dText = string.Empty;
                    foreach (ServiceQuestion question in questions)
                    {
                        dText = string.Format("{0}", question.QuestionTitle);
                        if (!string.IsNullOrEmpty(question.TokenName))
                            dText = string.Format("[${1}$] {0}", question.QuestionTitle, question.TokenName);

                        questionDropDown.Items.Add(new ListItem(dText, question.ID.ToString()));
                    }
                    #endregion

                    #region Add variables(Custom or inbuilt) in dropdown
                    if (sVariables != null && sVariables.Count > 0)
                    {
                        ListItem variableItem = new ListItem("--Variables--", string.Empty);
                        variableItem.Attributes.Add("disabled", "disabled");
                        questionDropDown.Items.Add(variableItem);
                        foreach (ServiceQuestion question in questions)
                        {
                            dText = string.Format("{0} Manager", question.QuestionTitle);
                            if (!string.IsNullOrEmpty(question.TokenName))
                                dText = string.Format("{0}[${2}~Manager$] {1}", HttpUtility.HtmlDecode("&nbsp;&nbsp;"), dText, question.TokenName);

                            questionDropDown.Items.Add(new ListItem(dText, question.TokenName + "~Manager"));
                        }
                        foreach (ServiceQuestion qVariable in sVariables)
                        {
                            variableItem = new ListItem(string.Format("{0}[${1}$] {2}", HttpUtility.HtmlDecode("&nbsp;&nbsp;"), qVariable.TokenName, qVariable.QuestionTitle), qVariable.TokenName);
                            questionDropDown.Items.Add(variableItem);
                        }
                    }
                    questionDropDown.Items.Insert(0, new ListItem("--None or Default--", string.Empty));
                    #endregion

                    #endregion

                    if (serviceQuestionMapping != null)
                    {
                        questionDropDown.SelectedIndex = questionDropDown.Items.IndexOf(questionDropDown.Items.FindByValue(serviceQuestionMapping.PickValueFrom));
                    }

                    questionDropDown.Attributes.Add("onchange", "fieldMapToQuestion(this)");
                    questionDropDown.CssClass = "donthide ddlquestionmapped";
                    if (fieldColumnType != "System.String" && fieldColumnType != "Date" && fieldColumnType != "DateTime" && fieldColumnType != "System.DateTime" && fieldColumnType != "NoteField")
                    {
                        questionDropDown.CssClass = string.Empty;
                        if (questionDropDown.SelectedValue != string.Empty && questionDropDown.SelectedIndex > 0)
                        {
                            if (serviceQuestionMapping != null)
                            {
                                txtUserCollection.Text = serviceQuestionMapping.ColumnValue;
                                HiddenField hfQuestionMapID = (HiddenField)(e.Item.FindControl("hfQuestionMapID"));
                                hfQuestionMapID.Value = serviceQuestionMapping.ID.ToString();
                            }
                            controlPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
                            ucontrolPanel.Style.Add(HtmlTextWriterStyle.Display, "block");

                        }
                        else
                        {
                            txtUserCollection.Text = string.Empty;
                            controlPanel.Style.Add(HtmlTextWriterStyle.Display, "block");
                            ucontrolPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
                        }
                    }
                }
            }
        }

        protected void BtSaveQuestionMapping_Click(object sender, EventArgs e)
        {
            RepeaterItemCollection repeaterItems = rItemMapping.Items;
            foreach (RepeaterItem rItem in repeaterItems)
            {
                string value = string.Empty;
                Panel controlPanel = (Panel)rItem.FindControl("controlPanel");
                Panel ucontrolPanel = (Panel)rItem.FindControl("ctpUserCollection");
                DropDownList ddlQuestionMapped = (DropDownList)rItem.FindControl("ddlQuestionMapped");
                int questionMapID = 0;
                HiddenField hfQuestionMapID = (HiddenField)(rItem.FindControl("hfQuestionMapID"));
                if (hfQuestionMapID != null)
                {
                    int.TryParse(hfQuestionMapID.Value, out questionMapID);
                }
                if (controlPanel.Controls.Count <= 0)
                {
                    continue;
                }
                if (Category.ToLower() == Constants.ModuleAgent.ToLower())
                {
                    CheckBox chkbxMapping = (CheckBox)rItem.FindControl("chkbxMapping");
                    if (!chkbxMapping.Checked)
                    {
                        ServiceQuestionMappingManager mappingManager = new ServiceQuestionMappingManager(HttpContext.Current.GetManagerContext());
                        List<ServiceQuestionMapping> lstServiceQuestionMapping = new List<ServiceQuestionMapping>();
                        ServiceQuestionMapping serviceQuestionMapping = service.QuestionsMapping.FirstOrDefault(x => x.ID == questionMapID);
                        if (serviceQuestionMapping != null)
                        {
                            lstServiceQuestionMapping.Add(serviceQuestionMapping);
                            mappingManager.Delete(lstServiceQuestionMapping);
                            service.QuestionsMapping.Remove(serviceQuestionMapping);
                        }
                        continue;
                    }
                }
                string pickValueFrom = string.Empty;
                int questionID = 0;
                if (ddlQuestionMapped != null)
                {
                    pickValueFrom = ddlQuestionMapped.SelectedValue;
                    int.TryParse(pickValueFrom, out questionID);
                }
                ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
                bool isDefaultSelectedPicker = serviceQuestionManager.IsDefaultTokenPicked(ddlQuestionMapped.SelectedValue);

                bool dontHideDefaultValCtr = false;
                Control valCtr;
                if (controlPanel.Controls[0].Controls.Count > 0)
                    valCtr = controlPanel.Controls[0].Controls[0];
                else
                    valCtr = controlPanel.Controls[0];

                if (valCtr is LookUpValueBox)
                {
                    LookUpValueBox ddlList = (LookUpValueBox)valCtr;

                    if (ddlList.GetValues() != "" && ddlList.GetValues() != "0" && ddlList.GetValues() != "(None)")
                    {
                        value = ddlList.GetValues();
                    }
                }
                else if (valCtr is ASPxComboBox)
                {
                    ASPxComboBox ddlList = (ASPxComboBox)valCtr;
                    if (ddlList.SelectedItem != null)
                    {
                        if (ddlList.SelectedItem.Value != null && ddlList.SelectedItem.Text != "0" && ddlList.SelectedItem.Text != "(None)")
                        {
                            value = ddlList.SelectedItem.Text;
                        }
                    }
                }
                else if (valCtr is TextBox)
                {

                    TextBox fieldObj = (TextBox)controlPanel.Controls[0].Controls[0];
                    value = Convert.ToString(fieldObj.Text);

                    if (value.Trim() == string.Empty)
                    {
                        fieldObj.Text = "[$Current$]";
                        value = "[$Current$]";
                    }
                    dontHideDefaultValCtr = true;
                }
                else if (valCtr is ASPxTextBox)
                {
                    ASPxTextBox fieldObj = (ASPxTextBox)controlPanel.Controls[0].Controls[0];
                    value = Convert.ToString(fieldObj.Text);

                    if (value.Trim() == string.Empty)
                    {
                        fieldObj.Text = "[$Current$]";
                        value = "[$Current$]";
                    }
                    dontHideDefaultValCtr = true;
                }
                else if (valCtr is UserValueBox)
                {
                    if (ddlQuestionMapped.SelectedIndex == 0)
                    {
                        UserValueBox fieldObj = (UserValueBox)valCtr;
                        value = Convert.ToString(fieldObj.GetValues());
                    }
                    else
                    {
                        ServiceQuestionMapping quesmap = service.QuestionsMapping.FirstOrDefault(x => x.ID == questionMapID);
                        if (quesmap != null && ucontrolPanel.Controls.Count > 0)
                        {
                            TextBox fieldObj = (TextBox)ucontrolPanel.FindControl("txtUserCollection");
                            if (fieldObj != null)
                            {
                                value = Convert.ToString(fieldObj.Text);
                                List<string> validToken = value.Split(new string[] { "$]", "[$" }, StringSplitOptions.None).ToList();
                                StringBuilder strjoin = new StringBuilder();
                                foreach (string str in validToken)
                                {
                                    ServiceQuestion valid = serviceVariables.FirstOrDefault(x => x.TokenName == str && x.QuestionType == Constants.ServiceQuestionType.USERFIELD);

                                    ListItem item = ddlQuestionMapped.Items.Cast<ListItem>().Where(x => x.Text.Contains(string.Format("[${0}$]", str))).FirstOrDefault();

                                    if (item == null && value.Contains(string.Format("[${0}$]", str)) && str.ToLower() != "current")
                                        value = value.Replace(string.Format("[${0}$]", str), "");

                                }

                                if (value.Trim() == string.Empty)
                                {
                                    fieldObj.Text = "[$Current$]";
                                    value = "[$Current$]";
                                }
                            }
                        }
                    }
                    dontHideDefaultValCtr = false;
                }
                else if (valCtr is DropDownList)
                {
                    DropDownList ddlPlusMinus = controlPanel.Controls[0].Controls[0] as DropDownList;

                    // Added below code to fix, SPR 293	SVC New Project Request: Target Completion Date is mapped and populated, but not showing on NPR created by SVC
                    if (controlPanel.Controls[0].Controls.Count > 1)
                    {
                        if (controlPanel.Controls[0].Controls[1] is ASPxSpinEdit)
                        {
                            ASPxSpinEdit dxNoDays = controlPanel.Controls[0].Controls[1] as ASPxSpinEdit;
                            if (dxNoDays != null && ddlPlusMinus != null)
                            {
                                if (ddlQuestionMapped.SelectedIndex == 0 && !string.IsNullOrEmpty(dxNoDays.Text))
                                {
                                    value = string.Format("f:adddays({0},{1})", "[$Today$]", ddlPlusMinus.SelectedItem.Text + dxNoDays.Text);
                                }
                                else
                                    value = string.Format("f:adddays({0},{1})", "[$Current$]", ddlPlusMinus.SelectedItem.Text + dxNoDays.Text);
                            }
                        }
                    }
                    else
                    {
                        value = Convert.ToString(ddlPlusMinus.SelectedItem.Text);
                    }
                }
                else if (valCtr is DepartmentDropdownList)
                {
                    DepartmentDropdownList departmentCtr = (DepartmentDropdownList)valCtr;
                    value = departmentCtr.Value;
                }
                //else if (valCtr is UGITDropDown)
                //{
                //    UGITDropDown fieldObj = (UGITDropDown)controlPanel.Controls[0];
                //    value = Convert.ToString(fieldObj.DropDown.Value);
                //}
                //else if (valCtr is UGITListBox)
                //{
                //    UGITListBox fieldObj = (UGITListBox)controlPanel.Controls[0];
                //    List<object> lstIssueTypes = fieldObj.DropDown.GridView.GetSelectedFieldValues(fieldObj.ChoiceFieldName);
                //    if (lstIssueTypes.Count == 0)
                //        value = Convert.ToString(fieldObj.DropDown.Value);
                //    else
                //        value = string.Join(Constants.Separator, lstIssueTypes);
                //}
                else if (valCtr is ASPxComboBox)
                {
                    ASPxComboBox combobox = controlPanel.Controls[0] as ASPxComboBox;
                    if (combobox != null && !string.IsNullOrEmpty(combobox.Text))
                        value = string.Format("{0}{1}{2}", combobox.Value, Constants.Separator, combobox.Text);
                }
                //else if (valCtr is ASPxGridLookup)
                //{
                //    ASPxGridLookup fieldObj = (ASPxGridLookup)controlPanel.Controls[0];
                //    List<object> selectedVals = fieldObj.GridView.GetSelectedFieldValues(DatabaseObjects.Columns.Id);
                //    List<string> values = new List<string>();
                //    foreach (object val in selectedVals)
                //    {
                //        if (!string.IsNullOrEmpty(Convert.ToString(val)))
                //        {
                //            values.Add(string.Format("{0}{1}", val, Constants.Separator));
                //        }
                //    }
                //    value = string.Join(Constants.Separator, values.ToArray());
                //}
                else if (valCtr is Panel)
                {
                    DropDownList ddlPlusMinus = valCtr.Controls[0] as DropDownList;
                    ASPxSpinEdit dxNoDays = valCtr.Controls[1] as ASPxSpinEdit;
                    if (dxNoDays != null && ddlPlusMinus != null)
                    {

                        if (ddlQuestionMapped.SelectedIndex == 0 && !string.IsNullOrEmpty(dxNoDays.Text))
                        {

                            value = string.Format("f:adddays({0},{1})", "[$Today$]", ddlPlusMinus.SelectedItem.Text + dxNoDays.Text);
                        }
                        else
                            value = string.Format("f:adddays({0},{1})", "[$Current$]", ddlPlusMinus.SelectedItem.Text + dxNoDays.Text);

                    }

                }
                if (valCtr is LookupValueBoxEdit)
                {
                    LookupValueBoxEdit ddlList = valCtr as LookupValueBoxEdit;

                    if (ddlList.GetValues() != "" && ddlList.GetValues() != "0" && ddlList.GetValues() != "(None)")
                    {
                        value = ddlList.GetValues();
                    }
                }

                int serviceTaskID = 0;
                int.TryParse(ddlTaskTickets.SelectedValue, out serviceTaskID);

                string columnName = string.Empty;
                HiddenField hfFieldInternalName = (HiddenField)(rItem.FindControl("hfFieldInternalName"));
                if (hfFieldInternalName != null)
                {
                    columnName = hfFieldInternalName.Value.Trim();
                }

                //Some controls are not able to pre select value. this code is for those control
                MappedControlInfo exitingMappedCtr = mappedInfo.FirstOrDefault(x => x.FieldName == columnName);
                if (exitingMappedCtr != null)
                {
                    exitingMappedCtr.Value = value;
                }

                if (!string.IsNullOrEmpty(columnName))
                {
                    ServiceQuestionMappingManager mappingManager = new ServiceQuestionMappingManager(HttpContext.Current.GetManagerContext());
                    ServiceQuestionMapping questionMap = service.QuestionsMapping.FirstOrDefault(x => x.ID == questionMapID);
                    if (questionMap == null)
                    {
                        questionMap = new ServiceQuestionMapping();
                        questionMap.ServiceID = service.ID;
                        if (serviceTaskID == 0)
                            questionMap.ServiceTaskID = null;
                        else
                            questionMap.ServiceTaskID = serviceTaskID;


                        service.QuestionsMapping.Add(questionMap);
                    }

                    questionMap.ColumnName = columnName;
                    questionMap.ColumnValue = value;
                    questionMap.PickValueFrom = pickValueFrom;
                    //questionMap.ServiceQuestionID = questionID;
                    if (!string.IsNullOrEmpty(Convert.ToString(questionID)) && questionID > 0)
                        questionMap.ServiceQuestionID = questionID;
                    mappingManager.Save(questionMap);
                    hfQuestionMapID.Value = questionMap.ID.ToString();
                }

                if (!dontHideDefaultValCtr)
                {
                    ddlQuestionMapped.CssClass = string.Empty;
                    if (ddlQuestionMapped.SelectedValue != string.Empty && ddlQuestionMapped.SelectedIndex > 0)
                    {
                        controlPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
                        ucontrolPanel.Style.Add(HtmlTextWriterStyle.Display, "block");
                    }
                    else
                    {
                        controlPanel.Style.Add(HtmlTextWriterStyle.Display, "block");
                        ucontrolPanel.Style.Add(HtmlTextWriterStyle.Display, "none");
                    }
                }
            }
            if (Category.ToLower() == Constants.ModuleAgent.ToLower())
                BindQuestionMapping(hfMapPreviousTaskID.Value);




            //Close popup when save and close button click
            if (UGITUtility.StringToBoolean(hfSaveAndClose.Value))
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            else
            {
                if (tabMenu.ActiveTab != null && tabMenu.ActiveTab.Index == 4)
                {
                    Context.Cache.Add(string.Format("SVCConfigMapRefresh-{0}", User.Id), ddlTaskTickets.SelectedValue, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                    Response.Redirect(UGITUtility.GetAbsoluteURL(Request.Url.PathAndQuery));
                }
            }

        }

        private void BindDropdownWithTaskTickets()
        {
            string selectedVal = selectedMappingTaskID;

            if (hfPreTab.Value != hfCurrentTab.Value)
                ddlTaskTickets.Items.Clear();

            if (service != null && service.Tasks != null)
            {
                string format = string.Empty;
                List<UGITTask> tasks = service.Tasks;
                foreach (UGITTask task in tasks)
                {
                    if (task.RelatedModule != null)
                    {
                        format = string.Format("{0}: ", task.RelatedModule);
                    }
                    format = format + task.Title;
                    ddlTaskTickets.Items.Add(new ListItem(format, task.ID.ToString()));
                    format = string.Empty;
                }

                if (service.CreateParentServiceRequest && ddlTaskTickets.Items.FindByValue("0") == null)
                    ddlTaskTickets.Items.Insert(0, new ListItem("SVC: Service Instance", "0"));
            }

            if (!string.IsNullOrWhiteSpace(selectedVal))
            {
                ddlTaskTickets.SelectedIndex = ddlTaskTickets.Items.IndexOf(ddlTaskTickets.Items.FindByValue(selectedVal));
            }
        }

        protected void DDLTaskTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedMappingTaskID = ddlTaskTickets.SelectedValue;
            hfMapPreviousTaskID.Value = selectedMappingTaskID;
            BindQuestionMapping(selectedMappingTaskID);
        }

        #endregion

        protected void btDeleteQuestion_Click(object sender, ImageClickEventArgs e)
        {
            ApplicationContext _context = HttpContext.Current.GetManagerContext();
            ServiceQuestionManager objServiceQuestionManager = new ServiceQuestionManager(_context);
            //First remove all reference from child tables
            ServiceQuestionMappingManager questMappingObj = new ServiceQuestionMappingManager(_context);
            ImageButton button = (ImageButton)sender;
            int id = 0;
            int.TryParse(button.CommandArgument, out id);
            //Delete Default Value mapping
            List<ServiceQuestionMapping> questionMapping = questMappingObj.Load(x => x.ServiceQuestionID == id);
            if (questionMapping != null && questionMapping.Count > 0)
                questMappingObj.Delete(questionMapping);
            ServiceQuestion objServiceQuestion = objServiceQuestionManager.LoadByID(id);
            if (objServiceQuestion != null)
                objServiceQuestionManager.Delete(objServiceQuestion);

            service = serviceManager.LoadByServiceID(service.ID);
            tabMenuActiveTabChanged(tabMenu, new TabControlEventArgs(tabMenu.ActiveTab));
        }

        protected void btDeleteSection_Click(object sender, ImageClickEventArgs e)
        {
            ServiceSectionManager objServiceSectionManager = new ServiceSectionManager(HttpContext.Current.GetManagerContext());
            ImageButton button = (ImageButton)sender;
            int id = 0;
            int.TryParse(button.CommandArgument, out id);
            if (service != null)
            {
                ServiceSection section = service.Sections.FirstOrDefault(x => x.ID == id); // objServiceSectionManager.LoadByID(id);
                if (section != null)
                {
                    List<ServiceQuestion> questionList = service.Questions.Where(x => x.ServiceSectionID == section.ID).ToList();
                    foreach (ServiceQuestion q in questionList)
                    {
                        ServiceQuestionManager objServiceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
                        objServiceQuestionManager.Delete(q);
                    }
                    objServiceSectionManager.Delete(section);
                }
            }

            service = serviceManager.LoadByServiceID(service.ID);
            tabMenuActiveTabChanged(tabMenu, new TabControlEventArgs(tabMenu.ActiveTab));
        }
        protected void rtSubTickets_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            if (e.CommandName == "DecIndent")
            {
                int taskID = int.Parse(e.CommandArgument.ToString());
                if (taskID > 0)
                {
                    DecIndent(taskID);
                }
            }
            else if (e.CommandName == "IncIndent")
            {
                int taskID = int.Parse(e.CommandArgument.ToString());
                if (taskID > 0)
                {
                    IncIndent(taskID);

                }
            }

        }

        protected string GetTitleHtmlData(bool withLink)
        {
            StringBuilder data = new StringBuilder();

            int id = UGITUtility.StringToInt(Eval(DatabaseObjects.Columns.Id));
            int childCount = service.Tasks.Where(x => x.ParentTaskID == id).ToList().Count;
            data.Append("<div class=\"task-title\" style=\"float:left;");
            if (childCount > 0)
                data.Append("font-weight:bold;");
            data.Append("\" >");

            string cssChanges = "color:#000;";
            if (childCount > 0)
            {
                data.AppendFormat("<img id='grpImage' style='float:left;padding-right:2px;' src='/Content/images/minimise.gif' colexpand='true' onclick=\"CloseChildren('{0}' , '{1}', this)\" />", Eval(DatabaseObjects.Columns.ItemOrder), Eval("ID"));
            }



            if (withLink)
            {
                //string[] arr = Convert.ToString(Eval("Title")).Replace('(', ' ').Replace(')', ' ').Split(new string[] { "Sprint:" }, StringSplitOptions.RemoveEmptyEntries);
                data.AppendFormat("<a href='javascript:void(0);' onclick='editTask(false,{1},\"{3}\")'><span style='padding-top:1px;\"{2}\"'>{0}</span></a> ", Eval("Title"), Eval("ID"), cssChanges, /*(SPFieldLookupValue)Eval("Module")*/ null != null ? "ticket" : "task");
                //if (arr.Length > 1)
                //{
                //    data.AppendFormat("<a href='javascript:void(0);' onclick='openSprintTask(\"{0}\")'><span style='padding-top:1px;{1}'>(Sprint:{0})</span></a>", arr[1].Trim(), cssChanges);
                //}

                // data.AppendFormat("<a href='javascript:void(0);' onclick='editTask({1},{3})'><span style='padding-top:1px;{2}'>{0}</span></a>", Eval("Title"), Eval("ID"), cssChanges, Eval("ItemOrder"));
            }
            else
                data.AppendFormat("<span style='padding-top:1px;{1}'>{0}</span>", Eval("Title"), cssChanges);
            data.Append("</div>");

            return data.ToString();
        }

        private void LoadService(bool reload)
        {
            if (service != null && !reload)
            {
                int sID = GetServiceID();
                if (sID > 0)
                {
                    //service = Services.LoadByID(sID);
                }
            }
        }

        private void IncIndent(int tskTaskID)
        {

            LoadService(false);

            if (service == null)
            {
                return;
            }


            UGITTask childTask = service.Tasks.FirstOrDefault(x => x.ID == tskTaskID);
            int childIndex = service.Tasks.IndexOf(childTask);
            UGITTask parentTask = null;
            for (int i = childIndex - 1; i >= 0; i--)
            {
                if (service.Tasks[i].Level == childTask.Level)
                {
                    parentTask = service.Tasks[i];
                    break;
                }
                else if (service.Tasks[i].Level < childTask.Level)
                {
                    parentTask = null;
                    break;
                }
            }

            long parentTaskID = 0;
            if (parentTask != null)
            {
                parentTaskID = parentTask.ID;
                childTask.Level = parentTask.Level + 1;
                childTask.ParentTaskID = parentTaskID;
                //childTask.Save();
            }

            //if (service.Tasks.Where(x => x.ParentTask == tskTaskID).ToList().Count > 0)
            //    ServiceTask.IncreaseLevel(service.Tasks, tskTaskID);

            //ServiceTask.SaveLevel(service.Tasks);
            GenerateSubTicketsList();
        }
        private void DecIndent(int tskTaskID)
        {
            LoadService(false);
            if (service == null)
            {
                return;
            }

            UGITTask childTask = service.Tasks.FirstOrDefault(x => x.ID == tskTaskID);
            if (childTask.Level > 0)
            {
                int childIndex = service.Tasks.IndexOf(childTask);
                UGITTask parentTask = null;
                for (int i = childIndex - 1; i >= 0; i--)
                {
                    if (service.Tasks[i].Level == childTask.Level - 1)
                    {
                        parentTask = service.Tasks[i];
                        break;
                    }
                }

                long parentTaskID = 0;
                if (parentTask != null)
                {
                    parentTaskID = parentTask.ParentTaskID;

                }
                childTask.ParentTaskID = parentTaskID;
                if (childTask.Level > 0)
                {
                    childTask.Level -= 1;
                    childTask.ItemOrder = service.Tasks.Where(x => x.ParentTaskID == parentTaskID).Max(x => x.ItemOrder);
                }
                else
                    childTask.Level = 0;
                //childTask.Save();

                //ServiceTask.ReduceLevel(service.Tasks, tskTaskID);
                //ServiceTask.SaveLevel(service.Tasks);
                //ServiceTask.SaveOrder(service.Tasks);
                GenerateSubTicketsList();
            }
        }

        protected string GetTaskType()
        {
            string type = null;

            //if (!string.IsNullOrEmpty(Convert.ToString((SPFieldLookupValue)Eval("Module"))))
            //{
            //    var ar = UGITUtility.SplitString(Convert.ToString((SPFieldLookupValue)Eval("Module")), Constants.Separator);
            //    type = ar[1];

            //    DataRow row = uGITCache.GetModuleDetails(type);
            //    if (row != null)
            //        type = Convert.ToString(row[DatabaseObjects.Columns.Title]);
            //}
            //else if (!string.IsNullOrEmpty(Convert.ToString(Eval(DatabaseObjects.Columns.UGITSubTaskType))))
            //{
            //    type = Convert.ToString(Eval(DatabaseObjects.Columns.UGITSubTaskType));
            //}
            //else { type = "Task"; }

            return type;
        }

        protected void ReOrder_Click(object sender, EventArgs e)
        {
            string idsInNewOrder = Request[order.UniqueID];
            List<string> newTasksOrder = UGITUtility.ConvertStringToList(idsInNewOrder, new string[] { Constants.Separator });

            string changedOrder = Request[changeOrder.UniqueID];
            List<string> movedIds = UGITUtility.ConvertStringToList(changedOrder, new string[] { Constants.Separator });

            LoadService(false);



            //Add childtask in new parent if there is any parent exist
            if (movedIds.Count == 2)
            {
                int afterTaskID = 0;
                int childTaskID = 0;
                int.TryParse(movedIds[0], out afterTaskID);
                int.TryParse(movedIds[1], out childTaskID);
                if (childTaskID > 0)
                {
                    UGITTask child = service.Tasks.FirstOrDefault(x => x.ID == childTaskID);
                    UGITTask afterTask = service.Tasks.FirstOrDefault(x => x.ID == afterTaskID);
                    List<UGITTask> coC = service.Tasks.Where(x => x.ParentTaskID == childTaskID).ToList();
                    if (coC.Count > 0)
                    {
                        foreach (UGITTask ctr in coC)
                        {
                            //ctr.ParentTask = child.ParentTask;
                            //ctr.TaskLevel = service.Tasks.Where(x => x.ID == child.ParentTask).ToList().Count > 0 ? service.Tasks.FirstOrDefault(x => x.ID == child.ParentTask).TaskLevel + 1 : 0;
                            //ctr.Save();
                        }
                    }

                    //ServiceTask.ReduceLevel(service.Tasks, childTaskID);
                    //ServiceTask.SaveLevel(service.Tasks);


                    if (afterTask != null)
                    {
                        if (service.Tasks.Where(x => x.ParentTask == afterTask.ParentTask).ToList().Count == 0)
                        {
                            child.ParentTaskID = afterTaskID;
                            child.Level = afterTask != null ? afterTask.Level + 1 : 0;
                        }
                        else
                        {
                            UGITTask test = service.Tasks.FirstOrDefault(x => x.ID == afterTask.ID);
                            if (service.Tasks.Where(x => x.ParentTaskID == test.ID).ToList().Count == 0)
                            {
                                child.ParentTask = afterTask.ParentTask;
                                child.Level = afterTask != null ? afterTask.Level : 0;
                            }
                            else
                            {
                                child.ParentTask = afterTask.ParentTask;
                                child.Level = afterTask != null ? afterTask.Level + 1 : 0;
                            }

                        }
                    }
                    else
                    {
                        child.ParentTaskID = 0;
                        child.Level = 0;
                    }
                    //child.Save();
                }
            }

            service.Tasks = service.Tasks.OrderBy(x => x.ItemOrder).ToList();
            int tempTaskID = 0;
            UGITTask tempTask = null;
            for (int i = 0; i < newTasksOrder.Count; i++)
            {
                int.TryParse(newTasksOrder[i], out tempTaskID);
                tempTask = service.Tasks.FirstOrDefault(x => x.ID == tempTaskID);
                if (tempTask != null)
                {
                    service.Tasks[service.Tasks.IndexOf(tempTask)].ItemOrder = i + 1;

                }
            }
            //ServiceTask.SaveOrder(service.Tasks);
            GenerateSubTicketsList();
        }

        protected void lnkFullDeleteService_Click(object sender, EventArgs e)
        {
            if (service != null)
            {
                //service.Delete();
                serviceManager.DeleteAll(service.ID);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        private Control GetMultiLookupControl(HttpContext context, DataColumn field, DataTable list, DataRow item)
        {
            Control ctr = new Control();
            //SPFieldLookup muliLookupField = (SPFieldLookup)field;
            //SPList lookupList = SPListHelper.GetSPList(new Guid(muliLookupField.LookupList));
            ASPxGridLookup cbx = new ASPxGridLookup();
            cbx.SelectionMode = GridLookupSelectionMode.Multiple;
            //cbx.ID = "0" + field.Id.ToString().Replace("-", "_");
            cbx.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
            cbx.GridView.Width = 200;
            cbx.Width = new Unit(100, UnitType.Percentage);

            GridViewCommandColumn cbColumn = new GridViewCommandColumn();
            cbColumn.ShowSelectCheckbox = true;
            cbColumn.Width = 20;
            cbx.Columns.Add(cbColumn);

            GridViewDataTextColumn column = new GridViewDataTextColumn();
            //column.FieldName = muliLookupField.LookupField;
            column.Caption = "#";
            cbx.Columns.Add(column);
            cbx.GridView.Settings.ShowColumnHeaders = false;
            cbx.TextFormatString = "{0}";

            cbx.Theme = "DevEx";
            cbx.KeyFieldName = DatabaseObjects.Columns.Id;
            cbx.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            //cbx.Attributes.Add("LookupList", lookupList.Title);
            cbx.Load += cbxmulti_Load;
            cbx.Init += cbxmulti_Init;
            //cbx.CssClass += " " + field.InternalName;
            ctr = cbx;
            return ctr;
        }

        protected static void cbxmulti_Load(object sender, EventArgs e)
        {
            ASPxGridLookup cbx = (ASPxGridLookup)sender;

            List<object> selectedVals = new List<object>();
            if (cbx.DataSource != null)
            {
                selectedVals = cbx.GridView.GetSelectedFieldValues(DatabaseObjects.Columns.Id);
                foreach (object val in selectedVals)
                {
                    cbx.GridView.Selection.SetSelectionByKey(val, true);
                }
            }
        }

        protected static void cbxmulti_Init(object sender, EventArgs e)
        {
            ASPxGridLookup cbx = (ASPxGridLookup)sender;

            List<object> selectedVals = new List<object>();
            if (cbx.DataSource == null)
            {
                //SPList lookupList = SPListHelper.GetSPList(cbx.Attributes["lookupList"]);
                //if (lookupList == null)
                //    return;


                //SPQuery query = new SPQuery();
                //query.Query = "<Where></Where>";
                //DataTable coll = lookupList.GetItems(query).GetDataTable();
                //cbx.DataSource = coll;
                //cbx.DataBind();
            }
        }

        private void BindAgentStages(string moduleID)
        {
            UGITModule ugitModules = ObjModuleViewManager.GetByName(moduleID); //.GetCachedModule(SPContext.Current.Web, Convert.ToString(drModule[DatabaseObjects.Columns.ModuleName]));
            if (ugitModules != null)
            {
                LifeCycle moduleLifeCycle = ugitModules.List_LifeCycles.FirstOrDefault(x => x.ID == 0);

                if (moduleLifeCycle != null && glModuleStages.DataSource == null)
                {
                    lstStages = moduleLifeCycle.Stages;
                    glModuleStages.DataSource = lstStages;
                    glModuleStages.KeyFieldName = DatabaseObjects.Columns.ID;
                    glModuleStages.DataBind();
                }
            }
        }


        protected void tabMenuActiveTabChanged(object source, TabControlEventArgs e)
        {
            tabServiceDetails.Visible = false;
            tabQuestions.Visible = false;
            tabTasks.Visible = false;
            tabMapping.Visible = false;
            tabRating.Visible = false;
            //Services objService;
            string moduleName = string.Empty;
            if (e.Tab.Name == "service")
            {
                defaultTabNumber = 0;
                tabServiceDetails.Visible = true;

                if (GetServiceID() <= 0)
                {
                    return;
                }

                txtServiceName.Text = service.Title;
                txtServiceDescription.Text = service.ServiceDescription;
                categoryDD.SetValues(service.CategoryId.ToString());  // categoryDD.SelectedIndex = categoryDD.Items.IndexOf(categoryDD.Items.FindByValue(service.CategoryId.ToString()));
                cbCreateParentServiceRequest.Checked = service.CreateParentServiceRequest;
                chkIncludeInDefaultData.Checked = service.IncludeInDefaultData;
                //chkbxAttachInChild.Checked = service.AttachmentsInChildTickets;
                txtOrder.Text = service.ItemOrder.ToString();

                if (service.ServiceType == Constants.ModuleFeedback.Replace("~", ""))
                {
                    trCompletionMessage.Visible = true;
                    txtCompletionMessage.Text = "Thank you for your valuable feedback! This helps us improve our quality of service.";
                    if (!string.IsNullOrEmpty(service.CompletionMessage))
                        txtCompletionMessage.Text = service.CompletionMessage;
                }
                else
                    trCompletionMessage.Visible = false;

                cbOwnerApprovalRequired.Checked = service.OwnerApprovalRequired;
                cbOwnerTasksInBackground.Checked = service.AllowServiceTasksInBackground;
                chkbxAttachInChild.Checked = service.AttachmentsInChildTickets;
                cbOwnerTasksInBackground.Style.Add("display", "block");

                ddlTargetType.SelectedIndex = ddlTargetType.Items.IndexOf(ddlTargetType.Items.FindByValue(Convert.ToString(service.NavigationType)));
                SetTargetTypeDependency();
                if (ddlTargetType.SelectedValue == "Wiki")
                {
                    txtWiki.Text = Convert.ToString(service.NavigationUrl);
                }

                if (ddlTargetType.SelectedValue == "Link")
                {
                    txtFileLink.Text = Convert.ToString(service.NavigationUrl);
                }

                if (ddlTargetType.SelectedValue == "File")
                {
                    var attachments = service.Attachments;

                    string fileName = Path.GetFileName(Convert.ToString(service.NavigationUrl));

                    lblUploadedFile.Text = fileName;

                }
                if (ddlTargetType.SelectedValue == "HelpCard")
                {
                    txtHelpCard.Text = Convert.ToString(service.NavigationUrl);
                }
                //txtWiki.Text = service.NavigationUrl;
                fileUploadIcon.SetImageUrl(service.ImageUrl); //txtServiceIcon.Text = service.ImageUrl;
                chkhidesummary.Checked = service.HideSummary;
                chkHideThankYouScreen.Checked = service.HideThankYouScreen;
                chkDisableSLA.Checked = service.SLADisabled;
                chkTaskReminders.Checked = service.EnableTaskReminder;
                chkUse24x7.Checked = service.Use24x7Calendar;
                cbStartResolutionSLAFromStart.Checked = service.StartResolutionSLAFromAssigned;

                if (!string.IsNullOrEmpty(service.Reminders))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(service.Reminders);
                    TaskReminderProperties reminderProperties = new TaskReminderProperties();
                    reminderProperties = (TaskReminderProperties)uHelper.DeSerializeAnObject(doc, reminderProperties);

                    chkReminder1.Checked = reminderProperties.Reminder1;
                    FillDayHourAndMinutesControls(reminderProperties.Reminder1Duration, txtReminder1Duration, ddlReminder1Unit, service.Use24x7Calendar);
                    ddlReminder1Frequency.SelectedValue = reminderProperties.Reminder1Frequency;
                    chkReminder2.Checked = reminderProperties.Reminder2;

                    FillDayHourAndMinutesControls(reminderProperties.Reminder2Duration, txtReminder2Duration, ddlReminder2Unit, service.Use24x7Calendar);
                    ddlReminder2Frequency.SelectedValue = reminderProperties.Reminder2Frequency;
                }
                FillSLADayHourAndMinutesFormat(service.ResolutionSLA, txtResolutionSLA, ddlResolutionSLAType);
                chkbxShowDefaultvalAgent.Checked = service.LoadDefaultValue;
                if (!cbCreateParentServiceRequest.Checked)
                {
                    //btNewTask.Visible = false;
                    dvTaskDisclaimer.Visible = true;
                    lbTaskDisclaimer.Visible = true;
                }

                peServiceOwner.SetValues(service.OwnerUser);
                peAuthorizedToRun.SetValues(service.AuthorizedToView);

                if (service.SLAConfig != null)
                {
                    chkEnableEscalation.Checked = service.SLAConfig.EnableEscalation;
                    FillDayHourAndMinutesControls(service.SLAConfig.EscalationAfter, txtEscalationAfter, ddlEscalationAfter, service.Use24x7Calendar);
                    FillDayHourAndMinutesControls(service.SLAConfig.EscalationFrequency, txtEsclationFrequency, ddlEsclationFrequency, service.Use24x7Calendar);

                    if (!string.IsNullOrEmpty(service.SLAConfig.EscalationTo))
                        pplEscalationTo.SetValues(service.SLAConfig.EscalationTo);

                    txtEmail.Text = service.SLAConfig.EscalationEmailTo;
                    ddlEscaltionUnit.SelectedIndex = ddlEscaltionUnit.Items.IndexOf(ddlEscaltionUnit.Items.FindByValue(service.SLAConfig.EscalationUnit));
                }

                lnkAttachmentConditionSpan.Style.Add(HtmlTextWriterStyle.Display, "none");
                ListItem attachmentOption = ddlAttachmentRequired.Items.Cast<ListItem>().FirstOrDefault(x => x.Value.ToLower() == service.AttachmentRequired.ToLower());
                if (attachmentOption != null)
                {
                    ddlAttachmentRequired.ClearSelection();
                    attachmentOption.Selected = true;
                    if (attachmentOption.Value == "Conditional")
                        lnkAttachmentConditionSpan.Style.Add(HtmlTextWriterStyle.Display, "block");
                }



                if (!string.IsNullOrEmpty(service.ModuleNameLookup) && service.ModuleNameLookup != "SVCConfig")
                {
                    //ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(service.ModuleId.ToString()));
                    int moduleId = uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), service.ModuleNameLookup);
                    ddlModule.SetValues(Convert.ToString(moduleId));
                }

                if (!string.IsNullOrEmpty(service.ModuleNameLookup) && service.ServiceType == "ModuleFeedback")
                {
                    rdbSurveyType.Visible = true;
                    rdbSurveyType.SelectedIndex = 1;
                    trModule.Visible = true;
                    int moduleId = uHelper.getModuleIdByModuleName(HttpContext.Current.GetManagerContext(), service.ModuleNameLookup);
                    ddlModule.SetValues(Convert.ToString(moduleId));
                }

                if (!string.IsNullOrEmpty(ddlModule.GetValues()))
                {
                    moduleName = ObjModuleViewManager.LoadByID(Convert.ToInt32(ddlModule.GetValues())).ModuleName;
                    BindAgentStages(moduleName);
                }
                List<string> moduleStages = UGITUtility.ConvertStringToList(service.ModuleStage, Constants.Separator);
                LifeCycleManager objLifeCycleHelper = new LifeCycleManager(HttpContext.Current.GetManagerContext());
                if (!string.IsNullOrEmpty(moduleName))
                {
                    LifeCycle obj = objLifeCycleHelper.LoadLifeCycleByModule(moduleName)[0];
                    foreach (string item in moduleStages)
                    {
                        if (obj != null && !IsPostBack)
                        {
                            LifeCycleStage sstage = obj.Stages.FirstOrDefault(x => x.StageStep == Convert.ToInt32(item));
                            glModuleStages.GridView.Selection.SetSelectionByKey(sstage.ID, true);
                            //ddlAgentStages.SelectedIndex = ddlAgentStages.Items.IndexOf(ddlAgentStages.Items.FindByValue(Convert.ToString(item.LookupId)));
                        }
                    }
                }
                chkbxShowStageTranBtns.Checked = service.ShowStageTransitionButtons;


            }
            else if (e.Tab.Name == "questions")
            {
                defaultTabNumber = 2;
                tabQuestions.Visible = true;
                BindQuestionSectionGrid();

            }
            else if (e.Tab.Name == "tasks")
            {
                defaultTabNumber = 3;
                tabTasks.Visible = true;
                GenerateSubTicketsList();

            }
            else if (e.Tab.Name == "mapping")
            {
                defaultTabNumber = 4;
                tabMapping.Visible = true;

                if (IsPostBack)
                    selectedMappingTaskID = Request[hfMapPreviousTaskID.UniqueID];

                BindDropdownWithTaskTickets();
                BindQuestionMapping(selectedMappingTaskID);
            }
            else if (e.Tab.Name == "ratings")
            {
                tabRating.Visible = true;
                BindRatingQuestionGrid();
            }
        }

        protected void cancelServicePopup_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void tabservicePanel_Callback(object sender, CallbackEventArgsBase e)
        {
            if (rdbSurveyType.SelectedIndex == 1)
            {
                trModule.Visible = true;
                ddlModule.devexListBox.ValidationSettings.RequiredField.IsRequired = true;
                ddlModule.devexListBox.ValidationSettings.ValidationGroup = "serviceIntitial";
                ddlModule.devexListBox.ValidationSettings.RequiredField.ErrorText = "Please Select Module Name";
                ddlModule.devexListBox.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
            }
            else
            {
                trModule.Visible = false;
                ddlModule.SetValues(string.Empty);
                ddlModule.devexListBox.Text = string.Empty;
            }
        }
        private void FillSLADayHourAndMinutesFormat(double minutes, TextBox textcontrol, DropDownList dropdown)
        {
            int workingHoursInADay = uHelper.GetWorkingHoursInADay(HttpContext.Current.GetManagerContext(), true);
            if (minutes % (workingHoursInADay * 60) == 0)
            {
                textcontrol.Text = string.Format("{0:0.##}", minutes / (workingHoursInADay * 60));
                dropdown.SelectedValue = Constants.SLAConstants.Days;
            }
            else if (minutes % 60 == 0)
            {
                textcontrol.Text = string.Format("{0:0.##}", minutes / 60);
                dropdown.SelectedValue = Constants.SLAConstants.Hours;
            }
            else
            {
                textcontrol.Text = string.Format("{0:0.##}", minutes);
                dropdown.SelectedValue = Constants.SLAConstants.Minutes;
            }
        }
        private double GetSLAValue(TextBox textcontrol, DropDownList dropdown)
        {
            double value = 0.0;
            // Converting days,hours into minutes
            if (dropdown.SelectedValue == Constants.SLAConstants.Days)
                value = Convert.ToDouble(textcontrol.Text.Trim()) * 60 * uHelper.GetWorkingHoursInADay(HttpContext.Current.GetManagerContext(), true);
            else if (dropdown.SelectedValue == Constants.SLAConstants.Hours)
                value = Convert.ToDouble(textcontrol.Text.Trim()) * 60;
            else
                value = Convert.ToDouble(textcontrol.Text.Trim());

            return value;
        }

        protected void pnlModuleStages_Callback(object sender, CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(e.Parameter))
            {
                int selectedModuleID = Convert.ToInt32(ddlModule.GetValues());
                string moduleName = uHelper.getModuleNameByModuleId(HttpContext.Current.GetManagerContext(), selectedModuleID);
                BindAgentStages(moduleName);
                //glModuleStages.ModuleName = moduleName;
            }
        }

        #region Method to bind Duration textbox and DurationUnit Dropdown
        /// <summary>
        /// This Method is used to bind Duration textbox and DurationUnit Dropdown on the basis of Duration in Minutes
        /// </summary>
        /// <param name="minutes"></param>
        /// <param name="textcontrol"></param>
        /// <param name="dropdown"></param>
        private void FillDayHourAndMinutesControls(double minutes, TextBox textcontrol, DropDownList dropdown, bool use24x7Calendar = false)
        {
            //Minutes can be -/+ now
            minutes = Math.Abs(minutes);
            int workingHoursInADay = uHelper.GetWorkingHoursInADay(context, false);
            if (use24x7Calendar)
                workingHoursInADay = 24;

            if (minutes % (workingHoursInADay * 60) == 0)
            {
                textcontrol.Text = string.Format("{0:0.##}", minutes / (workingHoursInADay * 60));
                dropdown.SelectedValue = Constants.SLAConstants.Days;
            }
            else if (minutes % 60 == 0)
            {
                textcontrol.Text = string.Format("{0:0.##}", minutes / 60);
                dropdown.SelectedValue = Constants.SLAConstants.Hours;
            }
            else
            {
                textcontrol.Text = string.Format("{0:0.##}", minutes);
                dropdown.SelectedValue = Constants.SLAConstants.Minutes;
            }
        }
        #endregion Method to bind Duration textbox and DurationUnit Dropdown

        #region Method to Get Duration in Minutes
        /// <summary>
        /// This method is used to Get Duration in Minutes on the basis of working hours 
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="selectedUnit"></param>
        /// <returns></returns>
        private double GetDurationInMinutes(string duration, string selectedUnit, bool use24x7Calendar = false)
        {
            double value = 0.0;

            if (string.IsNullOrEmpty(duration) || string.IsNullOrEmpty(selectedUnit))
                return value;

            value = uHelper.GetWorkingMinutes(context, duration, selectedUnit, use24x7Calendar);
            return value;
        }
        #endregion Method to Get Duration in Minutes


        private void BindTargetTypeCategories()
        {
            ddlTargetType.Items.Add(new ListItem("File", "File"));
            ddlTargetType.Items.Add(new ListItem("Link", "Link"));
            ddlTargetType.Items.Add(new ListItem("Wiki", "Wiki"));
            ddlTargetType.Items.Add(new ListItem("Help Card", "HelpCard"));
            ddlTargetType.DataBind();
        }


        protected void ddlTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTargetTypeDependency();
        }

        private void SetTargetTypeDependency()
        {
            //trLink.Visible = false;
            //trFileUpload.Visible = false;
            //trWiki.Visible = false;
            //trHelpCard.Visible = false;
            //switch (ddlTargetType.SelectedValue)
            //{
            //    case "File":
            //        trFileUpload.Visible = true;
            //        break;
            //    case "Link":
            //        trLink.Visible = true;
            //        break;
            //    case "Wiki":
            //        trWiki.Visible = true;
            //        break;
            //    case "HelpCard":
            //        trHelpCard.Visible = true;
            //        break;
            //    default:
            //        break;

            //}
        }
    }


}

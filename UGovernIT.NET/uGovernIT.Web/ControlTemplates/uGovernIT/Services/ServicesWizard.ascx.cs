using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using uGovernIT.DefaultConfig;
using uGovernIT.Manager;
using uGovernIT.Util.Cache;
using uGovernIT.Util.Log;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;
using uGovernIT.Web.Helpers;


namespace uGovernIT.Web
{
    public partial class ServicesWizard : UserControl
    {
        public ServicesWizard()
        {
            _serviceInput = new ServiceInput();
        }

        private ServiceCategory serviceCategory;
        private ServiceInput _serviceInput;
        private Services service;

        public string detailsUrl = string.Empty;
        private int currentStepID;
        private long currentStepSectionID;
        private long previousStepSectionID;
        private int startStepID;
        private bool isNextPrevious = false;

        private const int SUMMARYSUBSECTION = -1;
        private List<string> ticketIdsForAgent = new List<string>();

        private bool showApproveButton;
        private bool showRejectButton;
        private bool showReturnButton;
        private string approveButtonName = string.Empty;
        private string rejectButtonName = string.Empty;
        private string returnButtonName = string.Empty;
        private string approveButtonImage = string.Empty;
        private string returnButtonImage = string.Empty;
        private string rejectButtonImage = string.Empty;
        private string ticketId = string.Empty;
        private string moduleName = string.Empty;
        public int summaryColWidth = 6;

        protected ApplicationContext _applicationContext = null;
        private ServicesManager _serviceManager = null;
        private ServiceCategoryManager _serviceCategoryManager = null;
        private ServiceQuestionManager _serviceQuestionManager = null;
        private UserProfileManager _userProfileManager = null;
        private ConfigurationVariableManager _configurationVariableManager = null;
        private ModuleViewManager _moduleViewManager = null;
        private UserProfile User = null;
        private TenantOnBoardingHelper _tenantOnBoardingHelper = null;
        public Boolean TenantExist = false;
        private bool isModuleCall;
        int summaryIteration = 0;
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_applicationContext == null)
                {
                    _applicationContext = HttpContext.Current.GetManagerContext();
                }
                return _applicationContext;
            }
        }

        protected ServicesManager ServicesManager
        {
            get
            {
                if (_serviceManager == null)
                {
                    _serviceManager = new ServicesManager(ApplicationContext);
                }
                return _serviceManager;
            }
        }

        protected ServiceCategoryManager ServiceCategoryManager
        {
            get
            {
                if (_serviceCategoryManager == null)
                {
                    _serviceCategoryManager = new ServiceCategoryManager(ApplicationContext);
                }
                return _serviceCategoryManager;
            }
        }

        protected ServiceQuestionManager ServiceQuestionManager
        {
            get
            {
                if (_serviceQuestionManager == null)
                {
                    _serviceQuestionManager = new ServiceQuestionManager(ApplicationContext);
                }
                return _serviceQuestionManager;
            }
        }

        protected UserProfileManager UserProfileManager
        {
            get
            {
                if (_userProfileManager == null)
                {
                    _userProfileManager = new UserProfileManager(ApplicationContext);
                }
                return _userProfileManager;
            }
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(ApplicationContext);
                }
                return _configurationVariableManager;
            }
        }

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        protected TenantOnBoardingHelper tenantOnBoardingHelper
        {
            get
            {
                if (_tenantOnBoardingHelper == null)
                {
                    _tenantOnBoardingHelper = new TenantOnBoardingHelper(ApplicationContext);
                }
                return _tenantOnBoardingHelper;
            }
        }

        #region Global Events

        protected void Page_Init(object sender, EventArgs e)
        {

            User = HttpContext.Current.CurrentUser();

            if (!string.IsNullOrWhiteSpace(Request[hfQuestionInputs.UniqueID]))
            {
                string questionInputs = Server.HtmlDecode(Request[hfQuestionInputs.UniqueID]);
                if (!string.IsNullOrEmpty(questionInputs))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(questionInputs.Trim());
                    _serviceInput = (ServiceInput)uHelper.DeSerializeAnObject(doc, _serviceInput);
                }
            }

            //Adds attribute in forn element to support upload feature.
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            //if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
            //{
            //    lvStepSections.Visible = false;
            //    //divworkFlow.Visible = false;
            //}
            int.TryParse(Request[hfCurrentStepID.UniqueID], out currentStepID);
            long.TryParse(Request[hfCurrentStepSectionID.UniqueID], out currentStepSectionID);
            long.TryParse(Request[hfPreviousStepSectionID.UniqueID], out previousStepSectionID);

            startStepID = 1;
            if (Request["serviceID"] != null)
            {
                step1.Visible = false;
                step2.Visible = true;
                step3.Visible = true;
                startStepID = 2;
            }

            if (currentStepID == 0 && currentStepSectionID == 0)
            {
                currentStepID = startStepID;
                if (startStepID == 2)
                {
                    currentStepSectionID = 0;
                }
                else
                {
                    currentStepSectionID = 1;
                }
            }

            //Generate first step
            int serviceID = 0;
            //if (currentStepID == 1)
            //{
            //    control_layout.Visible = false;
            //    btnTopNext.Visible = false;
            //    divbtnTopNext.Visible = false;
            //}

            BindServiceCategories();
            if (!IsPostBack)
            {
                BindServices();
            }

            btNextL.Visible = true;
            btPreviousL.Visible = false;
            // divbtPreviousL.Visible = false;
            //divbtPreviousL.Attributes.CssStyle.Add("visibility", "hidden");

            serviceID = GetServiceID();
            //bool isAuthorized = true;
            if (serviceID > 0)
            {
                service = ServicesManager.LoadByServiceID(serviceID);

                overrideServiceQuestionFromParam(service);
                serviceCategory = ServiceCategoryManager.LoadByID(serviceID);
                if (service != null && service.ServiceType.ToLower() == Constants.ModuleAgent.ToLower().Replace("~", ""))
                {

                    moduleName = Regex.Replace(GetModuleName(), @"\s+", "");
                    ticketId = Regex.Replace(GetTicketId(), @"\s+", "");
                    ticketIdsForAgent = ticketId.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        spanTicketId.Visible = true;
                        List<string> lstTicketIdForAgent = new List<string>();
                        //isAuthorized = false;
                        string ticketIdLink = string.Empty;
                        foreach (string ticket in ticketIdsForAgent)
                        {
                            //Ticket ticketObj = new Ticket(HttpContext.Current.GetManagerContext());
                            DataRow currentTicketItem = Ticket.GetCurrentTicket(ApplicationContext, moduleName, ticket);
                            if (currentTicketItem != null)
                            {
                                if (Ticket.IsActionUser(ApplicationContext, currentTicketItem, User) || UserProfileManager.IsUGITSuperAdmin(User))//
                                {
                                    lstTicketIdForAgent.Add(ticket);

                                    string link = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}', '90', '90',0);", Ticket.GenerateTicketURL(HttpContext.Current.GetManagerContext(), moduleName, ticket), ticket);
                                    ticketIdLink += string.Format(" <a href=\"{0}\">{1}</a> ", link, ticket);
                                }

                            }
                        }
                        spanTicketId.Controls.Add(new LiteralControl(ticketIdLink));
                        if (lstTicketIdForAgent.Count > 0)
                        {
                            ticketIdsForAgent = lstTicketIdForAgent;
                            //isAuthorized = true;
                        }

                    }
                    else
                        spanTicketId.Visible = false;


                }

                //Check service access 
                List<string> authorizedToView = UGITUtility.ConvertStringToList(service.AuthorizedToView, Constants.Separator6);
                if (!UserProfileManager.IsUGITSuperAdmin(User) && service.AuthorizedToView != null && authorizedToView.Count > 0)
                {
                    //isAuthorized = false;
                    foreach (string userVal in authorizedToView)
                    {
                        UserProfile userProfile = UserProfileManager.GetUserInfoById(userVal);
                        if (!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest())
                        {
                            //isAuthorized = true;
                            break;
                        }
                        if (userProfile != null && userProfile.Id == User.Id)
                        {
                            //isAuthorized = true;
                            break;
                        }
                        else if (UserProfileManager.CheckUserIsInGroup(userProfile.Id, User))
                        {
                            //isAuthorized = true;
                            break;
                        }
                    }
                }

                if (service.AttachmentRequired.ToLower() == "always")
                {
                    hdnAttachmentMandatory.Value = "true";

                }

                lServiceTitle.Text = service.Title;
                litServiceTitle.Text = service.Title;
                if (!string.IsNullOrWhiteSpace(service.ImageUrl))
                {
                    imgServiceIcon.ImageUrl = UGITUtility.GetAbsoluteURL(service.ImageUrl);
                    Image2.ImageUrl = UGITUtility.GetAbsoluteURL(service.ImageUrl);
                }
            }

            if (currentStepID == 1)
            {
                BindStepSections(currentStepID);
                if (currentStepSectionID == 1)
                {
                    SelectStepAndSection(1, 1);
                }
            }
            else if (currentStepID == 2 && currentStepSectionID != SUMMARYSUBSECTION)
            {
                BindStepSections(currentStepID);
                if (service != null)
                {
                    if (!IsPostBack && service.Sections != null && service.Sections.Count > 0)
                    {
                        currentStepSectionID = service.Sections[0].ID;
                    }
                }

                SelectStepAndSection(currentStepID, currentStepSectionID);
                GenerateSectionQuestions(currentStepSectionID);
            }
            else if (currentStepID == 3)
            {

            }
            hideQuestionDesigner.Visible = ConfigurationVariableManager.GetValueAsBool(ConfigConstants.HideServiceQuestionGraphic);
            //fileuploadServiceAttach.fileUpload.ClientInstanceName = "fileuploadServiceAttach";
        }

        private void overrideServiceQuestionFromParam(Services service)
        {
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(_applicationContext);

            foreach (ServiceQuestion ques in service.Questions)
            {
                string defaultValue = UGITUtility.ObjectToString(Request.QueryString[ques.TokenName]);
                string newDefaultValue = string.Empty;
                if (!string.IsNullOrEmpty(defaultValue))
                {

                    if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME)
                    {
                        DateTime validDate;
                        if (DateTime.TryParse(defaultValue, out validDate))
                        {
                            newDefaultValue = UGITUtility.ObjectToString(validDate);
                        }
                        else
                            newDefaultValue = string.Empty;
                    }
                    else if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                    {
                        bool isMulti = false;
                        string isMultiS = string.Empty;
                        ques.QuestionTypePropertiesDicObj.TryGetValue("singleentryonly", out isMultiS);
                        isMulti = UGITUtility.StringToBoolean(isMultiS);

                        List<string> userIdList = new List<string>();
                        string[] uservalues = UGITUtility.SplitString(defaultValue, "||");
                        if (uservalues != null && uservalues.Count() > 0)
                        {
                            foreach (string str in uservalues)
                            {
                                UserProfile user = _applicationContext.UserManager.GetUserByBothUserNameandDisplayName(str);
                                if (user != null)
                                {
                                    userIdList.Add(user.Id);
                                }
                            }
                        }
                        if (userIdList.Count() > 0)
                        {
                            if (isMulti)
                                newDefaultValue = UGITUtility.ConvertListToString(userIdList, Constants.Separator6);
                            else
                                newDefaultValue = userIdList.First();
                        }
                    }
                    else if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.LOOKUP ||
                        ques.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE || ques.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets)
                    {
                        // change string to lookup ids
                        string listName = string.Empty;
                        ques.QuestionTypePropertiesDicObj.TryGetValue("lookuplist", out listName);
                        // change lisstName if question type is requesttype because requesttype question fieldname is not generic
                        if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE)
                            listName = DatabaseObjects.Columns.TicketRequestTypeLookup;
                        if (ques.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets)
                            listName = DatabaseObjects.Columns.AssetLookup;

                        string listfield = string.Empty;
                        ques.QuestionTypePropertiesDicObj.TryGetValue("lookupfield", out listfield);

                        string lpModuleName = string.Empty;
                        ques.QuestionTypePropertiesDicObj.TryGetValue("module", out lpModuleName);

                        bool isMulti = false;
                        string isMultiS = string.Empty;
                        ques.QuestionTypePropertiesDicObj.TryGetValue("multi", out isMultiS);
                        isMulti = UGITUtility.StringToBoolean(isMultiS);

                        FieldConfiguration configField = fieldConfigurationManager.GetFieldByFieldName(listName);
                        DataTable dt = fieldConfigurationManager.GetFieldDataByFieldName(listName, lpModuleName, string.Empty, _applicationContext.TenantID);
                        //list to hold multiple values
                        List<string> lookupValueLst = new List<string>();
                        string[] lookupParamLst = UGITUtility.SplitString(defaultValue, "||");
                        if (lookupParamLst != null && lookupParamLst.Count() > 0)
                        {
                            foreach (string str in lookupParamLst)
                            {
                                DataRow[] dtRows = dt.Select($"{configField.ParentFieldName} = '{str}' ");
                                if (dtRows != null && dtRows.Count() > 0)
                                {
                                    lookupValueLst.Add(UGITUtility.ObjectToString(dtRows[0][DatabaseObjects.Columns.ID]));
                                }
                            }
                        }
                        if (lookupValueLst.Count() > 0)
                        {
                            if (isMulti)
                                newDefaultValue = UGITUtility.ConvertListToString(lookupValueLst, Constants.Separator6);
                            else
                                newDefaultValue = lookupValueLst[0];
                        }
                    }
                    else
                    {
                        newDefaultValue = defaultValue;
                    }
                    //set default value in questions dictionary object if not exits add new one
                    if (ques.QuestionTypePropertiesDicObj.ContainsKey("defaultval"))
                    {
                        ques.QuestionTypePropertiesDicObj["defaultval"] = newDefaultValue;
                    }
                    else
                    {
                        ques.QuestionTypePropertiesDicObj.Add("defaultval", newDefaultValue);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            imgHelp.Visible = false;
            if (service != null)
            {
                if (!string.IsNullOrEmpty(service.NavigationUrl))
                {
                    detailsUrl = UGITUtility.GetAbsoluteURL(service.NavigationUrl);
                    imgHelp.Visible = true;

                    // imgHelp.Attributes.Add("onclick", "javascript:window.parent.UgitOpenPopupDialog(" + detailsUrl + ", '', 'Service Help', '1000px', '90', 0, " + Server.UrlEncode(Request.Url.AbsolutePath) +"));");
                    imgHelp.Attributes.Add("onclick", "javascript:openEditDialog()");
                    if (service.NavigationType.EqualsIgnoreCase("HelpCard"))
                    {
                        imgHelp.Attributes.Add("onclick", $"javascript:openHelpCard('{service.NavigationUrl}','Service')");
                    }

                }

                if (service.AttachmentRequired.ToLower() == "always")
                {
                    hdnAttachmentMandatory.Value = "true";
                }

                lServiceTitle.Text = service.Title;
                litServiceTitle.Text = service.Title;
                if (!string.IsNullOrWhiteSpace(service.ImageUrl))
                {
                    imgServiceIcon.ImageUrl = UGITUtility.GetAbsoluteURL(service.ImageUrl);
                    Image2.ImageUrl = UGITUtility.GetAbsoluteURL(service.ImageUrl);
                }

            }

            if (currentStepID != 1)
            {
                if (!IsPostBack)
                {
                    UpdateServiceInputObj(false);
                }
            }



            SaveAttachedFiles();
            if (!IsPostBack)
            {
                UGITUtility.DeleteCookie(HttpContext.Current.Request, HttpContext.Current.Response, "DependentUser");
                UGITUtility.DeleteCookie(HttpContext.Current.Request, HttpContext.Current.Response, "DependentAssetType");
            }
        }

        private void InitialiseButtonsForAgent()
        {
            if (!string.IsNullOrEmpty(moduleName) && ticketIdsForAgent != null && ticketIdsForAgent.Count > 0)
            {
                string ticket = ticketIdsForAgent.FirstOrDefault();
                Ticket ticketRequest = new Ticket(HttpContext.Current.GetManagerContext(), moduleName);
                DataRow currentTicket = Ticket.GetCurrentTicket(HttpContext.Current.GetManagerContext(), moduleName, ticket);
                if (currentTicket != null)
                {
                    LifeCycleStage currentStage = ticketRequest.GetTicketCurrentStage(currentTicket);
                    if (currentStage != null)
                    {
                        approveButtonName = currentStage.StageApproveButtonName;
                        rejectButtonName = currentStage.StageRejectedButtonName;
                        returnButtonName = currentStage.StageReturnButtonName;
                        if (currentStage.ApprovedStage != null && !string.IsNullOrEmpty(approveButtonName))
                            showApproveButton = true;
                        else
                            showApproveButton = false;

                        if (currentStage.RejectStage != null && !string.IsNullOrEmpty(rejectButtonName))
                            showRejectButton = true;
                        else
                            showRejectButton = false;

                        if (currentStage.ReturnStage != null && !string.IsNullOrEmpty(returnButtonName))
                            showReturnButton = true;
                        else
                            showReturnButton = false;

                        if (!string.IsNullOrEmpty(currentStage.ApproveIcon))
                            approveButtonImage = currentStage.ApproveIcon;
                        else if (!string.IsNullOrEmpty(currentStage.Prop_CustomIconApprove))
                            approveButtonImage = currentStage.Prop_CustomIconApprove;

                        if (!string.IsNullOrEmpty(currentStage.ReturnIcon))
                            returnButtonImage = currentStage.ReturnIcon;
                        else if (!string.IsNullOrEmpty(currentStage.Prop_CustomIconReturn))
                            returnButtonImage = currentStage.Prop_CustomIconReturn;

                        if (!string.IsNullOrEmpty(currentStage.RejectIcon))
                            rejectButtonImage = currentStage.RejectIcon;
                        else if (!string.IsNullOrEmpty(currentStage.Prop_CustomIconReject))
                            rejectButtonImage = currentStage.Prop_CustomIconReject;

                        if (showApproveButton)
                        {
                            btApproveL.Visible = true;
                            btApproveL.Text = approveButtonName;

                            //if (!string.IsNullOrEmpty(approveButtonImage))
                            //   imgApprove.Src = UGITUtility.GetAbsoluteURL(approveButtonImage);
                        }
                        if (showRejectButton)
                        {
                            btRejectL.Visible = true;
                            btRejectL.Text = rejectButtonName;
                            //if (!string.IsNullOrEmpty(rejectButtonImage))
                            //   imgReject.Src = UGITUtility.GetAbsoluteURL(rejectButtonImage);
                        }
                        if (showReturnButton)
                        {
                            btReturnL.Visible = true;
                            btReturnL.Text = returnButtonName;
                            //if (!string.IsNullOrEmpty(returnButtonImage)) ;
                            //imgReturn.Src = UGITUtility.GetAbsoluteURL(returnButtonImage);
                        }
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            newServiceCategory.Visible = false;

            if (currentStepID != 1)
            {

                pAttachmentContainer.Width = new Unit(91, UnitType.Percentage);
                pActions.Width = new Unit(90, UnitType.Percentage);


                newServiceCategory.Visible = true;
                divMainContainer.Attributes.Add("style", "background:#FFF !important;");
                string strSectionCookie = UGITUtility.GetCookieValue(Request, "SectionContainer");

                if (strSectionCookie == "Hide")
                {
                    divShowSectionContainer.Visible = true;
                    divShowSectionContainer.Attributes.Add("style", "display:block;float:left;");
                    divHideSectionContainer.Visible = true;
                    divHideSectionContainer.Attributes.Add("style", "display:none;");

                    colrightdescspan.Visible = false;
                    divSetSection.Attributes.Add("style", "display:none;");

                    divSetSectionNew.Attributes.Add("style", string.Format("display:none;float:left;background-color: #f6f7fb;"));
                }
                else
                {
                    divShowSectionContainer.Visible = true;
                    divSetSectionNew.Visible = true;
                    divShowSectionContainer.Attributes.Add("style", "display:none;float:left;");
                    colrightdescspan.Attributes.Add("style", "display:none");
                    divHideSectionContainer.Visible = true;
                    divHideSectionContainer.Attributes.Add("style", "display:block;float:left;");

                    colrightdescspan.Visible = false;

                    divSetSectionNew.Attributes.Add("style", "display:block;float:left;background-color: #f6f7fb;");
                    divSetSection.Attributes.Add("style", "display:none;");
                }
            }
            else
            {
                divMainContainer.Attributes.Add("style", "background:#eaf2f7 !important;");
                divShowSectionContainer.Visible = false;
                divHideSectionContainer.Visible = false;
                colrightdescspan.Visible = true;

                divSetSectionNew.Visible = false;
                divSetSection.Attributes.Add("style", "display:block;");
                divSetSectionNew.Attributes.Add("style", "display:none;");
            }

            bool isSummaryView = false;
            pAttachmentContainer.Visible = true;
            if (service != null)
            {
                hfServiceID.Value = service.ID.ToString();
            }

            hfCurrentStepID.Value = currentStepID.ToString();
            hfCurrentStepSectionID.Value = currentStepSectionID.ToString();
            hfPreviousStepSectionID.Value = previousStepSectionID.ToString();
            btCreateRequestL.Visible = false;

            //Skip Captcha it SuperAdmin creates Tenant
            if (UserProfileManager.IsUGITSuperAdmin(User))
            {
                Captcha.IsValid = true;
                Captcha.Visible = false;
                Captcha.Enabled = false;
                recaptchaWrap.Visible = false;
            }
            else
            {
                Captcha.IsValid = false;
                Captcha.Visible = false;
                Captcha.Enabled = false;
                recaptchaWrap.Visible = false;
            }
            btApproveL.Visible = false;
            btRejectL.Visible = false;
            btReturnL.Visible = false;
            if (service != null && currentStepID > 1)
            {
                if (service.Sections.Count == 0 || service.Questions.Count == 0)
                {
                    btNextL.Visible = false;
                }
                else if (service.HideSummary)
                {
                    long hvalue = service.Sections.LastOrDefault().ID;
                    if (currentStepID == 2 && hvalue == currentStepSectionID)
                    {
                        if (service.ShowStageTransitionButtons)
                        {
                            btCreateRequestL.Visible = true; // Acts as simple save
                            RenameButton();
                            InitialiseButtonsForAgent();
                        }
                        else
                        {
                            btCreateRequestL.Visible = true;
                            btApproveL.Visible = false;
                            btRejectL.Visible = false;
                            btReturnL.Visible = false;
                            //if (service.ServiceCategoryType.EqualsIgnoreCase("Tenant management"))
                            if (tenantOnBoardingHelper.GetNewTenantTemplateServiceTitles().Contains(service.Title))
                            {
                                Captcha.IsValid = true;
                                Captcha.Visible = true;
                                Captcha.Enabled = true;
                                recaptchaWrap.Visible = true;
                            }
                            else
                            {
                                Captcha.IsValid = false;
                                Captcha.Visible = false;
                                Captcha.Enabled = false;
                                recaptchaWrap.Visible = false;
                            }
                        }

                        btNextL.Visible = false;
                    }
                }
            }

            if (isNextPrevious)
            {
                if (currentStepID == 1)
                {
                    btPreviousL.Visible = false;
                    lServiceTitle.Text = "Service Catalog";
                    litServiceTitle.Text = "Service Catalog";
                    imgServiceIcon.ImageUrl = UGITUtility.GetAbsoluteURL("/Content/Images/Services.png");
                    Image2.ImageUrl = UGITUtility.GetAbsoluteURL("/Content/Images/Services.png");
                }
                else if (currentStepID == 2)
                {
                    if (currentStepSectionID == SUMMARYSUBSECTION)
                    {
                        if (!service.HideSummary && service != null)
                        {
                            isSummaryView = true;
                            GenerateSummary();
                            if (serviceCategory != null && serviceCategory.CategoryName.ToLower() == Constants.ModuleAgent.ToLower() && string.IsNullOrEmpty(GetTicketId()))
                            {
                                btCreateRequest.Visible = false;
                                btApproveL.Visible = false;
                                btRejectL.Visible = false;
                                btReturnL.Visible = false;
                            }
                            else
                            {
                                if (service.ShowStageTransitionButtons)
                                {
                                    btCreateRequestL.Visible = true;
                                    RenameButton();
                                    InitialiseButtonsForAgent();
                                }
                                else
                                {
                                    btCreateRequestL.Visible = true;
                                    btApproveL.Visible = false;
                                    btRejectL.Visible = false;
                                    btReturnL.Visible = false;
                                }
                            }
                            btNextL.Visible = false;
                        }
                    }
                    else
                    {
                        if (service.Sections != null && startStepID == 2)
                        {
                            ServiceSection section = service.Sections.FirstOrDefault(x => x.ID == currentStepSectionID);
                            if (section != null && service.Sections.IndexOf(section) == 0)
                            {
                                btPreviousL.Visible = false;
                            }
                        }


                        GenerateSectionQuestions(currentStepSectionID);
                        if (isModuleCall)
                        {
                            UpdateServiceInputObj(false);
                        }
                        UpdateCtrValue();
                    }
                }
                else if (currentStepID == 3)
                {

                    isSummaryView = true;
                    BindStepSections(currentStepID);
                    SelectStepAndSection(currentStepID, currentStepSectionID);
                    if (currentStepSectionID == 1)
                    {
                        btNextL.Visible = false;
                        btPreviousL.Visible = false;
                        btCreateRequestL.Visible = false;
                        btApproveL.Visible = false;
                        btRejectL.Visible = false;
                        btReturnL.Visible = false;
                        pAttachmentContainer.Visible = false;
                        divShowHideArrow.Visible = false;
                        divSetSectionNew.Visible = false;
                    }
                }
            }
            
            if (currentStepID == 2 && currentStepSectionID == SUMMARYSUBSECTION && !Captcha.IsValid)
            {
                //if (service.ServiceCategoryType.EqualsIgnoreCase("Tenant management"))
                if(tenantOnBoardingHelper.GetNewTenantTemplateServiceTitles().Contains(service.Title))
                {
                    Captcha.Visible = true;
                    Captcha.Enabled = true;
                    recaptchaWrap.Visible = true;
                    // Added below condition to fix Captcha issue; showing error message when coming to summary page , after filling details.
                    if (isNextPrevious)
                        Captcha.IsValid = true;
                    btCreateRequestL.Visible = true;
                }
            }

            if (_serviceInput != null)
            {
                XmlDocument doc = uHelper.SerializeObject(_serviceInput);
                if (doc != null)
                {
                    hfQuestionInputs.Value = Server.HtmlEncode(doc.OuterXml);
                }
            }

            //Shows attachments with edit mode
            ShowAttachmentList(isSummaryView);

            base.OnPreRender(e);

        }

        protected void RenameButton()
        {
            btCreateRequestL.Text = "Save";
            btCreateRequestL.Attributes.Add("style", "padding-top:2px");
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ServiceSectionInput sectionInput = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == currentStepSectionID);
            List<ServiceQuestionInput> sectionQuestionInputs = new List<ServiceQuestionInput>();
            if (sectionInput != null)
            {
                sectionQuestionInputs = sectionInput.Questions;
            }
            List<ServiceQuestion> questions = service.Questions.Where(x => x.ServiceSectionID == currentStepSectionID).OrderBy(x => x.ItemOrder).ToList();
            DataTable dt = null;
            List<QuestionList> groupQues = GetQuestionGrouping(questions);
            foreach (QuestionList qroupques in groupQues)
            {
                ListViewDataItem groupItem = lvgroupquestions.Items[groupQues.IndexOf(qroupques)];
                ListView listView = (ListView)groupItem.FindControl("lvQuestions");
                foreach (ServiceQuestion question in qroupques.qLists)
                {
                    ServiceQuestionInput input = sectionQuestionInputs.FirstOrDefault(x => x.Token == question.TokenName);
                    ListViewDataItem listviewDItem = listView.Items[qroupques.qLists.IndexOf(question)];
                    Control ctr = listviewDItem.FindControl(string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID));

                    if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets)
                    {
                        ASPxComboBox combox = ctr as ASPxComboBox;
                        if (combox == null)
                            continue;
                        combox.Items.Clear();
                        KeyValuePair<string, string> currentuser = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "currentuser");
                        KeyValuePair<string, string> specificuser = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "specificuser");
                        KeyValuePair<string, string> userquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "userquestion");
                        KeyValuePair<string, string> includedepartmentasset = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "includedepartmentasset");
                        if (currentuser.Key != null)
                        {

                        }
                        else if (specificuser.Key != null)
                        {

                        }
                        else if (userquestion.Key != null)
                        {

                            ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(userquestion.Value));
                            if (inputQuestion != null)
                            {
                                string tokenname = inputQuestion.TokenName;
                            }
                        }

                        combox.ValueField = DatabaseObjects.Columns.Id;
                        combox.ValueType = typeof(int);
                        combox.TextField = DatabaseObjects.Columns.AssetTagNum;
                        combox.DataSource = dt;
                        combox.DataBind();
                    }
                    else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.ApplicationAccess
                        || (question.QuestionType.ToLower() == "lookup" && question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist").Value.ToLower() == DatabaseObjects.Columns.DepartmentLookup.ToLower())
                        || (question.QuestionType.ToLower() == "lookup" && question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist").Value.ToLower() == DatabaseObjects.Columns.LocationLookup.ToLower())
                        || (question.QuestionType.ToLower() == "lookup" && question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist").Value.ToLower() == "companydivisions")
                        || question.QuestionType.ToLower() == "textbox"
                        || question.QuestionType.ToLower() == "userfield")
                    {
                        UpdateServiceInputObj(false);
                    }
                }
            }
        }

        protected void BtNext_Click(object sender, EventArgs e)
        {
            // Added below code again, as Mandatory fields in Service Wizard are not preventing to move to next page, when left blank.
            if (!Page.IsValid)
                return;

            isNextPrevious = true;
            btPreviousL.Visible = true;
            // divbtPreviousL.Visible = true;
            //divbtPreviousL.Attributes.CssStyle.Add("visibility", "visible");

            if (currentStepID == 1)
            {
                currentStepID = 2;
                currentStepSectionID = 0;

                if (service != null && service.Sections.Count > 0)
                {
                    currentStepSectionID = service.Sections[0].ID;
                }

                if (currentStepSectionID == 1 && service != null)
                {
                    if (_serviceInput.ServiceID != GetServiceID())
                        _serviceInput = new ServiceInput();
                }
                else if (currentStepSectionID == 2)
                {
                    _serviceInput.ServiceID = service.ID;
                }

                isModuleCall = true;
            }
            else if (currentStepID == 2)
            {
                UpdateServiceInputObj(true);

                if (currentStepSectionID == SUMMARYSUBSECTION)
                {
                    currentStepID = 3;
                    currentStepSectionID = 1;
                }
                else
                {
                    List<ServiceSectionCondition> skipSectionConditions = service.SkipSectionCondition;
                    ValidateSectionConditions(ref skipSectionConditions);
                    service.SkipSectionCondition = skipSectionConditions;

                    int index = 0;
                    long sectionID = 0;
                    if (service != null)
                    {
                        ServiceSection section = service.Sections.FirstOrDefault(x => x.ID == currentStepSectionID);
                        if (section != null)
                        {
                            index = service.Sections.IndexOf(section);
                            if (index != service.Sections.Count - 1)
                            {
                                for (int i = index + 1; i < service.Sections.Count; i++)
                                {
                                    sectionID = service.Sections[i].ID;
                                    if (skipSectionConditions != null && skipSectionConditions.Exists(x => x.SkipSectionsID.Exists(y => y == sectionID) && x.ConditionValidate))
                                    {
                                        if (i == service.Sections.Count - 1)
                                        {
                                            currentStepID = 2;
                                            sectionID = SUMMARYSUBSECTION;
                                            previousStepSectionID = currentStepSectionID;
                                        }
                                        continue;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                currentStepID = 2;
                                sectionID = SUMMARYSUBSECTION;
                                if (service.Sections.Count <= 1)
                                    previousStepSectionID = service.Sections.Count;
                                else
                                    previousStepSectionID = currentStepSectionID;
                            }
                        }
                    }
                    currentStepSectionID = sectionID;
                }
            }

            //Generate first step
            if (currentStepSectionID == SUMMARYSUBSECTION)
            {

                if (service != null && !service.HideSummary)
                {
                    BindStepSections(currentStepID);
                    SelectStepAndSection(currentStepID, currentStepSectionID);
                }

            }
            else
            {
                BindStepSections(currentStepID);
                SelectStepAndSection(currentStepID, currentStepSectionID);
            }

        }

        protected void BtPrevious_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            isNextPrevious = true;
            btNextL.Visible = true;

            if (currentStepID == 1)
            {
                if (currentStepSectionID == 2)
                    currentStepSectionID = 1;

                btPreviousL.Visible = true;
            }
            else if (currentStepID == 2)
            {
                btPreviousL.Visible = true;
                UpdateServiceInputObj(true);

                List<ServiceSectionCondition> skipSectionConditions = service.SkipSectionCondition;
                ValidateSectionConditions(ref skipSectionConditions);
                service.SkipSectionCondition = skipSectionConditions;


                int index = 0;
                long sectionID = 0;
                if (service != null)
                {

                    if (currentStepSectionID == SUMMARYSUBSECTION)
                    {
                        if (previousStepSectionID != 0)
                        {
                            sectionID = service.Sections.First().ID;
                            if (service.Sections.Exists(x => x.ID == previousStepSectionID))
                            {
                                sectionID = service.Sections.FirstOrDefault(x => x.ID == previousStepSectionID).ID;
                            }
                            previousStepSectionID = 0;
                        }
                        else
                            sectionID = service.Sections[0].ID;
                    }
                    else
                    {
                        ServiceSection secction = service.Sections.FirstOrDefault(x => x.ID == currentStepSectionID);
                        if (secction != null)
                        {
                            index = service.Sections.IndexOf(secction);
                            if (index != 0)
                            {
                                for (int i = index - 1; i >= 0; i--)
                                {
                                    sectionID = service.Sections[i].ID;

                                    if (skipSectionConditions != null && skipSectionConditions.Exists(x => x.SkipSectionsID.Exists(y => y == sectionID) && x.ConditionValidate))
                                    {
                                        if (i == service.Sections.Count - 1)
                                        {
                                            currentStepID = 1;
                                            sectionID = 1;
                                        }
                                        continue;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                currentStepID = 1;
                                sectionID = 1;
                            }
                        }
                    }
                }
                currentStepSectionID = sectionID;

            }
            else if (currentStepID == 3)
            {
                currentStepID = 2;
                currentStepSectionID = 0;
                if (service != null && service.Sections.Count > 0)
                {
                    currentStepSectionID = service.Sections[0].ID;
                }
            }

            //Generate first step
            if (currentStepSectionID == SUMMARYSUBSECTION)
            {

                if (!service.HideSummary)
                {
                    BindStepSections(currentStepID);
                    SelectStepAndSection(currentStepID, currentStepSectionID);
                }

            }
            else
            {
                BindStepSections(currentStepID);
                SelectStepAndSection(currentStepID, currentStepSectionID);
            }


        }

        protected void BtCancel_Click(object sender, EventArgs e)
        {
            if (btCancelL.Text.Equals("close", StringComparison.InvariantCultureIgnoreCase))
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
            //else if (btCancelL.Text.Equals("cancel", StringComparison.InvariantCultureIgnoreCase))
            //{
            //    uHelper.ClosePopUpAndEndResponse(Context, false);
            //}

        }

        protected void cvAttachment_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //if (hdnAttachmentMandatory.Value.ToLower() == "true")
            //{
            //    List<string> deleteFiles = uHelper.ConvertStringToList(txtDeleteFiles.Text, new string[] { "~" });
            //    bool hasAttachment = false;
            //    if (ddlExistingAttc.Items.Count - deleteFiles.Count > 0)
            //        hasAttachment = true;
            //    else
            //    {
            //        foreach (string key in Request.Files.Keys)
            //        {
            //            if (!string.IsNullOrEmpty(Request.Files[key].FileName))
            //            {
            //                hasAttachment = true;
            //                break;
            //            }
            //        }
            //    }


            //    if (!hasAttachment)
            //        args.IsValid = false;
            //}
        }
        #endregion

        #region Step 1
        private void BindServiceCategories()
        {
            if (!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest())

            {
                _applicationContext = ApplicationContext.Create();
                _serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext(), "SelfRegistration");
            }

            DataTable serviceCategories = UGITUtility.ToDataTable<ServiceCategory>(ServiceCategoryManager.Load()); // uGITCache.LoadTable(DatabaseObjects.Lists.ServiceCategories);
            if (serviceCategories != null && serviceCategories.Rows.Count > 0)
            {
                serviceCategories.DefaultView.RowFilter = string.Format("{0} <> '{1}' and {2} is not null", DatabaseObjects.Columns.CategoryName, Constants.ModuleFeedback, DatabaseObjects.Columns.Title);
                serviceCategories = serviceCategories.DefaultView.ToTable();
            }

            serviceCategoryDD.ClearSelection();
            if (serviceCategories != null)
            {
                foreach (DataRow row in serviceCategories.Rows)
                {
                    serviceCategoryDD.Items.Add(new ListItem(Convert.ToString(row[DatabaseObjects.Columns.CategoryName]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                }
            }
            serviceCategoryDD.Items.Insert(0, new ListItem("All", "0"));
        }

        private void BindServices()
        {
            serviceTypeDD.Items.Clear();

            if (!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest())

            {
                _applicationContext = ApplicationContext.Create();
                _serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext(), "SelfRegistration");
            }
            List<Services> serviceList = ServicesManager.LoadCurrentUserServices();
            serviceList = serviceList.Where(x => x.IsActivated && !x.Deleted).ToList();

            #region NewTenantTemplate
            var listOfNewTenantTemplate = ConfigurationManager.AppSettings["NewTenantTemplate:ServiceTitle"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (listOfNewTenantTemplate.Count > 0)
            {
                foreach (var item in listOfNewTenantTemplate)
                {
                    serviceList = serviceList.Where(x => x.Title != item).ToList();
                }

            }
            #endregion
            //if(serviceList.Contains(item1[0])|| serviceList.Contains(item1[0]))
            int selectedServiceID = 0;
            int.TryParse(Request[serviceTypeDD.UniqueID], out selectedServiceID);

            int selectedServiceCID = 0;
            int.TryParse(Request[serviceCategoryDD.UniqueID], out selectedServiceCID);
            if (selectedServiceCID > 0)
            {
                serviceList = serviceList.Where(x => x.CategoryId == selectedServiceCID).ToList();
            }

            foreach (Services service in serviceList)
            {
                serviceTypeDD.Items.Add(new ListItem(service.Title, service.ID.ToString()));
            }

            serviceTypeDD.SelectedIndex = serviceTypeDD.Items.IndexOf(serviceTypeDD.Items.FindByValue(selectedServiceID.ToString()));
        }

        protected void ServiceCategoryDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindServices();
        }

        #endregion

        #region Step 2

        private void GenerateSectionQuestions(long sectionID)
        {
            // Initialize itemId for each where expression if any Id is set to zero in any whereExpression
            foreach (ServiceSectionCondition sSkipCondition in service.SkipSectionCondition)
            {
                if (sSkipCondition.Conditions.Any(x => x.Id == 0))
                {
                    int itemIndex = 0;
                    foreach (WhereExpression wExpression in sSkipCondition.Conditions)
                    {
                        itemIndex++;
                        wExpression.Id = itemIndex;
                    }
                }
            }

            //Fetchs all condition 
            List<ServiceSectionCondition> mappedSkipConditions = service.SkipSectionCondition;
            string skipCondition = string.Empty;

            string key = string.Empty;
            string operatorVal = string.Empty;
            string cdnValues = string.Empty;
            string skipQuestions = string.Empty;
            string questionTypeJosn = string.Empty;
            ServiceQuestion kQuestion = null;

            List<string> questionSkipJsonExps = new List<string>();
            List<int> questionsNeedToSkipped = new List<int>();

            List<string> multipleQuestion = new List<string>();
            string logicalRelOperator = string.Empty;
            string attachmentJson = string.Empty;
            string itemId = string.Empty;
            string parentId = string.Empty;

            if (mappedSkipConditions != null && mappedSkipConditions.Count > 0)
            {
                for (int j = 0; j < mappedSkipConditions.Count; j++)
                {
                    if (j > 0)
                    {
                        logicalRelOperator = key = operatorVal = cdnValues = questionTypeJosn = itemId = parentId = attachmentJson = string.Empty;
                        multipleQuestion = new List<string>();
                    }

                    //Continue when there is no condition
                    if (mappedSkipConditions[j].Conditions == null || mappedSkipConditions[j].Conditions.Count <= 0)
                    {
                        Util.Log.ULog.WriteLog(string.Format("Service: {0} > Question skip condition: {1} > Condition Question not attached!",
                                                    service.Title, mappedSkipConditions[j].Title));
                        continue;
                    }

                    for (int k = 0; k < mappedSkipConditions[j].Conditions.Count; k++)
                    {
                        kQuestion = service.Questions.FirstOrDefault(x => x.TokenName == mappedSkipConditions[j].Conditions[k].Variable);
                        if (kQuestion == null)
                        {
                            Log.WriteLog(string.Format("Service: {0} > Question skip condition: {1} > Condition Question not found: {2}",
                                                        service.Title, mappedSkipConditions[j].Title, mappedSkipConditions[j].Conditions[k].Variable));
                            continue;
                        }

                        logicalRelOperator = string.Format("\"logicalRelOperator\":\"{0}\"", mappedSkipConditions[j].Conditions[k].LogicalRelOperator);
                        key = string.Format("\"key\":\"{0}\"", mappedSkipConditions[j].Conditions[k].Variable);
                        operatorVal = string.Format("\"operator\":\"{0}\"", mappedSkipConditions[j].Conditions[k].Operator);
                        cdnValues = string.Format("\"values\":\"{0}\"", Uri.EscapeDataString(mappedSkipConditions[j].Conditions[k].Value));
                        questionTypeJosn = string.Format("\"qtype\":\"{0}\"", kQuestion.QuestionType.ToLower());
                        itemId = string.Format("\"itemid\":\"{0}\"", mappedSkipConditions[j].Conditions[k].Id);
                        parentId = string.Format("\"parentid\":\"{0}\"", mappedSkipConditions[j].Conditions[k].ParentId);

                        multipleQuestion.Add(string.Format("{{ {0},{1},{2},{3},{4},{5},{6} }}", logicalRelOperator, key, operatorVal, cdnValues, questionTypeJosn, itemId, parentId));
                    }

                    if (multipleQuestion.Count == 0)
                        continue;

                    string multipleQuestionsArr = string.Format("\"mQuestions\":[{0}]", string.Join(",", multipleQuestion.ToArray()));
                    List<string> skippedQuestions = new List<string>();
                    for (int i = 0; i < mappedSkipConditions[j].SkipQuestionsID.Count; i++)
                    {
                        skippedQuestions.Add(Convert.ToString(mappedSkipConditions[j].SkipQuestionsID[i]));
                    }
                    attachmentJson = string.Format("\"attachment\":\"na\"");
                    string skippedQuestionsArr = string.Format("\"skipQuestions\":\"{0}\"", string.Join(",", skippedQuestions.ToArray()));

                    questionSkipJsonExps.Add(string.Format("{{ {0},{1},{2} }}", multipleQuestionsArr, attachmentJson, skippedQuestionsArr));
                    skippedQuestionsArr = string.Empty;
                }
            }

            // Attachment Mandatory condition
            if (service.AttachmentRequired.ToLower() == "conditional" && service.AttachmentRequiredCondition != null && service.AttachmentRequiredCondition.Conditions.Count > 0)
            {
                logicalRelOperator = key = operatorVal = cdnValues = questionTypeJosn = itemId = parentId = attachmentJson = string.Empty;
                multipleQuestion = new List<string>();

                for (int k = 0; k < service.AttachmentRequiredCondition.Conditions.Count; k++)
                {
                    kQuestion = service.Questions.FirstOrDefault(x => x.TokenName == service.AttachmentRequiredCondition.Conditions[k].Variable);

                    if (kQuestion == null)
                    {
                        Log.WriteLog(string.Format("Service: {0} > Question skip condition: {1} > Condition Question not found: {2}",
                                                    service.Title, service.AttachmentRequiredCondition.Title, service.AttachmentRequiredCondition.Conditions[k].Variable));
                        continue;
                    }

                    logicalRelOperator = string.Format("\"logicalRelOperator\":\"{0}\"", service.AttachmentRequiredCondition.Conditions[k].LogicalRelOperator);
                    key = string.Format("\"key\":\"{0}\"", service.AttachmentRequiredCondition.Conditions[k].Variable);
                    operatorVal = string.Format("\"operator\":\"{0}\"", service.AttachmentRequiredCondition.Conditions[k].Operator);
                    cdnValues = string.Format("\"values\":\"{0}\"", service.AttachmentRequiredCondition.Conditions[k].Value);
                    questionTypeJosn = string.Format("\"qtype\":\"{0}\"", kQuestion.QuestionType.ToLower());
                    itemId = string.Format("\"itemid\":\"{0}\"", service.AttachmentRequiredCondition.Conditions[k].Id);
                    parentId = string.Format("\"parentid\":\"{0}\"", service.AttachmentRequiredCondition.Conditions[k].ParentId);

                    multipleQuestion.Add(string.Format("{{ {0},{1},{2},{3},{4},{5},{6} }}", logicalRelOperator, key, operatorVal, cdnValues, questionTypeJosn, itemId, parentId));
                }

                string multipleQuestionsArr = string.Format("\"mQuestions\":[{0}]", string.Join(",", multipleQuestion.ToArray()));

                string skippedQuestionS = string.Format("\"skipQuestions\":\"\"");
                attachmentJson = string.Format("\"attachment\":\"mandatory\"");
                questionSkipJsonExps.Add(string.Format("{{ {0},{1},{2} }}", multipleQuestionsArr, attachmentJson, skippedQuestionS));
            }

            skipCondition = string.Format("[{0}]", string.Join(",", questionSkipJsonExps.ToArray()));
            pSkipConditions.Controls.Clear();
            pSkipConditions.Controls.Add(new LiteralControl(skipCondition));

            StringBuilder sectionQuestionJson = new StringBuilder();
            string sectionQuestions = "";
            if (service != null)
            {
                List<ServiceQuestion> questions = service.Questions.Where(x => x.ServiceSectionID == sectionID).OrderBy(x => x.ItemOrder).ToList();

                //Load Default Ticket Values only if single ticket is selected
                if (service.ServiceType == Constants.ModuleAgent.ToLower().Replace("~", "") && service.QuestionsMapping.Count > 0 && service.LoadDefaultValue && ticketIdsForAgent != null && ticketIdsForAgent.Count == 1)
                {
                    DataRow ticket = Ticket.GetCurrentTicket(ApplicationContext, moduleName, ticketId);
                    if (ticket != null)
                    {

                        foreach (ServiceQuestion question in questions)
                        {
                            ServiceQuestionMapping mapping = service.QuestionsMapping.FirstOrDefault(x => x.PickValueFrom == question.ID.ToString());
                            if (mapping == null)
                                continue;
                            if (!UGITUtility.IfColumnExists(mapping.ColumnName))
                                continue;

                            string fieldVal = Convert.ToString(ticket[mapping.ColumnName]);
                            string fieldName = mapping.ColumnName;

                            if (fieldName == DatabaseObjects.Columns.TicketPctComplete)
                            {
                                double pctComplete;
                                if (double.TryParse(fieldVal, out pctComplete))
                                {
                                    // Display 0-1 value as % with 1 or two decimals (if needed)
                                    if (fieldName == DatabaseObjects.Columns.TicketPctComplete)
                                        pctComplete *= 100;

                                    if (pctComplete > 99.9 && pctComplete < 100)
                                        fieldVal = "99.9"; // Don't show 100% unless all the way done!
                                    else
                                        fieldVal = string.Format("{0:0.#}", pctComplete);
                                }
                            }
                            string questionType = question.QuestionType.ToLower();

                            KeyValuePair<string, string> currentquestionKeyPair = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                            if (currentquestionKeyPair.Key != null)
                            {
                                ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(service.QuestionsMapping.FirstOrDefault().PickValueFrom));
                                if (inputQuestion != null)
                                {
                                    question.QuestionTypePropertiesDicObj[currentquestionKeyPair.Key] = fieldVal;
                                }
                            }
                            else
                            {
                                question.QuestionTypePropertiesDicObj.Add("defaultval", fieldVal);
                            }
                        }
                    }
                }

                //lvQuestions.DataSource = questions;
                //lvQuestions.DataBind();
                lvgroupquestions.DataSource = GetQuestionGrouping(questions);
                lvgroupquestions.DataBind();
                List<string> lstskipJSON = new List<string>();
                for (int i = 0; i < questions.Count; i++)
                {
                    lstskipJSON.Add(string.Format("{{\"QuestionID\":\"{0}\",\"Type\":\"{1}\",\"Token\":\"{2}\"}}", questions[i].ID, questions[i].QuestionType, questions[i].TokenName));
                }

                sectionQuestionJson.Append("[");
                sectionQuestionJson.Append(string.Join(",", lstskipJSON.ToArray()));
                sectionQuestionJson.Append("]");
                sectionQuestions = sectionQuestionJson.ToString();
                pSectionQuestion.Controls.Clear();
                pSectionQuestion.Controls.Add(new LiteralControl(sectionQuestions));
            }
        }

        protected void LVQuestions_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem listItem = (ListViewDataItem)e.Item;
                ServiceQuestion question = (ServiceQuestion)listItem.DataItem;

                Panel questionActionPanel = (Panel)listItem.FindControl("questionActionPanel");
                questionActionPanel.Visible = question.EnableZoomIn;

                Panel ctrContrainer = (Panel)listItem.FindControl("questionObject");
                HtmlGenericControl questionDiv = (HtmlGenericControl)listItem.FindControl("questionDiv");
                HtmlGenericControl controlTypes = (HtmlGenericControl)listItem.FindControl("controlTypes");



                Label questiontxt111 = (Label)listItem.FindControl("testText");
                questiontxt111.Text = question.QuestionTitle.Trim();
                if (question.FieldMandatory)
                {

                    questiontxt111.Text = string.Format("{0}<b style='color:red;'>*</b>", question.QuestionTitle.Trim());
                }

                HiddenField hdnQuestionHelp = (HiddenField)listItem.FindControl("hdnQuestionHelp");
                Image imgquestionHelp = (Image)listItem.FindControl("imgquestionHelp");
                if (string.IsNullOrEmpty(hdnQuestionHelp.Value))
                    imgquestionHelp.Visible = false;
                else
                {
                    string questiondetailsUrl = UGITUtility.GetAbsoluteURL(hdnQuestionHelp.Value);
                    imgquestionHelp.Attributes.Add("onclick", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','Service Question Help','1000px','90',0,'{1}')", questiondetailsUrl, Server.UrlEncode(Request.Url.AbsolutePath)));
                    if (question.NavigationType.EqualsIgnoreCase("HelpCard"))
                    {
                        imgquestionHelp.Attributes.Add("onclick", $"javascript:openHelpCard('{hdnQuestionHelp.Value}','Service')");
                    }

                }
                //Fetchs all questions of current question's section
                List<ServiceQuestion> sectionQuestions = service.Questions.Where(x => x.ServiceID == question.ServiceID).ToList();
                Control ctr = ExtendedControls.GenerateControlForQuestion(question, service.SkipSectionCondition);
                if (ctr != null)
                {
                    string txtMode = string.Empty;
                    string optiontype = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("textmode", out txtMode);
                    question.QuestionTypePropertiesDicObj.TryGetValue("optiontype", out optiontype);

                    //if (txtMode == "Multiline")
                    //    controlTypes.Attributes.Add("class", "col-md-12 commentBox");
                    //if (optiontype == "dropdown" || optiontype == "radiobuttons" || question.QuestionType == "Checkbox")
                    // {
                    //     //controlTypes.Attributes.Add("class", "col-md-4");

                    // }
                    // else if (question.QuestionType.Contains("ApplicationAccessRequest"))
                    // {
                    //     controlTypes.Attributes.Add("class", "col-md-12");
                    // }
                    // else if (question.QuestionTypeProperties.Split(';')[0].Contains("lookuplist=LocationLookup"))
                    // {
                    //     //controlTypes.Attributes.Add("class", "col-md-4 loacationDropDown");
                    // }
                    // else if (question.QuestionTypeProperties.Split(';')[0].Contains("lookuplist=BeneficiariesLookup")){
                    //     //controlTypes.Attributes.Add("class", "col-md-8");
                    // }
                    // else if (question.QuestionTypeProperties.Split(';')[0].Contains("lookuplist=FunctionalAreaTitleLookup"))
                    // {
                    //     //controlTypes.Attributes.Add("class", "col-md-4");
                    //     //controlTypes.Attributes.Add("style", "clear:both;");
                    // }
                    //else
                    //else
                    //    controlTypes.Attributes.Add("class", "col-md-4");


                    questionDiv.Attributes.Add("class", string.Format("question fleft w_92 questiondiv_{0}", question.ID));
                    if (service.SkipSectionCondition != null && service.SkipSectionCondition.Exists(x => x.ConditionValidate && x.SkipQuestionsID.Exists(y => y == question.ID)))
                    {
                        questionDiv.Attributes.Add("style", "display:none;");
                    }
                    FillDefaultDataForCtls(question, ctr);
                    ctrContrainer.Controls.Add(ctr);

                }
            }
        }

        void FillDefaultDataForCtls(ServiceQuestion question, Control ctr)
        {
            if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets)
            {
                if (question.QuestionTypePropertiesDicObj.Count > 0)
                {
                    foreach (Control c in ctr.Controls)
                    {
                        if (string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID) == c.ID)
                        {
                            ASPxGridLookup combox = c as ASPxGridLookup;


                            KeyValuePair<string, string> defaultvalAsset = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                            KeyValuePair<string, string> currentuser = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "currentuser");
                            KeyValuePair<string, string> specificuser = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "specificuser");
                            KeyValuePair<string, string> userquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "userquestion");
                            KeyValuePair<string, string> includedepartmentasset = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "includedepartmentasset");

                            DataTable dt = null;
                            if (currentuser.Key != null)
                            {
                                UserProfile user = HttpContext.Current.CurrentUser();
                                if (includedepartmentasset.Key == null)
                                    dt = AssetHelper.GetUserAssets(HttpContext.Current.GetManagerContext(), user.Id);
                                else
                                    dt = AssetHelper.GetAssetsByDepartment(HttpContext.Current.GetManagerContext(), user.DepartmentId);
                            }
                            else if (userquestion.Key != null)
                            {

                                ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(userquestion.Value));
                                if (inputQuestion != null)
                                {
                                    string tokenname = inputQuestion.TokenName;
                                    long? sectionid = inputQuestion.ServiceSectionID;
                                    ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                    string uValue = null;
                                    ServiceQuestionInput iqValue = null;
                                    if (sections != null)
                                    {
                                        iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                        if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                            uValue = UGITUtility.ObjectToString(iqValue.Value);
                                    }
                                    if (iqValue == null)
                                    {
                                        if (inputQuestion.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                                        {
                                            if (inputQuestion.QuestionTypePropertiesDicObj.Count > 0)
                                            {
                                                KeyValuePair<string, string> defaultval = inputQuestion.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                                                KeyValuePair<string, string> loggedinuser = inputQuestion.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "loggedinuser");
                                                if (defaultval.Key != null)
                                                {
                                                    uValue = defaultval.Value;
                                                    //UserProfile currentUser = currentUser = _userProfileManager.GetUserById(defaultval.Value);
                                                    //if (currentUser != null)
                                                    //    uValue = new SPFieldUserValue(SPContext.Current.Web, currentUser.ID, currentUser.LoginName);
                                                }
                                                else if (loggedinuser.Key != null)
                                                {
                                                    uValue = ApplicationContext.CurrentUser.Id;
                                                    //uValue = new SPFieldUserValue(SPContext.Current.Web, SPContext.Current.Web.CurrentUser.ID, SPContext.Current.Web.CurrentUser.LoginName);
                                                }
                                            }
                                        }
                                    }
                                    if (uValue != null)
                                    {
                                        if (includedepartmentasset.Key == null)
                                        {
                                            dt = AssetHelper.GetUserAssets(ApplicationContext, uValue);
                                        }
                                        else
                                        {
                                            UserProfile user = UserProfileManager.GetUserById(Convert.ToString(uValue));
                                            if (user != null)
                                                dt = AssetHelper.GetAssetsByDepartment(ApplicationContext, UGITUtility.StringToInt(user.Department));
                                        }
                                    }
                                }
                            }

                            if (combox != null && combox.DataSource == null && dt != null)
                                combox.DataSource = dt;

                            if (defaultvalAsset.Key != null)
                            {
                                List<string> asset = UGITUtility.ConvertStringToList(defaultvalAsset.Value,Constants.Separator6);
                                if (asset != null)
                                {
                                    foreach (string itemAssest in asset)
                                    {
                                        combox.GridView.Selection.SetSelectionByKey(itemAssest, true);
                                    }
                                }
                            }


                        }
                    }
                }
            }
            else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.ApplicationAccess)
            {
                if (question.QuestionTypePropertiesDicObj.Count > 0)
                {
                    foreach (Control c in ctr.Controls)
                    {
                        if (string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID) == c.ID)
                        {
                            Panel pApplications = (Panel)c;
                            ServiceMatrix serviceMatrix = (ServiceMatrix)pApplications.Controls[0];
                            List<ServiceMatrixData> lstServiceMatrixData = new List<ServiceMatrixData>();
                            KeyValuePair<string, string> userquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.ExistingUser);
                            KeyValuePair<string, string> newUSer = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.NewUSer);
                            KeyValuePair<string, string> mirroraccessfrom = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.MirrorAccessFrom);
                            if (userquestion.Key != null)
                            {
                                ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(userquestion.Value));
                                if (inputQuestion != null)
                                {
                                    string tokenname = inputQuestion.TokenName;
                                    int sectionid = Convert.ToInt32(inputQuestion.ServiceSectionID);
                                    ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                    if (sections != null)
                                    {
                                        ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                        if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                        {
                                            UserProfile user = HttpContext.Current.GetUserManager().GetUserById(UGITUtility.RemoveIDsFromLookupString(iqValue.Value));
                                            if (user != null)
                                            {
                                                serviceMatrix.RoleAssignee = Convert.ToString(user.Id);
                                            }
                                        }

                                    }
                                }
                            }
                            if (mirroraccessfrom.Key != null)
                            {
                                ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(mirroraccessfrom.Value));
                                if (inputQuestion != null)
                                {
                                    string tokenname = inputQuestion.TokenName;
                                    int sectionid = Convert.ToInt32(inputQuestion.ServiceSectionID);
                                    ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                    if (sections != null)
                                    {
                                        ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                        if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                        {
                                            string uValue = iqValue.Value;
                                            serviceMatrix.MirrorAccessFromUser = Convert.ToString(uValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.SINGLECHOICE)
            {
                if (question.QuestionTypePropertiesDicObj.Count > 0)
                {
                    foreach (Control c in ctr.Controls)
                    {
                        if (string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID) == c.ID)
                        {
                            KeyValuePair<string, string> selectedFields = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "selectedfield");
                            KeyValuePair<string, string> defaultvalSingleChoice = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                            if (selectedFields.Key != null && !string.IsNullOrEmpty(moduleName))
                            {
                                string choiceField = GetChoiceFields(selectedFields.Value);
                                if (!string.IsNullOrEmpty(choiceField))
                                {
                                    DataTable table = new DataTable();
                                    ServiceQuestion requestTypeQuestion = service.Questions.FirstOrDefault(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE);
                                    if (requestTypeQuestion == null)
                                        continue;
                                    ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == requestTypeQuestion.ServiceSectionID);
                                    if (sections != null)
                                    {
                                        ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token.ToLower() == requestTypeQuestion.TokenName.ToLower());
                                        if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                        {
                                            LookUpValueBox requestType = new LookUpValueBox();
                                            if (requestType != null)
                                                table = BindRequestType(requestType, choiceField);
                                        }
                                    }
                                    else
                                        table = BindChoiceFields(choiceField);
                                    if (table != null && table.Rows.Count > 0)
                                    {
                                        table = table.DefaultView.ToTable(true, choiceField);
                                        if (c is RadioButtonList)
                                        {
                                            RadioButtonList rdbRequestType = c as RadioButtonList;
                                            rdbRequestType.DataTextField = choiceField;
                                            rdbRequestType.DataValueField = choiceField;
                                            rdbRequestType.DataSource = table;
                                            rdbRequestType.DataBind();
                                            if (defaultvalSingleChoice.Key != null)
                                            {
                                                ListItem lItem = rdbRequestType.Items.Cast<ListItem>().FirstOrDefault(x => x.Value.ToLower() == defaultvalSingleChoice.Value.ToLower());
                                                if (lItem != null)
                                                {
                                                    lItem.Selected = true;
                                                }
                                            }
                                        }
                                        else if (c is DropDownList)
                                        {
                                            DropDownList ddlRequestType = c as DropDownList;
                                            ddlRequestType.DataTextField = choiceField;
                                            ddlRequestType.DataValueField = choiceField;
                                            ddlRequestType.DataSource = table;
                                            ddlRequestType.DataBind();
                                            ddlRequestType.Items.Insert(0, new ListItem("(None)", ""));
                                            if (defaultvalSingleChoice.Key != null)
                                            {
                                                ListItem lItem = ddlRequestType.Items.Cast<ListItem>().FirstOrDefault(x => x.Value.ToLower() == defaultvalSingleChoice.Value.ToLower());
                                                if (lItem != null)
                                                {
                                                    lItem.Selected = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.MULTICHOICE)
            {
                if (question.QuestionTypePropertiesDicObj.Count > 0)
                {
                    foreach (Control c in ctr.Controls)
                    {
                        if (string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID) == c.ID)
                        {
                            CheckBoxList chkbxList = c as CheckBoxList;
                            KeyValuePair<string, string> selectedFields = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "selectedfield");
                            KeyValuePair<string, string> defaultvalSingleChoice = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                            if (selectedFields.Key != null && !string.IsNullOrEmpty(moduleName))
                            {
                                string choiceField = GetChoiceFields(selectedFields.Value);
                                if (!string.IsNullOrEmpty(choiceField))
                                {
                                    DataTable table = BindChoiceFields(choiceField);
                                    if (table != null && table.Rows.Count > 0)
                                    {
                                        table = table.DefaultView.ToTable(true, choiceField);
                                        chkbxList.DataTextField = choiceField;
                                        chkbxList.DataValueField = choiceField;
                                        chkbxList.DataSource = table;
                                        chkbxList.DataBind();
                                        if (defaultvalSingleChoice.Key != null)
                                        {
                                            LookUpValueBox defaultChoices = new LookUpValueBox();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME)
            {
                if (question.QuestionTypePropertiesDicObj.Count > 0)
                {
                    ASPxDateEdit datetimeCtr = ctr.Controls.Cast<Control>().FirstOrDefault(x => x.ID == string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID)) as ASPxDateEdit;
                    if (datetimeCtr != null)
                    {
                        // datetimeCtr.CssClass = "";
                        DateTime defaultDate = new DateTime();

                        string dateconstraint = string.Empty;
                        string pastdate = string.Empty;
                        string futuredate = string.Empty;
                        string presentdate = string.Empty;

                        question.QuestionTypePropertiesDicObj.TryGetValue("dateconstraint", out dateconstraint);
                        question.QuestionTypePropertiesDicObj.TryGetValue("pastdate", out pastdate);
                        question.QuestionTypePropertiesDicObj.TryGetValue("futuredate", out futuredate);
                        question.QuestionTypePropertiesDicObj.TryGetValue("presentdate", out presentdate);

                        if (dateconstraint == "conditional")
                        {
                            DateTime minDate = DateTime.MinValue;
                            DateTime maxDate = DateTime.MaxValue;
                            KeyValuePair<string, string> validateAgainst = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "validateagainst");
                            if (validateAgainst.Key != null && validateAgainst.Value.ToLower() != "currentdate")
                            {
                                ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(validateAgainst.Value));
                                if (inputQuestion != null)
                                {
                                    string tokenname = inputQuestion.TokenName;
                                    long? sectionid = inputQuestion.ServiceSectionID;
                                    ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                    if (sections != null)
                                    {
                                        ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                        if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                        {
                                            defaultDate = Convert.ToDateTime(iqValue.Value);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                defaultDate = DateTime.Now.Date;
                            }
                            if (defaultDate == DateTime.MinValue)
                                return;
                            datetimeCtr.CssClass = ExtendedControls.GetDateValidationString(defaultDate, UGITUtility.StringToBoolean(pastdate), UGITUtility.StringToBoolean(presentdate), UGITUtility.StringToBoolean(futuredate));
                        }
                    }
                }
            }
            else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.LOOKUP)
            {
                if (question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist").Value.ToLower() == DatabaseObjects.Columns.LocationLookup.ToLower())
                {
                    if (question.QuestionTypePropertiesDicObj.Count > 0)
                    {
                        foreach (Control c in ctr.Controls)
                        {
                            if (string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID) == c.ID)
                            {
                                bool isMulti = false;
                                string isMultiS = string.Empty;
                                question.QuestionTypePropertiesDicObj.TryGetValue("multi", out isMultiS);
                                isMulti = UGITUtility.StringToBoolean(isMultiS);

                                KeyValuePair<string, string> locationuserquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "locationuserquestion");
                                ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(locationuserquestion.Value));
                                if (inputQuestion != null)
                                {
                                    string tokenname = inputQuestion.TokenName;
                                    long? sectionid = inputQuestion.ServiceSectionID;

                                    ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);

                                    if (sections != null)
                                    {
                                        ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);

                                        if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                        {
                                            List<UserProfile> lstUserProfile = UserProfileManager.GetUserInfosById(iqValue.Value);
                                            LookUpValueBox locationBox = c as LookUpValueBox;

                                            if (lstUserProfile != null && lstUserProfile.Count > 0)
                                            {
                                                List<string> lstLocation = new List<string>();
                                                foreach (UserProfile item in lstUserProfile)
                                                {
                                                    UserProfile user = UserProfileManager.LoadById(item.Id);
                                                    lstLocation.Add(Convert.ToString(user.Location));
                                                }
                                                locationBox.SetValues(string.Join(Constants.Separator6, lstLocation));
                                            }
                                            else
                                            {
                                                UserProfile users = UserProfileManager.LoadById(lstUserProfile.FirstOrDefault().Id);
                                                if (users != null)
                                                {
                                                    locationBox.SetValues(Convert.ToString(users.Location));
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LookUpValueBox locationBox = c as LookUpValueBox;
                                        if (!string.IsNullOrEmpty(locationBox.DefaultValue))
                                            locationBox.SetValues(locationBox.DefaultValue);
                                        else if (inputQuestion.QuestionTypePropertiesDicObj.ContainsKey("defaultval"))
                                        {
                                            KeyValuePair<string, string> defaultVal = inputQuestion.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key == "defaultval");
                                            List<UserProfile> lstUserProfile = UserProfileManager.GetUserInfosById(defaultVal.Value);

                                            if (lstUserProfile != null && lstUserProfile.Count > 0)
                                            {
                                                List<string> lstLocation = new List<string>();
                                                foreach (UserProfile item in lstUserProfile)
                                                {
                                                    UserProfile user = UserProfileManager.LoadById(item.Id);
                                                    lstLocation.Add(Convert.ToString(user.Location));
                                                }
                                                locationBox.SetValues(string.Join(Constants.Separator6, lstLocation));
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                else if (question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist").Value.ToLower() == DatabaseObjects.Columns.DepartmentLookup.ToLower())
                {
                    if (question.QuestionTypePropertiesDicObj.Count > 0)
                    {
                        foreach (Control c in ctr.Controls)
                        {
                            if (string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID) == c.ID)
                            {
                                bool isMulti = false;
                                string isMultiS = string.Empty;
                                question.QuestionTypePropertiesDicObj.TryGetValue("multi", out isMultiS);
                                isMulti = UGITUtility.StringToBoolean(isMultiS);

                                string value = string.Empty;
                                KeyValuePair<string, string> departmentuserquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "departmentuserquestion");
                                ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(departmentuserquestion.Value));
                                if (inputQuestion != null)
                                {
                                    string tokenname = inputQuestion.TokenName;
                                    long? sectionid = inputQuestion.ServiceSectionID;

                                    ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                    if (sections != null)
                                    {
                                        ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);

                                        if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                        {
                                            List<UserProfile> lstUserProfile = UserProfileManager.GetUserInfosById(iqValue.Value);
                                            if (lstUserProfile != null && lstUserProfile.Count > 0)
                                            {
                                                List<string> lstDepartment = new List<string>();
                                                foreach (UserProfile item in lstUserProfile)
                                                {
                                                    UserProfile user = UserProfileManager.LoadById(item.Id);
                                                    lstDepartment.Add(Convert.ToString(user.Department));
                                                }
                                                value = string.Join(Constants.Separator6, lstDepartment);
                                            }
                                            //else
                                            //{
                                            //    UserProfile users = UserProfileManager.LoadById(lstUserProfile.FirstOrDefault().Id);
                                            //    if (users != null)
                                            //    {
                                            //        value = Convert.ToString(users.Department);
                                            //    }
                                            //}
                                        }
                                    }
                                    else
                                    {
                                        if (inputQuestion.QuestionTypePropertiesDicObj.ContainsKey("defaultval"))
                                        {
                                            KeyValuePair<string, string> defaultVal = inputQuestion.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key == "defaultval");
                                            List<UserProfile> lstUserProfile = UserProfileManager.GetUserInfosById(defaultVal.Value);
                                            if (lstUserProfile != null && lstUserProfile.Count > 0)
                                            {
                                                List<string> lstDepartment = new List<string>();
                                                foreach (UserProfile item in lstUserProfile)
                                                {
                                                    UserProfile user = UserProfileManager.LoadById(item.Id);
                                                    lstDepartment.Add(Convert.ToString(user.Department));
                                                }
                                                value = string.Join(Constants.Separator6, lstDepartment);
                                            }
                                            //else
                                            //{
                                            //    if (lstUserProfile != null && lstUserProfile.Count > 0)
                                            //    {
                                            //        UserProfile users = UserProfileManager.LoadById(lstUserProfile.FirstOrDefault().Id);
                                            //        if (users != null)
                                            //        {
                                            //            value = Convert.ToString(users.Department);
                                            //        }
                                            //    }
                                            //}
                                        }
                                    }
                                }

                                FieldConfigurationManager configFieldManager = new FieldConfigurationManager(ApplicationContext);
                                FieldConfiguration configField = configFieldManager.GetFieldByFieldName(DatabaseObjects.Columns.DepartmentLookup);

                                if (configField != null)
                                {
                                    if (!string.IsNullOrEmpty(configField.TemplateType) && isMulti == true)
                                    {
                                        LookupValueBoxEdit dropDownBox = c as LookupValueBoxEdit;
                                        dropDownBox.Value = value;
                                        dropDownBox.dropBox.Value = value;
                                        dropDownBox.dropBox.KeyValue = value;
                                    }
                                    else
                                    {
                                        LookUpValueBox dropDownBox = c as LookUpValueBox;
                                        dropDownBox.SetValues(value);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist").Value.ToLower() == "sublocation")
                {

                }
            }
        }

        private DataTable BindRequestType(LookUpValueBox requestType, string choiceField)
        {
            DataTable table = new DataTable();
            string strType = string.Empty;
            string viewFields = string.Format("<FieldRef Name='{0}' Nullable='True'/>", choiceField);
            return table;
        }

        private string GetChoiceFields(string selectedValue)
        {
            string choiceField = string.Empty;
            switch (selectedValue.ToLower())
            {
                case "issue type":
                    choiceField = DatabaseObjects.Columns.IssueTypeOptions;
                    break;
                case "resolution type":
                    choiceField = DatabaseObjects.Columns.ResolutionTypes;
                    break;
            }
            return choiceField;
        }

        private DataTable BindChoiceFields(string choiceField)
        {
            DataTable table = new DataTable();
            string strType = string.Empty;
            foreach (string item in ticketIdsForAgent)
            {
                DataRow currentTicket = Ticket.GetCurrentTicket(ApplicationContext, moduleName, item);
                if (currentTicket != null && currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                {
                }
            }

            return table;
        }

        private DataTable BindResolutionType(ServiceQuestion question)
        {
            DataTable table = new DataTable();
            KeyValuePair<string, string> selectedFields = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "selectedfield");
            if (selectedFields.Key != null && selectedFields.Value.ToLower() == "resolution type" && !string.IsNullOrEmpty(moduleName))
            {
                string strType = string.Empty;
                foreach (string item in ticketIdsForAgent)
                {
                    DataRow currentTicket = Ticket.GetCurrentTicket(ApplicationContext, moduleName, item);
                    if (currentTicket != null && currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                    {

                    }
                }

            }
            return table;
        }

        //Get control value
        private void UpdateServiceInputObj(bool isNextPrevious)
        {
            if (service != null)
            {
                ServiceSection section = service.Sections.FirstOrDefault(x => x.ID == currentStepSectionID);
                List<ServiceQuestion> lstWithOldIndex = new List<ServiceQuestion>();
                List<ServiceQuestion> questions = service.Questions.Where(x => x.ServiceSectionID == currentStepSectionID).OrderBy(x => x.ItemOrder).ToList();
                #region If application access order less then user question order

                if (questions != null && questions.Count > 0)
                {
                    //To get item from lvQuestions list as per their index ,to keep old index as it is
                    lstWithOldIndex.AddRange(questions);
                    ServiceQuestion applicationAccessQues = questions.FirstOrDefault(x => x.QuestionType.ToLower() == "applicationaccessrequest");
                    ServiceQuestion userTypeQues = questions.FirstOrDefault(x => x.QuestionType.ToLower() == "userfield");
                    if (applicationAccessQues != null && userTypeQues != null && questions.IndexOf(applicationAccessQues) < questions.IndexOf(userTypeQues))
                    {
                        List<ServiceQuestion> lstApplAccessQues = questions.FindAll(x => x.QuestionType.ToLower() == "applicationaccessrequest");
                        //Change index
                        foreach (ServiceQuestion sQues in lstApplAccessQues)
                        {
                            questions.Remove(sQues);
                        }

                        //Add at end help us to set use question value first
                        foreach (ServiceQuestion sQues in lstApplAccessQues)
                        {
                            questions.Add(sQues);
                        }
                    }
                }

                #endregion
                _serviceInput.ServiceID = service.ID;
                ServiceSectionInput sectionInput = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == currentStepSectionID);
                if (sectionInput == null)
                {
                    sectionInput = new ServiceSectionInput();
                    sectionInput.SectionID = currentStepSectionID;
                    _serviceInput.ServiceSections.Add(sectionInput);
                }
                if (sectionInput.Questions == null)
                {
                    sectionInput.Questions = new List<ServiceQuestionInput>();
                }

                List<ServiceQuestionInput> sectionQuestionInputs = sectionInput.Questions;
                List<QuestionList> lstOfItems = GetQuestionGrouping(questions);
                foreach (QuestionList viewItem in lstOfItems)
                {
                    ListViewDataItem groupListItem = lvgroupquestions.Items[lstOfItems.IndexOf(viewItem)];
                    ListView lvquestion = (ListView)groupListItem.FindControl("lvQuestions");
                    foreach (ServiceQuestion question in viewItem.qLists)
                    {
                        ServiceQuestionInput input = sectionQuestionInputs.FirstOrDefault(x => x.Token == question.TokenName);
                        ListViewDataItem listviewDItem = lvquestion.Items[viewItem.qLists.IndexOf(question)];

                        if (input == null)
                        {
                            input = new ServiceQuestionInput();
                            input.Token = question.TokenName;
                            sectionQuestionInputs.Add(input);
                        }

                        Control ctr = listviewDItem.FindControlRecursive(string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID));
                        KeyValuePair<string, string> selectedFields = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                        if (ctr != null)
                        {
                            switch (question.QuestionType.ToLower())
                            {
                                case "singlechoice":
                                    {
                                        if (ctr is RadioButtonList)
                                        {
                                            RadioButtonList ddlCtr = (RadioButtonList)ctr;
                                            input.Value = ddlCtr.SelectedValue;
                                        }
                                        else if (ctr is ASPxComboBox)
                                        {
                                            ASPxComboBox ddlCtr = (ASPxComboBox)ctr;
                                            if (!string.IsNullOrEmpty(Convert.ToString(ddlCtr.Value)))
                                            {
                                                input.Value = Convert.ToString(ddlCtr.Value);

                                            }
                                            else
                                            {
                                                input.Value = Convert.ToString(selectedFields.Value);
                                                ddlCtr.SelectedItem = ddlCtr.Items.FindByValue(selectedFields.Value);
                                            }
                                        }
                                        else
                                        {
                                            DropDownList ddlCtr = (DropDownList)ctr;
                                            input.Value = ddlCtr.SelectedValue;
                                        }
                                    }
                                    break;
                                case "multichoice":
                                    {
                                        List<string> selectedVals = new List<string>();

                                        if (ctr is ASPxGridLookup)
                                        {
                                            ASPxGridLookup gridLookup = (ASPxGridLookup)ctr;
                                            if (gridLookup != null && !string.IsNullOrEmpty(gridLookup.Text))
                                            {
                                                selectedVals.Add(Convert.ToString(gridLookup.Text));
                                            }
                                        }
                                        else if (ctr is CheckBoxList)
                                        {
                                            CheckBoxList ddlCtr = (CheckBoxList)ctr;
                                            foreach (ListItem item in ddlCtr.Items)
                                            {
                                                if (item.Selected)
                                                {
                                                    selectedVals.Add(item.Value);
                                                }
                                            }

                                        }

                                        input.Value = string.Join(Constants.Separator5, selectedVals.ToArray());
                                    }
                                    break;
                                case "checkbox":
                                    {
                                        CheckBox checkbox = (CheckBox)ctr;
                                        input.Value = checkbox.Checked.ToString();
                                    }
                                    break;
                                case "textbox":
                                    {
                                        TextBox txtboxCtr = (TextBox)ctr;
                                        KeyValuePair<string, string> userdesklocationquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "dependentuserdesklocationquestion");
                                        ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(userdesklocationquestion.Value));

                                        if (inputQuestion != null && !isNextPrevious)
                                        {
                                            string tokenname = inputQuestion.TokenName;
                                            long? sectionid = inputQuestion.ServiceSectionID;
                                            ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                            if (sections != null)
                                            {
                                                ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                                if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                                {
                                                    UserProfile user = UserProfileManager.GetUserInfoById(iqValue.Value);

                                                    if (user != null)
                                                        input.Value = !string.IsNullOrEmpty(user.DeskLocation) ? user.DeskLocation.Trim() : string.Empty;
                                                }
                                                else
                                                {
                                                    input.Value = string.Empty;
                                                }

                                                txtboxCtr.Text = input.Value;
                                            }
                                        }
                                        else
                                        {
                                            input.Value = txtboxCtr.Text.Trim();
                                        }

                                        if (!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest() && question.TokenName == "username")
                                        {
                                            txtboxCtr.Visible = false;
                                        }

                                    }
                                    break;
                                case "number":
                                    {
                                        TextBox txtboxCtr = (TextBox)ctr;
                                        input.Value = txtboxCtr.Text.Trim();
                                    }
                                    break;
                                case "datetime":
                                    {
                                        ASPxDateEdit datetimeCtr = (ASPxDateEdit)ctr;
                                        datetimeCtr.CssClass = "CRMDueDate_inputField";
                                        datetimeCtr.DropDownButton.Image.Url = "~/Content/Images/calendarNew.png";
                                        datetimeCtr.DropDownButton.Image.Width = Unit.Pixel(16);
                                        if (datetimeCtr.Date != datetimeCtr.MinDate)
                                        {
                                            input.Value = Convert.ToString(datetimeCtr.Date);
                                            if (Convert.ToString(datetimeCtr.Date) == Convert.ToString(datetimeCtr.MaxDate))
                                                input.Value = string.Empty;
                                        }
                                        else
                                        {
                                            input.Value = Convert.ToString(datetimeCtr.Date);
                                        }
                                            //input.Value = string.Empty;
                                    }
                                    break;
                                case "userfield":
                                    {
                                        if (ctr is ASPxComboBox)
                                        {
                                            ASPxComboBox cmbUsers = (ASPxComboBox)ctr;
                                            cmbUsers.CssClass = "cmbUsers";
                                            if (cmbUsers != null && cmbUsers.SelectedItem != null)
                                                input.Value = string.Format("{0}{1}{2}", ((ASPxComboBox)ctr).SelectedItem.Value, Constants.Separator, ((ASPxComboBox)ctr).SelectedItem.Text);
                                        }
                                        else
                                        {
                                            UserValueBox peopleCtr = (UserValueBox)ctr;

                                            List<string> userPairList = new List<string>();
                                            KeyValuePair<string, string> userManagerQuestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "dependentusermanagerquestion");
                                            ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(userManagerQuestion.Value));

                                            if (inputQuestion != null && !isNextPrevious)
                                            {
                                                string tokenname = inputQuestion.TokenName;
                                                long? sectionid = inputQuestion.ServiceSectionID;
                                                ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                                if (sections != null)
                                                {
                                                    ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                                    if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                                    {
                                                        List<UserProfile> lstUserProfile = UserProfileManager.GetUserInfosById(iqValue.Value);
                                                        if (lstUserProfile != null && lstUserProfile.Count > 0)
                                                        {
                                                            foreach (UserProfile u in lstUserProfile)
                                                            {
                                                                if (User != null)
                                                                    userPairList.Add(string.Format("{0}", u.ManagerID));
                                                                else
                                                                    userPairList.Add(string.Format("{0}", u.ManagerID));
                                                            }
                                                            input.Value = string.Join(Constants.Separator6, userPairList.ToArray());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        input.Value = string.Empty;
                                                    }
                                                    peopleCtr.SetValues(input.Value);
                                                }
                                            }
                                            else
                                            {
                                                /*
                                                List<UserProfile> userVals = HttpContext.Current.GetUserManager().GetUserInfosById(peopleCtr.GetValues()); //uHelper.GetFieldUserValueCollection(peopleCtr.ResolvedEntities, SPContext.Current.Web);
                                                if (userVals != null && userVals.Count > 0)
                                                {
                                                    foreach (UserProfile u in userVals)
                                                    {
                                                        if (User != null)
                                                            userPairList.Add(string.Format("{0}", u.Id));
                                                        else
                                                            userPairList.Add(string.Format("{0}", u.Id));
                                                    }
                                                }
                                                */
                                                // Added below code, as selected Group Names are not added in userPairList List<>; Fetching only Users.
                                                List<string> userVals = UGITUtility.ConvertStringToList(peopleCtr.GetValues(), Constants.Separator6);
                                                if (userVals != null && userVals.Count > 0)
                                                {
                                                    userPairList.AddRange(userVals);
                                                }
                                                input.Value = string.Join(Constants.Separator6, userPairList.ToArray());
                                            }
                                        }
                                    }
                                    break;
                                case "lookup":
                                    {
                                        if (ctr is LookupValueBoxEdit)
                                        {
                                            if (question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist").Value.ToLower() == DatabaseObjects.Columns.DepartmentLookup.ToLower()
                                                || question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist").Value.ToLower() == DatabaseObjects.Columns.TicketBeneficiaries.ToLower())
                                            {
                                                if (question.QuestionTypePropertiesDicObj.Count > 0)
                                                {
                                                    if (string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID) == ctr.ID)
                                                    {
                                                        KeyValuePair<string, string> departmentuserquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "departmentuserquestion");
                                                        ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(departmentuserquestion.Value));

                                                        if (inputQuestion != null && !isNextPrevious)
                                                        {
                                                            string tokenname = inputQuestion.TokenName;
                                                            long? sectionid = inputQuestion.ServiceSectionID;

                                                            ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                                            if (sections != null)
                                                            {
                                                                ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);

                                                                if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                                                {
                                                                    List<UserProfile> lstUserProfile = UserProfileManager.GetUserInfosById(iqValue.Value);
                                                                    if (lstUserProfile != null && lstUserProfile.Count > 0)
                                                                    {
                                                                        List<string> lstDepartment = new List<string>();
                                                                        foreach (UserProfile item in lstUserProfile)
                                                                        {
                                                                            lstDepartment.Add(Convert.ToString(item.Department));
                                                                        }
                                                                        input.Value = string.Join(Constants.Separator6, lstDepartment);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (lstUserProfile != null && lstUserProfile.Count > 0)
                                                                        {
                                                                            UserProfile users = UserProfileManager.LoadById(lstUserProfile.FirstOrDefault().Id);
                                                                            if (users != null)
                                                                            {
                                                                                input.Value = Convert.ToString(users.Department);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                    input.Value = string.Empty;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            input.Value = ((LookupValueBoxEdit)ctr).GetValues();
                                                            if (!string.IsNullOrEmpty(((LookupValueBoxEdit)ctr).DefaultValue) && string.IsNullOrEmpty(input.Value))
                                                                input.Value = ((LookupValueBoxEdit)ctr).DefaultValue;
                                                        }

                                                        LookupValueBoxEdit dropDownBox = ctr as LookupValueBoxEdit;
                                                        dropDownBox.Value = input.Value;
                                                        dropDownBox.dropBox.Value = input.Value;
                                                        dropDownBox.dropBox.KeyValue = input.Value;
                                                        dropDownBox.SetValues(input.Value);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ctr is DropDownList)
                                            {
                                                DropDownList lookupCtr = (DropDownList)ctr;
                                                input.Value = string.Empty;
                                                if (lookupCtr.SelectedValue != string.Empty)
                                                {
                                                    input.Value = string.Format("{0}{1}{2}", lookupCtr.SelectedValue, Constants.Separator, lookupCtr.SelectedItem.Text);
                                                }
                                            }
                                            else if (ctr is ASPxDropDownEdit)
                                            {
                                                ASPxDropDownEdit lookupMultiCtr = (ASPxDropDownEdit)ctr;
                                                lookupMultiCtr.CssClass = "lookupMultiCtr";
                                                string checkboxID = string.Format("checkListBox_lookupMultiCtr_{0}", Convert.ToString(lookupMultiCtr.ID).Split('_')[2]);
                                                ASPxListBox listBox = (ASPxListBox)lookupMultiCtr.FindControl(checkboxID);
                                                input.Value = string.Empty;
                                                if (listBox.SelectedItems.Count > 0)
                                                {
                                                    List<string> selectedLookupidse = new List<string>();
                                                    foreach (ListEditItem selectedItem in listBox.SelectedItems)
                                                    {
                                                        if (!string.IsNullOrEmpty(Convert.ToString(selectedItem.Value)) && Convert.ToString(selectedItem.Text) != "All")
                                                        {
                                                            selectedLookupidse.Add(string.Format("{0}{2}{1}", Convert.ToString(selectedItem.Value), selectedItem.Text.Trim(), Constants.Separator));
                                                        }
                                                    }
                                                    string commaSeperatedIds = string.Join(Constants.Separator, selectedLookupidse.ToArray());
                                                    input.Value = commaSeperatedIds;
                                                }
                                            }
                                            else if (ctr is LookUpValueBox)
                                            {
                                                if (question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist").Value.ToLower() == DatabaseObjects.Columns.LocationLookup.ToLower())
                                                {
                                                    if (question.QuestionTypePropertiesDicObj.Count > 0)
                                                    {
                                                        if (string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID) == ctr.ID)
                                                        {
                                                            bool isMulti = false;
                                                            string isMultiS = string.Empty;
                                                            question.QuestionTypePropertiesDicObj.TryGetValue("multi", out isMultiS);
                                                            isMulti = UGITUtility.StringToBoolean(isMultiS);

                                                            KeyValuePair<string, string> locationuserquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "locationuserquestion");
                                                            ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(locationuserquestion.Value));

                                                            if (inputQuestion != null && !isNextPrevious)
                                                            {
                                                                string tokenname = inputQuestion.TokenName;
                                                                long? sectionid = inputQuestion.ServiceSectionID;

                                                                ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);

                                                                if (sections != null)
                                                                {
                                                                    ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);

                                                                    if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                                                    {
                                                                        List<UserProfile> lstUserProfile = UserProfileManager.GetUserInfosById(iqValue.Value);

                                                                        if (lstUserProfile != null && lstUserProfile.Count > 0)
                                                                        {
                                                                            List<string> lstLocation = new List<string>();
                                                                            foreach (UserProfile item in lstUserProfile)
                                                                            {
                                                                                lstLocation.Add(Convert.ToString(item.Location));
                                                                            }

                                                                            input.Value = string.Join(Constants.Separator6, lstLocation);
                                                                        }
                                                                        else
                                                                        {
                                                                            UserProfile user = UserProfileManager.LoadById(lstUserProfile.FirstOrDefault().Id);
                                                                            if (user != null)
                                                                                input.Value = user.Location;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        input.Value = string.Empty;
                                                                    }
                                                                }

                                                                LookUpValueBox locationBox = ctr as LookUpValueBox;
                                                                locationBox.SetValues(input.Value);
                                                                //input.Value = locationBox.GetValues();
                                                            }
                                                            else
                                                            {
                                                                LookUpValueBox lookupValueBox = (LookUpValueBox)ctr;
                                                                input.Value = lookupValueBox.GetValues();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    LookUpValueBox lookupValueBox = (LookUpValueBox)ctr;
                                                    lookupValueBox.CssClass = "lookupValueBox-dropown";
                                                    input.Value = lookupValueBox.GetValues();
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case "requesttype":
                                    {
                                        //Panel pRequestType = (Panel)ctr;
                                        //DropDownList ddlRequestType = null;
                                        ASPxDropDownEdit cmbIssueType = null;
                                        LookupValueBoxEdit lookUpRequestType = null;
                                        List<string> values = new List<string>();
                                        if (ctr.Controls.Count > 0)
                                        {
                                            string baseID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                                            lookUpRequestType = ctr as LookupValueBoxEdit;
                                            cmbIssueType = ctr.Controls[0] as ASPxDropDownEdit;
                                            //pRequestType.FindControl(string.Format("{0}_issuetype", baseID)) as LookupValueBoxEdit;

                                        }


                                        if (lookUpRequestType != null)
                                        {
                                            string inputValue = lookUpRequestType.GetValues();
                                            if (!string.IsNullOrEmpty(inputValue) /*&& inputValue.Contains(Constants.Separator)*/)
                                            {
                                                input.Value = inputValue;
                                            }


                                            if (inputValue != null && inputValue.Contains(Constants.Separator))
                                            {
                                                if (input.SubTokensValue == null)
                                                    input.SubTokensValue = new List<ServiceQuestionInput>();
                                                ServiceQuestionInput issueTypeQuestionInput = input.SubTokensValue.FirstOrDefault();
                                                if (issueTypeQuestionInput == null)
                                                {
                                                    issueTypeQuestionInput = new ServiceQuestionInput();
                                                    input.SubTokensValue.Add(issueTypeQuestionInput);
                                                }
                                                issueTypeQuestionInput.Token = "issuetype";
                                                List<string> subvalues = UGITUtility.ConvertStringToList(inputValue, Constants.Separator);
                                                if (subvalues.Count > 1)
                                                    issueTypeQuestionInput.Value = subvalues[1];
                                            }
                                        }
                                    }
                                    break;
                                case "applicationaccessrequest":
                                    {
                                        Panel pApplications = (Panel)ctr;
                                        ServiceMatrix serviceMatrix = (ServiceMatrix)pApplications.Controls[0];
                                        string roleAssignee = serviceMatrix.RoleAssignee;
                                        string mirrorAccessFrom = serviceMatrix.MirrorAccessFromUser;
                                        List<ServiceMatrixData> lstServiceMatrixData = new List<ServiceMatrixData>();
                                        KeyValuePair<string, string> userquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.ExistingUser);
                                        KeyValuePair<string, string> mirroraccessfrom = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.MirrorAccessFrom);

                                        KeyValuePair<string, string> newUSer = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.NewUSer);
                                        if (userquestion.Key != null)
                                        {
                                            ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(userquestion.Value));
                                            if (inputQuestion != null)
                                            {
                                                string tokenname = inputQuestion.TokenName;
                                                int sectionid = Convert.ToInt32(inputQuestion.ServiceSectionID);
                                                ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                                if (sections != null)
                                                {
                                                    ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                                    if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                                    {
                                                        serviceMatrix.RoleAssignee = Convert.ToString(iqValue.Value);

                                                    }
                                                }
                                            }
                                        }
                                        if (mirroraccessfrom.Key != null)
                                        {
                                            ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(mirroraccessfrom.Value));
                                            if (inputQuestion != null)
                                            {
                                                string tokenname = inputQuestion.TokenName;
                                                int sectionid = Convert.ToInt32(inputQuestion.ServiceSectionID);
                                                ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                                if (sections != null)
                                                {
                                                    ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                                    if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                                    {
                                                        string uValue = iqValue.Value;
                                                        serviceMatrix.MirrorAccessFromUser = Convert.ToString(uValue);
                                                    }
                                                }
                                            }
                                        }
                                        List<ServiceMatrixData> lst = new List<ServiceMatrixData>();
                                        if (!string.IsNullOrEmpty(input.Value))
                                        {
                                            XmlDocument doc = new XmlDocument();
                                            doc.LoadXml(input.Value.Trim());
                                            lst = (List<ServiceMatrixData>)uHelper.DeSerializeAnObject(doc, lstServiceMatrixData);

                                            if (!isNextPrevious && ((lst[0].RoleAssignee != null && lst[0].RoleAssignee != serviceMatrix.RoleAssignee) || (lst[0].MirrorAccessFromUser != serviceMatrix.MirrorAccessFromUser)))
                                                serviceMatrix.Reload();
                                            else
                                            {
                                                serviceMatrix.SaveState(lst);
                                            }

                                        }
                                        else
                                        {
                                            if (!isNextPrevious && (roleAssignee != serviceMatrix.RoleAssignee || mirrorAccessFrom != serviceMatrix.MirrorAccessFromUser))
                                                serviceMatrix.Reload();
                                            else
                                                serviceMatrix.SaveState();
                                        }
                                        List<ServiceMatrixData> serviceMatricData = serviceMatrix.GetSavedState();
                                        if (serviceMatricData != null && serviceMatricData.Count > 0)
                                        {
                                            XmlDocument doc = uHelper.SerializeObject(serviceMatricData);
                                            if (doc != null)
                                                input.Value = doc.OuterXml;
                                        }
                                    }
                                    break;


                                case "assets lookup":
                                    {
                                        ASPxGridLookup cbx = ctr as ASPxGridLookup;
                                        if (ctr != null)
                                        {
                                            List<string> userPairList = new List<string>();
                                            List<string> lookupColl = new List<string>();

                                            List<object> values = cbx.GridView.GetSelectedFieldValues(DatabaseObjects.Columns.ID, DatabaseObjects.Columns.AssetTagNum);

                                            foreach (object[] item in values)
                                            {
                                                string lookupValue = Convert.ToString(item[1]);
                                                lookupColl.Add(Convert.ToString(item[0]));
                                            }
                                            
                                            input.Value = string.Join(Constants.Separator6, lookupColl.ToArray());
                                        }
                                    }
                                    break;
                                case Constants.ServiceQuestionType.Rating:
                                    RatingCtr ratingCtr = (RatingCtr)ctr;
                                    input.Value = ratingCtr.SelectedValue.ToString();
                                    break;

                            }
                        }
                    }
                }
            }
        }

        //Set Control Value
        private void UpdateCtrValue()
        {
            if (service != null && currentStepID == 2)
            {
                ServiceSectionInput sectionInput = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == currentStepSectionID);
                List<ServiceQuestionInput> sectionQuestionInputs = new List<ServiceQuestionInput>();
                if (sectionInput != null)
                {
                    sectionQuestionInputs = sectionInput.Questions;
                }

                List<ServiceQuestion> questions = service.Questions.Where(x => x.ServiceSectionID == currentStepSectionID).OrderBy(x => x.ItemOrder).ToList();
                List<QuestionList> questionLists = GetQuestionGrouping(questions);
                foreach (QuestionList groupQues in questionLists)
                {
                    ListViewDataItem groupListItem = lvgroupquestions.Items[questionLists.IndexOf(groupQues)];
                    foreach (ServiceQuestion question in groupQues.qLists)
                    {
                        ListView listView = (ListView)groupListItem.FindControl("lvQuestions");
                        ServiceQuestionInput input = sectionQuestionInputs.FirstOrDefault(x => x.Token == question.TokenName);
                        if (input != null)
                        {
                            ListViewDataItem listviewDItem = listView.Items[groupQues.qLists.IndexOf(question)];
                            Control ctr = listviewDItem.FindControlRecursive(string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID));

                            TextBox hdnMandatory = (TextBox)listviewDItem.FindControl(string.Format("txthiddenCtr_{0}", question.ID));
                            if (hdnMandatory != null && hdnMandatory.Text != "[$skip$]")
                                hdnMandatory.Text = input.Value;

                            if (ctr != null)
                            {
                                #region switch to set values in controls
                                switch (question.QuestionType.ToLower())
                                {
                                    case "singlechoice":
                                        {
                                            if (ctr is RadioButtonList)
                                            {
                                                RadioButtonList ddlCtr = (RadioButtonList)ctr;
                                                ddlCtr.SelectedIndex = ddlCtr.Items.IndexOf(ddlCtr.Items.FindByValue(input.Value));
                                            }
                                            else if (ctr is ASPxComboBox)
                                            {
                                                ASPxComboBox ddlCtr = (ASPxComboBox)ctr;
                                                ddlCtr.SelectedIndex = ddlCtr.Items.IndexOf(ddlCtr.Items.FindByValue(input.Value));
                                            }
                                            else
                                            {
                                                DropDownList ddlCtr = (DropDownList)ctr;
                                                ddlCtr.SelectedIndex = ddlCtr.Items.IndexOf(ddlCtr.Items.FindByValue(input.Value));
                                            }
                                        }
                                        break;
                                    case "multichoice":
                                        {


                                            if (ctr is ASPxGridLookup)
                                            {
                                                ASPxGridLookup gridLookup = (ASPxGridLookup)ctr;
                                                if (!string.IsNullOrEmpty(input.Value))
                                                {
                                                    string[] selectedVals = input.Value.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
                                                    foreach (string selectedVal in selectedVals)
                                                    {
                                                        if (selectedVal != null)
                                                        {

                                                            gridLookup.GridView.Selection.SetSelectionByKey(selectedVal, true);
                                                        }
                                                    }
                                                }


                                            }
                                            else if (ctr is CheckBoxList)
                                            {
                                                string[] selectedVals = input.Value.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
                                                CheckBoxList ddlCtr = (CheckBoxList)ctr;
                                                foreach (string selectedVal in selectedVals)
                                                {
                                                    ListItem item = ddlCtr.Items.FindByValue(selectedVal.Trim());
                                                    if (item != null)
                                                    {
                                                        item.Selected = true;
                                                    }
                                                }
                                            }

                                        }
                                        break;
                                    case "checkbox":
                                        {
                                            CheckBox checkboxCtr = (CheckBox)ctr;
                                            checkboxCtr.Checked = UGITUtility.StringToBoolean(input.Value);
                                        }
                                        break;
                                    case "textbox":
                                        {
                                            TextBox txtboxCtr = (TextBox)ctr;
                                            txtboxCtr.Text = input.Value;

                                        }
                                        break;
                                    case "number":
                                        {
                                            TextBox txtboxCtr = (TextBox)ctr;
                                            txtboxCtr.Text = input.Value;
                                        }
                                        break;
                                    case "datetime":
                                        {
                                            ASPxDateEdit datetimeCtr = (ASPxDateEdit)ctr;
                                            if (!string.IsNullOrEmpty(input.Value))
                                            {
                                                datetimeCtr.Date = Convert.ToDateTime(input.Value);
                                            }
                                            else
                                            {
                                                datetimeCtr.Date = DateTime.MinValue;

                                            }
                                        }
                                        break;
                                    case "userfield":
                                        {
                                            if (ctr is ASPxComboBox)
                                            {
                                                ASPxComboBox cmbUsers = (ASPxComboBox)ctr;
                                                cmbUsers.SelectedItem = cmbUsers.Items.FindByValue(Convert.ToString(input.Value));
                                            }
                                            else
                                            {
                                                UserValueBox userValueBox = (UserValueBox)ctr;
                                                userValueBox.SetValues(input.Value);


                                            }
                                        }
                                        break;
                                    case "lookup":
                                        {
                                            if (ctr is DepartmentDropdownList)
                                            {
                                                ((DepartmentDropdownList)ctr).SetValue(input.Value);
                                            }
                                            else if (ctr is LookupValueBoxEdit)
                                            {
                                                LookupValueBoxEdit lookUpValueBox = (LookupValueBoxEdit)ctr;
                                                lookUpValueBox.SetValues(input.Value);
                                                lookUpValueBox.dropBox.KeyValue = input.Value;
                                            }
                                            else
                                            {
                                                LookUpValueBox lookUpValueBox = (LookUpValueBox)ctr;
                                                lookUpValueBox.SetValues(input.Value);
                                            }
                                        }
                                        break;
                                    case "requesttype":
                                        {
                                            ASPxDropDownEdit cmbIssueType = null;
                                            LookupValueBoxEdit lookUpRequestType = null;
                                            List<string> values = new List<string>();
                                            if (ctr.Controls.Count > 0)
                                            {
                                                string baseID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                                                lookUpRequestType = ctr as LookupValueBoxEdit;
                                                cmbIssueType = ctr.Controls[0] as ASPxDropDownEdit;

                                            }

                                            if (lookUpRequestType != null)
                                            {
                                                string lookup = input.Value;
                                                if (!string.IsNullOrEmpty(lookup))
                                                {
                                                    lookUpRequestType.SetValues(lookup);

                                                }
                                            }

                                        }
                                        break;
                                    case "applicationaccessrequest":
                                        {
                                            Panel pApplications = (Panel)ctr;
                                            ServiceMatrix serviceMatrix = (ServiceMatrix)pApplications.Controls[0];
                                            List<ServiceMatrixData> lstServiceMatrixData = new List<ServiceMatrixData>();
                                            if (!string.IsNullOrEmpty(input.Value))
                                            {
                                                XmlDocument doc = new XmlDocument();
                                                doc.LoadXml(input.Value.Trim());
                                                lstServiceMatrixData = (List<ServiceMatrixData>)uHelper.DeSerializeAnObject(doc, lstServiceMatrixData);
                                            }
                                            string ID = string.Format("question_serviceMatrix_{0}_{1}", question.ServiceSectionID, question.ID);
                                            serviceMatrix.ID = ID;
                                            serviceMatrix.IsNoteEnabled = true;

                                            KeyValuePair<string, string> userquestion = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.ExistingUser);
                                            KeyValuePair<string, string> mirroraccessfrom = question.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.MirrorAccessFrom);
                                            if (userquestion.Key != null)
                                            {
                                                ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(userquestion.Value));
                                                if (inputQuestion != null)
                                                {
                                                    string tokenname = inputQuestion.TokenName;
                                                    int sectionid = Convert.ToInt32(inputQuestion.ServiceSectionID);
                                                    ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                                    if (sections != null)
                                                    {
                                                        ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                                        if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                                        {
                                                            UserProfile user = HttpContext.Current.GetUserManager().GetUserByUserName(UGITUtility.RemoveIDsFromLookupString(iqValue.Value));
                                                            if (user != null)
                                                            {
                                                                serviceMatrix.RoleAssignee = Convert.ToString(user.Id);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (mirroraccessfrom.Key != null)
                                            {
                                                ServiceQuestion inputQuestion = service.Questions.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(mirroraccessfrom.Value));
                                                if (inputQuestion != null)
                                                {
                                                    string tokenname = inputQuestion.TokenName;
                                                    int sectionid = Convert.ToInt32(inputQuestion.ServiceSectionID);
                                                    ServiceSectionInput sections = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == sectionid);
                                                    if (sections != null)
                                                    {
                                                        ServiceQuestionInput iqValue = sections.Questions.FirstOrDefault(x => x.Token == tokenname);
                                                        if (iqValue != null && !string.IsNullOrEmpty(iqValue.Value))
                                                        {
                                                            string uValue = iqValue.Value;
                                                            serviceMatrix.MirrorAccessFromUser = Convert.ToString(uValue);
                                                        }
                                                    }
                                                }
                                            }
                                            if (input.Value == string.Empty || (lstServiceMatrixData != null && lstServiceMatrixData.Count > 0 && lstServiceMatrixData[0].RoleAssignee != serviceMatrix.RoleAssignee))
                                            {
                                                serviceMatrix.Reload();
                                                List<ServiceMatrixData> serviceMatricData = serviceMatrix.GetSavedState();
                                                if (serviceMatricData != null && serviceMatricData.Count > 0)
                                                {
                                                    XmlDocument doc = uHelper.SerializeObject(serviceMatricData);
                                                    if (doc != null)
                                                        input.Value = doc.OuterXml;
                                                }
                                            }
                                            else
                                            {
                                                serviceMatrix.LoadOnState(lstServiceMatrixData);
                                            }
                                        }
                                        break;
                                    case "removeuseraccess":
                                        {

                                        }
                                        break;

                                    case "assets lookup":
                                        {
                                            ASPxGridLookup cbx = ctr as ASPxGridLookup;
                                            if (cbx != null)
                                            {
                                                List<string> keys = UGITUtility.ConvertStringToList(input.Value, Constants.Separator6);
                                                foreach (string itemAssest in keys)
                                                {
                                                    cbx.GridView.Selection.SetSelectionByKey(itemAssest, true);
                                                }
                                            }
                                        }
                                        break;
                                    case Constants.ServiceQuestionType.Rating:
                                        RatingCtr ratingCtr = (RatingCtr)ctr;
                                        ratingCtr.SelectedValue = UGITUtility.StringToInt(input.Value);
                                        break;
                                }
                                #endregion

                            }
                        }
                    }
                }

            }
        }

        #endregion

        #region Step 3

        #region Summary

        private void GenerateSummary()
        {
            RefreshServiceInputs();

            if (service != null)
            {
                rSummaryTable.DataSource = _serviceInput.ServiceSections.Where(x => x.IsSkiped == false && x.SectionID != -1);
                rSummaryTable.DataBind();
                summaryColWidth = _serviceInput.ServiceSections.Where(x => x.IsSkiped == false && x.SectionID != -1).Count() <= 1 ? 12 : 6;
            }
        }

        protected void RSummaryTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ServiceSectionInput sectionInput = (ServiceSectionInput)e.Item.DataItem;
            ServiceSection section = service.Sections.FirstOrDefault(x => x.ID == sectionInput.SectionID);
            if (sectionInput != null)
            {
                Label lbSection = (Label)e.Item.FindControl("lbSection");
                lbSection.Text = section.SectionName;
                Repeater rgroupQuesSummary = (Repeater)e.Item.FindControl("groupQuesRepeater");
                List<ServiceQuestionInput> serviceQuesInput = sectionInput.Questions.Where(x => x.IsSkiped == false).ToList();
                List<ServiceQuestion> lstOfSectionQues = service.Questions.Where(x => x.ServiceSectionID == section.ID).ToList();
                List<ServiceQuestion> validQuesLst = new List<ServiceQuestion>();
                serviceQuesInput.ForEach(x =>
                {
                    ServiceQuestion serviceques = lstOfSectionQues.FirstOrDefault(y => y.TokenName == x.Token);
                    if (serviceques != null)
                        validQuesLst.Add(serviceques);
                });

                List<QuestionList> groupQues = GetQuestionGrouping(validQuesLst);
                rgroupQuesSummary.DataSource = groupQues;
                rgroupQuesSummary.DataBind();
            }

            //#region updated code
            //ServiceSectionInput sectionInput = (ServiceSectionInput)e.Item.DataItem;
            //ServiceSection section = service.Sections.FirstOrDefault(x => x.ID == sectionInput.SectionID);
            //if (sectionInput != null)
            //{
            //    Label lbSection = (Label)e.Item.FindControl("lbSection");
            //    lbSection.Text = section.SectionTitle;

            //    Repeater rgroupQuesSummary = (Repeater)e.Item.FindControl("groupQuesRepeater");
            //    List<ServiceQuestionInput> serviceQuesInput = sectionInput.Questions.Where(x => x.IsSkiped == false).ToList();
            //    List<ServiceQuestion> lstOfSectionQues = service.Questions.Where(x => x.ServiceSectionID == section.ID).ToList();
            //    List<ServiceQuestion> validQuesLst = new List<ServiceQuestion>();
            //    serviceQuesInput.ForEach(x =>
            //    {
            //        ServiceQuestion serviceques = lstOfSectionQues.FirstOrDefault(y => y.TokenName == x.Token);
            //        if (serviceques != null)
            //            validQuesLst.Add(serviceques);
            //    });

            //    List<QuestionList> groupQues = GetQuestionGrouping(validQuesLst);
            //    //service.Questions.Where(x=> serviceQuesInput.Contains())
            //    rgroupQuesSummary.DataSource = groupQues;
            //    rgroupQuesSummary.DataBind();
            //    //Repeater rSummaryioQuest = (Repeater)e.Item.FindControl("rSummaryioQuest");
            //    //rSummaryioQuest.DataSource = sectionInput.Questions.Where(x => x.IsSkiped == false).ToList();
            //    //rSummaryioQuest.DataBind();
            //}
            //#endregion
        }

        protected void RSummaryioQuest_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ++summaryIteration;
            Repeater rSummaryioQuest = (Repeater)sender;
            RepeaterItem sectionItem = ((RepeaterItem)rSummaryioQuest.Parent.Parent.Parent.Parent);
            ServiceSectionInput sectionInput = (ServiceSectionInput)sectionItem.DataItem;
            ServiceQuestionInput questionInput = (ServiceQuestionInput)e.Item.DataItem;
            ServiceQuestion question = service.Questions.FirstOrDefault(x => x.ServiceSectionID == sectionInput.SectionID && x.TokenName == questionInput.Token);

            if (questionInput != null)
            {
                HtmlGenericControl summaryQuestionDiv = (HtmlGenericControl)e.Item.FindControl("summaryQuestionDiv");
                if (summaryIteration > 1)
                {
                    HtmlControl questionlbid = (HtmlControl)e.Item.FindControl("questionlbidSummary");
                    string clss = questionlbid.Attributes["class"];
                    questionlbid.Attributes["class"] = clss + " questionlb_hide";
                }
                List<ServiceQuestionInput> questionInputs = (List<ServiceQuestionInput>)rSummaryioQuest.DataSource;
                if (e.Item.ItemIndex == questionInputs.Count - 1)
                {
                    summaryQuestionDiv.Style.Add(HtmlTextWriterStyle.BorderWidth, "0px");
                }

                Label lbQuestion = (Label)e.Item.FindControl("lbQuestion");
                Label lbQuestionVal = (Label)e.Item.FindControl("lbQuestionVal");
                lbQuestion.Text = question.QuestionTitle;
                if (question.FieldMandatory)
                {
                    lbQuestion.Text = string.Format("{0}<b style='color:red;'>*</b>", question.QuestionTitle);
                }

                lbQuestionVal.Text = questionInput.Value;
                if (question.QuestionType.ToLower() == "userfield")
                {
                    //lbQuestionVal.Text = string.Join("; ", HttpContext.Current.GetUserManager().CommaSeparatedNamesFrom(questionInput.Value));
                    lbQuestionVal.Text = HttpContext.Current.GetUserManager().CommaSeparatedNamesFrom(UGITUtility.ConvertStringToList(questionInput.Value, Constants.Separator6), Constants.Separator6);
                }
                else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.MULTICHOICE && !string.IsNullOrEmpty(questionInput.Value))
                {
                    lbQuestionVal.Text = Regex.Replace(questionInput.Value, string.Format("[{0}{1}]", Constants.Separator2, Constants.Separator5), Constants.BreakLineSeparator);
                }
                else if (question.QuestionType.ToLower() == "datetime" && !string.IsNullOrEmpty(questionInput.Value))
                {
                    DateTime date = Convert.ToDateTime(questionInput.Value);

                    string dateMode = string.Empty;
                    if (question.QuestionTypePropertiesDicObj != null && question.QuestionTypePropertiesDicObj.Count > 0)
                    {
                        question.QuestionTypePropertiesDicObj.TryGetValue("datemode", out dateMode);
                        if (dateMode.ToLower() == "dateonly")
                        {
                            lbQuestionVal.Text = date.ToString("MM/dd/yyyy");
                        }
                        else if (dateMode.ToLower() == "timeonly")
                        {
                            lbQuestionVal.Text = date.ToString("h:mm tt");
                        }
                    }
                }
                else if ((question.QuestionType.ToLower() == "lookup" || question.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets.ToLower()) && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                    List<string> lookupcoll = UGITUtility.ConvertStringToList(questionInput.Value, Constants.Separator6);
                    if (lookupcoll != null && lookupcoll.Count > 0)
                    {
                        FieldConfigurationManager fcManager = new FieldConfigurationManager(Context.GetManagerContext());

                        string listName = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("lookuplist", out listName);
                        if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets.ToLower())
                        {
                            Ticket tk = new Ticket(Context.GetManagerContext(), "CMDB");
                            lbQuestionVal.Text = tk.GetValue(questionInput.Value);
                        }
                        else if (listName == DatabaseObjects.Tables.Department)
                        {
                            lbQuestionVal.Text = uHelper.FormatDepartment(HttpContext.Current.GetManagerContext(), questionInput.Value, false,Constants.BreakLineSeparator);
                        }
                        else
                        {
                            string value = string.Empty;
                            List<string> items = new List<string>();
                            if (!string.IsNullOrWhiteSpace(listName))
                            {
                                FieldConfigurationManager fcm = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                                value = fcm.GetFieldConfigurationData(listName, questionInput.Value);
                            }
                            if (!string.IsNullOrEmpty(value))
                                value= Regex.Replace(value, string.Format("[{0}{1}]", Constants.Separator2, Constants.Separator6), Constants.BreakLineSeparator);
                            lbQuestionVal.Text = value;
                        }
                    }
                }
                else if (question.QuestionType.ToLower() == "requesttype" && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                    string lookup = string.Empty;
                    List<string> inputValues = UGITUtility.ConvertStringToList(questionInput.Value, Constants.Separator);
                    lookup = inputValues[0];

                    string moduleName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("module", out moduleName);
                    if (lookup != null && Convert.ToInt32(lookup) > 0)
                    {
                        lbQuestionVal.Text = lookup;
                        UGITModule module = ModuleViewManager.LoadByName(moduleName);
                        ModuleRequestType requestType = module.List_RequestTypes.FirstOrDefault(x => x.ID == UGITUtility.StringToInt(lookup));
                        if (requestType != null)
                        {
                            List<string> rName = new List<string>();
                            if (!string.IsNullOrWhiteSpace(requestType.Category))
                                rName.Add(requestType.Category);
                            if (!string.IsNullOrWhiteSpace(requestType.SubCategory))
                                rName.Add(requestType.SubCategory);
                            rName.Add(requestType.RequestType);
                            if (questionInput.SubTokensValue != null && questionInput.SubTokensValue.Count > 0)
                            {
                                ServiceQuestionInput issueType = questionInput.SubTokensValue.FirstOrDefault(x => x.Token == "issuetype");
                                if (issueType != null)
                                {
                                    string[] issueTypes = UGITUtility.SplitString(issueType.Value, Constants.Separator5);
                                    // rName.Add(string.Join("; ", issueTypes));
                                    rName.AddRange(issueTypes.ToList());
                                }
                            }
                            lbQuestionVal.Text = string.Join(" > ", rName.ToArray());
                        }
                    }
                }
                else if (question.QuestionType.ToLower() == "applicationaccessrequest" && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                    FormatApplicationRoleModules(questionInput.Value, lbQuestionVal);
                }
                else if (question.QuestionType.ToLower() == "checkbox" && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                    lbQuestionVal.Text = "No";
                    if (UGITUtility.StringToBoolean(questionInput.Value))
                        lbQuestionVal.Text = "Yes";
                }
            }
        }

        #endregion

        public void FormatApplicationRoleModules(string value, Label lblControl)
        {
            FormatApplicationRoleModules(value, lblControl, false);
        }

        public void FormatApplicationRoleModules(string value, Label lblControl, bool isMobile)
        {
            Page page = new Page();
            Panel pnlapplications = new Panel();
            List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
            ServiceMatrix serviceMatrix = (ServiceMatrix)page.LoadControl("~/controltemplates/uGovernIT/services/ServiceMatrix.ascx");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(value.Trim());
            serviceMatrixDataList = (List<ServiceMatrixData>)uHelper.DeSerializeAnObject(doc, serviceMatrixDataList);
            List<ServiceMatrixData> newServiceMatrixDataList = new List<ServiceMatrixData>();
            if (serviceMatrixDataList != null && serviceMatrixDataList.Count > 0)
            {
                ServiceRequestBL serviceRequestBL = new ServiceRequestBL(HttpContext.Current.GetManagerContext());
                foreach (ServiceMatrixData serviceMatrixData in serviceMatrixDataList)
                {
                    if (string.IsNullOrEmpty(serviceMatrixData.RoleAssignee))
                        serviceMatrixData.RoleAssignee = serviceMatrixDataList[0].RoleAssignee;
                    if (serviceRequestBL.CheckIsRelationChanged(serviceMatrixData))
                    {
                        newServiceMatrixDataList.Add(serviceMatrixData);
                    }
                }
            }

            serviceMatrix.IsReadOnly = true;
            serviceMatrix.IsNoteEnabled = true;
            serviceMatrix.IsMobile = isMobile;
            serviceMatrix.ParentControl = "Service";
            pnlapplications.Controls.Add(serviceMatrix);
            lblControl.Controls.Add(pnlapplications);


            serviceMatrix.LoadOnState(newServiceMatrixDataList);
        }

        protected void BtCreateRequest_Click(object sender, EventArgs e)
        {
            //if (service.ServiceCategoryType.EqualsIgnoreCase("Tenant management"))
            if (tenantOnBoardingHelper.GetNewTenantTemplateServiceTitles().Contains(service.Title))
            {
                //Skip Captcha it SuperAdmin creates Tenant
                if (UserProfileManager.IsUGITSuperAdmin(User))
                    Captcha.IsValid = true;

                if (!Captcha.IsValid)
                {
                    return;
                }
            }
            if (!Page.IsValid)
                return;

            if (service == null)
                return;

            long hVallue = service.Sections.LastOrDefault().ID;
            if (service.HideSummary && hVallue == currentStepSectionID)
                BtNext_Click(null, null);

            lvStepSections.Visible = false;
            pStep3Section1Container.CssClass = "col_b_complete";

            isNextPrevious = true;

            List<ServiceSectionCondition> skipSectionConditions = service.SkipSectionCondition;
            ValidateSectionConditions(ref skipSectionConditions);

            RefreshServiceInputs();

            pAttachmentContainer.Visible = false;
            ServiceInput serviceInput = _serviceInput;

            //Convert input object into service request object which is used to create service
            ServiceRequestDTO serviceRequestObj = new ServiceRequestDTO();
            serviceRequestObj.ServiceId = serviceInput.ServiceID;
            QuestionsDTO requestQuestion = null;
            foreach (ServiceSectionInput sectionInput in serviceInput.ServiceSections)
            {
                if (!sectionInput.IsSkiped)
                {
                    foreach (ServiceQuestionInput questionInput in sectionInput.Questions)
                    {
                        if (!questionInput.IsSkiped)
                        {
                            requestQuestion = new QuestionsDTO();
                            requestQuestion.Token = questionInput.Token;
                            requestQuestion.Value = questionInput.Value;
                            serviceRequestObj.Questions.Add(requestQuestion);

                            ServiceQuestion question = service.Questions.FirstOrDefault(x => x.TokenName == questionInput.Token);
                            if (question.QuestionType.ToLower() == "requesttype" && questionInput.SubTokensValue != null)
                            {
                                ServiceQuestionInput issutTypeInput = questionInput.SubTokensValue.FirstOrDefault(x => x.Token.ToLower() == "issuetype");
                                if (issutTypeInput != null)
                                {
                                    QuestionsDTO requestSubQuestion = new QuestionsDTO();
                                    requestSubQuestion.Token = "issuetype";
                                    requestSubQuestion.Value = issutTypeInput.Value;

                                    if (requestQuestion.SubTokensValue == null)
                                        requestQuestion.SubTokensValue = new List<QuestionsDTO>();

                                    requestQuestion.SubTokensValue.Add(requestSubQuestion);
                                }
                            }
                        }
                    }
                }
            }
            ServiceRequestBL requestBL = null;
            // requestPage
            if (!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest())

            {
                _applicationContext = ApplicationContext.Create();
                requestBL = new ServiceRequestBL(_applicationContext, "SelfRegistration");
            }
            else
            {
                requestBL = new ServiceRequestBL(HttpContext.Current.GetManagerContext());
            }
            requestBL.ServiceInput = serviceInput;
            if (!string.IsNullOrEmpty(fileuploadServiceAttach.GetValue()))
            {
                requestQuestion = new QuestionsDTO();
                requestQuestion.Token = "attachments";
                requestQuestion.Value = fileuploadServiceAttach.GetValue();
                serviceRequestObj.Questions.Add(requestQuestion);
            }
            ServiceResponseTreeNodeParent result = null;

            if (service != null && service.ServiceType.ToLower() == Constants.ModuleAgent.ToLower().Replace("~", "") && !string.IsNullOrEmpty(moduleName))
            {
                result = SubmitAgent(string.Empty);
            }
            else if (service != null && service.ServiceType.ToLower() == Constants.ModuleFeedback.ToLower().Replace("~", ""))
            {
                result = SubmitSurvey(serviceRequestObj, service);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request["requestPage"]) && (PermissionHelper.IsOnboardingUIRequest()))
                {
                    _applicationContext = ApplicationContext.Create();
                    _serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext(), "SelfRegistration");
                    ConfigurationVariableManager ObjConfigVariable = new ConfigurationVariableManager(_applicationContext);

                    bool.TryParse(ObjConfigVariable.GetValue(ConfigConstants.IsEmailVerificationActivated), out bool isEmailVerificationActivated);

                    if (!isEmailVerificationActivated)
                    {
                        string serviceTicket = ObjConfigVariable.GetValue(ConfigConstants.NewTenantServiceTicket);

                        if (serviceTicket == "True" || PermissionHelper.IsOnboardingUIRequestType() == false)
                        {
                            result = requestBL.CreateService(serviceRequestObj, "SelfRegistration");
                        }
                        if (PermissionHelper.IsOnboardingUIRequestType())
                        {
                            UserProfileManager _userManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                            var defaultConfigManager = new DefaultConfigManager();
                            string companynamenew = serviceRequestObj.Questions[0].Value;
                            serviceRequestObj.Questions[0].Value = Regex.Replace(serviceRequestObj.Questions[0].Value, "[^0-9A-Za-z]", "");
                            serviceRequestObj.Questions[0].Value = serviceRequestObj.Questions[0].Value.Replace(" ", "").Trim();

                            var resultTenantCreation = new UpdateResult();
                            var accountInfo = defaultConfigManager.GetAdminAccountInfo(serviceRequestObj.Questions[0].Value, serviceRequestObj.Questions[1].Value, _userManager.GeneratePassword());
                            //bool? SelfRegisteredTenant;
                            //if (PermissionHelper.IsOnboardingUIRequest())
                            //{
                            //    SelfRegisteredTenant = false;
                            //}
                            //else
                            //{
                            //    SelfRegisteredTenant = true;
                            //}

                            Tenant model = new Tenant
                            {
                                AccountID = serviceRequestObj.Questions[0].Value,
                                TenantName = companynamenew,
                                TenantUrl = serviceRequestObj.Questions[4].Value,
                                Email = serviceRequestObj.Questions[1].Value,
                                SelfRegisteredTenant = true,
                                Name = serviceRequestObj.Questions[2].Value,
                                Title = serviceRequestObj.Questions[3].Value,
                                Contact = serviceRequestObj.Questions[5].Value

                            };

                            try
                            {
                                Thread createTenantThread = new Thread(delegate ()
                                {
                                    resultTenantCreation = defaultConfigManager.CreateTenantInfo(model, "all", accountInfo);

                                    if (resultTenantCreation.success)
                                    {
                                        //Util.Cache.CacheHelper<object>.Clear();
                                        string forwardMailAddress = ObjConfigVariable.GetValue(ConfigConstants.ForwardMailAddress);

                                        if (!string.IsNullOrEmpty(forwardMailAddress))
                                        {
                                            var lstforwardMailAddress = forwardMailAddress.Split(',');
                                            foreach (string strMail in lstforwardMailAddress)
                                            {
                                                var response = new EmailHelper(_applicationContext).SendEmailToTenantAdminAccount(strMail, accountInfo.AccountID, accountInfo.UserName, accountInfo.Password, accountInfo.Email);
                                            }
                                        }
                                        else
                                        {
                                            var response = new EmailHelper(_applicationContext).SendEmailToTenantAdminAccount(accountInfo.Email, accountInfo.AccountID, accountInfo.UserName, accountInfo.Password, accountInfo.Email);
                                        }

                                    }
                                });

                                createTenantThread.Start();

                            }
                            catch (Exception exception)
                            {
                                ULog.WriteException(exception);
                            }

                        }

                    }


                }
                else
                {
                    result = requestBL.CreateService(serviceRequestObj);
                }
            }
            currentStepID = 3;
            currentStepSectionID = 1;
            if (result != null)
            {
                if (result.TicketId != null)
                {
                    if (!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest())

                    {
                        _applicationContext = ApplicationContext.Create();
                        _serviceManager = new ServicesManager(_applicationContext, "SelfRegistration");
                    }
                    else
                    {
                        _serviceManager = new ServicesManager(ApplicationContext);
                    }
                    TenantOnBoardingHelper tenantOnBoardingHelper = new TenantOnBoardingHelper(_applicationContext);
                    var listNewTenantServiceTitle = tenantOnBoardingHelper.GetNewTenantTemplateServiceTitles();
                    var services = ServicesManager.LoadAllServices("services").OrderBy(x => x.CategoryId).OrderBy(x => x.ItemOrder).ToList();
                    var newTenantServiceId = ServicesManager.LoadAllServices("services").Where(x => listNewTenantServiceTitle.Any(y => y.EqualsIgnoreCase(x.Title))).Select(x => x.ID).FirstOrDefault();
                    //  var newTenantServiceId = ServicesManager.LoadAllServices("services").Where(x => x.Title.Equals(ConfigurationManager.AppSettings["NewTenantTemplate:ServiceTitle"].ToString())).Select(x => x.ID).FirstOrDefault();
                    if (service.ID == newTenantServiceId)
                    {
                        if (PermissionHelper.IsOnboardingUIRequestType() == false)
                        {
                            ConfigurationVariableManager ObjConfigVariable = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
                            string forwardMailAddress = ObjConfigVariable.GetValue(ConfigConstants.ForwardMailAddress);

                            if (!string.IsNullOrEmpty(forwardMailAddress))
                            {
                                var lstforwardMailAddress = forwardMailAddress.Split(',');
                                foreach (string strMail in lstforwardMailAddress)
                                {
                                    var requesterEmail = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("email")).SingleOrDefault().Value;
                                    string companyname = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("companyname")).SingleOrDefault().Value;
                                    string name = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("companyname")).SingleOrDefault().Value;
                                    string title = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("title")).SingleOrDefault().Value;
                                    string email = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("email")).SingleOrDefault().Value;
                                    SendEmailToNewApplicaitonRequester(strMail, result.TicketId, companyname, name, title, email, true);
                                }

                            }
                            else
                            {
                                var requesterEmail = serviceInput.ServiceSections[0].Questions.Where(x => x.Token.Equals("email")).SingleOrDefault().Value;
                                SendEmailToNewApplicaitonRequester(requesterEmail, result.TicketId);
                            }



                        }


                    }

                }
            }


            if (service != null && service.HideThankYouScreen)
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);

            }
            else
            {
                GenerateTaskSummary(result);
            }

            //set cookie to refresh parent page when he saved detail. which force parent to refresh.
            var refreshParentID = UGITUtility.GetCookieValue(Request, "framePopupID");
            if (!string.IsNullOrWhiteSpace(refreshParentID))
            {
                UGITUtility.CreateCookie(Response, "refreshParent", refreshParentID);
            }
            if ((!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest()) && Session["IsRegistrationSubmit"]?.ToString() == "true")
            {
                Session["IsRegistrationSubmit"] = false;
                Context.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);
                //uGovernIT.Util.Cache.CacheHelper<object>.Clear();
                //Response.Redirect("RegistrationSuccessm.aspx");
            }
            if (PermissionHelper.IsOnboardingUIRequest())
            {
                TenantOnBoardingHelper tenantOnBoardingHelper = new TenantOnBoardingHelper(_applicationContext);
                bool isSuperAdmin = UserProfileManager.IsUGITSuperAdmin(User);

                long Id = tenantOnBoardingHelper.SaveToRegistrationTenant(true, serviceInput, !isSuperAdmin);
                if (!isSuperAdmin)
                {
                    string routeUrl = "/ControlTemplates/RegistrationSuccessm.aspx";
                    uHelper.ClosePopUpAndRedirect(Context, routeUrl);
                }
                else
                {
                    uHelper.ClosePopUpAndRedirect(Context, $"{ConfigurationManager.AppSettings["SiteUrl"]}ApplicationRegistrationRequest/TenantCreation?id={Id}");
                }
            }
        }




        private void GenerateTaskSummary(ServiceResponseTreeNodeParent resultNode)
        {
            btCancelL.Text = "Close";
            divpAttachmentContainer.Visible = false;
            pAttachmentContainer.Visible = false;
            if (resultNode != null)
            {
                TreeNode serviceTicketNode = null;

                if (service.CreateParentServiceRequest)
                {
                    // Have a parent service ticket, use that as the top-level node
                    string title = string.Format("{0}: {1}", resultNode.TicketId, resultNode.Text);
                    serviceTicketNode = new TreeNode(title, resultNode.TicketId);
                    if (!service.AllowServiceTasksInBackground)
                        serviceTicketNode.NavigateUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}', '90', '90',0);", Ticket.GenerateTicketURL(HttpContext.Current.GetManagerContext(), resultNode.MName, resultNode.TicketId), title);
                    else
                        serviceTicketNode.SelectAction = TreeNodeSelectAction.None;
                    summaryTree.Nodes.Add(serviceTicketNode);
                }
                else if (resultNode.ServiceResponseTreeNode.Count > 1)
                {
                    // No parent service ticket AND more than one child non-SVC ticket, create dummy "Requests" top-level node
                    serviceTicketNode = new TreeNode("Requests", "Requests");
                    summaryTree.Nodes.Add(serviceTicketNode);
                }
                else if ((service != null && service.ServiceType.ToLower() == Constants.ModuleAgent.ToLower().Replace("~", "")))
                {
                    serviceTicketNode = new TreeNode(resultNode.TicketId, resultNode.TicketId);
                    serviceTicketNode.SelectAction = TreeNodeSelectAction.None;
                    summaryTree.Nodes.Add(serviceTicketNode);
                }
                else if (service != null && service.ServiceType.ToLower() == Constants.ModuleFeedback.ToLower().Replace("~", ""))
                {
                    serviceTicketNode = new TreeNode(resultNode.Text, resultNode.TicketId);
                    serviceTicketNode.SelectAction = TreeNodeSelectAction.None;
                    summaryTree.Nodes.Add(serviceTicketNode);
                }
                foreach (ServiceResponseTreeNode task in resultNode.ServiceResponseTreeNode)
                {
                    TreeNode taskNode = new TreeNode();
                    if (task.Type == 0)
                    {
                        taskNode.Text = task.Text;
                        taskNode.Value = task.Text;
                    }
                    else
                    {
                        string title = string.Format("{0}: {1}", task.TicketID, task.Text);
                        taskNode.Text = title;
                        if (!string.IsNullOrEmpty(task.MName))
                            taskNode.NavigateUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}', '90', '90',0);", Ticket.GenerateTicketURL(HttpContext.Current.GetManagerContext(), task.MName, task.TicketID), title);
                    }

                    if (serviceTicketNode != null)
                    {
                        // Either we have parent service ticket OR dummy "Requests" node at top with multiple child non-SVC tickets
                        serviceTicketNode.ChildNodes.Add(taskNode);
                    }
                    else
                    {
                        // No parent ticket, and exactly ONE non-SVC ticket, just add it at the top-level
                        summaryTree.Nodes.Add(taskNode);
                    }
                }
            }
            else
            {

                if (!string.IsNullOrEmpty(Request["requestPage"]) && PermissionHelper.IsOnboardingUIRequest())
                {
                    TreeNode taskNode = new TreeNode();
                    taskNode.Text = "Your registration has been accepted. Trial login information will be shared with you via mail in a short period of time.";
                    //taskNode.Text = "New Tenant Created succesfully!";
                    taskNode.Value = "New Tenant Creation";
                    taskNode.SelectAction = TreeNodeSelectAction.None;
                    summaryTree.Nodes.Add(taskNode);
                }
                else
                {
                    TreeNode taskNode = new TreeNode();
                    taskNode.Text = "No request(s) created, please check service configuration!";
                    taskNode.Value = "No request(s) Created";
                    taskNode.SelectAction = TreeNodeSelectAction.None;
                    summaryTree.Nodes.Add(taskNode);
                }


            }
        }

        private void GenerateTaskSummary(TaskTempInfo ticket)
        {
            TreeNode serviceTicketNode = new TreeNode(ticket.TicketID, ticket.TicketID);
            UGITModule moduleRow = ModuleViewManager.LoadByID(ticket.ModuleID);
            if (moduleRow != null)
            {
                serviceTicketNode.NavigateUrl = string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}', '90', '90',0);", Ticket.GenerateTicketURL(HttpContext.Current.GetManagerContext(), moduleRow.ModuleName, ticket.TicketID), ticket.TicketID);
            }
            summaryTree.Nodes.Add(serviceTicketNode);
        }

        public static string ParseQuestionVal(string questionType, string value, ServiceQuestion question = null)
        {
            if (questionType.ToLower() == "userfield")
            {
                UserProfileManager UManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                value = UManager.CommaSeparatedNamesFrom(value);

            }
            else if (questionType.ToLower() == Constants.ServiceQuestionType.LOOKUP ||
                     questionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE || questionType.ToLower() == Constants.ServiceQuestionType.Assets)
            {
                string fieldName = string.Empty;
                string lookup = string.Empty;
                if (question != null)
                {
                    try
                    {
                        FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                        if (question.QuestionTypePropertiesDicObj.Keys.Contains("lookuplist"))
                            fieldName = question.QuestionTypePropertiesDicObj["lookuplist"];
                        try
                        {
                            lookup = fieldConfigurationManager.GetFieldConfigurationData(fieldName, value);
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex);
                        }
                        if (!string.IsNullOrEmpty(lookup))
                        {
                            value = lookup;
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log.ULog.WriteException(ex);
                    }
                }
            }
            else if (questionType.ToLower() == Constants.ServiceQuestionType.MULTICHOICE)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    value= Regex.Replace(value, string.Format("[{0}{1}]", Constants.Separator2, Constants.Separator5), Constants.BreakLineSeparator);
               
            }
            else if (question != null && question.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME)
            {
                string dateMode;
                DateTime DateVal = UGITUtility.StringToDateTime(value);
                if (DateVal != DateTime.MinValue && question.QuestionTypePropertiesDicObj.TryGetValue("datemode", out dateMode))
                {
                    if (!string.IsNullOrEmpty(dateMode) && dateMode.ToLower() == "dateonly")
                    {
                        value = UGITUtility.GetDateStringInFormat(DateVal, false);
                    }
                    else if (!string.IsNullOrEmpty(dateMode) && dateMode.ToLower() == "timeonly")
                    {
                        value = DateVal.ToString("HH:mm");
                    }
                    else
                    {
                        value = UGITUtility.GetDateStringInFormat(DateVal, true);
                    }
                }
            }
            return value;
        }

        class TaskTempInfo
        {
            public int TaskID { get; set; }
            public int ModuleID { get; set; }
            public string TicketID { get; set; }
            public string Title { get; set; }
            public string Desc { get; set; }
            public string AssignTo { get; set; }
            public DateTime DueDate { get; set; }
            public double EstimatedHours { get; set; }
        }
        #endregion

        #region global helper function

        private int GetServiceID()
        {
            int serviceID = 0;
            int.TryParse(Request["ServiceID"], out serviceID);
            if (serviceID <= 0)
            {
                int.TryParse(Request[serviceTypeDD.UniqueID], out serviceID);
            }
            if (serviceID <= 0)
            {
                int.TryParse(Request[hfServiceID.UniqueID], out serviceID);
            }

            return serviceID;
        }

        private string GetTicketId()
        {
            if (Request["TicketId"] != null && !string.IsNullOrEmpty(Convert.ToString(Request["TicketId"])))
                return Convert.ToString(Request["TicketId"]);
            return string.Empty;
        }

        private string GetCategoryType()
        {
            if (Request["categorytype"] != null && !string.IsNullOrEmpty(Convert.ToString(Request["categorytype"])))
                return Convert.ToString(Request["categorytype"]);
            return string.Empty;
        }

        private string GetModuleName()
        {
            if (Request["ModuleName"] != null && !string.IsNullOrEmpty(Convert.ToString(Request["ModuleName"])))
                return Convert.ToString(Request["ModuleName"]);
            return string.Empty;
        }

        /// <summary>
        /// Get Step sections. for step 2 section comes from service object
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        private Dictionary<long, string> GetStepSections(int step)
        {
            Dictionary<long, string> sections = new Dictionary<long, string>();
            HtmlGenericControl activeIconDiv = new HtmlGenericControl();
            //List<int> itemOrderList = new List<int>();
            if (step == 1)
            {

                //  sections.Add(1, "Pick a service from the catalog");
                // sections.Add(2, "Select the sub services");
            }
            else if (step == 2)
            {
                if (service != null)
                {
                    foreach (ServiceSection section in service.Sections)
                    {
                        sections.Add(section.ID, section.SectionName);
                    }

                    if (service != null && !service.HideSummary)
                    {
                        sections.Add(SUMMARYSUBSECTION, "Summary");
                    }

                }
            }
            else if (step == 3)
            {
                sections.Add(1, "Complete");
            }





            return sections;
        }

        private void BindStepSections(int step)
        {
            lvStepSections.DataSource = GetStepSections(step);
            lvStepSections.DataBind();

            lvStepSectionsNew.DataSource = GetStepSections(step);
            lvStepSectionsNew.DataBind();

            lvStepSectionsNewcopy.DataSource = GetStepSections(step);
            lvStepSectionsNewcopy.DataBind();
        }

        private void SelectStepAndSection(int stepID, long sectionID)
        {
            step1.CssClass = string.Empty;
            step2.CssClass = string.Empty;
            step3.CssClass = string.Empty;
            if (stepID == 2)
            {
                step2.CssClass = "active";
            }
            else if (stepID == 3)
            {
                step3.CssClass = "active";
            }
            else
            {
                step1.CssClass = "active";
            }

            foreach (ListViewDataItem item in lvStepSections.Items)
            {
                Label lb = (Label)item.FindControl("sectionSideBarContainer");
                HtmlGenericControl activeIconDiv = (HtmlGenericControl)item.FindControl("activeIconDiv");
                HtmlImage imgSection = (HtmlImage)item.FindControl("imgSection");
                //activeIconDiv.CssClass = string.Empty;
                var stepsection = Convert.ToString(lvStepSections.DataKeys[item.DataItemIndex].Values["Value"]);
                string isActive = string.Empty;
                //SectionActiveInActive(activeIconDiv,stepsection, isActive=string.Empty);
                long Key = Convert.ToInt64(lvStepSections.DataKeys[item.DataItemIndex].Values["Key"]);
                string SectionIconUrl = string.Empty;

                if (service.Sections.Where(x => x.ID == Key).FirstOrDefault() != null)
                    SectionIconUrl = service.Sections.Where(x => x.ID == Key).FirstOrDefault().IconUrl;
                else
                    SectionIconUrl = string.Empty;
            }

            foreach (ListViewDataItem item in lvStepSectionsNew.Items)
            {
                HtmlGenericControl activeIconDiv = (HtmlGenericControl)item.FindControl("divnewsection");
                ASPxImage img2 = (ASPxImage)item.FindControl("img2");
                ASPxLabel lb = (ASPxLabel)item.FindControl("newsectionSideBarContainer");
                var stepsection = Convert.ToString(lvStepSectionsNew.DataKeys[item.DataItemIndex].Values["Value"]);
                string isActive = string.Empty;
                //SectionActiveInActive(activeIconDiv,stepsection, isActive=string.Empty);
                long Key = Convert.ToInt64(lvStepSectionsNew.DataKeys[item.DataItemIndex].Values["Key"]);
                string SectionIconUrl = string.Empty;

                if (service.Sections.Where(x => x.ID == Key).FirstOrDefault() != null)
                    SectionIconUrl = service.Sections.Where(x => x.ID == Key).FirstOrDefault().IconUrl;

                if (stepsection.Contains("Employee Info"))
                {
                    activeIconDiv.Attributes.Add("class", "step_content ");
                    img2.ImageUrl = "/Content/Images/employee-info-active.png";
                }
                else if (stepsection.Contains("Equipment Details"))
                {
                    activeIconDiv.Attributes.Add("class", "step_content ");
                    img2.ImageUrl = "/Content/Images/Equipment-Details-active.png";
                }
                else if (stepsection.Contains("Application Access Details"))
                {
                    activeIconDiv.Attributes.Add("class", "step_content ");
                    img2.ImageUrl = "/Content/Images/Application-Access-Details-active.png";
                }
                else if (stepsection.Contains("Summary"))
                {
                    activeIconDiv.Attributes.Add("class", "step_content");
                    img2.ImageUrl = "/Content/Images/Summary-active.png";
                }
                else if (stepsection.Contains("Employee Details"))
                {
                    SetIcons(activeIconDiv, img2, SectionIconUrl, "");
                }
                else
                {
                    SetIcons(activeIconDiv, img2, SectionIconUrl, "");
                }

                if (Convert.ToString(lvStepSectionsNew.DataKeys[lvStepSectionsNew.Items.IndexOf(item)].Value) == sectionID.ToString())
                {
                    if (stepsection == "Employee Info")
                    {
                        activeIconDiv.Attributes.Add("class", "step_content  active");
                        img2.ImageUrl = "/Content/Images/employee-info-active.png";
                    }
                    else if (stepsection == "Equipment Details")
                    {
                        activeIconDiv.Attributes.Add("class", "step_content  active");
                        img2.ImageUrl = "/Content/Images/Equipment-Details-active.png";
                    }
                    else if (stepsection == "Application Access Details")
                    {
                        activeIconDiv.Attributes.Add("class", "step_content  active");
                        img2.ImageUrl = "/Content/Images/Application-Access-Details-active.png";
                    }
                    else if (stepsection == "Summary")
                    {
                        activeIconDiv.Attributes.Add("class", "step_content  active");
                        img2.ImageUrl = "/Content/Images/Summary-active.png";
                    }
                    else if (stepsection.Contains("Employee Details"))
                    {
                        SetIcons(activeIconDiv, img2, SectionIconUrl, "");
                    }
                    else
                    {
                        SetIcons(activeIconDiv, img2, SectionIconUrl, "", true);
                    }

                }
            }

            foreach (ListViewDataItem item in lvStepSectionsNewcopy.Items)
            {
                HtmlGenericControl activeIconDiv = (HtmlGenericControl)item.FindControl("divnewsectionhide");
                ASPxImage img2 = (ASPxImage)item.FindControl("img2hide");
                var stepsection = Convert.ToString(lvStepSectionsNewcopy.DataKeys[item.DataItemIndex].Values["Value"]);
                string isActive = string.Empty;
                //SectionActiveInActive(activeIconDiv,stepsection, isActive=string.Empty);
                long Key = Convert.ToInt64(lvStepSectionsNewcopy.DataKeys[item.DataItemIndex].Values["Key"]);
                string SectionIconUrl = string.Empty;

                if (service.Sections.Where(x => x.ID == Key).FirstOrDefault() != null)
                    SectionIconUrl = service.Sections.Where(x => x.ID == Key).FirstOrDefault().IconUrl;

                if (stepsection.Contains("Employee Info"))
                {
                    activeIconDiv.Attributes.Add("class", "step_content ");
                    img2.ImageUrl = "/Content/Images/employee-info-active.png";
                }
                else if (stepsection.Contains("Equipment Details"))
                {
                    activeIconDiv.Attributes.Add("class", "step_content ");
                    img2.ImageUrl = "/Content/Images/Equipment-Details-active.png";
                }
                else if (stepsection.Contains("Application Access Details"))
                {
                    activeIconDiv.Attributes.Add("class", "step_content ");
                    img2.ImageUrl = "/Content/Images/Application-Access-Details-active.png";
                }
                else if (stepsection.Contains("Summary"))
                {
                    activeIconDiv.Attributes.Add("class", "step_content");
                    img2.ImageUrl = "/Content/Images/Summary-active.png";
                }
                else if (stepsection.Contains("Employee Details"))
                {
                    SetIcons(activeIconDiv, img2, SectionIconUrl, "");
                }
                else
                {
                    SetIcons(activeIconDiv, img2, SectionIconUrl, "");
                }

                if (Convert.ToString(lvStepSectionsNewcopy.DataKeys[lvStepSectionsNewcopy.Items.IndexOf(item)].Value) == sectionID.ToString())
                {
                    if (stepsection == "Employee Info")
                    {
                        activeIconDiv.Attributes.Add("class", "step_content  active");
                        img2.ImageUrl = "/Content/Images/employee-info-active.png";
                    }
                    else if (stepsection == "Equipment Details")
                    {
                        activeIconDiv.Attributes.Add("class", "step_content  active");
                        img2.ImageUrl = "/Content/Images/Equipment-Details-active.png";
                    }
                    else if (stepsection == "Application Access Details")
                    {
                        activeIconDiv.Attributes.Add("class", "step_content  active");
                        img2.ImageUrl = "/Content/Images/Application-Access-Details-active.png";
                    }
                    else if (stepsection == "Summary")
                    {
                        activeIconDiv.Attributes.Add("class", "step_content  active");
                        img2.ImageUrl = "/Content/Images/Summary-active.png";
                    }
                    else if (stepsection.Contains("Employee Details"))
                    {
                        SetIcons(activeIconDiv, img2, SectionIconUrl, "");
                    }
                    else
                    {
                        SetIcons(activeIconDiv, img2, SectionIconUrl, "", true);
                    }

                }
            }

            pStep1Section1Container.Visible = false;
            pStep2SectionContainer.Visible = false;
            pStep2SectionSContainer.Visible = false;
            pStep3Section1Container.Visible = false;

            if (stepID == 1 || stepID == 3)
            {
                Panel pnl = (Panel)this.FindControl(string.Format("pStep{0}Section{1}Container", stepID, sectionID));
                if (pnl != null)
                    pnl.Visible = true;

                if (stepID == 3)
                {
                    imgHelp.Visible = false;
                    if (service != null && service.ServiceType == Constants.ModuleAgent.ToLower().Replace("~", ""))
                        sectionDescription.Text = "Thank you for submitting your request!";
                    else if (!service.AllowServiceTasksInBackground)
                    {
                        //sectionDescription.Text = "Thank you for submitting your request! We have opened the following request(s) on your behalf. A summary of your request has also been sent to you via email.";
                        sectionDescription.Text = "Thank you for submitting your request! We have opened the following request(s) on your behalf. A summary of your request has also been sent to you via email.";
                    }
                    else
                    {
                        //sectionDescription.Text = "Thank you for submitting your request! We have opened the following ticket on your behalf. Some sub-task(s) or ticket(s) are still being created in the background. You will be notified via email once everything is created.";
                        sectionDescription.Text = "Thank you for submitting your request! We have opened the following ticket on your behalf. Some sub-task(s) or ticket(s) are still being created in the background. You will be notified via email once everything is created.";
                        // "Click on the service ticket below to view the service, or click X to close. Some sub-task(s) or ticket(s) are still being created in the background. A summary of your request will also be sent to you via email.";
                    }

                }
                else if (stepID == 1)
                    sectionDescription.Text = "How can we help you today? Please select a service from the list below.";
            }
            else if (stepID == 2)
            {
                if (sectionID == SUMMARYSUBSECTION)
                {
                    _tenantOnBoardingHelper = new TenantOnBoardingHelper(ApplicationContext);
                    pStep2SectionSContainer.Visible = true;
                    sectionDescription.Text = "Please review  information and If ok, click \"Submit\", or \"Previous\" to make corrections.";
                    if (_tenantOnBoardingHelper.GetNewTenantTemplateOnBoarding().Any(x => x.EqualsIgnoreCase(service.Title)))
                    {
                        sectionDescription.Text = "Please review employee information and If ok, click \"Submit\", or \"Previous\" to make corrections.";
                    }
                    if (_tenantOnBoardingHelper.GetNewTenantTemplateServiceTitles().Any(x => x.EqualsIgnoreCase(service.Title)))
                    {
                        sectionDescription.Text = "Please review registration information and If ok, click \"Submit\", or \"Previous\" to make corrections.";
                    }
                }
                else
                {
                    pStep2SectionContainer.Visible = true;
                    ServiceSection section = service.Sections.FirstOrDefault(x => x.ID == sectionID);
                    sectionDescription.Text = string.Empty;
                    if (section != null)
                    {
                        if (!string.IsNullOrEmpty(section.Description))
                            sectionDescription.Text = section.Description.Replace("\n\r", "<br/>").Replace("\n", "<br/>");
                        else
                            sectionDescription.Text = section.Title;
                    }
                }
            }
        }

        private void SetIcons(HtmlGenericControl activeIconDiv, ASPxImage imgSection, string SectionIconUrl, string DefaultIconCss, bool Active = false)
        {
            if (!string.IsNullOrEmpty(SectionIconUrl))
            {
                if (Active)
                    activeIconDiv.Attributes.Add("class", "step_content active");
                else
                    activeIconDiv.Attributes.Add("class", "step_content");

                imgSection.ImageUrl = SectionIconUrl;
                imgSection.Visible = true;
            }
            else
            {
                if (Active)
                    activeIconDiv.Attributes.Add("class", $"step_content {DefaultIconCss} active");
                else
                    activeIconDiv.Attributes.Add("class", $"step_content {DefaultIconCss}");
                imgSection.Visible = true;
                imgSection.ImageUrl = "/Content/Images/employee-info-active.png";
            }
        }

        private void ValidateSectionConditions(ref List<ServiceSectionCondition> skipSectionCondition)
        {
            List<ServiceSectionCondition> sectionConditions = skipSectionCondition;
            if (sectionConditions != null)
            {
                DataTable conditionTable = GetSkipSectionConditions(sectionConditions);
                foreach (ServiceSectionCondition condition in sectionConditions)
                {
                    condition.ConditionValidate = UGITUtility.StringToBoolean(conditionTable.Rows[0][condition.ID.ToString()]);
                }
            }
        }

        private DataTable GetSkipTaskConditions(List<ServiceTaskCondition> Conditions)
        {
            //Creates table which contain all condition as boolean column
            DataTable conditionTable = new DataTable();
            foreach (ServiceTaskCondition condition in Conditions)
            {
                conditionTable.Columns.Add(condition.ID.ToString(), typeof(bool));
            }

            string conditionToken = string.Empty;

            //Iterate each condition to check its validity
            foreach (ServiceTaskCondition condition in Conditions)
            {
                //If there is not condition defined then just discard that condition
                if (condition.Conditions == null || condition.Conditions.Count <= 0) continue;


                //Only picks first condition because we are supporting and or logical operator right now.
                WhereExpression whereClause = condition.Conditions[0];

                //Gets question token and if it is not come with [$$] format then remove it into token
                conditionToken = whereClause.Variable;
                if (conditionToken.IndexOf("[$") > 0)
                {
                    conditionToken = conditionToken.Replace("[$", string.Empty).Replace("$]", string.Empty);
                }

                //Discard condition if condition's question is not found against token
                ServiceQuestion tokenQuestion = service.Questions.FirstOrDefault(x => x.TokenName.ToLower() == conditionToken.ToLower());
                if (tokenQuestion == null) continue;

                //Discard condition if input is not ask yet
                string lsValue = string.Empty;
                ServiceSectionInput tokenSectionInput = _serviceInput.ServiceSections.FirstOrDefault(x => x.Questions.Exists(y => y.Token.ToLower() == conditionToken.ToLower()));
                ServiceQuestionInput tokenQuestionInput = null;
                if (tokenSectionInput != null)
                    tokenQuestionInput = tokenSectionInput.Questions.FirstOrDefault(x => x.Token.ToLower() == conditionToken.ToLower());

                if (tokenQuestionInput != null)
                    lsValue = tokenQuestionInput.Value;

                bool result = ServiceQuestionManager.TestCondition(lsValue, whereClause.Operator, whereClause.Value, tokenQuestion.QuestionType);
                conditionTable.Columns[condition.ID.ToString()].Expression = string.Format("{0}", result);
            }

            DataRow row = conditionTable.NewRow();
            conditionTable.Rows.Add(row);

            return conditionTable;
        }

        private DataTable GetSkipSectionConditions(List<ServiceSectionCondition> Conditions)
        {
            //Creates table which contain all condition as boolean column
            DataTable conditionTable = new DataTable();
            if (Conditions != null)
            {
                foreach (ServiceSectionCondition condition in Conditions)
                {
                    conditionTable.Columns.Add(condition.ID.ToString(), typeof(bool));
                }

                string conditionToken = string.Empty;

                //Iterate each condition to check its validity
                foreach (ServiceSectionCondition condition in Conditions)
                {
                    //If there is not condition defined then just discard that condition
                    if (condition.Conditions == null || condition.Conditions.Count <= 0) continue;

                    bool isQuestionNotFound = false;
                    List<WhereExpression> whrExpressions = condition.Conditions;

                    // Initialize itemId for each where expression if any Id is set to zero in any whereExpression
                    if (whrExpressions.Any(x => x.Id == 0))
                    {
                        int itemIndex = 0;
                        foreach (WhereExpression wExpression in whrExpressions)
                        {
                            itemIndex++;
                            wExpression.Id = itemIndex;
                        }
                    }

                    StringBuilder expression = new StringBuilder();
                    List<WhereExpression> rootWhere = whrExpressions.Where(x => x.ParentId == 0).ToList();

                    foreach (WhereExpression rWhere in rootWhere)
                    {
                        if (!string.IsNullOrEmpty(rWhere.LogicalRelOperator) && rWhere.LogicalRelOperator.ToLower() != "none")
                            expression.Append(rWhere.LogicalRelOperator + " ");

                        List<WhereExpression> subWhere = new List<WhereExpression>();
                        WhereExpression rWhereCopy = rWhere;
                        subWhere.Add(rWhereCopy);
                        subWhere.AddRange(whrExpressions.Where(x => x.ParentId == rWhere.Id));

                        List<string> expList = new List<string>();
                        for (int i = 0; i < subWhere.Count; i++)
                        {
                            StringBuilder subQuery = new StringBuilder();
                            WhereExpression where = subWhere[i];

                            if (expList.Count > 0 && !string.IsNullOrEmpty(where.LogicalRelOperator) && where.LogicalRelOperator.ToLower() != "none")
                                subQuery.AppendFormat(where.LogicalRelOperator + " ");

                            #region calculation to find whether the input value meets the skip logic for current question only
                            //Gets question token and if it is not come with [$$] format then remove it into token
                            conditionToken = where.Variable;
                            if (conditionToken.IndexOf("[$") > 0)
                            {
                                conditionToken = conditionToken.Replace("[$", string.Empty).Replace("$]", string.Empty);
                            }

                            //Discard condition if condition's question is not found against token
                            ServiceQuestion tokenQuestion = service.Questions.FirstOrDefault(x => x.TokenName.ToLower() == conditionToken.ToLower());
                            if (tokenQuestion == null)
                            {
                                isQuestionNotFound = true;
                                break;
                            }

                            //Discard condition if input is not ask yet
                            string lsValue = string.Empty;
                            ServiceSectionInput tokenSectionInput = _serviceInput.ServiceSections.FirstOrDefault(x => x.Questions.Exists(y => y.Token.ToLower() == conditionToken.ToLower()));
                            ServiceQuestionInput tokenQuestionInput = null;
                            if (tokenSectionInput != null)
                                tokenQuestionInput = tokenSectionInput.Questions.FirstOrDefault(x => x.Token.ToLower() == conditionToken.ToLower());

                            if (tokenQuestionInput != null)
                                lsValue = tokenQuestionInput.Value;

                            bool result = ServiceQuestionManager.TestCondition(lsValue, where.Operator, where.Value, tokenQuestion.QuestionType);

                            #endregion calculation to find whether the input value meets the skip logic for current question only

                            subQuery.Append(result);
                            expList.Add(subQuery.ToString());
                        }

                        if (expList.Count == 1)
                            expression.AppendFormat("{0} ", string.Join(" ", expList));
                        else if (expList.Count > 1)
                            expression.AppendFormat("({0}) ", string.Join(" ", expList));

                        if (isQuestionNotFound)
                            break;
                    }

                    string finalExpression = (expression.ToString()).Trim().ToLower();

                    bool finalResult = EvaluateStringToBoolean(finalExpression);

                    if (isQuestionNotFound)
                        continue;

                    conditionTable.Columns[condition.ID.ToString()].Expression = string.Format("{0}", finalResult);
                }
            }
            DataRow row = conditionTable.NewRow();
            conditionTable.Rows.Add(row);

            return conditionTable;
        }

        private List<UGITTask> GetTasksNeedToCreate(List<UGITTask> tasks)
        {
            if (service.SkipTaskCondition != null && service.SkipTaskCondition.Count > 0)
            {
                DataTable skipTaskConditions = GetSkipTaskConditions(service.SkipTaskCondition);
                bool isTrue = false;
                ServiceTaskCondition taskCondition = null;
                foreach (DataColumn coln in skipTaskConditions.Columns)
                {
                    isTrue = UGITUtility.StringToBoolean(skipTaskConditions.Rows[0][coln.ColumnName]);
                    if (isTrue)
                    {
                        taskCondition = service.SkipTaskCondition.FirstOrDefault(x => x.ID.ToString() == coln.ColumnName);
                        foreach (int taskID in taskCondition.SkipTasks)
                        {
                            UGITTask sTask = tasks.FirstOrDefault(x => x.ID == taskID);
                            if (sTask != null)
                            {
                                tasks.Remove(sTask);
                            }
                        }
                    }
                    isTrue = false;
                }
            }
            return tasks;
        }

        private void SaveAttachedFiles()
        {

            //Save attachment in temp area
            if (Request.Files.Count > 0)
            {
                string fileName = string.Empty;
                string extension = string.Empty;
                string suffix = string.Empty;
                string fullName = string.Empty;
                string tempPath = uHelper.GetTempFolderPath();

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    extension = Path.GetExtension(Request.Files[i].FileName);
                    fileName = Path.GetFileNameWithoutExtension(Request.Files[i].FileName);
                    fileName = System.Text.RegularExpressions.Regex.Replace(fileName, @"[~#%&*{}\<>?/+|""]", "_", RegexOptions.IgnoreCase);
                    string fileWithExt = fileName + extension;
                    if (Request.Files[i].ContentLength > 0 && ddlExistingAttc.Items.FindByText(fileWithExt) == null)
                    {
                        suffix = Guid.NewGuid().ToString();
                        fullName = string.Format("{0}_{1}{2}", fileName, suffix, extension);
                        Request.Files[i].SaveAs(string.Format("{0}/{1}", tempPath, fullName));
                        ddlExistingAttc.Items.Add(new ListItem(fileWithExt, fullName));
                    }
                }
            }

        }

        private void ShowAttachmentList(bool showSummary)
        {
            if (service != null && service.AttachmentRequired.ToLower() == "disabled")
            {
                pAttachmentContainer.Visible = false;
                return;
            }
            else
            {
                pAttachmentContainer.Visible = true;
            }

            if (service != null && service.ServiceType.ToLower() == Constants.ModuleFeedback.ToLower().Replace("~", ""))
            {
                divpAttachmentContainer.Style.Add("display", "none");
            }

            string deleteElement = string.Empty;
            if (showSummary)
            {
                if (!string.IsNullOrEmpty(fileuploadServiceAttach.GetValue()))
                {
                    fileuploadServiceAttach.fileUpload.Visible = false;
                }
            }

            //if (showSummary)
            //{
            //    if (ddlExistingAttc.Items.Count <= 0)
            //    {
            //        pAttachmentContainer.Visible = false;
            //    }
            //    lbAttachment.Text = "Attachment(s): ";
            //    pAddattachment.Visible = false;
            //}
            //else
            //{
            //    lbAttachment.Text = "Please attach any supporting document(s) or screenshot(s): ";
            //    if (hdnAttachmentMandatory.Value == "true")
            //        lbAttachment.Text += "<b id='attachmentmandatoryicon' style='color:red;'>*</b>";
            //    else
            //        lbAttachment.Text += "<b id='attachmentmandatoryicon' style='color:red;display:none;'>*</b>";

            //    //Remove deleted attachmen from existing attachment list
            //    List<string> deleteFiles = uHelper.ConvertStringToList(txtDeleteFiles.Text, new string[] { "~" });
            //    ListItem item = null;
            //    foreach (string dFile in deleteFiles)
            //    {
            //        item = ddlExistingAttc.Items.FindByValue(dFile);
            //        if (item != null)
            //            ddlExistingAttc.Items.Remove(item);
            //    }
            //    txtDeleteFiles.Text = string.Empty;
            //}
            //foreach (ListItem item in ddlExistingAttc.Items)
            //{
            //    deleteElement = string.Format("<label onclick='removeAttachment(this);'><img src='/_layouts/15/images/ugovernit/delete-icon.png' alt='Delete'/><span style='display:none;'>{0}</span></label>", item.Value);
            //    if (showSummary)
            //        deleteElement = string.Empty;

            //    string documentUrl = uHelper.GetAbsoluteURL(string.Format("/_layouts/15/images/ugovernittemp/{0}", item.Value));
            //    pAttachment.Controls.Add(new LiteralControl(string.Format("<div class='fileitem fileread'><span onclick='window.open(\"{2}\")'><b>{1}.&nbsp;</b>{0}</span>{3}</div>", item.Text.Replace("'", "\'").Replace("\"", "\\\""), ddlExistingAttc.Items.IndexOf(item) + 1, documentUrl, deleteElement)));
            //}

            //if (ddlExistingAttc.Items.Count < 5 && !showSummary)
            //{
            //    deleteElement = string.Format("<label onclick='removeAttachment(this);'><img src='/_layouts/15/images/ugovernit/delete-icon.png' alt='Delete'/></label>");
            //    if (showSummary)
            //        deleteElement = string.Empty;

            //    pNewAttachment.Controls.Add(new LiteralControl(string.Format("<div class='fileitem fileupload'><span><input type='file' name='pAttachment1' /></span>{0}</div>", deleteElement)));
            //    if (ddlExistingAttc.Items.Count == 4)
            //    {
            //        pAddattachment.Style.Add(HtmlTextWriterStyle.Display, "none");
            //    }
            //}
            //else
            //{
            //    pAddattachment.Style.Add(HtmlTextWriterStyle.Display, "none");
            //}
        }

        /// <summary>
        /// Its marks input section or question skip if they come under any validated ServiceSectionCondition
        /// </summary>
        protected void RefreshServiceInputs()
        {
            if (_serviceInput == null || _serviceInput.ServiceSections == null)
                return;

            ServiceQuestion question = null;
            foreach (ServiceSectionInput sInput in _serviceInput.ServiceSections)
            {
                if (service.SkipSectionCondition != null)
                {
                    List<ServiceSectionCondition> sectionConditions = service.SkipSectionCondition.Where(x => x.ConditionValidate).ToList();
                    if (sectionConditions.Exists(x => x.SkipSectionsID.Exists(y => y == sInput.SectionID)))
                    {
                        sInput.IsSkiped = true;
                        foreach (ServiceQuestionInput qInput in sInput.Questions)
                        {
                            qInput.IsSkiped = true;
                        }
                    }
                    else
                    {
                        sInput.IsSkiped = false;
                        foreach (ServiceQuestionInput qInput in sInput.Questions)
                        {
                            question = service.Questions.FirstOrDefault(x => x.TokenName == qInput.Token);
                            if (question == null)
                                continue;

                            if (sectionConditions.Exists(x => x.SkipQuestionsID.Exists(y => y == question.ID)))
                            {
                                qInput.IsSkiped = true;
                            }
                            else
                            {
                                qInput.IsSkiped = false;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        private ServiceResponseTreeNodeParent SubmitAgent(string actionType)
        {
            int moduleId = uHelper.getModuleIdByModuleName(ApplicationContext, moduleName);
            List<TicketColumnError> errors = new List<TicketColumnError>();
            ServiceResponseTreeNodeParent result = null;
            foreach (string item in ticketIdsForAgent)
            {
                Ticket ticketRequest = new Ticket(ApplicationContext, moduleName);
                DataRow currentTicket = Ticket.GetCurrentTicket(ApplicationContext, moduleName, item);

                List<ServiceQuestionMapping> qMappingList = service.QuestionsMapping;
                List<ServiceQuestionMapping> serviceInstMappingList = qMappingList.Where(x => x.ServiceTaskID == null).ToList();
                List<TicketColumnValue> formValues = new List<TicketColumnValue>();
                Dictionary<string, object> sTicketData = new Dictionary<string, object>();
                foreach (ServiceQuestionMapping questMap in serviceInstMappingList)
                {
                    if (!currentTicket.Table.Columns.Contains(questMap.ColumnName))
                        continue;
                    TicketColumnValue ticket = new TicketColumnValue();
                    ticket.InternalFieldName = questMap.ColumnName;

                    ServiceRequestBL requestBL = new ServiceRequestBL(HttpContext.Current.GetManagerContext());
                    ServiceRequestDTO serviceRequestObj = new ServiceRequestDTO();
                    ticket.Value = requestBL.generateColumnValue(moduleId, moduleName, questMap, currentTicket.Table.Columns[questMap.ColumnName], service, serviceRequestObj);
                    if (ticket.InternalFieldName.ToLower() == DatabaseObjects.Columns.TicketPctComplete.ToLower())
                        ticket.Value = Convert.ToDouble(ticket.Value) / 100;
                    if (Convert.ToString(ticket.Value).Contains("~"))
                    {
                        ticket.Value = Convert.ToString(ticket.Value).Replace("~", ";");
                    }
                    formValues.Add(ticket);
                    if (ticket.InternalFieldName.ToLower() == DatabaseObjects.Columns.TicketRequestTypeLookup.ToLower())
                    {
                        ServiceQuestion mappedQuestion = service.Questions.FirstOrDefault(x => x.ID == questMap.ServiceQuestionID);
                        ServiceQuestionInput mappedQInputObj = null;
                        if (mappedQuestion != null)
                        {
                            ServiceSectionInput questSection = _serviceInput.ServiceSections.FirstOrDefault(x => x.SectionID == mappedQuestion.ServiceSectionID && x.IsSkiped == false);
                            if (questSection != null)
                            {
                                mappedQInputObj = questSection.Questions.FirstOrDefault(x => x.Token == mappedQuestion.TokenName && x.IsSkiped == false);
                                if (mappedQInputObj != null && mappedQInputObj.SubTokensValue != null)
                                {
                                    ServiceQuestionInput issueType = mappedQInputObj.SubTokensValue.FirstOrDefault(x => x.Token == "issuetype");
                                    if (issueType != null)
                                    {
                                        TicketColumnValue ticketIssueType = new TicketColumnValue();
                                        ticketIssueType.InternalFieldName = DatabaseObjects.Columns.UGITIssueType;
                                        ticketIssueType.Value = issueType.Value;
                                        formValues.Add(ticketIssueType);
                                    }
                                }
                            }
                        }
                    }


                }

                ticketRequest.SetItemValues(currentTicket, formValues, false, true, User.Id);

                if (actionType.ToLower() == "approve")
                {
                    if (!Ticket.IsActionUser(ApplicationContext, currentTicket, User) && UserProfileManager.IsUGITSuperAdmin(User))
                        ticketRequest.ApproveTicket(errors, currentTicket, true);
                    else
                        ticketRequest.ApproveTicket(errors, currentTicket, false);
                }
                else if (actionType.ToLower() == "reject")
                {
                    bool valid = true;
                    string rejectComments = UGITUtility.WrapComment(popedRejectComments.Text.Trim(), "reject");
                    ticketRequest.Reject(currentTicket, rejectComments);
                    string error = ticketRequest.CommitChanges(currentTicket, string.Empty, Request.Url);
                    if (!string.IsNullOrEmpty(error))
                    {
                        errors.Add(TicketColumnError.AddError(error));
                        valid = false;
                    }
                    if (valid)
                    {
                        // Send reject/cancel notification to action users
                        if (ticketRequest.Module.ActionUserNotificationOnCancel || ticketRequest.Module.RequestorNotificationOnCancel || ticketRequest.Module.InitiatorNotificationOnCancel)
                        {
                            string ticketId = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketId]);
                            string title = Convert.ToString(currentTicket[DatabaseObjects.Columns.Title]);
                            string moduleType = UGITUtility.moduleTypeName(moduleName);
                            string subject = string.Format("{0} Cancelled: {1}", moduleType, title);
                            string mailBody = string.Format("The {0} <b>{1}</b> was cancelled. <br/><br/>{2}", moduleType, title, UGITUtility.WrapCommentForEmail("rejected", "reject"));
                            if (ticketRequest.Module.ActionUserNotificationOnCancel)
                                ticketRequest.SendEmailToActionUsers(currentTicket, subject, mailBody);
                            if (ticketRequest.Module.RequestorNotificationOnCancel)
                                ticketRequest.SendEmailToRequestor(currentTicket, subject, mailBody);
                            if (ticketRequest.Module.InitiatorNotificationOnCancel)
                                ticketRequest.SendEmailToInitiator(currentTicket, subject, mailBody);
                        }

                        // Delete any pending escalations for this ticket
                        // Got to do it here since escalations block below may not be executed if not valid
                        EscalationProcess escalationProcess = new EscalationProcess(ApplicationContext);
                        escalationProcess.DeleteEscalation(currentTicket);
                    }
                }
                else if (actionType.ToLower() == "return")
                {
                    string currentTicketStatus = Convert.ToString(UGITUtility.GetSPItemValue(currentTicket, DatabaseObjects.Columns.TicketStatus));
                    string returnType = (currentTicketStatus.ToLower() == "closed" ? "reopen" : "return");

                    LifeCycleStage currentStage = ticketRequest.GetTicketCurrentStage(currentTicket);
                    ticketRequest.Return(moduleId, currentTicket, popedReturnComments.Text.Trim());
                    ticketRequest.CommitChanges(currentTicket, string.Empty, Request.Url);
                }
                else if (string.IsNullOrEmpty(actionType))
                {
                    // If action users were changed as a result of the agent, update the field
                    bool actionUsersChanged = false;
                    if (currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUsers) && currentTicket.Table.Columns.Contains(DatabaseObjects.Columns.TicketStageActionUserTypes))
                    {
                        string oldActionUsers = Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUsers]);
                        string newActionUsers = uHelper.GetUsersAsString(ApplicationContext, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), currentTicket);
                        if (oldActionUsers != newActionUsers)
                        {
                            actionUsersChanged = true;
                            currentTicket[DatabaseObjects.Columns.TicketStageActionUsers] = newActionUsers;
                        }
                    }
                    ticketRequest.CommitChanges(currentTicket, string.Empty, Request.Url);
                    if (actionUsersChanged)
                    {
                        LifeCycleStage currentStage = ticketRequest.GetTicketCurrentStage(currentTicket);
                        ticketRequest.SendEmailToActionUsers(currentStage.ID.ToString(), currentTicket, Convert.ToString(moduleId), string.Empty, string.Empty);
                    }
                }
            }

            result = new ServiceResponseTreeNodeParent();
            result.TicketId = "Data collected for Ticket(s) " + string.Join(";", ticketIdsForAgent.ToArray());
            return result;
        }

        private ServiceResponseTreeNodeParent SubmitSurvey(ServiceRequestDTO serviceRequestObj, Services service)
        {
            ServiceResponseTreeNodeParent result = null;
            Dictionary<string, object> sfTicketData = new Dictionary<string, object>();
            SurveyFeedbackManager surveyFeedbackMGR = new SurveyFeedbackManager(HttpContext.Current.GetManagerContext());
            SurveyFeedback sfItem = new SurveyFeedback();
            //Generate Question Answer list which will be used to save data in description of surveyfeedback
            List<ServiceQuestionAnswer> questAnsList = GetFormatedAnswers();
            double totalRating = 0;

            sfItem.Rating1 = 0;
            sfItem.Rating2 = 0;
            sfItem.Rating3 = 0;
            sfItem.Rating4 = 0;
            sfItem.Rating5 = 0;
            sfItem.Rating6 = 0;
            sfItem.Rating7 = 0;
            sfItem.Rating8 = 0;
            sfItem.TotalRating = 0;

            List<ServiceQuestion> ratingQuestions = service.Questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.Rating).ToList();

            //service lookup..
            sfItem.ServiceLookUp = service.ID;

            Dictionary<string, double> ratingWeights = new Dictionary<string, double>();
            double weight = 0;
            string weightStr = string.Empty;
            foreach (ServiceQuestion rQuestion in ratingQuestions)
            {
                rQuestion.QuestionTypePropertiesDicObj.TryGetValue("weight", out weightStr);
                double.TryParse(weightStr, out weight);
                ratingWeights.Add(rQuestion.TokenName, weight);
            }
            double totalWeight = ratingWeights.Sum(x => x.Value);

            foreach (ServiceQuestion rQuestion in ratingQuestions)
            {
                if (rQuestion.TokenName == DatabaseObjects.Columns.Rating1)
                {
                    QuestionsDTO questionsDTO = serviceRequestObj.Questions.FirstOrDefault(x => x.Token == DatabaseObjects.Columns.Rating1);
                    if (questionsDTO != null && !string.IsNullOrEmpty(questionsDTO.Value))
                    {
                        sfItem.Rating1 = UGITUtility.StringToInt(questionsDTO.Value);
                        if (totalWeight > 0 && ratingWeights.ContainsKey(DatabaseObjects.Columns.Rating1))
                            totalRating += (sfItem.Rating1 * ratingWeights[DatabaseObjects.Columns.Rating1]) / totalWeight;
                    }
                }
                if (rQuestion.TokenName == DatabaseObjects.Columns.Rating2)
                {
                    QuestionsDTO questionsDTO = serviceRequestObj.Questions.FirstOrDefault(x => x.Token == DatabaseObjects.Columns.Rating2);
                    if (questionsDTO != null && !string.IsNullOrEmpty(questionsDTO.Value))
                    {
                        sfItem.Rating2 = UGITUtility.StringToInt(questionsDTO.Value);
                        if (totalWeight > 0 && ratingWeights.ContainsKey(DatabaseObjects.Columns.Rating2))
                            totalRating += (sfItem.Rating2 * ratingWeights[DatabaseObjects.Columns.Rating2]) / totalWeight;
                    }

                }
                if (rQuestion.TokenName == DatabaseObjects.Columns.Rating3)
                {
                    QuestionsDTO questionsDTO = serviceRequestObj.Questions.FirstOrDefault(x => x.Token == DatabaseObjects.Columns.Rating3);
                    if (questionsDTO != null && !string.IsNullOrEmpty(questionsDTO.Value))
                    {
                        sfItem.Rating3 = UGITUtility.StringToInt(questionsDTO.Value);
                        if (totalWeight > 0 && ratingWeights.ContainsKey(DatabaseObjects.Columns.Rating3))
                            totalRating += (sfItem.Rating3 * ratingWeights[DatabaseObjects.Columns.Rating3]) / totalWeight;
                    }

                }
                if (rQuestion.TokenName == DatabaseObjects.Columns.Rating4)
                {
                    QuestionsDTO questionsDTO = serviceRequestObj.Questions.FirstOrDefault(x => x.Token == DatabaseObjects.Columns.Rating4);
                    if (questionsDTO != null && !string.IsNullOrEmpty(questionsDTO.Value))
                    {
                        sfItem.Rating4 = UGITUtility.StringToInt(questionsDTO.Value);
                        if (totalWeight > 0 && ratingWeights.ContainsKey(DatabaseObjects.Columns.Rating4))
                            totalRating += (sfItem.Rating4 * ratingWeights[DatabaseObjects.Columns.Rating4]) / totalWeight;
                    }

                }
                if (rQuestion.TokenName == DatabaseObjects.Columns.Rating5)
                {
                    QuestionsDTO questionsDTO = serviceRequestObj.Questions.FirstOrDefault(x => x.Token == DatabaseObjects.Columns.Rating5);
                    if (questionsDTO != null && !string.IsNullOrEmpty(questionsDTO.Value))
                    {
                        sfItem.Rating5 = UGITUtility.StringToInt(questionsDTO.Value);
                        if (totalWeight > 0 && ratingWeights.ContainsKey(DatabaseObjects.Columns.Rating5))
                            totalRating += (sfItem.Rating5 * ratingWeights[DatabaseObjects.Columns.Rating5]) / totalWeight;
                    }

                }
                if (rQuestion.TokenName == DatabaseObjects.Columns.Rating6)
                {
                    QuestionsDTO questionsDTO = serviceRequestObj.Questions.FirstOrDefault(x => x.Token == DatabaseObjects.Columns.Rating6);
                    if (questionsDTO != null && !string.IsNullOrEmpty(questionsDTO.Value))
                    {
                        sfItem.Rating6 = UGITUtility.StringToInt(questionsDTO.Value);
                        if (totalWeight > 0 && ratingWeights.ContainsKey(DatabaseObjects.Columns.Rating6))
                            totalRating += (sfItem.Rating6 * ratingWeights[DatabaseObjects.Columns.Rating6]) / totalWeight;
                    }

                }
                if (rQuestion.TokenName == DatabaseObjects.Columns.Rating7)
                {
                    QuestionsDTO questionsDTO = serviceRequestObj.Questions.FirstOrDefault(x => x.Token == DatabaseObjects.Columns.Rating7);
                    if (questionsDTO != null && !string.IsNullOrEmpty(questionsDTO.Value))
                    {
                        sfItem.Rating7 = UGITUtility.StringToInt(questionsDTO.Value);
                        if (totalWeight > 0 && ratingWeights.ContainsKey(DatabaseObjects.Columns.Rating7))
                            totalRating += (sfItem.Rating7 * ratingWeights[DatabaseObjects.Columns.Rating7]) / totalWeight;
                    }

                }
                if (rQuestion.TokenName == DatabaseObjects.Columns.Rating8)
                {
                    QuestionsDTO questionsDTO = serviceRequestObj.Questions.FirstOrDefault(x => x.Token == DatabaseObjects.Columns.Rating8);
                    if (questionsDTO != null && !string.IsNullOrEmpty(questionsDTO.Value))
                    {
                        sfItem.Rating1 = UGITUtility.StringToInt(questionsDTO.Value);
                        if (totalWeight > 0 && ratingWeights.ContainsKey(DatabaseObjects.Columns.Rating8))
                            totalRating += (sfItem.Rating8 * ratingWeights[DatabaseObjects.Columns.Rating8]) / totalWeight;
                    }

                }
            }

            surveyFeedbackMGR.Insert(sfItem);
            //sfItem.TotalRating = Math.Round(Convert.ToDouble( totalRating), 2);
            result = new ServiceResponseTreeNodeParent();
            sfItem.UserLocation = User.Location;
            sfItem.UserDepartment = User.Department;
            //set blank if generic type survey feedback..
            if (moduleName.ToLower() != "generic")
                sfItem.ModuleName = GetModuleName();

            string ticketID = string.Empty;
            if (Request["ticketid"] != null && Request["ticketid"].Trim() != string.Empty)
            {
                ticketID = Request["ticketid"].Trim();
            }
            if (ticketID.Contains('-'))
                sfItem.TicketId = ticketID;

            if (ticketID.ToLower() == "generic")
                sfItem.Title = ticketID;
            if (!string.IsNullOrEmpty(ticketID))
            {
                string modulename = uHelper.getModuleNameByTicketId(ticketID);
                if (!string.IsNullOrEmpty(modulename))
                {
                    DataRow currentone = Ticket.GetCurrentTicket(ApplicationContext, modulename, ticketID);
                    if (currentone != null)
                    {
                        if (currentone.Table.Columns.Contains(DatabaseObjects.Columns.Title))
                            sfItem.Title = Convert.ToString(currentone[DatabaseObjects.Columns.Title]);

                        if (currentone.Table.Columns.Contains(DatabaseObjects.Columns.TicketRequestTypeCategory))
                            sfItem.CategoryName = UGITUtility.RemoveIDsFromLookupString(Convert.ToString(currentone[DatabaseObjects.Columns.TicketRequestTypeCategory]));

                        if (currentone.Table.Columns.Contains(DatabaseObjects.Columns.SubCategory))
                            sfItem.SubCategory = Convert.ToString(currentone[DatabaseObjects.Columns.SubCategory]);

                        if (currentone.Table.Columns.Contains(DatabaseObjects.Columns.TicketRequestType))
                            sfItem.RequestType = Convert.ToString(currentone[DatabaseObjects.Columns.TicketRequestType]);

                        if (currentone.Table.Columns.Contains(DatabaseObjects.Columns.SLADisabled))
                            sfItem.SLADisabled = Convert.ToBoolean(currentone[DatabaseObjects.Columns.SLADisabled]);
                    }
                }
            }

            //Save question answer in to description
            //Added here to save questions answers in to description
            if (questAnsList.Count > 0)
            {
                XmlDocument xmlDoc = uHelper.SerializeObject(questAnsList);
                sfItem.Description = xmlDoc.OuterXml;
            }

            surveyFeedbackMGR.Update(sfItem);
            /*
            //Save question answer in to description
            if (questAnsList.Count > 0)
            {
                XmlDocument xmlDoc = uHelper.SerializeObject(questAnsList);
                sfItem.Description = xmlDoc.OuterXml;
            }
            */
            result.Text = "Thank you! for you submitting Survey.";
            return result;
        }

        protected void btApprove_Click(object sender, EventArgs e)
        {
            long hVallue = service.Sections.LastOrDefault().ID;
            if (service.HideSummary && hVallue == currentStepSectionID)
                BtNext_Click(null, null);
            lvStepSections.Visible = false;
            pStep3Section1Container.CssClass = "col_b_complete";
            isNextPrevious = true;
            RefreshServiceInputs();
            pAttachmentContainer.Visible = false;
            //If service null then returns right away
            if (service == null)
                return;

            isNextPrevious = true;
            ServiceResponseTreeNodeParent result = null;
            if (service != null && service.ServiceType.ToLower() == Constants.ModuleAgent.ToLower().Replace("~", "") && !string.IsNullOrEmpty(moduleName))
            {
                result = SubmitAgent("approve");
            }
            currentStepID = 3;
            currentStepSectionID = 1;
            if (service != null && service.HideThankYouScreen)
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);

            }
            else
            {
                GenerateTaskSummary(result);
            }
        }

        protected void btReject_Click(object sender, EventArgs e)
        {
            long hVallue = service.Sections.LastOrDefault().ID;
            if (service.HideSummary && hVallue == currentStepSectionID)
                BtNext_Click(null, null);
            lvStepSections.Visible = false;
            pStep3Section1Container.CssClass = "col_b_complete";
            isNextPrevious = true;
            RefreshServiceInputs();
            pAttachmentContainer.Visible = false;
            //If service null then returns right away
            if (service == null)
                return;

            isNextPrevious = true;
            ServiceResponseTreeNodeParent result = null;
            List<TicketColumnError> errors = new List<TicketColumnError>();
            if (service != null && service.ServiceType.ToLower() == Constants.ModuleAgent.ToLower().Replace("~", "") && !string.IsNullOrEmpty(moduleName))
            {
                result = SubmitAgent("reject");
            }
            currentStepID = 3;
            currentStepSectionID = 1;
            if (service != null && service.HideThankYouScreen)
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);

            }
            else
            {
                GenerateTaskSummary(result);
            }
        }

        protected void btReturn_Click(object sender, EventArgs e)
        {
            long hVallue = service.Sections.LastOrDefault().ID;
            if (service.HideSummary && hVallue == currentStepSectionID)
                BtNext_Click(null, null);
            lvStepSections.Visible = false;
            pStep3Section1Container.CssClass = "col_b_complete";
            isNextPrevious = true;
            RefreshServiceInputs();
            pAttachmentContainer.Visible = false;
            //If service null then returns right away
            if (service == null)
                return;

            isNextPrevious = true;
            ServiceResponseTreeNodeParent result = null;
            List<TicketColumnError> errors = new List<TicketColumnError>();
            if (service != null && service.ServiceType.ToLower() == Constants.ModuleAgent.ToLower().Replace("~", "") && !string.IsNullOrEmpty(moduleName))
            {
                result = SubmitAgent("return");
            }
            currentStepID = 3;
            currentStepSectionID = 1;
            if (service != null && service.HideThankYouScreen)
            {
                uHelper.ClosePopUpAndEndResponse(Context, true);

            }
            else
            {
                GenerateTaskSummary(result);
            }
        }

        protected void btnRequestType_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Generate Question Answer list which will be used to save data in description of surveyfeedback
        /// </summary>
        /// <returns></returns>
        private List<ServiceQuestionAnswer> GetFormatedAnswers()
        {
            List<ServiceQuestionAnswer> questAnsList = new List<ServiceQuestionAnswer>();
            ServiceQuestionAnswer questAns = null;
            ServiceSectionInput tokenSectionInput = null;
            ServiceQuestionInput questionInput = null;

            string ans = string.Empty;
            foreach (ServiceQuestion question in service.Questions)
            {
                tokenSectionInput = _serviceInput.ServiceSections.FirstOrDefault(x => x.Questions.Exists(y => y.Token.ToLower() == question.TokenName.ToLower()));
                if (tokenSectionInput != null)
                    questionInput = tokenSectionInput.Questions.FirstOrDefault(x => x.Token.ToLower() == question.TokenName.ToLower());


                if (questionInput == null)
                {
                    continue;
                }

                ans = questionInput.Value;
                if (question.QuestionType.ToLower() == "userfield")
                {
                    UserProfile user = null;
                    List<string> userVals = new List<string>();
                    List<UserProfile> userValCollection = UserProfileManager.GetUserInfosById(questionInput.Value); // new SPFieldUserValueCollection(SPContext.Current.Web, questionInput.Value);
                    foreach (UserProfile userV in userValCollection)
                    {
                        user = userV;
                        if (user != null)
                            userVals.Add(user.Name);
                    }
                    ans = string.Join("; ", userVals.ToArray());
                }
                else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.MULTICHOICE && !string.IsNullOrEmpty(questionInput.Value))
                {
                    ans = questionInput.Value.Replace(Constants.Separator2, "; ");
                }
                else if (question.QuestionType.ToLower() == "datetime" && !string.IsNullOrEmpty(questionInput.Value))
                {
                    DateTime date = Convert.ToDateTime(questionInput.Value);

                    string dateMode = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("datemode", out dateMode);
                    if (dateMode.ToLower() == "dateonly")
                    {
                        ans = date.ToString("MM/dd/yyyy");
                    }
                    else if (dateMode.ToLower() == "timeonly")
                    {
                        ans = date.ToString("h:mm tt");
                    }
                }
                else if (question.QuestionType.ToLower() == "lookup" && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {

                }
                else if (question.QuestionType.ToLower() == "requesttype" && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                }

                questAns = new ServiceQuestionAnswer();
                questAns.Token = question.TokenName;
                questAns.Question = question.QuestionTitle;
                questAns.Answer = ans;
                questAnsList.Add(questAns);
                questionInput = null;
            }

            return questAnsList;
        }

        private void SendEmailToNewApplicaitonRequester(string receiverAddress, string TicketId, string companyname = null, string name = null, string title = null, string email = null, bool? iSForwardMailAddress = null)
        {
            string subject = String.Empty;
            string tid = String.Empty;
            string SiteUrl = String.Empty;
            string htmlBody = String.Empty;


            string url = string.Format("{3}{0}?TicketId={1}&ModuleName={2}&Tid={4}", UGITUtility.GetAbsoluteURL(Constants.HomePagePath), TicketId, "SVC",
                ConfigurationManager.AppSettings["SiteUrl"].ToString().TrimEnd('/'), _applicationContext.TenantID);
            string tokenValue = "<a href='" + url + "'>" + TicketId + "</a>";

            tid = QueryString.Encode($"{TicketId}&{receiverAddress}");
            SiteUrl = $"{ConfigurationManager.AppSettings["SiteUrl"]}ApplicationRegistrationRequest/index?tid={tid}";
            if (iSForwardMailAddress == true)
            {
                subject = "New tenant request is registered successfully!";
                htmlBody = @"<html>
                                <head></head>
                                <body>
                                  <p>New tenant request is registered successfully! Registration Ticket id is <strong> " + tokenValue + @" </strong>.
                                  <br>Name: " + name + @" 
                                  <br>Company: " + companyname + @" 
                                  <br>Title: " + title + @" 
                                  <br>Email: " + email + @" 
                                 <br>
                                 Please <a href=" + SiteUrl + @">click here</a> to check the Status of the request.Click on 'Check status' button to enter ticket ID and registered email on registration work-flow page. </p>
                                </body>
                                </html>";
            }
            else
            {
                subject = "Your New tenant request is registered successfully!";
                // The email body for recipients with non-HTML email clients.
                htmlBody = @"<html>
                                <head></head>
                                <body>
                                  <p>Your New tenant request is registered successfully! Registration Ticket id is <strong> " + TicketId + @" </strong>.
                                 <br>
                                 Please <a href=" + SiteUrl + @">click here</a> to check the Status of the request.Click on 'Check status' button to enter ticket ID and registered email on registration work-flow page. </p>
                                </body>
                                </html>";
            }
            var mail = new MailMessenger(HttpContext.Current.GetManagerContext());
            var response = mail.SendMail(receiverAddress, subject, "", htmlBody, true);



            #region AWS SES code
            /*
            // Replace USWest2 with the AWS Region you're using for Amazon SES.
            // Acceptable values are EUWest1, USEast1, and USWest2.
            // Replace sender@example.com with your "From" address.
            // This address must be verified with Amazon SES.
            string senderAddress = ConfigurationManager.AppSettings["registrationSenderMail"].ToString();

            // Replace recipient@example.com with a "To" address. If your account
            // is still in the sandbox, this address must be verified.
            //string receiverAddress = "amar.l@infogen-labs.com";
            // The subject line for the email.

            string awsAccessKeyId = ConfigurationManager.AppSettings["awsAccessKeyId"].ToString();
            string awsSecretAccessKey = ConfigurationManager.AppSettings["awsSecretAccessKey"].ToString();
            using (var client = new AmazonSimpleEmailServiceClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USWest2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { receiverAddress }
                    },
                    Message = new Message
                    {
                        Subject = new Amazon.SimpleEmail.Model.Content(subject),
                        Body = new Body
                        {
                            Html = new Amazon.SimpleEmail.Model.Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            },
                            Text = new Amazon.SimpleEmail.Model.Content
                            {
                                Charset = "UTF-8",
                                Data = textBody
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    // ConfigurationSetName = configSet
                };
                try
                {
                    var response = client.SendEmail(sendRequest);
                }
                catch (Exception ex)
                {
                }
            }
            */
            #endregion
        }

        private bool EvaluateStringToBoolean(string inputExpression)
        {
            DataTable table = new DataTable();
            table.Columns.Add("", typeof(Boolean));
            table.Columns[0].Expression = inputExpression;

            DataRow row = table.NewRow();
            table.Rows.Add(row);
            bool boolResult = UGITUtility.StringToBoolean(row[0]);

            return boolResult;
        }

        #region Show in same line 
        private List<QuestionList> GetQuestionGrouping(List<ServiceQuestion> questions)
        {
            List<QuestionList> lstOf = new List<QuestionList>();
            List<ServiceQuestion> lstOfNextQuestion = new List<ServiceQuestion>();

            QuestionList questionlist = null;
            for (int i = 0; i < questions.Count; i++)
            {
                var ques = questions[i];

                if (i == 0)
                {
                    lstOfNextQuestion.Add(ques);
                    if (questions.Count == 1)
                    {
                        questionlist = new QuestionList();
                        if (lstOfNextQuestion.Count > 0)
                        {
                            questionlist.qLists = lstOfNextQuestion;
                            questionlist.ID = i + 1;
                            lstOf.Add(questionlist);
                        }
                    }
                    continue;
                }

                if (!ques.ContinueSameLine.HasValue || !ques.ContinueSameLine.Value)
                {
                    questionlist = new QuestionList();
                    if (lstOfNextQuestion.Count > 0)
                    {
                        questionlist.qLists = lstOfNextQuestion;
                        questionlist.ID = i + 1;
                        lstOf.Add(questionlist);

                    }
                    lstOfNextQuestion = new List<ServiceQuestion>();
                    lstOfNextQuestion.Add(ques);
                }
                else
                {
                    lstOfNextQuestion.Add(ques);
                }
                if (i == questions.Count - 1)
                {
                    questionlist = new QuestionList();
                    if (lstOfNextQuestion.Count > 0)
                    {
                        questionlist.qLists = lstOfNextQuestion;
                        questionlist.ID = i + 2;
                        lstOf.Add(questionlist);
                    }
                    break;
                }

            }

            return lstOf;
        }
        public class QuestionList
        {
            public int ID { get; set; }
            public List<ServiceQuestion> qLists { get; set; }
        }
        protected void lvgroupquestions_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem listItem = (ListViewDataItem)e.Item;
                QuestionList questionList = (QuestionList)listItem.DataItem;

                HtmlGenericControl groupquesdiv = (HtmlGenericControl)listItem.FindControl("divgroupquestions");
                ListView listView = (ListView)listItem.FindControl("lvQuestions");
                List<long> qListIds = questionList.qLists.Select(x => x.ID).ToList();
               
                
                groupquesdiv.Attributes.Add("containIds", string.Join(Constants.Separator6, qListIds));
                groupquesdiv.Attributes.Add("class", "lvgroupsupportcls");
                string clsStr = "lvgroupsupportcls";

                //if (questionList.qLists.Where(x => x.QuestionType == "ApplicationAccessRequest") != null && questionList.qLists.Where(x => x.QuestionType == "ApplicationAccessRequest").Count() !=0)
                //{
                //    groupquesdiv.Attributes.Add("class", "lvgroupsupportcls col-md-4 col-sm-4 col-xs-12 noPadding");
                //     clsStr = "lvgroupsupportcls col-md-12 col-sm-12 col-xs-12 noPadding";
                //}

                //if (questionList.qLists.Where(x => x.ItemOrder == 7) != null && questionList.qLists.Where(x => x.ItemOrder == 7).Count() != 0)
                //{
                //    groupquesdiv.Attributes.Add("style", "clear:both;");
                //}
                //if (questionList.qLists.Where(x => x.ItemOrder == 10) != null && questionList.qLists.Where(x => x.ItemOrder == 10).Count() != 0)
                //{
                //    groupquesdiv.Attributes.Add("style", "clear:both;");
                //}
                //if (questionList.qLists.Where(x => x.ItemOrder == 4) != null && questionList.qLists.Where(x => x.ItemOrder == 4).Count() != 0)
                //{
                //    groupquesdiv.Attributes.Add("style", "clear:both;");
                //}
                if (qListIds != null && qListIds.Count > 0)
                {
                    List<int> qTohide = new List<int>();
                    foreach (int qid in qListIds)
                    {
                        if (service.SkipSectionCondition.Exists(x => x.ConditionValidate && x.SkipQuestionsID.Exists(y => y == qid)))
                            qTohide.Add(qid);
                    }

                    if (qTohide.Count == qListIds.Count)
                        clsStr += " lvhidegrouping";
                    else
                        clsStr += " lvshowgrouping";

                }

                groupquesdiv.Attributes.Add("class", clsStr);
                listView.DataSource = questionList.qLists;
                listView.DataBind();
            }
        }
        protected void groupQuesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rgroupques = (Repeater)sender;
            RepeaterItem sectionItem = (RepeaterItem)rgroupques.Parent;
            ServiceSectionInput sectionInput = (ServiceSectionInput)sectionItem.DataItem;
            RepeaterItem repeaterItem = (RepeaterItem)e.Item;

            QuestionList groupques = (QuestionList)repeaterItem.DataItem;
            List<ServiceQuestionInput> srvquesinput = new List<ServiceQuestionInput>();
            groupques.qLists.ForEach(x =>
            {
                ServiceQuestionInput quesinput = sectionInput.Questions.FirstOrDefault(y => y.Token == x.TokenName);
                if (quesinput != null)
                    srvquesinput.Add(quesinput);
            });

            Repeater rSummaryioQuest = (Repeater)e.Item.FindControl("rSummaryioQuest");
            rSummaryioQuest.DataSource = srvquesinput;
            rSummaryioQuest.DataBind();
        }
        #endregion
    }
}


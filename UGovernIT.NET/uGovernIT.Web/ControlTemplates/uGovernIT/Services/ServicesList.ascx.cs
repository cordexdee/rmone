using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DevExpress.Utils.Zip;
using System.IO.Compression;
using System.Web;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Core;
using System.Reflection;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Xml.Serialization;
using uGovernIT.Utility.Entities;
using System.Runtime.Serialization;
using System.Xml;
using uGovernIT.Web.Helpers;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class ServicesList : UserControl
    {
        protected string delegateUrl;
        protected string moveToProductionUrl = "";
        protected string serviceHomeUrl;
        public string serviceTypeCookie = string.Empty;
        public string PageTitle = "Edit Service";
        protected string sendSurveyURL;
        protected List<Services> lstServices = new List<Services>();
        ConfigurationVariableManager configVariableHelper = null;
        ServicesManager serviceManager = null;
        ApplicationContext context = null;
        ServiceCategoryManager categoryManager = null;
        NameValueCollection queryCollection = null;
        List<string> keyColl = new List<string>() {
                "userquestion",
                "locationuserquestion",
                "departmentuserquestion",
                "dependentlocationquestion",
                "companydivisions",
                "MirrorAccessFrom",
                "ExistingUser",
                "NewUSer",
                "validateagainst"
            };
        protected override void OnInit(EventArgs e)
        {
            context = HttpContext.Current.GetManagerContext();
            configVariableHelper = new ConfigurationVariableManager(context);
            serviceManager = new ServicesManager(context);
            categoryManager = new ServiceCategoryManager(context);

            serviceTypeCookie = UGITUtility.GetCookieValue(Request, "servicetype");
            if (Request.QueryString["ServiceType"] != null)
                serviceTypeCookie = Constants.ModuleFeedback;
            if (string.IsNullOrEmpty(serviceTypeCookie))
                serviceTypeCookie = "services";
            rdbServiceCategory.SelectedIndex = rdbServiceCategory.Items.IndexOf(rdbServiceCategory.Items.FindByValue(serviceTypeCookie));
            if (serviceTypeCookie.ToLower() == Constants.ModuleService.ToLower())
                PageTitle = "Edit Service";
            if (serviceTypeCookie.ToLower() == Constants.ModuleAgent.ToLower())
                PageTitle = "Edit Agent";
            if (serviceTypeCookie.ToLower() == Constants.ModuleFeedback.ToLower())
                PageTitle = "Edit Survey";

            BindServicesData(serviceTypeCookie);
        }

        protected void BindServicesData(string serviceName)
        {
            if (string.IsNullOrEmpty(serviceName))
                return;

            if (serviceName == "~ModuleAgent~")
                serviceName = Constants.ModuleAgent;
            if (serviceName == "~ModuleFeedback~")
                serviceName = Constants.ModuleFeedback;

            delegateUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=service&category=" + serviceName + "&serviceid=");
            //ServiceCategoryManager objServiceCategoryManager = new ServiceCategoryManager(HttpContext.Current.GetManagerContext());
            //var categories = objServiceCategoryManager.Load();
            //List<Services> services = serviceManager.LoadAllServices(serviceName).OrderBy(x => x.CategoryItemOrder).OrderBy(x => x.CategoryId).ToList();


            //var q = from a in services
            //        join b in categories on a.CategoryId equals b.ID into sc
            //        from newb in sc.DefaultIfEmpty()
            //        select new Services
            //        {
            //            ID = a.ID,
            //            AllowServiceTasksInBackground = a.AllowServiceTasksInBackground,
            //            AttachmentRequired = a.AttachmentRequired,
            //            AttachmentsInChildTickets = a.AttachmentsInChildTickets,
            //            AuthorizedToView = a.AuthorizedToView,
            //            ConditionalLogic = a.ConditionalLogic,
            //            CreateParentServiceRequest = a.CreateParentServiceRequest,
            //            IncludeInDefaultData = a.IncludeInDefaultData,
            //            CustomProperties = a.CustomProperties,
            //            HideSummary = a.HideSummary,
            //            HideThankYouScreen = a.HideThankYouScreen,
            //            ImageUrl = a.ImageUrl,
            //            IsActivated = a.IsActivated,
            //            IsDeleted = a.IsDeleted,
            //            ItemOrder = a.ItemOrder,
            //            CategoryItemOrder = newb?.ItemOrder ?? 0,
            //            LoadDefaultValue = a.LoadDefaultValue,
            //            ModuleNameLookup = a.ModuleNameLookup,
            //            ModuleStage = a.ModuleStage,
            //            NavigationUrl = a.NavigationUrl,
            //            OwnerUser = a.OwnerUser,
            //            OwnerApprovalRequired = a.OwnerApprovalRequired,
            //            QuestionMapVariables = a.QuestionMapVariables,
            //            SectionConditionalLogic = a.SectionConditionalLogic,
            //            CategoryId = a.CategoryId,
            //            ServiceCategoryType = a.ServiceCategoryType,
            //            ServiceDescription = a.ServiceDescription,
            //            ShowStageTransitionButtons = a.ShowStageTransitionButtons,
            //            Title = a.Title,
            //            ServiceType = a.ServiceType,
            //            TenantID = a.TenantID,
            //            Created = a.Created,
            //            Modified = a.Modified,
            //            Deleted = a.Deleted,
            //            Attachments = a.Attachments,
            //            SLADisabled = a.SLADisabled,
            //            ResolutionSLA = a.ResolutionSLA,
            //            CompletionMessage = a.CompletionMessage,
            //            Use24x7Calendar = a.Use24x7Calendar,
            //            EnableTaskReminder = a.EnableTaskReminder,
            //            Reminders = a.Reminders,
            //            NavigationType = a.NavigationType,
            //            StartResolutionSLAFromAssigned = a.StartResolutionSLAFromAssigned,
            //            SLAConfiguration = a.SLAConfiguration
            //        };


            //services = q.ToList();


            UserProfile user;
            UserProfileManager userManager;
            user = HttpContext.Current.CurrentUser();
            userManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            userManager.IsUGITSuperAdmin(user);

            if (user != null && userManager.IsUGITSuperAdmin(user))
            {
                btnPushServices.Visible = true;
            }

            //if (services != null && services.Count > 0)
            //{
            //    lstServices = services;

                if (serviceName == Constants.ModuleFeedback)
                {
                    lnkArchive.Visible = lnkUnArchive.Visible = chkShowDeleted.Visible = btnMigrate.Visible = chkShowDeleted.Checked = false;
                }
                else
                {
                    chkShowDeleted.Visible = btnMigrate.Visible = true;
                }

                if (Request["showarchive"] == null || Request["showarchive"].Trim().ToLower() == "false")
                {
                    if (serviceName == Constants.ModuleFeedback)
                    {
                        lnkArchive.Visible = lnkUnArchive.Visible = chkShowDeleted.Visible = btnMigrate.Visible = false;
                    }
                    else
                    {
                        lnkArchive.Visible = chkShowDeleted.Visible = btnMigrate.Visible = true;
                    }

                    chkShowDeleted.Checked = false;
                    //services = services.Where(x => x.IsDeleted == false).OrderBy(x => x.CategoryItemOrder).ThenBy(x => x.ItemOrder).ToList();
                    //if (services.Count <= 0)
                    //    services = new List<Services>();

                }
                else if (Request["showarchive"].Trim().ToLower() == "true")
                {
                    if (serviceName == Constants.ModuleFeedback)
                    {
                        lnkArchive.Visible = lnkUnArchive.Visible = chkShowDeleted.Visible = chkShowDeleted.Checked = btnMigrate.Visible = false;
                    }
                    else
                    {
                        chkShowDeleted.Visible = btnMigrate.Visible = lnkUnArchive.Visible = true;//chkShowDeleted.Checked = 
                }

                    lnkFullDeleteService.Visible = true;

                    //services = services.Where(x => x.IsDeleted == true).OrderBy(x => x.CategoryItemOrder).ThenBy(x => x.ItemOrder).ToList();
                    //if (services.Count <= 0)
                    //    services = new List<Services>();

                }
            //if (services.Count > 0)
            //{
            //    // update serviceUrl columns with urls
            //    services.Count(v => (v.serviceUrl = string.Format("{0}{1}", delegateUrl, v.ID)) == null);
            //}
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            DataTable dt = GetTableDataManager.GetData(DatabaseObjects.Tables.Services, values);
            DataView view = dt.DefaultView;
            view.RowFilter = string.Format("1 = 1");
            if (serviceName.ToLower() == Constants.ModuleAgent.ToLower())
            {
                if (chkShowDeleted.Visible && chkShowDeleted.Checked == false)
                    view.RowFilter += string.Format(" AND {0} = '{1}' AND {2} = '{3}'", DatabaseObjects.Columns.ServiceType, "ModuleAgent", DatabaseObjects.Columns.Deleted, false);
                else if (chkShowDeleted.Visible && chkShowDeleted.Checked == true)
                    view.RowFilter += string.Format(" AND {0} = '{1}' ", DatabaseObjects.Columns.ServiceType, "ModuleAgent");
            }
            else if (serviceName.ToLower() == Constants.ModuleFeedback.ToLower())
                view.RowFilter += string.Format(" AND {0} = '{1}'", DatabaseObjects.Columns.ServiceType, "ModuleFeedback");
            else
                view.RowFilter += string.Format(" AND {0} <> '{1}' AND {0} <> '{2}' AND {3} = '{4}'",
                    DatabaseObjects.Columns.ServiceType, "ModuleAgent", "ModuleFeedback", DatabaseObjects.Columns.Deleted, false);


            dt = view.ToTable();
            if (dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("serviceUrl"))
                    {
                        dt.Columns.Add("serviceUrl", typeof(string));
                        dt.AcceptChanges();
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["serviceUrl"] = string.Format("{0}{1}", delegateUrl, dr["ID"]);
                    }

                }
            ServiceGrid.DataSource = dt;
            ServiceGrid.DataBind();
            ServiceGrid.SettingsBehavior.AllowGroup = true;
            if (serviceName == Constants.ModuleAgent)
            {
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ModuleNameLookup]).GroupIndex = 0;
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ModuleNameLookup]).Visible = true;
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ServiceCategoryType]).GroupIndex = -1;
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ServiceCategoryType]).Visible = false;
            }
            else if (serviceName == Constants.ModuleFeedback)
            {
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ModuleNameLookup]).GroupIndex = -1;
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ModuleNameLookup]).Visible = false;
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ModuleNameLookup]).GroupIndex = -1;
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ModuleNameLookup]).Visible = false;
            }
            else
            {
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ServiceCategoryType]).GroupIndex = 0;
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ServiceCategoryType]).Visible = true;
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ModuleNameLookup]).GroupIndex = -1;
                ((DevExpress.Web.GridViewDataColumn)ServiceGrid.Columns[DatabaseObjects.Columns.ModuleNameLookup]).Visible = false;
            }
            EnableMigrate();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string serviceCategory = rdbServiceCategory.SelectedItem.Value;
            if (serviceTypeCookie.ToLower() == Constants.ModuleFeedback.ToLower())
                serviceCategory = Constants.ModuleFeedback;

            sendSurveyURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=sendsurvey&sendsurvey=true");
            //serviceHomeUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/uGovernIT/uGovernITConfiguration?control=ServicesWizard&serviceID=")); //UGITUtility.GetAbsoluteURL(string.Format("/SitePages/Home.aspx?serviceID="));
            serviceHomeUrl = UGITUtility.GetAbsoluteURL(string.Format(UGITUtility.ToAbsoluteUrl(Constants.HomePagePath) + "?control=ServicesWizard&serviceID="));

            string categoryListUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=servicecategories");
            //btCategories.NavigateUrl = "javascript:void(0);";
            btCategories.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}' , '', 'Service Categories', '60', '60', 0, \"{1}\");", categoryListUrl, Server.UrlEncode(Request.Url.AbsolutePath)));

            delegateUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/delegatecontrol.aspx?control=service&category=" + serviceCategory + "&serviceid=");
            delegateUrl = string.Format("{0}", delegateUrl);
            btNewbutton.ClientSideEvents.Click = "function(){ window.parent.UgitOpenPopupDialog('" + delegateUrl + "' , '', '" + rdbServiceCategory.SelectedItem.Text + "', '90', '90', 0, '" + Server.UrlEncode(Request.Url.AbsolutePath) + "'); }";


            string importUrl = UGITUtility.GetAbsoluteURL("/layouts/ugovernit/DelegateControl.aspx?control=serviceimport");
            btImport.ClientSideEvents.Click = "function(){ window.parent.UgitOpenPopupDialog('" + importUrl + "' , '', 'Import Service', '90', '90', 0, '" + Server.UrlEncode(Request.Url.AbsolutePath) + "'); }";

            moveToProductionUrl = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/uGovernITConfiguration.aspx?control=movestagetoproduction&list={0}", DatabaseObjects.Tables.Services));

            if (serviceCategory.ToLower() == Constants.ModuleAgent.ToLower())
            {
                btCategories.Visible = false;
                btCopyLink.Visible = false;
                btSendSurvey.Visible = false;
                rdbServiceCategory.Items[2].Attributes.CssStyle.Add("visibility", "hidden");
            }
            else if (serviceCategory.ToLower() == Constants.ModuleFeedback.ToLower())
            {
                btSendSurvey.Visible = true;
                chkShowDeleted.Visible = false;
                btCategories.Visible = false;
                btCopyLink.Visible = false;
                btnDuplicate.Visible = false;
                btnMigrate.Visible = false;
                rdbServiceCategory.Visible = false;
                //ServiceGrid.UnGroup(ServiceGrid.Columns["CategoryId"]);
                ServiceGrid.UnGroup(ServiceGrid.Columns["ServiceCategoryType"]);
                ServiceGrid.Columns["ModuleName"].Visible = true;
            }
            else
            {
                btCategories.Visible = true;
                btCopyLink.Visible = true;
                btSendSurvey.Visible = false;
                rdbServiceCategory.Items[2].Attributes.CssStyle.Add("visibility", "hidden");
            }
        }

        private void EnableMigrate()
        {
            if (configVariableHelper.GetValueAsBool(ConfigConstants.EnableMigrate))
            {
                if (serviceTypeCookie.ToLower() == Constants.ModuleFeedback.ToLower())
                {
                    lnkArchive.Visible = false;
                    lnkUnArchive.Visible = false;
                    chkShowDeleted.Checked = false;
                    btnMigrate.Visible = false;
                }
                else
                {
                    btnMigrate.Visible = true;
                }
                //btnMigrate.Visible = true; //SPDelta 155(Commented:-Survey complete functionality)
                //SPDelta 155(End:-Survey complete functionality)
            }
        }

        protected void btExport_Click(object sender, EventArgs e)
        {
            string selectedIds = string.Empty;
            List<int> lstServiceIds = new List<int>();

            for (int i = 0; i < ServiceGrid.VisibleRowCount; i++)
            {
                ASPxCheckBox chkBx = ServiceGrid.FindRowCellTemplateControl(i, null, "chkIsExport") as ASPxCheckBox;
                if (chkBx != null && chkBx.Checked)
                {
                    selectedIds = chkBx.Attributes["ServiceId"];
                    lstServiceIds.Add(UGITUtility.StringToInt(selectedIds));
                }
            }

            if (lstServiceIds == null || lstServiceIds.Count <= 0)
                return;

            DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");

            if (lstServiceIds.Count == 1)
            {
                Services service = serviceManager.LoadByServiceID(lstServiceIds[0]);
                ServiceExportHelper serviceExportHelper = new ServiceExportHelper(context);
                ServiceExtension serviceExtension= serviceExportHelper.GetServiceExtension(service, dtApplications);
                //GetServiceExtension(service, dtApplications);
                //if (serviceExtension.AuthorizedToView != "" && serviceExtension.AuthorizedToView != null)
                //{
                //    List<UserProfile> lstAuthtoView = HttpContext.Current.GetUserManager().GetUserInfosById(serviceExtension.AuthorizedToView);
                //    string authtoView = string.Empty;
                //    if (lstAuthtoView.Count > 0)
                //    {
                //        foreach (UserProfile item in lstAuthtoView)
                //        {
                //            authtoView += Constants.Separator6 + item.UserName;
                //        }
                //        authtoView = authtoView.Remove(0, 1);
                //    }
                //    serviceExtension.AuthorizedToView = authtoView;
                //}
                //if (serviceExtension.OwnerUser != "" && serviceExtension.OwnerUser != null)
                //{

                //    List<UserProfile> lstOwner = HttpContext.Current.GetUserManager().GetUserInfosById(serviceExtension.OwnerUser);
                //    string owner = string.Empty;
                //    if (lstOwner.Count > 0)
                //    {
                //        foreach (UserProfile item in lstOwner)
                //        {
                //            owner += "," + item.UserName;
                //        }
                //        owner = owner.Remove(0, 1);
                //    }
                //    serviceExtension.OwnerUser = owner;
                //}
                string serviceNameForFile = UGITUtility.ReplaceInvalidCharsInFolderName(serviceExtension.Title);
                string folderPath = uHelper.GetDefaultServicesPath();
                bool folderExists = System.IO.Directory.Exists(folderPath);
                if (!folderExists)
                    System.IO.Directory.CreateDirectory(folderPath);
                string filePath = System.IO.Path.Combine(uHelper.GetDefaultServicesPath(), string.Format("{0}{1}.xml", serviceNameForFile, DateTime.Now.ToString("yyyyMMdd")));
                using (FileStream writer = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(ServiceExtension));
                    ser.WriteObject(writer, serviceExtension);
                }

                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.ContentType = "application/xml";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + string.Format("{0}.xml", serviceNameForFile));
                Response.TransmitFile(filePath);
                Response.Flush();
                Response.End();
            }
            else if (lstServiceIds.Count > 1)
            {
                string zipfileName = string.Format("Services{0}.zip", DateTime.Now.ToString("yyyyMMdd"));
                string folderPath = uHelper.GetTempFolderPath();
                bool folderExists = System.IO.Directory.Exists(folderPath);
                if (!folderExists)
                    System.IO.Directory.CreateDirectory(folderPath);
                string ZipFolderName = System.IO.Path.Combine(uHelper.GetUploadFolderPath(), string.Format("Services{0}", DateTime.Now.ToString("yyyyMMdd")));

                DeleteFolder(ZipFolderName);
                System.IO.Directory.CreateDirectory(ZipFolderName);
                try
                {
                    using (FileStream zipToOpen = new FileStream(string.Format("{0}.zip", ZipFolderName), FileMode.Create))
                    {
                        using (ZipArchive zip = new ZipArchive(zipToOpen, ZipArchiveMode.Create, true))
                        {


                            foreach (int id in lstServiceIds)
                            {
                                Services service = serviceManager.LoadByServiceID(id);
                                ServiceExportHelper exportHelper = new ServiceExportHelper(context);
                                ServiceExtension serviceExtension = exportHelper.GetServiceExtension(service, dtApplications);
                                //if (serviceExtension.AuthorizedToView != "" && serviceExtension.AuthorizedToView != null)
                                //{
                                //    //serviceExtension.AuthorizedToView = HttpContext.Current.GetUserManager().GetUserById(serviceExtension.AuthorizedToView).UserName;
                                //    List<UserProfile> lstAuthtoView = HttpContext.Current.GetUserManager().GetUserInfosById(serviceExtension.AuthorizedToView);
                                //    string authtoView = string.Empty;
                                //    if (lstAuthtoView.Count > 0)
                                //    {
                                //        foreach (UserProfile item in lstAuthtoView)
                                //        {
                                //            authtoView += Constants.Separator6 + item.UserName;
                                //        }
                                //        authtoView = authtoView.Remove(0, 1);
                                //    }
                                //    serviceExtension.AuthorizedToView = authtoView;
                                //}
                                //if (serviceExtension.OwnerUser != "" && serviceExtension.OwnerUser != null)
                                //{
                                //    //serviceExtension.Owner = HttpContext.Current.GetUserManager().GetUserById(serviceExtension.Owner).UserName;
                                //    List<UserProfile> lstOwner = HttpContext.Current.GetUserManager().GetUserInfosById(serviceExtension.OwnerUser);
                                //    string owner = string.Empty;
                                //    if (lstOwner.Count > 0)
                                //    {
                                //        foreach (UserProfile item in lstOwner)
                                //        {
                                //            owner += "," + item.UserName;
                                //        }
                                //        owner = owner.Remove(0, 1);
                                //    }
                                //    serviceExtension.OwnerUser = owner;
                                //}


                                string serviceNameForFile = UGITUtility.ReplaceInvalidCharsInFolderName(serviceExtension.Title);
                                string fileName = string.Format(@"{0}\\{1}.xml", ZipFolderName, serviceExtension.Title);
                                using (FileStream writer = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
                                {
                                    DataContractSerializer ser = new DataContractSerializer(typeof(ServiceExtension));
                                    ser.WriteObject(writer, serviceExtension);

                                }

                                ZipArchiveEntry readmeEntry = zip.CreateEntry(serviceNameForFile + ".xml");
                                using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                                {
                                    writer.Write(File.ReadAllText(fileName));
                                }
                            }
                        }
                    }
                    //DeleteFolder(ZipFolderName);
                    FileStream fs = new FileStream(string.Format("{0}.zip", ZipFolderName), FileMode.Open);
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.ContentType = "application/zip";

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(zipfileName, System.Text.Encoding.UTF8));
                    Response.TransmitFile(string.Format("{0}.zip", ZipFolderName));
                    //Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();
                    File.Delete(string.Format("{0}.zip", ZipFolderName));
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "Error exporting selected services");
                }
            }
        }


        private ServiceExtension GetServiceExtension(Services service, DataTable dtApplications)
        {
            if (service.Questions != null && service.Questions.Count > 0)
                ReplaceIdWithToken(service, keyColl);

            string fieldColumnType = string.Empty;
            FieldConfigurationManager configFieldManager = new FieldConfigurationManager(context);
            List<string> lstApplicationIds = new List<string>();
            ServiceExtension serviceExtension = new ServiceExtension(service);
            List<ServiceQuestion> lstServiceQuestion = serviceExtension.Questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.ApplicationAccess).ToList();
            foreach (ServiceQuestion ques in lstServiceQuestion)
            {
                string sfOptions = string.Empty;
                ques.QuestionTypePropertiesDicObj.TryGetValue("application", out sfOptions);
                if (sfOptions != null && sfOptions.ToLower() != "all")
                {
                    List<string> sfOptionList = UGITUtility.ConvertStringToList(sfOptions, new string[] { Constants.Separator1 });
                    List<string> applications = new List<string>();
                    foreach (string item in sfOptionList)
                    {
                        string applicationid = Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[1];
                        DataRow[] drApps = dtApplications.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, applicationid));
                        if (drApps.Length > 0)
                        {
                            applications.Add(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[0] + Constants.Separator2 + Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[0] + "-" + Convert.ToString(drApps[0][DatabaseObjects.Columns.Id]) + ";" + Convert.ToString(drApps[0][DatabaseObjects.Columns.Title]));

                        }
                    }
                    ques.QuestionTypePropertiesDicObj["application"] = string.Join(Constants.Separator1, applications.ToArray());
                }
                else
                {
                    foreach (DataRow appItem in dtApplications.Rows)
                    {
                        lstApplicationIds.Add(Convert.ToString(appItem[DatabaseObjects.Columns.Id]));
                    }
                }
            }
            List<LookupValueServiceExtension> lstLookUp = new List<LookupValueServiceExtension>();
            // List<UserInfo> lstUserLookUp = new List<UserInfo>();
            List<UserProfile> lstUserLookUp = new List<UserProfile>();
            foreach (UGITTask item in serviceExtension.Tasks)
            {
                List<PropertyInfo> properties = item.GetType().GetProperties().Where(c => c.PropertyType == typeof(List<string>) || c.PropertyType == typeof(string)).ToList();
                foreach (PropertyInfo prop in properties)
                {
                    var value = item.GetType().GetProperty(prop.Name).GetValue(item, null);
                    var valueType = prop.Name;
                    FieldConfiguration configField = configFieldManager.Get(valueType);
                    fieldColumnType = string.Empty;
                    if (configField != null)
                    {
                        fieldColumnType = Convert.ToString(configField.Datatype);
                    }
                    switch (fieldColumnType)
                    {
                        case "Lookup":
                            CreatelookupValuesList(ref lstLookUp, prop, value);
                            break;
                        case "UserType":
                            CreateUserLookupList(ref lstUserLookUp, prop, value);
                            break;
                    }
                }
                //List<PropertyInfo> propertiesUser = item.GetType().GetProperties().Where(c => c.PropertyType == typeof(List<string>) || c.PropertyType == typeof(string)).ToList();
                //foreach (PropertyInfo prop in propertiesUser)
                //{
                //    var value = item.GetType().GetProperty(prop.Name).GetValue(item, null);
                //    var valueType = prop.Name;
                //    FieldConfiguration configField = configFieldManager.GetFieldConfigurationData().Where(x => x.FieldName == Convert.ToString(valueType)).FirstOrDefault();
                //    fieldColumnType = string.Empty;
                //    if (configField != null)
                //    {
                //        fieldColumnType = Convert.ToString(configField.Datatype);
                //    }
                //    switch (fieldColumnType)
                //    {
                //        case "UserType":
                //            CreateUserLookupList(ref lstUserLookUp, prop, value);
                //            break;
                //    }

                //}

            }

            if (service.SkipTaskCondition != null && service.SkipTaskCondition.Count > 0)
            {
                foreach (ServiceTaskCondition taskCondition in service.SkipTaskCondition)
                {
                    if (taskCondition.Conditions != null && taskCondition.Conditions.Count > 0)
                    {
                        List<WhereExpression> lstWhereExpression = taskCondition.Conditions;
                        foreach (WhereExpression expression in lstWhereExpression)
                        {
                            ServiceQuestion question = service.Questions.Where(c => c.TokenName == expression.Variable).FirstOrDefault();
                            if (question == null)
                            {
                                Util.Log.ULog.WriteLog("ERROR: Missing question in skip logic with token [$" + expression.Variable + "$] for service: " + service.Title);
                                continue;
                            }

                            if (question.QuestionType.ToLower() == "userfield")
                            {
                                try
                                {
                                    string lookup = expression.Value;
                                    if (lookup != null)
                                        InsertUserValue(lookup, ref lstUserLookUp);
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error exporting " + expression.Variable);
                                }
                            }
                            if (question.QuestionType.ToLower() == "requestCategory")
                            {
                                try
                                {
                                    string lookupColl = expression.Value;
                                    if (lookupColl != null)
                                    {
                                        InsertLookupValue(lookupColl, ref lstLookUp, DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.Title);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error exporting " + expression.Variable);
                                }

                            }
                            if (question.QuestionType.ToLower() == "lookup")
                            {
                                try
                                {
                                    string lookupColl = expression.Value;
                                    if (lookupColl != null)
                                    {
                                        if (question.QuestionTypePropertiesDicObj != null && question.QuestionTypePropertiesDicObj.Count > 0)
                                        {
                                            string listName = question.QuestionTypePropertiesDicObj["lookuplist"];
                                            string fieldName = question.QuestionTypePropertiesDicObj["lookupfield"];
                                            InsertLookupValue(lookupColl, ref lstLookUp, listName, fieldName);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ULog.WriteException(ex, "Error exporting " + expression.Variable);
                                }
                            }
                        }

                    }
                }
            }

            CreateUserLookupList(ref lstUserLookUp, serviceExtension.GetType().GetProperty(DatabaseObjects.Columns.AuthorizedToView), serviceExtension.AuthorizedToView);
            CreateUserLookupList(ref lstUserLookUp, serviceExtension.GetType().GetProperty(DatabaseObjects.Columns.Owner), serviceExtension.OwnerUser);
            serviceExtension.LookupValues = lstLookUp;
            serviceExtension.UserInfo = lstUserLookUp;

            return serviceExtension;
        }
        private void DeleteFolder(string ZipFolderName)
        {
            bool isExists = System.IO.Directory.Exists(ZipFolderName);
            if (isExists)
            {
                System.IO.DirectoryInfo serviceFolder = new DirectoryInfo(ZipFolderName);
                foreach (System.IO.FileInfo file in serviceFolder.GetFiles())
                {
                    file.Delete();
                }
                serviceFolder.Delete();
            }
        }
        private void CreatelookupValuesList(ref List<LookupValueServiceExtension> lstLookUp, PropertyInfo item, object value)
        {
            if (value != null)
            {
                string itemName = item.Name;
                string listName = string.Empty;
                string fieldName = string.Empty;
                bool isUpdatelist = false;
                if (itemName.ToLower() == "requestcategory")
                {
                    listName = DatabaseObjects.Tables.RequestType;
                    fieldName = DatabaseObjects.Columns.Title;
                    isUpdatelist = true;
                }
                else if (itemName.ToLower() == "modulename")
                {
                    listName = DatabaseObjects.Tables.Modules;
                    fieldName = DatabaseObjects.Columns.ModuleName;
                    isUpdatelist = true;
                }
                else if (itemName.ToLower() == "predecessors")
                {
                    listName = DatabaseObjects.Tables.ServiceTicketRelationships;
                    fieldName = DatabaseObjects.Columns.Title;
                    isUpdatelist = true;
                }
                else if (itemName.ToLower() == "service")
                {
                    listName = DatabaseObjects.Tables.Services;
                    fieldName = DatabaseObjects.Columns.Title;
                    isUpdatelist = true;
                }
                if (isUpdatelist)
                {
                    if (item.PropertyType == typeof(List<string>))
                    {
                        try
                        {
                            // SPFieldLookupValueCollection lookups = new SPFieldLookupValueCollection(Convert.ToString(value));
                            List<string> lookups = UGITUtility.ConvertStringToList(Convert.ToString(value), Constants.Separator6);
                            foreach (string lookup in lookups)
                            {
                                if (!string.IsNullOrEmpty(lookup))
                                    InsertLookupValue(lookup, ref lstLookUp, listName, fieldName);
                            }
                        }
                        catch (Exception ex)
                        {
                            ULog.WriteException(ex, "Error exporting " + itemName);
                        }
                    }
                }
            }
        }
        private void InsertLookupValue(string lookup, ref List<LookupValueServiceExtension> lstLookUp, string listName, string fieldName)
        {
            var itemExist = lstLookUp.FirstOrDefault(c => c.ID == lookup);
            if (itemExist == null)
            {
                // DataRow spListItem = null;
                // DataTable spTable = GetTableDataManager.GetTableData(listName,string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleName, lookup));
                //if (spTable.Rows.Count > 0)
                //{
                // SPListItem spListItem = SPListHelper.GetSPListItem(listName, lookup.LookupId);
                // spListItem = spTable.Rows[0];
                //if (spListItem != null && !string.IsNullOrEmpty(Convert.ToString(spListItem[fieldName])))
                //{
                LookupValueServiceExtension lookupValue = new LookupValueServiceExtension(lookup, fieldName, listName);
                lstLookUp.Add(lookupValue);
                //}
                // }
            }
        }
        private void CreateUserLookupList(ref List<UserProfile> lstUserLookUp, PropertyInfo item, object value)
        {
            if (value != null && item != null)
            {
                if (item.PropertyType == typeof(string))
                {
                    //SPFieldUserValueCollection userValCollection = new SPFieldUserValueCollection(SPContext.Current.Web, item.GetValue());
                    List<UserProfile> users = HttpContext.Current.GetUserManager().GetUserInfosById(Convert.ToString(value));
                    //SPFieldUserValueCollection users = (SPFieldUserValueCollection)value;
                    foreach (UserProfile lookup in users)
                    {
                        if (lookup != null)
                            InsertUserValue(lookup.Id, ref lstUserLookUp);
                    }
                }
            }
        }
        private void InsertUserValue(string lookup, ref List<UserProfile> lstUserLookUp)
        {
            var itemExist = lstUserLookUp.FirstOrDefault(c => c.Id == lookup);
            if (itemExist == null)
            {
                UserProfile profile = HttpContext.Current.GetUserManager().GetUserById(lookup);
                if (profile != null)
                {
                    lstUserLookUp.Add(profile);
                }
                //UserProfile group = UserProfile.GetGroupByID(lookup.LookupId);
                //if (group != null)
                //{
                //    UserInfo lookupValue = new UserInfo(lookup.LookupId, lookup.LookupValue, true);
                //    lstUserLookUp.Add(lookupValue);
                //}
            }
        }

        protected void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            //ReloadPage();
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("@TenantID", context.TenantID);
            DataTable dt = GetTableDataManager.GetData(DatabaseObjects.Tables.Services, values);
            DataView view = dt.DefaultView;
            view.RowFilter = string.Format("1 = 1");



            if (serviceTypeCookie.ToLower() == Constants.ModuleAgent.ToLower())
            {
                view.RowFilter += string.Format(" AND {0} = '{1}'", DatabaseObjects.Columns.ServiceType, "ModuleAgent");
                if (!chkShowDeleted.Checked)
                    view.RowFilter += string.Format(" AND {0} = '{1}'", DatabaseObjects.Columns.Deleted, false);
            }
            else if (serviceTypeCookie.ToLower() == Constants.ModuleFeedback.ToLower())
                view.RowFilter += string.Format(" AND {0} = '{1}'", DatabaseObjects.Columns.ServiceType, "ModuleFeedback");
            else
            {
                if (chkShowDeleted.Checked)
                    view.RowFilter += string.Format(" AND {0} <> '{1}' AND {0} <> '{2}'",
                        DatabaseObjects.Columns.ServiceType, "ModuleAgent", "ModuleFeedback");
                else
                    view.RowFilter += string.Format(" AND {0} <> '{1}' AND {0} <> '{2}' AND {3} = '{4}'",
                    DatabaseObjects.Columns.ServiceType, "ModuleAgent", "ModuleFeedback", DatabaseObjects.Columns.Deleted, false);
            }


            dt = view.ToTable();
            if (dt.Rows.Count > 0)
            {
                if (!dt.Columns.Contains("serviceUrl"))
                {
                    dt.Columns.Add("serviceUrl", typeof(string));
                    dt.AcceptChanges();
                }
                foreach (DataRow dr in dt.Rows)
                {
                    dr["serviceUrl"] = string.Format("{0}{1}", delegateUrl, dr["ID"]);
                }

            }
            ServiceGrid.DataSource = dt;
            ServiceGrid.DataBind();
        }

        private void ReloadPage()
        {
            string listUrl = UGITUtility.GetAbsoluteURL(Request.Path);
            queryCollection = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            //SPDelta 155(Start:-Survey complete functionality)
            if (serviceTypeCookie.ToLower() != Constants.ModuleFeedback.ToLower())
            {
                queryCollection.Set("showarchive", chkShowDeleted.Checked.ToString());
            }
            //queryCollection.Set("showarchive", chkShowDeleted.Checked.ToString());
            //SPDelta 155(End:-Survey complete functionality)
            listUrl = string.Format("{0}?{1}", listUrl, queryCollection.ToString());
            Context.Response.Redirect(listUrl);
        }

        #region Method to Create Duplicate Service
        /// <summary>
        /// This Method is used to create Duplicate Service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDuplicate_Click(object sender, EventArgs e)
        {
            int selectedId = 0;

            for (int i = 0; i < ServiceGrid.VisibleRowCount; i++)
            {
                ASPxCheckBox chkBx = ServiceGrid.FindRowCellTemplateControl(i, null, "chkIsExport") as ASPxCheckBox;
                if (chkBx != null && chkBx.Checked)
                {
                    selectedId = UGITUtility.StringToInt(chkBx.Attributes["ServiceId"]);
                }
            }

            if (selectedId == 0)
                return;

            Services service = serviceManager.LoadByServiceID(selectedId);
            if (service == null)
                return;

            if (service.ServiceType == "ModuleAgent")
                lstServices = serviceManager.Load(x => x.ServiceType == "ModuleAgent");

            if (lstServices == null || lstServices.Count == 0)
                lstServices = serviceManager.LoadAllServices(serviceTypeCookie).OrderBy(x => x.CategoryItemOrder).OrderBy(x => x.ItemOrder).ToList();

            DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            ServiceExtension serviceExtension = GetServiceExtension(service, dtApplications);
            ServiceExtension servcieError = new ServiceExtension();
            serviceExtension.Title = ServicesManager.GetUpdatedServiceTitle(serviceExtension.Title, lstServices);
            serviceManager.SaveDuplicateService(serviceExtension, servcieError);
            BindServicesData(rdbServiceCategory.SelectedItem.Value);
        }
        #endregion Method to Create Duplicate Service

        protected void lnkFullDeleteService_Click(object sender, EventArgs e)
        {
            List<int> selectedIds = ValidationCheck();

            if (selectedIds.Count == 0)
            {
                lblMessage.Visible = true;
                return;
            }

            foreach (int sID in selectedIds)
            {
                serviceManager.DeleteAll(sID);
            }

            ReloadPage();
        }

        protected void lnkArchive_Click(object sender, EventArgs e)
        {
            List<int> selectedIds = ValidationCheck();
            if (selectedIds.Count == 0)
            {
                lblMessage.Visible = true;
                return;
            }
            foreach (int sID in selectedIds)
            {
                serviceManager.Archive(sID);
            }
            ReloadPage();
        }

        protected void lnkUnArchive_Click(object sender, EventArgs e)
        {
            List<int> selectedIds = ValidationCheck();

            if (selectedIds.Count == 0)
            {
                lblMessage.Visible = true;
                return;
            }

            foreach (int sID in selectedIds)
            {
                serviceManager.UnArchive(sID);
            }

            ReloadPage();
        }

        private List<int> ValidationCheck()
        {
            string selectedIds = string.Empty;
            List<int> lstServiceIds = new List<int>();
            for (int i = 0; i < ServiceGrid.VisibleRowCount; i++)
            {
                ASPxCheckBox chkBx = ServiceGrid.FindRowCellTemplateControl(i, null, "chkIsExport") as ASPxCheckBox;
                if (chkBx != null && chkBx.Checked)
                {
                    selectedIds = chkBx.Attributes["ServiceId"];
                    lstServiceIds.Add(Convert.ToInt32(selectedIds));
                }
            }

            return lstServiceIds;
        }

        protected void rdbServiceCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            UGITUtility.DeleteCookie(Request, Response, "servicetype");
            UGITUtility.CreateCookie(Response, "servicetype", rdbServiceCategory.SelectedItem.Value);
            if (rdbServiceCategory.SelectedItem.Value.ToLower() == Constants.ModuleAgent.ToLower())
                PageTitle = "Edit Agent";
            if (rdbServiceCategory.SelectedItem.Value.ToLower() == Constants.ModuleService.ToLower())
                PageTitle = "Edit Service";
            if (rdbServiceCategory.SelectedItem.Value.ToLower() == Constants.ModuleFeedback.ToLower())
                PageTitle = "Edit Survey";
            chkShowDeleted.Checked = false;
            BindServicesData(rdbServiceCategory.SelectedItem.Value);
        }

        protected void btnMigrate_Click(object sender, EventArgs e)
        {
            hdnServices.Value = string.Join(",", ValidationCheck());

            Page.ClientScript.RegisterStartupScript(this.GetType(), "migratescript", "MoveToProduction();", true);
        }

        protected void ServiceGrid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {

            if (e.RowType == DevExpress.Web.GridViewRowType.Data)
            {
                if (UGITUtility.StringToBoolean(e.GetValue(DatabaseObjects.Columns.Deleted)))
                {
                    e.Row.CssClass += " archived-row";
                }



                string[] vals = uHelper.GetMultiLookupValue(Convert.ToString(e.GetValue(DatabaseObjects.Columns.TicketOwner)));
                //if (vals != null && vals.Length > 0)
                //{
                //    lbOwner.Text = string.Join("; ", vals);
                //}
            }
        }
        protected void ServiceGrid_HeaderFilterFillItems(object sender, ASPxGridViewHeaderFilterEventArgs e)
        {

        }
        protected void ServiceGrid_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            /*
            if (e.Column.FieldName == "CategoryId")
            {
                //string val1 = uHelper.GetUserNameBasedOnId(HttpContext.Current.GetManagerContext(), UGITUtility.ObjectToString(e.Value1));
                //string val2 = uHelper.GetUserNameBasedOnId(HttpContext.Current.GetManagerContext(), UGITUtility.ObjectToString(e.Value2));
                object name1 = e.GetRow1Value(DatabaseObjects.Columns.Name);
                object name2 = e.GetRow2Value(DatabaseObjects.Columns.Name);
                e.Handled = true;
                e.Result = System.Collections.Comparer.Default.Compare(name1, name2);
            }
            */

            if (e.Column.FieldName == "ServiceCategoryType")
            {
                Int32 val1 = GetCategoryOrder(Convert.ToString(e.Value1));
                Int32 val2 = GetCategoryOrder(Convert.ToString(e.Value2));
                e.Handled = true;
                e.Result = System.Collections.Comparer.Default.Compare(val1, val2);
            }
        }

        private int GetCategoryOrder(string value)
        {
            var record = (ServiceGrid.DataSource as DataTable).AsEnumerable().Where(x => x.Field<string>("ServiceCategoryType") == value).FirstOrDefault();
            if (record != null)
                return UGITUtility.StringToInt(record["ServiceCategoryItemOrder"]);

            return 0;
        }

        protected void ServiceGrid_CustomGroupDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "CategoryId" && !string.IsNullOrEmpty(e.DisplayText))
            {
                long id = Convert.ToInt64(e.Value);
                ServiceCategory category = categoryManager.LoadByID(id);
                if (category != null)
                    e.DisplayText = category.CategoryName;
            }
        }

        protected void cbCheck_Load(object sender, EventArgs e)
        {
            ASPxCheckBox cb = sender as ASPxCheckBox;

            GridViewDataItemTemplateContainer container = cb.NamingContainer as GridViewDataItemTemplateContainer;
            //cb.ClientInstanceName = String.Format("cbCheck{0}", container.VisibleIndex);
            cb.Checked = ServiceGrid.Selection.IsRowSelected(container.VisibleIndex);

            cb.ClientSideEvents.CheckedChanged = String.Format("function (s, e) {{ ServiceGrid.SelectRowOnPage ({0}, s.GetChecked()); }}", container.VisibleIndex);
        }

        protected void btnPushServices_Click(object sender, EventArgs e)
        {
            List<int> selectedIds = ValidationCheck();

            if (selectedIds.Count == 0)
            {
                lblMessage.Visible = true;
                return;
            }

            DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");
            //foreach (string Servicefile in Directory.EnumerateFiles(Path.Combine(uHelper.GetDefaultServicesPath()), "*.xml"))
            //{
            //    File.Delete(Servicefile);
            //}
            foreach (int sID in selectedIds)
            {


                Services service = serviceManager.LoadByServiceID(sID);
                ServiceExtension serviceExtension = GetServiceExtension(service, dtApplications);
                if (serviceExtension.AuthorizedToView != "" && serviceExtension.AuthorizedToView != null)
                {
                    List<UserProfile> lstAuthtoView = HttpContext.Current.GetUserManager().GetUserInfosById(serviceExtension.AuthorizedToView);
                    string authtoView = string.Empty;
                    if (lstAuthtoView.Count > 0)
                    {
                        foreach (UserProfile item in lstAuthtoView)
                        {
                            authtoView += Constants.Separator6 + item.UserName;
                        }
                        authtoView = authtoView.Remove(0, 1);
                    }
                    serviceExtension.AuthorizedToView = authtoView;
                }
                if (serviceExtension.OwnerUser != "" && serviceExtension.OwnerUser != null)
                {

                    List<UserProfile> lstOwner = HttpContext.Current.GetUserManager().GetUserInfosById(serviceExtension.OwnerUser);
                    string owner = string.Empty;
                    if (lstOwner.Count > 0)
                    {
                        foreach (UserProfile item in lstOwner)
                        {
                            owner += "," + item.UserName;
                        }
                        owner = owner.Remove(0, 1);
                    }
                    serviceExtension.OwnerUser = owner;
                }
                string serviceNameForFile = UGITUtility.ReplaceInvalidCharsInFolderName(serviceExtension.Title);
                string folderPath = uHelper.GetDefaultServicesPath();
                bool folderExists = System.IO.Directory.Exists(folderPath);
                if (!folderExists)
                    System.IO.Directory.CreateDirectory(folderPath);
                string filePath = System.IO.Path.Combine(uHelper.GetDefaultServicesPath(), string.Format("{0}.xml", serviceNameForFile));


                using (FileStream writer = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(ServiceExtension));
                    ser.WriteObject(writer, serviceExtension);

                }

                string ServiceInfo = "";
                DataContractSerializer serializer = new DataContractSerializer(typeof(ServiceExtension));
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter writer = new XmlTextWriter(sw))
                    {
                        // add formatting so the XML is easy to read in the log
                        writer.Formatting = Formatting.Indented;
                        serializer.WriteObject(writer, serviceExtension);
                        writer.Flush();
                        ServiceInfo = sw.ToString();
                    }

                }


                //Response.Clear();
                //Response.ClearHeaders();
                //Response.ClearContent();
                //Response.ContentType = "application/xml";
                //Response.End();
                //Update all 

                string Query = $"Update ServiceUpdates_Master Set AvailableForUpdate = 0;";
                var success = GetTableDataManager.ExecuteNonQuery(Query);

                ServiceUpdates_Master serviceUpdates_Master = new ServiceUpdates_Master();
                UpdateServicesMasterManager updateServicesMasterManager = new UpdateServicesMasterManager(context);
                serviceUpdates_Master.Title = serviceExtension.Title;
                serviceUpdates_Master.ServiceId = serviceExtension.ID;
                serviceUpdates_Master.ServiceInfo = ServiceInfo;
                //serviceUpdates_Master.Version = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + "";
                serviceUpdates_Master.Version = DateTime.Now.ToString("yyyy/mm/dd");
                serviceUpdates_Master.ServiceType = serviceExtension.ServiceType;
                serviceUpdates_Master.Description = serviceExtension.ServiceDescription;
                serviceUpdates_Master.AvailableForUpdate = true;
                updateServicesMasterManager.Insert(serviceUpdates_Master);
            }

            ImportServiceHelper importServiceHelper = new ImportServiceHelper(context);
            importServiceHelper.UpdateServices();
            lblPushMessage.Visible = true;
        }
        private void ReplaceIdWithToken(Services service, List<string> keyColl)
        {
            foreach (string key in keyColl)
            {
                service.Questions.Where(x => x.QuestionTypePropertiesDicObj.ContainsKey(key)).ToList().ForEach(y =>
                {
                    Dictionary<string, string> dic = y.QuestionTypePropertiesDicObj;
                    if (dic != null)
                    {
                        string val = dic[key];
                        int valId = 0;
                        int.TryParse(val, out valId);
                        if (valId > 0)
                        {
                            ServiceQuestion ques = service.Questions.Where(z => z.ID == valId).FirstOrDefault();
                            if (ques != null)
                                y.QuestionTypePropertiesDicObj[key] = ques.TokenName.ToLower();
                        }
                    }
                });
            }
        }
    }
}

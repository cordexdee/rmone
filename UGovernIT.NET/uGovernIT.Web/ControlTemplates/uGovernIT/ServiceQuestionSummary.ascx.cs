using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Util.Log;
using System.Text.RegularExpressions;
using uGovernIT.Manager.Managers;

namespace uGovernIT.Web
{
    public partial class ServiceQuestionSummary : UserControl
    {
        public int TicketId { get; set; }
        public string ListName { get; set; }
        public bool ReadOnly { get; set; }
        public string FrameId { get; set; }
        public string ModuleName { get; set; }
        public int summaryColWidth = 6;
        DataRow item = null;
        private ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        private Services service;
        private ServiceInput inputObj;
        ServiceQuestionManager ObjSQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
        ServiceSectionManager objSSectionManager = new ServiceSectionManager(HttpContext.Current.GetManagerContext());
        ServicesManager ObjServicesManager = new ServicesManager(HttpContext.Current.GetManagerContext());
        TicketManager objTicketManager = new TicketManager(HttpContext.Current.GetManagerContext());
        TicketManager ticketMgr = new TicketManager(HttpContext.Current.GetManagerContext());
        UGITModule module = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (TicketId > 0)
            {
                module = ObjModuleViewManager.LoadByName(ModuleName);
                bool flag = false;
                item = GetTableDataManager.GetTableData(module.ModuleTable, DatabaseObjects.Columns.ID + "=" + Convert.ToString(TicketId)).Rows[0];
                if (item != null)
                {
                    service = ObjServicesManager.LoadByID(UGITUtility.StringToLong(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.ServiceTitleLookup])));
                    if (service != null)
                    {
                        service.Sections = objSSectionManager.Load(x => x.ServiceID == service.ID);
                        service.Questions = ObjSQuestionManager.Load(x => x.ServiceID == service.ID);
                        string questionInputs = Convert.ToString(UGITUtility.GetSPItemValue(item, DatabaseObjects.Columns.UserQuestionSummary));
                        if(questionInputs.Contains("ServiceSummary"))
                        {
                            questionInputs = questionInputs.Replace("ServiceSummary", "Service");
                            flag = true;
                        }
                        if (!string.IsNullOrEmpty(questionInputs))
                        {
                            XmlDocument doc = new XmlDocument();
                            try
                            {
                                doc.LoadXml(questionInputs.Trim());
                                inputObj = (ServiceInput)uHelper.DeSerializeAnObject(doc, new ServiceInput());
                                if (flag || inputObj.ServiceID != service.ID)
                                {
                                    inputObj.ServiceID = service.ID;
                                    int i = 0;
                                    foreach (var val in inputObj.ServiceSections)
                                    {
                                        if (val.SectionID != service.Sections[i].ID)
                                        {
                                            val.SectionID = service.Sections[i].ID;
                                        }
                                        i++;
                                    }
                                }
                                GenerateSummary();
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex, "Invalid User Question Summary XML");
                            }
                        }
                    }
                }
            }
        }

        private void GenerateSummary()
        {
            if (service != null && inputObj != null && inputObj.ServiceSections != null)
            {
                inputObj.ServiceSections = inputObj.ServiceSections.Where(x => x.SectionID > 0 && x.Questions != null && x.Questions.Count > 0).ToList();
                rSummaryTable.DataSource = inputObj.ServiceSections.Where(x => x.IsSkiped == false);
                rSummaryTable.DataBind();
                summaryColWidth = inputObj.ServiceSections.Where(x => x.IsSkiped == false).Count() <= 1 ? 12 : 6;
            }
            else
            {
                Label lblmsg = new Label();
                lblmsg.Text = "No question input summary";
                rSummaryTable.Controls.Add(lblmsg);
            }
        }

        protected void RSummaryTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ServiceSectionInput sectionInput = (ServiceSectionInput)e.Item.DataItem;
            ServiceSection section = service.Sections.FirstOrDefault(x => x.ID == sectionInput.SectionID);

            if (sectionInput != null && section != null)
            {
                Label lbSection = (Label)e.Item.FindControl("lbSection");
                lbSection.Text = section.Title;
                Repeater rSummaryioQuest = (Repeater)e.Item.FindControl("rSummaryioQuest");
                rSummaryioQuest.DataSource = sectionInput.Questions.Where(x => x.IsSkiped == false).ToList();
                rSummaryioQuest.DataBind();
            }

        }

        protected void RSummaryioQuest_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Repeater rSummaryioQuest = (Repeater)sender;
            RepeaterItem sectionItem = (RepeaterItem)rSummaryioQuest.Parent;
            ServiceSectionInput sectionInput = (ServiceSectionInput)sectionItem.DataItem;
            ServiceQuestionInput questionInput = (ServiceQuestionInput)e.Item.DataItem;
            if (sectionInput == null || questionInput == null)
                return;

            ServiceQuestion question = service.Questions.FirstOrDefault(x => x.ServiceSectionID == sectionInput.SectionID && x.TokenName == questionInput.Token);
            if(question==null)
                question = service.Questions.FirstOrDefault(x => x.ServiceSectionID == sectionInput.SectionID || x.ServiceID==service.ID && x.TokenName == questionInput.Token);
            if (question != null)
                question.QuestionTypePropertiesDicObj = UGITUtility.DeserializeDicObjects(question.QuestionTypeProperties);
            if (question != null)
            {
                HtmlGenericControl summaryQuestionDiv = (HtmlGenericControl)e.Item.FindControl("summaryQuestionDiv");
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
                string txtMode = string.Empty;
                question.QuestionTypePropertiesDicObj.TryGetValue("textmode", out txtMode);
                if (txtMode != null)
                    if (txtMode.ToLower() == "multiline")
                    {
                        summaryQuestionDiv.Attributes.Add("class", "col-md-12 col-sm-12 question-wrap");
                    }

                if (question.QuestionType.ToLower() == "userfield")
                {
                    string UserName = HttpContext.Current.GetUserManager().CommaSeparatedNamesFrom(questionInput.Value);
                    if (string.IsNullOrEmpty(UserName))
                    {
                        UserName = UGITUtility.ConvertStringToList(Convert.ToString(questionInput.Value), "\\").LastOrDefault();
                        UserProfileManager UserManager = new  UserProfileManager(HttpContext.Current.GetManagerContext());
                        UserName = UserManager.GetUserInfoByIdOrName(UserName).Name;
                    }
                    lbQuestionVal.Text = UserName;
                }
                else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.MULTICHOICE && !string.IsNullOrEmpty(questionInput.Value))
                {
                    //show each selected option in new line
                    lbQuestionVal.Text = Regex.Replace(questionInput.Value, string.Format("[{0}{1}]", Constants.Separator2, Constants.Separator5), Constants.BreakLineSeparator);
                }
                else if (question.QuestionType.ToLower() == "datetime" && !string.IsNullOrEmpty(questionInput.Value))
                {
                    DateTime date = UGITUtility.StringToDateTime(questionInput.Value);

                    string dateMode = string.Empty;
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
                else if ((question.QuestionType.ToLower() == "lookup" || question.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets.ToLower()) && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                    string lookup = null;
                    string fieldName = string.Empty;
                    FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());

                    if (question.QuestionTypePropertiesDicObj.Keys.Contains("lookuplist"))
                        fieldName = question.QuestionTypePropertiesDicObj["lookuplist"];
                    else if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.Assets.ToLower())
                        fieldName = DatabaseObjects.Columns.AssetLookup;

                    try
                    {
                        if ((questionInput.Value).Contains(Constants.Separator))
                        {
                            questionInput.Value = (questionInput.Value).Split('#')[1];
                            lookup = fieldConfigurationManager.GetFieldConfigurationData(fieldName, questionInput.Value);
                        }
                        else
                        {
                            lookup = fieldConfigurationManager.GetFieldConfigurationData(fieldName, questionInput.Value);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }
                    if (!string.IsNullOrEmpty(lookup))
                    {

                        lbQuestionVal.Text = lookup.Replace(Constants.Separator6, Constants.BreakLineSeparator);

                    }
                }
                else if (question.QuestionType.ToLower() == "requesttype" && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                    string lookup = null;
                    string fieldName = string.Empty;
                    FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                    IModuleViewManager moduleManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                    if (question.QuestionTypePropertiesDicObj.Keys.Contains("lookuplist"))
                        fieldName = question.QuestionTypePropertiesDicObj["lookuplist"];
                    try
                    {
                        lookup = fieldConfigurationManager.GetFieldConfigurationData(fieldName, questionInput.Value);// new SPFieldLookupValue(questionInput.Value);
                    }
                    catch (Exception ex)
                    {
                        ULog.WriteException(ex);
                    }

                    if (!string.IsNullOrEmpty(lookup))
                    {
                        lbQuestionVal.Text = lookup;
                        string moduleName = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("module", out moduleName);

                        UGITModule module = moduleManager.LoadByName(moduleName);
                        ModuleRequestType requestType = module.List_RequestTypes.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(questionInput.Value));
                        if (requestType != null)
                        {
                            List<string> rName = new List<string>();
                            if (!string.IsNullOrWhiteSpace(requestType.Category))
                                rName.Add(requestType.Category);
                            if (!string.IsNullOrWhiteSpace(requestType.SubCategory))
                                rName.Add(requestType.SubCategory);

                            rName.Add(requestType.RequestType);
                            lbQuestionVal.Text = string.Join(" > ", rName.ToArray());
                        }
                    }
                }
                else if (question.QuestionType.ToLower() == "applicationaccessrequest" && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                    FormatApplicationRoleModules(questionInput.Value, lbQuestionVal);
                }
                else if (question.QuestionType.ToLower() == "removeuseraccess" && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                    //uHelper.FormatRemoveUserAccess(questionInput.Value, lbQuestionVal);
                }
                else if (question.QuestionType.ToLower() == "checkbox" && !string.IsNullOrEmpty(questionInput.Value) && questionInput.Value.Trim() != string.Empty)
                {
                    lbQuestionVal.Text = "No";
                    if (UGITUtility.StringToBoolean(questionInput.Value))
                        lbQuestionVal.Text = "Yes";
                }
            }
        }

        public void FormatApplicationRoleModules(string value, Label lblControl)
        {
            DataTable allApplications = new DataTable();

            allApplications = objTicketManager.GetAllTickets(ObjModuleViewManager.GetByName("APP"));
            //DataRow app = null;
            //if (applicationRows.Length > 0)
            //{
            //    app = applicationRows[0];
            //    applicationData.Rows.Add(app.ItemArray);
            //    Applications.Add(UGITUtility.StringToInt(app[DatabaseObjects.Columns.Id]));
            //}
            
            List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
 
            XmlDocument _doc = new XmlDocument();
            _doc.LoadXml(value);
            serviceMatrixDataList = (List<ServiceMatrixData>)uHelper.DeSerializeAnObject(_doc, serviceMatrixDataList);
            foreach (ServiceMatrixData serviceMatrixData in serviceMatrixDataList)
            {
                DataRow[] applicationRows = allApplications.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.Title, serviceMatrixData.Name));
                //DataTable dt = applicationRows.CopyToDataTable();
                serviceMatrixData.ID = UGITUtility.ObjectToString(applicationRows.CopyToDataTable().Rows[0][DatabaseObjects.Columns.ID]);
                if (serviceMatrixData.lstGridData.Count > 0)
                {
                    foreach (var item in serviceMatrixData.lstGridData)
                    {
                        item.ID = serviceMatrixData.ID;
                    }
                    
                }
            }
           string val=  uHelper.SerializeObject(serviceMatrixDataList).OuterXml;
            //item[DatabaseObjects.Columns.UserQuestionSummary]
            //int rowEffected = ticketMgr.Save(module, targetItem);
            FormatApplicationRoleModules(val, lblControl, false);
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
    }
}

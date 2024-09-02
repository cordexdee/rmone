using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility.Entities;
using uGovernIT.Util.Log;
using System.Text.RegularExpressions;

namespace uGovernIT.Manager
{
    public class ServiceQuestionManager : ManagerBase<ServiceQuestion>, IServiceQuestionManager
    {
        ModuleViewManager ObjModuleViewManager;
        public ServiceQuestionManager(ApplicationContext context) : base(context)
        {
            ObjModuleViewManager = new ModuleViewManager(context);
        }


        public ServiceQuestion GetByID(int ID)
        {
            ServiceQuestion serviceQuestion = new ServiceQuestion();
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(dbContext);
            serviceQuestion = serviceQuestionManager.GetByID(ID);
            return serviceQuestion;
        }

        public ServiceQuestion LoadObj(ServiceQuestion item)
        {

            if (item.ServiceSectionID !=null && item.ServiceSectionID > 0)
            {
                ServiceSectionManager manager = new ServiceSectionManager(dbContext);
                ServiceSection section = manager.GetByID(item.ServiceSectionID, item.ServiceID);
                if (section != null)      
                {                                                   
                    item.ServiceSectionName = section.SectionName;
                }
            }

            item.QuestionTypePropertiesDicObj = UGITUtility.GetCustomProperties(Convert.ToString(item.QuestionTypeProperties), Constants.Separator);

            return item;

        }

        public DataTable GetTableByServiceID(int serviceID, bool questionsToBeAskedToUser)
        {
            DataTable sQuestions = new DataTable();
            return sQuestions;
        }

        public List<ServiceQuestion> GetByServiceID(long serviceID, bool questionsToBeAskedToUser)
        {
            List<ServiceQuestion> questions = new List<ServiceQuestion>();
            questions = Load().Where(x => x.ServiceID == serviceID).ToList();
            foreach (ServiceQuestion item in questions)
            {
                LoadObj(item);
            }
           
            return questions;
        }

        public void Save(ServiceQuestion objServiceQuestion)
        {

            if (objServiceQuestion.ID <= 0)
            {
                if(objServiceQuestion.ServiceSectionID <= 0)
                     objServiceQuestion.ServiceSectionID = null;
                Insert(objServiceQuestion);
                
            }
            else
            {
                Update(objServiceQuestion);
            }


            if (objServiceQuestion.QuestionTypePropertiesDicObj != null && objServiceQuestion.QuestionTypePropertiesDicObj.Count > 0)
            {
                StringBuilder param = new StringBuilder();
                foreach (string key in objServiceQuestion.QuestionTypePropertiesDicObj.Keys)
                {
                    param.AppendFormat("{0}={1}{2}", key, objServiceQuestion.QuestionTypePropertiesDicObj[key], Constants.Separator);
                }
                objServiceQuestion.QuestionTypeProperties = param.ToString();
            }
            else
            {
                objServiceQuestion.QuestionTypeProperties = string.Empty;
            }

            Update(objServiceQuestion);
            //this.ID = item.ID;
        }

        public bool Delete(Services service)
        {
            return false;
        }


      

        public void SaveOrder(List<ServiceQuestion> questions)
        {
            if (questions.Count <= 0)
            {
                return;
            }

            foreach(ServiceQuestion item in questions)
            {
                Save(item);
            }

            //SPList questionList = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceQuestions);

            //string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
            //string updateMethodFormat = "<Method ID=\"{0}\">" +
            // "<SetList>{1}</SetList>" +
            // "<SetVar Name=\"Cmd\">Save</SetVar>" +
            // "<SetVar Name=\"ID\">{2}</SetVar>" +
            // "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ItemOrder\">{3}</SetVar>" +
            // "</Method>";

            //StringBuilder query = new StringBuilder();
            //foreach (ServiceQuestion question in questions)
            //{
            //    query.AppendFormat(updateMethodFormat, question.ID, questionList.ID, question.ID, question.ItemOrder);
            //}
            //string batch = string.Format(batchFormat, query.ToString());
            //string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
        }

        public void SaveSectionMapping(List<ServiceQuestion> questions)
        {
            if (questions.Count <= 0)
            {
                return;
            }

            //SPList questionList = SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceQuestions);

            //string batchFormat = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "<ows:Batch OnError=\"Return\">{0}</ows:Batch>";
            //string updateMethodFormat = "<Method ID=\"{0}\">" +
            // "<SetList>{1}</SetList>" +
            // "<SetVar Name=\"Cmd\">Save</SetVar>" +
            // "<SetVar Name=\"ID\">{2}</SetVar>" +
            // "<SetVar Name=\"urn:schemas-microsoft-com:office:office#ServiceSectionsTitleLookup\">{3}</SetVar>" +
            // "</Method>";

            //StringBuilder query = new StringBuilder();
            //foreach (ServiceQuestion question in questions)
            //{
            //    query.AppendFormat(updateMethodFormat, question.ID, questionList.ID, question.ID, question.ServiceSectionID);
            //}
            //string batch = string.Format(batchFormat, query.ToString());
            //string batchReturn = SPContext.Current.Web.ProcessBatchData(batch);
        }

        public bool IsDefaultTokenPicked(string value)
        {
            value = value.ToLower().Replace("[$", string.Empty).Replace("$]", string.Empty);
            bool isDefaultSelectedPicker = (value == "initiator" || value == "initiatormanager" ||
                                            value == "today" || value == "initiatordepartment" ||
                                            value == "initiatordepartmentmanager" || value == "initiatordivisionmanager" ||
                                            value == "initiatorlocation" || value == "serviceowner");

            return isDefaultSelectedPicker;
        }

        public Control GenerateMobileControlForQuestion(Page page, ServiceQuestion question, List<ServiceSectionCondition> skipConditions)
        {
            return new Control();
        }
            //public Control GenerateMobileControlForQuestion(Page page, ServiceQuestion question, List<ServiceSectionCondition> skipConditions)
            //{
            //    Panel ctrPanel = new Panel();

            //    ctrPanel.ID = string.Format("question_{0}_{1}_Panel", question.ServiceSectionID, question.ID);
            //    string questionType = question.QuestionType.ToLower();
            //    string defaultVal = string.Empty;
            //    string defaultMandatoryErrorMsg = "Please specify";

            //    TextBox tempBoxForValidation = new TextBox();
            //    tempBoxForValidation.Style.Add(HtmlTextWriterStyle.Display, "none");
            //    tempBoxForValidation.CssClass += string.Format(" hiddenCtr hiddenCtr_{0}", question.ID);
            //    tempBoxForValidation.ID = string.Format("txthiddenCtr_{0}", question.ID);
            //    if (question.FieldMandatory && skipConditions.Exists(x => x.ConditionValidate && x.SkipQuestionsID.Exists(y => y == question.ID)))
            //    {
            //        tempBoxForValidation.Text = "[$skip$]";
            //    }

            //    switch (questionType)
            //    {
            //        case Constants.ServiceQuestionType.TEXTBOX:
            //        case Constants.ServiceQuestionType.Number:
            //            {
            //                TextBox txtCtr = new TextBox();
            //                txtCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //                ctrPanel.Controls.Add(txtCtr);

            //                //txtCtr.CssClass = "textboxctr";
            //                txtCtr.Attributes.Add("ctype", "textbox");
            //                txtCtr.Attributes.Add("placeholder", question.Helptext);
            //                string txtMode = string.Empty;
            //                question.QuestionTypeProperties.TryGetValue("textmode", out txtMode);
            //                defaultVal = string.Empty;
            //                question.QuestionTypeProperties.TryGetValue("defaultval", out defaultVal);

            //                string maxlength = string.Empty;
            //                question.QuestionTypeProperties.TryGetValue("maxlength", out maxlength);
            //                if (!string.IsNullOrEmpty(maxlength) && maxlength != "0")
            //                {
            //                    txtCtr.MaxLength = Convert.ToInt16(maxlength);
            //                }

            //                if (txtMode != null)
            //                {
            //                    if (txtMode.ToLower() == "multiline")
            //                    {
            //                        txtCtr.TextMode = TextBoxMode.MultiLine;
            //                        txtCtr.Rows = 5;
            //                    }

            //                    if (question.FieldMandatory)
            //                    {
            //                        txtCtr.Attributes.Add("required", "required");
            //                    }
            //                }

            //                txtCtr.Width = new Unit("96%");
            //                txtCtr.Text = defaultVal;
            //                txtCtr.Attributes.Add("onkeyup", string.Format("questionBrachLogic(this,'{0}', 'textbox', \"{1}\")", questionType, question.TokenName));
            //            }
            //            break;
            //        case Constants.ServiceQuestionType.SINGLECHOICE:
            //            List<ListItem> items = new List<ListItem>();
            //            string optionType = "dropdownctr";
            //            question.QuestionTypeProperties.TryGetValue("optiontype", out optionType);
            //            string options = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("options", out options);

            //            string optionPickFromField = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("selectedfield", out optionPickFromField);

            //            string pickFromField = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("pickfromfield", out pickFromField);

            //            string SelectedList = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("selectedlist", out SelectedList);

            //            defaultVal = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("defaultval", out defaultVal);
            //            if (Convert.ToBoolean(pickFromField) == true)
            //            {
            //                //SPWeb oWebsite = SPContext.Current.Web;
            //                //SPList splist = oWebsite.Lists[SelectedList];
            //                //SPListItemCollection colItems = splist.Items;

            //                //SPFieldChoice choiceField = (SPFieldChoice)splist.Fields[optionPickFromField];
            //                //if (choiceField != null)
            //                //{
            //                //    foreach (string option in choiceField.Choices)
            //                //    {
            //                //        items.Add(new ListItem(option, option));
            //                //    }
            //                //}
            //            }
            //            else
            //            {
            //                if (options != null)
            //                {
            //                    List<string> optionList = UGITUtility.ConvertStringToList(options, new string[] { Constants.Separator1, Constants.Separator2 });
            //                    foreach (string option in optionList)
            //                    {
            //                        items.Add(new ListItem(option, option));
            //                    }
            //                }
            //            }

            //            if (defaultVal != null)
            //            {
            //                ListItem lItem = items.FirstOrDefault(x => x.Value == defaultVal);
            //                if (lItem != null)
            //                {
            //                    lItem.Selected = true;
            //                }
            //            }

            //            if (optionType.ToLower() == "radiobuttons")
            //            {
            //                foreach (ListItem item in items)
            //                {
            //                    item.Attributes.Add("onclick", string.Format("questionBrachLogic(this,'{0}','radiobuttonlist',\"{1}\")", questionType, question.TokenName));
            //                    item.Attributes.Add("onblur", string.Format("questionBrachLogic(this,'{0}','radiobuttonlist',\"{1}\")", questionType, question.TokenName));
            //                }

            //                RadioButtonList ddlCtr = new RadioButtonList();
            //                ddlCtr.CssClass = "radiodropdownctr";
            //                ddlCtr.Items.AddRange(items.ToArray());
            //                ddlCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //                ctrPanel.Controls.Add(ddlCtr);

            //                if (question.FieldMandatory)
            //                {
            //                    ddlCtr.Attributes.Add("required", "required");
            //                }
            //            }
            //            else
            //            {
            //                DropDownList ddlCtr = new DropDownList();
            //                ddlCtr.CssClass = "dropdownctr";
            //                ddlCtr.Attributes.Add("onchange", string.Format("questionBrachLogic(this,'{0}','dropdownlist',\"{1}\")", questionType, question.TokenName));
            //                ddlCtr.Items.AddRange(items.ToArray());
            //                ddlCtr.Items.Insert(0, new ListItem("(None)", string.Empty));
            //                ddlCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //                ctrPanel.Controls.Add(ddlCtr);

            //                if (question.FieldMandatory)
            //                {
            //                    ddlCtr.Attributes.Add("required", "required");
            //                }
            //            }

            //            break;
            //        case Constants.ServiceQuestionType.MULTICHOICE:
            //            List<ListItem> optList = new List<ListItem>();

            //            question.QuestionTypeProperties.TryGetValue("mOptionType", out optionType);
            //            string mOptions = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("options", out mOptions);
            //            defaultVal = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("defaultval", out defaultVal);
            //            if (mOptions != null)
            //            {
            //                List<string> optionList = UGITUtility.ConvertStringToList(mOptions, new string[] { Constants.Separator1, Constants.Separator2 });
            //                foreach (string option in optionList)
            //                {
            //                    ListItem lItem = new ListItem(option, option);
            //                    optList.Add(lItem);
            //                    lItem.Attributes.Add("onclick", string.Format("questionBrachLogic(this,'{0}','checkboxlist',\"{1}\")", questionType, question.TokenName));
            //                    lItem.Attributes.Add("onblur", string.Format("questionBrachLogic(this,'{0}','radiobuttonlist',\"{1}\")", questionType, question.TokenName));
            //                }
            //            }

            //            if (defaultVal != null)
            //            {
            //                ListItem lItem = optList.FirstOrDefault(x => x.Value == defaultVal);
            //                if (lItem != null)
            //                {
            //                    lItem.Selected = true;
            //                }
            //            }


            //            //Check checkbox list for multi choice 
            //            CheckBoxList ddlMCtr = new CheckBoxList();
            //            ddlMCtr.Attributes.Add("ctype", "multichoice");
            //            ddlMCtr.CssClass = "checkboxdropdownctr";
            //            ddlMCtr.Items.AddRange(optList.ToArray());
            //            ddlMCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //            ctrPanel.Controls.Add(ddlMCtr);

            //            if (question.FieldMandatory)
            //            {
            //                ddlMCtr.Attributes.Add("onclick", @";{var tempbox = $('#'+$(this).prop('id')+'_TempBox');    
            //                                                                 if($(this).find('input:checked').length > 0) tempbox.val('true');
            //                                                                 else tempbox.val('');};");
            //                ddlMCtr.Attributes.Add("required", "required");

            //                // TextBox tempBoxForValidation = new TextBox();
            //                tempBoxForValidation.Style.Add(HtmlTextWriterStyle.Display, "none");
            //                tempBoxForValidation.ID = string.Format("question_{0}_{1}_TempBox", question.ServiceSectionID, question.ID);
            //                ctrPanel.Controls.Add(tempBoxForValidation);

            //                tempBoxForValidation.Attributes.Add("required", "required");
            //            }

            //            break;
            //        case Constants.ServiceQuestionType.CHECKBOX:
            //            CheckBox checkboxCtr = new CheckBox();
            //            checkboxCtr.CssClass = "checkboxctr";
            //            checkboxCtr.Attributes.Add("ctype", "checkbox");
            //            defaultVal = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("defaultval", out defaultVal);
            //            checkboxCtr.Checked = UGITUtility.StringToBoolean(defaultVal);
            //            checkboxCtr.Attributes.Add("onclick", string.Format("questionBrachLogic(this,'{0}','checkbox',\"{1}\")", questionType, question.TokenName));
            //            checkboxCtr.Attributes.Add("onblur", string.Format("questionBrachLogic(this,'{0}','checkbox',\"{1}\")", questionType, question.TokenName));

            //            checkboxCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);

            //            ctrPanel.Controls.Add(checkboxCtr);
            //            break;
            //        case Constants.ServiceQuestionType.DATETIME:
            //            TextBox datetimeCtr = new TextBox();
            //            datetimeCtr.Attributes.Add("data-role", "date");


            //            //DateTime Questions - add validation for Present ,Past,Future  for mobile :Start
            //            string presentdate = string.Empty;
            //            string pastdate = string.Empty;
            //            string futuredate = string.Empty;

            //            string dateconstraint = string.Empty;
            //            string conditional = string.Empty;

            //            question.QuestionTypeProperties.TryGetValue("presentdate", out presentdate);
            //            question.QuestionTypeProperties.TryGetValue("pastdate", out pastdate);
            //            question.QuestionTypeProperties.TryGetValue("futuredate", out futuredate);
            //            question.QuestionTypeProperties.TryGetValue("dateconstraint", out dateconstraint);

            //            if (dateconstraint == "conditional")
            //            {
            //                if (!UGITUtility.StringToBoolean(pastdate) || !UGITUtility.StringToBoolean(futuredate))
            //                {
            //                    if (UGITUtility.StringToBoolean(pastdate))
            //                    {
            //                        if (Convert.ToBoolean(presentdate))
            //                        {
            //                            datetimeCtr.Attributes.Add("maxDate", DateTime.Now.ToString("MM-dd-yyyy"));
            //                        }
            //                        else
            //                        {
            //                            datetimeCtr.Attributes.Add("maxDate", DateTime.Now.AddDays(-1).ToString("MM-dd-yyyy"));
            //                        }
            //                    }
            //                    else if (UGITUtility.StringToBoolean(futuredate))
            //                    {
            //                        if (Convert.ToBoolean(presentdate))
            //                        {
            //                            datetimeCtr.Attributes.Add("minDate", DateTime.Now.ToString("MM-dd-yyyy"));
            //                        }
            //                        else
            //                        {
            //                            datetimeCtr.Attributes.Add("minDate", DateTime.Now.AddDays(1).ToString("MM-dd-yyyy"));
            //                        }
            //                    }
            //                    else
            //                    {
            //                        datetimeCtr.Attributes.Add("maxDate", DateTime.Now.ToString("MM-dd-yyyy"));
            //                        datetimeCtr.Attributes.Add("minDate", DateTime.Now.ToString("MM-dd-yyyy"));
            //                    }
            //                }
            //            }

            //            //DateTime Questions - add validation for Present ,Past,Future for mobile :End

            //            //Set default value 
            //            if (defaultVal != null)
            //            {
            //                defaultVal = defaultVal.ToLower();
            //                if (defaultVal.Contains("[today]"))
            //                {
            //                    defaultVal = defaultVal.Replace("[today]", string.Format("[{0}]", DateTime.UtcNow.ToString()));
            //                    datetimeCtr.Text = DateTime.UtcNow.ToString();
            //                }

            //                //Evaluate function 
            //                if (defaultVal.Contains("f:"))
            //                {
            //                    object tempDate = ExpressionCalc.ExecuteFunctions(defaultVal);
            //                    DateTime endDate = DateTime.MinValue;
            //                    DateTime.TryParse(Convert.ToString(tempDate), out endDate);
            //                    if (endDate != DateTime.MinValue)
            //                    {
            //                        datetimeCtr.Text = endDate.ToString();
            //                    }
            //                }
            //            }

            //            datetimeCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);

            //            if (question.FieldMandatory)
            //            {
            //                datetimeCtr.Attributes.Add("required", "required");
            //            }

            //            ctrPanel.Controls.Add(datetimeCtr);
            //            break;
            //        case Constants.ServiceQuestionType.USERFIELD:

            //            DropDownList ddlctrl = new DropDownList();
            //            //List<UserProfile> userList = uGITCache.UserProfileCache.GetEnabledUsers();
            //            //foreach (var user in userList)
            //            //{
            //            //    ddlctrl.Items.Add(new ListItem(user.Name, Convert.ToString(user.ID)));
            //            //}

            //            ddlctrl.Items.Insert(0, new ListItem("–Select–", "")); //"", "–Select–"

            //            ddlctrl.Attributes.Add("data-native-menu", "false");
            //            ddlctrl.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //            ddlctrl.CssClass = ".questiondiv_" + question.ID + " userctr";
            //            ctrPanel.Controls.Add(ddlctrl);

            //            if (question.FieldMandatory)
            //                ddlctrl.Attributes.Add("required", "required");

            //            break;

            //        case Constants.ServiceQuestionType.LOOKUP:
            //            DropDownList lookupCtr = new DropDownList();
            //            lookupCtr.CssClass = "dropdownctr";
            //            lookupCtr.Attributes.Add("ctype", "lookup");

            //            string listName = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("lookuplist", out listName);

            //            string listfield = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("lookupfield", out listfield);

            //            string lpModuleName = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("module", out lpModuleName);

            //            bool isMulti = false;
            //            string isMultiS = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("multi", out isMultiS);
            //            isMulti = UGITUtility.StringToBoolean(isMultiS);


            //            //SPList list = SPContext.Current.Web.Lists.TryGetList(listName);
            //            //SPField field = null;
            //            //if (list != null && list.Fields.ContainsField(listfield))
            //            //{
            //            //    field = list.Fields.GetField(listfield);
            //            //}

            //            //if (field != null)
            //            //{
            //            //    if (list.Title == DatabaseObjects.Lists.Department)
            //            //    {
            //            //        //Page dd = new Page();
            //            //        DepartmentDropdownListMobile mlf = (DepartmentDropdownListMobile)page.LoadControl("~/_controltemplates/15/uGovernIT/Utility/DepartmentDropdownListMobile.ascx");
            //            //        mlf.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //            //        mlf.IsMulti = isMulti;
            //            //        mlf.ControlMode = SPControlMode.Edit;
            //            //        mlf.Attributes.Add("ctype", "lookup");
            //            //        mlf.EnableViewState = true;
            //            //        if (question.FieldMandatory)
            //            //        {
            //            //            mlf.MandatoryCheck = true;
            //            //            mlf.MandatoryMessage = defaultMandatoryErrorMsg;
            //            //        }
            //            //        ctrPanel.Controls.Add(mlf);
            //            //    }
            //            //    else
            //            //    {
            //            //        DataTable table = list.GetItems().GetDataTable();
            //            //        DataTable columnVals = new DataTable();
            //            //        if (table != null)
            //            //        {
            //            //            DataView dView = table.DefaultView;
            //            //            dView.Sort = string.Format("{0} asc", field.InternalName);
            //            //            string filter = string.Empty;
            //            //            if (table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
            //            //                filter = string.Format("{0} = '{1}'", DatabaseObjects.Columns.ModuleNameLookup, lpModuleName);

            //            //            if (table.Columns.Contains(DatabaseObjects.Columns.IsDeleted))
            //            //            {
            //            //                if (!string.IsNullOrEmpty(filter))
            //            //                    filter += " AND ";
            //            //                filter += string.Format("{0} <> '1'", DatabaseObjects.Columns.IsDeleted);
            //            //            }
            //            //            if (!string.IsNullOrEmpty(filter))
            //            //                dView.RowFilter = filter;
            //            //            if (listName == DatabaseObjects.Lists.TicketPriority ||
            //            //            listName == DatabaseObjects.Lists.TicketSeverity ||
            //            //            listName == DatabaseObjects.Lists.TicketImpact)
            //            //            {
            //            //                dView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.ItemOrder);
            //            //            }

            //            //            columnVals = dView.ToTable(true, field.InternalName, DatabaseObjects.Columns.Id);
            //            //        }

            //            //        foreach (DataRow row in columnVals.Rows)
            //            //        {
            //            //            lookupCtr.Items.Add(new ListItem(Convert.ToString(row[field.InternalName]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
            //            //        }

            //            //        lookupCtr.Items.Insert(0, new ListItem("(None)", ""));
            //            //        lookupCtr.Attributes.Add("onchange", string.Format("questionBrachLogic(this,'{0}','dropdownlist',\"{1}\")", questionType, question.TokenName));

            //            //        lookupCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //            //        ctrPanel.Controls.Add(lookupCtr);


            //            //        if (question.FieldMandatory)
            //            //        {
            //            //            lookupCtr.Attributes.Add("required", "required");
            //            //        }
            //            //    }
            //            //}

            //            break;
            //        case Constants.ServiceQuestionType.REQUESTTYPE:

            //            string dashboardType = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("dropdowntype", out dashboardType);

            //            string requestTypes = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("requesttypes", out requestTypes);

            //            string moduleName = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("module", out moduleName);

            //            string showCategoryDropDown = string.Empty;
            //            question.QuestionTypeProperties.TryGetValue("enablecategorydropdown", out showCategoryDropDown);
            //            bool enableCategoryDropdown = UGITUtility.StringToBoolean(showCategoryDropDown);


            //            Panel pRequestTypeCtr = new Panel();
            //            pRequestTypeCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);


            //            DataRow moduleRow = uGITCache.GetModuleDetails(moduleName);
            //            List<string> reqestTypes = UGITUtility.ConvertStringToList(requestTypes, new string[] { Constants.Separator1, Constants.Separator2 });

            //            if (reqestTypes == null || (reqestTypes.Count > 0 && (reqestTypes[0].ToLower() == string.Empty || reqestTypes[0].ToLower() == "all")))
            //                reqestTypes = new List<string>();

            //            DropDownList ddlRequestType = UGITUtility.GetRequestTypesWithCategoriesDropDown(dbContext, moduleRow, false, true, reqestTypes);
            //            ddlRequestType.ID = string.Format("{0}_dropdown", pRequestTypeCtr.ID);

            //            ddlRequestType.Attributes.Remove("onchange");
            //            ddlRequestType.Attributes.Add("ctype", "requesttype");
            //            // pRequestTypeCtr.Controls.Add(ddlRequestType);

            //            ddlRequestType.Attributes.Add("onchange", string.Format("questionBrachLogic(this,'{0}', 'dropdownlist',\"{1}\")", questionType, question.TokenName));

            //            if (enableCategoryDropdown)
            //            {
            //                //DropDownList subCateDropdown = new DropDownList();

            //                //subCateDropdown.ID = string.Format("question_{0}_{1}_category", question.ServiceSectionID, question.ID);

            //                //subCateDropdown = UGITUtility.GetCategoryWithSubCategoriesDropDown(moduleRow, true, reqestTypes);
            //                //subCateDropdown.Attributes.Add("ModuleName", Convert.ToString(moduleRow[DatabaseObjects.Columns.ModuleName]));
            //                //subCateDropdown.Attributes.Add("onchange", "requestTypeCategoryChanges(this)");
            //                //subCateDropdown.Attributes.Add("class", "subCatg");


            //                //pRequestTypeCtr.Controls.Add(subCateDropdown);
            //                //pRequestTypeCtr.Controls.Add(ddlRequestType);

            //            }
            //            else
            //            {
            //                pRequestTypeCtr.Controls.Add(ddlRequestType);
            //            }
            //            if (question.FieldMandatory)
            //            {
            //                ddlRequestType.Attributes.Add("required", "required");
            //            }


            //            ctrPanel.Controls.Add(pRequestTypeCtr);
            //            break;

            //        case Constants.ServiceQuestionType.Rating:
            //            {
            //                //string sfOptions = string.Empty;
            //                //question.QuestionTypeProperties.TryGetValue("options", out sfOptions);
            //                //List<string> sfOptionList = new List<string>();
            //                //if (sfOptions != null)
            //                //    sfOptionList = UGITUtility.ConvertStringToList(sfOptions, new string[] { Constants.Separator1, Constants.Separator2 });


            //                ////Page page = new Page();
            //                //RatingCtr ratingCtr = (RatingCtr)page.LoadControl("~/_controltemplates/15/uGovernIT/RatingCtr.ascx");
            //                //ratingCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //                //ratingCtr.RatingOptions = new List<string>();
            //                //foreach (string sfl in sfOptionList)
            //                //{
            //                //    ratingCtr.RatingOptions.Add(sfl);
            //                //}

            //                ////configurable rating bar..
            //                //ratingCtr.MaxRating = sfOptionList.Count;

            //                //ratingCtr.JSOnChange = string.Format("questionBrachLogic(this,'{0}', 'textbox', \"{1}\")", questionType, question.TokenName);
            //                //ctrPanel.Controls.Add(ratingCtr);
            //            }
            //            break;
            //        case Constants.ServiceQuestionType.ApplicationAccess:
            //            {
            //                string sfOptions = string.Empty;
            //                string strDefaultUser = string.Empty;
            //                string strNewUser = string.Empty;
            //                string strExistingUser = string.Empty;
            //                string accessRequestModes = string.Empty;
            //                question.QuestionTypeProperties.TryGetValue("application", out sfOptions);
            //                question.QuestionTypeProperties.TryGetValue(ConfigConstants.NewUSer, out strNewUser);
            //                question.QuestionTypeProperties.TryGetValue(ConfigConstants.ExistingUser, out strExistingUser);
            //                question.QuestionTypeProperties.TryGetValue(ConfigConstants.AccessRequestMode, out accessRequestModes);
            //                List<int> lstApplicationIds = new List<int>();
            //                if (sfOptions != null && sfOptions.ToLower() != "all")
            //                {
            //                    List<string> sfOptionList = UGITUtility.ConvertStringToList(sfOptions, new string[] { Constants.Separator1 });
            //                    foreach (string item in sfOptionList)
            //                    {
            //                        string id = Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[1];
            //                        int appID = UGITUtility.StringToInt(id);
            //                        if (appID > 0)
            //                            lstApplicationIds.Add(appID);
            //                    }
            //                }
            //                Panel pnlapplications = new Panel();
            //                //ServiceMatrix serviceMatrix = (ServiceMatrix)page.LoadControl("~/_controltemplates/15/uGovernIT/ServiceMatrix.ascx");
            //                //serviceMatrix.ID = string.Format("question_serviceMatrix_{0}_{1}", question.ServiceSectionID, question.ID);
            //                //serviceMatrix.IsMobile = true;
            //                //serviceMatrix.Applications = lstApplicationIds.Select(x => x).ToList();
            //                //serviceMatrix.RoleAssignee = string.Empty;
            //                //serviceMatrix.AccessType = "add";
            //                //if (!string.IsNullOrWhiteSpace(strExistingUser))
            //                //    serviceMatrix.DependentToExistingUser = true;

            //                //if (!string.IsNullOrEmpty(accessRequestModes))
            //                //{
            //                //    serviceMatrix.LstAccessRequestModes = accessRequestModes.ToLower().Split(';').ToList();
            //                //    serviceMatrix.AccessType = serviceMatrix.LstAccessRequestModes.First();
            //                //}
            //                //serviceMatrix.IsReadOnly = false;
            //                //serviceMatrix.ControlIDPrefix = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //                //serviceMatrix.IsNoteEnabled = true;
            //                //if (question.FieldMandatory)
            //                //{
            //                //    defaultMandatoryErrorMsg = "    ";
            //                //    serviceMatrix.MandatoryCheck = true;
            //                //    serviceMatrix.MandatoryMessage = defaultMandatoryErrorMsg;
            //                //}
            //                //pnlapplications.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //                //pnlapplications.Controls.Add(serviceMatrix);

            //                //if (!string.IsNullOrEmpty(strExistingUser))
            //                //    pnlapplications.Attributes.Add("dependentquestion", strExistingUser);
            //                //pnlapplications.CssClass += " applicationaccessservice";
            //                //ctrPanel.Controls.Add(pnlapplications);

            //            }
            //            break;
            //        case Constants.ServiceQuestionType.RemoveAccess:
            //            {
            //                //Page page = new Page();
            //                Panel pnlRemoveUserAccess = new Panel();
            //                //RemoveUserAccess removeUserAccess = (RemoveUserAccess)page.LoadControl("~/_controltemplates/15/uGovernIT/RemoveUserAccess.ascx");
            //                //removeUserAccess.QuestionId = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //                //removeUserAccess.IsReadOnly = false;
            //                //removeUserAccess.IsShowFieldSet = true;
            //                //pnlRemoveUserAccess.Controls.Add(removeUserAccess);
            //                //removeUserAccess.ID = string.Format("question_removeaccess_{0}_{1}", question.ServiceSectionID, question.ID); ;
            //                //pnlRemoveUserAccess.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //                //ctrPanel.Controls.Add(pnlRemoveUserAccess);
            //            }
            //            break;
            //        case Constants.ServiceQuestionType.Assets:
            //            {
            //                DropDownList userSpecificAssets = new DropDownList();
            //                userSpecificAssets.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            //                ctrPanel.Controls.Add(userSpecificAssets);
            //                string inputQ = string.Empty;
            //                question.QuestionTypeProperties.TryGetValue("userquestion", out inputQ);
            //                if (!string.IsNullOrEmpty(inputQ))
            //                    userSpecificAssets.Attributes.Add("DependentQuestion", inputQ);
            //                userSpecificAssets.CssClass += " assetquestionservice";
            //                userSpecificAssets.EnableViewState = true;

            //                userSpecificAssets.Attributes.Add("onchange", string.Format("questionBrachLogic(this,'{0}', 'dropdownlist',\"{1}\")", "singlechoice", question.TokenName));

            //                if (question.FieldMandatory)
            //                {
            //                    ctrPanel.Controls.Add(tempBoxForValidation);
            //                    RequiredFieldValidator reqValidator = new RequiredFieldValidator();
            //                    reqValidator.ControlToValidate = tempBoxForValidation.ID;
            //                    reqValidator.ErrorMessage = defaultMandatoryErrorMsg;
            //                    reqValidator.CssClass = "errormsg-container";
            //                    reqValidator.Display = ValidatorDisplay.Dynamic;
            //                    ctrPanel.Controls.Add(reqValidator);
            //                }

            //            }
            //            break;
            //    }

            //    return ctrPanel;
            //}

        static void ddlctrl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //private void GridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        //{
        //    ASPxGridView cbx = (ASPxGridView)sender;


        //    string parameters = Uri.UnescapeDataString(e.Parameters);
        //    string userName = string.Empty;
        //    string strUserChange = string.Empty;
        //    if (parameters.Contains(Constants.Separator))
        //    {
        //        userName = parameters.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[0];
        //        strUserChange = parameters.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[1];
        //    }
        //    else
        //        userName = parameters;
        //    UserProfile user = UserProfile.LoadUserProfileByName(userName);
        //    UGITModule cmdbModule = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "CMDB");
        //    DataTable result = null;

        //    if (user != null)
        //    {
        //        result = uGITCache.ModuleDataCache.GetOpenTickets(cmdbModule.ID);
        //        if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.AssetOwner))
        //        {
        //            //Filter all the assets where current user is assetower or current user
        //            var xbase = result.AsEnumerable().Where(x => (!x.IsNull(DatabaseObjects.Columns.AssetOwner) && x.Field<string>(DatabaseObjects.Columns.AssetOwner) == user.Name) ||
        //            (!x.IsNull(DatabaseObjects.Columns.CurrentUser) && x.Field<string>(DatabaseObjects.Columns.CurrentUser) == user.Name));
        //            if (xbase.Count() > 0)
        //            {
        //                result = xbase.CopyToDataTable();
        //            }
        //            else
        //            {
        //                result = null;
        //            }
        //        }
        //        if (strUserChange != "userChange")
        //        {
        //            if ((cbx.DataSource != null || cbx.Selection.Count > 0))
        //                return;
        //        }
        //        cbx.DataSource = result;
        //        cbx.DataBind();
        //    }


        //}

        
        private void requestTypeDropdown_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
           // ASPxCallbackPanel panel = (ASPxCallbackPanel)sender;

        }

        

        


        

        /// <summary>
        /// Generate control for skip logic.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="selectedVals"></param>
        /// <returns></returns>
        public Control GenerateControlForSkipLogic(ServiceQuestion question, string selectedVals)
        {
            List<string> selValues = UGITUtility.ConvertStringToList(selectedVals, Constants.Separator2);
            Control ctr = new Control();
            string questionType = question.QuestionType.ToLower();
            string defaultVal = string.Empty;

            TextBox tempBoxForValidation = new TextBox();
            tempBoxForValidation.Style.Add(HtmlTextWriterStyle.Display, "none");
            tempBoxForValidation.CssClass += string.Format(" hiddenCtr hiddenCtr_{0}", question.ID);
            tempBoxForValidation.ID = string.Format("txthiddenCtr_{0}", question.ID);

            switch (questionType)
            {
                case Constants.ServiceQuestionType.TEXTBOX:
                case Constants.ServiceQuestionType.Number:
                    TextBox txtCtr = new TextBox();
                    txtCtr.CssClass += string.Format("txtCtr_{0} textboxctr", question.ID);

                    txtCtr.Attributes.Add("ctype", "textbox");
                    string txtMode = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("textmode", out txtMode);
                    if (txtMode != null)
                    {
                        if (txtMode.ToLower() == "multiline")
                        {
                            txtCtr.TextMode = TextBoxMode.MultiLine;
                        }
                    }

                    //Set select value
                    if (selValues.Count > 0)
                    {
                        txtCtr.Text = selValues[0];
                    }
                    txtCtr.Width = new Unit("96%");
                    ctr = txtCtr;
                    break;
                case Constants.ServiceQuestionType.SINGLECHOICE:
                case Constants.ServiceQuestionType.MULTICHOICE:
                    List<ListItem> items = new List<ListItem>();
                    string optionType = "dropdownctr";

                    question.QuestionTypePropertiesDicObj.TryGetValue("optiontype", out optionType);

                    string options = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("options", out options);

                    string optionPickFromField = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("selectedfield", out optionPickFromField);

                    string pickFromField = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("pickfromfield", out pickFromField);

                    string SelectedList = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("selectedlist", out SelectedList);

                    if (UGITUtility.StringToBoolean(pickFromField))
                    {

                        //SPWeb oWebsite = SPContext.Current.Web;
                        //SPList splist = oWebsite.Lists[SelectedList];
                        //SPListItemCollection colItems = splist.Items;

                        //SPFieldChoice choiceField = splist.Fields[optionPickFromField] as SPFieldChoice;
                        //if (choiceField != null)
                        //{
                        //    foreach (string option in choiceField.Choices)
                        //    {
                        //        items.Add(new ListItem(option, option));
                        //    }
                        //}

                    }
                    else
                    {
                        if (options != null)
                        {
                            List<string> optionList = UGITUtility.ConvertStringToList(options, new string[] { Constants.Separator1, Constants.Separator2 });
                            foreach (string option in optionList)
                            {
                                items.Add(new ListItem(option, option));
                            }
                        }
                    }

                    //Check checkbox list for multi choice 
                    CheckBoxList ddlMCtr = new CheckBoxList();
                    ddlMCtr.Items.AddRange(items.ToArray());

                    //Set select value
                    foreach (string sVal in selValues)
                    {
                        ListItem item = ddlMCtr.Items.FindByValue(sVal);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }
                    ctr = ddlMCtr;
                    break;

                case Constants.ServiceQuestionType.CHECKBOX:
                    CheckBox checkboxCtr = new CheckBox();
                    //Set select value
                    if (selValues.Count > 0)
                    {
                        checkboxCtr.Checked = UGITUtility.StringToBoolean(selValues[0]);
                    }
                    ctr = checkboxCtr;
                    break;
                case Constants.ServiceQuestionType.DATETIME:
                    ASPxDateEdit datetimeCtr = new ASPxDateEdit();

                    //datetimeCtr.CssClassTextBox = "datetimectr";
                    string dateMode = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("datemode", out dateMode);
                    if (dateMode != null)
                    {
                        if (dateMode.ToLower() == "dateonly")
                        {
                            datetimeCtr.TimeSectionProperties.Visible = false;
                        }
                        else if (dateMode.ToLower() == "timeonly")
                        {
                            datetimeCtr.TimeSectionProperties.Visible = true;
                        }
                    }
                    datetimeCtr.TimeSectionProperties.Visible = false;
                    //datetimeCtr.date = true;

                    //Set selected value
                    if (selValues.Count > 0)
                    {
                        DateTime date = DateTime.MinValue;
                        DateTime.TryParse(selValues[0].Trim(), out date);
                        if (date != DateTime.MinValue)
                            datetimeCtr.Date = date;
                    }
                    ctr = datetimeCtr;
                    break;
                case Constants.ServiceQuestionType.USERFIELD:
                    UserLookupValue userCtr = new UserLookupValue();
                    //userCtr.PrincipalSource = SPPrincipalSource.UserInfoList;
                    //userCtr.SelectionSet = "User";
                    //userCtr.Attributes.Add("ugselectionset", userCtr.SelectionSet);
                    //userCtr.MultiSelect = true;
                    //userCtr.AugmentEntitiesFromUserInfo = true;
                    //userCtr.CssClass = "userctr";
                    //userCtr.DialogHeight = 200;
                    //userCtr.DialogWidth = 300;
                    //userCtr.Width = new Unit("100%");

                    UserProfileManager userManager = new UserProfileManager(dbContext);
                    //Set selected value
                    if (selValues.Count > 0)
                    {
                        List<string> userNames = new List<string>();
                        foreach (string userIDVal in selValues)
                        {                                         
                            UserProfile user = userManager.GetUserById(userIDVal);
                            if (user != null)
                            {
                                userNames.Add(user.UserName);
                            }
                        }
                      // userCtr.s = string.Join(",", userNames.ToArray());
                    }

                   // ctr = userCtr;
                    break;

                case Constants.ServiceQuestionType.LOOKUP:

                    CheckBoxList lookupCtr = new CheckBoxList();
                    string listName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("lookuplist", out listName);

                    string listfield = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("lookupfield", out listfield);

                    string lpModuleName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("module", out lpModuleName);

                    FieldConfigurationManager configManager = new FieldConfigurationManager(dbContext);
                    FieldConfiguration fieldconfig = configManager.GetFieldByFieldName(listName);
                    if (fieldconfig != null)
                    {
                        DataTable list = GetTableDataManager.GetTableData(fieldconfig.ParentTableName, $"{DatabaseObjects.Columns.TenantID} = '{dbContext.TenantID}'"); // SPContext.Current.Web.Lists.TryGetList(listName);
                        DataColumn field = null;
                        if (list != null && list.Columns.Contains(listfield))
                        {
                            field = list.Columns[listfield];
                        }

                        if (field != null)
                        {
                            DataTable table = list;
                            DataTable columnVals = new DataTable();
                            if (table != null)
                            {
                                DataView dView = table.DefaultView;
                                dView.Sort = string.Format("{0} asc", field.ColumnName);
                                if (table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                                {
                                    dView.RowFilter = string.Format("{0} = '{1}'", DatabaseObjects.Columns.ModuleNameLookup, lpModuleName);
                                }

                                columnVals = dView.ToTable(true, field.ColumnName, DatabaseObjects.Columns.Id);
                            }

                            foreach (DataRow row in columnVals.Rows)
                            {
                                lookupCtr.Items.Add(new ListItem(Convert.ToString(row[field.ColumnName]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                            }

                            //set selected values
                            foreach (string sVal in selValues)
                            {
                                ListItem item = lookupCtr.Items.FindByValue(sVal);
                                if (item != null)
                                {
                                    item.Selected = true;
                                }
                            }


                            ctr = lookupCtr;
                        }
                    }
                    break;
                case Constants.ServiceQuestionType.REQUESTTYPE:

                    string requestTypes = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("requesttypes", out requestTypes);

                    string moduleName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("module", out moduleName);
                   UGITModule moduleRow = ObjModuleViewManager.LoadByName(moduleName);
                    List<string> reqestTypes = UGITUtility.ConvertStringToList(requestTypes, new string[] { Constants.Separator1, Constants.Separator2 });
                    if (reqestTypes == null || (reqestTypes.Count > 0 && (reqestTypes[0].ToLower() == string.Empty || reqestTypes[0].ToLower() == "all")))
                        reqestTypes = new List<string>();                  
                   // DropDownList ddlRequestType = uHelper.GetRequestTypesWithCategoriesDropDown(dbContext, moduleRow, reqestTypes, true, true);
                    CheckBoxList rCBList = new CheckBoxList();
                    //foreach (ListItem item in ddlRequestType.Items)
                    //{
                    //    item.Selected = false;
                    //    if (item.Attributes["disabled"] != null)
                    //    {
                    //        item.Enabled = false;
                    //    }
                    //    rCBList.Items.Add(item);
                    //}

                    foreach (string sVal in selValues)
                    {
                        ListItem item = rCBList.Items.FindByValue(sVal);
                        if (item != null)
                        {
                            item.Selected = true;
                        }
                    }

                    ctr = rCBList;

                    break;

                case Constants.ServiceQuestionType.Rating:
                    //Create dropdown with value 1 to 5 for rating
                    DropDownList ddlRatings = new DropDownList();
                    ddlRatings.Items.Add(new ListItem("(None)", ""));
                    ddlRatings.Items.Add(new ListItem("1", "1"));
                    ddlRatings.Items.Add(new ListItem("2", "2"));
                    ddlRatings.Items.Add(new ListItem("3", "3"));
                    ddlRatings.Items.Add(new ListItem("4", "4"));
                    ddlRatings.Items.Add(new ListItem("5", "5"));
                    ctr = ddlRatings;
                    ddlRatings.SelectedIndex = ddlRatings.Items.IndexOf(ddlRatings.Items.FindByValue(selectedVals));
                    break;

            }
            ctr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
            return ctr;
        }

        public string ReadSelectedValueSkipLogic(ServiceQuestion question, Control ctr, string operatorVal)
        {
            List<string> ctrValues = new List<string>();
            if (ctr is CheckBoxList)
            {
                CheckBoxList cList = (CheckBoxList)ctr;
                foreach (ListItem item in cList.Items)
                {
                    if (item.Selected)
                    {
                        ctrValues.Add(item.Value);
                    }
                }
            }
            else if (ctr is CheckBox)
            {
                ctrValues.Add(((CheckBox)ctr).Checked.ToString());
            }
            else if (ctr is ASPxDateEdit)
            {
                ASPxDateEdit dateCtr = (ASPxDateEdit)ctr;
                ctrValues.Add(dateCtr.Date.ToString());
            }
            //else if (ctr is UserValueBox)
            //{
            //    PeopleEditor ufCtr = (PeopleEditor)ctr;

            //    if (ufCtr.Accounts.Count > 0)
            //    {
            //        for (int i = 0; i < ufCtr.Accounts.Count; i++)
            //        {
            //            PickerEntity entity = (PickerEntity)ufCtr.Entities[i];
            //            if (entity != null && entity.Key != null)
            //            {
            //                SPUser user = UserProfile.GetUserByName(entity.Key, SPPrincipalType.User);
            //                if (user != null)
            //                {
            //                    SPFieldUserValue userLookup = new SPFieldUserValue();
            //                    userLookup.LookupId = user.ID;
            //                    ctrValues.Add(string.Format("{0}", userLookup.LookupId));
            //                }
            //            }
            //        }
            //    }
            //}
            else if (ctr is TextBox)
            {
                ctrValues.Add(((TextBox)ctr).Text.Trim());
            }
            else if (ctr is DropDownList)
            {
                ctrValues.Add(((DropDownList)ctr).SelectedValue);
            }

            return string.Join(Constants.Separator2, ctrValues.ToArray());
        }
        public List<string> GetQTOperators(string questionType)
        {
            List<string> operators = new List<string>();

            questionType = questionType.ToLower();
            if (questionType == Constants.ServiceQuestionType.SINGLECHOICE ||
                questionType == Constants.ServiceQuestionType.USERFIELD ||
                questionType == Constants.ServiceQuestionType.LOOKUP ||
                questionType == Constants.ServiceQuestionType.REQUESTTYPE)
            {
                operators = new List<string>();
                operators.Add("=");
                operators.Add("!=");
                operators.Add("OneOf");
                operators.Add("NotOneOf");
            }
            else if (questionType == Constants.ServiceQuestionType.MULTICHOICE)
            {
                operators = new List<string>();
                operators.Add("=");
                operators.Add("!=");
                operators.Add("OneOf");
                operators.Add("NotOneOf");
                operators.Add("Contain");
                operators.Add("NotContain");
            }
            else if (questionType == Constants.ServiceQuestionType.TEXTBOX ||
                questionType == Constants.ServiceQuestionType.CHECKBOX ||
                questionType == Constants.ServiceQuestionType.Rating)
            {
                operators = new List<string>();
                operators.Add("=");
                operators.Add("!=");
            }
            else if (questionType == Constants.ServiceQuestionType.DATETIME || questionType == Constants.ServiceQuestionType.Number)
            {
                operators = new List<string>();
                operators.Add("=");
                operators.Add("!=");
                operators.Add(">");
                operators.Add(">=");
                operators.Add("<");
                operators.Add("<=");
            }

            return operators;
        }

        /// <summary>
        /// This method test the condition based on left value,right value,operator and type of question
        /// </summary>
        /// <param name="lsValue">left value</param>
        /// <param name="operatorVal">operator like =,!=,oneof,notoneof,..</param>
        /// <param name="values">right value</param>
        /// <param name="questionType">type of question like singlechoice,textbox,multichoice,...</param>
        /// <returns></returns>
        public bool TestCondition(string lsValue, string operatorVal, string values, string questionType)
        {
            bool isValid = false;
            questionType = questionType.ToLower();
            operatorVal = operatorVal.ToLower();
            List<string> lsValueArr = UGITUtility.ConvertStringToList(lsValue, questionType == Constants.ServiceQuestionType.MULTICHOICE ? Constants.Separator5 : Constants.Separator2);
            List<string> rsValueArr = UGITUtility.ConvertStringToList(values, Constants.Separator2);
            try
            {
                switch (operatorVal)
                {
                    case "=":
                        {
                            if (questionType == Constants.ServiceQuestionType.SINGLECHOICE ||
                                questionType == Constants.ServiceQuestionType.TEXTBOX ||
                                questionType == Constants.ServiceQuestionType.MULTICHOICE ||
                                questionType == Constants.ServiceQuestionType.Rating)
                            {
                                if (values.ToLower() == lsValue.ToLower())
                                {
                                    isValid = true;
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.LOOKUP ||
                                questionType == Constants.ServiceQuestionType.REQUESTTYPE)
                            {
                                if (!string.IsNullOrEmpty(lsValue) && lsValue == values)
                                {
                                    isValid = true;
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.Number)
                            {
                                double lsVal = 0;
                                double rsVal = 0;
                                if (double.TryParse(lsValue, out lsVal) && double.TryParse(values, out rsVal))
                                {
                                    if (lsVal == rsVal)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.CHECKBOX)
                            {
                                bool ls = UGITUtility.StringToBoolean(lsValue);
                                bool rs = UGITUtility.StringToBoolean(values);
                                if (ls == rs)
                                {
                                    isValid = true;
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.DATETIME)
                            {
                                DateTime lsVal = DateTime.MinValue;
                                DateTime rsVal = DateTime.MinValue;
                                if (DateTime.TryParse(lsValue, out lsVal) && DateTime.TryParse(values, out rsVal))
                                {
                                    if (lsVal.Date == rsVal.Date)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.USERFIELD)
                            {

                                //SPFieldUserValue lookup = new SPFieldUserValue(SPContext.Current.Web, lsValue);
                                //if (lookup.LookupId > 0 && lookup.LookupId.ToString() == values)
                                //{
                                //    isValid = true;
                                //}

                            }
                        }
                        break;
                    case "!=":
                        {
                            if (questionType == Constants.ServiceQuestionType.SINGLECHOICE ||
                               questionType == Constants.ServiceQuestionType.TEXTBOX ||
                               questionType == Constants.ServiceQuestionType.MULTICHOICE ||
                               questionType == Constants.ServiceQuestionType.Rating)
                            {
                                if (values.ToLower() != lsValue.ToLower())
                                {
                                    isValid = true;
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.LOOKUP ||
                                questionType == Constants.ServiceQuestionType.REQUESTTYPE)
                            {

                                //SPFieldLookupValue lookup = new SPFieldLookupValue(lsValue);
                                //if (lookup.LookupId > 0 && lookup.LookupId.ToString() != values)
                                //{
                                //    isValid = true;
                                //}

                            }
                            else if (questionType == Constants.ServiceQuestionType.Number)
                            {
                                double lsVal = 0;
                                double rsVal = 0;
                                if (double.TryParse(lsValue, out lsVal) && double.TryParse(values, out rsVal))
                                {
                                    if (lsVal != rsVal)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.CHECKBOX)
                            {
                                bool ls = UGITUtility.StringToBoolean(lsValue);
                                bool rs = UGITUtility.StringToBoolean(values);
                                if (ls != rs)
                                {
                                    isValid = true;
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.DATETIME)
                            {
                                DateTime lsVal = DateTime.MinValue;
                                DateTime rsVal = DateTime.MinValue;
                                if (DateTime.TryParse(lsValue, out lsVal) && DateTime.TryParse(values, out rsVal))
                                {
                                    if (lsVal.Date != rsVal.Date)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.USERFIELD)
                            {
                                //SPFieldUserValue lookup = new SPFieldUserValue(SPContext.Current.Web, lsValue);
                                //if (lookup.LookupId > 0 && lookup.LookupId.ToString() != values)
                                //{
                                //    isValid = true;
                                //}
                            }
                        }
                        break;
                    case "oneof":
                        {
                            if (questionType == Constants.ServiceQuestionType.SINGLECHOICE ||
                               questionType == Constants.ServiceQuestionType.TEXTBOX)
                            {
                                if (rsValueArr.Contains(lsValue))
                                {
                                    isValid = true;
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.MULTICHOICE)
                            {
                                foreach (string lsVal in lsValueArr)
                                {
                                    if (rsValueArr.Contains(lsVal))
                                    {
                                        isValid = true;
                                        break;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.LOOKUP ||
                                questionType == Constants.ServiceQuestionType.REQUESTTYPE)
                            {

                                //SPFieldLookupValue lookup = new SPFieldLookupValue(lsValue);
                                //if (lookup.LookupId > 0 && rsValueArr.Contains(lookup.LookupId.ToString()))
                                //{
                                //    isValid = true;
                                //}
                            }
                            else if (questionType == Constants.ServiceQuestionType.Number)
                            {
                                double lsVal = 0;
                                double rsVal = 0;
                                if (double.TryParse(lsValue, out lsVal) && double.TryParse(values, out rsVal))
                                {
                                    if (lsVal == rsVal)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.DATETIME)
                            {
                                DateTime lsVal = DateTime.MinValue;
                                DateTime rsVal = DateTime.MinValue;
                                if (DateTime.TryParse(lsValue, out lsVal) && DateTime.TryParse(values, out rsVal))
                                {
                                    if (lsVal.Date == rsVal.Date)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.USERFIELD)
                            {

                                //SPFieldUserValue lookup = new SPFieldUserValue(SPContext.Current.Web, lsValue);
                                //if (lookup.LookupId > 0 && rsValueArr.Contains(lookup.LookupId.ToString()))
                                //{
                                //    isValid = true;
                                //}

                            }
                        }
                        break;
                    case "notoneof":
                        {
                            if (questionType == Constants.ServiceQuestionType.SINGLECHOICE ||
                               questionType == Constants.ServiceQuestionType.TEXTBOX)
                            {
                                if (!rsValueArr.Contains(lsValue))
                                {
                                    isValid = true;
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.MULTICHOICE)
                            {
                                foreach (string lsVal in lsValueArr)
                                {
                                    if (rsValueArr.Contains(lsVal))
                                    {
                                        isValid = true;
                                    }
                                }

                                if (isValid)
                                    isValid = false;
                                else
                                    isValid = true;

                            }
                            else if (questionType == Constants.ServiceQuestionType.LOOKUP ||
                                questionType == Constants.ServiceQuestionType.REQUESTTYPE)
                            {
                                //SPFieldLookupValue lookup = new SPFieldLookupValue(lsValue);
                                //if (lookup.LookupId > 0 && !rsValueArr.Contains(lookup.LookupId.ToString()))
                                //{
                                //    isValid = true;
                                //}
                            }
                            else if (questionType == Constants.ServiceQuestionType.Number)
                            {
                                double lsVal = 0;
                                double rsVal = 0;
                                if (double.TryParse(lsValue, out lsVal) && double.TryParse(values, out rsVal))
                                {
                                    if (lsVal != rsVal)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.DATETIME)
                            {
                                DateTime lsVal = DateTime.MinValue;
                                DateTime rsVal = DateTime.MinValue;
                                if (DateTime.TryParse(lsValue, out lsVal) && DateTime.TryParse(values, out rsVal))
                                {
                                    if (lsVal.Date != rsVal.Date)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.USERFIELD)
                            {

                                //SPFieldUserValue lookup = new SPFieldUserValue(SPContext.Current.Web, lsValue);
                                //if (lookup.LookupId > 0 && !rsValueArr.Contains(lookup.LookupId.ToString()))
                                //{
                                //    isValid = true;
                                //}

                            }
                        }
                        break;
                    case "contain":
                        {
                            if (questionType == Constants.ServiceQuestionType.MULTICHOICE)
                            {
                                if (lsValueArr.Intersect(rsValueArr).Count() == rsValueArr.Count())
                                {
                                    isValid = true;
                                }
                            }
                        }
                        break;
                    case "notcontain":
                        {
                            if (questionType == Constants.ServiceQuestionType.MULTICHOICE)
                            {
                                if (lsValueArr.Intersect(rsValueArr).Count() != rsValueArr.Count())
                                {
                                    isValid = true;
                                }
                            }
                        }
                        break;
                    case ">":
                        {
                            if (questionType == Constants.ServiceQuestionType.Number)
                            {
                                double lsVal = 0;
                                double rsVal = 0;
                                if (double.TryParse(lsValue, out lsVal) && double.TryParse(values, out rsVal))
                                {
                                    if (lsVal > rsVal)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.DATETIME)
                            {
                                DateTime lsVal = DateTime.MinValue;
                                DateTime rsVal = DateTime.MinValue;
                                if (DateTime.TryParse(lsValue, out lsVal) && DateTime.TryParse(values, out rsVal))
                                {
                                    if (lsVal.Date > rsVal.Date)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                        }
                        break;
                    case "<":
                        {
                            if (questionType == Constants.ServiceQuestionType.Number)
                            {
                                double lsVal = 0;
                                double rsVal = 0;
                                if (double.TryParse(lsValue, out lsVal) && double.TryParse(values, out rsVal))
                                {
                                    if (lsVal < rsVal)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.DATETIME)
                            {
                                DateTime lsVal = DateTime.MinValue;
                                DateTime rsVal = DateTime.MinValue;
                                if (DateTime.TryParse(lsValue, out lsVal) && DateTime.TryParse(values, out rsVal))
                                {
                                    if (lsVal.Date < rsVal.Date)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                        }
                        break;
                    case ">=":
                        {
                            if (questionType == Constants.ServiceQuestionType.Number)
                            {
                                double lsVal = 0;
                                double rsVal = 0;
                                if (double.TryParse(lsValue, out lsVal) && double.TryParse(values, out rsVal))
                                {
                                    if (lsVal >= rsVal)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.DATETIME)
                            {
                                DateTime lsVal = DateTime.MinValue;
                                DateTime rsVal = DateTime.MinValue;
                                if (DateTime.TryParse(lsValue, out lsVal) && DateTime.TryParse(values, out rsVal))
                                {
                                    if (lsVal.Date >= rsVal.Date)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                        }
                        break;
                    case "<=":
                        {
                            if (questionType == Constants.ServiceQuestionType.Number)
                            {
                                double lsVal = 0;
                                double rsVal = 0;
                                if (double.TryParse(lsValue, out lsVal) && double.TryParse(values, out rsVal))
                                {
                                    if (lsVal <= rsVal)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (questionType == Constants.ServiceQuestionType.DATETIME)
                            {
                                DateTime lsVal = DateTime.MinValue;
                                DateTime rsVal = DateTime.MinValue;
                                if (DateTime.TryParse(lsValue, out lsVal) && DateTime.TryParse(values, out rsVal))
                                {
                                    if (lsVal.Date <= rsVal.Date)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
               ULog.WriteException(ex, "TestCondition for skip in servicewizard");
            }

            return isValid;
        }

        public List<ServiceQuestion> GetBuiltInQuestions()
        {
            List<ServiceQuestion> questions = new List<ServiceQuestion>();
            ServiceQuestion sQuestion = new ServiceQuestion();
            sQuestion.QuestionTitle = "Initiator";
            sQuestion.TokenName = "Initiator";
            sQuestion.QuestionType = Constants.ServiceQuestionType.USERFIELD;
            questions.Add(sQuestion);

            sQuestion = new ServiceQuestion();
            sQuestion.QuestionTitle = "Initiator Manager";
            sQuestion.TokenName = "InitiatorManager";
            sQuestion.QuestionType = Constants.ServiceQuestionType.USERFIELD;
            questions.Add(sQuestion);

            sQuestion = new ServiceQuestion();
            sQuestion.QuestionTitle = "Today Date";
            sQuestion.TokenName = "Today";
            sQuestion.QuestionType = Constants.ServiceQuestionType.DATETIME;
            questions.Add(sQuestion);

            sQuestion = new ServiceQuestion();
            sQuestion.QuestionTitle = "Initiator Location";
            sQuestion.TokenName = "InitiatorLocation";
            sQuestion.QuestionType = Constants.ServiceQuestionType.LOOKUP;
            questions.Add(sQuestion);

            sQuestion = new ServiceQuestion();
            sQuestion.QuestionTitle = "Initiator Department";
            sQuestion.TokenName = "InitiatorDepartment";
            sQuestion.QuestionType = Constants.ServiceQuestionType.LOOKUP;
            questions.Add(sQuestion);

            sQuestion = new ServiceQuestion();
            sQuestion.QuestionTitle = "Initiator Department Manager";
            sQuestion.TokenName = "InitiatorDepartmentManager";
            sQuestion.QuestionType = Constants.ServiceQuestionType.USERFIELD;
            questions.Add(sQuestion);

            sQuestion = new ServiceQuestion();
            sQuestion.QuestionTitle = "Initiator Division Manager";
            sQuestion.TokenName = "InitiatorDivisionManager";
            sQuestion.QuestionType = Constants.ServiceQuestionType.USERFIELD;
            questions.Add(sQuestion);

            sQuestion = new ServiceQuestion();
            sQuestion.QuestionTitle = "Service Owner";
            sQuestion.TokenName = "ServiceOwner";
            sQuestion.QuestionType = Constants.ServiceQuestionType.USERFIELD;
            questions.Add(sQuestion);

            return questions;
        }

        /// <summary>
        /// Parses the question values
        /// </summary>
        /// <param name="questionType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ParseQuestionVal(string questionType, string value, ServiceQuestion question = null)
        {
            if (questionType.ToLower() == "userfield")
            {
                try
                {
                    UserProfileManager userManager = dbContext.UserManager;
                    List<UserProfile> userValCollection = userManager.GetUserInfosById(value);
                    if (userValCollection != null && userValCollection.Count > 0)
                    {
                        List<string> expVal = new List<string>();
                        foreach (UserProfile userVal in userValCollection)
                        {
                                expVal.Add(userVal.Name);
                        }
                        value = string.Join("; ", expVal.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            else if(questionType.ToLower() == Constants.ServiceQuestionType.LOOKUP && questionType.ToLower() != Constants.ServiceQuestionType.REQUESTTYPE && questionType.ToLower() != Constants.ServiceQuestionType.Assets)
            {
                FieldConfigurationManager fieldManager = new FieldConfigurationManager(dbContext);

                value = fieldManager.GetFieldConfigurationData(question.QuestionTypePropertiesDicObj["lookuplist"], value);
                if (!string.IsNullOrWhiteSpace(value))
                    value= Regex.Replace(value, string.Format("[{0}{1}]", Constants.Separator2, Constants.Separator6), Constants.BreakLineSeparator);
            }
            else if (questionType.ToLower() == Constants.ServiceQuestionType.REQUESTTYPE)
            {
                try
                {
                    FieldConfigurationManager fieldManager = new FieldConfigurationManager(dbContext);

                    value = fieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.TicketRequestTypeLookup, value);
                    
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex);
                }
            }
            else if(questionType.ToLower() == Constants.ServiceQuestionType.Assets)
            {
                FieldConfigurationManager fieldManager = new FieldConfigurationManager(dbContext);

                value = fieldManager.GetFieldConfigurationData(DatabaseObjects.Columns.AssetLookup, value);
            }
            else if (questionType.ToLower() == Constants.ServiceQuestionType.MULTICHOICE)
            {
                if (!string.IsNullOrWhiteSpace(value))
                    value = Regex.Replace(value, string.Format("[{0}{1}]", Constants.Separator2, Constants.Separator5), Constants.BreakLineSeparator);
            }
            else if (question != null && question.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME)
            {
                string dateMode = string.Empty;
                question.QuestionTypePropertiesDicObj.TryGetValue("datemode", out dateMode);
                DateTime DateVal = DateTime.MinValue;
                if (DateTime.TryParse(value, out DateVal) && DateVal != DateTime.MinValue)
                {
                    if (!string.IsNullOrEmpty(dateMode) && dateMode.ToLower() == "dateonly")
                        value = UGITUtility.GetDateStringInFormat(DateVal, false);
                    else if (!string.IsNullOrEmpty(dateMode) && dateMode.ToLower() == "timeonly")
                        value = DateVal.ToString("HH:mm");
                    else
                        value = UGITUtility.GetDateStringInFormat(DateVal, true);
                }
            }
            return value;
        }
        public Department GetDepartment(int departmentID)
        {
            DepartmentManager departmanager = new DepartmentManager(dbContext);
            Department selectedDepartment = departmanager.LoadByID(departmentID);
            //Manager.Department selectedDepartment = allDepartments.FirstOrDefault(x => x.ID == departmentID);
            return selectedDepartment;
        }
        
        public string ParseQuestionVal(ServiceQuestion question, string value)
        {
            value = ParseQuestionVal(question.QuestionType, value, question);
            if (!string.IsNullOrEmpty(value))
            {
                if (question.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME)
                {
                    string dateMode = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("datemode", out dateMode);
                    DateTime DateVal = DateTime.MinValue;
                    DateTime.TryParse(value, out DateVal);
                    if (DateVal != DateTime.MinValue)
                    {
                        if (!string.IsNullOrEmpty(dateMode) && dateMode.ToLower() == "dateonly")
                            value = UGITUtility.GetDateStringInFormat(DateVal, false);
                        else
                            value = UGITUtility.GetDateStringInFormat(DateVal, true);
                    }
                }
            }

            return value;
        }

        public void SetQuestionId()
        {
            //ID = 0;
        }
    }
    interface IServiceQuestionManager: IManagerBase<ServiceQuestion>
    {

    }
}

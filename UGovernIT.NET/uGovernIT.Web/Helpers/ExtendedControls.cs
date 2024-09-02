using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public static class ExtendedControls
    {
        public static Control GenerateControlForQuestion(ServiceQuestion question, List<ServiceSectionCondition> skipConditions)
        {
            ApplicationContext dbContext = HttpContext.Current.GetManagerContext();
            ModuleViewManager moduleViewManager = new ModuleViewManager(dbContext);

            Panel ctrPanel = new Panel();
            ctrPanel.ID = string.Format("question_{0}_{1}_Panel", question.ServiceSectionID, question.ID);
            UserProfile currentUser = dbContext.CurrentUser;
            UserProfileManager userManager = dbContext.UserManager;
            ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(dbContext);

            string questionType = question.QuestionType.ToLower();
            string defaultVal = string.Empty;
            string defaultMandatoryErrorMsg = "Please specify";

            TextBox tempBoxForValidation = new TextBox();
            tempBoxForValidation.Style.Add(HtmlTextWriterStyle.Display, "none");
            tempBoxForValidation.CssClass += string.Format(" hiddenCtr hiddenCtr_{0}", question.ID);
            tempBoxForValidation.ID = string.Format("txthiddenCtr_{0}", question.ID);

            if (question.FieldMandatory && skipConditions != null && skipConditions.Exists(x => x.ConditionValidate && x.SkipQuestionsID.Exists(y => y == question.ID)))
            {
                tempBoxForValidation.Text = "[$skip$]";
            }
            switch (questionType)
            {
                case Constants.ServiceQuestionType.TEXTBOX:
                case Constants.ServiceQuestionType.Number:
                    {
                        TextBox txtCtr = new TextBox();

                        txtCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        //txtCtr.ID = "usr";
                        ctrPanel.Controls.Add(txtCtr);
                        // txtCtr.CssClass = "textboxctr";
                        txtCtr.CssClass = "form-control bg-light-blue";
                        //txtCtr.Attributes.Add("placeholder", question.Helptext);
                        txtCtr.Attributes.Add("ctype", "textbox");

                        string txtMode = string.Empty;




                        question.QuestionTypePropertiesDicObj.TryGetValue("textmode", out txtMode);
                        defaultVal = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("defaultval", out defaultVal);

                        string maxlength = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("maxlength", out maxlength);
                        if (!string.IsNullOrEmpty(maxlength) && UGITUtility.StringToInt(maxlength) > 0)
                        {
                            txtCtr.MaxLength = UGITUtility.StringToInt(maxlength);
                        }
                        if (txtMode != null)
                        {
                            if (txtMode.ToLower() == "multiline")
                            {
                                txtCtr.TextMode = TextBoxMode.MultiLine;
                                txtCtr.Rows = 5;
                            }

                            else if (txtMode.ToLower() == "integer")
                            {
                                RegularExpressionValidator numberValidator = new RegularExpressionValidator();
                                numberValidator.ControlToValidate = txtCtr.ID;
                                numberValidator.ErrorMessage = "Incorrect entry, please enter whole number";
                                numberValidator.CssClass = "errormsg-container";
                                numberValidator.ValidationExpression = "^\\d+$";
                                ctrPanel.Controls.Add(numberValidator);
                            }
                            else if (txtMode.ToLower() == "double")
                            {
                                RegularExpressionValidator numberValidator = new RegularExpressionValidator();
                                numberValidator.ControlToValidate = txtCtr.ID;
                                numberValidator.ErrorMessage = "Incorrect entry, please use number format xxx.xx";
                                numberValidator.CssClass = "errormsg-container";
                                numberValidator.ValidationExpression = "^\\d*\\.?\\d*$";
                                ctrPanel.Controls.Add(numberValidator);
                            }

                            if (question.FieldMandatory)
                            {

                                ctrPanel.Controls.Add(tempBoxForValidation);

                                RequiredFieldValidator reqValidator = new RequiredFieldValidator();
                                reqValidator.ControlToValidate = tempBoxForValidation.ID;
                                reqValidator.ErrorMessage = defaultMandatoryErrorMsg;
                                //reqValidator.ErrorMessage = question.Helptext;
                                reqValidator.CssClass = "errormsg-container";
                                reqValidator.Display = ValidatorDisplay.Dynamic;
                                ctrPanel.Controls.Add(reqValidator);

                                //// if control is for EMAIL
                                //// then use regular expression validator and add regular expression to validate email
                                if ("Email".EqualsIgnoreCase(question.TokenName) && "Registration".EqualsIgnoreCase(question.ServiceSectionName))
                                {
                                    RegularExpressionValidator emailValidator = new RegularExpressionValidator();
                                    emailValidator.ControlToValidate = txtCtr.ID;
                                    emailValidator.ErrorMessage = "Invalid Email Id";
                                    emailValidator.CssClass = "errormsg-container";
                                    emailValidator.ValidationExpression = "^((\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*)\\s*[;]{0,1}\\s*)+$";
                                    ctrPanel.Controls.Add(emailValidator);

                                    CustomValidator customValidator = new CustomValidator();
                                    customValidator.ControlToValidate = txtCtr.ID;
                                    customValidator.ErrorMessage = "Email already exists for a Registered Trial";
                                    customValidator.CssClass = "errormsg-container";
                                    customValidator.ServerValidate += CustomValidator_ServerValidate_EmailRegistration;
                                    ctrPanel.Controls.Add(customValidator);
                                }
                                //// end email validation
                                ///if control is Company name
                                ///
                                if ("companyName".EqualsIgnoreCase(question.TokenName) && "Registration".EqualsIgnoreCase(question.ServiceSectionName))
                                {

                                    CustomValidator customValidator = new CustomValidator();
                                    customValidator.ControlToValidate = txtCtr.ID;
                                    customValidator.ErrorMessage = "Company Name already exists";
                                    customValidator.CssClass = "errormsg-container";
                                    customValidator.ServerValidate += CustomValidator_ServerValidate_CompanyRegistration;
                                    ctrPanel.Controls.Add(customValidator);
                                }

                            }

                        }

                        // txtCtr.Width = new Unit("97%");
                        txtCtr.Text = defaultVal;
                        txtCtr.Attributes.Add("onkeyup", string.Format("questionBrachLogic(this,'{0}', 'textbox', \"{1}\")", questionType, question.TokenName));
                        txtCtr.Attributes.Add("onchange", string.Format("questionBrachLogic(this,'{0}', 'textbox', \"{1}\")", questionType, question.TokenName));
                    }
                    break;
                case Constants.ServiceQuestionType.SINGLECHOICE:
                    #region SINGLECHOICE
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

                    defaultVal = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("defaultval", out defaultVal);

                    if (UGITUtility.StringToBoolean(pickFromField))
                    {
                        FieldConfigurationManager fManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                        DataTable dt = fManager.GetFieldDataByFieldName(optionPickFromField);
                        if (dt != null)
                        {
                            foreach (DataRow option in dt.Rows)
                            {
                                items.Add(new ListItem(Convert.ToString(option["ID"]), Convert.ToString(option["Title"])));
                            }
                        }
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

                    if (defaultVal != null)
                    {
                        ListItem lItem = items.FirstOrDefault(x => x.Value.ToLower() == defaultVal.ToLower());
                        if (lItem != null)
                        {
                            lItem.Selected = true;
                        }
                    }

                    if (optionType.ToLower() == "radiobuttons")
                    {
                        foreach (ListItem item in items)
                        {
                            item.Attributes.Add("onclick", string.Format("questionBrachLogic(this,'{0}','radiobuttonlist',\"{1}\")", questionType, question.TokenName));
                            item.Attributes.Add("onblur", string.Format("questionBrachLogic(this,'{0}','radiobuttonlist',\"{1}\")", questionType, question.TokenName));
                        }

                        RadioButtonList ddlCtr = new RadioButtonList();
                        ddlCtr.CssClass = "radiodropdownctr equ_details_radiobtn";
                        ddlCtr.Items.AddRange(items.ToArray());
                        ddlCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        //ddlCtr.ToolTip = question.Helptext;
                        ctrPanel.Controls.Add(ddlCtr);

                        if (question.FieldMandatory)
                        {

                            ctrPanel.Controls.Add(tempBoxForValidation);

                            RequiredFieldValidator reqValidator = new RequiredFieldValidator();
                            //   reqValidator.ControlToValidate = ddlCtr.ID;
                            reqValidator.ControlToValidate = tempBoxForValidation.ID;
                            reqValidator.ErrorMessage = defaultMandatoryErrorMsg;
                            reqValidator.CssClass = "errormsg-container";
                            reqValidator.Display = ValidatorDisplay.Dynamic;
                            ctrPanel.Controls.Add(reqValidator);
                        }
                    }
                    else
                    {
                        ASPxComboBox ddlCtr = new ASPxComboBox();
                        ddlCtr.DropDownStyle = DropDownStyle.DropDown;
                        ddlCtr.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                        ddlCtr.CssClass = "dropdownctr aspxDropDownList  itsmDropDownList svcDropDownlist";
                        ddlCtr.CssClass += string.Format(" ddlCtr_{0}", question.ID);
                        ddlCtr.Width = Unit.Pixel(300);
                        ddlCtr.ClientSideEvents.Validation = string.Format("function(s,e){2}questionBrachLogic(s,'{0}','devexdropdownlist',\"{1}\");{3}", questionType, question.TokenName, "{", "}");
                        ddlCtr.ClientSideEvents.Init = string.Format("function(s,e){2}questionBrachLogic(s,'{0}','devexdropdownlist',\"{1}\");{3}", questionType, question.TokenName, "{", "}");
                        ddlCtr.ClientSideEvents.ValueChanged = string.Format("function(s,e){2}questionBrachLogic(s,'{0}','devexdropdownlist',\"{1}\");{3}", questionType, question.TokenName, "{", "}");
                        ddlCtr.CssClass = questionType == "singlechoice" ? ddlCtr.CssClass = ddlCtr.CssClass + " dropdownForSingleChoice" : ddlCtr.CssClass;
                        ddlCtr.Items.AddRange(items.ToArray());
                        ddlCtr.Items.Insert(0, new ListEditItem("(None)", string.Empty));
                        int maxDropDownRows = 25;
                        ddlCtr.SelectedItem = ddlCtr.Items.FindByValue(defaultVal);
                        ddlCtr.DropDownRows = items.Count > maxDropDownRows ? maxDropDownRows : items.Count + 1;
                        ddlCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        //ddlCtr.HelpText = question.Helptext;
                        ctrPanel.Controls.Add(ddlCtr);

                        if (question.FieldMandatory)
                        {
                            ctrPanel.Controls.Add(tempBoxForValidation);
                            RequiredFieldValidator reqValidator = new RequiredFieldValidator();
                            reqValidator.ControlToValidate = tempBoxForValidation.ID;
                            reqValidator.ErrorMessage = defaultMandatoryErrorMsg;
                            reqValidator.CssClass = "errormsg-container";
                            reqValidator.Display = ValidatorDisplay.Dynamic;
                            ctrPanel.Controls.Add(reqValidator);
                        }
                    }
                    break;
                #endregion SINGLECHOICE
                case Constants.ServiceQuestionType.MULTICHOICE:
                    #region MULTICHOICE
                    List<ListItem> optList = new List<ListItem>();
                    string mOptions = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("options", out mOptions);
                    string moptiontype = "dropdownctr";
                    question.QuestionTypePropertiesDicObj.TryGetValue("moptiontype", out moptiontype);
                    defaultVal = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("defaultval", out defaultVal);
                    string optionPickFromFieldMC = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("selectedfield", out optionPickFromFieldMC);

                    string pickFromFieldMC = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("pickfromfield", out pickFromFieldMC);

                    string SelectedListMC = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("selectedlist", out SelectedListMC);
                    if (!string.IsNullOrEmpty(pickFromFieldMC))
                    {
                        FieldConfigurationManager fManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                        DataTable dt = fManager.GetFieldDataByFieldName(optionPickFromFieldMC);
                        if (dt != null)
                        {
                            foreach (DataRow option in dt.Rows)
                            {
                                optList.Add(new ListItem(Convert.ToString(option["ID"]), Convert.ToString(option["Title"])));
                            }
                        }
                    }
                    else if (mOptions != null)
                    {
                        List<string> optionList = UGITUtility.ConvertStringToList(mOptions, new string[] { Constants.Separator1, Constants.Separator2 });
                        foreach (string option in optionList)
                        {
                            ListItem lItem = new ListItem(option, option);
                            optList.Add(lItem);
                            lItem.Attributes.Add("onclick", string.Format("questionBrachLogic(this,'{0}','checkboxlist',\"{1}\")", questionType, question.TokenName));
                            lItem.Attributes.Add("onblur", string.Format("questionBrachLogic(this,'{0}','radiobuttonlist',\"{1}\")", questionType, question.TokenName));
                        }
                    }
                    if (defaultVal != null)
                    {
                        List<string> defaultValList = UGITUtility.ConvertStringToList(defaultVal, Constants.Separator6);
                        if (defaultValList != null && defaultValList.Count > 0)
                        {
                            for (int i = 0; i < defaultValList.Count; i++)
                            {
                                ListItem lItem = optList.FirstOrDefault(x => x.Value.ToLower() == defaultValList[i].Trim().ToLower());
                                if (lItem != null)
                                {
                                    lItem.Selected = true;
                                }
                            }
                        }
                    }
                    if (moptiontype != null && moptiontype.ToLower() == "dropdowngrid")
                    {
                        ASPxGridLookup gridLookup = new ASPxGridLookup();
                        GridViewDataColumn column = new GridViewDataColumn();
                        column.FieldName = "Title";
                        column.Caption = "Title";
                        gridLookup.Columns.Add(column);
                        GridViewCommandColumn commandCol = new GridViewCommandColumn();
                        commandCol.ShowSelectCheckbox = true;
                        commandCol.VisibleIndex = 0;
                        gridLookup.GridView.Width = Unit.Pixel(180);
                        gridLookup.Columns.Add(commandCol);
                        gridLookup.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        gridLookup.GridViewProperties.Settings.ShowFilterRow = true;
                        gridLookup.AutoGenerateColumns = false;
                        gridLookup.AutoPostBack = false;
                        gridLookup.CssClass = "gridLookupctr";
                        gridLookup.GridView.SettingsPager.PageSize = 15;
                        gridLookup.SelectionMode = GridLookupSelectionMode.Multiple;
                        gridLookup.Attributes.Add("optList", string.Join(",", optList));
                        gridLookup.ClientInstanceName = "multichoicedropdowngrid";
                        gridLookup.GridView.ClientSideEvents.Init = string.Format("function(s,e){2}questionBrachLogic(s,'{0}','multichoicedropdowngridChange',\"{1}\");{3}", questionType, question.TokenName, "{", "}");
                        gridLookup.GridView.ClientSideEvents.SelectionChanged = "function(s,e){onmultichoicedropdowngridChange(s,e)}";
                        if (!string.IsNullOrWhiteSpace(defaultVal))
                        {
                            List<string> defaultValList = UGITUtility.ConvertStringToList(defaultVal, Constants.Separator6);
                            defaultValList.ForEach(x => gridLookup.GridView.Selection.SelectRowByKey(x));
                        }
                        gridLookup.Load += GridLookup_Load;
                        ctrPanel.Controls.Add(gridLookup);
                    }
                    else
                    {
                        //Check checkbox list for multi choice 
                        CheckBoxList ddlMCtr = new CheckBoxList();
                        ddlMCtr.Width = Unit.Pixel(180);
                        ddlMCtr.Attributes.Add("ctype", "multichoice");
                        ddlMCtr.CssClass = "checkboxdropdownctr";
                        ddlMCtr.Items.AddRange(optList.ToArray());
                        ddlMCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        //ddlMCtr.ToolTip = question.Helptext;
                        ctrPanel.Controls.Add(ddlMCtr);
                    }


                    if (question.FieldMandatory)
                    {
                        ctrPanel.Controls.Add(tempBoxForValidation);

                        RequiredFieldValidator reqValidator = new RequiredFieldValidator();
                        reqValidator.ControlToValidate = tempBoxForValidation.ID;
                        reqValidator.ErrorMessage = defaultMandatoryErrorMsg;
                        reqValidator.CssClass = "errormsg-container";
                        reqValidator.Display = ValidatorDisplay.Dynamic;
                        ctrPanel.Controls.Add(reqValidator);
                    }
                    break;
                #endregion MULTICHOICE
                case Constants.ServiceQuestionType.CHECKBOX:
                    CheckBox checkboxCtr = new CheckBox();
                    checkboxCtr.CssClass = "checkboxctr";
                    checkboxCtr.Attributes.Add("ctype", "checkbox");
                    defaultVal = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("defaultval", out defaultVal);
                    checkboxCtr.Checked = UGITUtility.StringToBoolean(defaultVal);
                    checkboxCtr.Attributes.Add("onclick", string.Format("questionBrachLogic(this,'{0}','checkbox',\"{1}\")", questionType, question.TokenName));
                    checkboxCtr.Attributes.Add("onblur", string.Format("questionBrachLogic(this,'{0}','checkbox',\"{1}\")", questionType, question.TokenName));
                    //checkboxCtr.ToolTip = question.Helptext;
                    checkboxCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);


                    ctrPanel.Controls.Add(checkboxCtr);
                    break;
                case Constants.ServiceQuestionType.DATETIME:
                    #region DATETIME
                    //Creating datetime control with mandate conditions
                    ASPxDateEdit datetimeCtr = new ASPxDateEdit();
                    datetimeCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                    datetimeCtr.Width = Unit.Pixel(300);
                    datetimeCtr.AutoPostBack = false;
                    datetimeCtr.MinDate = new DateTime(1800, 1, 1);
                    datetimeCtr.CssClass = "CRMDueDate_inputField svcDateField";
                    datetimeCtr.DropDownButton.Image.Url = "/Content/Images/calendarNew.png";
                    datetimeCtr.DropDownButton.Image.Width = 18;
                    // datetimeCtr.OnValueChangeClientScript = string.Format("questionBrachLogic(this,'{0}','textbox',\"{1}\")", questionType, question.TokenName);
                    datetimeCtr.ClientSideEvents.ValueChanged = "function(s,e){questionBrachLogic(s,'" + questionType + "','textbox','" + question.TokenName + "')}";

                    string dateMode = string.Empty;
                    defaultVal = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("datemode", out dateMode);
                    question.QuestionTypePropertiesDicObj.TryGetValue("defaultval", out defaultVal);
                    string validateagainst = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("validateagainst", out validateagainst);
                    //datetimeCtr.HelpText = question.Helptext;
                    datetimeCtr.ValidateRequestMode = ValidateRequestMode.Disabled;

                    string dateconstraint = string.Empty;
                    string pastdate = string.Empty;
                    string futuredate = string.Empty;
                    string presentdate = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("dateconstraint", out dateconstraint);
                    question.QuestionTypePropertiesDicObj.TryGetValue("pastdate", out pastdate);
                    question.QuestionTypePropertiesDicObj.TryGetValue("futuredate", out futuredate);
                    question.QuestionTypePropertiesDicObj.TryGetValue("presentdate", out presentdate);

                    //To fix BTS-21-000674: SVC Desired Completion Date Mapped not populating (for Todays date)
                    DateTime dateTime = dateMode == "DateOnly" ? DateTime.Today : DateTime.Now;

                    if (pastdate != null && !UGITUtility.StringToBoolean(pastdate))
                        datetimeCtr.MinDate = dateTime;
                    if (futuredate != null && !UGITUtility.StringToBoolean(futuredate))
                        datetimeCtr.MaxDate = dateTime;
                    if (presentdate != null && !UGITUtility.StringToBoolean(presentdate))
                        datetimeCtr.Date = dateTime;

                    ctrPanel.Controls.Add(datetimeCtr);


                    CustomValidator reqDateCValidator = new CustomValidator();
                    if (!string.IsNullOrEmpty(dateconstraint))
                        reqDateCValidator.Attributes.Add("dateconstraint", dateconstraint);
                    if (!string.IsNullOrEmpty(pastdate))
                        reqDateCValidator.Attributes.Add("pastdate", pastdate);
                    if (!string.IsNullOrEmpty(futuredate))
                        reqDateCValidator.Attributes.Add("futuredate", futuredate);
                    if (!string.IsNullOrEmpty(presentdate))
                        reqDateCValidator.Attributes.Add("presentdate", presentdate);

                    reqDateCValidator.ControlToValidate = datetimeCtr.ID;// string.Format("{0}${0}Date", datetimeCtr.ID);
                    if (!string.IsNullOrEmpty(validateagainst) && validateagainst.ToLower() != "currentdate")
                        reqDateCValidator.Attributes.Add("validateagainst", string.Format("question_{0}_{1}", question.ServiceSectionID, validateagainst));
                    reqDateCValidator.ServerValidate += reqDateCValidator_ServerValidate;
                    reqDateCValidator.CssClass = "errormsg-container";
                    reqDateCValidator.Display = ValidatorDisplay.Dynamic;
                    ctrPanel.Controls.Add(reqDateCValidator);
                    //DateTime Questions - add validation for Present ,Past,Future  :End

                    if (dateMode != null)
                    {
                        if (dateMode.ToLower() == "dateonly")
                        {
                            datetimeCtr.EditFormat = EditFormat.Date;
                        }
                        else if (dateMode.ToLower() == "timeonly")
                        {
                            datetimeCtr.EditFormat = EditFormat.Time;
                        }
                    }


                    //Set default value 
                    if (!string.IsNullOrEmpty(defaultVal))
                    {
                        defaultVal = defaultVal.ToLower();
                        if (defaultVal.Contains("[today]"))
                        {
                            defaultVal = defaultVal.Replace("[today]", string.Format("[{0}]", DateTime.UtcNow.ToString()));
                            datetimeCtr.Date = DateTime.UtcNow;
                        }
                        //Evaluate function 
                        if (defaultVal.Contains("f:"))
                        {
                            object tempDate = ExpressionCalc.ExecuteFunctions(dbContext, defaultVal);
                            DateTime endDate = UGITUtility.StringToDateTime(tempDate);
                            if (endDate != DateTime.MinValue)
                                datetimeCtr.Date = endDate;
                        }
                        else
                        {
                            DateTime dateDefaultValue;
                            if (DateTime.TryParse(defaultVal, out dateDefaultValue))
                            {
                                datetimeCtr.Date = dateDefaultValue;
                            }
                            else
                            {
                                DateTime dtDefaultVal = DateTime.Now.AddDays(UGITUtility.StringToDouble(defaultVal));
                                if (dtDefaultVal != null)
                                    datetimeCtr.Date = dtDefaultVal;
                            }
                        }
                    }

                    ctrPanel.Controls.Add(tempBoxForValidation);
                    if (question.FieldMandatory)
                    {
                        RequiredFieldValidator reqValidator = new RequiredFieldValidator();
                        reqValidator.ControlToValidate = datetimeCtr.ID;
                        reqValidator.ErrorMessage = defaultMandatoryErrorMsg;
                        reqValidator.CssClass = "errormsg-container";
                        reqValidator.Display = ValidatorDisplay.Dynamic;
                        ctrPanel.Controls.Add(reqValidator);
                    }
                    //end 
                    break;
                #endregion DATETIME
                case Constants.ServiceQuestionType.USERFIELD:
                    #region userfield
                    UserValueBox userCtr = new UserValueBox();
                    string questionBrachLogic = string.Format("questionBrachLogic(s, '{0}', 'userfield', \"{1}\")", questionType, question.TokenName);
                    userCtr.UserTokenBoxAdd.ClientSideEvents.ValueChanged = "function(s,e){" + questionBrachLogic + "}";
                    userCtr.UserTokenBoxAdd.ClientSideEvents.Validation = "function(s,e){" + questionBrachLogic + "}";
                    userCtr.UserTokenBoxAdd.CssClass = "userctr userValueBox-dropDown";
                    defaultVal = string.Empty;

                    question.QuestionTypePropertiesDicObj.TryGetValue("defaultval", out defaultVal);

                    string specificusergroup = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("specificusergroup", out specificusergroup);

                    string userType = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("usertype", out userType);

                    bool isManager = false;
                    string groupName = string.Empty;

                    if (!string.IsNullOrEmpty(defaultVal))
                    {
                        userCtr.SetValues(defaultVal);
                    }


                    if (!string.IsNullOrEmpty(userType) && userType == "3")
                    {
                        userCtr.SetValues(currentUser.Id);
                    }

                    if (!string.IsNullOrEmpty(userType) && userType == "4")
                        isManager = true;
                    else if (!string.IsNullOrEmpty(userType) && userType == "5")
                        groupName = specificusergroup;


                    if (!string.IsNullOrEmpty(groupName))
                    {
                        ASPxComboBox ddlUsers = new ASPxComboBox();
                        ddlUsers.Width = Unit.Pixel(200);
                        ddlUsers.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        ddlUsers.ClientSideEvents.SelectedIndexChanged = "function(s, e) {changeUsers(s,e);}";
                        ddlUsers.ClientInstanceName = "ddlUsers";
                        string[] grps = groupName.Split(new string[] { Constants.Separator6 }, StringSplitOptions.RemoveEmptyEntries);
                        //SPFieldUserValueCollection usera = new SPFieldUserValueCollection();
                        List<UserProfile> lstUsers = new List<UserProfile>();
                        List<UserProfile> lstGrpUsers = new List<UserProfile>();

                        lstGrpUsers = lstGrpUsers.ToArray().Except(lstUsers.ToArray()).ToList<UserProfile>();
                        lstUsers.AddRange(lstGrpUsers.ToList<UserProfile>());
                        ddlUsers.ValueField = "ID";
                        ddlUsers.TextField = "Name";
                        ddlUsers.CssClass = "dropdownctrUsers";
                        ddlUsers.DataSource = lstUsers;
                        ddlUsers.Items.Add(new ListEditItem("<Select User>", 0));
                        ddlUsers.DataBind();
                        //ddlUsers.HelpText = question.Helptext;
                        ctrPanel.Controls.Add(ddlUsers);
                    }
                    else
                    {
                        //userCtr.PrincipalSource = SPPrincipalSource.UserInfoList;

                        if (userType == "0")//any
                            userCtr.SelectionSet = "";
                        else if (userType == "1")//users only
                            userCtr.SelectionSet = "User";
                        else if (userType == "2")//groups only
                            userCtr.SelectionSet = "Group";
                        else if (userType == "3")//logged in user
                            userCtr.SelectionSet = "User";
                        else if (userType == "4")//managers only
                            userCtr.SelectionSet = "User,Manager";

                        userCtr.Attributes.Add("ugselectionset", userCtr.SelectionSet);

                        string strIsMulti = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("singleentryonly", out strIsMulti);
                        userCtr.isMulti = string.IsNullOrEmpty(strIsMulti) ? true : !UGITUtility.StringToBoolean(strIsMulti);
                        userCtr.Width = Unit.Pixel(300);
                        userCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        userCtr.UserTokenBoxAdd.ClientSideEvents.ValueChanged = "function(s,e){" + string.Format("FillOnUserFieldChange(\"{0}\");", userCtr.ID) + questionBrachLogic + "}";
                        userCtr.UserTokenBoxAdd.ClientSideEvents.TextChanged = "function(s,e){" + string.Format("FillOnUserFieldChange(\"{0}\");", userCtr.ID) + questionBrachLogic + "}";
                        //userCtr.UserTokenBoxAdd.HelpText = question.Helptext;
                        userCtr.UserTokenBoxAdd.CssClass = ".questiondiv_" + question.ID + " userctr";
                        ctrPanel.Controls.Add(userCtr);
                    }
                    if (question.FieldMandatory)
                    {
                        ctrPanel.Controls.Add(tempBoxForValidation);

                        CustomValidator reqValidator = new CustomValidator();
                        reqValidator.ID = string.Format("question_{0}_{1}__{0}{1}_error", question.ServiceSectionID, question.ID);
                        reqValidator.ControlToValidate = tempBoxForValidation.ID;
                        reqValidator.ValidateEmptyText = true;
                        reqValidator.ServerValidate += cvUserField_ServerValidate;
                        //reqValidator.ErrorMessage = defaultFieldErrorMsg;
                        reqValidator.Display = ValidatorDisplay.Dynamic;
                        reqValidator.CssClass = "errormsg-container";
                        reqValidator.Attributes.Add("isManager", Convert.ToString(isManager));
                        reqValidator.Attributes.Add("GroupName", groupName);
                        ctrPanel.Controls.Add(reqValidator);
                    }
                    #endregion
                    break;

                case Constants.ServiceQuestionType.LOOKUP:
                    #region LOOKUP
                    questionBrachLogic = string.Format("questionBrachLogic(s, '{0}', 'userfield', \"{1}\")", questionType, question.TokenName);

                    string listName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("lookuplist", out listName);

                    string listfield = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("lookupfield", out listfield);

                    string lpModuleName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("module", out lpModuleName);

                    string lookupDefaultVal = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("defaultval", out lookupDefaultVal);

                    bool isMulti = false;
                    string isMultiS = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("multi", out isMultiS);
                    isMulti = UGITUtility.StringToBoolean(isMultiS);

                    FieldConfigurationManager configFieldManager = new FieldConfigurationManager(dbContext);
                    FieldConfiguration configField = configFieldManager.GetFieldByFieldName(listName);

                    string fieldColumnType = string.Empty;

                    if (configField != null)
                        fieldColumnType = Convert.ToString(configField.Datatype);
                    else
                        fieldColumnType = "System.String";

                    if (configField != null)
                    {
                        if (!string.IsNullOrWhiteSpace(configField.TemplateType) && isMulti == true)
                        {
                            string editcss1 = "field_" + configField.FieldName.ToLower() + "_edit";
                            LookupValueBoxEdit dropDownBoxEdit = new LookupValueBoxEdit();
                            dropDownBoxEdit.dropBox.AutoPostBack = false;
                            dropDownBoxEdit.Width = Unit.Pixel(300);
                            //dropDownBoxEdit.dropBox.CssClass = editcss1 + " down-arrow text-left all-input form-control btn btn-default dropdown-toggle bg-light-blue";
                            dropDownBoxEdit.dropBox.CssClass = editcss1 + " all-input form-control overrideFontSize";
                            dropDownBoxEdit.ModuleName = lpModuleName;
                            dropDownBoxEdit.FieldName = configField.FieldName;
                            dropDownBoxEdit.IsMulti = isMulti;
                            dropDownBoxEdit.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                            dropDownBoxEdit.dropBox.ClientSideEvents.ValueChanged = "function(s,e){" + questionBrachLogic + "}";
                            dropDownBoxEdit.dropBox.ClientSideEvents.Validation = "function(s,e){" + questionBrachLogic + "}";
                            dropDownBoxEdit.CssClass += " lookupValueBox-edit";
                            //dropDownBoxEdit.dropBox.HelpText = question.Helptext;

                            if (listName.ToLower() == "departmentlookup")
                            {
                                dropDownBoxEdit.CssClass += " DependendentQuestionDepartment";
                                string departmentuservalue = lookupDefaultVal;
                                question.QuestionTypePropertiesDicObj.TryGetValue("departmentuserquestion", out departmentuservalue);
                                if (!string.IsNullOrEmpty(lookupDefaultVal))
                                    departmentuservalue = lookupDefaultVal;
                                if (!string.IsNullOrEmpty(departmentuservalue))
                                {
                                    dropDownBoxEdit.Attributes.Add("DependendentQuestionUser", departmentuservalue);
                                    dropDownBoxEdit.Attributes.Add("class", " " + departmentuservalue);
                                }
                                if (!string.IsNullOrEmpty(lookupDefaultVal))
                                {
                                    dropDownBoxEdit.SetValues(departmentuservalue);
                                    dropDownBoxEdit.DefaultValue = departmentuservalue;
                                    dropDownBoxEdit.Value = departmentuservalue;
                                }
                            }

                            ctrPanel.Controls.Add(dropDownBoxEdit);
                        }
                        else
                        {
                            LookUpValueBox lookup = new LookUpValueBox();
                            lookup.Width = Unit.Pixel(300);
                            lookup.FieldName = configField.FieldName;
                            lookup.devexListBox.ClientSideEvents.TextChanged = "function(s,e){" + questionBrachLogic + "}";
                            lookup.devexListBox.ClientSideEvents.Validation = "function(s,e){" + questionBrachLogic + "}";
                            lookup.devexListBox.ClientSideEvents.ValueChanged = "function(s,e){" + questionBrachLogic + "}";
                            lookup.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                            lookup.devexListBox.ID = string.Format("question_{0}_{1}_lookup", question.ServiceSectionID, question.ID);
                            lookup.CssClass = "lookupValueBox-dropown";
                            //lookup.devexListBox.NullText = question.Helptext;

                            if (listName.ToLower() == "locationlookup")
                            {
                                string locationUservalue = string.Empty;
                                question.QuestionTypePropertiesDicObj.TryGetValue("locationuserquestion", out locationUservalue);
                                if (!string.IsNullOrEmpty(lookupDefaultVal))
                                    locationUservalue = lookupDefaultVal;
                                if (!string.IsNullOrWhiteSpace(locationUservalue))
                                {
                                    lookup.Attributes.Add("LocationQuestionUser", locationUservalue);
                                    lookup.CssClass += " LocationQuestionUser";
                                }
                                if (!string.IsNullOrEmpty(lookupDefaultVal))
                                {
                                    lookup.SetValues(locationUservalue);
                                    lookup.DefaultValue = locationUservalue;
                                }
                            }
                            lookup.ModuleName = lpModuleName;
                            lookup.IsMulti = isMulti;

                            ctrPanel.Controls.Add(lookup);

                        }
                        if (question.FieldMandatory)
                        {

                            ctrPanel.Controls.Add(tempBoxForValidation);
                            RequiredFieldValidator reqValidator = new RequiredFieldValidator();
                            reqValidator.ControlToValidate = tempBoxForValidation.ID;
                            reqValidator.ErrorMessage = defaultMandatoryErrorMsg;
                            reqValidator.Display = ValidatorDisplay.Dynamic;
                            reqValidator.CssClass = "errormsg-container";
                            ctrPanel.Controls.Add(reqValidator);
                        }
                    }

                    break;
                #endregion LOOKUP
                case Constants.ServiceQuestionType.REQUESTTYPE:
                    #region REQUESTTYPE

                    
                    string dashboardType = string.Empty;
                    questionBrachLogic = string.Format("questionBrachLogic(s, '{0}', 'requesttype', \"{1}\")", questionType, question.TokenName);
					// this code block required to validate skip logic when do next & previous because AspcDropDownEdit does not fire ValueChanged and validate event if set through programmig 
                    ASPxTextBox aSPxTextBox = new ASPxTextBox();
                    aSPxTextBox.ClientSideEvents.Init = "function(s,e){" + questionBrachLogic + "}";
                    aSPxTextBox.ClientVisible = false;
                    
                    question.QuestionTypePropertiesDicObj.TryGetValue("dropdowntype", out dashboardType);

                    string showCategoryDropDown = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("enablecategorydropdown", out showCategoryDropDown);
                    bool enableCategoryDropdown = UGITUtility.StringToBoolean(showCategoryDropDown);

                    string showIssueDropDown = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("enableissuetypedropdown", out showIssueDropDown);
                    bool enableIssueDropdown = UGITUtility.StringToBoolean(showIssueDropDown);

                    string requestTypes = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("requesttypes", out requestTypes);
                    string moduleName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("module", out moduleName);

                    string requestTypeDefaultVal = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("defaultval", out requestTypeDefaultVal);

                    RequestTypeCustomProperties customPropertiesObj = new RequestTypeCustomProperties();
                    customPropertiesObj.RequestTypesList = UGITUtility.ConvertStringToList(requestTypes, Constants.Separator2);
                    customPropertiesObj.enableissuetypedropdown = enableIssueDropdown;

                    //pRequestTypeCtr.ID = string.Format("question_requesttype_{0}_{1}", question.ServiceSectionID, question.ID);
                    ctrPanel.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID) + "IssueType";
                    LookupValueBoxEdit dropDownBox = new LookupValueBoxEdit();
                    dropDownBox.Attributes.Add("ctype", "requesttype");
                    string editcss = "field_" + DatabaseObjects.Columns.TicketRequestTypeLookup.ToLower() + "_edit";
                    dropDownBox.dropBox.CssClass = editcss;
                    dropDownBox.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                    dropDownBox.dropBox.ClientSideEvents.ValueChanged = "function(s,e){" + questionBrachLogic + "}";
                    dropDownBox.dropBox.ClientSideEvents.Validation = "function(s,e){" + questionBrachLogic + "}";
                    //dropDownBox.dropBox.ClientSideEvents.TextChanged = "function(s,e){" + questionBrachLogic + "}";
                    dropDownBox.CustomParameters = customPropertiesObj;
                    dropDownBox.isRequestType = true;
                    dropDownBox.ModuleName = moduleName;
                    dropDownBox.FieldName = DatabaseObjects.Columns.TicketRequestTypeLookup;
                    dropDownBox.gridView.SettingsBehavior.AllowSelectByRowClick = true;
                    dropDownBox.gridView.ClientSideEvents.SelectionChanged = "requestTypeSelectionChanged";
                    dropDownBox.gridView.CssClass = editcss + "gview";
                    dropDownBox.gridView.Width = Unit.Percentage(100);
                    dropDownBox.gridView.EnableCallBacks = true;
                    if (!string.IsNullOrEmpty(requestTypeDefaultVal))
                    {
                        dropDownBox.SetValues(requestTypeDefaultVal);
                        dropDownBox.DefaultValue = requestTypeDefaultVal;
                    }
                    ctrPanel.Controls.Add(dropDownBox);
                    if (question.FieldMandatory)
                    {
                        ctrPanel.Controls.Add(tempBoxForValidation);
                        RequiredFieldValidator reqValidator = new RequiredFieldValidator();
                        reqValidator.ControlToValidate = tempBoxForValidation.ID;
                        reqValidator.ErrorMessage = defaultMandatoryErrorMsg;
                        reqValidator.Display = ValidatorDisplay.Dynamic;
                        reqValidator.CssClass = "errormsg-container";
                        ctrPanel.Controls.Add(reqValidator);
                        // pRequestTypeCtr.Controls.Add(reqValidator);
                    }
                    ctrPanel.Controls.Add(aSPxTextBox);
                    break;
                #endregion REQUESTTYPE
                case Constants.ServiceQuestionType.Rating:
                    {
                        string sfOptions = string.Empty;
                        //Dictionary<string, object> tempObj = null;

                        question.QuestionTypePropertiesDicObj.TryGetValue("options", out sfOptions);
                        string displayMode = RatingDisplayMode.RatingBar.ToString();
                        question.QuestionTypePropertiesDicObj.TryGetValue("displaymode", out displayMode);
                        if (string.IsNullOrWhiteSpace(displayMode))
                            displayMode = RatingDisplayMode.RatingBar.ToString();
                        string defaultValue = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("defaultvalue", out defaultValue);
                        List<string> sfOptionList = new List<string>();
                        if (sfOptions != null)
                            sfOptionList = UGITUtility.ConvertStringToList(sfOptions, new string[] { Constants.Separator1, Constants.Separator2 });


                        Page page = new Page();
                        RatingCtr ratingCtr = (RatingCtr)page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/RatingCtr.ascx");
                        ratingCtr.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        ratingCtr.IsMandatory = question.FieldMandatory;
                        ratingCtr.DisplayMode = displayMode;
                        ratingCtr.DefaultValue = defaultValue;
                        ratingCtr.RatingOptions = new List<string>();
                        foreach (string sfl in sfOptionList)
                        {
                            ratingCtr.RatingOptions.Add(sfl);
                        }

                        ratingCtr.MaxRating = sfOptionList.Count;
                        ratingCtr.JSOnChange = string.Format("questionBrachLogic(this,'{0}', 'textbox', \"{1}\")", questionType, question.TokenName);
                        ratingCtr.JSOnMouseOutParam = string.Format("s,'{0}', 'rating', \"{1}\"", questionType, question.TokenName);
                        ctrPanel.Controls.Add(ratingCtr);
                    }
                    break;
                case Constants.ServiceQuestionType.ApplicationAccess:
                    {
                        string sfOptions = string.Empty;
                        string strDefaultUser = string.Empty;
                        string strNewUser = string.Empty;
                        string strExistingUser = string.Empty;
                        string accessRequestModes = string.Empty;
                        string mirrorAccessFrom = string.Empty;
                        string disableAllCheckBoxStr = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("application", out sfOptions);
                        question.QuestionTypePropertiesDicObj.TryGetValue(ConfigConstants.NewUSer, out strNewUser);
                        question.QuestionTypePropertiesDicObj.TryGetValue(ConfigConstants.ExistingUser, out strExistingUser);
                        question.QuestionTypePropertiesDicObj.TryGetValue(ConfigConstants.MirrorAccessFrom, out mirrorAccessFrom);
                        question.QuestionTypePropertiesDicObj.TryGetValue(ConfigConstants.AccessRequestMode, out accessRequestModes);
                        question.QuestionTypePropertiesDicObj.TryGetValue("disableallcheck", out disableAllCheckBoxStr);
                        List<int> lstApplicationIds = new List<int>();
                        if (sfOptions != null && sfOptions.ToLower() != "all")
                        {
                            List<string> sfOptionList = UGITUtility.ConvertStringToList(sfOptions, new string[] { Constants.Separator1 });
                            foreach (string item in sfOptionList)
                            {
                                string id = Convert.ToString(item.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1]).Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[1];
                                lstApplicationIds.Add(UGITUtility.StringToInt(id));
                            }
                        }
                        Page page = new Page();
                        Panel pnlapplications = new Panel();
                        string Ids = string.Empty;
                        ServiceMatrix serviceMatrix = (ServiceMatrix)page.LoadControl("~/controltemplates/uGovernIT/services/ServiceMatrix.ascx");
                        serviceMatrix.ID = string.Format("question_serviceMatrix_{0}_{1}", question.ServiceSectionID, question.ID);
                        serviceMatrix.Applications = lstApplicationIds.Select(x => x).ToList();
                        serviceMatrix.RoleAssignee = string.Empty;
                        serviceMatrix.IsMobile = false;
                        serviceMatrix.AccessType = "add";
                        serviceMatrix.ParentControl = "Service";
                        serviceMatrix.DisableAllCheckBox = UGITUtility.StringToBoolean(disableAllCheckBoxStr);
                        if (!string.IsNullOrWhiteSpace(strExistingUser))
                            serviceMatrix.DependentToExistingUser = true;
                        if (!string.IsNullOrEmpty(accessRequestModes))
                        {
                            serviceMatrix.LstAccessRequestModes = accessRequestModes.ToLower().Split(';').ToList();
                            serviceMatrix.AccessType = serviceMatrix.LstAccessRequestModes.First();
                        }
                        serviceMatrix.IsReadOnly = false;
                        serviceMatrix.ControlIDPrefix = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        serviceMatrix.IsNoteEnabled = true;
                        // Get application list page size from config variable if available, else default to 10
                        if (question.FieldMandatory)
                        {
                            defaultMandatoryErrorMsg = "    ";
                            serviceMatrix.MandatoryCheck = true;
                            serviceMatrix.MandatoryMessage = defaultMandatoryErrorMsg;
                        }
                        pnlapplications.Controls.Add(serviceMatrix);
                        pnlapplications.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        if (!string.IsNullOrEmpty(strExistingUser))
                            pnlapplications.Attributes.Add("dependentquestion", strExistingUser);
                        if (!string.IsNullOrEmpty(mirrorAccessFrom))
                            pnlapplications.Attributes.Add("mirrorAccessFrom", mirrorAccessFrom);
                        pnlapplications.CssClass += " applicationaccessservice";
                        ctrPanel.Controls.Add(pnlapplications);

                    }
                    break;
                //change need to active this control after confirmation by manish sir
                case Constants.ServiceQuestionType.RemoveAccess:
                    {
                        //Page page = new Page();
                        //Panel pnlRemoveUserAccess = new Panel();
                        //RemoveUserAccess removeUserAccess = (RemoveUserAccess)page.LoadControl("~/_CONTROLTEMPLATES/15/uGovernIT/RemoveUserAccess.ascx");
                        //removeUserAccess.QuestionId = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        //removeUserAccess.IsReadOnly = false;
                        //removeUserAccess.IsShowFieldSet = true;
                        //pnlRemoveUserAccess.Controls.Add(removeUserAccess);
                        //removeUserAccess.ID = string.Format("question_removeaccess_{0}_{1}", question.ServiceSectionID, question.ID); ;
                        //pnlRemoveUserAccess.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        //ctrPanel.Controls.Add(pnlRemoveUserAccess);
                    }
                    break;

                case Constants.ServiceQuestionType.Assets:
                    #region SP Code
                    {
                        ASPxGridLookup userSpecificAssets = new ASPxGridLookup();
                        userSpecificAssets.Width = Unit.Pixel(250);
                        userSpecificAssets.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                        ctrPanel.Controls.Add(userSpecificAssets);
                        userSpecificAssets.Attributes.Add("sectionId", Convert.ToString(question.ServiceSectionID));
                        userSpecificAssets.Attributes.Add("questionId", Convert.ToString(question.ID));
                        userSpecificAssets.GridView.Attributes.Add("sectionId", Convert.ToString(question.ServiceSectionID));
                        userSpecificAssets.GridView.Attributes.Add("questionId", Convert.ToString(question.ID));
                        userSpecificAssets.GridView.Settings.VerticalScrollableHeight = 200;
                        userSpecificAssets.GridView.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                        userSpecificAssets.GridView.Settings.HorizontalScrollBarMode = ScrollBarMode.Auto;
                        userSpecificAssets.GridView.SettingsPager.Mode = GridViewPagerMode.ShowAllRecords;
                        //userSpecificAssets.GridView.SettingsPager.PageSize = 10;

                        userSpecificAssets.KeyFieldName = DatabaseObjects.Columns.ID;
                        userSpecificAssets.SelectionMode = GridLookupSelectionMode.Multiple;
                        userSpecificAssets.GridView.Width = Unit.Percentage(100);
                        string assetType = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("assettype", out assetType);
                        if (!string.IsNullOrEmpty(assetType))
                        {
                            userSpecificAssets.Attributes.Add("assetType", assetType);
                            userSpecificAssets.GridView.Attributes.Add("assetType", assetType);
                        }
                        userSpecificAssets.GridView.ClientInstanceName = "cbAssetsGridView";
                        string currentuser = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("currentuser", out currentuser);
                        if (!string.IsNullOrEmpty(currentuser))
                            userSpecificAssets.Attributes.Add("showassetsofuser", dbContext.CurrentUser.Id);

                        string specificuser = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("specificuser", out specificuser);
                        if (!string.IsNullOrEmpty(specificuser))
                            userSpecificAssets.Attributes.Add("showassetsofuser", specificuser);


                        string inputQ = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("userquestion", out inputQ);
                        if (!string.IsNullOrEmpty(inputQ))
                        {
                            userSpecificAssets.Attributes.Add("DependentQuestion", inputQ);
                            userSpecificAssets.GridView.Attributes.Add("DependentQuestion", inputQ);
                        }
                        string includedepartmentAsset = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("includedepartmentasset", out includedepartmentAsset);
                        if (!string.IsNullOrEmpty(includedepartmentAsset))
                        {
                            userSpecificAssets.Attributes.Add("IncludedepartmentAsset", includedepartmentAsset);
                            userSpecificAssets.GridView.Attributes.Add("IncludedepartmentAsset", includedepartmentAsset);
                        }
                        userSpecificAssets.GridView.CssClass += " assetGridView";
                        string allassets = string.Empty;
                        question.QuestionTypePropertiesDicObj.TryGetValue("allassets", out allassets);
                        if (!string.IsNullOrEmpty(allassets))
                            userSpecificAssets.Attributes.Add("allassets", allassets);

                        userSpecificAssets.CssClass += " assetquestionservice";
                        ModuleColumnManager moduleColumnManager = new ModuleColumnManager(dbContext);
                        List<ModuleColumn> columns = moduleColumnManager.Load(x => x.CategoryName.EqualsIgnoreCase(Constants.MyAssets));
                        GridViewCommandColumn select = new GridViewCommandColumn();
                        select.SelectAllCheckboxMode = GridViewSelectAllCheckBoxMode.AllPages;
                        select.ShowSelectCheckbox = true;
                        select.Width = 30;
                        userSpecificAssets.Columns.Add(select);
                        userSpecificAssets.GridView.CustomCallback += GridView_CustomCallback;
                        userSpecificAssets.GridView.Init += GridView_Init;
                        userSpecificAssets.ClientSideEvents.DropDown = "function(s, e) { try{ FillRelatedAsset(s,e);}catch(ex){}}";
                        userSpecificAssets.GridView.Settings.ShowFilterRow = true;
                        userSpecificAssets.GridView.Settings.ShowFilterRowMenu = true;
                        userSpecificAssets.GridView.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                        // userSpecificAssets.GridView.SettingsBehavior.FilterRowMode = GridViewFilterRowMode.OnClick;
                        foreach (ModuleColumn moduleColumn in columns)
                        {
                            string fieldColumn = Convert.ToString(moduleColumn.FieldName);
                            string fieldDisplayName = Convert.ToString(moduleColumn.FieldDisplayName);
                            if (fieldColumn != DatabaseObjects.Columns.AssetOwner && fieldColumn != DatabaseObjects.Columns.Attachments &&
                                fieldColumn != DatabaseObjects.Columns.DepartmentLookup && fieldColumn != DatabaseObjects.Columns.LocationLookup)
                            {
                                GridViewDataTextColumn column = new GridViewDataTextColumn();
                                if (fieldColumn.EndsWith("user", StringComparison.OrdinalIgnoreCase) || fieldColumn.EndsWith("lookup", StringComparison.OrdinalIgnoreCase))
                                    fieldColumn = string.Format("{0}$", fieldColumn);
                                column.FieldName = fieldColumn;
                                column.Caption = fieldDisplayName;
                                column.CellStyle.Wrap = DevExpress.Utils.DefaultBoolean.True;
                                column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                                column.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                                if (fieldColumn == DatabaseObjects.Columns.TicketId || fieldColumn == DatabaseObjects.Columns.AssetTagNum)
                                    column.Width = new Unit(120, UnitType.Pixel);
                                if (fieldColumn == DatabaseObjects.Columns.AssetModelLookup)
                                    column.Width = new Unit(200, UnitType.Pixel);
                                userSpecificAssets.Columns.Add(column);
                            }
                        }

                        if (userSpecificAssets.Columns != null && userSpecificAssets.Columns.Count > 0)
                        {
                            GridViewColumn col = userSpecificAssets.Columns[DatabaseObjects.Columns.AssetTagNum];//.Cast<GridViewDataTextColumn>().AsEnumerable().FirstOrDefault(x => x.FieldName == DatabaseObjects.Columns.AssetTagNum);
                            if (col != null)
                            {
                                int index = userSpecificAssets.Columns.IndexOf(col);
                                userSpecificAssets.TextFormatString = "{" + (index - 1) + "}";
                            }
                            else
                                userSpecificAssets.TextFormatString = "{0}";
                        }

                        userSpecificAssets.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                        userSpecificAssets.EnableClientSideAPI = true;
                        userSpecificAssets.EnableViewState = false;

                        userSpecificAssets.ClientInstanceName = "cbAssets";
                        if (question.FieldMandatory)
                        {
                            ctrPanel.Controls.Add(tempBoxForValidation);
                            CustomValidator reqValidator = new CustomValidator();
                            reqValidator.ID = string.Format("question_{0}_{1}__{0}{1}_error", question.ServiceSectionID, question.ID);
                            reqValidator.ControlToValidate = tempBoxForValidation.ID;
                            reqValidator.ErrorMessage = defaultMandatoryErrorMsg;
                            reqValidator.CssClass = "errormsg-container";
                            reqValidator.ValidateEmptyText = true;
                            reqValidator.Display = ValidatorDisplay.Dynamic;
                            reqValidator.ServerValidate += ReqValidator_ServerValidate;
                            ctrPanel.Controls.Add(reqValidator);
                        }
                    }
                    #endregion
                    
                    break;
                    //change need to active this control after confirmation by manish sir
                    //case Constants.ServiceQuestionType.Resources:
                    //    {
                    //        Page dd = new Page();
                    //        AddResources resources = (AddResources)dd.LoadControl("~/CONTROLTEMPLATES/uGovernIT/AddResources.ascx");
                    //        resources.ID = string.Format("question_{0}_{1}", question.ServiceSectionID, question.ID);
                    //        ctrPanel.Controls.Add(resources);
                    //    }
                    //    break;
            }
            return ctrPanel;
        }

        public static Control GenerateControlForSkipLogic(ServiceQuestion question, string selectedVals)
        {
            List<string> selValues = UGITUtility.ConvertStringToList(selectedVals, Constants.Separator2);
            Control ctr = new Control();
            string questionType = question.QuestionType.ToLower();
            string defaultVal = string.Empty;

            TextBox tempBoxForValidation = new TextBox();
            tempBoxForValidation.Style.Add(HtmlTextWriterStyle.Display, "none");
            tempBoxForValidation.CssClass += string.Format(" hiddenCtr hiddenCtr_{0}", question.ID);
            tempBoxForValidation.ID = string.Format("txthiddenCtr_{0}", question.ID);
            ApplicationContext context = HttpContext.Current.GetManagerContext();

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
                    UserValueBox userCtr = new UserValueBox();
                    UserProfileManager userManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                    //Set selected value
                    if (selValues.Count > 0)
                    {
                        List<string> userNames = new List<string>();
                        foreach (string userIDVal in selValues)
                        {
                            UserProfile user = userManager.GetUserById(userIDVal);
                            if (user != null)
                            {
                                userNames.Add(user.Id);
                            }
                        }
                        userCtr.SetValues(string.Join(",", userNames.ToArray()));
                    }
                    ctr = userCtr;
                    break;

                case Constants.ServiceQuestionType.LOOKUP:
                    CheckBoxList lookupCtr = new CheckBoxList();
                    string listName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("lookuplist", out listName);

                    string listfield = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("lookupfield", out listfield);

                    string lpModuleName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("module", out lpModuleName);

                    FieldConfigurationManager configManager = new FieldConfigurationManager(context);
                    FieldConfiguration fieldconfig = configManager.GetFieldByFieldName(listName);

                    if (fieldconfig != null)
                    {
                        DataTable list = GetTableDataManager.GetTableData(fieldconfig.ParentTableName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
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
                                string filter = string.Empty;

                                if (table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                                    filter = string.Format("{0} = '{1}'", DatabaseObjects.Columns.ModuleNameLookup, lpModuleName);

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
                                    item.Selected = true;
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

                    ModuleViewManager viewManager = new ModuleViewManager(context);
                    DataTable moduledt = UGITUtility.ObjectToData(viewManager.LoadByName(moduleName));
                    DataRow moduleRow = moduledt.Rows[0];

                    List<string> reqestTypes = UGITUtility.ConvertStringToList(requestTypes, new string[] { Constants.Separator1, Constants.Separator2 });

                    if (reqestTypes == null || (reqestTypes.Count > 0 && (reqestTypes[0].ToLower() == string.Empty || reqestTypes[0].ToLower() == "all")))
                        reqestTypes = new List<string>();

                    DropDownList ddlRequestType = uHelper.GetRequestTypesWithCategoriesDropDown(context, moduleRow, reqestTypes, true, true, null);
                    CheckBoxList rCBList = new CheckBoxList();

                    foreach (ListItem item in ddlRequestType.Items)
                    {
                        item.Selected = false;

                        if (item.Attributes["disabled"] != null)
                            item.Enabled = false;

                        rCBList.Items.Add(item);
                    }

                    foreach (string sVal in selValues)
                    {
                        ListItem item = rCBList.Items.FindByValue(sVal);

                        if (item != null)
                            item.Selected = true;
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

        public static string ReadSelectedValueSkipLogic(ServiceQuestion question, Control ctr, string operatorVal)
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
            else if (ctr is UserValueBox)
            {
                UserValueBox ufCtr = (UserValueBox)ctr;
                ctrValues.Add(string.Format("{0}", ufCtr.GetValues()));

            }
            else if (ctr is LookUpValueBox)
            {
                LookUpValueBox ufCtr = (LookUpValueBox)ctr;
                ctrValues.Add(string.Format("{0}", ufCtr.GetValues()));
            }
            else if (ctr is LookupValueBoxEdit)
            {
                LookupValueBoxEdit ufCtr = (LookupValueBoxEdit)ctr;
                ctrValues.Add(string.Format("{0}", ufCtr.GetValues()));
            }
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

        public static void GridLookup_Load(object sender, EventArgs e)
        {
            ASPxGridLookup grdlookup = sender as ASPxGridLookup;
            List<string> lst = UGITUtility.ConvertStringToList(grdlookup.Attributes["optList"], ",");
            DataTable dt = new DataTable();
            dt.Columns.Add(DatabaseObjects.Columns.Title);
            dt.Columns.Add(DatabaseObjects.Columns.Id);
            int i = 1;
            foreach (string item in lst)
            {
                DataRow dr = dt.NewRow();
                dr[DatabaseObjects.Columns.Title] = item;
                dr[DatabaseObjects.Columns.Id] = i;
                i++;
                dt.Rows.Add(dr);
            }
            dt = dt.DefaultView.ToTable(true, DatabaseObjects.Columns.Title);
            grdlookup.DataSource = dt;
            grdlookup.KeyFieldName = DatabaseObjects.Columns.Title;
            grdlookup.DataBind();


        }

        /// <summary>
        /// This callback method is called to get assets list of related requestor of ticket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void userSpecificAssets_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxComboBox comboBox = (ASPxComboBox)sender;
            DataTable table = null;
            comboBox.Items.Clear();
            //UserProfile user = UserProfile.LoadUserProfileByName(e.Parameter.Trim());
            //if (user != null)
            //{
            //    UGITModule cmdbModule = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "CMDB");
            //    DataTable result = uGITCache.ModuleDataCache.GetOpenTickets(cmdbModule.ID);
            //    if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.AssetOwner))
            //    {
            //        var xbase = result.AsEnumerable().Where(x => (!x.IsNull(DatabaseObjects.Columns.AssetOwner) && x.Field<string>(DatabaseObjects.Columns.AssetOwner) == user.Name) ||
            //       (!x.IsNull(DatabaseObjects.Columns.CurrentUser) && x.Field<string>(DatabaseObjects.Columns.CurrentUser) == user.Name));
            //        if (xbase.Count() > 0)
            //        {
            //            result = xbase.CopyToDataTable();
            //        }
            //        else
            //        {
            //            result = null;
            //        }
            //    }

            //    table = result;
            //}

            comboBox.ValueField = DatabaseObjects.Columns.Id;
            comboBox.ValueType = typeof(int);
            comboBox.TextField = DatabaseObjects.Columns.AssetTagNum;
            comboBox.DataSource = table;
            comboBox.DataBind();
        }

        public static void cvUserField_ServerValidate(object source, ServerValidateEventArgs args)
        {
            UserProfileManager userManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            CustomValidator cvTxtBox = (CustomValidator)source;
            bool isManager = Convert.ToBoolean(cvTxtBox.Attributes["isManager"]);
            string groupName = cvTxtBox.Attributes["GroupName"];

            string userCtrID = UGITUtility.SplitString(cvTxtBox.ID, "__")[0];
            string ctrlToValidate = cvTxtBox.ControlToValidate;
            if (!string.IsNullOrEmpty(groupName))
            {
                ASPxComboBox cmbUsers = (ASPxComboBox)cvTxtBox.Parent.FindControl(userCtrID);
                if (cmbUsers.SelectedItem == null)
                    args.IsValid = false;
            }
            else
            {
                UserValueBox pEditor = (UserValueBox)cvTxtBox.Parent.FindControl(userCtrID);
                TextBox txtHidden = (TextBox)cvTxtBox.Parent.FindControl(ctrlToValidate);
                if (txtHidden.Text == "[$skip$]")
                {
                    args.IsValid = true;
                }
                else if (pEditor != null)
                {
                    if (string.IsNullOrEmpty(pEditor.GetValues()))
                    {
                        args.IsValid = false;
                        cvTxtBox.ErrorMessage = "Please Specify User";
                    }
                    else
                    {
                        UserProfile spUser = userManager.GetUserByUserName(pEditor.GetValues());
                        if (spUser != null)
                        {
                            if (isManager && !spUser.IsManager)
                            {
                                args.IsValid = false;
                                cvTxtBox.ErrorMessage = "User is not a Manager";
                                return;
                            }
                        }
                    }
                }
            }


        }

        public static void cvUserField_ServerValidateMobile(object source, ServerValidateEventArgs args)
        {
            CustomValidator cvTxtBox = (CustomValidator)source;
            string userCtrID = UGITUtility.SplitString(cvTxtBox.ID, "__")[0];
            string ctrlToValidate = cvTxtBox.ControlToValidate;
            DropDownList pEditor = (DropDownList)cvTxtBox.Parent.FindControl(userCtrID);
            TextBox txtHidden = (TextBox)cvTxtBox.Parent.FindControl(ctrlToValidate);
            // HtmlInputText txtHidden = (HtmlInputText)cvTxtBox.Parent.FindControl(ctrlToValidate);
            if (txtHidden.Text == "[$skip$]")
            {
                args.IsValid = true;
            }
            else if (pEditor != null)
            {
                if (pEditor.SelectedItem == null)
                {
                    args.IsValid = false;
                }
                //else if (txtHidden. == "[$skip$]")
                //{
                //    args.IsValid = true;
                //}
            }

        }

        public static void reqDateCValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cValidator = source as CustomValidator;
            string dateTimeCtrID = cValidator.ControlToValidate.Split('$').FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(dateTimeCtrID))
            {
                DateTime validationMinDate = DateTime.MinValue;
                DateTime validationMaxDate = DateTime.MaxValue;
                ASPxDateEdit dtCtr = cValidator.Parent.FindControl(dateTimeCtrID) as ASPxDateEdit;
                string validateAgainstCtrlID = cValidator.Attributes["validateagainst"];
                bool pastdate = UGITUtility.StringToBoolean(cValidator.Attributes["pastdate"]);
                bool futuredate = UGITUtility.StringToBoolean(cValidator.Attributes["futuredate"]);
                bool presentdate = UGITUtility.StringToBoolean(cValidator.Attributes["presentdate"]);
                DateTime defaultDate = DateTime.Now;
                string validationString = dtCtr.CssClass;

                if (!string.IsNullOrEmpty(validateAgainstCtrlID))
                {
                    System.Web.UI.WebControls.ListView lvQuestions = uHelper.GetParentControl(cValidator.Parent, "lvQuestions") as System.Web.UI.WebControls.ListView;
                    if (lvQuestions != null)
                    {
                        ASPxDateEdit dtCtrDep = null;

                        foreach (ListViewDataItem item in lvQuestions.Items)
                        {
                            dtCtrDep = item.FindControl(validateAgainstCtrlID) as ASPxDateEdit;
                            if (dtCtrDep != null)
                                break;
                        }
                        if (dtCtrDep != null)
                        {
                            defaultDate = dtCtrDep.Date;
                        }
                    }
                    validationString = GetDateValidationString(defaultDate, pastdate, presentdate, futuredate);
                }

                if (!string.IsNullOrEmpty(validationString) && validationString.Contains("#"))
                {
                    string[] validDates = validationString.Split('#');
                    validationMaxDate = Convert.ToDateTime(validDates[0]);
                    validationMinDate = Convert.ToDateTime(validDates[1]);
                }


                DateTime selectedDate = DateTime.MinValue;
                DateTime.TryParse(args.Value, out selectedDate);

                if (dtCtr != null && selectedDate != DateTime.MinValue)
                {
                    if (selectedDate.Date < validationMinDate.Date)
                    {
                        args.IsValid = false;
                        cValidator.ErrorMessage = string.Format("Date must be on or after {0}", UGITUtility.GetDateStringInFormat(validationMinDate, false));
                    }
                    else if (selectedDate.Date > validationMaxDate.Date)
                    {
                        args.IsValid = false;
                        cValidator.ErrorMessage = string.Format("Date must be on or before {0}", UGITUtility.GetDateStringInFormat(validationMaxDate, false));
                    }
                }
                else
                {
                    args.IsValid = false;
                    cValidator.ErrorMessage = "Please Enter a Valid Date";
                }


            }
        }

        public static string GetDateValidationString(DateTime defaultDate, bool pastdate, bool presentdate, bool futuredate)
        {
            DateTime minDate = DateTime.MinValue;
            DateTime maxDate = DateTime.MaxValue;
            if (!UGITUtility.StringToBoolean(pastdate) || !UGITUtility.StringToBoolean(futuredate))
            {
                if (UGITUtility.StringToBoolean(pastdate))
                {
                    if (UGITUtility.StringToBoolean(presentdate))
                    {
                        maxDate = defaultDate.AddMinutes(1439);// DateTime.Now.Date.AddMinutes(1439);
                    }
                    else
                    {
                        maxDate = defaultDate.AddDays(-1);// DateTime.Now.AddDays(-1);
                    }
                }
                else if (UGITUtility.StringToBoolean(futuredate))
                {
                    if (UGITUtility.StringToBoolean(presentdate))
                    {
                        minDate = defaultDate;// DateTime.Now.Date;
                    }
                    else
                    {
                        minDate = defaultDate.AddDays(1);// DateTime.Now.AddDays(1);
                    }
                }
                else
                {
                    maxDate = defaultDate.AddMinutes(1439);// DateTime.Now.Date.AddMinutes(1439);
                    minDate = defaultDate;// DateTime.Now.Date;
                }
            }
            return Convert.ToString(maxDate) + "#" + Convert.ToString(minDate);
        }

        public static void subCategoryDropdown_Callback(object sender, CallbackEventArgsBase e)
        {

            ASPxCallbackPanel panel = (ASPxCallbackPanel)sender;
            List<string> requestKeys = UGITUtility.ConvertStringToList(panel.Attributes["requestKeys"], ",");
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            UGITModule moduleRow = moduleViewManager.LoadByName(panel.Attributes["ModuleName"]);
            if (moduleRow == null)
                return;
            string category = string.Empty;
            string subCategory = string.Empty;

            string parameters = e.Parameter;

            if (parameters.IndexOf("RequestType=") != -1)
            {
                string requestTypeId = UGITUtility.SplitString(parameters.Trim(), "=")[1];
                int RequestTypeId = UGITUtility.StringToInt(requestTypeId, 0);
                //uHelper.BindIssueTypeDDl(panel, RequestTypeId);
            }
            else
            {
                if (parameters.IndexOf("**") == -1)
                {
                    string[] paramsData = parameters.Split(new string[] { ";#;" }, StringSplitOptions.None);
                    if (paramsData.Length > 0)
                        category = paramsData[0];
                    if (paramsData.Length > 1)
                        subCategory = paramsData[1];
                }
                else
                {
                    category = e.Parameter.Replace("**", ";#");
                }

                foreach (Control ctr in panel.Controls)
                {
                    if (ctr is DropDownList)
                    {
                        DropDownList requestTypeCtr = ctr as DropDownList;
                        string selectedVal = string.Empty;
                        if (requestTypeCtr.SelectedItem != null)
                        {
                            selectedVal = requestTypeCtr.SelectedValue;
                        }
                        //DropDownList tempdd = uHelper.GetRequestTypesWithCategoriesDropDownOnChange(moduleRow, category, false, false, subCategory, requestKeys);

                        //requestTypeCtr.Items.Clear();
                        //foreach (ListItem item in tempdd.Items)
                        //{
                        //    requestTypeCtr.Items.Add(item);
                        //}
                        //requestTypeCtr.Items.Insert(0, new ListItem("", ""));
                        //requestTypeCtr.SelectedIndex = requestTypeCtr.Items.IndexOf(requestTypeCtr.Items.FindByValue(selectedVal));

                        //if (requestTypeCtr.SelectedIndex == 0)
                        //{
                        //    if (uGITCache.GetConfigVariableValueAsBool(ConfigConstants.EnableRequestTypeSubCategory) && requestTypeCtr.Items.Count > 1)
                        //    {
                        //        requestTypeCtr.SelectedIndex = 1;
                        //        selectedVal = requestTypeCtr.Items[1].Value;
                        //    }
                        //    else
                        //    {
                        //        selectedVal = requestTypeCtr.Items[0].Value;
                        //    }
                        //}

                        //int requestTypeID = 0;
                        //int.TryParse(selectedVal, out requestTypeID);
                        //uHelper.BindIssueTypeDDl(panel, requestTypeID);
                    }
                }
            }
        }

        public static void datetimeCtr_DateChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Mandatory Validation- Asset does not have client side support so fix on server side
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private static void ReqValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            CustomValidator cvTxtBox = (CustomValidator)source;
            string assetCtrID = UGITUtility.SplitString(cvTxtBox.ID, "__")[0];
            string ctrlToValidate = cvTxtBox.ControlToValidate;
            {
                ASPxGridLookup assetCtr = (ASPxGridLookup)cvTxtBox.Parent.FindControl(assetCtrID);
                TextBox txtHidden = (TextBox)cvTxtBox.Parent.FindControl(ctrlToValidate);
                if (txtHidden.Text == "[$skip$]")
                {
                    args.IsValid = true;
                }
                else if (assetCtr != null)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(assetCtr.Value)))
                    {
                        args.IsValid = false;
                    }
                }
            }
        }
        private static void GridView_Init(object sender, EventArgs e)
        {
            #region .net code
            DevExpress.Web.ASPxGridView cbx = (DevExpress.Web.ASPxGridView)sender;
            bool includeDeptAsset = UGITUtility.StringToBoolean(cbx.Attributes["IncludedepartmentAsset"]);
            //Cookie should be question specific if we have more then one question having assettype
            string assetTypeCookieVar = string.Format("DependentAssetType_{0}_{1}", cbx.Attributes["sectionId"], cbx.Attributes["questionId"]);
            string userNameCookieVar = string.Format("DependentUser_{0}_{1}", cbx.Attributes["sectionId"], cbx.Attributes["questionId"]);
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            string strUserChange = string.Empty;
            string assetType = UGITUtility.GetCookieValue(HttpContext.Current.Request, assetTypeCookieVar);
            string userName = UGITUtility.GetCookieValue(HttpContext.Current.Request, userNameCookieVar);
            UserProfile user = HttpContext.Current.GetUserManager().GetUserByUserName(userName);
            UGITModule cmdbModule = moduleViewManager.LoadByName("CMDB");
            DataTable result = null;
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            if (userName == "all")
            {
                result = ticketManager.GetOpenTickets(cmdbModule);
                cbx.DataSource = result;
            }
            if (user != null)
            {
                result = ticketManager.GetOpenTickets(cmdbModule);
                if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.AssetOwner))
                {
                    //Filter all the assets where current user is assetower or current user
                    DataRow[] xbase = null;
                    if (includeDeptAsset)
                        xbase = result.AsEnumerable().Where(x => (!x.IsNull(DatabaseObjects.Columns.AssetOwner) && x.Field<string>(DatabaseObjects.Columns.AssetOwner) == user.Id) ||
                         (!x.IsNull(DatabaseObjects.Columns.CurrentUser) && x.Field<string>(DatabaseObjects.Columns.CurrentUser) == user.Id) || (!x.IsNull(DatabaseObjects.Columns.DepartmentLookup) && x.Field<long>(DatabaseObjects.Columns.DepartmentLookup) == UGITUtility.StringToDouble(user.Department))).ToArray();
                    else
                        xbase = result.AsEnumerable().Where(x => (!x.IsNull(DatabaseObjects.Columns.AssetOwner) && x.Field<string>(DatabaseObjects.Columns.AssetOwner) == user.Id) ||
                            (!x.IsNull(DatabaseObjects.Columns.CurrentUser) && x.Field<string>(DatabaseObjects.Columns.CurrentUser) == user.Id)).ToArray();

                    if (xbase != null && xbase.Count() > 0)
                    {
                        result = xbase.CopyToDataTable();
                    }
                    else
                    {
                        result = null;
                    }
                }
                cbx.DataSource = result;
            }

            if (!string.IsNullOrEmpty(assetType))
            {
                #region asset type
                List<ModuleRequestType> selectedAssetTypes = cmdbModule.List_RequestTypes.Where(x => assetType.Contains(Convert.ToString(x.ID))).ToList();
                if (selectedAssetTypes != null && selectedAssetTypes.Count > 0)
                {
                    string formatedAssetType = string.Join(",", selectedAssetTypes.Select(x => string.Format("'{0}'", x.RequestType)));
                    if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                    {
                        DataRow[] dr = result.Select(string.Format("{0} IN ({1})", DatabaseObjects.Columns.TicketRequestTypeLookup, formatedAssetType));

                        if (dr.Length > 0)
                            result = dr.CopyToDataTable();
                        else
                            result = null;
                    }
                    else if (cbx.DataSource != null)
                    {
                        result = (DataTable)cbx.DataSource;
                        if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                        {
                            DataRow[] dr = result.Select(string.Format("{0} IN ({1})", DatabaseObjects.Columns.TicketRequestTypeLookup, formatedAssetType));

                            if (dr.Length > 0)
                                result = dr.CopyToDataTable();
                            else
                                result = null;
                        }
                    }

                    cbx.DataSource = result;
                }
                #endregion
            }
           
            cbx.DataBind();
            #endregion
        }

        private static void GridView_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            DevExpress.Web.ASPxGridView cbx = (DevExpress.Web.ASPxGridView)sender;
            bool includeDeptAsset = UGITUtility.StringToBoolean(cbx.Attributes["IncludedepartmentAsset"]);
            string parameters = Uri.UnescapeDataString(e.Parameters);
            //bool isMobileCall = false;
            if (!string.IsNullOrEmpty(e.Parameters) && e.Parameters.IndexOf("mobilecall") != -1)
            {
                //isMobileCall = true;
                parameters = parameters.Replace("|mobilecall", "").Trim();
            }
            //Cookie should be question specific if we have more then one question having assettype
            string assetTypeCookieVar = string.Format("DependentAssetType_{0}_{1}", cbx.Attributes["sectionId"], cbx.Attributes["questionId"]);
            string userNameCookieVar = string.Format("DependentUser_{0}_{1}", cbx.Attributes["sectionId"], cbx.Attributes["questionId"]);
            string userName = string.Empty;
            userName = UGITUtility.GetCookieValue(HttpContext.Current.Request, userNameCookieVar);
            string assetType = string.Empty;
            assetType = UGITUtility.GetCookieValue(HttpContext.Current.Request, assetTypeCookieVar);
            string sSelectAll = string.Empty;
            ModuleViewManager moduleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
            string includeDepartment = string.Empty;
            if (parameters.Contains(Constants.Separator2))
            {
                assetType = parameters.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[1];
                parameters = parameters.Split(new string[] { Constants.Separator2 }, StringSplitOptions.RemoveEmptyEntries)[0];
                if (parameters.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).Length == 3)
                    includeDepartment = parameters.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[2];

            }
            if (parameters.Contains(Constants.Separator))
            {
                userName = parameters.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[0];
                sSelectAll = parameters.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[1];
                if (parameters.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries).Length == 3)
                    includeDepartment = parameters.Split(new string[] { Constants.Separator }, StringSplitOptions.RemoveEmptyEntries)[2];

            }
            else
                userName = parameters;
            UserProfile user = HttpContext.Current.GetUserManager().GetUserById(userName);
            UGITModule cmdbModule = moduleViewManager.LoadByName("CMDB");
            TicketManager ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
            DataTable result = null;
            if (userName == "all")
            {
                result = ticketManager.GetOpenTickets(cmdbModule);
            }
            UGITUtility.CreateCookie(HttpContext.Current.Response, userNameCookieVar, userName);
            if (user != null)
            {
                result = ticketManager.GetOpenTickets(cmdbModule);
                if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.AssetOwner))
                {
                    //Filter all the assets where current user is assetower or current user
                    DataRow[] xbase = null;
                    if (includeDeptAsset)
                        xbase = result.AsEnumerable().Where(x => (!x.IsNull(DatabaseObjects.Columns.AssetOwner) && x.Field<string>(DatabaseObjects.Columns.AssetOwner) == user.Id) ||
                         (!x.IsNull(DatabaseObjects.Columns.CurrentUser) && x.Field<string>(DatabaseObjects.Columns.CurrentUser) == user.Id) || (!x.IsNull(DatabaseObjects.Columns.DepartmentLookup) && x.Field<long>(DatabaseObjects.Columns.DepartmentLookup) == UGITUtility.StringToDouble(user.Department))).ToArray();
                    else
                        xbase = result.AsEnumerable().Where(x => (!x.IsNull(DatabaseObjects.Columns.AssetOwner) && x.Field<string>(DatabaseObjects.Columns.AssetOwner) == user.Id) ||
                            (!x.IsNull(DatabaseObjects.Columns.CurrentUser) && x.Field<string>(DatabaseObjects.Columns.CurrentUser) == user.Id)).ToArray();

                    if (xbase != null && xbase.Count() > 0)
                    {
                        result = xbase.CopyToDataTable();
                    }
                    else
                    {
                        result = null;
                    }
                }

            }

            UGITUtility.CreateCookie(HttpContext.Current.Response, assetTypeCookieVar, assetType);
            if (!string.IsNullOrEmpty(assetType) && assetType != "all")
            {
                List<ModuleRequestType> selectedAssetTypes = cmdbModule.List_RequestTypes.Where(x => assetType.Contains(Convert.ToString(x.ID))).ToList();
                if (selectedAssetTypes != null && selectedAssetTypes.Count > 0)
                {
                    string formatedAssetType = string.Join(",", selectedAssetTypes.Select(x => string.Format("'{0}'", x.RequestType)));
                    if (result != null && result.Rows.Count > 0 && result.Columns.Contains(DatabaseObjects.Columns.TicketRequestTypeLookup))
                    {
                        DataRow[] dr = result.Select(string.Format("{0} IN ({1})", DatabaseObjects.Columns.TicketRequestTypeLookup, formatedAssetType));

                        if (dr.Length > 0)
                            result = dr.CopyToDataTable();
                        else
                            result = null;
                    }
                }

            }

            if (cbx.DataSource == null || !e.Parameters.StartsWith("GLP_AC"))
                cbx.DataSource = result;
            cbx.DataBind();

            GridViewCommandColumn cbSelectAllCol = cbx.Columns[0] as GridViewCommandColumn;
            ASPxCheckBox cbSelectAll = cbx.FindHeaderTemplateControl(cbSelectAllCol, "cbSelectAll") as ASPxCheckBox;

            if (cbSelectAll != null)
            {
                if (sSelectAll.ToLower() == "selectall")
                {
                    cbSelectAll.Checked = true;
                    cbx.Selection.SelectAll();
                }
                else if (sSelectAll.ToLower() == "unselectall")
                {
                    cbSelectAll.Checked = false;
                    cbx.Selection.UnselectAll();
                }
            }
        }

        public static void CustomValidator_ServerValidate_EmailRegistration(object source, ServerValidateEventArgs e)
        {
            var isExist = TenantOnBoardingHelper().IsEmailExist(e.Value);

            if (isExist)
                e.IsValid = false;
            else
                e.IsValid = true;
        }

        public static void CustomValidator_ServerValidate_CompanyRegistration(object source, ServerValidateEventArgs e)
        {
            var isExist = TenantOnBoardingHelper().IsCompanyExist(e.Value);

            if (isExist)
                e.IsValid = false;
            else
                e.IsValid = true;
        }

        public static TenantOnBoardingHelper TenantOnBoardingHelper()
        {
            ApplicationContext applicationContext = ApplicationContext.Create();

            return new TenantOnBoardingHelper(applicationContext);
        }

        /// <summary>
        /// Generate value for skip logic.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="selectedVals"></param>
        /// <returns></returns>
        public static string GetQuestionValueForSkipLogic(ServiceQuestion question, string selectedVals)
        {
            List<string> selValues = UGITUtility.ConvertStringToList(selectedVals, Constants.Separator2);
            string qValue = string.Empty;
            string questionType = question.QuestionType.ToLower();
            string defaultVal = string.Empty;
            ApplicationContext context = HttpContext.Current.GetManagerContext();

            switch (questionType)
            {
                case Constants.ServiceQuestionType.TEXTBOX:
                case Constants.ServiceQuestionType.Number:

                    //Set select value
                    if (selValues.Count > 0)
                        qValue = selValues[0];

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

                    if (!string.IsNullOrEmpty(pickFromField))
                    {
                        FieldConfigurationManager fManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                        DataTable dt = fManager.GetFieldDataByFieldName(optionPickFromField);

                        if (dt != null)
                        {
                            foreach (DataRow option in dt.Rows)
                            {
                                items.Add(new ListItem(Convert.ToString(option["ID"]), Convert.ToString(option["Title"])));
                            }
                        }
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
                    List<string> lstChoices = new List<string>();
                    foreach (string sVal in selValues)
                    {
                        ListItem item = ddlMCtr.Items.FindByValue(sVal);
                        if (item != null)
                        {
                            lstChoices.Add(item.Text);
                        }
                    }
                    qValue = string.Join(";", lstChoices);

                    break;

                case Constants.ServiceQuestionType.CHECKBOX:

                    //Set select value
                    if (selValues.Count > 0)
                    {
                        qValue = Convert.ToString(selValues[0]);
                    }

                    break;
                case Constants.ServiceQuestionType.DATETIME:
                    ASPxDateEdit datetimeCtr = new ASPxDateEdit();
                    datetimeCtr.TimeSectionProperties.Visible = false;
                    datetimeCtr.CssClass = "datetimectr";

                    string dateMode = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("datemode", out dateMode);

                    if (dateMode != null)
                    {
                        if (dateMode.ToLower() == "dateonly")
                            datetimeCtr.EditFormat = EditFormat.Date;
                        else if (dateMode.ToLower() == "timeonly")
                            datetimeCtr.EditFormat = EditFormat.Time;
                        else if (dateMode.ToLower() == "datetime")
                            datetimeCtr.EditFormat = EditFormat.DateTime;
                    }
                    datetimeCtr.EditFormat = EditFormat.Date;

                    //Set selected value
                    if (selValues.Count > 0)
                    {
                        DateTime date = DateTime.MinValue;
                        DateTime.TryParse(selValues[0].Trim(), out date);

                        if (date != DateTime.MinValue)
                            qValue = Convert.ToString(date);
                    }

                    break;
                case Constants.ServiceQuestionType.USERFIELD:
                    //Set selected value
                    if (selValues.Count > 0)
                    {
                        UserProfileManager userProfileManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
                        List<string> userNames = new List<string>();
                        foreach (string userIDVal in selValues)
                        {
                            UserProfile uProfile = userProfileManager.GetUserInfoById(userIDVal);

                            if (uProfile != null)
                                userNames.Add(uProfile.Name);
                        }
                        qValue = string.Join(",", userNames.ToArray());
                    }

                    break;

                case Constants.ServiceQuestionType.LOOKUP:

                    CheckBoxList lookupCtr = new CheckBoxList();
                    string listName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("lookuplist", out listName);

                    string listfield = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("lookupfield", out listfield);

                    string lpModuleName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("module", out lpModuleName);

                    FieldConfigurationManager configManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                    FieldConfiguration fieldconfig = configManager.GetFieldByFieldName(listName);

                    if (fieldconfig != null)
                    {
                        DataTable list = GetTableDataManager.GetTableData(fieldconfig.ParentTableName, $"{DatabaseObjects.Columns.TenantID} = '{context.TenantID}'");
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
                                string filter = string.Empty;

                                if (table.Columns.Contains(DatabaseObjects.Columns.ModuleNameLookup))
                                    filter = string.Format("{0} = '{1}'", DatabaseObjects.Columns.ModuleNameLookup, lpModuleName);

                                columnVals = dView.ToTable(true, field.ColumnName, DatabaseObjects.Columns.Id);
                            }

                            foreach (DataRow row in columnVals.Rows)
                            {
                                lookupCtr.Items.Add(new ListItem(Convert.ToString(row[field.ColumnName]), Convert.ToString(row[DatabaseObjects.Columns.Id])));
                            }

                            //set selected values
                            List<string> lstLookupVal = new List<string>();
                            foreach (string sVal in selValues)
                            {
                                ListItem item = lookupCtr.Items.FindByValue(sVal);
                                if (item != null)
                                {
                                    item.Selected = true;
                                    lstLookupVal.Add(item.Text);
                                }
                            }

                            qValue = string.Join(";", lstLookupVal);
                        }
                    }

                    break;
                case Constants.ServiceQuestionType.REQUESTTYPE:

                    string requestTypes = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("requesttypes", out requestTypes);

                    string moduleName = string.Empty;
                    question.QuestionTypePropertiesDicObj.TryGetValue("module", out moduleName);

                    ModuleViewManager viewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
                    DataTable moduledt = UGITUtility.ObjectToData(viewManager.LoadByName(moduleName));
                    DataRow moduleRow = moduledt.Rows[0];

                    List<string> reqestTypes = UGITUtility.ConvertStringToList(requestTypes, new string[] { Constants.Separator1, Constants.Separator2 });

                    if (reqestTypes == null || (reqestTypes.Count > 0 && (reqestTypes[0].ToLower() == string.Empty || reqestTypes[0].ToLower() == "all")))
                        reqestTypes = new List<string>();

                    DropDownList ddlRequestType = uHelper.GetRequestTypesWithCategoriesDropDown(context, moduleRow, reqestTypes, true, true, null);
                    CheckBoxList rCBList = new CheckBoxList();
                    foreach (ListItem item in ddlRequestType.Items)
                    {
                        item.Selected = false;
                        if (item.Attributes["disabled"] != null)
                        {
                            item.Enabled = false;
                        }
                        rCBList.Items.Add(item);
                    }

                    List<string> lstOfVal = new List<string>();
                    foreach (string sVal in selValues)
                    {
                        ListItem item = rCBList.Items.FindByValue(sVal);
                        if (item != null)
                        {
                            item.Selected = true;
                            lstOfVal.Add(item.Text);
                        }
                    }
                    qValue = string.Join(";", lstOfVal);

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
                    ddlRatings.SelectedIndex = ddlRatings.Items.IndexOf(ddlRatings.Items.FindByValue(selectedVals));

                    if (ddlRatings.SelectedIndex != -1)
                        qValue = ddlRatings.SelectedItem.Text;

                    break;

            }
            return qValue;
        }
    }
}
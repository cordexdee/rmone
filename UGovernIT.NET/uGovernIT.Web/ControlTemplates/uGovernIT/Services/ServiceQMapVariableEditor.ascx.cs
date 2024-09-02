using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using uGovernIT.Utility;
using uGovernIT.Manager;
using System.Web;
using uGovernIT.Utility.Entities;
using DevExpress.XtraEditors;
using DevExpress.Web;
using System.Data;

namespace uGovernIT.Web
{
    public partial class ServiceQMapVariableEditor : UserControl
    {
        public int svcConfigID;
        public int sectionID;
        Services service;
        QuestionMapVariable qmVariable;
        ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
        ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
        UserProfile User;
        Dictionary<string, string> dictSelectedQuestions = new Dictionary<string, string>();
        List<WhereExpression> whrExpressions;
        DataTable dtQuestions;
        bool isMandatoryAttachmentCondition;
        DataTable dtExpresstionResult;
        private UserProfileManager userProfileManager = null;

        protected override void OnInit(EventArgs e)
        {
            if (Convert.ToString(Request["conditionType"]) == "attachment")
            {
                isMandatoryAttachmentCondition = true;
            }
            pOperatorValue.Visible = false;
            pPickerValueContainer.Visible = false;
            pnlQuestion2.Visible = pnlQuestion3.Visible = pnlQuestion4.Visible = pnlQuestion5.Visible = false;
            pnlOperatorValue2.Visible = pnlOperatorValue3.Visible = pnlOperatorValue4.Visible = pnlOperatorValue5.Visible = false;
            pnlPickerValueContainer2.Visible = pnlPickerValueContainer3.Visible = pnlPickerValueContainer4.Visible = pnlPickerValueContainer5.Visible = false;
            dtQuestions = new DataTable();
            dtQuestions.Columns.Add("Text");
            dtQuestions.Columns.Add("Value");

            User = HttpContext.Current.CurrentUser();
            userProfileManager = new UserProfileManager(HttpContext.Current.GetManagerContext());
            rfvText.ControlToValidate = txtTitle.ID;
            RequiredFieldValidator2.ControlToValidate = txtShortName.ID;
            cvShortname.ControlToValidate = txtShortName.ID;
            cvCompatibilityCheck.ControlToValidate = ddlDefaultValQuestion.ID;
            cvcConditionValidator.ControlToValidate = ddlDefaultValQuestion.ID;

            int.TryParse(Request["svcConfigID"], out svcConfigID);

            if (svcConfigID > 0)
            {
                service = serviceManager.LoadByServiceID(svcConfigID);
                if (service == null)
                    return;

                fillQuestionValDropdown(ddlIfValQuestion);
                fillQuestionValDropdown(ddlDefaultValQuestion);

                FillVariablesDropDown();
              
                //Fill question dropdown used to create condition
                FillAllQuestions();

                Guid conditionID = Guid.Empty;
                if (!string.IsNullOrEmpty(Request[ddlExistingVariables.UniqueID]))
                {
                    conditionID = new Guid(Request[ddlExistingVariables.UniqueID]);
                }

                qmVariable = service.QMapVariables.FirstOrDefault(x => x.ID == conditionID);
                BindQuestionControls();
            }

            btDeleteButton.Visible = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Reset multiquestion controls if existing skip condition is selected from the dropdown
            if (hdnSelectedSLogic.Contains("ctrName") && Convert.ToString(hdnSelectedSLogic["ctrName"]) == ddlExistingVariables.ID)
            {
                ResetMultiQuestionControls();
                hdnSelectedSLogic.Clear();
            }
            // Update dictSelectedQuestions to keep track of visible questions and their selected values
            UpdateQuestionDictionary();
        }

        protected override void OnPreRender(EventArgs e)
        {
            //BindConditions();

            pIfConstantValText.Visible = false;
            pIfConstantValDate.Visible = false;
            pIfConstantValUser.Visible = false;

            if (ddlIfValQuestion.SelectedValue.ToLower() == string.Empty)
            {
                if (ddlVariableType.SelectedValue.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                {
                    pIfConstantValUser.Visible = true;
                }
                else if (ddlVariableType.SelectedValue.ToLower() == Constants.ServiceQuestionType.DATETIME)
                {
                    pIfConstantValDate.Visible = true;
                }
                else
                {
                    pIfConstantValText.Visible = true;
                }
            }

            pElseConstantValText.Visible = false;
            pElseConstantValDate.Visible = false;
            pElseConstantValUser.Visible = false;
            if (ddlDefaultValQuestion.SelectedValue.ToLower() == string.Empty)
            {
                if (ddlVariableType.SelectedValue.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                {
                    pElseConstantValUser.Visible = true;
                }
                else if (ddlVariableType.SelectedValue.ToLower() == Constants.ServiceQuestionType.DATETIME)
                {
                    pElseConstantValDate.Visible = true;
                }
                else
                {
                    pElseConstantValText.Visible = true;
                }
            }

            base.OnPreRender(e);
        }

        protected void DDlExistingVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            reset();
            ResetMultiQuestionControls();
            if (qmVariable != null)
            {
                txtTitle.Text = qmVariable.Title;
                txtShortName.Text = qmVariable.ShortName;
                ddlVariableType.SelectedIndex = ddlVariableType.Items.IndexOf(ddlVariableType.Items.FindByValue(qmVariable.Type));

                string pickFrom = qmVariable.DefaultValue.PickFrom;
                pickFrom = pickFrom.Replace(Constants.TokenStart, string.Empty).Replace(Constants.TokenEnd, string.Empty);
                ddlDefaultValQuestion.SelectedIndex = ddlDefaultValQuestion.Items.IndexOf(ddlDefaultValQuestion.Items.FindByValue(pickFrom));
                btDeleteButton.Visible = true;

               
                if (qmVariable.DefaultValue != null && qmVariable.DefaultValue.IsPickFromConstant)
                {
                    if (qmVariable.Type.ToLower() == Constants.ServiceQuestionType.DATETIME)
                    {
                        DateTime date = DateTime.MinValue;
                        DateTime.TryParse(qmVariable.DefaultValue.PickFrom, out date);
                        if (date != DateTime.MinValue)
                        {
                            pElseConstantValDate.Date = date;
                        }
                    }
                    else if (qmVariable.Type.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                    {
                        //SPFieldUserValue users = new SPFieldUserValue(SPContext.Current.Web, qmVariable.DefaultValue.PickFrom);
                        //pElseConstantValUser.UpdateEntities(uHelper.getUsersListFromCollection(users));
                    }
                    else
                    {
                        pElseConstantValText.Text = qmVariable.DefaultValue.PickFrom;
                    }
                }
                BindConditions();
            }
        }

        private void FillVariablesDropDown()
        {
            //Fills conditions dropdown
            foreach (QuestionMapVariable cdn in service.QMapVariables)
            {
                ddlExistingVariables.Items.Add(new ListItem(string.Format("[${0}$] {1}", cdn.ShortName, cdn.Title), cdn.ID.ToString()));
            }
            ddlExistingVariables.Items.Insert(0, new ListItem("<New Variable>", ""));
        }

        private void FillAllQuestions()
        {
            if (service != null && service.Questions != null)
            {
                List<ServiceQuestion> questions = service.Questions.ToList();

                if (questions.Count > 0)
                {
                    questions = questions.Where(x => x.QuestionType.ToLower() != Constants.ServiceQuestionType.DATETIME && x.QuestionType.ToLower() != Constants.ServiceQuestionType.USERFIELD
                    && x.QuestionType.ToLower() != Constants.ServiceQuestionType.ApplicationAccess && x.QuestionType.ToLower() != Constants.ServiceQuestionType.Assets && x.QuestionType.ToLower() != Constants.ServiceQuestionType.Resources).ToList();

                    foreach (ServiceQuestion question in questions)
                    {
                        ddlCQuestions.Items.Add(new ListItem(string.Format("{0} [${1}$]", question.QuestionTitle, question.TokenName), string.Format("{0}", question.TokenName)));
                        // Adding rows to dtQuestions table
                        DataRow drow = dtQuestions.NewRow();
                        drow["Text"] = string.Format("{0} [${1}$]", question.QuestionTitle, question.TokenName);
                        drow["Value"] = string.Format("{0}", question.TokenName);
                        dtQuestions.Rows.Add(drow);
                    }
                }
                ddlCQuestions.Items.Insert(0, new ListItem("--Select Question--", ""));
            }
        }

        private void FillQuestionOperators(string qType)
        {
            List<string> operators = serviceQuestionManager.GetQTOperators(qType);
            foreach (string optr in operators)
            {
                ddlOperators.Items.Add(new ListItem(UGITUtility.AddSpaceBeforeWord(optr), optr));
            }
        }

        protected void DDLCQuestions_SelectedIndexChanged(object sender, EventArgs e)
        {
            pOperatorValue.Controls.Clear();
            pPickerValuePopup.Controls.Clear();
            string sQuestionToken = ddlCQuestions.SelectedValue;
            ddlOperators.Items.Clear();
            if (!string.IsNullOrEmpty(sQuestionToken))
            {
                ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                FillQuestionOperators(sQuestion.QuestionType.ToLower());
                addControlForQuestionValue(sQuestion, string.Empty);
            }
        }

        protected void BtDelete_Click(object sender, EventArgs e)
        {
            if (qmVariable != null)
            {
                service.QMapVariables.Remove(qmVariable);
                serviceManager.Save(service);

                ddlExistingVariables.Items.Clear();
                FillVariablesDropDown();
                reset();
                qmVariable = null;
            }
        }

        private void reset()
        {
            txtTitle.Text = string.Empty;
            ddlCQuestions.ClearSelection();
            ddlOperators.ClearSelection();
            // txtCValue.Text = string.Empty;
            ddlIfValQuestion.ClearSelection();
            ddlDefaultValQuestion.ClearSelection();
            pOperatorValue.Controls.Clear();
            btDeleteButton.Visible = false;
            lbPickedValue.Text = string.Empty;
            pPickerValuePopup.Controls.Clear();
            pPickerValueContainer.Visible = false;
            pOperatorValue.Visible = false;
            txtShortName.Text = string.Empty;
            ddlVariableType.ClearSelection();

            rVariableConditions.DataSource = null;
            rVariableConditions.DataBind();
           
            
            pIfConstantValDate.Visible = false;
            pIfConstantValText.Visible = false;
            pIfConstantValUser.Visible = false;
            //pIfConstantValDate.Date = null;
            pIfConstantValText.Text = string.Empty;
            //pIfConstantValUser.CommaSeparatedAccounts = string.Empty;

            //pElseConstantValDate.SetDate(null);
            pElseConstantValText.Text = string.Empty;
            //pElseConstantValUser.CommaSeparatedAccounts = string.Empty;
        }

        private void addControlForQuestionValue(ServiceQuestion selectedQuestion, string conditionValue)
        {
            Control ctr = serviceQuestionManager.GenerateControlForSkipLogic(selectedQuestion, conditionValue);
            string questionType = selectedQuestion.QuestionType.ToLower();
            if (questionType == Constants.ServiceQuestionType.LOOKUP ||
                questionType == Constants.ServiceQuestionType.MULTICHOICE ||
                questionType == Constants.ServiceQuestionType.REQUESTTYPE ||
                questionType == Constants.ServiceQuestionType.SINGLECHOICE)
            {

                pPickerValueContainer.Visible = true;
                pOperatorValue.Visible = false;
                pPickerValuePopup.Controls.Add(ctr);

                List<string> selectedValues = new List<string>();
                CheckBoxList chkList = ctr as CheckBoxList;
                if (chkList != null)
                {
                    foreach (ListItem item in chkList.Items)
                    {
                        if (item.Selected)
                        {
                            selectedValues.Add(item.Text);
                        }
                    }
                }
                lbPickedValue.Text = string.Join(", ", selectedValues.ToArray());
            }
            else
            {
                pPickerValueContainer.Visible = false;
                pOperatorValue.Visible = true;
                pOperatorValue.Controls.Add(ctr);
            }

        }

        private void fillQuestionValDropdown(DropDownList dropdown)
        {
            string qType = ddlVariableType.SelectedValue;
            if (Request[ddlVariableType.UniqueID.ToString()] != null)
            {
                qType = Request[ddlVariableType.UniqueID.ToString()];
            }

            List<ServiceQuestion> questions = GetQuestionsByVarType(qType);
            if (dropdown.Items.Count <= 0)
            {
                foreach (ServiceQuestion question in questions)
                {
                    dropdown.Items.Add(new ListItem(string.Format("[${0}$] {1}", question.TokenName, question.QuestionTitle), question.TokenName));
                }
                dropdown.Items.Insert(0, new ListItem("--Constant--", ""));
            }
        }

        protected void CVCompatibilityCheck_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (ddlDefaultValQuestion.SelectedValue == string.Empty)
            {
                cvCompatibilityCheck.ErrorMessage = "Please select default value for variable";
                args.IsValid = false;
                return;
            }

            if (ddlDefaultValQuestion.SelectedValue != string.Empty)
            {
                ServiceQuestion elseQuestion = service.Questions.FirstOrDefault(x => x.TokenName == ddlDefaultValQuestion.SelectedValue);
                ServiceQuestion elsebQuestion = serviceQuestionManager.GetBuiltInQuestions().FirstOrDefault(x => x.TokenName == ddlDefaultValQuestion.SelectedValue.Replace("[$", string.Empty).Replace("$]", string.Empty));
                if ((elseQuestion == null && elsebQuestion == null))
                {
                    cvCompatibilityCheck.ErrorMessage = "Question is not found";
                    args.IsValid = false;
                    return;
                }
                if (elseQuestion == null)
                    elseQuestion = elsebQuestion;
            }
        }

        protected void cvShortname_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtShortName.Text.IndexOf("[$") != -1 || txtShortName.Text.IndexOf("$]") != -1)
            {
                cvShortname.ErrorMessage = "[$, $] characters are not allowed.";
                args.IsValid = false;
                return;
            }

            List<ServiceQuestion> questions = service.Questions.Where(x => x.TokenName.ToLower() == txtShortName.Text.ToLower().Trim()).ToList();
            List<QuestionMapVariable> variables = service.QMapVariables.Where(x => x.ShortName.ToLower() == txtShortName.Text.ToLower().Trim()).ToList();

            if ((variables.Count == 1 && ddlExistingVariables.SelectedValue != variables[0].ID.ToString()) ||
                (variables.Count > 1 && ddlExistingVariables.SelectedValue != string.Empty))
            {
                cvShortname.ErrorMessage = "Variable already exists, please enter another name";
                args.IsValid = false;
                return;
            }

            if (questions.Count > 0)
            {
                cvShortname.ErrorMessage = "Question with same name already exists, please enter another name";
                args.IsValid = false;
            }
        }
    
        protected void BtClose_Click(object sender, EventArgs e)
        {
            Context.Cache.Add(string.Format("SVCConfigMapVariable-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btAddCondition_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (ddlCQuestions.SelectedValue == string.Empty)
            {
                cvcConditionValidator.Style.Add(HtmlTextWriterStyle.Display, "block");
                return;
            }

            if (qmVariable != null)
            {
                if (!SaveNewCondition(qmVariable, true))
                {
                    return;
                }      
            }
            else
            {
                if (!SaveVariable())
                {
                    return;
                }

                //Load Variable in drop and select it
                ddlExistingVariables.Items.Clear();
                FillVariablesDropDown();
                ddlExistingVariables.SelectedIndex = ddlExistingVariables.Items.IndexOf(ddlExistingVariables.Items.FindByValue(qmVariable.ID.ToString()));
            }

            //Update Ctr id dictionary 
            //UpdateQuestionCtrIdDic();
            //ResetConditionBox();
            ResetMultiQuestionControls();
            BindConditions();
        }

        private void ResetConditionBox()
        {
            ddlCQuestions.ClearSelection();
            ddlOperators.ClearSelection();
            ddlIfValQuestion.ClearSelection();
            pOperatorValue.Controls.Clear();
            lbPickedValue.Text = string.Empty;
            pPickerValuePopup.Controls.Clear();
            pPickerValueContainer.Visible = false;
            pOperatorValue.Visible = false;
            pIfConstantValDate.Visible = false;
            pIfConstantValText.Visible = false;
            pIfConstantValUser.Visible = false;
            //pIfConstantValDate.SetDate(null);
            pIfConstantValText.Text = string.Empty;
            //pIfConstantValUser.CommaSeparatedAccounts = string.Empty;
            btAddCondition.Text = "Add Condition";
            hdnConditionIndex.Value = string.Empty;

            ResetMultiQuestionControls();
        }

        protected void btSaveAndNewVariable_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (!SaveVariable())
                return;

            qmVariable = null;
            reset();
            
            //Refresh dropdown list to add new Variable
            ddlExistingVariables.Items.Clear();
            FillVariablesDropDown();
            ddlExistingVariables.ClearSelection();
        }

        private bool SaveVariable()
        {
            if (qmVariable == null)
            {
                qmVariable = new QuestionMapVariable();
                service.QMapVariables.Add(qmVariable);
            }

            //Set title
            qmVariable.Title = txtTitle.Text.Trim();
            qmVariable.ShortName = txtShortName.Text;
            qmVariable.Type = ddlVariableType.SelectedValue;

            //set condition
            if (qmVariable.VariableValues == null)
                qmVariable.VariableValues = new List<VariableValue>();

            ServiceQuestion sdefautQuestion = service.Questions.FirstOrDefault(x => x.TokenName == ddlDefaultValQuestion.SelectedValue);
            ServiceQuestion bQuestion = serviceQuestionManager.GetBuiltInQuestions().FirstOrDefault(x => x.TokenName == ddlDefaultValQuestion.SelectedValue.Replace("[$", string.Empty).Replace("$]", string.Empty));
            ServiceQuestion defaultQuestion = sdefautQuestion;
            if (sdefautQuestion == null)
                defaultQuestion = bQuestion;

            VariableValue defaultValue = new VariableValue();
            if (defaultQuestion != null)
                defaultValue.PickFrom = defaultQuestion.TokenName;

            defaultValue.IsPickFromConstant = false;
            if (ddlDefaultValQuestion.SelectedValue == string.Empty)
            {
                defaultValue.IsPickFromConstant = true;
                if (ddlVariableType.SelectedValue.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                {
                    List<UserProfile> userColl = Context.GetUserManager().GetUserInfosById(pElseConstantValUser.GetValues());  // uHelper.GetFieldUserValueCollection(pElseConstantValUser.ResolvedEntities);
                    if (userColl != null && userColl.Count > 0)
                    {
                        defaultValue.PickFrom = string.Format("{0}{1}{2}", userColl[0].Id, Constants.Separator, userColl[0].UserName);
                    }
                }
                else if (ddlVariableType.SelectedValue.ToLower() == Constants.ServiceQuestionType.DATETIME)
                {
                    defaultValue.PickFrom = pElseConstantValDate.Date.ToString();
                }
                else
                {
                    defaultValue.PickFrom = pElseConstantValText.Text.Trim();
                }
            }

            qmVariable.DefaultValue = defaultValue;
            
            //Save condition if any
            if (ddlCQuestions.SelectedValue != string.Empty)
            {
                if (!SaveNewCondition(qmVariable, false))
                {
                    return false;
                }
            }

            serviceManager.Save(service);

            return true;
        }

        private bool SaveNewCondition(QuestionMapVariable qmVariable, bool saveCondition)
        {
            if (qmVariable == null)
                return false;

            VariableValue ifValue = new VariableValue();
            ifValue.Conditions = new List<WhereExpression>();
            WhereExpression whereClause = new WhereExpression();
            whrExpressions = GenerateExpressionsList();

            if (whrExpressions.Any(x => string.IsNullOrEmpty(x.Variable)))
            {
                spnQuestion.Attributes.Remove("class");
                return false;
            }

            if (whrExpressions != null && whrExpressions.Count > 0)
                ifValue.Conditions = whrExpressions;

            ifValue.PickFrom = ddlIfValQuestion.SelectedValue;

            ifValue.IsPickFromConstant = false;
            if (ddlIfValQuestion.SelectedValue == string.Empty)
            {
                ifValue.IsPickFromConstant = true;
                if (ddlVariableType.SelectedValue.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                {
                    List<UserProfile> userColl = Context.GetUserManager().GetUserInfosById(pIfConstantValUser.GetValues()); // uHelper.GetFieldUserValueCollection(pIfConstantValUser.ResolvedEntities);
                    if (userColl != null && userColl.Count > 0)
                    {
                        ifValue.PickFrom = string.Format("{0}{1}{2}", userColl[0].Id, Constants.Separator, userColl[0].UserName);
                    }
                }
                else if (ddlVariableType.SelectedValue.ToLower() == Constants.ServiceQuestionType.DATETIME)
                {
                    ifValue.PickFrom = pIfConstantValDate.Date.ToString();
                }
                else
                {
                    ifValue.PickFrom = pIfConstantValText.Text.Trim();
                }
            }

            VariableValue variableVal = qmVariable.VariableValues.FirstOrDefault(x => x.Conditions.Count > 0 && x.Conditions[0].Variable == whereClause.Variable
                && x.Conditions[0].Operator == whereClause.Operator && Convert.ToString(x.Conditions[0].Value) == whereClause.Value);

             //fetch existing condition if it is in edit mode.
             VariableValue existingValue = null;
             int index = -1;
             if (int.TryParse(hdnConditionIndex.Value, out index))
             {
                 existingValue = qmVariable.VariableValues[index];
             }

             if (variableVal != null && existingValue != null && variableVal != existingValue)
             {
                 cvcConditionValidator.ErrorMessage = "Condition already exists";
                 cvcConditionValidator.Style.Add(HtmlTextWriterStyle.Display, "block");
                 return false;
             }

             //Edit existing codition or added new one
             if (existingValue != null)
             {
                 existingValue = ifValue;
                 qmVariable.VariableValues[index] = ifValue;
             }
             else
             {
                 qmVariable.VariableValues.Add(ifValue);
             }

             if (saveCondition)
             {
                 serviceManager.Save(service);
                 ResetConditionBox();
             }
            return true;
        }

        private List<ServiceQuestion> GetQuestionsByVarType(string varType)
        {
            List<ServiceQuestion> reqQuestions = new List<ServiceQuestion>();

            List<ServiceQuestion> questions = service.Questions.Where(x => x.QuestionType.ToLower() != Constants.ServiceQuestionType.REQUESTTYPE && x.QuestionType.ToLower() != Constants.ServiceQuestionType.LOOKUP).ToList();
            List<ServiceQuestion> bQuestions = serviceQuestionManager.GetBuiltInQuestions().Where(x => x.QuestionType.ToLower() != Constants.ServiceQuestionType.LOOKUP).ToList();

            if (varType.ToLower() == "textbox")
            {
                reqQuestions.AddRange(bQuestions);
                reqQuestions.AddRange(questions);
            }
            else if (varType.ToLower() == "numeric")
            {
                questions = questions.Where(x =>x.QuestionType.ToLower() == Constants.ServiceQuestionType.Number).ToList();
                bQuestions = bQuestions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.Number).ToList();

                reqQuestions.AddRange(bQuestions);
                reqQuestions.AddRange(questions);
            }
            else if (varType.ToLower() == "userfield")
            {
                questions = questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD).ToList();
                bQuestions = bQuestions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.USERFIELD).ToList();

                reqQuestions.AddRange(bQuestions);
                reqQuestions.AddRange(questions);
            }
            else if (varType.ToLower() == "datetime")
            {
                questions = questions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME).ToList();
                bQuestions = bQuestions.Where(x => x.QuestionType.ToLower() == Constants.ServiceQuestionType.DATETIME).ToList();

                reqQuestions.AddRange(bQuestions);
                reqQuestions.AddRange(questions);
            }

            return reqQuestions;
        }

        protected void ddlVariableType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlIfValQuestion.Items.Clear();
            ddlDefaultValQuestion.Items.Clear();

            fillQuestionValDropdown(ddlIfValQuestion);
            fillQuestionValDropdown(ddlDefaultValQuestion);

            if (qmVariable != null)
            {
                qmVariable.VariableValues = new List<VariableValue>();
                serviceManager.Save(service);
            }
        }

        private void BindConditions()
        {
            if (qmVariable != null)
            {
                CreateExpressionSchema(qmVariable);
                rVariableConditions.DataSource = dtExpresstionResult;
                rVariableConditions.DataBind();
            }
            else
            {
                rVariableConditions.DataSource = null;
                rVariableConditions.DataBind();
            }
        }
        protected void rVariableConditions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRow varVal = ((DataRowView)e.Item.DataItem).Row;

                Label lbCondition = (Label)e.Item.FindControl("lbCondition");
                Label lbConditionValue = (Label)e.Item.FindControl("lbConditionValue");
                
                ImageButton btDeleteVarCondition = (ImageButton)e.Item.FindControl("btDeleteVarCondition");
                btDeleteVarCondition.CommandArgument = Convert.ToString(dtExpresstionResult.Rows.IndexOf(varVal));

                ImageButton btEditVarCondition = (ImageButton)e.Item.FindControl("btEditVarCondition");
                btEditVarCondition.CommandArgument = Convert.ToString(dtExpresstionResult.Rows.IndexOf(varVal));

                if (varVal != null)
                {
                    string valueExpress = Convert.ToString(varVal["Express"]);
                    string pickFrom = Convert.ToString(varVal["Value"]);
                    bool isPickFromConstant = UGITUtility.StringToBoolean(varVal["IsPickFromConstant"]);

                    lbCondition.Text = string.Format("({0})", valueExpress);
                    lbConditionValue.Text = pickFrom;
                    
                    if (isPickFromConstant && !string.IsNullOrEmpty(pickFrom))
                    {
                        if (qmVariable.Type.ToLower() == Constants.ServiceQuestionType.DATETIME)
                        {
                            DateTime date = DateTime.MinValue;
                            DateTime.TryParse(pickFrom, out date);

                            if (date != DateTime.MinValue)
                            {
                                lbConditionValue.Text = date.ToString("MMM-dd-yyyy");
                            }
                        }
                        else if (qmVariable.Type.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                        {
                            try
                            {
                                //SPFieldUserValue users = new SPFieldUserValue(SPContext.Current.Web, varVal.PickFrom);
                                //lbConditionValue.Text = users.LookupValue;
                            }
                            catch
                            {
                                lbConditionValue.Text = "Problem in showing data";
                                lbConditionValue.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                        else
                        {
                            lbConditionValue.Text = pickFrom;
                        }
                    }
                }
            }
        }

        protected void btDeleteVarCondition_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            if (qmVariable != null && qmVariable.VariableValues != null && qmVariable.VariableValues.Count > 0)
            {
                int index = -1;
                int.TryParse(button.CommandArgument, out index);
                qmVariable.VariableValues.RemoveAt(index);
                serviceManager.Save(service);
                hdnConditionIndex.Value = string.Empty;
            }

            ResetMultiQuestionControls();
            BindConditions();
        }

        protected void btEditVarCondition_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            if (qmVariable != null && qmVariable.VariableValues != null)
            {
                int index = -1;
                if (!int.TryParse(button.CommandArgument, out index))
                {
                    return;
                }

                VariableValue condition = qmVariable.VariableValues[index];
                if (condition != null)
                {
                    hdnConditionIndex.Value = index.ToString();
                    btAddCondition.Text = "Save";
                    whrExpressions = condition.Conditions;
                    ResetMultiQuestionControls();
                    UpdateQuestionCtrIdDic(condition);
                    BindCtrForSelectedSkipLogic();
                    // Create skip logic expression
                    string expression = GetSkipLogicExpression(whrExpressions);
                    skipLogicExpression.Text = expression;

                    ddlOperators.SelectedIndex = ddlOperators.Items.IndexOf(ddlOperators.Items.FindByValue(condition.Conditions[0].Operator));
                    ddlIfValQuestion.SelectedIndex = ddlIfValQuestion.Items.IndexOf(ddlIfValQuestion.Items.FindByValue(condition.PickFrom));

                    if (qmVariable.DefaultValue != null && condition.IsPickFromConstant)
                    {
                        if (qmVariable.Type.ToLower() == Constants.ServiceQuestionType.DATETIME)
                        {
                            DateTime date = DateTime.MinValue;
                            DateTime.TryParse(condition.PickFrom, out date);
                            if (date != DateTime.MinValue)
                            {
                                pIfConstantValDate.Date = date;
                            }
                        }
                        else if (qmVariable.Type.ToLower() == Constants.ServiceQuestionType.USERFIELD)
                        {
                            //SPFieldUserValue users = new SPFieldUserValue(SPContext.Current.Web, condition.PickFrom);
                            //pIfConstantValUser.UpdateEntities(uHelper.getUsersListFromCollection(users));
                        }
                        else
                        {
                            pIfConstantValText.Text = condition.PickFrom;
                        }
                    }
                }
            }
        }

        #region Method to create controls for visible Questions 
        /// <summary>
        /// This method is used to initialize and create controls for visible question controls
        /// </summary>
        private void BindQuestionControls()
        {
            string selectedQuestion = string.Empty;
            if (!string.IsNullOrEmpty(Request[ddlCQuestions.UniqueID]))
            {
                selectedQuestion = Request[ddlCQuestions.UniqueID];
                ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == selectedQuestion);
                if (sQuestion != null)
                {
                    AddControlForQuestionValue(sQuestion, string.Empty, pPickerValueContainer, pOperatorValue, pPickerValuePopup, lbPickedValue);
                }
            }

            if (!string.IsNullOrEmpty(Request[ddlQuestion2.UniqueID]))
            {
                selectedQuestion = Request[ddlQuestion2.UniqueID];
                ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == selectedQuestion);
                if (sQuestion != null)
                {
                    AddControlForQuestionValue(sQuestion, string.Empty, pnlPickerValueContainer2, pnlOperatorValue2, pnlPickerValuePopup2, lblPickedValue2);
                }
            }
            if (!string.IsNullOrEmpty(Request[ddlQuestion3.UniqueID]))
            {
                selectedQuestion = Request[ddlQuestion3.UniqueID];
                ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == selectedQuestion);
                if (sQuestion != null)
                {
                    AddControlForQuestionValue(sQuestion, string.Empty, pnlPickerValueContainer3, pnlOperatorValue3, pnlPickerValuePopup3, lblPickedValue3);
                }
            }
            if (!string.IsNullOrEmpty(Request[ddlQuestion4.UniqueID]))
            {
                selectedQuestion = Request[ddlQuestion4.UniqueID];
                ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == selectedQuestion);
                if (sQuestion != null)
                {
                    AddControlForQuestionValue(sQuestion, string.Empty, pnlPickerValueContainer4, pnlOperatorValue4, pnlPickerValuePopup4, lblPickedValue4);
                }
            }
            if (!string.IsNullOrEmpty(Request[ddlQuestion5.UniqueID]))
            {
                selectedQuestion = Request[ddlQuestion5.UniqueID];
                ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == selectedQuestion);
                if (sQuestion != null)
                {
                    AddControlForQuestionValue(sQuestion, string.Empty, pnlPickerValueContainer5, pnlOperatorValue5, pnlPickerValuePopup5, lblPickedValue5);
                }
            }
        }
        #endregion Method to create controls for visible Questions

        #region Method to create controls when user changes any Question
        /// <summary>
        /// This method is used to bind operators and to create custom control for changed question
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlQuestion = (sender as DropDownList);
            string sQuestionToken = string.Empty;

            switch (ddlQuestion.ID)
            {
                case "ddlCQuestions":
                    pOperatorValue.Controls.Clear();
                    pPickerValuePopup.Controls.Clear();
                    sQuestionToken = ddlCQuestions.SelectedValue;
                    ddlOperators.Items.Clear();
                    if (!string.IsNullOrEmpty(sQuestionToken))
                    {
                        ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                        FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperators);
                        AddControlForQuestionValue(sQuestion, string.Empty, pPickerValueContainer, pOperatorValue, pPickerValuePopup, lbPickedValue);

                        if (!isMandatoryAttachmentCondition)
                            chkBox1.Disabled = false;
                    }
                    else
                    {
                        chkBox1.Disabled = true;
                    }

                    break;
                case "ddlQuestion2":
                    pnlOperatorValue2.Controls.Clear();
                    pnlPickerValuePopup2.Controls.Clear();
                    sQuestionToken = ddlQuestion2.SelectedValue;
                    ddlOperator2.Items.Clear();
                    if (!string.IsNullOrEmpty(sQuestionToken))
                    {
                        ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                        FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperator2);
                        AddControlForQuestionValue(sQuestion, string.Empty, pnlPickerValueContainer2, pnlOperatorValue2, pnlPickerValuePopup2, lblPickedValue2);
                        chkBox2.Disabled = false;
                    }
                    else
                    {
                        chkBox2.Disabled = true;
                    }

                    break;
                case "ddlQuestion3":
                    pnlOperatorValue3.Controls.Clear();
                    pnlPickerValuePopup3.Controls.Clear();
                    sQuestionToken = ddlQuestion3.SelectedValue;
                    ddlOperator3.Items.Clear();
                    if (!string.IsNullOrEmpty(sQuestionToken))
                    {
                        ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                        FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperator3);
                        AddControlForQuestionValue(sQuestion, string.Empty, pnlPickerValueContainer3, pnlOperatorValue3, pnlPickerValuePopup3, lblPickedValue3);
                        chkBox3.Disabled = false;
                    }
                    else
                    {
                        chkBox3.Disabled = true;
                    }

                    break;
                case "ddlQuestion4":
                    pnlOperatorValue4.Controls.Clear();
                    pnlPickerValuePopup4.Controls.Clear();
                    sQuestionToken = ddlQuestion4.SelectedValue;
                    ddlOperator4.Items.Clear();
                    if (!string.IsNullOrEmpty(sQuestionToken))
                    {
                        ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                        FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperator4);
                        AddControlForQuestionValue(sQuestion, string.Empty, pnlPickerValueContainer4, pnlOperatorValue4, pnlPickerValuePopup4, lblPickedValue4);
                        chkBox4.Disabled = false;
                    }
                    else
                    {
                        chkBox4.Disabled = true;
                    }

                    break;
                case "ddlQuestion5":
                    pnlOperatorValue5.Controls.Clear();
                    pnlPickerValuePopup5.Controls.Clear();
                    sQuestionToken = ddlQuestion5.SelectedValue;
                    ddlOperator5.Items.Clear();
                    if (!string.IsNullOrEmpty(sQuestionToken))
                    {
                        ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                        FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperator5);
                        AddControlForQuestionValue(sQuestion, string.Empty, pnlPickerValueContainer5, pnlOperatorValue5, pnlPickerValuePopup5, lblPickedValue5);
                        chkBox5.Disabled = false;
                    }
                    else
                    {
                        chkBox5.Disabled = true;
                    }

                    break;
                default:
                    break;
            }

            // Update dictionary for current question value
            if (dictSelectedQuestions.Keys.Contains(ddlQuestion.ID))
                dictSelectedQuestions[ddlQuestion.ID] = sQuestionToken;
            else
                dictSelectedQuestions.Add(ddlQuestion.ID, sQuestionToken);

            // Rebind all controls
            RebindControls(true);
        }
        #endregion Method to create controls when user changes any Question

        #region Method to add a new Question 
        /// <summary>
        /// This method is used to new question when user click on add button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            // Add a new question
            AddNewQuestion();

            // Rebind all controls
            RebindControls(true);
        }

        private void AddNewQuestion()
        {
            bool questionsExist = dtQuestions != null && dtQuestions.Rows.Count > 0;

            if (!pnlQuestion2.Visible)
            {
                pnlQuestion2.Visible = true;

                if (questionsExist)
                {
                    ddlQuestion2.DataSource = GetQuestions();
                    ddlQuestion2.DataTextField = "Text";
                    ddlQuestion2.DataValueField = "Value";
                    ddlQuestion2.DataBind();
                    dictSelectedQuestions.Add(ddlQuestion2.ID, ddlQuestion2.SelectedValue);
                    ibtnAdd1.Visible = ibtnAdd3.Visible = ibtnAdd4.Visible = false;
                    ibtnAdd2.Visible = true;
                    chkBox2.Disabled = true;

                    UpdateQuestionControls();
                }
                return;
            }
            if (!pnlQuestion3.Visible)
            {
                pnlQuestion3.Visible = true;

                if (questionsExist)
                {
                    ddlQuestion3.DataSource = GetQuestions();
                    ddlQuestion3.DataTextField = "Text";
                    ddlQuestion3.DataValueField = "Value";
                    ddlQuestion3.DataBind();
                    dictSelectedQuestions.Add(ddlQuestion3.ID, ddlQuestion3.SelectedValue);
                    ibtnAdd1.Visible = ibtnAdd2.Visible = ibtnAdd4.Visible = false;
                    ibtnAdd3.Visible = true;
                    chkBox3.Disabled = true;

                    UpdateQuestionControls();
                }
                return;
            }
            if (!pnlQuestion4.Visible)
            {
                pnlQuestion4.Visible = true;

                if (questionsExist)
                {
                    ddlQuestion4.DataSource = GetQuestions();
                    ddlQuestion4.DataTextField = "Text";
                    ddlQuestion4.DataValueField = "Value";
                    ddlQuestion4.DataBind();
                    dictSelectedQuestions.Add(ddlQuestion4.ID, ddlQuestion4.SelectedValue);
                    ibtnAdd1.Visible = ibtnAdd2.Visible = ibtnAdd3.Visible = false;
                    ibtnAdd4.Visible = true;
                    chkBox4.Disabled = true;

                    UpdateQuestionControls();
                }
                return;
            }
            if (!pnlQuestion5.Visible)
            {
                pnlQuestion5.Visible = true;
                if (questionsExist)
                {
                    ddlQuestion5.DataSource = GetQuestions();
                    ddlQuestion5.DataTextField = "Text";
                    ddlQuestion5.DataValueField = "Value";
                    ddlQuestion5.DataBind();
                    dictSelectedQuestions.Add(ddlQuestion5.ID, ddlQuestion5.SelectedValue);
                    ibtnAdd1.Visible = ibtnAdd2.Visible = ibtnAdd3.Visible = ibtnAdd4.Visible = false;
                    chkBox5.Disabled = true;

                    UpdateQuestionControls();
                }
                return;
            }
        }
        #endregion Method to add a new Question 

        #region Method to remove a Question from the multiple question control
        /// <summary>
        /// Method to remove a Question from the multiple question control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRemoveQuestion_Click(object sender, ImageClickEventArgs e)
        {
            switch ((sender as ImageButton).ID)
            {
                case "ibtnRemoveQues2":
                    if (dictSelectedQuestions.Keys.Contains(ddlQuestion2.ID))
                        dictSelectedQuestions.Remove(ddlQuestion2.ID);

                    break;
                case "ibtnRemoveQues3":
                    if (dictSelectedQuestions.Keys.Contains(ddlQuestion3.ID))
                        dictSelectedQuestions.Remove(ddlQuestion3.ID);

                    break;
                case "ibtnRemoveQues4":
                    if (dictSelectedQuestions.Keys.Contains(ddlQuestion4.ID))
                        dictSelectedQuestions.Remove(ddlQuestion4.ID);

                    break;
                case "ibtnRemoveQues5":
                    if (dictSelectedQuestions.Keys.Contains(ddlQuestion5.ID))
                        dictSelectedQuestions.Remove(ddlQuestion5.ID);

                    break;
                default:
                    break;
            }

            ResetMultiQuestionControls();
        }
        #endregion Method to remove a Question from the multiple question control

        #region Method to bind Operator dropdown data for the selected Question
        /// <summary>
        /// Method to bind Operator dropdown data for the selected Question
        /// </summary>
        /// <param name="qType"></param>
        /// <param name="ddoperators"></param>
        private void FillQuestionOperators(string qType, DropDownList ddoperators)
        {
            List<string> operators = serviceQuestionManager.GetQTOperators(qType);

            foreach (string optr in operators)
            {
                ddoperators.Items.Add(new ListItem(UGITUtility.AddSpaceBeforeWord(optr), optr));
            }
        }
        #endregion Method to bind Operator dropdown data for the selected Question

        #region Method to generate control and bind data for the selected question
        /// <summary>
        /// This method is used to generate control and bind data for the selected question
        /// </summary>
        /// <param name="selectedQuestion"></param>
        /// <param name="conditionValue"></param>
        /// <param name="pnlPickerValueContainer"></param>
        /// <param name="pnlOperatorValue"></param>
        /// <param name="pnlPickerValuePopup"></param>
        /// <param name="lblPickedValue"></param>
        private void AddControlForQuestionValue(ServiceQuestion selectedQuestion, string conditionValue, Panel pnlPickerValueContainer, Panel pnlOperatorValue, Panel pnlPickerValuePopup, Label lblPickedValue)
        {
            Control ctr = ExtendedControls.GenerateControlForSkipLogic(selectedQuestion, conditionValue);
            string questionType = selectedQuestion.QuestionType.ToLower();
            pnlOperatorValue.Attributes.Remove("style");

            if (questionType == Constants.ServiceQuestionType.LOOKUP ||
                questionType == Constants.ServiceQuestionType.MULTICHOICE ||
                questionType == Constants.ServiceQuestionType.REQUESTTYPE ||
                questionType == Constants.ServiceQuestionType.SINGLECHOICE)
            {

                pnlPickerValueContainer.Visible = true;
                pnlOperatorValue.Visible = false;
                pnlPickerValuePopup.Controls.Add(ctr);

                List<string> selectedValues = new List<string>();

                if (ctr is CheckBoxList)
                {
                    CheckBoxList chkList = (CheckBoxList)ctr;

                    if (chkList != null)
                    {
                        foreach (ListItem item in chkList.Items)
                        {
                            if (item.Selected)
                            {
                                selectedValues.Add(item.Text);
                            }
                        }
                    }
                }
                else if (ctr is LookUpValueBox)
                {
                    LookUpValueBox valueBox = (LookUpValueBox)ctr;
                    selectedValues.Add(string.Format("{0}", valueBox.GetText()));
                }
                else if (ctr is LookupValueBoxEdit)
                {
                    LookupValueBoxEdit valueBoxEdit = (LookupValueBoxEdit)ctr;
                    selectedValues.Add(string.Format("{0}", valueBoxEdit.GetText()));
                }
                else
                {
                    lblPickedValue.Text = ExtendedControls.ReadSelectedValueSkipLogic(selectedQuestion, ctr, string.Empty).Replace("~", ", ");
                }

                if (selectedValues != null && selectedValues.Count > 0)
                    lblPickedValue.Text = string.Join(", ", selectedValues.ToArray());
            }
            else
            {
                pnlPickerValueContainer.Visible = false;

                if (questionType == Constants.ServiceQuestionType.USERFIELD)
                    pnlOperatorValue.Attributes.Add("style", "width:75%;");

                pnlOperatorValue.Visible = true;
                pnlOperatorValue.Controls.Add(ctr);
            }
        }
        #endregion Method to generate control and bind data for the selected question

        #region Method to update the Questions data for Question dropdown controls
        /// <summary>
        /// This method is used to remove the questions from Question dropdowns which are already selected by other question controls
        /// </summary>
        private void UpdateQuestionControls()
        {
            if (dictSelectedQuestions.Count == 0)
                return;

            foreach (var item in dictSelectedQuestions)
            {
                // Show/Hide question panels
                switch (item.Key)
                {
                    case "ddlQuestion2":
                        pnlQuestion2.Visible = true;
                        ibtnAdd1.Visible = ibtnAdd3.Visible = ibtnAdd4.Visible = false;
                        ibtnAdd2.Visible = true;
                        break;
                    case "ddlQuestion3":
                        pnlQuestion3.Visible = true;
                        ibtnAdd1.Visible = ibtnAdd2.Visible = ibtnAdd4.Visible = false;
                        ibtnAdd3.Visible = true;
                        break;
                    case "ddlQuestion4":
                        pnlQuestion4.Visible = true;
                        ibtnAdd1.Visible = ibtnAdd2.Visible = ibtnAdd3.Visible = false;
                        ibtnAdd4.Visible = true;
                        break;
                    case "ddlQuestion5":
                        pnlQuestion5.Visible = true;
                        ibtnAdd1.Visible = ibtnAdd2.Visible = ibtnAdd3.Visible = ibtnAdd4.Visible = false;
                        break;
                    default:
                        break;
                }

                DataRow[] rows = null;

                // Get selected question tokens in a list
                List<string> otherSelectedQuestions = dictSelectedQuestions.AsEnumerable().Where(x => x.Key != item.Key).Select(y => y.Value).ToList();

                if (otherSelectedQuestions == null || otherSelectedQuestions.Count == 0)
                    break;

                // Filter out the questions data and bind this to the current dropdown
                rows = dtQuestions.AsEnumerable().Where(x => !otherSelectedQuestions.Contains(x["Value"])).ToArray();

                if (rows == null || rows.Length == 0)
                    break;

                DropDownList ddList = this.FindControl(item.Key) as DropDownList;

                if (ddList != null)
                {
                    if (ddList.SelectedIndex > 0)
                    {
                        ddList.ClearSelection();
                        ddList.SelectedIndex = 0;
                    }

                    DataTable dtSource = rows.CopyToDataTable();
                    DataRow drow = dtSource.NewRow();
                    drow["Text"] = "--Select Question--";
                    drow["Value"] = "";
                    dtSource.Rows.InsertAt(drow, 0);

                    ddList.DataSource = dtSource;
                    ddList.DataTextField = "Text";
                    ddList.DataValueField = "Value";
                    ddList.DataBind();

                    ddList.SelectedIndex = ddList.Items.IndexOf(ddList.Items.FindByValue(item.Value));
                }
            }
        }
        #endregion Method to update the Questions data for Question dropdown controls

        #region Method to update the dictionary for the selected questions and their values
        /// <summary>
        /// Method to update the dictionary for the selected questions and their values
        /// </summary>
        private void UpdateQuestionDictionary()
        {
            if (dictSelectedQuestions.Keys.Contains(ddlCQuestions.ID))
                dictSelectedQuestions[ddlCQuestions.ID] = ddlCQuestions.SelectedValue;
            else
                dictSelectedQuestions.Add(ddlCQuestions.ID, ddlCQuestions.SelectedValue);       // The first question control is visible by default

            if (!isMandatoryAttachmentCondition)
                ibtnAdd1.Visible = true;

            if (pnlQuestion2.Visible)
            {
                if (dictSelectedQuestions.Keys.Contains(ddlQuestion2.ID))
                    dictSelectedQuestions[ddlQuestion2.ID] = ddlQuestion2.SelectedValue;
                else
                    dictSelectedQuestions.Add(ddlQuestion2.ID, ddlQuestion2.SelectedValue);

                ibtnAdd1.Visible = ibtnAdd3.Visible = ibtnAdd4.Visible = false;
                ibtnAdd2.Visible = true;
            }

            if (pnlQuestion3.Visible)
            {
                if (dictSelectedQuestions.Keys.Contains(ddlQuestion3.ID))
                    dictSelectedQuestions[ddlQuestion3.ID] = ddlQuestion3.SelectedValue;
                else
                    dictSelectedQuestions.Add(ddlQuestion3.ID, ddlQuestion3.SelectedValue);

                ibtnAdd1.Visible = ibtnAdd2.Visible = ibtnAdd4.Visible = false;
                ibtnAdd3.Visible = true;
            }

            if (pnlQuestion4.Visible)
            {
                if (dictSelectedQuestions.Keys.Contains(ddlQuestion4.ID))
                    dictSelectedQuestions[ddlQuestion4.ID] = ddlQuestion4.SelectedValue;
                else
                    dictSelectedQuestions.Add(ddlQuestion4.ID, ddlQuestion4.SelectedValue);

                ibtnAdd1.Visible = ibtnAdd2.Visible = ibtnAdd3.Visible = false;
                ibtnAdd4.Visible = true;
            }

            if (pnlQuestion5.Visible)
            {
                if (dictSelectedQuestions.Keys.Contains(ddlQuestion5.ID))
                    dictSelectedQuestions[ddlQuestion5.ID] = ddlQuestion5.SelectedValue;
                else
                    dictSelectedQuestions.Add(ddlQuestion5.ID, ddlQuestion5.SelectedValue);

                ibtnAdd1.Visible = ibtnAdd2.Visible = ibtnAdd3.Visible = ibtnAdd4.Visible = false;
            }
        }
        #endregion Method to update the dictionary for the selected questions and their values

        #region Methods to Reset all the controls for Skip Logic
        private void Reset()
        {
            ddlExistingVariables.Items.Clear();
            //Fills conditions dropdown
            foreach (ServiceSectionCondition cdn in service.SkipSectionCondition)
            {
                ddlExistingVariables.Items.Add(new ListItem(cdn.Title, cdn.ID.ToString()));
            }
            ddlExistingVariables.Items.Insert(0, new ListItem("<New Condition>", ""));

            txtTitle.Text = string.Empty;
            ddlCQuestions.ClearSelection();
            ddlOperators.Items.Clear();
            //ddlSectionList.ClearSelection();
            pOperatorValue.Controls.Clear();
            btDeleteButton.Visible = false;
            lbPickedValue.Text = string.Empty;
            pPickerValuePopup.Controls.Clear();
            pPickerValueContainer.Visible = false;
            pOperatorValue.Visible = false;

            ResetMultiQuestionControls();
        }

        private void ResetMultiQuestionControls()
        {
            ddlCQuestions.ClearSelection();
            ddlIfValQuestion.ClearSelection();
            pOperatorValue.Controls.Clear();
            ddlOperators.Items.Clear();

            if (!isMandatoryAttachmentCondition)
                ibtnAdd1.Visible = true;

            ibtnAdd2.Visible = ibtnAdd3.Visible = ibtnAdd4.Visible = false;
            chkBox1.Checked = chkBox2.Checked = chkBox3.Checked = chkBox4.Checked = chkBox5.Checked = false;
            chkBox1.Disabled = true;

            chkBox1.Attributes.Remove("itemid");
            chkBox2.Attributes.Remove("itemid");
            chkBox3.Attributes.Remove("itemid");
            chkBox4.Attributes.Remove("itemid");
            chkBox5.Attributes.Remove("itemid");
            chkBox1.Attributes.Remove("parentid");
            chkBox2.Attributes.Remove("parentid");
            chkBox3.Attributes.Remove("parentid");
            chkBox4.Attributes.Remove("parentid");
            chkBox5.Attributes.Remove("parentid");

            ibtnRemoveGroup1.Attributes.Remove("itemid");
            ibtnRemoveGroup2.Attributes.Remove("itemid");
            ibtnRemoveGroup3.Attributes.Remove("itemid");
            ibtnRemoveGroup4.Attributes.Remove("itemid");
            ibtnRemoveGroup5.Attributes.Remove("itemid");
            ibtnRemoveGroup1.CssClass = ibtnRemoveGroup2.CssClass = ibtnRemoveGroup3.CssClass = ibtnRemoveGroup4.CssClass = ibtnRemoveGroup5.CssClass = "hide";
            ibtnRemoveGroup1.OnClientClick = ibtnRemoveGroup2.OnClientClick = ibtnRemoveGroup3.OnClientClick = ibtnRemoveGroup4.OnClientClick = ibtnRemoveGroup5.OnClientClick = string.Empty;

            pnlQuestion2.Visible = pnlQuestion3.Visible = pnlQuestion4.Visible = pnlQuestion5.Visible = false;
            ddlCondition1.ClearSelection();
            ddlCondition2.ClearSelection();
            ddlCondition3.ClearSelection();
            ddlCondition4.ClearSelection();
            ddlCondition5.ClearSelection();
            ddlQuestion2.ClearSelection();
            ddlQuestion3.ClearSelection();
            ddlQuestion4.ClearSelection();
            ddlQuestion5.ClearSelection();
            pnlPickerValueContainer2.Visible = pnlPickerValueContainer3.Visible = pnlPickerValueContainer4.Visible = pnlPickerValueContainer5.Visible = false;
            pnlOperatorValue2.Controls.Clear();
            pnlOperatorValue3.Controls.Clear();
            pnlOperatorValue4.Controls.Clear();
            pnlOperatorValue5.Controls.Clear();
            pnlOperatorValue2.Visible = pnlOperatorValue3.Visible = pnlOperatorValue4.Visible = pnlOperatorValue5.Visible = false;
            pnlPickerValuePopup2.Controls.Clear();
            pnlPickerValuePopup3.Controls.Clear();
            pnlPickerValuePopup4.Controls.Clear();
            pnlPickerValuePopup5.Controls.Clear();
            ddlOperator2.Items.Clear();
            ddlOperator3.Items.Clear();
            ddlOperator4.Items.Clear();
            ddlOperator5.Items.Clear();
            lblPickedValue2.Text = lblPickedValue3.Text = lblPickedValue4.Text = lblPickedValue5.Text = skipLogicExpression.Text = string.Empty;
            //dictSelectedQuestions.Clear();
        }
        #endregion Methods to Reset all the controls for Skip Logic

        #region Method to bind controls for selected Skip Logic
        /// <summary>
        /// This method is used to bind data to the controls for selected questions when user select any saved Skip Logic
        /// </summary>
        private void BindCtrForSelectedSkipLogic()
        {
            DropDownList ddlQuestion = null;
            string sQuestionToken = string.Empty;

            for (int index = 1; index <= whrExpressions.Count; index++)
            {
                switch (index)
                {
                    case 1:
                        pOperatorValue.Controls.Clear();
                        pPickerValuePopup.Controls.Clear();
                        ddlOperators.Items.Clear();
                        ddlCQuestions.ClearSelection();
                        ddlCQuestions.Items.Clear();

                        ddlCQuestions.DataSource = GetQuestions();
                        ddlCQuestions.DataTextField = "Text";
                        ddlCQuestions.DataValueField = "Value";
                        ddlCQuestions.DataBind();
                        ddlCQuestions.Items.Insert(0, new ListItem("--Select Question--", ""));
                        ddlCQuestions.SelectedIndex = ddlCQuestions.Items.IndexOf(ddlCQuestions.Items.FindByValue(whrExpressions[index - 1].Variable));
                        sQuestionToken = ddlCQuestions.SelectedValue;
                        ddlQuestion = ddlCQuestions;
                        chkBox1.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                        chkBox1.Attributes.Add("parentid", Convert.ToString(whrExpressions[index - 1].ParentId));

                        if (whrExpressions[index - 1].ParentId > 0)
                        {
                            chkBox1.Disabled = true;
                            ibtnRemoveGroup1.CssClass = "hide";
                            ibtnRemoveGroup1.Attributes.Add("itemid", "");
                        }
                        else if (IsGroupParent(whrExpressions[index - 1].Id))
                        {
                            chkBox1.Disabled = true;
                            ibtnRemoveGroup1.CssClass = "remove-group";
                            ibtnRemoveGroup1.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                            ibtnRemoveGroup1.OnClientClick = "btRemoveGroup_click(" + whrExpressions[index - 1].Id + ")";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sQuestionToken))
                                chkBox1.Disabled = true;
                            else if (!isMandatoryAttachmentCondition)
                                chkBox1.Disabled = false;

                            ibtnRemoveGroup1.CssClass = "hide";
                            ibtnRemoveGroup1.Attributes.Add("itemid", "");
                            ibtnRemoveGroup1.OnClientClick = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(sQuestionToken))
                        {
                            ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                            FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperators);
                            ddlOperators.SelectedIndex = ddlOperators.Items.IndexOf(ddlOperators.Items.FindByValue(whrExpressions[index - 1].Operator));
                            AddControlForQuestionValue(sQuestion, whrExpressions[index - 1].Value, pPickerValueContainer, pOperatorValue, pPickerValuePopup, lbPickedValue);
                        }

                        break;
                    case 2:
                        pnlOperatorValue2.Controls.Clear();
                        pnlPickerValuePopup2.Controls.Clear();
                        ddlOperator2.Items.Clear();
                        ddlQuestion2.ClearSelection();
                        ddlQuestion2.Items.Clear();
                        ddlCondition2.SelectedIndex = ddlCondition2.Items.IndexOf(ddlCondition2.Items.FindByValue(whrExpressions[index - 1].LogicalRelOperator));
                        AddNewQuestion();
                        ddlQuestion2.SelectedIndex = ddlQuestion2.Items.IndexOf(ddlQuestion2.Items.FindByValue(whrExpressions[index - 1].Variable));
                        sQuestionToken = ddlQuestion2.SelectedValue;
                        ddlQuestion = ddlQuestion2;
                        chkBox2.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                        chkBox2.Attributes.Add("parentid", Convert.ToString(whrExpressions[index - 1].ParentId));

                        if (whrExpressions[index - 1].ParentId > 0)
                        {
                            chkBox2.Disabled = true;
                            ddlCondition2.CssClass = "fright";
                            ibtnRemoveGroup2.CssClass = "hide";
                            ibtnRemoveGroup2.Attributes.Add("itemid", "");
                        }
                        else if (IsGroupParent(whrExpressions[index - 1].Id))
                        {
                            chkBox2.Disabled = true;
                            ddlCondition2.CssClass = string.Empty;
                            ibtnRemoveGroup2.CssClass = "remove-group";
                            ibtnRemoveGroup2.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                            ibtnRemoveGroup2.OnClientClick = "btRemoveGroup_click(" + whrExpressions[index - 1].Id + ")";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sQuestionToken))
                                chkBox2.Disabled = true;
                            else
                                chkBox2.Disabled = false;

                            ddlCondition2.CssClass = string.Empty;
                            ibtnRemoveGroup2.CssClass = "hide";
                            ibtnRemoveGroup2.Attributes.Add("itemid", "");
                            ibtnRemoveGroup2.OnClientClick = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(sQuestionToken))
                        {
                            ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                            FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperator2);
                            ddlOperator2.SelectedIndex = ddlOperator2.Items.IndexOf(ddlOperator2.Items.FindByValue(whrExpressions[index - 1].Operator));
                            AddControlForQuestionValue(sQuestion, whrExpressions[index - 1].Value, pnlPickerValueContainer2, pnlOperatorValue2, pnlPickerValuePopup2, lblPickedValue2);
                        }

                        break;
                    case 3:
                        pnlOperatorValue3.Controls.Clear();
                        pnlPickerValuePopup3.Controls.Clear();
                        ddlOperator3.Items.Clear();
                        ddlQuestion3.ClearSelection();
                        ddlQuestion3.Items.Clear();
                        ddlCondition3.SelectedIndex = ddlCondition3.Items.IndexOf(ddlCondition3.Items.FindByValue(whrExpressions[index - 1].LogicalRelOperator));
                        AddNewQuestion();
                        ddlQuestion3.SelectedIndex = ddlQuestion3.Items.IndexOf(ddlQuestion3.Items.FindByValue(whrExpressions[index - 1].Variable));
                        sQuestionToken = ddlQuestion3.SelectedValue;
                        ddlQuestion = ddlQuestion3;
                        chkBox3.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                        chkBox3.Attributes.Add("parentid", Convert.ToString(whrExpressions[index - 1].ParentId));

                        if (whrExpressions[index - 1].ParentId > 0)
                        {
                            chkBox3.Disabled = true;
                            ddlCondition3.CssClass = "fright";
                            ibtnRemoveGroup3.CssClass = "hide";
                            ibtnRemoveGroup3.Attributes.Add("itemid", "");
                        }
                        else if (IsGroupParent(whrExpressions[index - 1].Id))
                        {
                            chkBox3.Disabled = true;
                            ddlCondition3.CssClass = string.Empty;
                            ibtnRemoveGroup3.CssClass = "remove-group";
                            ibtnRemoveGroup3.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                            ibtnRemoveGroup3.OnClientClick = "btRemoveGroup_click(" + whrExpressions[index - 1].Id + ")";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sQuestionToken))
                                chkBox3.Disabled = true;
                            else
                                chkBox3.Disabled = false;

                            ddlCondition3.CssClass = string.Empty;
                            ibtnRemoveGroup3.CssClass = "hide";
                            ibtnRemoveGroup3.Attributes.Add("itemid", "");
                            ibtnRemoveGroup3.OnClientClick = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(sQuestionToken))
                        {
                            ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                            FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperator3);
                            ddlOperator3.SelectedIndex = ddlOperator3.Items.IndexOf(ddlOperator3.Items.FindByValue(whrExpressions[index - 1].Operator));
                            AddControlForQuestionValue(sQuestion, whrExpressions[index - 1].Value, pnlPickerValueContainer3, pnlOperatorValue3, pnlPickerValuePopup3, lblPickedValue3);
                        }

                        break;
                    case 4:
                        pnlOperatorValue4.Controls.Clear();
                        pnlPickerValuePopup4.Controls.Clear();
                        ddlOperator4.Items.Clear();
                        ddlQuestion4.ClearSelection();
                        ddlQuestion4.Items.Clear();
                        ddlCondition4.SelectedIndex = ddlCondition4.Items.IndexOf(ddlCondition4.Items.FindByValue(whrExpressions[index - 1].LogicalRelOperator));
                        AddNewQuestion();
                        ddlQuestion4.SelectedIndex = ddlQuestion4.Items.IndexOf(ddlQuestion4.Items.FindByValue(whrExpressions[index - 1].Variable));
                        sQuestionToken = ddlQuestion4.SelectedValue;
                        ddlQuestion = ddlQuestion4;
                        chkBox4.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                        chkBox4.Attributes.Add("parentid", Convert.ToString(whrExpressions[index - 1].ParentId));

                        if (whrExpressions[index - 1].ParentId > 0)
                        {
                            chkBox4.Disabled = true;
                            ddlCondition4.CssClass = "fright";
                            ibtnRemoveGroup4.CssClass = "hide";
                            ibtnRemoveGroup4.Attributes.Add("itemid", "");
                        }
                        else if (IsGroupParent(whrExpressions[index - 1].Id))
                        {
                            chkBox4.Disabled = true;
                            ddlCondition4.CssClass = string.Empty;
                            ibtnRemoveGroup4.CssClass = "remove-group";
                            ibtnRemoveGroup4.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                            ibtnRemoveGroup4.OnClientClick = "btRemoveGroup_click(" + whrExpressions[index - 1].Id + ")";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sQuestionToken))
                                chkBox4.Disabled = true;
                            else
                                chkBox4.Disabled = false;

                            ddlCondition4.CssClass = string.Empty;
                            ibtnRemoveGroup4.CssClass = "hide";
                            ibtnRemoveGroup4.Attributes.Add("itemid", "");
                            ibtnRemoveGroup4.OnClientClick = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(sQuestionToken))
                        {
                            ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                            FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperator4);
                            ddlOperator4.SelectedIndex = ddlOperator4.Items.IndexOf(ddlOperator4.Items.FindByValue(whrExpressions[index - 1].Operator));
                            AddControlForQuestionValue(sQuestion, whrExpressions[index - 1].Value, pnlPickerValueContainer4, pnlOperatorValue4, pnlPickerValuePopup4, lblPickedValue4);
                        }

                        break;
                    case 5:
                        pnlOperatorValue5.Controls.Clear();
                        pnlPickerValuePopup5.Controls.Clear();
                        ddlOperator5.Items.Clear();
                        ddlQuestion5.ClearSelection();
                        ddlQuestion5.Items.Clear();
                        ddlCondition5.SelectedIndex = ddlCondition5.Items.IndexOf(ddlCondition5.Items.FindByValue(whrExpressions[index - 1].LogicalRelOperator));
                        AddNewQuestion();
                        ddlQuestion5.SelectedIndex = ddlQuestion5.Items.IndexOf(ddlQuestion5.Items.FindByValue(whrExpressions[index - 1].Variable));
                        sQuestionToken = ddlQuestion5.SelectedValue;
                        ddlQuestion = ddlQuestion5;
                        chkBox5.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                        chkBox5.Attributes.Add("parentid", Convert.ToString(whrExpressions[index - 1].ParentId));

                        if (whrExpressions[index - 1].ParentId > 0)
                        {
                            chkBox5.Disabled = true;
                            ddlCondition5.CssClass = "fright";
                            ibtnRemoveGroup5.CssClass = "hide";
                            ibtnRemoveGroup5.Attributes.Add("itemid", "");
                        }
                        else if (IsGroupParent(whrExpressions[index - 1].Id))
                        {
                            chkBox5.Disabled = true;
                            ddlCondition5.CssClass = string.Empty;
                            ibtnRemoveGroup5.CssClass = "remove-group";
                            ibtnRemoveGroup5.Attributes.Add("itemid", Convert.ToString(whrExpressions[index - 1].Id));
                            ibtnRemoveGroup5.OnClientClick = "btRemoveGroup_click(" + whrExpressions[index - 1].Id + ")";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sQuestionToken))
                                chkBox5.Disabled = true;
                            else
                                chkBox5.Disabled = false;

                            ddlCondition5.CssClass = string.Empty;
                            ibtnRemoveGroup5.CssClass = "hide";
                            ibtnRemoveGroup5.Attributes.Add("itemid", "");
                            ibtnRemoveGroup5.OnClientClick = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(sQuestionToken))
                        {
                            ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == sQuestionToken);
                            FillQuestionOperators(sQuestion.QuestionType.ToLower(), ddlOperator5);
                            ddlOperator5.SelectedIndex = ddlOperator5.Items.IndexOf(ddlOperator5.Items.FindByValue(whrExpressions[index - 1].Operator));
                            AddControlForQuestionValue(sQuestion, whrExpressions[index - 1].Value, pnlPickerValueContainer5, pnlOperatorValue5, pnlPickerValuePopup5, lblPickedValue5);
                        }

                        break;
                    default:
                        break;
                }

                if (ddlQuestion == null)
                    continue;

                // Update dictionary for current question value
                if (index == 1)
                    dictSelectedQuestions.Clear();

                if (dictSelectedQuestions.Keys.Contains(ddlQuestion.ID))
                    dictSelectedQuestions[ddlQuestion.ID] = sQuestionToken;
                else
                    dictSelectedQuestions.Add(ddlQuestion.ID, sQuestionToken);
            }

            // Update other question dropdowns data i.e. remove this question from other Question controls
            UpdateQuestionControls();
        }
        #endregion Method to bind controls for selected Skip Logic

        #region Method to create and bind skip logic expression to expression area
        /// <summary>
        /// This method is used to create and bind skip logic expression to expression area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuildExpression_Click(object sender, EventArgs e)
        {
            RebindControls(false);
        }
        #endregion Method to create and bind skip logic expression to expression area

        #region Method to create list of WhereExpressions for selected questions
        /// <summary>
        /// This method is used to create list of WhereExpressions for selected questions
        /// </summary>
        /// <returns></returns>
        protected List<WhereExpression> GenerateExpressionsList()
        {
            List<WhereExpression> wExpressions = new List<WhereExpression>();

            if (dictSelectedQuestions == null || dictSelectedQuestions.Count == 0)
                return wExpressions;

            int itemIndex = 0;
            foreach (var item in dictSelectedQuestions)
            {
                WhereExpression whereClause = new WhereExpression();
                wExpressions.Add(whereClause);
                itemIndex++;

                switch (item.Key)
                {
                    case "ddlCQuestions":
                        whereClause.Variable = ddlCQuestions.SelectedValue;
                        whereClause.Operator = ddlOperators.SelectedValue;

                        if (UGITUtility.StringToInt(chkBox1.Attributes["itemid"]) == 0)
                            whereClause.Id = itemIndex;
                        else
                            whereClause.Id = UGITUtility.StringToInt(chkBox1.Attributes["itemid"]);

                        whereClause.ParentId = UGITUtility.StringToInt(chkBox1.Attributes["parentid"]);
                        ServiceQuestion sQuestion = service.Questions.FirstOrDefault(x => x.TokenName == ddlCQuestions.SelectedValue);

                        if (sQuestion == null)
                            break;

                        string questionType = sQuestion.QuestionType.ToLower();
                        if (pOperatorValue.Controls.Count > 1 || pPickerValuePopup.Controls.Count > 1)
                        {
                            Control ctr = null;
                            if (questionType == Constants.ServiceQuestionType.LOOKUP ||
                               questionType == Constants.ServiceQuestionType.MULTICHOICE ||
                               questionType == Constants.ServiceQuestionType.REQUESTTYPE ||
                               questionType == Constants.ServiceQuestionType.SINGLECHOICE)
                            {
                                ctr = pPickerValuePopup.Controls[1];
                            }
                            else
                            {
                                ctr = pOperatorValue.Controls[1];
                            }

                            whereClause.Value = ExtendedControls.ReadSelectedValueSkipLogic(sQuestion, ctr, whereClause.Operator);
                        }

                        break;
                    case "ddlQuestion2":
                        whereClause.LogicalRelOperator = ddlCondition2.SelectedValue;
                        whereClause.Variable = ddlQuestion2.SelectedValue;
                        whereClause.Operator = ddlOperator2.SelectedValue;

                        if (UGITUtility.StringToInt(chkBox2.Attributes["itemid"]) == 0)
                            whereClause.Id = itemIndex;
                        else
                            whereClause.Id = UGITUtility.StringToInt(chkBox2.Attributes["itemid"]);

                        whereClause.ParentId = UGITUtility.StringToInt(chkBox2.Attributes["parentid"]);
                        ServiceQuestion selectedQuestion2 = service.Questions.FirstOrDefault(x => x.TokenName == ddlQuestion2.SelectedValue);

                        if (selectedQuestion2 == null)
                            break;

                        string questionType2 = selectedQuestion2.QuestionType.ToLower();
                        if (pnlOperatorValue2.Controls.Count > 1 || pnlPickerValuePopup2.Controls.Count > 1)
                        {
                            Control ctr = null;
                            if (questionType2 == Constants.ServiceQuestionType.LOOKUP ||
                               questionType2 == Constants.ServiceQuestionType.MULTICHOICE ||
                               questionType2 == Constants.ServiceQuestionType.REQUESTTYPE ||
                               questionType2 == Constants.ServiceQuestionType.SINGLECHOICE)
                            {
                                ctr = pnlPickerValuePopup2.Controls[1];
                            }
                            else
                            {
                                ctr = pnlOperatorValue2.Controls[1];
                            }

                            whereClause.Value = ExtendedControls.ReadSelectedValueSkipLogic(selectedQuestion2, ctr, whereClause.Operator);
                        }

                        break;
                    case "ddlQuestion3":
                        whereClause.LogicalRelOperator = ddlCondition3.SelectedValue;
                        whereClause.Variable = ddlQuestion3.SelectedValue;
                        whereClause.Operator = ddlOperator3.SelectedValue;

                        if (UGITUtility.StringToInt(chkBox3.Attributes["itemid"]) == 0)
                            whereClause.Id = itemIndex;
                        else
                            whereClause.Id = UGITUtility.StringToInt(chkBox3.Attributes["itemid"]);

                        whereClause.ParentId = UGITUtility.StringToInt(chkBox3.Attributes["parentid"]);
                        ServiceQuestion selectedQuestion3 = service.Questions.FirstOrDefault(x => x.TokenName == ddlQuestion3.SelectedValue);

                        if (selectedQuestion3 == null)
                            break;

                        string questionType3 = selectedQuestion3.QuestionType.ToLower();
                        if (pnlOperatorValue3.Controls.Count > 1 || pnlPickerValuePopup3.Controls.Count > 1)
                        {
                            Control ctr = null;
                            if (questionType3 == Constants.ServiceQuestionType.LOOKUP ||
                               questionType3 == Constants.ServiceQuestionType.MULTICHOICE ||
                               questionType3 == Constants.ServiceQuestionType.REQUESTTYPE ||
                               questionType3 == Constants.ServiceQuestionType.SINGLECHOICE)
                            {
                                ctr = pnlPickerValuePopup3.Controls[1];
                            }
                            else
                            {
                                ctr = pnlOperatorValue3.Controls[1];
                            }

                            whereClause.Value = ExtendedControls.ReadSelectedValueSkipLogic(selectedQuestion3, ctr, whereClause.Operator);
                        }

                        break;
                    case "ddlQuestion4":
                        whereClause.LogicalRelOperator = ddlCondition4.SelectedValue;
                        whereClause.Variable = ddlQuestion4.SelectedValue;
                        whereClause.Operator = ddlOperator4.SelectedValue;

                        if (UGITUtility.StringToInt(chkBox4.Attributes["itemid"]) == 0)
                            whereClause.Id = itemIndex;
                        else
                            whereClause.Id = UGITUtility.StringToInt(chkBox4.Attributes["itemid"]);

                        whereClause.ParentId = UGITUtility.StringToInt(chkBox4.Attributes["parentid"]);
                        ServiceQuestion selectedQuestion4 = service.Questions.FirstOrDefault(x => x.TokenName == ddlQuestion4.SelectedValue);

                        if (selectedQuestion4 == null)
                            break;

                        string questionType4 = selectedQuestion4.QuestionType.ToLower();
                        if (pnlOperatorValue4.Controls.Count > 1 || pnlPickerValuePopup4.Controls.Count > 1)
                        {
                            Control ctr = null;
                            if (questionType4 == Constants.ServiceQuestionType.LOOKUP ||
                               questionType4 == Constants.ServiceQuestionType.MULTICHOICE ||
                               questionType4 == Constants.ServiceQuestionType.REQUESTTYPE ||
                               questionType4 == Constants.ServiceQuestionType.SINGLECHOICE)
                            {
                                ctr = pnlPickerValuePopup4.Controls[1];
                            }
                            else
                            {
                                ctr = pnlOperatorValue4.Controls[1];
                            }

                            whereClause.Value = ExtendedControls.ReadSelectedValueSkipLogic(selectedQuestion4, ctr, whereClause.Operator);
                        }

                        break;
                    case "ddlQuestion5":
                        whereClause.LogicalRelOperator = ddlCondition5.SelectedValue;
                        whereClause.Variable = ddlQuestion5.SelectedValue;
                        whereClause.Operator = ddlOperator5.SelectedValue;

                        if (UGITUtility.StringToInt(chkBox5.Attributes["itemid"]) == 0)
                            whereClause.Id = itemIndex;
                        else
                            whereClause.Id = UGITUtility.StringToInt(chkBox5.Attributes["itemid"]);

                        whereClause.ParentId = UGITUtility.StringToInt(chkBox5.Attributes["parentid"]);
                        ServiceQuestion selectedQuestion5 = service.Questions.FirstOrDefault(x => x.TokenName == ddlQuestion5.SelectedValue);

                        if (selectedQuestion5 == null)
                            break;

                        string questionType5 = selectedQuestion5.QuestionType.ToLower();
                        if (pnlOperatorValue5.Controls.Count > 1 || pnlPickerValuePopup5.Controls.Count > 1)
                        {
                            Control ctr = null;
                            if (questionType5 == Constants.ServiceQuestionType.LOOKUP ||
                               questionType5 == Constants.ServiceQuestionType.MULTICHOICE ||
                               questionType5 == Constants.ServiceQuestionType.REQUESTTYPE ||
                               questionType5 == Constants.ServiceQuestionType.SINGLECHOICE)
                            {
                                ctr = pnlPickerValuePopup5.Controls[1];
                            }
                            else
                            {
                                ctr = pnlOperatorValue5.Controls[1];
                            }

                            whereClause.Value = ExtendedControls.ReadSelectedValueSkipLogic(selectedQuestion5, ctr, whereClause.Operator);
                        }

                        break;
                    default:
                        break;
                }
            }

            return wExpressions;
        }
        #endregion Method to create list of WhereExpressions for selected questions

        #region Method to create expression for Skip Logic Condition
        /// <summary>
        /// Method to create expression for Skip Logic Condition
        /// </summary>
        /// <param name="whrExpressions"></param>
        /// <returns></returns>
        private string GetSkipLogicExpression(List<WhereExpression> whrExpressions)
        {
            StringBuilder expression = new StringBuilder();
            if (whrExpressions == null || whrExpressions.Count == 0)
                return expression.ToString();

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

                    // Set the value of current question
                    ServiceQuestion currentQuestion = service.Questions.FirstOrDefault(x => x.TokenName == where.Variable);

                    if (currentQuestion == null)
                        continue;

                    string value = string.Empty;
                    string questionType = currentQuestion.QuestionType.ToLower();

                    if (questionType == Constants.ServiceQuestionType.CHECKBOX && string.IsNullOrEmpty(where.Value))
                    {
                        value = "'False'";
                    }
                    else if (questionType == Constants.ServiceQuestionType.USERFIELD && !string.IsNullOrEmpty(where.Value))
                    {
                        string[] selectedIDs = where.Value.Split('~');

                        foreach (string sID in selectedIDs)
                        {
                            UserProfile uProfile = userProfileManager.GetUserInfoById(sID);

                            if (uProfile != null)           // check whether we should use Name or UserName.
                            {
                                if (string.IsNullOrEmpty(value))
                                    value = uProfile.Name;
                                else
                                    value += "~" + uProfile.Name; ;
                            }
                        }
                        value = "'" + value + "'";
                    }
                    else if ((questionType == Constants.ServiceQuestionType.LOOKUP || questionType == Constants.ServiceQuestionType.MULTICHOICE
                        || questionType == Constants.ServiceQuestionType.REQUESTTYPE || questionType == Constants.ServiceQuestionType.SINGLECHOICE) && !string.IsNullOrEmpty(where.Value))
                    {
                        string ddlQuestionId = dictSelectedQuestions.Where(x => x.Value == where.Variable).Select(y => y.Key).FirstOrDefault();
                        value = GetCheckListValues(ddlQuestionId);

                        if (string.IsNullOrEmpty(value) || value == "''")
                            value = "'" + ExtendedControls.GetQuestionValueForSkipLogic(currentQuestion, where.Value).Replace(", ", "~") + "'";
                    }
                    else
                    {
                        value = !string.IsNullOrEmpty(where.Value) ? "'" + where.Value + "'" : "''";
                    }

                    subQuery.Append(where.Variable + " ");
                    subQuery.Append(where.Operator + " ");
                    subQuery.Append(value);
                    expList.Add(subQuery.ToString());
                }

                if (expList.Count == 1)
                    expression.AppendFormat("{0} ", string.Join(" ", expList));
                else if (expList.Count > 1)
                    expression.AppendFormat("({0}) ", string.Join(" ", expList));
            }

            return expression.ToString();
        }
        #endregion Method to create expression for Skip Logic Condition

        #region This method is used to rebind all the controls
        /// <summary>
        /// This method is used to rebind all the controls
        /// </summary>
        private void RebindControls(bool removeAllGroups)
        {
            // Get the updated list of whereExpressions
            whrExpressions = GenerateExpressionsList();

            if (removeAllGroups)
            {
                int defaultId = 1;
                foreach (WhereExpression wExpression in whrExpressions)
                {
                    wExpression.Id = defaultId;
                    wExpression.ParentId = 0;
                    defaultId++;
                }
            }

            // Set controls to default values
            ResetMultiQuestionControls();

            // Rebind all the controls
            BindCtrForSelectedSkipLogic();

            string expression = GetSkipLogicExpression(whrExpressions);
            skipLogicExpression.Text = expression;
        }
        #endregion This method is used to rebind all the controls

        #region Method to apply grouping between the selected questions
        /// <summary>
        /// This method is used to apply grouping between the selected questions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btCreateGroup_Click(object sender, EventArgs e)
        {
            if (!hdnSelectedSLogic.Contains("items") || string.IsNullOrWhiteSpace(Convert.ToString(hdnSelectedSLogic["items"])))
                return;

            var param = Convert.ToString(hdnSelectedSLogic["items"]);
            List<string> items = UGITUtility.ConvertStringToList(param, ",");
            if (items == null || items.Count == 0)
                return;
            int parentId = UGITUtility.StringToInt(items[0]);
            List<int> children = items.Select(x => UGITUtility.StringToInt(x)).Where(x => x != parentId).ToList();

            // Get the updated list of whereExpressions
            whrExpressions = GenerateExpressionsList();

            if (whrExpressions != null && whrExpressions.Count > 0)
            {
                List<WhereExpression> childExpressions = whrExpressions.Where(x => children.Contains(x.Id)).ToList();

                foreach (WhereExpression wExp in childExpressions)
                {
                    wExp.ParentId = parentId;
                }

                // Change order of where expressions to maintain parent-child relation
                whrExpressions = whrExpressions.OrderBy(x => x.ParentId == 0 ? x.Id : x.ParentId).ThenBy(x => x.Id).ToList();
            }

            // Set controls to default values
            ResetMultiQuestionControls();

            // Rebind all the controls
            BindCtrForSelectedSkipLogic();

            // Create skip logic expression
            string expression = GetSkipLogicExpression(whrExpressions);
            skipLogicExpression.Text = expression;
        }
        #endregion Method to apply grouping between the selected questions

        #region Method to check if the current question item is a group parent 
        /// <summary>
        /// This method is used to check if the current question item is a group parent 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected bool IsGroupParent(int id)
        {
            WhereExpression wInfo = whrExpressions.FirstOrDefault(x => x.Id == id);
            if (wInfo != null)
            {
                return whrExpressions.Exists(x => x.ParentId == wInfo.Id);
            }

            return false;
        }
        #endregion Method to check if the current question item is a group parent 

        #region Method to remove grouping from the condition
        /// <summary>
        /// This method is used to remove grouping from the condition based on groupid/parentid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btRemoveGroup_Click(object sender, EventArgs e)
        {
            if (!hdnSelectedSLogic.Contains("items") || string.IsNullOrWhiteSpace(Convert.ToString(hdnSelectedSLogic["items"])))
                return;

            int itemId = UGITUtility.StringToInt(hdnSelectedSLogic["items"]);

            // Get the updated list of whereExpressions
            whrExpressions = GenerateExpressionsList();

            if (whrExpressions != null && whrExpressions.Count > 0)
            {
                List<WhereExpression> childExpressions = whrExpressions.Where(x => x.ParentId == itemId).ToList();

                foreach (WhereExpression wExp in childExpressions)
                {
                    wExp.ParentId = 0;
                }

                // Change order of where expressions to maintain parent-child relation
                whrExpressions = whrExpressions.OrderBy(x => x.ParentId == 0 ? x.Id : x.ParentId).ThenBy(x => x.Id).ToList();
            }

            // Rebind all the controls
            BindCtrForSelectedSkipLogic();

            // Create skip logic expression
            string expression = GetSkipLogicExpression(whrExpressions);
            skipLogicExpression.Text = expression;

            // Set controls to default values
            ResetMultiQuestionControls();
        }
        #endregion Method to remove grouping from the condition

        #region Method to get text values of questions containing checkbox list
        private string GetCheckListValues(string ddlQuestionId)
        {
            string value = string.Empty;

            switch (ddlQuestionId)
            {
                case "ddlCQuestions":
                    value = lbPickedValue.Text;
                    break;
                case "ddlQuestion2":
                    value = lblPickedValue2.Text;
                    break;
                case "ddlQuestion3":
                    value = lblPickedValue3.Text;
                    break;
                case "ddlQuestion4":
                    value = lblPickedValue4.Text;
                    break;
                case "ddlQuestion5":
                    value = lblPickedValue5.Text;
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(value))
                value = "'" + value.Replace(", ", "~") + "'";
            else
                value = "''";

            return value;
        }
        #endregion Method to get text values of questions containing checkbox list

        private void UpdateQuestionCtrIdDic(VariableValue variableValue = null)
        {
            if (variableValue != null)
            {
                dictSelectedQuestions.Clear();
                DropDownList ddlQuestion = null;
                string sQuestionToken = string.Empty;

                for (int index = 1; index <= variableValue.Conditions.Count; index++)
                {
                    WhereExpression whrExpress = variableValue.Conditions[index - 1];
                    switch (index)
                    {
                        case 1:
                            sQuestionToken = whrExpress.Variable;
                            ddlQuestion = ddlCQuestions;

                            break;
                        case 2:
                            sQuestionToken = whrExpress.Variable;
                            ddlQuestion = ddlQuestion2;

                            break;
                        case 3:
                            sQuestionToken = whrExpress.Variable;
                            ddlQuestion = ddlQuestion3;

                            break;
                        case 4:
                            sQuestionToken = whrExpress.Variable;
                            ddlQuestion = ddlQuestion4;

                            break;
                        case 5:
                            sQuestionToken = whrExpress.Variable;
                            ddlQuestion = ddlQuestion5;

                            break;
                        default:
                            break;
                    }

                    if (ddlQuestion == null)
                        continue;

                    // Update dictionary for current question value
                    if (index == 1)
                        dictSelectedQuestions.Clear();

                    if (dictSelectedQuestions.Keys.Contains(ddlQuestion.ID))
                        dictSelectedQuestions[ddlQuestion.ID] = sQuestionToken;
                    else
                        dictSelectedQuestions.Add(ddlQuestion.ID, sQuestionToken);
                }
            }

        }

        private void CreateExpressionSchema(QuestionMapVariable qesMapVariable)
        {
            dtExpresstionResult = new DataTable();
            dtExpresstionResult.Columns.Add("Express");
            dtExpresstionResult.Columns.Add("Value");
            dtExpresstionResult.Columns.Add("IsPickFromConstant", typeof(bool));
            if (qesMapVariable != null)
            {
                foreach (VariableValue varValue in qesMapVariable.VariableValues)
                {
                    UpdateQuestionCtrIdDic(varValue);
                    string whrCondition = GetSkipLogicExpression(varValue.Conditions);
                    if (!string.IsNullOrEmpty(whrCondition))
                    {
                        DataRow row = dtExpresstionResult.NewRow();
                        row["Express"] = whrCondition;
                        row["Value"] = varValue.PickFrom;
                        row["IsPickFromConstant"] = Convert.ToBoolean(varValue.IsPickFromConstant);
                        dtExpresstionResult.Rows.Add(row);
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to Get all the questions with a default record
        /// </summary>
        /// <returns></returns>
        private DataTable GetQuestions()
        {
            DataTable dtSource = dtQuestions.Copy();
            DataRow drow = dtSource.NewRow();
            drow["Text"] = "--Select Question--";
            drow["Value"] = "";
            dtSource.Rows.InsertAt(drow, 0);
            return dtSource;
        }
    }
}

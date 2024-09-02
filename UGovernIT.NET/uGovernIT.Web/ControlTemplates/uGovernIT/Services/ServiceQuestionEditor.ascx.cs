using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using DevExpress.Web.ASPxTreeList;
using DevExpress.Web;
using uGovernIT.Core;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Utility.Entities;
using System.Web;
using System.Configuration;
using System.IO;

namespace uGovernIT.Web
{
    public partial class ServiceQuestionEditor : UserControl
    {
        protected int SVCConfigID { get; set; }
        protected int QuestionID { get; set; }
        private ServiceQuestion serviceQuest;
        Services service;
        //SPFieldChoice questionTypeField;
        bool saveAndNewQuestionEnable;
        bool flag = true;
        protected string applicationApproval = string.Empty;
        KeyValuePair<string, string> param;
        UserProfile User;
        ConfigurationVariableManager ObjConfigManager = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        private const string absoluteUrlView = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private string newParam = "listpicker";
        private string formTitle = "Picker List";
		 private const string absoluteUrlViewHelpCard = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
        private const string absoluteUrlView1 = "/layouts/ugovernit/DelegateControl.aspx?control={0}&pageTitle={1}&IsDlg=1&Module={2}&TicketId={3}&Type={4}&ControlId={5}";
		
        TaskPredecessorsControl tpControl;
        protected override void OnInit(EventArgs e)
        {


            string urlHelpCard = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlViewHelpCard, newParam, "Help Card Picker", "HLP", string.Empty, "HelpCardList", txtHelpCards.ClientID)); //"TicketWiki"
            aAddHelpCards.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", urlHelpCard, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView1, newParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtWikis.ClientID)); //"TicketWiki"
            aAddItems.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            User = HttpContext.Current.CurrentUser();

            ddlUserFieldAppRequestQuestions.GridView.Width = 250;
            int svcConfigID = 0;
            int.TryParse(Request["svcConfigID"], out svcConfigID);
            SVCConfigID = svcConfigID;

            int questionID = 0;
            int.TryParse(Request["questionID"], out questionID);
            QuestionID = questionID;

            hfQuestionID.Value = QuestionID.ToString();
            hfServiceID.Value = SVCConfigID.ToString();

            // PickfromField.Visible = true;
            pDropdownControlType.Visible = true;
            trDropdownDefaultOptions.Visible = true;
            pDropdownControlTypeButtion.Visible = true;

            pDropdownProperties.Visible = false;
            pMultiSelectProperties.Visible = false;
            pTxtBoxProperties.Visible = false;
            pDateProperties.Visible = false;
            pUserProperties.Visible = false;
            pCheckboxProperties.Visible = false;
            pLookupProperties.Visible = false;
            pAttachmentProperties.Visible = false;
            pRequestTypeProperties.Visible = false;
            pNumberProperties.Visible = false;
            pAppAccessReqProperties.Visible = false;
            lblMeesageNoParentService.Visible = false;
            pUserFieldProperties.Attributes.Add("style", "display:none");

            //questionTypeField = (SPFieldChoice)SPListHelper.GetSPList(DatabaseObjects.Lists.ServiceQuestions).Fields.GetFieldByInternalName(DatabaseObjects.Columns.SWQuestionType);

            if (SVCConfigID > 0)
            {
                ServicesManager serviceManager = new ServicesManager(HttpContext.Current.GetManagerContext());
                service = serviceManager.LoadByServiceID(SVCConfigID);
                List<ServiceQuestion> userfield = service.Questions;
                LoadQuestionTypes(questionBasedOnNew);
                BindTargetTypeCategories();

                GetUserFieldWithinService(userfield, ddlUserField);
                GetUserFieldWithinService(userfield, ddlUserField1);
                GetUserFieldWithinService(userfield, ddlUserFieldDeskLocation);
                GetUserFieldWithinService(userfield, ddlUserManager);

                BindDateTimeQuestions(userfield, ddlDateQuestions);
                BindDateTimeQuestions(userfield, ddlDueDateFrom);
                ddlDueDateFrom.Items.RemoveAt(0);
                ddlDueDateFrom.Items.Insert(0, new ListItem("<Today>", "today"));
                //GetUserFieldWithinService(userfield);

                BindServiceSectionDropDown(service.Sections);

                addDefaultUserControl(UGITUtility.StringToInt(ddlUserType.SelectedValue));
            }

            if (!string.IsNullOrEmpty(ObjConfigManager.GetValue(ConfigurationVariable.ServiceChoiceQuestionPickLists)))
                PickfromField.Visible = true;
            else
                PickfromField.Visible = false;

            if (service != null && service.Questions != null && QuestionID > 0)
            {
                serviceQuest = service.Questions.FirstOrDefault(x => x.ID == QuestionID);
                if (service.ID == serviceQuest.ServiceID)
                {
                    btDelete.Visible = true;
                    txtQuestion.Text = serviceQuest.QuestionTitle;
                    txtToken.Text = serviceQuest.TokenName;
                    questionHelpTextNew.Text = serviceQuest.Helptext;
                    questionBasedOnNew.SelectedIndex = questionBasedOnNew.Items.IndexOf(questionBasedOnNew.Items.FindByValue(serviceQuest.QuestionType));
                    questionServiceSectionsDD.SelectedIndex = questionServiceSectionsDD.Items.IndexOf(questionServiceSectionsDD.Items.FindByValue(serviceQuest.ServiceSectionID.ToString()));
                    cbMandatory.Checked = serviceQuest.FieldMandatory;
                    chkEnableZoomIn.Checked = serviceQuest.EnableZoomIn;
                    if (serviceQuest.ContinueSameLine.HasValue)
                        chkIsContinue.Checked = serviceQuest.ContinueSameLine.Value;
                    ddlTargetTypes.SelectedIndex = ddlTargetTypes.Items.IndexOf(ddlTargetTypes.Items.FindByValue(Convert.ToString(serviceQuest.NavigationType)));
                    SetTargetTypeDependency(); //added by amar 
                    if (ddlTargetTypes.SelectedValue == "Wiki")
                    {
                        txtWikis.Text = Convert.ToString(serviceQuest.NavigationUrl);
                    }

                    if (ddlTargetTypes.SelectedValue == "Link")
                    {
                        txtFileLinks.Text = Convert.ToString(serviceQuest.NavigationUrl);
                    }
                    


                    if (ddlTargetTypes.SelectedValue == "File")
                    {
                        var attachments = serviceQuest.Attachments;

                        string fileName = Path.GetFileName(Convert.ToString(serviceQuest.NavigationUrl));

                        lblUploadedFiles.Text = fileName;

                    }

                    if (ddlTargetTypes.SelectedValue == "HelpCard")
                    {
                        txtHelpCards.Text = Convert.ToString(serviceQuest.NavigationUrl);
                    }
                    
                    if (serviceQuest.QuestionType.ToLower() == "singlechoice")
                    {
                        pDropdownProperties.Visible = true;
                        trDropdownDefaultOptions.Visible = false;

                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "optiontype");
                        if (param.Key != null)
                        {
                            ddlOptionType.SelectedIndex = ddlOptionType.Items.IndexOf(ddlOptionType.Items.FindByValue(param.Value));
                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "options");
                        if (param.Key != null)
                        {
                            txtDropdownOptions.Text = param.Value.Replace(Constants.Separator1, "\r\n").Replace(Constants.Separator2, "\r\n");
                        }

                        trDropdownDefaultOptions.Visible = true;
                        if (!string.IsNullOrEmpty(ObjConfigManager.GetValue(ConfigurationVariable.ServiceChoiceQuestionPickLists)))
                        {

                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "pickfromfield");
                            if (param.Key != null)
                            {
                                PickfromField.Checked = Convert.ToBoolean(param.Value.Replace(Constants.Separator1, "\r\n").Replace(Constants.Separator2, "\r\n"));
                                if (PickfromField.Checked)
                                {
                                    //Bind Single Choice ChooseFromListdrpdwn
                                    FillChooseFromListsDropdown(ChooseFromList);
                                    //FillChooseFromListsDropdown(ChooseFromFields);

                                    //Bind Single Choice ChooseFromFieldListdrpdwn
                                    FillChooseFromFieldListDropdown(ChooseFromFields);
                                    ChoseFromDropdown.Visible = true;
                                    trDropdownDefaultOptions.Visible = false;
                                }

                            }

                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "selectedlist");
                            if (param.Key != null)
                            {
                                ChooseFromList.SelectedIndex = ChooseFromList.Items.IndexOf(ChooseFromList.Items.FindByValue(param.Value));
                            }

                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "selectedfield");
                            if (param.Key != null)
                            {
                                ChooseFromFields.SelectedIndex = ChooseFromFields.Items.IndexOf(ChooseFromFields.Items.FindByValue(param.Value));
                            }

                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                        if (param.Key != null)
                        {
                            txtDropDownDefaultVal.Text = param.Value;
                        }
                    }
                    else if (serviceQuest.QuestionType.ToLower() == "multichoice")
                    {
                        pMultiSelectProperties.Visible = true;
                        trpDropdownControlTypeButtion.Visible = true;
                        trpDropdownControlType.Visible = true;
                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "moptiontype");

                        if (param.Key != null)
                        {
                            ddlMultiSelectOptionType.SelectedIndex = ddlMultiSelectOptionType.Items.IndexOf(ddlMultiSelectOptionType.Items.FindByValue(param.Value));
                        }
                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "options");
                        if (param.Key != null)
                        {
                            txtMSOptions.Text = param.Value.Replace(Constants.Separator1, "\r\n").Replace(Constants.Separator2, "\r\n");
                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                        if (param.Key != null)
                        {
                            txtMSDefault.Text = param.Value;
                        }
                        if (!string.IsNullOrEmpty(ObjConfigManager.GetValue(ConfigurationVariable.ServiceChoiceQuestionPickLists)))
                        {
                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "pickfromfield");
                            if (param.Key != null)
                            {
                                chkbxPickFromFieldMC.Checked = Convert.ToBoolean(param.Value.Replace(Constants.Separator1, "\r\n").Replace(Constants.Separator2, "\r\n"));
                                if (chkbxPickFromFieldMC.Checked)
                                {
                                    trChooseFromListMC.Visible = true;
                                    //Bind Single Choice ChooseFromListdrpdwn
                                     FillChooseFromListsDropdown(ddlChoosefromListMC);
                                    //FillChooseFromListsDropdown(ddlChoosefromFieldMC);
                                    //Bind Single Choice ChooseFromFieldListdrpdwn
                                    //FillChooseFromFieldListDropdown(ddlChoosefromFieldMC);
                                    ChoseFromDropdown.Visible = true;
                                    trMultiChoiceValuesCaption.Visible = false;
                                    trMultiChoiceValues.Visible = false;
                                }
                            }
                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "selectedlist");
                            if (param.Key != null)
                            {
                                ddlChoosefromListMC.SelectedIndex = ddlChoosefromListMC.Items.IndexOf(ddlChoosefromListMC.Items.FindByValue(param.Value));
                            }

                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "selectedfield");
                            if (param.Key != null)
                            {
                                ddlChoosefromFieldMC.SelectedIndex = ddlChoosefromFieldMC.Items.IndexOf(ddlChoosefromFieldMC.Items.FindByValue(param.Value));
                            }

                        }

                    }
                    else if (serviceQuest.QuestionType.ToLower() == "checkbox")
                    {
                        trMandatory.Visible = false;
                        pCheckboxProperties.Visible = true;

                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                        if (param.Key != null)
                        {
                            cbDefaultValue.Checked = UGITUtility.StringToBoolean(param.Value);
                        }
                    }
                    else if (serviceQuest.QuestionType.ToLower() == "textbox")
                    {
                        pTxtBoxProperties.Visible = true;
                        trUserDeskLocation.Visible = true;
                        trZoom.Visible = true;

                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "textmode");
                        if (param.Key != null)
                        {
                            ddlTxtBoxType.SelectedIndex = ddlTxtBoxType.Items.IndexOf(ddlTxtBoxType.Items.FindByValue(param.Value));

                            chkSetLength.ClientVisible = false;
                            if (param.Value == "Text")
                            {
                                chkSetLength.ClientVisible = true;
                            }
                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                        if (param.Key != null)
                        {
                            txtTBDefault.Text = param.Value;
                        }


                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "maxlength");
                        if (param.Key != null)
                        {

                            chkSetLength.CssClass = "";
                            chkSetLength.Checked = true;
                            lengthLabelTr.Attributes.Add("class", "");
                            lengthChkbxTr.Attributes.Add("class", "");
                            spnbtnTextMaxLength.Value = param.Value;
                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "dependentuserdesklocationquestion");
                        if (param.Key != null)
                        {
                            ddlUserFieldDeskLocation.SelectedIndex = ddlUserFieldDeskLocation.Items.IndexOf(ddlUserFieldDeskLocation.Items.FindByValue(param.Value));
                            chkUserDeskLocation.Checked = true;
                            ddlUserFieldDeskLocation.Visible = true;
                        }
                    }
                    else if (serviceQuest.QuestionType.ToLower() == "number")
                    {
                        pNumberProperties.Visible = true;
                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "textmode");
                        if (param.Key != null)
                        {
                            ddlNumberBoxType.SelectedIndex = ddlNumberBoxType.Items.IndexOf(ddlNumberBoxType.Items.FindByValue(param.Value));
                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                        if (param.Key != null)
                        {
                            txtNumberDefaultValue.Text = param.Value;
                        }

                    }
                    else if (serviceQuest.QuestionType.ToLower() == "userfield")
                    {
                        trUserManager.Visible = true;
                        pUserProperties.Visible = true;

                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                        KeyValuePair<string, string> paramloggedinUser = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "loggedinuser");
                        KeyValuePair<string, string> paramManagersOnly = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "managersonly");
                        KeyValuePair<string, string> paramSpecificUserGroup = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "specificusergroup");
                        KeyValuePair<string, string> paramUserType = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "usertype");
                        KeyValuePair<string, string> singleEntryOnly = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "singleentryonly");

                        divDefaultUser.Visible = true;
                        trGrpPeoplePicker.Visible = false;
                        trUserDefaultValue.Visible = true;
                        //add default user control on runtime
                        int selectionType = UGITUtility.StringToInt(paramUserType.Value);
                        
                        if (paramUserType.Key != null)
                        {
                            ddlUserType.SelectedIndex = UGITUtility.StringToInt(paramUserType.Value);

                            if (ddlUserType.SelectedIndex == 3)
                                trUserDefaultValue.Visible = false;
                            else if (ddlUserType.SelectedIndex == 5)
                            {
                                trUserDefaultValue.Visible = false;
                                trGrpPeoplePicker.Visible = true;
                                
                                if (paramSpecificUserGroup.Key != null)
                                    pEditorGrp.SetValues(paramSpecificUserGroup.Value);
                            }
                        }

                        if (param.Key != null && !IsPostBack)
                        {
                            UserValueBox pEditorUser = divDefaultUser.FindControl("pEditor") as UserValueBox;
                            if (pEditorUser != null)
                            {
                                pEditorUser.SetValues(param.Value);
                            }
                        }
                        if (singleEntryOnly.Key != null)
                            chkSingleEntryOnly.Checked = UGITUtility.StringToBoolean(singleEntryOnly.Value);

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "dependentusermanagerquestion");
                        if (param.Key != null)
                        {
                            trUserManager.Attributes.Add("style", "display:block");
                            ddlUserManager.SelectedIndex = ddlUserManager.Items.IndexOf(ddlUserManager.Items.FindByValue(param.Value));
                            chkUserManager.Checked = true;
                            ddlUserManager.Visible = true;
                        }
                    }
                    else if (serviceQuest.QuestionType.ToLower() == "datetime")
                    {
                        pDateProperties.Visible = true;

                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "datemode");
                        if (param.Key != null)
                        {
                            ddlDateFormat.SelectedIndex = ddlDateFormat.Items.IndexOf(ddlDateFormat.Items.FindByValue(param.Value));
                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "defaultval");
                        if (param.Key != null)
                        {
                            double noOfDays = UGITUtility.StringToDouble(param.Value);
                            speDefaultDateNoofDays.Text = Convert.ToString(Math.Abs(noOfDays));
                            if (noOfDays > 0)
                                ddlPlusMinusDefaultDate.SelectedIndex = 0;
                            else
                                ddlPlusMinusDefaultDate.SelectedIndex = 1;
                        }

                        //DateTime Questions - add validation for Present ,Past,Future  :Start

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "dateconstraint");

                        if (param.Key != null && param.Value == "conditional")
                        {
                            chkDateValidations.Checked = true;
                            conditionalDateHandler.Style.Add("display", "inline-block");
                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "validateagainst");
                            if (param.Key != null)
                            {
                                if (param.Value.ToLower() == "currentdate")
                                    rdbDateValidationAgainst.SelectedIndex = 0;
                                else
                                {
                                    rdbDateValidationAgainst.SelectedIndex = 1;
                                    ddlDateQuestions.Style.Add("display", "inline-block");
                                    ddlDateQuestions.SelectedIndex = ddlDateQuestions.Items.IndexOf(ddlDateQuestions.Items.FindByValue(param.Value));
                                }
                            }
                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "presentdate");
                            if (param.Key != null)
                            {
                                presentDates.Checked = Convert.ToBoolean(param.Value);
                            }

                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "pastdate");
                            if (param.Key != null)
                            {
                                pastDates.Checked = Convert.ToBoolean(param.Value);
                            }
                            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "futuredate");
                            if (param.Key != null)
                            {
                                futureDates.Checked = Convert.ToBoolean(param.Value);
                            }
                        }

                        //DateTime Questions - add validation for Present ,Past,Future  :end

                    }
                    else if (serviceQuest.QuestionType.ToLower() == "lookup")
                    {
                        pLookupProperties.Visible = true;
                        FillLookupListsDropDown();

                        string listName = string.Empty;
                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookuplist");
                        if (param.Key != null)
                        {
                            ddlLookupList.SelectedIndex = ddlLookupList.Items.IndexOf(ddlLookupList.Items.FindByValue(param.Value));
                            listName = param.Value;

                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "lookupfield");
                        if (param.Key != null)
                        {
                            DDLLookupList_SelectedIndexChanged(ddlLookupFields, new EventArgs());
                            ddlLookupFields.SelectedIndex = ddlLookupFields.Items.IndexOf(ddlLookupFields.Items.FindByValue(param.Value));
                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "module");
                        if (param.Key != null)
                        {
                            ddlLPModule.SelectedIndex = ddlLPModule.Items.IndexOf(ddlLPModule.Items.FindByValue(param.Value));
                        }

                        chbAllowMultiLookup.Visible = true;
                        if (listName == DatabaseObjects.Tables.Department)
                        {
                            chbAllowMultiLookup.Visible = true;
                        }
                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "multi");
                        if (param.Key != null)
                        {
                            chbAllowMultiLookup.Checked = UGITUtility.StringToBoolean(param.Value);
                        }
                        if (listName.ToLower().ToString() == DatabaseObjects.Columns.LocationLookup.ToLower() || listName.ToLower().ToString() == DatabaseObjects.Columns.DepartmentLookup.ToLower())
                        {
                            if (listName.ToLower().ToString() == DatabaseObjects.Columns.LocationLookup.ToLower())
                            {
                                chkDepartmentowner.Text = "Pre-fill location of user";
                                param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "locationuserquestion");
                            }
                            else if (listName.ToLower().ToString() == DatabaseObjects.Columns.DepartmentLookup.ToLower())
                            {
                                chkDepartmentowner.Text = "Pre-fill department of user";
                                param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "departmentuserquestion");
                            }
                            if (param.Key != null)
                            {
                                tr1radiooption.Attributes.Add("style", "display:block");
                                //ddlUserField1.SetValues(param.Value); 
                                ddlUserField1.SelectedIndex = ddlUserField.Items.IndexOf(ddlUserField.Items.FindByValue(param.Value));
                                chkDepartmentowner.Checked = true;
                                ddlUserField1.Visible = true;
                            }
                        }
                        else if (listName.ToLower().ToString() == "sublocation")
                        {
                            if (serviceQuest.QuestionTypePropertiesDicObj.Any(x => x.Key.Trim().ToLower() == "dependentlocationquestion"))
                            {
                                param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "dependentlocationquestion");
                                trSubLocationConfig.Attributes.Add("style", "display:block");
                                tr1radiooption.Attributes.Add("style", "display:none");
                                ddlLocationQuestions.SelectedIndex = ddlLocationQuestions.Items.IndexOf(ddlLocationQuestions.Items.FindByValue(param.Value));
                                chkDepndLocation.Checked = true;
                                ddlLocationQuestions.Visible = true;
                            }
                        }
                    }
                    else if (serviceQuest.QuestionType.ToLower() == "requesttype")
                    {
                        //Load requesttype configuration
                        pRequestTypeProperties.Visible = true;
                        ddlRTModule.Items.Add(new ListItem("Select Module", ""));
                        DataTable modules = ObjModuleViewManager.LoadAllModules();
                        if (modules != null)
                        {
                            modules.DefaultView.RowFilter = string.Format("{0} <> 'SVC'", DatabaseObjects.Columns.ModuleName);
                            modules.DefaultView.Sort = string.Format("{0} Asc", DatabaseObjects.Columns.ModuleName);
                            foreach (DataRowView module in modules.DefaultView)
                            {
                                string moduleName = Convert.ToString(module[DatabaseObjects.Columns.ModuleName]);

                                if (Convert.ToString(module[DatabaseObjects.Columns.ModuleType]) == Convert.ToString(ModuleType.SMS) || moduleName == "NPR" || moduleName == "TSK")
                                    ddlRTModule.Items.Add(new ListItem(Convert.ToString(module[DatabaseObjects.Columns.Title]), Convert.ToString(module[DatabaseObjects.Columns.ModuleName])));
                            }                           
                        }
                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "module");
                        if (param.Key != null)
                        {
                            ddlRTModule.SelectedIndex = ddlRTModule.Items.IndexOf(ddlRTModule.Items.FindByValue(param.Value));
                        }
                        DDLRTModules_SelectedIndexChanged(ddlRTModule, new EventArgs());

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "dropdowntype");
                        if (param.Key != null)
                        {
                            ddlRTDropdownType.SelectedIndex = ddlRTDropdownType.Items.IndexOf(ddlRTDropdownType.Items.FindByValue(param.Value));
                        }

                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "enablecategorydropdown");
                        if (param.Key != null)
                        {
                            chkEnableRCategoryDropDown.Checked = UGITUtility.StringToBoolean(param.Value);
                        }
                        param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "enableissuetypedropdown");
                        if (param.Key != null)
                        {
                            chkbxEnableIssueType.Checked = UGITUtility.StringToBoolean(param.Value);
                        }

                    }
                    else if (serviceQuest.QuestionType.ToLower() == "applicationaccessrequest")
                    {
                        trZoom.Visible = true;
                        if (service != null && !service.CreateParentServiceRequest)
                        {
                            lblMeesageNoParentService.Visible = true;
                        }

                        pAppAccessReqProperties.Visible = true;

                        BindApplications();
                        List<ListItem> dropboxItems = chkRTApplications.Items.Cast<ListItem>().ToList();
                        KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "application");
                        KeyValuePair<string, string> disableAllCheck = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "disableallcheck");
                        KeyValuePair<string, string> existinguser = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.ExistingUser);
                        KeyValuePair<string, string> newuser = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.NewUSer);
                        KeyValuePair<string, string> accessrequestmode = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.AccessRequestMode);
                        KeyValuePair<string, string> mirroraccessfrom = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == ConfigConstants.MirrorAccessFrom);
                        KeyValuePair<string, string> duedatefrom = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "duedatefrom");
                        KeyValuePair<string, string> predecessors = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "predecessors");
                        List<ServiceQuestion> userfield = service.Questions;
                        trMirrorAccessFrom.Visible = true;
                        ddlAccessMirrorFrom.ClearSelection();
                        ddlAccessMirrorFrom.Items.Clear();
                        GetUserFieldWithinService(userfield, ddlAccessMirrorFrom);
                        if (duedatefrom.Key != null)
                        {
                            string[] dueDate = duedatefrom.Value.Split(':');
                            if (dueDate.Length > 0)
                            {
                                string strDueDatefrom = dueDate[0];
                                if (strDueDatefrom.ToLower() == "<today>")
                                    ddlDueDateFrom.SelectedIndex = 0;
                                else
                                {
                                    ddlDueDateFrom.SelectedIndex = 1;
                                    ddlDueDateFrom.SelectedIndex = ddlDueDateFrom.Items.IndexOf(ddlDueDateFrom.Items.FindByValue(dueDate[0]));
                                }

                                int NoofDays = UGITUtility.StringToInt(dueDate[1]);
                                if (NoofDays == 0)
                                {
                                    dxNoOfDays.Text = string.Empty;
                                    ddlPlusMinus.SelectedIndex = 0;
                                }
                                else if (NoofDays > 0)
                                {
                                    dxNoOfDays.Text = Convert.ToString(NoofDays);
                                    ddlPlusMinus.SelectedIndex = 0;
                                }
                                else
                                {
                                    dxNoOfDays.Text = Convert.ToString(Math.Abs(NoofDays));
                                    ddlPlusMinus.SelectedIndex = 1;
                                }
                            }
                        }
                        if (mirroraccessfrom.Key != null)
                        {
                            ddlAccessMirrorFrom.SelectedIndex = ddlAccessMirrorFrom.Items.IndexOf(ddlAccessMirrorFrom.Items.FindByValue(mirroraccessfrom.Value));
                        }
                        if (newuser.Key != null)
                        {

                            trUserFieldAppRequest.Visible = true;
                            trAccessRequestMode.Visible = false;
                            chkAutoCreateAccountTask.Checked = service.Tasks.Exists(x => x.SubTaskType.ToLower() == ServiceSubTaskType.AccountTask.ToLower() && x.AutoCreateUser);
                            rdbPickUserFrom.SelectedIndex = 0;
                            lbUserFieldAppRequestQuestions.Visible = true;
                            if (!IsPostBack)
                            {
                                ddlUserFieldAppRequestQuestions.SelectionMode = GridLookupSelectionMode.Multiple;
                                BindTextQuestions(userfield, ddlUserFieldAppRequestQuestions);
                                List<string> questions = UGITUtility.ConvertStringToList(newuser.Value, ",");
                                foreach (string q in questions)
                                {
                                    ddlUserFieldAppRequestQuestions.GridView.Selection.SelectRowByKey(q);
                                }
                            }

                            trAutoCreateAccountTask.Visible = true;
                        }
                        else if (existinguser.Key != null)
                        {
                            rdbPickUserFrom.SelectedIndex = 1;
                            trUserFieldAppRequest.Visible = true;
                            lbUserFieldAppRequestQuestions.Visible = false;
                            if (!IsPostBack)
                            {
                                ddlUserFieldAppRequestQuestions.SelectionMode = GridLookupSelectionMode.Single;
                                GetUserFieldWithinService(userfield, ddlUserFieldAppRequestQuestions);
                                ddlUserFieldAppRequestQuestions.GridView.Selection.SelectRowByKey(existinguser.Value);
                            }

                            trAccessRequestMode.Visible = true;

                            if (!string.IsNullOrEmpty(accessrequestmode.Key))
                            {
                                chbxEnableAccessMode.Checked = true;
                                trAccessModeOptions.Visible = true;
                                List<string> lstSelectedAccessModes = accessrequestmode.Value.Split(';').ToList();
                                foreach (var item in lstSelectedAccessModes)
                                {
                                    ListItem lstItem = chkbxlstAccessReqMode.Items.FindByValue(item);
                                    if (lstItem != null)
                                        lstItem.Selected = true;
                                }
                            }

                        }
                        else if (newuser.Key == null && existinguser.Key == null)
                        {
                            rdbPickUserFrom.ClearSelection();
                            trMirrorAccessFrom.Visible = false;
                        }
                        if (disableAllCheck.Key != null)
                            cbDisableAllCheckBox.Checked = UGITUtility.StringToBoolean(disableAllCheck.Value);

                   
                        if (param.Key != null)
                        {
                            if (param.Value.ToLower() == "all")
                            {
                                cbRTAllApplications.Checked = true;
                                foreach (ListItem item in chkRTApplications.Items)
                                {
                                    item.Selected = true;
                                }
                                foreach (ListItem item1 in dropboxItems)
                                {
                                    item1.Selected = true;
                                }
                            }
                            else
                            {
                                string[] lstValues = param.Value.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string item in lstValues)
                                {
                                    ListItem listItem = dropboxItems.FirstOrDefault(x => x.Value == item);
                                    if (listItem != null)
                                        listItem.Selected = true;
                                }
                            }
                            foreach (ListItem lstItem in chkRTApplications.Items)
                            {
                                if (!lstItem.Value.Contains("applicationid"))
                                {
                                    bool isSelected = true;
                                    string[] catArrColl= lstItem.Value.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (catArrColl != null && catArrColl.Length > 1)
                                    {
                                        string categoryValue = catArrColl[1];

                                        List<ListItem> lstItems = dropboxItems.FindAll(x => x.Value.StartsWith("category-" + categoryValue + Constants.Separator2 + "applicationid"));
                                        foreach (ListItem item in lstItems)
                                        {
                                            if (item.Selected != true)
                                            {
                                                isSelected = false;
                                            }
                                        }
                                        if (isSelected == true)
                                        {
                                            lstItem.Selected = true;
                                        }
                                    }
                                }
                            }
                        }


                        tpControl = (TaskPredecessorsControl)Page.LoadControl(@"~/ControlTemplates/uGovernIT/Task/TaskPredecessorsControl.ascx");
                        List<UGITTask> relations = service.Tasks.Where(x => x.ID > 0 && x.SubTaskType.ToLower() == ServiceSubTaskType.Task).OrderBy(x => x.ItemOrder).ToList();
                        tpControl.ServiceTasks = relations;
                        tpControl.PredecessorMode = PredecessorType.ServiceTask;

                        if (predecessors.Key != null)
                        {
                            string[] pres = predecessors.Value.Split(new string[] { Constants.Separator5 }, StringSplitOptions.RemoveEmptyEntries);
                            tpControl.SelectedPredecessorsId = pres.ToList();
                        }

                        trpredecessorslable.Visible = true;
                        trpredecessorsControl.Visible = true;
                        pheditcontrol.Controls.Add(tpControl);
                    }
                    else if (serviceQuest.QuestionType.ToLower() == "removeuseraccess")
                    {
                        trMandatory.Visible = false;
                    }
                    else if (serviceQuest.QuestionType.ToLower() == "assets lookup")
                    {
                        pUserFieldProperties.Attributes.Add("style", "display:block");
                        BindAssetTypeDropdown();
                        KeyValuePair<string, string> currentuser = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "currentuser");
                        KeyValuePair<string, string> specificuser = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "specificuser");
                        KeyValuePair<string, string> userquestion = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "userquestion");
                        KeyValuePair<string, string> allAssets = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "allassets");
                        KeyValuePair<string, string> includedepartmentasset = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "includedepartmentasset");
                        KeyValuePair<string, string> assettype = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "assettype");

                        if (includedepartmentasset.Key != null)
                        {
                            chkincludeasset.Checked = true;
                        }
                        if (assettype.Key != null)
                        {
                            chkbxAssetType.Checked = true;

                            divAssetTypeDropdown.Attributes.Add("class", "");
                            RequestTypeDropDownList rd = rqDropdown as RequestTypeDropDownList;
                            if (rd != null)
                                rd.SelectedRequestTypes = assettype.Value;
                            rd.LoadRequestTypeTree("CMDB", !IsPostBack);
                            // rd.SetSelectedValue(assettype.Value);
                        }
                        if (currentuser.Key != null)
                        {
                            rdAssetowner.SelectedIndex = 0;
                            trcurrentuserpeoplepicker.Attributes.Add("style", "display:none");
                            //  trcurrentuseruserfield.Attributes.Add("style", "display:none");
                            trcurrentuseruserfield1.Attributes.Add("style", "display:none");
                            trcurrentuserpeoplepicker1.Attributes.Add("style", "display:none");
                        }
                        else if (specificuser.Key != null)
                        {
                            trcurrentuserpeoplepicker.Attributes.Add("style", "display:block");
                            trcurrentuserpeoplepicker1.Attributes.Add("style", "display:block");
                            //   trcurrentuseruserfield.Attributes.Add("style", "display:none");
                            trcurrentuseruserfield1.Attributes.Add("style", "display:none");
                            //pAssetpeopleeditor.CommaSeparatedAccounts = specificuser.Value;
                            pAssetpeopleeditor.SetValues(specificuser.Value);
                            rdAssetowner.SelectedIndex = 1;
                        }
                        else if (userquestion.Key != null)
                        {
                            trcurrentuserpeoplepicker.Attributes.Add("style", "display:none");
                            trcurrentuserpeoplepicker1.Attributes.Add("style", "display:none");
                            //   trcurrentuseruserfield.Attributes.Add("style", "display:block");
                            trcurrentuseruserfield1.Attributes.Add("style", "display:block");
                            ddlUserField.SelectedIndex = ddlUserField.Items.IndexOf(ddlUserField.Items.FindByValue(userquestion.Value));

                            rdAssetowner.SelectedIndex = 2;
                        }
                        else if (allAssets.Key != null)
                        {
                            rdAssetowner.SelectedIndex = 3;
                            trcurrentuserpeoplepicker.Attributes.Add("style", "display:none");
                            //   trcurrentuseruserfield.Attributes.Add("style", "display:none");
                            trcurrentuseruserfield1.Attributes.Add("style", "display:none");
                            trcurrentuserpeoplepicker1.Attributes.Add("style", "display:none");
                        }

                    }
                    else if (serviceQuest.QuestionType.ToLower() == "resources")
                    {
                        trZoom.Visible = true;
                    }
                    else
                    {
                        if (service != null && !service.CreateParentServiceRequest)
                        {
                            lblMeesageNoParentService.Visible = true;
                        }

                        pDropdownProperties.Visible = true;
                    }
                }
            }
            else
            {
                pDropdownProperties.Visible = true;
            }


            if (IsPostBack && !string.IsNullOrEmpty(Request[questionBasedOnNew.UniqueID]) && Request[questionBasedOnNew.UniqueID].ToLower() == "requesttype"
                && Request[ddlRTModule.UniqueID] == Request[hdnRequestTypeModule.UniqueID])
            {
                LoadRequestTypeTree(Request[ddlRTModule.UniqueID]);
            }

            base.OnInit(e);
        }

        private void addDefaultUserControl(int selectionType)
        {
            
            UserValueBox pEditorUser = divDefaultUser.FindControl("pEditor") as UserValueBox;
            if (pEditorUser == null)
                pEditorUser = new UserValueBox();
            pEditorUser.ID = "pEditor";
            pEditorUser.SelectionSet = "";
            selectionType = UGITUtility.StringToInt(ddlUserType.SelectedValue);
            if (selectionType == 1)
                pEditorUser.SelectionSet = "User";
            else if (selectionType == 2)
                pEditorUser.SelectionSet = "Group";
            else if (selectionType == 3)
            {
                trUserDefaultValue.Visible = false;
                pEditorUser.SelectionSet = "User";
            }
            else if (selectionType == 4)
                pEditorUser.SelectionSet = "User,Manager";
            else if (selectionType== 5)
            {
                trGrpPeoplePicker.Visible = true;
                trUserDefaultValue.Visible = false;
            }
            Session.Add(pEditorUser.UserTokenBoxAdd.ClientID, pEditorUser.UserType);
            pEditorUser.Attributes.Add("ugselectionset", pEditorUser.UserType);
            pEditorUser.CssClass = "userValueBox-dropDown";
            //pEditorUser.UserTokenBoxAdd.AutoPostBack = false;
            //pEditorUser.DataBind();

            pEditorUser.isMulti = !chkSingleEntryOnly.Checked;
            divDefaultUser.Controls.Add(pEditorUser);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //string url = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlView, newParam, formTitle, "WIKI", string.Empty, "WikiHelp", txtWikis.ClientID)); //"TicketWiki"
            //aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','75','90',0,'{1}')", url, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));

            if (service != null && service.Tasks != null && service.Tasks.Count > 0 && questionBasedOnNew.SelectedValue.ToLower() == "applicationaccessrequest" && tpControl == null)
            {
                trpredecessorsControl.Visible = true;
                trpredecessorslable.Visible = true;
                tpControl = (TaskPredecessorsControl)Page.LoadControl(@"~/ControlTemplates/uGovernIT/Task/TaskPredecessorsControl.ascx");
                List<UGITTask> relations = service.Tasks.Where(x => x.ID > 0 && x.SubTaskType == ServiceSubTaskType.Task).OrderBy(x => x.ItemOrder).ToList();
                tpControl.ServiceTasks = relations;
                tpControl.PredecessorMode = PredecessorType.ServiceTask;

                pheditcontrol.Controls.Add(tpControl);
            }
        }
        private void LoadQuestionTypes(DropDownList dropdownList)
        {
            List<string> lstQuestionTypes = new List<string>();
            lstQuestionTypes.Add("SingleChoice");
            lstQuestionTypes.Add("MultiChoice");
            lstQuestionTypes.Add("Checkbox");
            lstQuestionTypes.Add("TextBox");
            lstQuestionTypes.Add("Number");
            lstQuestionTypes.Add("UserField");
            lstQuestionTypes.Add("DateTime");
            lstQuestionTypes.Add("Lookup");
            lstQuestionTypes.Add("Assets Lookup");
            lstQuestionTypes.Add("RequestType");
            lstQuestionTypes.Add("Rating");
            lstQuestionTypes.Add("ApplicationAccessRequest");
            lstQuestionTypes.Add("RemoveUserAccess");
            lstQuestionTypes.Add("Resources");
            foreach (string choice in lstQuestionTypes)
            {
                //Remove rating question from question editor because it is only being used in survey.
                //Remove RemoveAccess question from question editor .

                if (choice.ToLower() != Constants.ServiceQuestionType.Rating && choice.ToLower() != Constants.ServiceQuestionType.RemoveAccess)
                {
                    ////Remove ApplicationAccess question from question editor if it is agent
                    //if (service != null && !string.IsNullOrEmpty(service.Category) && service.Category == "~ModuleAgent~" && choice.ToLower() == Constants.ServiceQuestionType.ApplicationAccess)
                    //    continue;

                    questionBasedOnNew.Items.Add(new ListItem(UGITUtility.AddSpaceBeforeWord(choice), choice));
                }
            }
        }
        private void BindServiceSectionDropDown(List<ServiceSection> sections)
        {
            if (sections != null)
            {
                sections = sections.Where(x => x.SectionName != null).ToList();
                questionServiceSectionsDD.DataSource = sections;
                questionServiceSectionsDD.DataTextField = "SectionName";
                questionServiceSectionsDD.DataValueField = DatabaseObjects.Columns.Id;
                questionServiceSectionsDD.DataBind();
            }
            questionServiceSectionsDD.Items.Insert(0, new ListItem("<Please Select>", "0"));
        }

        protected void GetUserFieldWithinService(List<ServiceQuestion> sections, DropDownList ddl)
        {
            ddl.ClearSelection();
            if (sections != null)
            {
                ddl.DataSource = null;
                ddl.SelectedIndex = -1;

                if (QuestionID > 0)
                    ddl.DataSource = sections.Where(x => x.QuestionType == "UserField" && x.ID != QuestionID).ToList();
                else
                    ddl.DataSource = sections.Where(x => x.QuestionType == "UserField").ToList();

                ddl.DataTextField = DatabaseObjects.Columns.QuestionTitle;
                ddl.DataValueField = DatabaseObjects.Columns.Id;
                ddl.ClearSelection();
                ddl.DataBind();
            }
            ddl.Items.Insert(0, new ListItem("<Select Question>", "0"));
        }

        private void BindAssetTypeDropdown()
        {
            Page dd = new Page();
            RequestTypeDropDownList mlf = rqDropdown as RequestTypeDropDownList; //(RequestTypeDropDownList)dd.LoadControl("~/_controltemplates/15/uGovernIT/Utility/RequestTypeDropDownList.ascx");
            if (mlf != null)
            {
                mlf.ModuleName = "CMDB";
                //mlf.LoadRequestTypeTree("CMDB");
            }
        }
        protected void GetUserFieldWithinService(List<ServiceQuestion> sections, ASPxGridLookup ddl)
        {
            ddl.DataSource = sections.Where(x => x.QuestionType == "UserField").ToList();
            ddl.DataBind();

            //ddl.Items.Insert(0, new ListItem("<Select Question>", "0"));
        }
        protected void BindDateTimeQuestions(List<ServiceQuestion> sections, DropDownList ddl)
        {
            ddl.ClearSelection();
            if (sections != null)
            {
                ddl.DataSource = null;
                ddl.DataSource = sections.Where(x => x.QuestionType.ToLower() == "datetime" && x.ID != QuestionID).ToList();
                ddl.DataTextField = DatabaseObjects.Columns.QuestionTitle;
                ddl.DataValueField = DatabaseObjects.Columns.Id;
                ddl.ClearSelection();
                ddl.DataBind();
            }
            ddl.Items.Insert(0, new ListItem("<Select Question>", "0"));
        }
        protected void BindTextQuestions(List<ServiceQuestion> sections, DropDownList ddl)
        {
            ddl.ClearSelection();
            ddl.DataSource = null;
            ddl.SelectedIndex = -1;
            ddl.DataSource = sections.Where(x => x.QuestionType.ToLower() == "textbox").ToList();
            ddl.DataTextField = DatabaseObjects.Columns.QuestionTitle;
            ddl.DataValueField = DatabaseObjects.Columns.Id;
            ddl.ClearSelection();
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("<Select Question>", "0"));
        }

        protected void BindTextQuestions(List<ServiceQuestion> sections, ASPxGridLookup ddl)
        {
            ddl.DataSource = sections.Where(x => x.QuestionType.ToLower() == "textbox").ToList();
            ddl.DataBind();

        }
        protected void BtSaveQuestionClick(object sender, EventArgs e)
        {
            ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
            ServiceQuestion question = null;
            if (!Page.IsValid || !ASPxEdit.ValidateEditorsInContainer(this))
            {
                return;
            }
            if (txtEditSection.Visible && txtEditSection.Text != string.Empty)
            {
                questionServiceSectionsDD.CausesValidation = false;
            }
            int svcConfigID = 0;
            int.TryParse(hfServiceID.Value, out svcConfigID);

            int questionID = 0;
            int.TryParse(hfQuestionID.Value, out questionID);

            if (svcConfigID > 0)
            {
                if (serviceQuest == null && questionID <= 0)
                {
                    question = new ServiceQuestion();
                    question.ServiceID = svcConfigID;
                    //finds next order number
                    int maxOrder = 0;
                    if (service.Questions != null && service.Questions.Count > 0)
                    {
                        maxOrder = service.Questions.Max(x => x.ItemOrder);
                    }
                    question.ItemOrder = maxOrder + 1;
                }
                else
                {
                    question = serviceQuest;
                }

                if (question.ServiceID == svcConfigID)
                {
                    long sectionID = 0;
                    long.TryParse(hfEditSectionID.Value, out sectionID);
                    if (sectionID > 0)
                    {
                        ServiceSectionManager serviceSectionManager = new ServiceSectionManager(HttpContext.Current.GetManagerContext());
                        ServiceSection section = serviceSectionManager.LoadByID(sectionID);
                        section.ServiceID = svcConfigID;
                        section.SectionName = txtEditSection.Text.Trim();
                        section.Title = txtEditSection.Text;
                        serviceSectionManager.Save(section);
                        sectionID = section.ID;

                    }
                    else if (sectionID == -1)
                    {
                        ServiceSectionManager serviceSectionManager = new ServiceSectionManager(HttpContext.Current.GetManagerContext());
                        ServiceSection section = new ServiceSection();
                        section.ServiceID = svcConfigID;
                        section.SectionName = txtEditSection.Text.Trim();
                        section.Title = txtEditSection.Text;
                        if (service.Sections != null)
                        {
                            section.ItemOrder = service.Sections.Count;
                            if (service.Sections.Count == 0)
                                section.ItemOrder = 1;
                        }
                        serviceSectionManager.Save(section);
                        sectionID = section.ID;
                    }
                    else
                    {
                        long.TryParse(questionServiceSectionsDD.SelectedValue, out sectionID);
                    }

                    question.TokenName = txtToken.Text.Trim();
                    question.QuestionType = questionBasedOnNew.SelectedValue;
                    question.FieldMandatory = cbMandatory.Checked;
                    question.EnableZoomIn = false;

                    question.QuestionTitle = txtQuestion.Text.Trim();
                    question.Helptext = questionHelpTextNew.Text.Trim();
                    question.ServiceSectionID = sectionID;
                    question.ContinueSameLine = chkIsContinue.Checked;
                    string uploadFileURL = string.Empty;
                    switch (ddlTargetTypes.SelectedValue)
                    {
                        case "File":
                            if (fileUploadControls.HasFile)
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
                                fileUploadControls.SaveAs(folderPath + "/" + Path.GetFileName(fileUploadControls.FileName));

                                question.NavigationUrl = finalPath + "/" + Path.GetFileName(fileUploadControls.FileName);
                                question.NavigationType = ddlTargetTypes.SelectedValue;
                            }
                            break;
                        case "Link":
                            question.NavigationUrl = txtFileLinks.Text.Trim();
                            question.NavigationType = ddlTargetTypes.SelectedValue;
                            break;
                        case "Wiki":
                            {
                                if (txtWikis.Visible)
                                {
                                    question.NavigationUrl = txtWikis.Text.Trim();
                                }
                                question.NavigationType = ddlTargetTypes.SelectedValue;
                            }
                            break;
                        case "HelpCard":
                            {
                                if (txtHelpCards.Visible)
                                {
                                    question.NavigationUrl = txtHelpCards.Text.Trim();
                                }
                                question.NavigationType = ddlTargetTypes.SelectedValue;
                            }
                            break;
                        default:
                            break;
                    }

                    Dictionary<string, string> dataTypeParams = new Dictionary<string, string>();
                    if (questionBasedOnNew.SelectedValue.ToLower() == "singlechoice")
                    {
                        dataTypeParams.Add("optiontype", ddlOptionType.SelectedValue);

                        if (txtDropdownOptions.Text != string.Empty && !PickfromField.Checked)
                        {
                            dataTypeParams.Add("options", txtDropdownOptions.Text.Trim().Replace("\r\n", Constants.Separator2));
                        }

                        if (txtDropDownDefaultVal.Text != string.Empty)
                        {
                            dataTypeParams.Add("defaultval", txtDropDownDefaultVal.Text.Trim());
                        }

                        //Save selected values from ChooseFromFields list to dictinory if condition true 
                        if (PickfromField.Visible && PickfromField.Checked)
                        {
                            dataTypeParams.Add("pickfromfield", Convert.ToString(PickfromField.Checked));
                            dataTypeParams.Add("selectedlist", ChooseFromList.SelectedValue);
                            dataTypeParams.Add("selectedfield", ChooseFromFields.SelectedValue);
                        }

                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "multichoice")
                    {
                        dataTypeParams.Add("moptiontype", ddlMultiSelectOptionType.SelectedValue);
                        if (txtMSOptions.Text != string.Empty)
                        {
                            dataTypeParams.Add("options", txtMSOptions.Text.Trim().Replace("\r\n", Constants.Separator2));
                        }
                        if (txtMSDefault.Text != string.Empty)
                        {
                            dataTypeParams.Add("defaultval", txtMSDefault.Text.Trim());
                        }
                        if (chkbxPickFromFieldMC.Visible && chkbxPickFromFieldMC.Checked)
                        {
                            dataTypeParams.Add("pickfromfield", Convert.ToString(chkbxPickFromFieldMC.Checked));
                            dataTypeParams.Add("selectedlist", ddlChoosefromListMC.SelectedValue);
                            dataTypeParams.Add("selectedfield", ddlChoosefromFieldMC.SelectedValue);
                        }
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "checkbox")
                    {
                        question.FieldMandatory = false;
                        dataTypeParams.Add("defaultval", cbDefaultValue.Checked.ToString());
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "textbox")
                    {
                        question.EnableZoomIn = chkEnableZoomIn.Checked;
                        if (ddlTxtBoxType.SelectedItem != null)
                        {
                            dataTypeParams.Add("textmode", ddlTxtBoxType.SelectedValue);
                        }

                        if (txtTBDefault.Text != string.Empty)
                        {
                            dataTypeParams.Add("defaultval", txtTBDefault.Text.Trim());
                        }

                        if (chkSetLength.Checked && spnbtnTextMaxLength.Value != null)
                        {
                            dataTypeParams.Add("maxlength", Convert.ToString(spnbtnTextMaxLength.Value));
                        }

                        if (chkUserDeskLocation.Checked == true)
                        {
                            if (ddlUserFieldDeskLocation.SelectedIndex != 0)
                                dataTypeParams.Add("dependentuserdesklocationquestion", ddlUserFieldDeskLocation.SelectedValue);
                        }
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "number")
                    {
                        if (ddlNumberBoxType.SelectedItem != null)
                        {
                            dataTypeParams.Add("textmode", ddlNumberBoxType.SelectedValue);
                        }

                        if (txtNumberDefaultValue.Text != string.Empty)
                        {
                            if (ddlNumberBoxType.SelectedValue == "Integer")
                            {
                                dataTypeParams.Add("defaultval", Convert.ToString(UGITUtility.StringToInt(txtNumberDefaultValue.Text.Trim())));
                            }
                            else if (ddlNumberBoxType.SelectedValue == "Double")
                            {
                                dataTypeParams.Add("defaultval", Convert.ToString(UGITUtility.StringToDouble(txtNumberDefaultValue.Text.Trim())));
                            }
                        }
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "userfield")
                    {
                        question.FieldMandatory = cbMandatory.Checked;
                        UserValueBox pEditorUser = divDefaultUser.FindControl("pEditor") as UserValueBox;
                        string defaultVal = string.Empty;
                        if (pEditorUser != null)
                            defaultVal = pEditorUser.GetValues();
                        if (ddlUserType.SelectedIndex == 0)
                        {
                            dataTypeParams.Add("usertype", "0");
                            if (!string.IsNullOrEmpty(defaultVal))
                            {
                                dataTypeParams.Add("defaultval", defaultVal);
                            }
                        }
                        else if (ddlUserType.SelectedIndex == 1)
                        {
                            dataTypeParams.Add("usertype", "1");
                            if (!string.IsNullOrEmpty(defaultVal))
                            {
                                dataTypeParams.Add("defaultval", defaultVal);
                            }
                        }
                        else if (ddlUserType.SelectedIndex == 2)
                        {
                            dataTypeParams.Add("usertype", "2");
                            if (!string.IsNullOrEmpty(defaultVal))
                            {
                                dataTypeParams.Add("defaultval", defaultVal);
                            }
                        }
                        else if (ddlUserType.SelectedIndex == 3)
                        {
                            dataTypeParams.Add("usertype", "3");
                            dataTypeParams.Add("loggedinUser", Convert.ToString(User.Id));
                        }
                        else if (ddlUserType.SelectedIndex == 4)
                        {
                            dataTypeParams.Add("usertype", "4");
                            if (!string.IsNullOrEmpty(defaultVal))
                            {
                                dataTypeParams.Add("defaultval", defaultVal);
                            }
                        }
                        else if (ddlUserType.SelectedIndex == 5 && !string.IsNullOrEmpty(pEditorGrp.GetValues()))
                        {
                            dataTypeParams.Add("usertype", "5");
                            dataTypeParams.Add("specificusergroup", pEditorGrp.GetValues());
                        }

                        dataTypeParams.Add("singleentryonly", Convert.ToString(chkSingleEntryOnly.Checked));

                        if (chkUserManager.Checked == true)
                        {
                            if (ddlUserManager.SelectedIndex != 0)
                                dataTypeParams.Add("dependentusermanagerquestion", ddlUserManager.SelectedValue);
                        }
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "datetime")
                    {

                        if (ddlDateFormat.SelectedItem != null)
                        {
                            dataTypeParams.Add("datemode", ddlDateFormat.SelectedValue);
                        }
                        if (speDefaultDateNoofDays.Text != string.Empty)
                        {
                            string dfltDate = string.Empty;
                            if (ddlPlusMinusDefaultDate.SelectedIndex == 0)
                                dfltDate = speDefaultDateNoofDays.Text;
                            else
                                dfltDate += "-" + speDefaultDateNoofDays.Text;
                            dataTypeParams.Add("defaultval", dfltDate);
                        }
                        //DateTime Questions - add validation for Present ,Past,Future  :Start
                        if (chkDateValidations.Checked)
                        {
                            dataTypeParams.Add("dateconstraint", "conditional");
                            dataTypeParams.Add("presentdate", Convert.ToString(presentDates.Checked));
                            dataTypeParams.Add("pastdate", Convert.ToString(pastDates.Checked));
                            dataTypeParams.Add("futuredate", Convert.ToString(futureDates.Checked));
                            if (rdbDateValidationAgainst.SelectedIndex == 1)
                                dataTypeParams.Add("validateagainst", ddlDateQuestions.SelectedValue);
                            else
                                dataTypeParams.Add("validateagainst", "currentdate");
                        }
                        //DateTime Questions - add validation for Present ,Past,Future  :end
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "lookup")
                    {
                        if (ddlLookupList.SelectedItem != null)
                        {
                            dataTypeParams.Add("lookuplist", ddlLookupList.SelectedValue);
                        }

                        if (lpModuletr.Visible && ddlLPModule.SelectedItem != null)
                        {
                            dataTypeParams.Add("module", ddlLPModule.SelectedValue);
                        }

                        if (ddlLookupFields.SelectedItem != null)
                        {
                            dataTypeParams.Add("lookupfield", ddlLookupFields.SelectedValue);
                        }
                        if (chkDepartmentowner.Checked == true)
                        {
                            //if (ddlUserField1.GetValues() != null)
                            //    dataTypeParams.Add("departmentuserquestion", ddlUserField1.GetValues());
                            //dataTypeParams.Add("locationuserquestion", ddlUserField1.GetValues());
                            if (ddlUserField1.SelectedIndex != 0 && ddlLookupList.SelectedValue.ToLower() == "departmentlookup")
                                dataTypeParams.Add("departmentuserquestion", ddlUserField1.SelectedValue);

                            if (ddlLookupList.SelectedItem != null && ddlLookupList.SelectedValue.ToLower() == "locationlookup")
                                dataTypeParams.Add("locationuserquestion", ddlUserField1.SelectedValue);

                            if (ddlLookupList.SelectedItem != null && ddlLookupList.SelectedValue.ToLower() == "companydivisions")
                                dataTypeParams.Add("companydivisions", ddlUserField1.SelectedValue);
                        }
                        dataTypeParams.Add("multi", chbAllowMultiLookup.Checked.ToString());
                        if (ddlLookupList.SelectedValue.ToLower() == "sublocation" && chkDepndLocation.Checked)
                        {
                            if (ddlLocationQuestions.SelectedIndex != 0)
                                dataTypeParams.Add("dependentlocationquestion", ddlLocationQuestions.SelectedValue);
                        }
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "requesttype")
                    {
                        if (ddlRTModule.SelectedItem != null)
                        {
                            dataTypeParams.Add("module", ddlRTModule.SelectedValue);
                        }

                        List<string> requestTypes = new List<string>();
                        List<TreeListNode> sNodes = requestTypeTreeList.GetSelectedNodes();
                        if (sNodes.Count != requestTypeTreeList.GetAllNodes().Count)
                        {
                            foreach (TreeListNode node in sNodes)
                            {
                                if (UGITUtility.StringToInt(node.Key) > 0)
                                    requestTypes.Add(node.Key);
                            }
                        }
                        else
                        {
                            requestTypes.Add("all");
                        }

                        dataTypeParams.Add("requesttypes", string.Join(Constants.Separator2, requestTypes.ToArray()));
                        dataTypeParams.Add("enablecategorydropdown", chkEnableRCategoryDropDown.Checked.ToString().ToLower());
                        dataTypeParams.Add("enableissuetypedropdown", chkbxEnableIssueType.Checked.ToString().ToLower());
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "attachment")
                    {
                        //Supressed this feature. Doing simple attachment indepdend to section 
                        //if (ddlLookupList.SelectedItem != null)
                        //{
                        //    dataTypeParams.Add("attachmenttype", "single");
                        //    if (cbAttMultiple.Checked)
                        //    {
                        //        dataTypeParams.Add("attachmenttype", "multiple");

                        //        if (ddlLookupFields.SelectedItem != null)
                        //        {
                        //            dataTypeParams.Add("sizelimit", txtAttSizeLimit.Text.Trim());
                        //        }
                        //    }
                        //}


                        //if (ddlLookupFields.SelectedItem != null)
                        //{
                        //    dataTypeParams.Add("attachmentcount", ddlAttCount.SelectedValue);
                        //}
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "applicationaccessrequest")
                    {
                        question.EnableZoomIn = chkEnableZoomIn.Checked;
                        string dueDateFrom = string.Empty;
                        if (ddlDueDateFrom.SelectedIndex == 0)
                            dueDateFrom = ddlDueDateFrom.SelectedItem.Text + ":";
                        else
                            dueDateFrom = ddlDueDateFrom.SelectedValue + ":";
                        if (ddlPlusMinus.SelectedIndex == 0)
                            dueDateFrom += dxNoOfDays.Text;
                        else
                            dueDateFrom += "-" + dxNoOfDays.Text;

                        dataTypeParams.Add("duedatefrom", dueDateFrom);

                        if (ddlAccessMirrorFrom.SelectedIndex != 0)
                            dataTypeParams.Add(ConfigConstants.MirrorAccessFrom, ddlAccessMirrorFrom.SelectedValue);
                        if (rdbPickUserFrom.SelectedIndex == 0)
                        {
                            var selectedVals = ddlUserFieldAppRequestQuestions.GridView.GetSelectedFieldValues(DatabaseObjects.Columns.ID);
                            string selectedQuestions ="";
                            if (selectedVals != null && selectedVals.Count() > 0)
                               selectedQuestions = string.Join(",", selectedVals.Select(x => Convert.ToString(x)));
                            dataTypeParams.Add(ConfigConstants.NewUSer, selectedQuestions);
                            //enable auto create account 
                            //ServiceTask.AutoCreateAccountTask(service, chkAutoCreateAccountTask.Checked, selectedQuestions);
                        }
                        else if (rdbPickUserFrom.SelectedIndex == 1)
                        {

                            dataTypeParams.Add(ConfigConstants.ExistingUser, Convert.ToString(ddlUserFieldAppRequestQuestions.Value));

                            if (chbxEnableAccessMode.Checked)
                            {
                                List<ListItem> selected = chkbxlstAccessReqMode.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
                                if (selected != null && selected.Count > 0)
                                    dataTypeParams.Add(ConfigConstants.AccessRequestMode, string.Join(";", selected.Select(x => x.Value)));
                            }
                        }
                        if (cbRTAllApplications.Checked)
                        {
                            dataTypeParams.Add("application", "All");
                        }
                        else
                        {
                            List<string> applications = new List<string>();
                            foreach (ListItem item in chkRTApplications.Items)
                            {
                                if (item.Selected && item.Value.IndexOf("applicationid") != -1)
                                {
                                    applications.Add(item.Value);
                                }
                            }
                            dataTypeParams.Add("application", string.Join(Constants.Separator1, applications.ToArray()));
                        }

                        dataTypeParams.Add("disableAllCheck", cbDisableAllCheckBox.Checked.ToString());
                        string predecessorIDs = string.Empty;
                        if (tpControl != null)
                        {
                            foreach (var item in tpControl.SelectedPredecessorsId)
                            {
                                if (!string.IsNullOrEmpty(predecessorIDs))
                                    predecessorIDs += Constants.Separator6;

                                predecessorIDs += item;
                            }
                        }
                        dataTypeParams.Add(ConfigConstants.Predecessors, predecessorIDs);
                    }
                    else if (questionBasedOnNew.SelectedValue.ToLower() == "assets lookup")
                    {
                        if (chkincludeasset.Checked)
                        {
                            dataTypeParams.Add("IncludedepartmentAsset", "TRUE");
                        }
                        if (chkbxAssetType.Checked)
                        {
                            RequestTypeDropDownList rqDropDown = rqDropdown as RequestTypeDropDownList;
                            if (rqDropDown != null)
                            {
                                string sAssetType = string.Join(",", rqDropDown.GetSelectedValue());
                                if (!string.IsNullOrEmpty(sAssetType))
                                    dataTypeParams.Add("assettype", sAssetType);
                            }

                        }
                        if (rdAssetowner.SelectedIndex == 0)
                            dataTypeParams.Add("currentuser", User.Id);
                        else if (rdAssetowner.SelectedIndex == 1)
                        {
                            dataTypeParams.Add("specificuser", pAssetpeopleeditor.GetValues());
                        }
                        else if (rdAssetowner.SelectedIndex == 2)
                        {
                            if (ddlUserField.SelectedIndex != 0)
                                dataTypeParams.Add("userquestion", ddlUserField.SelectedValue);
                        }
                        else if (rdAssetowner.SelectedIndex == 3)
                        {
                            dataTypeParams.Add("allAssets", "true");
                        }

                    }
                    question.QuestionTypePropertiesDicObj = dataTypeParams;
                    serviceQuestionManager.Save(question);
                }
                else if (questionBasedOnNew.SelectedValue.ToLower() == "resources")
                {
                    question.EnableZoomIn = chkEnableZoomIn.Checked;
                }

                if (saveAndNewQuestionEnable)
                {
                    //Redirect to create new question
                    string questionListUrl = UGITUtility.GetAbsoluteURL(Request.Path);
                    NameValueCollection queryCollection = new System.Collections.Specialized.NameValueCollection(Request.QueryString);
                    StringBuilder querys = new StringBuilder();
                    foreach (string key in queryCollection.AllKeys)
                    {
                        if (key == null)
                            continue;

                        if (key.Trim() != "questionid")
                            querys.AppendFormat("&{0}={1}", key, queryCollection[key]);
                        else
                            querys.AppendFormat("&questionid=0");
                    }
                    questionListUrl = string.Format("{0}?{1}", questionListUrl, querys.ToString());
                    Context.Response.Redirect(questionListUrl);
                }
                else
                {
                    Context.Cache.Add(string.Format("SVCConfigQuestion-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
            }
        }
        protected void BtSaveAndNewQuestionClick(object sender, EventArgs e)
        {
            saveAndNewQuestionEnable = true;
            BtSaveQuestionClick(sender, e);
        }

        protected void BtDelete_Click(object sender, EventArgs e)
        {
            if (serviceQuest != null)
            {
                ServiceQuestionManager serviceQuestionManager = new ServiceQuestionManager(HttpContext.Current.GetManagerContext());
                serviceQuestionManager.Delete(serviceQuest);

                Context.Cache.Add(string.Format("SVCConfigQuestion-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        protected void BtClosePopup_Click(object sender, EventArgs e)
        {
            Context.Cache.Add(string.Format("SVCConfigQuestion-{0}", User.Id), true, null, DateTime.Now.AddMinutes(50), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void QuestionBasedOnNew_SelectedIndexChanged(object sender, EventArgs e)
        {
            PickfromField.Visible = false;
            pDropdownControlType.Visible = false;
            pDropdownControlTypeButtion.Visible = false;

            pDropdownProperties.Visible = false;
            pTxtBoxProperties.Visible = false;
            pDateProperties.Visible = false;
            pUserProperties.Visible = false;
            pCheckboxProperties.Visible = false;
            pLookupProperties.Visible = false;
            pRequestTypeProperties.Visible = false;
            pMultiSelectProperties.Visible = false;
            pNumberProperties.Visible = false;
            pAppAccessReqProperties.Visible = false;
            trpDropdownControlTypeButtion.Visible = false;
            trpDropdownControlType.Visible = false;
            ChooseFromList.ClearSelection();
            ChooseFromFields.ClearSelection();

            //make dropdown invisible
            ChoseFromDropdown.Visible = false;
            //make the checkbox bydefault unselected
            PickfromField.Checked = false;

            trpredecessorslable.Visible = false;
            trpredecessorsControl.Visible = false;

            trMandatory.Visible = true;
            lblMeesageNoParentService.Visible = false;
            pUserFieldProperties.Attributes.Add("style", "display:none");
            trZoom.Visible = false;
            if (questionBasedOnNew.SelectedValue.ToLower() == "singlechoice")
            {
                trDropdownDefaultOptions.Visible = true;
                singleChoiceDefaulttr.Visible = true;
                singleChoiceTextboxtr.Visible = true;
                pDropdownControlTypeButtion.Visible = true;
                pDropdownControlType.Visible = true;


                pDropdownProperties.Visible = true;
                txtDropdownOptions.Text = string.Empty;
                txtDropDownDefaultVal.Text = string.Empty;
                if (!string.IsNullOrEmpty(ObjConfigManager.GetValue(ConfigurationVariable.ServiceChoiceQuestionPickLists)))
                {
                    PickfromField.Visible = true;
                    FillChooseFromListsDropdown(ChooseFromList);
                   // ddlOptionType.ClearSelection();
                }
                else { PickfromField.Visible = false; }

            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "multichoice")
            {

                pMultiSelectProperties.Visible = true;
                trpDropdownControlTypeButtion.Visible = true;
                trpDropdownControlType.Visible = true;
                txtMSOptions.Text = string.Empty;
                txtMSDefault.Text = string.Empty;
            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "checkbox")
            {
                pCheckboxProperties.Visible = true;
                cbDefaultValue.Checked = false;
                trMandatory.Visible = false;

            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "textbox")
            {
                pTxtBoxProperties.Visible = true;
                trUserDeskLocation.Visible = true;
                ddlTxtBoxType.ClearSelection();
                txtTBDefault.Text = string.Empty;
                spnbtnTextMaxLength.Value = null;
                trZoom.Visible = true;
            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "number")
            {
                pNumberProperties.Visible = true;
                ddlNumberBoxType.ClearSelection();
            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "userfield")
            {
                pUserProperties.Visible = true;
                trUserManager.Visible = true;
            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "datetime")
            {
                pDateProperties.Visible = true;
                ddlDateFormat.ClearSelection();
                speDefaultDateNoofDays.Text = string.Empty;
            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "lookup")
            {
                pLookupProperties.Visible = true;
                ddlLookupList.Items.Clear();
                FillLookupListsDropDown();
                //chbAllowMultiLookup.Visible = true;
                chbAllowMultiLookup.Checked = false;
                //if (ddlLookupList.SelectedValue == DatabaseObjects.Lists.Department)
                //{
                //    chbAllowMultiLookup.Visible = true;
                //}
            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "requesttype")
            {
                pRequestTypeProperties.Visible = true;

                if (ddlRTModule.Items.Count <= 0)
                {
                    ddlRTModule.Items.Add(new ListItem("Select Module", ""));
                    DataTable modules = ObjModuleViewManager.LoadAllModules();
                    if (modules != null)
                    {
                        modules.DefaultView.RowFilter = string.Format("{0} <> 'SVC'", DatabaseObjects.Columns.ModuleName);
                        modules.DefaultView.Sort = string.Format("{0} Asc", DatabaseObjects.Columns.ModuleName);
                       
                        foreach (DataRowView module in modules.DefaultView)
                        {
                            string moduleName = Convert.ToString(module[DatabaseObjects.Columns.ModuleName]);

                            if (Convert.ToString(module[DatabaseObjects.Columns.ModuleType]) == Convert.ToString(ModuleType.SMS) || moduleName == "PMM" || moduleName == "NPR" || moduleName == "TSK")
                                ddlRTModule.Items.Add(new ListItem(Convert.ToString(module[DatabaseObjects.Columns.Title]), Convert.ToString(module[DatabaseObjects.Columns.ModuleName])));
                        }
                       
                    }
                }

                DDLRTModules_SelectedIndexChanged(ddlRTCategory, new EventArgs());
                chkEnableRCategoryDropDown.Checked = false;
            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "attachment")
            {
                //Supressed this feature. Doing simple attachment indepdend to section 
                //pAttachmentProperties.Visible = true;
                //cbAttSingle.Checked = true;
                //attSubContainer.Visible = false;
            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "applicationaccessrequest")
            {
                if (service != null && !service.CreateParentServiceRequest)
                {
                    lblMeesageNoParentService.Visible = true;
                }
                pAppAccessReqProperties.Visible = true;
                rdbPickUserFrom.ClearSelection();
                ddlAccessMirrorFrom.ClearSelection();
                BindApplications();
                trZoom.Visible = true;
                trpredecessorsControl.Visible = true;
                trpredecessorslable.Visible = true;
            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "removeuseraccess")
            {
                if (service != null && !service.CreateParentServiceRequest)
                {
                    lblMeesageNoParentService.Visible = true;
                }
                trMandatory.Visible = false;
            }

            else if (questionBasedOnNew.SelectedValue.ToLower() == "assets lookup")
            {
                BindAssetTypeDropdown();
                pUserFieldProperties.Attributes.Add("style", "display:block");
                trassetowner.Attributes.Add("style", "display:block");
                trradiooption.Attributes.Add("style", "display:block");
                // trcurrentuseruserfield.Attributes.Add("style", "display:none");
                trcurrentuseruserfield1.Attributes.Add("style", "display:none");
                trcurrentuserpeoplepicker.Attributes.Add("style", "display:none");
                trcurrentuserpeoplepicker1.Attributes.Add("style", "display:none");
                if (service != null && !service.CreateParentServiceRequest)
                {
                    lblMeesageNoParentService.Visible = true;
                }
                if (rdAssetowner.SelectedIndex == 0)
                {

                }
                else if (rdAssetowner.SelectedIndex == 1)
                {
                    trcurrentuserpeoplepicker.Attributes.Add("style", "display:block");
                    trcurrentuserpeoplepicker1.Attributes.Add("style", "display:block");
                }
                else if (rdAssetowner.SelectedIndex == 2)
                {
                    //   trcurrentuseruserfield.Attributes.Add("style", "display:block");
                    trcurrentuseruserfield1.Attributes.Add("style", "display:block");
                }
                //pUserProperties.Attributes.Add("style", "display:none");
                //pUserFieldProperties.Attributes.Add("style", "display:block");
                //trcurrentuseruserfield.Attributes.Add("style", "display:none");
                //trcurrentuserpeoplepicker.Attributes.Add("style", "display:none");
                //trcurrentuseruserfield1.Attributes.Add("style", "display:none");
                //trcurrentuserpeoplepicker1.Attributes.Add("style", "display:none");

            }
            else if (questionBasedOnNew.SelectedValue.ToLower() == "resources")
            {
                trZoom.Visible = true;
            }
        }
        private void SelectApplications()
        {

            List<ListItem> dropboxItems = chkRTApplications.Items.Cast<ListItem>().ToList();
            if (serviceQuest != null)
            {
                KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "application");
                if (param.Key != null)
                {
                    if (param.Value.ToLower() == "all")
                    {
                        cbRTAllApplications.Checked = true;
                        foreach (ListItem item in chkRTApplications.Items)
                        {
                            item.Selected = true;
                        }
                        foreach (ListItem item1 in dropboxItems)
                        {
                            item1.Selected = true;
                        }
                    }
                    else
                    {
                        string[] lstValues = param.Value.Split(new string[] { Constants.Separator1 }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in lstValues)
                        {
                            ListItem lstitem = dropboxItems.FirstOrDefault(x => x.Value == item);
                            if (lstitem != null)
                            {
                                lstitem.Selected = true;
                            }
                        }
                    }
                    foreach (ListItem lstItem in chkRTApplications.Items)
                    {
                        if (!lstItem.Value.Contains("applicationid"))
                        {
                            bool isSelected = true;
                            string[] catArrColl = lstItem.Value.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                            if (catArrColl != null && catArrColl.Length > 1)
                            {
                                string categoryValue = catArrColl[1];
                                List<ListItem> lstItems = dropboxItems.FindAll(x => x.Value.StartsWith("category-" + categoryValue + Constants.Separator2 + "applicationid"));
                                foreach (ListItem item in lstItems)
                                {
                                    if (item.Selected != true)
                                    {
                                        isSelected = false;
                                    }
                                }
                                if (isSelected == true)
                                {
                                    lstItem.Selected = true;
                                }
                            }
                               
                        }
                    }
                }
            }

        }
        private void BindApplications()
        {
            string openTicketQuery = string.Format(" {0}!=1 and {1}='{2}' and {3}='False'", DatabaseObjects.Columns.TicketClosed, DatabaseObjects.Columns.TenantID, HttpContext.Current.GetManagerContext().TenantID,DatabaseObjects.Columns.Deleted);
            DataTable dtApplications = GetTableDataManager.GetTableData(DatabaseObjects.Tables.Applications, openTicketQuery);
            if (dtApplications != null && dtApplications.Rows.Count > 0)
            {
                DataView view = new DataView(dtApplications);
                view.Sort = DatabaseObjects.Columns.CategoryNameChoice + " ASC," + DatabaseObjects.Columns.Title + " ASC";
                DataTable distinctValues = view.ToTable(true, DatabaseObjects.Columns.CategoryNameChoice);
                for (int i = distinctValues.Rows.Count - 1; i >= 0; i--)
                {
                    if (distinctValues.Rows[i][DatabaseObjects.Columns.CategoryNameChoice] == DBNull.Value || UGITUtility.ObjectToString(distinctValues.Rows[i][DatabaseObjects.Columns.CategoryNameChoice])==string.Empty)
                        distinctValues.Rows[i].Delete();
                }
                distinctValues.AcceptChanges();
                chkRTApplications.Items.Clear();
                foreach (DataRow dr in distinctValues.Rows)
                {
                    ListItem sectionItem = new ListItem(Convert.ToString(dr[DatabaseObjects.Columns.CategoryNameChoice]), string.Format("category-{0}", Convert.ToString(dr[DatabaseObjects.Columns.CategoryNameChoice])));
                    sectionItem.Attributes.Add("source", string.Format("category-{0}", Convert.ToString(dr[DatabaseObjects.Columns.CategoryNameChoice])));
                    sectionItem.Attributes.Add("class", "skipitem");
                    chkRTApplications.Items.Add(sectionItem);
                    DataRow[] drApps = dtApplications.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.CategoryNameChoice, Convert.ToString(dr[DatabaseObjects.Columns.CategoryNameChoice])));

                    if (drApps.Length > 0)
                    {
                        foreach (DataRow drAppItem in drApps)
                        {
                            ListItem item = new ListItem(Convert.ToString(drAppItem[DatabaseObjects.Columns.Title]), string.Format("category-{1}{2}applicationid-{0}", Convert.ToString(drAppItem[DatabaseObjects.Columns.Id]), Convert.ToString(dr[DatabaseObjects.Columns.CategoryNameChoice]), Constants.Separator2));
                            item.Attributes.Add("style", "padding-left:20px;");
                            item.Attributes.Add("source", string.Format("category-{1}{2}applicationid-{0}", Convert.ToString(drAppItem[DatabaseObjects.Columns.Id]), Convert.ToString(dr[DatabaseObjects.Columns.CategoryNameChoice]), Constants.Separator2));
                            item.Attributes.Add("class", "skipitem");
                            chkRTApplications.Items.Add(item);
                        }
                    }
                }
            }
        }
        protected void CVFieldValidator1_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            if (service != null)
            {
                if (service.Questions != null)
                {
                    ServiceQuestion question = service.Questions.FirstOrDefault(x => x.ID != QuestionID && x.TokenName.ToLower() == e.Value.ToLower());
                    if (question != null)
                    {
                        e.IsValid = false;
                        return;
                    }
                }

                //Check variable is exist with same token name or not. if exist then through error.
                if (service.QMapVariables != null)
                {
                    QuestionMapVariable variable = service.QMapVariables.FirstOrDefault(x => x.ShortName.ToLower() == e.Value.ToLower());
                    if (variable != null)
                    {
                        cvFieldValidator1.ErrorMessage = "Variable already exists with same name, please enter another name";
                        e.IsValid = false;
                    }
                }
            }
        }

        protected void Attm_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            attSubContainer.Visible = false;
            if (button.Text.ToLower() == "multiple")
            {
                attSubContainer.Visible = true;
            }
        }

        private void FillChooseFromFieldListDropdown(DropDownList ddl)
        {
            param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "selectedlist");
            if (param.Key != null)
                BindChoiceFieldFromList(ddl, param.Value);
        }

        private void FillChooseFromListsDropdown(DropDownList ddl)
        {
            if (ddl.Items.Count <= 0)
            {
                string lists = ObjConfigManager.GetValue(ConfigurationVariable.ServiceChoiceQuestionPickLists);
                string[] listArray = lists.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                listArray = listArray.OrderBy(x => x).ToArray();
                foreach (string item in listArray)
                {
                    ddl.Items.Add(new ListItem(item, item));
                }
                ddl.Items.Insert(0, new ListItem("--Select List--", ""));
            }
        }
    

        private void FillLookupListsDropDown()
        {
            if (ddlLookupList.Items.Count <= 0)
            {
                //in sharepoint lookup drop down list loaded from config variable table but in .net version lookup values can be 
                // loaded from fieldconfiguration table with type = 'Lookup'
                FieldConfigurationManager objFieldConfigManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());  //string lists = ConfigManager.GetValue(ConfigurationVariable.ServiceLookupLists);
                List<FieldConfiguration> fieldConfigList = objFieldConfigManager.Load(x => x.Datatype == "Lookup");

                fieldConfigList = fieldConfigList.OrderBy(x => x.FieldName).ToList();
                foreach (FieldConfiguration item in fieldConfigList)
                {
                    if (item.FieldName != "RequestTypeLookup")
                        ddlLookupList.Items.Add(new ListItem(item.FieldName.Replace("Lookup"," ").Trim(), item.FieldName));
                }
                ddlLookupList.Items.Insert(0, new ListItem("--Select List--", ""));
            }
        }

        protected void DDLLookupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlLookupFields.Items.Clear();
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            FieldConfiguration objField = fieldManager.Get(x => x.FieldName == ddlLookupList.SelectedValue);
            if (objField != null)
            {
                ddlLookupFields.Items.Add(new ListItem(objField.ParentFieldName, objField.ParentFieldName));
            }
            else
            {
                ddlLookupFields.Items.Insert(0, new ListItem("--Select Field-- ", ""));
            }

            trSubLocationConfig.Attributes.Add("style", "display:none");
            if (objField != null)
            {
                if (objField.FieldName.ToString().ToLower() == DatabaseObjects.Columns.DepartmentLookup.ToLower() || objField.FieldName.ToString().ToLower() == DatabaseObjects.Columns.LocationLookup.ToLower())
                {
                    chkDepartmentowner.Text = "Pre-fill department of user";
                    tr1radiooption.Attributes.Add("style", "display:block");
                    if (objField.FieldName.ToString().ToLower() == DatabaseObjects.Columns.LocationLookup.ToLower())
                        chkDepartmentowner.Text = "Pre-fill location of user";
                }
                else if (objField.FieldName.ToString().ToLower() == "sublocation")
                {
                    trSubLocationConfig.Attributes.Add("style", "display:block");
                }
                else
                {
                    chkDepartmentowner.Checked = false;
                    ddlUserField1.ClearSelection();
                    tr1radiooption.Attributes.Add("style", "display:none");
                    trSubLocationConfig.Attributes.Add("style", "display:none");
                    ddlUserField1.Visible = false;

                }
            }
            if(objField != null)
            {
                if(objField.Datatype == "Lookup")
                {
                    DataTable moduleList = GetModulesInfoFromLookup(objField.ParentTableName,objField.TenantID);
                    lpModuletr.Visible = false;
                    ddlLPModule.Items.Clear();
                    if (moduleList.Rows.Count > 0)
                    {
                        lpModuletr.Visible = true;
                        foreach (DataRow row in moduleList.Rows)
                        {
                            ddlLPModule.Items.Add(new ListItem(Convert.ToString(row["Name"]), Convert.ToString(row["Name"])));
                        }
                    }

                    chbAllowMultiLookup.Visible = true;
                    chbAllowMultiLookup.Checked = false;
                }
            }
            
        }

        protected void DDLRTModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            requestTypeTreeList.ClearNodes();
            requestTypeTreeList.UnselectAll();
            hdnRequestTypeModule.Value = ddlRTModule.SelectedValue;
            if (sender is DropDownList)
            {
                flag = true;
                LoadRequestTypeTree(ddlRTModule.SelectedValue);
            }
            else
                LoadRequestTypeTree(ddlRTModule.SelectedValue);
        }

        private DataTable GetModulesInfoFromLookup(string lookupList, string tenantid)
        {
            DataTable modules = new DataTable();
            modules.Columns.Add("Name");
            DataTable table = GetTableDataManager.GetTableData(lookupList, $"{DatabaseObjects.Columns.TenantID} = '{tenantid}'");
            if (table != null && UGITUtility.IfColumnExists(DatabaseObjects.Columns.ModuleNameLookup, table))
            {
                //uGITCache.GetDataTable(lookupList);
                modules = table.DefaultView.ToTable(true, DatabaseObjects.Columns.ModuleNameLookup);
                modules.Columns[DatabaseObjects.Columns.ModuleNameLookup].ColumnName = "Name";
            }
            return modules;
        }

        protected void rdAssetowner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkbxAssetType.Checked)
                divAssetTypeDropdown.Attributes.Add("class", "");
            else
                divAssetTypeDropdown.Attributes.Add("class", "hide");
            if (rdAssetowner.SelectedIndex == 0)
            {

                trcurrentuserpeoplepicker.Attributes.Add("style", "display:none");
                // trcurrentuseruserfield.Attributes.Add("style", "display:none");
                trcurrentuserpeoplepicker1.Attributes.Add("style", "display:none");
                trcurrentuseruserfield1.Attributes.Add("style", "display:none");


            }
            else if (rdAssetowner.SelectedIndex == 1)
            {
                trcurrentuserpeoplepicker.Attributes.Add("style", "display:block");
                // trcurrentuseruserfield.Attributes.Add("style", "display:none");
                trcurrentuserpeoplepicker1.Attributes.Add("style", "display:block");
                trcurrentuseruserfield1.Attributes.Add("style", "display:none");

            }
            else if (rdAssetowner.SelectedIndex == 2)
            {
                trcurrentuserpeoplepicker.Attributes.Add("style", "display:none");
                //   trcurrentuseruserfield.Attributes.Add("style", "display:block");
                trcurrentuserpeoplepicker1.Attributes.Add("style", "display:none");
                trcurrentuseruserfield1.Attributes.Add("style", "display:block");

            }
            else if (rdAssetowner.SelectedIndex == 3)
            {
                trcurrentuserpeoplepicker.Attributes.Add("style", "display:none");
                //   trcurrentuseruserfield.Attributes.Add("style", "display:none");
                trcurrentuserpeoplepicker1.Attributes.Add("style", "display:none");
                trcurrentuseruserfield1.Attributes.Add("style", "display:none");

            }
        }

        private void LoadRequestTypeTree(string moduleName)
        {
            DataTable data = GetRequestTypeData(moduleName);
            requestTypeTreeList.DataSource = data;
            requestTypeTreeList.DataBind();
        }

        private DataTable GetRequestTypeData(string moduleName)
        {
            DataTable requestTypeData = null;
            RequestTypeManager requestTypeManager = new RequestTypeManager(HttpContext.Current.GetManagerContext());
            DataTable dt = requestTypeManager.GetDataTable(); // uGITCache.GetDataTable(DatabaseObjects.Lists.RequestType);
            if (dt != null)
            {
                DataRow[] dr = dt.Select(string.Format("{0}='{1}' and ({2}={3} or {2} IS NULL or {2}='')", DatabaseObjects.Columns.ModuleNameLookup, moduleName, DatabaseObjects.Columns.Deleted, false));

                DataTable dtmenu = new DataTable();
                if (dr != null && dr.Length > 0)
                {
                    DataTable tp = dr.CopyToDataTable();
                    DataTable dttemp = dr.CopyToDataTable();

                    if (!dttemp.Columns.Contains("SortRequestTypeCol"))
                        dttemp.Columns.Add("SortRequestTypeCol", typeof(int));
                    dttemp.Columns["SortRequestTypeCol"].Expression = string.Format("IIF([{0}] = '1', '1', '0')", DatabaseObjects.Columns.SortToBottom);

                    //dttemp.DefaultView.Sort = DatabaseObjects.Columns.Category + " ASC, " + DatabaseObjects.Columns.TicketRequestType + " ASC";
                    dttemp.DefaultView.Sort = DatabaseObjects.Columns.Category + " ASC, " + "SortRequestTypeCol" + " ASC," + DatabaseObjects.Columns.TicketRequestType + " ASC";
                    //dttemp.Columns.Add("ParentID", typeof(string));

                    var groupData = dttemp.AsEnumerable().GroupBy(x => x.Field<string>(DatabaseObjects.Columns.Category));
                    int counter = 1;
                    DataRow cRow = null;
                    foreach (var category in groupData)
                    {
                        cRow = dttemp.NewRow();
                        cRow[DatabaseObjects.Columns.Category] = category.Key;
                        cRow[DatabaseObjects.Columns.TicketRequestType] = "Category: " + category.Key;
                        cRow[DatabaseObjects.Columns.Id] = -counter;
                        cRow["ParentID"] = 0;

                        dttemp.Rows.Add(cRow);
                        int cID = -counter;
                        counter += 1;
                        var subCategories = category.GroupBy(x => x.Field<string>(DatabaseObjects.Columns.SubCategory));
                        foreach (var subCategory in subCategories)
                        {
                            int scID = 0;
                            cRow = dttemp.NewRow();
                            if (ObjConfigManager.GetValueAsBool(ConfigConstants.EnableRequestTypeSubCategory))
                            {
                                cRow[DatabaseObjects.Columns.Category] = category.Key;
                                if (!string.IsNullOrEmpty(subCategory.Key))
                                {
                                    cRow[DatabaseObjects.Columns.SubCategory] = subCategory.Key;
                                    cRow[DatabaseObjects.Columns.TicketRequestType] = "Sub Category: " + subCategory.Key;
                                    cRow[DatabaseObjects.Columns.Id] = -counter;
                                    cRow["ParentID"] = cID;
                                    dttemp.Rows.Add(cRow);
                                    scID = -counter;
                                    counter += 1;
                                }
                                else { scID = cID; }
                            }
                            else { scID = cID; }

                            foreach (var requestTypeR in subCategory.ToArray())
                            {
                                requestTypeR["ParentID"] = scID;
                            }
                        }
                    }
                    requestTypeData = dttemp;
                }
            }
            return requestTypeData;
        }

        protected void requestTypeTreeList_DataBound(object sender, EventArgs e)
        {
            if (flag && serviceQuest != null)
            {
                flag = false;
                KeyValuePair<string, string> param = serviceQuest.QuestionTypePropertiesDicObj.FirstOrDefault(x => x.Key.Trim().ToLower() == "requesttypes");
                if (param.Key != null && !string.IsNullOrWhiteSpace(param.Value))
                {
                    List<string> requesttypes = UGITUtility.ConvertStringToList(param.Value, Constants.Separator2);
                    if (requesttypes.Count == 1 && requesttypes[0].ToLower() == "all")
                    {
                        requestTypeTreeList.SelectAll();
                    }
                    else
                    {
                        foreach (string rt in requesttypes)
                        {
                            TreeListNode node = requestTypeTreeList.FindNodeByKeyValue(rt);
                            if (node != null)
                                node.Selected = true;
                        }
                    }
                }
            }
        }

        protected void rdbPickUserFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindApplications();
            SelectApplications();
            List<ServiceQuestion> userfield = service.Questions;
            ddlAccessMirrorFrom.Items.Clear();
            GetUserFieldWithinService(userfield, ddlAccessMirrorFrom);
            trMirrorAccessFrom.Visible = true;
            if (rdbPickUserFrom.SelectedIndex == 1)
            {
                trAutoCreateAccountTask.Visible = false;
                trUserFieldAppRequest.Visible = true;
                lbUserFieldAppRequestQuestions.Visible = false;
                ddlUserFieldAppRequestQuestions.SelectionMode = GridLookupSelectionMode.Single;
                GetUserFieldWithinService(userfield, ddlUserFieldAppRequestQuestions);
                trAccessRequestMode.Visible = true;
            }
            else
            {
                trUserFieldAppRequest.Visible = false;
                trUserFieldAppRequest.Visible = true;
                trAutoCreateAccountTask.Visible = true;
                trAccessRequestMode.Visible = false;
                lbUserFieldAppRequestQuestions.Visible = true;
                ddlUserFieldAppRequestQuestions.SelectionMode = GridLookupSelectionMode.Multiple;
                BindTextQuestions(userfield, ddlUserFieldAppRequestQuestions);
            }
        }
        private void BindChoiceFieldFromList(DropDownList ddl, string strListName)
        {
            ddl.Items.Clear();
            FieldConfigurationManager fieldManager = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
            List<FieldConfiguration> objField = fieldManager.Load(x => x.Datatype == "Choices" && x.TableName == strListName).ToList(); //fieldManager.Get(x => x.FieldName == strListName);
            if (objField != null && objField.Count > 0)
            {
                foreach (var item in objField)
                {
                    ddl.Items.Add(new ListItem(item.FieldName, item.FieldName));
                }                
            }
            
            ddl.Items.Insert(0, new ListItem("--Select Field-- ", ""));
            
            //SPList list = SPContext.Current.Web.Lists.TryGetList(strListName);
            //if (list != null)
            //{
            //    var fields = list.Fields.Cast<SPField>();
            //    //select all fields from list of the type Choice
            //    List<SPField> selectedFields = null;
            //    if (questionBasedOnNew.SelectedValue.ToLower() == "multichoice")
            //        selectedFields = fields.Where(x => x.Type == SPFieldType.MultiChoice).OrderBy(x => x.Title).ToList();
            //    else
            //        selectedFields = fields.Where(x => x.Type == SPFieldType.Choice).OrderBy(x => x.Title).ToList();

            //    foreach (var field in selectedFields)
            //    {
            //        ddl.Items.Add(new ListItem(field.Title, field.Title));
            //    }
            //    ddl.Items.Insert(0, new ListItem("--Select Field-- ", ""));

            //}
        }
        protected void DDLChooseFromList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChooseFromList.SelectedIndex > 0)
                BindChoiceFieldFromList(ChooseFromFields, ChooseFromList.SelectedValue);
        }

        protected void PickfromField_CheckedChanged(object sender, EventArgs e)
        {
            if (PickfromField.Checked)
            {
                trDropdownDefaultOptions.Visible = false;
                ChoseFromDropdown.Visible = true;
            }
            else
            {
                ChooseFromList.ClearSelection();
               // ChooseFromFields.ClearSelection();
                trDropdownDefaultOptions.Visible = true;
                ChoseFromDropdown.Visible = false;
            }
            FillChooseFromListsDropdown(ChooseFromList);
            //FillChooseFromListsDropdown(ChooseFromFields);
        }

        protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            trGrpPeoplePicker.Visible = false;
            trUserDefaultValue.Visible = true;
            
            divDefaultUser.Controls.Clear();   // clear already added control so that on type change of dropdown new values can be filled.
            addDefaultUserControl(UGITUtility.StringToInt(ddlUserType.SelectedIndex));
        }

        protected void chbxEnableAccessMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chbxEnableAccessMode.Checked)
                trAccessModeOptions.Visible = true;
            else
                trAccessModeOptions.Visible = false;
        }


        protected void chkbxPickFromFieldMC_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbxPickFromFieldMC.Checked)
            {
                trChooseFromListMC.Visible = true;
                trMultiChoiceValuesCaption.Visible = false;
                trMultiChoiceValues.Visible = false;
            }
            else
            {
                trChooseFromListMC.Visible = false;
                trMultiChoiceValuesCaption.Visible = true;
                trMultiChoiceValues.Visible = true;
                ddlChoosefromListMC.ClearSelection();
               // ddlChoosefromFieldMC.ClearSelection();
            }
            FillChooseFromListsDropdown(ddlChoosefromListMC);
        }

        protected void ddlChoosefromListMC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlChoosefromListMC.SelectedIndex > 0)
                BindChoiceFieldFromList(ddlChoosefromFieldMC, ddlChoosefromListMC.SelectedValue);
        }

        protected void chkDepartmentowner_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepartmentowner.Checked)
            {
                ddlUserField1.Visible = true;
            }
            else
            {
                ddlUserField1.Visible = false;
            }
        }

        protected void chkDepndLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDepndLocation.Checked)
            {
                ddlLocationQuestions.Visible = true;
            }
            else
            {
                ddlLocationQuestions.Visible = false;
            }
        }

        protected void chkUserManager_CheckedChanged(object sender, EventArgs e)
        {
            ddlUserManager.Visible = chkUserManager.Checked;
        }

        protected void chkUserDeskLocation_CheckedChanged(object sender, EventArgs e)
        {
            ddlUserFieldDeskLocation.Visible = chkUserDeskLocation.Checked;
        }

        private void BindTargetTypeCategories()
        {
            ddlTargetTypes.Items.Add(new ListItem("File", "File"));
            ddlTargetTypes.Items.Add(new ListItem("Link", "Link"));
            ddlTargetTypes.Items.Add(new ListItem("Wiki", "Wiki"));
            ddlTargetTypes.Items.Add(new ListItem("Help Card", "HelpCard"));
            ddlTargetTypes.DataBind();
        }


        protected void ddlTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTargetTypeDependency();
        }

        private void SetTargetTypeDependency()
        {
            //trLinks.Visible = false;
            //trFileUploads.Visible = false;
            //trWikis.Visible = false;
            //trHelpCards.Visible = false;
            //switch (ddlTargetTypes.SelectedValue)
            //{
            //    case "File":
            //        trFileUploads.Visible = true;
            //        break;
            //    case "Link":
            //        trLinks.Visible = true;
            //        break;
            //    case "Wiki":
            //        trWikis.Visible = true;
            //        break;
            //    case "HelpCard":
            //        trHelpCards.Visible = true;
            //        break;
            //    default:
            //        break;

            //}
        }
    }
}

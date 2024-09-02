using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;
using DevExpress.Web;
using System.Web;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class RequestTypeListEdit : UserControl
    {
        public int requestId { get; set; }
        public string module { get; set; }
        public List<string> RequestIds { get; set; }
        public bool IsMultiEdit { get; set; }
        public bool IsVaries { get; set; }
        public string Varie { get; set; }
        public int Id { get; set; }
        protected const string Varies = "<Value Varies>";
        string spanTag = "<span class='keyspan' data='{0}'><strong>{0}</strong><img onclick='javascript:removeTag(this);' class='spanimg' src='/content/images/cross_icn.png' alt='delete' > </span>";
        string ticketsenderspantag = "<span class='ticketsenderspan' data='{0}'><strong>{0}</strong><img onclick='javascript:removeTicketSenderTag(this);' class='ticketsenderspanimg' src='/content/images/cross_icn.png' alt='delete' > </span>";
        DataTable dtRequestType;
        //SPList spRequestTypeList;
        //SPListItemCollection spListitemCollection;
        DataRow[] spListitemCollection;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        bool use24x7OldValue;
        DataTable requestTypeData;
        ConfigurationVariableManager ConfigVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
            try
            {
                RequestTypeManager RequestTypeMgr = new RequestTypeManager(context);
                dtRequestType = RequestTypeMgr.GetDataTable();
                DataRow[] datarows = dtRequestType.Select(string.Format("{0}='{1}' And {2}=False", DatabaseObjects.Columns.ModuleNameLookup, module, DatabaseObjects.Columns.Deleted));
                if (datarows != null && datarows.Length > 0)
                {
                    requestTypeData = datarows.CopyToDataTable();
                }
                //if (!IsPostBack)
                //{
                //    // Full request type list used to bind categories, sub-categories, etc                   
                if (dtRequestType != null && dtRequestType.Rows.Count > 0)
                {
                    string category = string.Empty;
                    if (Request["category"] != null)
                        category = Uri.UnescapeDataString(Request["category"]);
                    string subCategory = string.Empty;
                    if (Request["subcategory"] != null)
                        subCategory = Uri.UnescapeDataString(Request["subcategory"]);

                    List<string> lst = null;
                    if (Request["ID"] != null)
                    {
                        RequestIds = Convert.ToString(Request["ID"]).Split(new string[] { "," }, StringSplitOptions.None).ToList();
                        //if (RequestIds != null && RequestIds.Count > 1)
                        //{
                        //    IsMultiEdit = true;
                        //    Session["IsMultiEdit"] = true;
                        //}
                        //else
                        //{
                        //    IsMultiEdit = false;
                        //    Session["IsMultiEdit"] = false;
                        //}
                    }
                    else if (!string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(subCategory))
                    {
                        lst = new List<string>();
                        DataTable allrequesttypeids = null;
                        DataRow[] datarow;
                        //SPQuery query = new SPQuery();
                        if (string.IsNullOrWhiteSpace(subCategory))
                        {
                            datarow = dtRequestType.Select(string.Format("{0}='{1}' And {2}='{3}' OR {4}={5}", category, DatabaseObjects.Columns.Category, DatabaseObjects.Columns.ModuleNameLookup, module, DatabaseObjects.Columns.SubCategory, subCategory));
                        }
                        else
                        {
                            string exp = string.Format("{0}='{1}' AND {2}='{3}' And {4}='{5}'", DatabaseObjects.Columns.Category, category, DatabaseObjects.Columns.ModuleNameLookup, module, DatabaseObjects.Columns.SubCategory, subCategory);
                            datarow = dtRequestType.Select(exp);
                        }
                        //allrequesttypeids = SPListHelper.GetDataTable(spRequestTypeList, query);
                        if (datarow.Length > 0)
                        {
                            allrequesttypeids = datarow.CopyToDataTable();

                        }
                        if (allrequesttypeids != null && allrequesttypeids.Rows.Count > 0)
                        {
                            foreach (DataRow row in allrequesttypeids.Rows)
                            {
                                lst.Add(Convert.ToString(row[DatabaseObjects.Columns.Id]));
                            }
                            RequestIds = lst;
                            lst = null;
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(category))
                    {
                        lst = new List<string>();
                        DataTable allrequesttypeids = null;
                        DataRow[] datarow;

                        //SPQuery query = new SPQuery();
                        if (string.IsNullOrWhiteSpace(category))
                        {
                            datarow = dtRequestType.Select(string.Format("{0}='{1}' OR {2}='{3}'", category, DatabaseObjects.Columns.Category, DatabaseObjects.Columns.ModuleNameLookup, module));
                        }
                        else
                        {
                            IsMultiEdit = true;
                            string exp = string.Format("{0}='{1}' AND {2}='{3}'", DatabaseObjects.Columns.Category, category, DatabaseObjects.Columns.ModuleNameLookup, module);
                            datarow = dtRequestType.Select(exp);
                        }
                        if (datarow.Length > 0)
                        {
                            allrequesttypeids = datarow.CopyToDataTable();

                        }
                        if (allrequesttypeids != null && allrequesttypeids.Rows.Count > 0)
                        {
                            foreach (DataRow row in allrequesttypeids.Rows)
                            {
                                lst.Add(Convert.ToString(row[DatabaseObjects.Columns.Id]));
                            }
                            RequestIds = lst;
                            lst = null;
                        }
                    }

                    if (RequestIds.Count == 1)
                        requestId = UGITUtility.StringToInt(RequestIds[0]);

                    if (RequestIds != null && RequestIds.Count >= 1 && !string.IsNullOrWhiteSpace(category))
                    {
                        IsMultiEdit = true;
                        trOutOfOffice.Visible = false;
                        trDeleted.Visible = false;
                        regextxtEstimatedHours.Enabled = false;
                        csvdvWorkflow.Enabled = false;
                        csvdivSubCategory.Enabled = false;
                        trSortToBottom.Visible = false;
                        LoadRequestTypeCollection();
                    }
                    else
                    {
                        IsMultiEdit = false;
                        trOutOfOffice.Visible = true;
                        trDeleted.Visible = true;
                        regextxtEstimatedHours.Enabled = true;
                        csvdivSubCategory.Enabled = false;
                        csvdvWorkflow.Enabled = true;
                    }

                    trCategory.Visible = true;

                    BindModule();
                    //if (moduleRow != null)
                    //    ddlModule.SelectedValue = moduleRow[DatabaseObjects.Columns.Id].ToString();
                    // ddlModule.SelectedValue = module;


                    //  ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByText(module));
                    ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(module));
                    BindSubCategory((ddlModule.SelectedValue));
                    BindCategoryFunctionArea();

                    BindTaskTemplate();

                    //if (IsMultiEdit)
                    //{
                    //    ddlRequestorContactSLAType.Items.Insert(0, new ListItem(Varies, "-1"));
                    //    ddlCloseSLAType.Items.Insert(0, new ListItem(Varies, "-1"));
                    //    ddlAssignmentSLAType.Items.Insert(0, new ListItem(Varies, "-1"));
                    //    ddlResolutionSLAType.Items.Insert(0, new ListItem(Varies, "-1"));
                    //    ddlTaskTemplate.Items.Insert(0, new ListItem(Varies, "-1"));
                    //}

                    if (requestId > 0)
                    {
                        DataTable dtRequestTypeData = RequestTypeMgr.GetDataTable($" id= {requestId}");
                        // Set Category  name 
                        ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.RequestCategory])));
                        // Set module  name 

                        // SPFieldLookupValue valueModule = new SPFieldLookupValue(Convert.ToString(spListitem[DatabaseObjects.Columns.ModuleNameLookup]));
                        //ddlModule.SelectedValue = Convert.ToString(valueModule.LookupId);///ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Convert.ToString(valueModule.LookupId)));
                        BindSubCategory((ddlModule.SelectedValue));
                        ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.ModuleNameLookup])));


                        //Set sub category
                        ddlSubCategory.SelectedIndex = ddlSubCategory.Items.IndexOf(ddlSubCategory.Items.FindByText(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.Category])));
                        BindSubCategoryAsPerCategory();
                        ddlsubcategoryaspercategory.SelectedIndex = ddlsubcategoryaspercategory.Items.IndexOf(ddlsubcategoryaspercategory.Items.FindByText(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.SubCategory])));


                        txtRequestType.Text = Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.TicketRequestType]);
                        lblRequestTypeLink.Text = Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.TicketRequestType]);
                        // Set budget item

                        //SPFieldLookupValue budgetItemValue = new SPFieldLookupValue(Convert.ToString(spListitem[DatabaseObjects.Columns.BudgetIdLookup]));
                        //ddlBudgetItem.SelectedIndex = ddlBudgetItem.Items.IndexOf(ddlBudgetItem.Items.FindByValue(Convert.ToString(budgetItemValue.LookupId)));
                        ddlBudgetItem.SetValues(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.BudgetIdLookup]));
                        //Set Estimated hour 
                        txtEstimatedHours.Text = Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.UGITEstimatedHours]);
                        // Set functional area

                        //SPFieldLookupValue functionalValue = new SPFieldLookupValue(Convert.ToString(spListitem[DatabaseObjects.Columns.FunctionalAreaLookup]));
                        //ddlFuncArea.SelectedIndex = ddlFuncArea.Items.IndexOf(ddlFuncArea.Items.FindByText(functionalValue.LookupValue));
                        ddlFuncArea.SetValues(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.FunctionalAreaLookup]));


                        // Set work flow type
                        ddlWorkflow.SelectedIndex = ddlWorkflow.Items.IndexOf(ddlWorkflow.Items.FindByValue(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.WorkflowType])));
                        txtDescription.Text = Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.RequestTypeDescription]);
                        chkDeleted.Checked = UGITUtility.StringToBoolean(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.Deleted]);

                        //Auto Assign PRP
                        chkAutoAssignPRP.Checked = UGITUtility.StringToBoolean(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.AutoAssignPRP]);
                        //added outofoffice 
                        chkOutOfOffice.Checked = UGITUtility.StringToBoolean(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.OutOfOffice]);

                        // set resolution type
                        txtResolutionType.Text = Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.ResolutionTypes]).Replace(Constants.Separator, Environment.NewLine);
                        txtIssueTypes.Text = Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.IssueTypeOptions]).Replace(Constants.Separator, Environment.NewLine);
                        chkSortToBottom.Checked = UGITUtility.StringToBoolean(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.SortToBottom]);

                        // For Owner With Multiuser            
                        if (dtRequestType.Columns.Contains(DatabaseObjects.Columns.Owner))
                            ppeOwner.SetValues(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.Owner]));


                        // For prp group sinlge user
                        ppePrpGroup.SetValues(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.PRPGroup]));
                        // For ORP               
                        ppeORP.SetValues(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.TicketORP]));
                        ppePRP.SetValues(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.TicketPRP]));
                        // For Escalation Manager               
                        ppeExecManeger.SetValues(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.RequestTypeEscalationManager]));

                        // For Back up Manager
                        ppeBackUpMan.SetValues(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.RequestTypeBackupEscalationManager]));

                        ///keywords filling with html.
                        string keyword = Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.KeyWords]);
                        hdnkw.Value = keyword;

                        string divInnerHTML = divShowKeywords.InnerHtml;
                        string completeInnerHTML = string.Empty;
                        foreach (var item in keyword.Split(';'))
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                string spanwithvalue = string.Format(spanTag, item);
                                completeInnerHTML += spanwithvalue;
                            }
                        }
                        completeInnerHTML += divInnerHTML;
                        divShowKeywords.InnerHtml = completeInnerHTML;

                        string ticketsender = Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.EmailToTicketSender]);
                        hdnticketsender.Value = ticketsender;
                        divInnerHTML = dvticketsender.InnerHtml;
                        completeInnerHTML = string.Empty;
                        foreach (var item in ticketsender.Split(';'))
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                string spanwithvalue = string.Format(ticketsenderspantag, item);
                                completeInnerHTML += spanwithvalue;
                            }
                        }
                        completeInnerHTML += divInnerHTML;
                        dvticketsender.InnerHtml = completeInnerHTML;

                        if (!string.IsNullOrEmpty(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.AppReferenceInfo])))
                        {
                            lblRequestType.Text = "Application (Request Type)";
                            lblApplicationId.Visible = true;
                            lblApplicationId.Text = string.Format("({0})", Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.AppReferenceInfo]));
                            //issue type
                            lblissuetypeoption.Visible = true;

                            txtIssueTypes.Visible = false;

                            lblissuetypeoption.Text = string.Join("<br/>", UGITUtility.SplitString(Convert.ToString(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.IssueTypeOptions]), new string[] { Constants.Separator, Constants.NewLineSeperator }, StringSplitOptions.RemoveEmptyEntries));

                            //SPListItem spListitemTicket = Ticket.getCurrentTicket("APP", Convert.ToString(spListitem[DatabaseObjects.Columns.AppReferenceInfo]));
                            //if (spListitemTicket != null &&  UGITUtility.StringToBoolean(spListitemTicket[DatabaseObjects.Columns.SyncAtModuleLevel]))
                            //{
                            //    trCategory.Visible = false;
                            //    lblRequestType.Text = "Application Module (Request Type)";
                            //    lblRequestTypeLink.Text = string.Format("Application {0} - Module {1}", spListitemTicket[DatabaseObjects.Columns.Title], Convert.ToString(spListitem[DatabaseObjects.Columns.TicketRequestType]));
                            //    if (ddlFuncArea != null && !string.IsNullOrEmpty(ddlFuncArea.SelectedItem.Text))
                            //    {
                            //        lblfunctionalareaText.Text = ddlFuncArea.SelectedItem.Text;
                            //        lblfunctionalareaText.Visible = true;
                            //        ddlFuncArea.Visible = false;
                            //    }
                            //    else
                            //    {
                            //        lblfunctionalareaText.Visible = false;
                            //        lblfunctionalareaText.Visible = false;
                            //        ddlFuncArea.Visible = true;
                            //    }
                            //    bool enableAppModuleRoles =  UGITUtility.GetConfigVariableValueAsBool(ConfigConstants.EnableAppModuleRoles);
                            //    if (!enableAppModuleRoles)
                            //    {
                            //        if (ownerColl != null)
                            //            lblOwner.Text = string.Join(",", ownerColl.Select(x => x.LookupValue).ToArray());
                            //        lblOwner.Visible = true;
                            //        ppeOwner.Visible = false;

                            //        // show PRP in readonly mode when enableappmodules is false
                            //        // lblPRPGroup.Text = arrPRPGroup.LookupValue;
                            //        lblPRPGroup.Visible = true;
                            //        ppePrpGroup.Visible = false;
                            //    }
                            //    else
                            //    {
                            //        lblOwner.Visible = false;
                            //        ppeOwner.Visible = true;

                            //        lblPRPGroup.Visible = false;
                            //        ppePrpGroup.Visible = true;
                            //    }
                            //}

                            txtRequestType.Visible = false;
                            lblRequestTypeLink.Visible = true;

                            //string viewUrl = Ticket.GenerateTicketURL("APP", Convert.ToString(spListitem[DatabaseObjects.Columns.AppReferenceInfo]), SPContext.Current.Web);
                            //var sourceUrl = Request["source"] != null ? Request["source"] : Server.UrlEncode(Request.Url.AbsolutePath);
                            //if (!string.IsNullOrEmpty(viewUrl))
                            //{
                            //    string func = string.Format("javascript:openTicketDialog('{0}','','{1}','90','90', 0, '{2}')", viewUrl, spListitemTicket[DatabaseObjects.Columns.Title], sourceUrl);
                            //    lblApplicationId.Attributes.Add("onclick", func);
                            //    lblRequestTypeLink.Attributes.Add("onclick", func);
                            //}
                        }
                        else
                        {
                            lblRequestType.Text = "Request Type";
                            lblApplicationId.Visible = false;
                        }



                        //SPFieldLookupValue applicationlookup = new SPFieldLookupValue(Convert.ToString(spListitem[DatabaseObjects.Columns.APPTitleLookup]));
                        // if (applicationlookup != null && applicationlookup.LookupId > 0)
                        // {
                        //     hdnApplicationId.Value = Convert.ToString(applicationlookup.LookupId);
                        // }
                        // else
                        // {
                        //     hdnApplicationId.Value = string.Empty;
                        // }
                        //// SPFieldLookupValue applicationModulelookup = new SPFieldLookupValue(Convert.ToString(spListitem[DatabaseObjects.Columns.ApplicationModulesLookup]));
                        // if (applicationModulelookup != null && applicationModulelookup.LookupId > 0)
                        // {
                        //     hdnAppModuleId.Value = Convert.ToString(applicationModulelookup.LookupId);
                        // }
                        // else
                        // {
                        //     hdnAppModuleId.Value = string.Empty;
                        // }

                        chkUse24x7Calendar.Checked = use24x7OldValue = UGITUtility.StringToBoolean(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.Use24x7Calendar]);
                        FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.RequestorContactSLA]), txtRequestorContactSLA, ddlRequestorContactSLAType, chkUse24x7Calendar.Checked);
                        FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.AssignmentSLA]), txtAssignmentSLA, ddlAssignmentSLAType, chkUse24x7Calendar.Checked);
                        FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.ResolutionSLA]), txtResolutionSLA, ddlResolutionSLAType, chkUse24x7Calendar.Checked);
                        FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.CloseSLA]), txtCloseSLA, ddlCloseSLAType, chkUse24x7Calendar.Checked);

                        //if (uHelper.IsSPItemExist(spListitem, DatabaseObjects.Columns.TaskTemplateLookup))
                        //{
                        //    SPFieldLookupValue taskTemplate = new SPFieldLookupValue(Convert.ToString(spListitem[DatabaseObjects.Columns.TaskTemplateLookup]));
                        //    ddlTaskTemplate.SelectedValue = Convert.ToString(taskTemplate.LookupId);
                        //}
                        chkDisableSLA.Checked = UGITUtility.StringToBoolean(dtRequestTypeData.Rows[0][DatabaseObjects.Columns.SLADisabled]);
                        if (chkDisableSLA.Checked)
                        {
                            trSLARequestor.Style.Add("display", "none");
                            trSLAAssignment.Style.Add("display", "none");
                            trSLAResolution.Style.Add("display", "none");
                            trSLAClose.Style.Add("display", "none");
                        }
                    }
                    else if (IsMultiEdit)
                    {
                        if (!IsPostBack)
                        {

                            FillMultiEditView();
                            IsMultiEdit = true;
                        }
                    }
                    else
                    {
                        txtRequestType.Visible = true;
                        lblRequestTypeLink.Visible = false;
                        ddlWorkflow.SelectedIndex = ddlWorkflow.Items.IndexOf(ddlWorkflow.Items.FindByValue("Full"));  // Default to "Full" workflow type
                    }

                    ///Location Wise bind data.

                    base.OnInit(e);
                }
                // }
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            //BindGvLocation();
        }

        private void BindSubCategoryAsPerCategory()
        {
            ddlsubcategoryaspercategory.Items.Clear();

            if (requestTypeData != null)
            {
                DataRow[] drows = new DataRow[0];
                if (ddlSubCategory.SelectedIndex > 0)
                {
                    drows = requestTypeData.Select(string.Format("{0}='{1}' AND {2}='{3}'",
                                                                DatabaseObjects.Columns.ModuleNameLookup, module,
                                                                DatabaseObjects.Columns.Category, ddlSubCategory.SelectedValue));
                }
                else
                {
                    drows = requestTypeData.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, module));
                }

                if (drows != null && drows.Length > 0)
                {
                    // Bind Subcategory
                    DataTable dtSubCategory = drows.CopyToDataTable().DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.SubCategory, DatabaseObjects.Columns.ModuleNameLookup });

                    //DataTable dttemp = drows.CopyToDataTable();
                    dtSubCategory.DefaultView.Sort = DatabaseObjects.Columns.SubCategory + " ASC";
                    dtSubCategory = dtSubCategory.DefaultView.ToTable();
                    foreach (DataRow item in dtSubCategory.Rows)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(item[DatabaseObjects.Columns.SubCategory])))
                            ddlsubcategoryaspercategory.Items.Add(new ListItem(Convert.ToString(item[DatabaseObjects.Columns.SubCategory])));
                    }
                }
            }
            ddlsubcategoryaspercategory.Items.Insert(0, new ListItem("None", "0"));
            if (IsMultiEdit)
            {
                ddlsubcategoryaspercategory.Items.Insert(0, new ListItem(Varies, "-1"));
            }
        }

        protected override void OnPreRender(EventArgs e)
        {

            if (ddlCategory.SelectedItem != null && (ddlCategory.SelectedItem.Text == "None" || ddlCategory.SelectedItem.Text == Varies))
                btRMMCategoryEdit.Style.Add(HtmlTextWriterStyle.Display, "none");

            if (ddlSubCategory.SelectedItem != null && (ddlSubCategory.SelectedItem.Text == "None" || ddlSubCategory.SelectedItem.Text == Varies))
                btCategoryEdit.Style.Add(HtmlTextWriterStyle.Display, "none");

            if (ddlsubcategoryaspercategory.SelectedItem != null && (ddlsubcategoryaspercategory.SelectedItem.Text == "None" || ddlsubcategoryaspercategory.SelectedItem.Text == Varies))
                btSubCategoryEdit.Style.Add(HtmlTextWriterStyle.Display, "none");

            base.OnPreRender(e);
        }

        private void FillSLADayHourAndMinutesFormat(double minutes, TextBox textcontrol, DropDownList dropdown, bool use24x7Calendar)
        {
            int workingHoursInADay = 0;
            if (!use24x7Calendar)
                workingHoursInADay = uHelper.GetWorkingHoursInADay(HttpContext.Current.GetManagerContext(), true);
            else
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

        protected void LoadRequestTypeCollection()
        {
            if (spListitemCollection == null && RequestIds.Count > 0)
            {
                spListitemCollection = dtRequestType.Select(string.Format("{0} in ({1})", DatabaseObjects.Columns.ID, string.Join(",", RequestIds.Select(x => "'" + x + "'"))));

            }
        }

        protected void FillMultiEditView()
        {
            if (spListitemCollection.Length > 0)
            {
                DataTable dtItems = spListitemCollection.CopyToDataTable();
                DataView view = new DataView(dtItems);

                // Set Category  name 
                DataTable distinctValues = view.ToTable(true, DatabaseObjects.Columns.RequestCategory);
                if (distinctValues.Rows.Count == 1)
                {
                    ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.RequestCategory])));
                }
                else
                {
                    ddlCategory.SelectedValue = "-1";
                }

                // Set module  name 
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.ModuleNameLookup);
                if (distinctValues.Rows.Count == 1)
                {
                    ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.ModuleNameLookup])));
                    //SPFieldLookupValue valueModule = new SPFieldLookupValue(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.ModuleNameLookup]));
                    //ddlModule.SelectedValue = Convert.ToString(valueModule.LookupId);
                    BindSubCategory((ddlModule.SelectedValue));
                }
                else
                {
                    ddlModule.SelectedValue = "-1";
                    BindSubCategory((ddlModule.SelectedValue));
                }

                distinctValues = view.ToTable(true, DatabaseObjects.Columns.TaskTemplateLookup);
                if (distinctValues.Rows.Count == 2)
                {
                    ddlTaskTemplate.SelectedIndex = ddlTaskTemplate.Items.IndexOf(ddlTaskTemplate.Items.FindByValue(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.TaskTemplateLookup])));
                    //SPFieldLookupValue taskTemplate = new SPFieldLookupValue(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.TaskTemplateLookup]));
                    //ddlTaskTemplate.SelectedValue = Convert.ToString(taskTemplate.LookupId);
                }
                else
                {
                    ddlTaskTemplate.SelectedValue = "-1";
                }

                //Set sub category
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.Category);
                if (distinctValues.Rows.Count == 1)
                {
                    ddlSubCategory.SelectedIndex = ddlSubCategory.Items.IndexOf(ddlSubCategory.Items.FindByText(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.Category])));
                }
                else
                {
                    ddlSubCategory.SelectedValue = "-1";
                }

                BindSubCategoryAsPerCategory();
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.SubCategory);
                if (distinctValues.Rows.Count == 1)
                {
                    ddlsubcategoryaspercategory.SelectedIndex = ddlsubcategoryaspercategory.Items.IndexOf(ddlsubcategoryaspercategory.Items.FindByText(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.SubCategory])));
                }
                else
                {
                    ddlsubcategoryaspercategory.SelectedValue = "-1";
                }

                distinctValues = view.ToTable(true, DatabaseObjects.Columns.TicketRequestType);
                if (distinctValues.Rows.Count == 1)
                {
                    txtRequestType.Text = Convert.ToString(dtItems.Rows[0][DatabaseObjects.Columns.TicketRequestType]);
                    lblRequestTypeLink.Text = Convert.ToString(dtItems.Rows[0][DatabaseObjects.Columns.TicketRequestType]);
                }
                else
                {
                    txtRequestType.Text = Varies;
                    lblApplicationId.Text = Server.HtmlEncode(Varies);
                }

                // Set budget item
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.BudgetIdLookup);
                if (distinctValues.Rows.Count == 1)
                {
                    ddlBudgetItem.SetValues(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.BudgetIdLookup]));
                }
                else
                {
                    ddlBudgetItem.AllowVaries = true;
                    ddlBudgetItem.SetText(Varies);
                }

                //Set Estimated hour 
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.UGITEstimatedHours);
                if (distinctValues.Rows.Count == 1)
                {
                    txtEstimatedHours.Text = Convert.ToString(dtItems.Rows[0][DatabaseObjects.Columns.UGITEstimatedHours]);
                }
                else
                {
                    //txtEstimatedHours.Text = Varies;
                    txtEstimatedHours.Text = "0";
                }

                // Set functional area
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.FunctionalAreaLookup);
                if (distinctValues.Rows.Count == 1)
                {
                    ddlFuncArea.SetValues(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.FunctionalAreaLookup]));
                }
                else
                {
                    //ddlFuncArea.SetValues(Varies);
                    ddlFuncArea.AllowVaries = true;
                    ddlFuncArea.SetText(Varies);
                }


                // Set work flow type
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.WorkflowType);
                if (distinctValues.Rows.Count == 1)
                {
                    ddlWorkflow.SelectedIndex = ddlWorkflow.Items.IndexOf(ddlWorkflow.Items.FindByValue(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.WorkflowType])));
                }
                else
                {
                    ddlWorkflow.SelectedValue = "-1";
                }

                distinctValues = view.ToTable(true, DatabaseObjects.Columns.RequestTypeDescription);
                if (distinctValues.Rows.Count == 1)
                {
                    txtDescription.Text = Convert.ToString(dtItems.Rows[0][DatabaseObjects.Columns.RequestTypeDescription]);
                }
                else
                {
                    txtDescription.Text = Varies;
                }
                List<string> ownerList = new List<string>();

                // For Owner With Multiuser 
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.Owner);
                if (distinctValues.Rows.Count == 1 && !IsMultiEdit)
                {
                    ppeOwner.SetValues(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.Owner]));
                }
                else
                {
                    lblOwner.Text = Server.HtmlEncode(Varies);
                    hndOwner.Value = Varies;
                    ppeOwner.Style.Add("Display", "none");
                    dvOwner.Style.Remove("Display");
                }

                // For prp group sinlge user
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.PRPGroup);
                if (distinctValues.Rows.Count == 1)
                {
                    ppePrpGroup.SetValues(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.PRPGroup]));
                }
                else
                {
                    lblPRPGroup.Text = Server.HtmlEncode(Varies);
                    hndPRPGroup.Value = Varies;
                    ppePrpGroup.Style.Add("Display", "none");
                    dvPRPGroup.Style.Remove("Display");
                }

                // For ORP  
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.TicketORP);
                if (distinctValues.Rows.Count == 1)
                {
                    ppeORP.SetValues(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.TicketORP]));
                }
                else
                {
                    lblORP.Text = Server.HtmlEncode(Varies);
                    hndORP.Value = Varies;
                    ppeORP.Style.Add("Display", "none");
                    dvORP.Style.Remove("Display");
                }

                // For Ecscalation Manager    
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.RequestTypeEscalationManager);
                if (distinctValues.Rows.Count == 1)
                {
                    ppeExecManeger.SetValues(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.RequestTypeEscalationManager]));
                }
                else
                {
                    lblExecManeger.Text = Server.HtmlEncode(Varies);
                    hndExecManeger.Value = Varies;
                    ppeExecManeger.Style.Add("Display", "none");
                    dvExecManeger.Style.Remove("Display");
                }


                // For Back up Manager
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.RequestTypeEscalationManager);
                if (distinctValues.Rows.Count == 1)
                {
                    ppeBackUpMan.SetValues(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.RequestTypeBackupEscalationManager]));
                }
                else
                {
                    lblBackUpMan.Text = Server.HtmlEncode(Varies);
                    hndBackUpMan.Value = Varies;
                    ppeBackUpMan.Style.Add("Display", "none");
                    dvBackUpMan.Style.Remove("Display");
                }

                ///keywords filling with html.
                ///
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.KeyWords);
                string keyword = Varies;
                if (distinctValues.Rows.Count == 1)
                {
                    keyword = Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.KeyWords]);
                    hdnkw.Value = keyword;
                }

                string divInnerHTML = divShowKeywords.InnerHtml;
                string completeInnerHTML = string.Empty;
                foreach (var item in keyword.Split(';'))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string spanwithvalue = string.Format(spanTag, item == Varies ? Server.HtmlEncode(item) : item);
                        completeInnerHTML += spanwithvalue;
                    }
                }
                completeInnerHTML += divInnerHTML;
                divShowKeywords.InnerHtml = completeInnerHTML;


                distinctValues = view.ToTable(true, DatabaseObjects.Columns.EmailToTicketSender);
                string ticketsender = Varies;
                if (distinctValues.Rows.Count == 1)
                {
                    ticketsender = Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.EmailToTicketSender]);
                    hdnticketsender.Value = ticketsender;
                }

                divInnerHTML = dvticketsender.InnerHtml;
                completeInnerHTML = string.Empty;
                foreach (var item in ticketsender.Split(';'))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string spanwithvalue = string.Format(ticketsenderspantag, item == Varies ? Server.HtmlEncode(item) : item);
                        completeInnerHTML += spanwithvalue;
                    }
                }
                completeInnerHTML += divInnerHTML;
                dvticketsender.InnerHtml = completeInnerHTML;

                distinctValues = view.ToTable(true, DatabaseObjects.Columns.AppReferenceInfo);
                if (distinctValues.Rows.Count == 1 && !string.IsNullOrEmpty(Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.AppReferenceInfo])))
                {

                    trCategory.Visible = false;
                    lblRequestType.Text = "Application Module (Request Type)";
                    lblRequestTypeLink.Text = string.Format("Application {0} - Module {1}", spListitemCollection[0][DatabaseObjects.Columns.Title], Convert.ToString(spListitemCollection[0][DatabaseObjects.Columns.TicketRequestType]));
                    bool enableAppModuleRoles = ConfigVariableHelper.GetValueAsBool(ConfigConstants.EnableAppModuleRoles);
                    if (!enableAppModuleRoles)
                    {
                        lblOwner.Text = string.Join(",", ownerList.ToArray());
                        lblOwner.Visible = true;
                        ppeOwner.Visible = false;

                        // show PRP in readonly mode when enableappmodules is false
                        // lblPRPGroup.Text = arrPRPGroup.LookupValue;
                        lblPRPGroup.Visible = true;
                        ppePrpGroup.Visible = false;
                    }
                    else
                    {
                        lblOwner.Visible = false;
                        ppeOwner.Visible = true;

                        lblPRPGroup.Visible = false;
                        ppePrpGroup.Visible = true;
                    }

                    lblApplicationId.Visible = false;
                    lblRequestTypeLink.Visible = true;
                }
                else
                {
                    lblRequestType.Text = "Request Type";
                    lblApplicationId.Visible = true;
                }
                txtRequestType.Visible = false;

                //For Use24x7Calendar
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.Use24x7Calendar);
                if (distinctValues.Rows.Count == 1)
                {
                    chkUse24x7Calendar.Checked = use24x7OldValue = UGITUtility.StringToBoolean(spListitemCollection[0][DatabaseObjects.Columns.Use24x7Calendar]);
                }
                else
                {
                    //lblUse24x7Calendar.Text = Server.HtmlEncode(Varies);
                    hdnUse24x7Calendar.Value = Varies;
                    trUse24x7Calendar.Style.Add("Display", "none");
                    //dvUse24x7Calendar.Style.Remove("Display");
                }

                ///Requestor Contact SLA 
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.RequestorContactSLA);
                if (distinctValues.Rows.Count == 1)
                {
                    double sla = 0;
                    double.TryParse(Convert.ToString(dtItems.Rows[0][DatabaseObjects.Columns.RequestorContactSLA]), out sla);
                    FillSLADayHourAndMinutesFormat(sla, txtRequestorContactSLA, ddlRequestorContactSLAType, chkUse24x7Calendar.Checked);
                }
                else
                {
                    //txtRequestorContactSLA.Text = Varies;
                    txtRequestorContactSLA.Text = "0";
                    ddlRequestorContactSLAType.SelectedValue = "-1";
                }

                ///Assignment SLA 
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.AssignmentSLA);
                if (distinctValues.Rows.Count == 1)
                {
                    double sla = 0;
                    double.TryParse(Convert.ToString(dtItems.Rows[0][DatabaseObjects.Columns.AssignmentSLA]), out sla);
                    FillSLADayHourAndMinutesFormat(sla, txtAssignmentSLA, ddlAssignmentSLAType, chkUse24x7Calendar.Checked);
                }
                else
                {
                    txtAssignmentSLA.Text = "0";
                    //txtAssignmentSLA.Text = Varies;
                    ddlAssignmentSLAType.SelectedValue = "-1";
                }

                ///Resolution SLA
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.ResolutionSLA);
                if (distinctValues.Rows.Count == 1)
                {
                    double sla = 0;
                    double.TryParse(Convert.ToString(dtItems.Rows[0][DatabaseObjects.Columns.ResolutionSLA]), out sla);
                    FillSLADayHourAndMinutesFormat(sla, txtResolutionSLA, ddlResolutionSLAType, chkUse24x7Calendar.Checked);
                }
                else
                {
                    //txtResolutionSLA.Text = Varies;
                    txtResolutionSLA.Text = "0";
                    ddlResolutionSLAType.SelectedValue = "-1";
                }

                ///Close SLA
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.CloseSLA);
                if (distinctValues.Rows.Count == 1)
                {
                    double sla = 0;
                    double.TryParse(Convert.ToString(dtItems.Rows[0][DatabaseObjects.Columns.CloseSLA]), out sla);
                    FillSLADayHourAndMinutesFormat(sla, txtCloseSLA, ddlCloseSLAType, chkUse24x7Calendar.Checked);
                }
                else
                {
                    //txtCloseSLA.Text = Varies;
                    txtCloseSLA.Text = "0";
                    ddlCloseSLAType.SelectedValue = "-1"; ;
                }


                //resolution type
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.ResolutionTypes);
                if (distinctValues.Rows.Count > 1)
                {
                    txtResolutionType.Text = Varies;
                }

                //issue type
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.IssueTypeOptions);
                if (distinctValues.Rows.Count > 1)
                {
                    txtIssueTypes.Text = Varies;
                }
                //For AutoAssign PRP
                distinctValues = view.ToTable(true, DatabaseObjects.Columns.SLADisabled);
                if (distinctValues.Rows.Count == 1)
                {
                    chkDisableSLA.Checked = UGITUtility.StringToBoolean(dtItems.Rows[0][DatabaseObjects.Columns.SLADisabled]);
                    if (chkDisableSLA.Checked)
                    {
                        trSLARequestor.Style.Add("display", "none");
                        trSLAAssignment.Style.Add("display", "none");
                        trSLAResolution.Style.Add("display", "none");
                        trSLAClose.Style.Add("display", "none");
                    }
                }
                else
                {
                    lblDisableSLA.Text = Server.HtmlEncode(Varies);
                    hdnDisableSLA.Value = Varies;
                    dvChkDisableSLA.Style.Add("Display", "none");
                    dvDisableSLA.Style.Remove("Display");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindGvLocation();
        }

        /// <summary>
        /// Bind Module Dropdown
        /// </summary>
        private void BindModule()
        {
            ModuleViewManager ModuleView = new ModuleViewManager(context);
            DataTable dtModule = ModuleView.GetDataTable();
            if (dtModule != null && dtModule.Rows.Count > 0)
            {
                DataRow[] moduleRows = dtModule.Select(string.Format("{0}=True", DatabaseObjects.Columns.EnableModule));
                dtModule = moduleRows.CopyToDataTable();
                dtModule.DefaultView.Sort = DatabaseObjects.Columns.ModuleName + " ASC";
                ddlModule.DataSource = dtModule;
                ddlModule.DataTextField = DatabaseObjects.Columns.Title;
                ddlModule.DataValueField = DatabaseObjects.Columns.ModuleName;
                ddlModule.DataBind();
                ddlModule.Items.Insert(0, new ListItem("None", "0"));
            }


            if (IsMultiEdit)
            {
                ddlModule.Items.Insert(0, new ListItem(Varies, "-1"));
            }
        }

        /// <summary>
        /// Bind Category and Subcategory
        /// </summary>
        private void BindCategoryFunctionArea()
        {
            if (requestTypeData != null && requestTypeData.Rows.Count > 0)
            {
                //Bind Category
                DataTable dtCategory = requestTypeData.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.RequestCategory });
                dtCategory.DefaultView.Sort = DatabaseObjects.Columns.RequestCategory + " ASC";
                foreach (DataRow dr in dtCategory.Rows)
                {
                    string requestCategory = Convert.ToString(dr[DatabaseObjects.Columns.RequestCategory]);
                    if (!string.IsNullOrEmpty(requestCategory.Trim()) && ddlCategory.Items.FindByText(requestCategory) == null)
                    {
                        ddlCategory.Items.Add(new ListItem(requestCategory));
                    }
                }
                ddlCategory.Items.Insert(0, new ListItem("None", "0"));
                if (IsMultiEdit)
                {
                    ddlCategory.Items.Insert(0, new ListItem(Varies, "-1"));
                }
            }
            // Bind Workflow Types
            ddlWorkflow.Items.Insert(0, new ListItem("None", "0"));
            if (IsMultiEdit)
            {
                ddlWorkflow.Items.Insert(0, new ListItem(Varies, "-1"));
            }
            if (requestTypeData != null && requestTypeData.Rows.Count > 0)
            {
                DataTable dtWorkflowType = requestTypeData.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.WorkflowType });
                foreach (DataRow dr in dtWorkflowType.Rows)
                {
                    string workflowType = Convert.ToString(dr[DatabaseObjects.Columns.WorkflowType]);
                    if (!string.IsNullOrEmpty(workflowType.Trim()) && ddlWorkflow.Items.FindByText(workflowType) == null)
                    {
                        ddlWorkflow.Items.Add(new ListItem(workflowType));
                    }
                }
            }

            //Bind Budget Item
            //SPList spBudgetCategoriesList = SPListHelper.GetSPList(DatabaseObjects.Lists.BudgetCategories);
            BudgetCategoryViewManager BudgetCategoriesMgr = new BudgetCategoryViewManager(context);
            DataTable dtBudgetCategory = BudgetCategoriesMgr.GetDataTable();
            if (dtBudgetCategory != null && dtBudgetCategory.Rows.Count > 0)
            {
            }
        }

        private void BindTaskTemplate()
        {

            TaskTemplateManager taskTempMgr = new TaskTemplateManager(context);
            DataTable dtTaskTemplate = taskTempMgr.GetDataTable();
            dtTaskTemplate.DefaultView.Sort = DatabaseObjects.Columns.Title + " ASC";
            foreach (DataRow dr in dtTaskTemplate.Rows)
            {
                ddlTaskTemplate.Items.Add(new ListItem(Convert.ToString(dr[DatabaseObjects.Columns.Title]), Convert.ToString(dr[DatabaseObjects.Columns.Id])));
            }

            ddlTaskTemplate.Items.Insert(0, new ListItem("None", "0"));
            //if (IsMultiEdit)
            //{
            //    ddlTaskTemplate.Items.Insert(0, new ListItem(Varies, "-1"));
            //}
        }
        void BindSubCategory(string ModuleName)
        {
            ddlSubCategory.Items.Clear();
            if (requestTypeData != null)
            {
                // Bind Subcategory
                DataTable dtSubCategory = requestTypeData.DefaultView.ToTable(true, new string[] { DatabaseObjects.Columns.Category, DatabaseObjects.Columns.ModuleNameLookup });
                DataRow[] drows = dtSubCategory.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.ModuleNameLookup, module));
                if (drows.Length > 0)
                {
                    DataTable dttemp = drows.CopyToDataTable();
                    dttemp.DefaultView.Sort = DatabaseObjects.Columns.Category + " ASC";
                    dttemp = dttemp.DefaultView.ToTable();
                    foreach (DataRow item in dttemp.Rows)
                    {
                        ddlSubCategory.Items.Add(new ListItem(Convert.ToString(item[DatabaseObjects.Columns.Category])));
                    }

                }
                if (Request["category"] != null)
                { ddlSubCategory.SelectedIndex = ddlSubCategory.Items.IndexOf(ddlSubCategory.Items.FindByText(Request["category"].ToString())); }
            }
            //ddlSubCategory.Items.Insert(0, new ListItem("None", "0"));
            //if (IsMultiEdit)
            //{
            //    ddlSubCategory.Items.Insert(0, new ListItem(Varies, "-1"));
            //}

        }
        private void SaveMultipleRequestTypes()
        {

            if (spListitemCollection != null)
            {
                DataTable dtMultiUpdate = spListitemCollection.CopyToDataTable();
                foreach (DataRow dr in dtMultiUpdate.Rows)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    if (ddlModule.SelectedValue != "-1")
                    {
                        if (ddlModule.SelectedIndex > 1)
                        {
                            values.Add(DatabaseObjects.Columns.ModuleNameLookup, ddlModule.SelectedValue);

                        }
                        else
                        {
                            //lblModule.Text = "Enter Module Name";
                            break;
                        }
                    }

                    if (ddlTaskTemplate.SelectedValue != "-1")
                    {
                        if (ddlTaskTemplate.SelectedIndex > 1)
                        {

                            values.Add(DatabaseObjects.Columns.TaskTemplateLookup, ddlTaskTemplate.SelectedValue);
                        }
                    }

                    // To check category Value will be pick from Dropdown or TextBox
                    if (hdnCategory.Value == "1" && !string.IsNullOrWhiteSpace(txtCategory.Text))
                    {

                        values.Add(DatabaseObjects.Columns.RequestCategory, txtCategory.Text.Trim());
                    }
                    else
                    {
                        if (ddlCategory.SelectedValue != "-1")
                        {
                            dr[DatabaseObjects.Columns.RequestCategory] = null;
                            if (ddlCategory.SelectedIndex > 1)
                            {

                                values.Add(DatabaseObjects.Columns.RequestCategory, ddlCategory.SelectedItem.Text);
                            }
                        }
                    }

                    if (hdnSubCategory.Value == "1" && !string.IsNullOrWhiteSpace(txtSubCategory.Text))
                    {

                        values.Add(DatabaseObjects.Columns.Category, txtSubCategory.Text.Trim());
                    }
                    else
                    {
                        if (ddlSubCategory.SelectedValue != "-1")
                        {
                            dr[DatabaseObjects.Columns.Category] = null;
                            //if (ddlSubCategory.SelectedIndex > 1)
                            //{
                            values.Add(DatabaseObjects.Columns.Category, ddlSubCategory.SelectedItem.Text);
                            //}
                        }
                    }


                    if (hdnsubcategoryaspercategory.Value == "1" && !string.IsNullOrWhiteSpace(txtSubcategoryaspercategory.Text))
                    {

                        values.Add(DatabaseObjects.Columns.SubCategory, txtSubcategoryaspercategory.Text.Trim());
                    }
                    else
                    {
                        if (ddlsubcategoryaspercategory.SelectedValue != "-1")
                        {
                            dr[DatabaseObjects.Columns.SubCategory] = null;
                            if (ddlsubcategoryaspercategory.SelectedIndex > 1)
                            {

                                values.Add(DatabaseObjects.Columns.SubCategory, ddlsubcategoryaspercategory.SelectedItem.Text);
                            }
                        }
                    }

                    ///Estimated Hours
                    if (txtEstimatedHours.Text.Trim() != Varies)
                    {
                        double estimatedHrs = UGITUtility.StringToDouble(txtEstimatedHours.Text.Trim());

                        values.Add(DatabaseObjects.Columns.UGITEstimatedHours, Math.Round(estimatedHrs * 4, MidpointRounding.ToEven) / 4);
                    }

                    ///Requestor Contact SLA
                    if (txtRequestorContactSLA.Text.Trim() != Varies && ddlRequestorContactSLAType.SelectedValue != "-1")

                        values.Add(DatabaseObjects.Columns.RequestorContactSLA, GetSLAValue(txtRequestorContactSLA, ddlRequestorContactSLAType, chkUse24x7Calendar.Checked));

                    ///Assignment SLA
                    if (txtAssignmentSLA.Text.Trim() != Varies && ddlAssignmentSLAType.SelectedValue != "-1")

                        values.Add(DatabaseObjects.Columns.AssignmentSLA, GetSLAValue(txtAssignmentSLA, ddlAssignmentSLAType, chkUse24x7Calendar.Checked));

                    ///ResolutionSLA
                    if (txtResolutionSLA.Text.Trim() != Varies && ddlResolutionSLAType.SelectedValue != "-1")

                        values.Add(DatabaseObjects.Columns.ResolutionSLA, GetSLAValue(txtResolutionSLA, ddlResolutionSLAType, chkUse24x7Calendar.Checked));

                    ///CloseSLA
                    if (txtCloseSLA.Text.Trim() != Varies && ddlCloseSLAType.SelectedValue != "-1")

                        values.Add(DatabaseObjects.Columns.CloseSLA, GetSLAValue(txtCloseSLA, ddlCloseSLAType, chkUse24x7Calendar.Checked));


                    // To check workflow value will be pick from Dropdown or TextBox
                    if (hdnWorkflow.Value == "1" && !string.IsNullOrWhiteSpace(txtWorkflow.Text))
                    {

                        values.Add(DatabaseObjects.Columns.WorkflowType, txtWorkflow.Text.Trim());
                    }
                    else
                    {
                        if (ddlWorkflow.SelectedValue != "-1")
                        {
                            dr[DatabaseObjects.Columns.WorkflowType] = null;
                            if (ddlWorkflow.SelectedIndex >= 1)
                            {

                                values.Add(DatabaseObjects.Columns.WorkflowType, ddlWorkflow.SelectedItem.Text);
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(ddlBudgetItem.GetValues()))
                    {
                        values.Add(DatabaseObjects.Columns.BudgetIdLookup, ddlBudgetItem.GetValues());
                    }
                    if (hndOwner.Value.Trim() != Varies)
                    {
                        values.Add(DatabaseObjects.Columns.Owner, ppeOwner.GetValues());
                    }
                    if (hndPRPGroup.Value.Trim() != Varies)
                    {
                        values.Add(DatabaseObjects.Columns.PRPGroup, ppePrpGroup.GetValues());
                    }

                    //For Prp Group People Picker
                    //For ORP people Picker
                    if (hndORP.Value.Trim() != Varies)
                    {
                        values.Add(DatabaseObjects.Columns.TicketORP, ppePrpGroup.GetValues());
                    }
                    //For Escalation manager people Picker
                    if (hndExecManeger.Value.Trim() != Varies)
                    {

                        values.Add(DatabaseObjects.Columns.RequestTypeEscalationManager, ppeExecManeger.GetValues());

                    }

                    // For Back Up manager people Picker
                    if (hndBackUpMan.Value.Trim() != Varies)
                    {


                        values.Add(DatabaseObjects.Columns.RequestTypeBackupEscalationManager, ppeBackUpMan.GetValues());

                    }
                    if (!string.IsNullOrEmpty(ddlFuncArea.GetValues()))
                    {
                        values.Add(DatabaseObjects.Columns.FunctionalAreaLookup, ddlFuncArea.GetValues());
                    }

                    //if (!cvBackUpMan.IsValid || !cvOwner.IsValid || !cvPrpGroup.IsValid || !cvExecManeger.IsValid)
                    //{
                    //    return;
                    //}

                    if (txtDescription.Text.Trim() != Varies)
                    {
                        //spLItem[DatabaseObjects.Columns.RequestTypeDescription] = txtDescription.Text;
                        values.Add(DatabaseObjects.Columns.RequestTypeDescription, txtDescription.Text);
                    }

                    if (hdnkw.Value.Trim() != Server.HtmlEncode(Varies))
                    {
                        //spLItem[DatabaseObjects.Columns.KeyWords] = hdnkw.Value.Trim();
                        values.Add(DatabaseObjects.Columns.KeyWords, hdnkw.Value.Trim());
                    }

                    if (hdnticketsender.Value.Trim() != Server.HtmlEncode(Varies))
                    {
                        //spLItem[DatabaseObjects.Columns.EmailToTicketSender] = hdnticketsender.Value.Trim();
                        values.Add(DatabaseObjects.Columns.EmailToTicketSender, hdnticketsender.Value.Trim());
                    }

                    // Resolution Type..
                    if (txtResolutionType.Text.Trim() != Varies)
                    {
                        string[] arrayResolutionType = txtResolutionType.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        //spLItem[DatabaseObjects.Columns.ResolutionTypes] = string.Join(Constants.Separator, arrayResolutionType.Where(s => !string.IsNullOrEmpty(s)));
                        values.Add(DatabaseObjects.Columns.ResolutionTypes, string.Join(Constants.Separator, arrayResolutionType.Where(s => !string.IsNullOrEmpty(s))));
                    }

                    //issue Type..
                    if (txtIssueTypes.Text.Trim() != Varies)
                    {
                        string[] arrayIssueType = txtIssueTypes.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        //spLItem[DatabaseObjects.Columns.IssueTypeOptions] = string.Join(Constants.Separator, arrayIssueType.Where(s => !string.IsNullOrEmpty(s)));
                        values.Add(DatabaseObjects.Columns.IssueTypeOptions, string.Join(Constants.Separator, arrayIssueType.Where(s => !string.IsNullOrEmpty(s))));
                    }
                    if (hdnDisableSLA.Value.Trim() != Varies)
                        values.Add(DatabaseObjects.Columns.SLADisabled, chkDisableSLA.Checked);
                    GetTableDataManager.UpdateItem<int>(DatabaseObjects.Tables.RequestType, Convert.ToInt32(dr[DatabaseObjects.Columns.ID]), values);
                    //spLItem.Update();
                }


            }


        }

        protected void btSave_Click(object sender, EventArgs e)
        {

            RequestTypeManager requestTypeManager = new RequestTypeManager(HttpContext.Current.GetManagerContext());
            ModuleRequestType moduleRequestType = new ModuleRequestType();
            //RequestIds = Convert.ToString(Request["ID"]).Split(new string[] { "," }, StringSplitOptions.None).ToList();
            if (requestId > 0 && !IsMultiEdit)
            {
                //moduleRequestType = requestTypeManager.LoadByID(requestId);
                moduleRequestType.ID = requestId;
            }
            lblFuncArea.Text = "";
            if (!Page.IsValid)
                return;

            if (IsMultiEdit)
            {
                IsMultiEdit = true;
                SaveMultipleRequestTypes();
            }
            else
            {

                #region Save Request Type
                //if (spListitem == null)
                //{
                //    spListitem = spRequestTypeList.Items.Add();
                //}
                if (ddlModule.SelectedIndex > 0)
                {
                    moduleRequestType.ModuleNameLookup = ddlModule.SelectedValue;
                }
                else
                {
                    return;
                }

                if (ddlTaskTemplate.SelectedIndex > 0)
                {
                    // values.Add(DatabaseObjects.Columns.TaskTemplateLookup, ddlTaskTemplate.SelectedValue);
                    //spListitem[DatabaseObjects.Columns.TaskTemplateLookup] = ddlTaskTemplate.SelectedValue;
                }




                // Get RMM Category from Dropdown or TextBox
                if (!UGITUtility.StringToBoolean(hdnCategory.Value))
                {
                    if (ddlCategory.SelectedIndex > 0)
                    {
                        moduleRequestType.RequestCategory = ddlCategory.SelectedItem.Text;
                    }

                    else
                    {
                        moduleRequestType.RequestCategory = null;
                    }
                }
                else
                {
                    moduleRequestType.RequestCategory = txtCategory.Text;
                }

                // Get Category Value will be pick from Dropdown or TextBox
                if (!UGITUtility.StringToBoolean(hdnSubCategory.Value))
                {
                    if (!string.IsNullOrEmpty(ddlSubCategory.SelectedValue))
                    {
                        moduleRequestType.Category = ddlSubCategory.SelectedItem.Text;
                    }
                    else
                    {
                        moduleRequestType.Category = null;
                    }
                }
                else
                {
                    moduleRequestType.Category = txtSubCategory.Text;
                }

                // Get Request Sub-Category Value will be pick from Dropdown or TextBox
                if (!UGITUtility.StringToBoolean(hdnsubcategoryaspercategory.Value))
                {
                    if (ddlsubcategoryaspercategory.SelectedIndex > 0)
                    {
                        moduleRequestType.SubCategory = ddlsubcategoryaspercategory.SelectedItem.Text;
                    }
                    else
                    {
                        moduleRequestType.SubCategory = null;
                    }
                }
                else
                {
                    moduleRequestType.SubCategory = txtSubcategoryaspercategory.Text.Trim();
                }

                ///Estimated Hours
                double estimatedHrs = UGITUtility.StringToDouble(txtEstimatedHours.Text.Trim());
                moduleRequestType.EstimatedHours = Math.Round(estimatedHrs * 4, MidpointRounding.ToEven) / 4;
                // To check workflow value will be pick from Dropdown or TextBox
                if (ddlWorkflow.SelectedIndex > 0)
                {
                    moduleRequestType.WorkflowType = ddlWorkflow.SelectedItem.Text;
                }
                else
                {
                    moduleRequestType.WorkflowType = txtWorkflow.Text;
                }

                long BudgetIdLookup = 0;
                long.TryParse(ddlBudgetItem.GetValues(), out BudgetIdLookup);
                moduleRequestType.BudgetIdLookup = BudgetIdLookup;
                moduleRequestType.RequestType = txtRequestType.Text.Trim();
                //moduleRequestType.Title = txtRequestType.Text.Trim();

                if (moduleRequestType.SubCategory != null)
                    moduleRequestType.Title = $"{moduleRequestType.Category} > {moduleRequestType.SubCategory} > {txtRequestType.Text.Trim()}";
                else
                    moduleRequestType.Title = $"{moduleRequestType.Category} > {txtRequestType.Text.Trim()}";

                //for Owner People Picker
                moduleRequestType.Owner = ppeOwner.GetValues();
                // For Prp Group People Picker
                moduleRequestType.PRPGroup = ppePrpGroup.GetValues();
                //For ORP people Picker
                moduleRequestType.ORP = ppeORP.GetValues();
                moduleRequestType.PRPUser = ppePRP.GetValues();
                // For Escalation manager people Picker
                moduleRequestType.BackupEscalationManager = ppeBackUpMan.GetValues();
                moduleRequestType.EscalationManager = ppeExecManeger.GetValues();
                if (ddlFuncArea.GetValues() != null)
                {
                    if (ddlFuncArea.GetValues() == "" || ddlFuncArea.GetValues() == "0")
                    {
                        moduleRequestType.FunctionalAreaLookup = null;
                    }
                    else
                    {
                        long functionalAreaLookup = 0;
                        long.TryParse(ddlFuncArea.GetValues(), out functionalAreaLookup);
                        moduleRequestType.FunctionalAreaLookup = functionalAreaLookup;
                    }

                }

                moduleRequestType.Description = txtDescription.Text;
                moduleRequestType.KeyWords = hdnkw.Value.Trim();
                // moduleRequestType.em
                moduleRequestType.Deleted = chkDeleted.Checked;
                //Auto Assign PRP
                moduleRequestType.AutoAssignPRP = chkAutoAssignPRP.Checked;
                moduleRequestType.OutOfOffice = chkOutOfOffice.Checked;
                if (!string.IsNullOrEmpty(hdnApplicationId.Value))
                {
                    int hdnApplication = 0;
                    int.TryParse(hdnApplicationId.Value, out hdnApplication);
                    moduleRequestType.APPTitleLookup = hdnApplication;
                }
                if (!string.IsNullOrEmpty(hdnAppModuleId.Value))
                {
                    int appTitleLookup = 0;
                    int.TryParse(hdnAppModuleId.Value, out appTitleLookup);
                    moduleRequestType.APPTitleLookup = appTitleLookup;
                }

                ///SLA Saving code.
                moduleRequestType.RequestorContactSLA = (int)SetSLAValueSPListItem(txtRequestorContactSLA, ddlRequestorContactSLAType);
                moduleRequestType.AssignmentSLA = (int)SetSLAValueSPListItem(txtAssignmentSLA, ddlAssignmentSLAType);
                moduleRequestType.ResolutionSLA = (int)SetSLAValueSPListItem(txtResolutionSLA, ddlResolutionSLAType);
                moduleRequestType.CloseSLA = (int)SetSLAValueSPListItem(txtCloseSLA, ddlCloseSLAType);
                //Resolution Type..
                string[] arrayResolutionType = txtResolutionType.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                moduleRequestType.ResolutionTypes = string.Join(Constants.Separator, arrayResolutionType.Where(s => !string.IsNullOrEmpty(s)));
                string[] arrayIssueType = txtIssueTypes.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                moduleRequestType.IssueTypeOptions = string.Join(Constants.Separator, arrayIssueType.Where(s => !string.IsNullOrEmpty(s)));
                moduleRequestType.SortToBottom = chkSortToBottom.Checked;
                moduleRequestType.SLADisabled = chkDisableSLA.Checked;
                moduleRequestType.Use24x7Calendar = chkUse24x7Calendar.Checked;
                #endregion

                //SyncBackToApplication(spListitem);
                if (requestId > 0)
                {
                    requestTypeManager.Update(moduleRequestType);
                    Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Updated Request Type: {moduleRequestType.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                }
                else
                {
                    requestTypeManager.Insert(moduleRequestType);
                    Util.Log.ULog.WriteUGITLog(context.CurrentUser.Id, $"Added Request Type: {moduleRequestType.Title}", Convert.ToString(UGITLogSeverity.Information), Convert.ToString(UGITLogCategory.Admin), context.TenantID);
                }
            }

            // Update SLA values for RequestTypeByLocation if user is updating any RequestType
            if (RequestIds.Count > 0)
                UpdateRequestTypeByLocationSLA(chkDisableSLA.Checked, chkUse24x7Calendar.Checked);

            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private double SetSLAValueSPListItem(TextBox textcontrol, DropDownList dropdown, bool use24x7Calendar = true)
        {
            // Converting days,hours into minutes
            double calculateTime = 0.0;
            if (textcontrol.Text != Varies)
            {
                int workingHoursInADay = 0;

                if (!use24x7Calendar)
                    workingHoursInADay = uHelper.GetWorkingHoursInADay(HttpContext.Current.GetManagerContext(), true);
                else
                    workingHoursInADay = 24;
                // Converting days,hours into minutes
                if (dropdown.SelectedValue == Constants.SLAConstants.Days)
                {
                    calculateTime = Convert.ToDouble(textcontrol.Text.Trim()) * 60 * workingHoursInADay;
                }
                else if (dropdown.SelectedValue == Constants.SLAConstants.Hours)
                {
                    calculateTime = Convert.ToDouble(textcontrol.Text.Trim()) * 60;
                }
                else
                {
                    calculateTime = Convert.ToDouble(textcontrol.Text.Trim());
                }
            }
            return calculateTime;
        }

        private double GetSLAValue(TextBox textcontrol, DropDownList dropdown, bool use24x7Calendar)
        {
            double value = 0.0;
            int workingHoursInADay = 0;

            if (!use24x7Calendar)
                workingHoursInADay = uHelper.GetWorkingHoursInADay(context, true);
            else
                workingHoursInADay = 24;
            // Converting days,hours into minutes
            if (dropdown.SelectedValue == Constants.SLAConstants.Days)
            {
                value = Convert.ToDouble(textcontrol.Text.Trim()) * 60 * workingHoursInADay;
            }
            else if (dropdown.SelectedValue == Constants.SLAConstants.Hours)
            {
                value = Convert.ToDouble(textcontrol.Text.Trim()) * 60;
            }
            else
            {
                value = Convert.ToDouble(textcontrol.Text.Trim());
            }

            return value;
        }


        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void csvdvWorkflow_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Validate Work Flow Type name 
            bool argsval = false;
            if (ddlWorkflow.SelectedIndex < 1)
            {
                if (string.IsNullOrEmpty(txtWorkflow.Text) && hdnWorkflow.Value == "1")
                {
                    divddlWorkflow.Attributes.Add("style", "display:none");
                    divWorkflow.Attributes.Add("style", "display:block");
                }

                if (!string.IsNullOrEmpty(txtWorkflow.Text))
                {
                    argsval = true;
                }
                args.IsValid = argsval;
            }
            else
            {
                divddlWorkflow.Attributes.Add("style", "display:block");
                divWorkflow.Attributes.Add("style", "display:none");
            }

        }

        protected void csvdivSubCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Validate SubCategory name 
            bool argsval = false;
            if (ddlSubCategory.SelectedIndex < 1)
            {
                if (string.IsNullOrEmpty(txtSubCategory.Text) && hdnSubCategory.Value == "1")
                {
                    divddlSubCategory.Attributes.Add("style", "display:none");
                    divSubCategory.Attributes.Add("style", "display:block");
                }
                if (!string.IsNullOrEmpty(txtSubCategory.Text))
                {
                    argsval = true;
                }
                args.IsValid = argsval;
            }
            else
            {
                divddlSubCategory.Attributes.Add("style", "display:block");
                divSubCategory.Attributes.Add("style", "display:none");
            }
        }

        protected void csvtxtCategory_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // Validate Category name 
            bool argsval = false;
            if (ddlCategory.SelectedIndex < 1)
            {
                if (string.IsNullOrEmpty(txtCategory.Text) && hdnCategory.Value == "1")
                {
                    argsval = false;
                    divCategory.Attributes.Add("style", "display:block");
                    divddlCategory.Attributes.Add("style", "display:none");
                }
                if (!string.IsNullOrEmpty(txtCategory.Text))
                {
                    argsval = true;
                }
                args.IsValid = argsval;
            }
            else
            {
                divddlSubCategory.Attributes.Add("style", "display:block");
                divSubCategory.Attributes.Add("style", "display:none");
            }
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            //if (spListitem != null)
            //{
            //    spListitem.Delete();
            //}
            //uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubCategory((ddlModule.SelectedValue));
        }

        #region Request Type By Location

        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&Location={1}&RequestTypeID={2}&Use24x7Calendar={3}";
        string editparam = "requesttypeloc";
        string newparam = "requesttypeloc";
        string formTitle = "Location";
        string LocAddItem = string.Empty;

        private void BindGvLocation()
        {
            //hide for new request type
            if (requestId <= 0 && !IsMultiEdit)
                locationWiseDiv.Visible = false;

            if (RequestIds == null)
                RequestIds = new List<string>();

            if (requestId > 0 && !RequestIds.Exists(x => x == requestId.ToString()))
            {
                RequestIds.Add(requestId.ToString());
            }


            LocAddItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit + "&ReqID=0", newparam, "", string.Join(",", RequestIds), chkUse24x7Calendar.Checked));
            aLocAddItem.Attributes.Add("href", string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - New Item','600','600',0,'{1}','true')", LocAddItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle));
            DataTable dttemp = new DataTable();
            RequestTypeByLocationManager requestTypeByLocationManager = new RequestTypeByLocationManager(HttpContext.Current.GetManagerContext());
            DataRow[] coll = null;
            if (RequestIds.Count == 1)
            {
                DataTable requsestLocation = requestTypeByLocationManager.GetDataTable();
                coll = requsestLocation.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketRequestTypeLookup, requestId));
                if (coll != null && coll.Length > 0)
                {
                    dttemp = coll.CopyToDataTable();
                }
            }
            else
            {
                if (RequestIds.Count > 0)
                {
                    DataTable requsestLocation = requestTypeByLocationManager.GetDataTable();
                    //coll = requsestLocation.Select(string.Format("{0}='{1}'", DatabaseObjects.Columns.TicketRequestTypeLookup, string.Join(",", RequestIds.Select(x => string.Format("'{0}'", x))))); //GetTableDataManager.GetDataRow(DatabaseObjects.Tables.RequestTypeByLocation, DatabaseObjects.Columns.TicketRequestTypeLookup, string.Join(",", RequestIds.Select(x => string.Format("'{0}'", x))));
                    coll = requsestLocation.Select(string.Format("{0} in ({1})", DatabaseObjects.Columns.TicketRequestTypeLookup, string.Join(",", RequestIds.Select(x => "'" + x + "'"))));
                    if (coll != null && coll.Length > 0)
                    {
                        dttemp = coll.CopyToDataTable();
                    }
                }
            }
            if (IsMultiEdit)
            {
                //Show only comman location entries in case of multi edit
                if (dttemp != null && dttemp.Rows.Count > 0)
                {
                    DataTable uniqueData = dttemp.Clone();
                    var locations = dttemp.AsEnumerable().ToLookup(x => x.Field<string>(DatabaseObjects.Columns.LocationLookup));
                    foreach (var loc in locations)
                    {
                        if (loc.Count() == RequestIds.Count)
                        {
                            DataRow fisrtRow = loc.First();
                            DataRow row = uniqueData.NewRow();

                            row[DatabaseObjects.Columns.Id] = fisrtRow[DatabaseObjects.Columns.Id];
                            row[DatabaseObjects.Columns.LocationLookup] = fisrtRow[DatabaseObjects.Columns.LocationLookup];
                            row[DatabaseObjects.Columns.Owner] = fisrtRow[DatabaseObjects.Columns.Owner];
                            row[DatabaseObjects.Columns.PRPGroup] = fisrtRow[DatabaseObjects.Columns.PRPGroup];

                            int rowCount = loc.ToArray().Where(x => !x.IsNull(DatabaseObjects.Columns.Owner)).Select(x => x.Field<string>(DatabaseObjects.Columns.Owner)).Distinct().Count();
                            if (rowCount > 1)
                                row[DatabaseObjects.Columns.Owner] = Varies;

                            rowCount = loc.ToArray().Where(x => !x.IsNull(DatabaseObjects.Columns.PRPGroup)).Select(x => x.Field<string>(DatabaseObjects.Columns.PRPGroup)).Distinct().Count();
                            if (rowCount > 1)
                                row[DatabaseObjects.Columns.PRPGroup] = Varies;
                            uniqueData.Rows.Add(row);
                        }
                    }
                    dttemp = uniqueData;
                }
            }

            if (dttemp != null && dttemp.Rows.Count > 0)
            {
                dttemp.DefaultView.Sort = string.Format("{0} asc", DatabaseObjects.Columns.LocationLookup);
                divLocation.Visible = true;
                gvLocation.DataSource = dttemp;
                gvLocation.DataBind();
            }
            else
            {
                divLocation.Visible = false;
            }
        }
        protected void gvLocation_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.Name == "aEdit")
            {

                int index = e.VisibleIndex;
                string dataKeyValue = Convert.ToString(e.KeyValue);
                string title = Convert.ToString(e.GetValue(DatabaseObjects.Columns.Title));
                string editItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editparam, e.GetValue(DatabaseObjects.Columns.LocationLookup), string.Join(",", RequestIds), chkUse24x7Calendar.Checked)) + "&ReqID=" + dataKeyValue;
                //string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{3} - {1}','600','300',0,'{2}','true')", editItem, "", Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                string url = string.Format("javascript:UgitOpenPopupDialog('{0}','','{2} - Edit Item','600','600',0,'{1}','true')", editItem, Server.UrlEncode(Request.Url.AbsolutePath), formTitle);
                HtmlAnchor aHtml = (HtmlAnchor)gvLocation.FindRowCellTemplateControl(index, e.DataColumn, "aEdit");
                aHtml.Attributes.Add("href", url);
            }
            else
            {
                string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));
                FieldConfigurationManager fmanger = new FieldConfigurationManager(HttpContext.Current.GetManagerContext());
                string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);
                if (!string.IsNullOrEmpty(value))
                {
                    e.Cell.Text = value;
                }
            }
        }

        #endregion

        protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubCategoryAsPerCategory();
        }
        protected void overrideSLAPanel_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            trSLARequestor.Style.Remove("display");
            trSLAAssignment.Style.Remove("display");
            trSLAResolution.Style.Remove("display");
            trSLAClose.Style.Remove("display");
            if (chkDisableSLA.Checked)
            {
                trSLARequestor.Style.Add("display", "none");
                trSLAAssignment.Style.Add("display", "none");
                trSLAResolution.Style.Add("display", "none");
                trSLAClose.Style.Add("display", "none");
                chkUse24x7Calendar.Checked = false;
                trUse24x7Calendar.Style.Add("display", "none");
            }
        }

        #region Method to Update SLA values for RequestTypeByLocation
        /// <summary>
        /// Method to Update SLA values for RequestTypeByLocation
        /// </summary>
        /// <param name="SLADisabled"></param>
        /// <param name="use24x7NewValue"></param>
        private void UpdateRequestTypeByLocationSLA(bool SLADisabled, bool use24x7NewValue)
        {
            // Exit if no change occurs for Use24x7Calendar field on RequestType
            if (use24x7OldValue == use24x7NewValue)
                return;

            RequestTypeByLocationManager requestTypeByLocationMGR = new RequestTypeByLocationManager(context);

            //string requiredQuery = string.Format("<In><FieldRef Name='{0}' LookupId='True'/><Values>{1}</Values></In>", DatabaseObjects.Columns.TicketRequestTypeLookup, string.Join("", RequestIds.Select(x => string.Format("<Value Type='Integer'>{0}</Value>", x))));
            //SPQuery sQuery = new SPQuery();
            //sQuery.ViewFields = string.Format("<FieldRef Name= '{0}' Nullable='True'/><FieldRef Name= '{1}' Nullable='True'/><FieldRef Name= '{2}' Nullable='True'/><FieldRef Name= '{3}' Nullable='True'/>", DatabaseObjects.Columns.AssignmentSLA, DatabaseObjects.Columns.RequestorContactSLA, DatabaseObjects.Columns.ResolutionSLA, DatabaseObjects.Columns.CloseSLA);
            //sQuery.Query = string.Format("<Where>{0}</Where>", requiredQuery);
            //SPListItemCollection resultCollection = requestTypeByLocationList.GetItems(sQuery);
            List<ModuleRequestTypeLocation> resultCollection = requestTypeByLocationMGR.Load(x => RequestIds.Contains(Convert.ToString(x.RequestTypeLookup)) && x.AssignmentSLA == 0 && x.RequestorContactSLA == 0 && x.ResolutionSLA == 0 && x.CloseSLA == 0);
            if (resultCollection == null || resultCollection.Count == 0)
                return;

            foreach (ModuleRequestTypeLocation item in resultCollection)
            {
                if (SLADisabled)
                {
                    item.RequestorContactSLA = 0.0;
                    item.AssignmentSLA = 0.0;
                    item.ResolutionSLA = 0.0;
                    item.CloseSLA = 0.0;
                }
                else
                {
                    double requestContactSLA = item.RequestorContactSLA;
                    double assignmentSLA = item.AssignmentSLA;
                    double resolutionSLA = item.ResolutionSLA;
                    double closeSLA = item.CloseSLA;

                    item.RequestorContactSLA = GetLocationSLAValue(requestContactSLA, use24x7OldValue, use24x7NewValue);
                    item.AssignmentSLA = GetLocationSLAValue(assignmentSLA, use24x7OldValue, use24x7NewValue);
                    item.ResolutionSLA = GetLocationSLAValue(resolutionSLA, use24x7OldValue, use24x7NewValue);
                    item.CloseSLA = GetLocationSLAValue(closeSLA, use24x7OldValue, use24x7NewValue);
                }
                requestTypeByLocationMGR.Update(item);
            }
        }
        #endregion Method to Update SLA values for RequestTypeByLocation

        #region Method to Calculate Minutes for RequestTypeByLocation SLA
        /// <summary>
        /// Method to Calculate Minutes for RequestTypeByLocation SLA
        /// </summary>
        /// <param name="oldMinutes"></param>
        /// <param name="use24x7OldValue"></param>
        /// <param name="use24x7NewValue"></param>
        /// <returns></returns>
        private double GetLocationSLAValue(double oldMinutes, bool use24x7OldValue, bool use24x7NewValue)
        {
            double finalMinutes = 0.0;
            double value = 0.0;
            string unit = string.Empty;
            int workingHoursInADay = 0;

            // Calculating units and minutes based on use24x7OldValue
            if (use24x7OldValue)
                workingHoursInADay = 24;
            else
                workingHoursInADay = uHelper.GetWorkingHoursInADay(context, true);

            if (oldMinutes % (workingHoursInADay * 60) == 0)
            {
                value = oldMinutes / (workingHoursInADay * 60);
                unit = Constants.SLAConstants.Days;
            }
            else if (oldMinutes % 60 == 0)
            {
                value = oldMinutes / 60;
                unit = Constants.SLAConstants.Hours;
            }
            else
            {
                value = oldMinutes;
                unit = Constants.SLAConstants.Minutes;
            }

            //Calculating final minutes according to use24x7NewValue
            if (use24x7NewValue)
                workingHoursInADay = 24;
            else
                workingHoursInADay = uHelper.GetWorkingHoursInADay(context, true);

            // Converting days and hours into minutes
            if (unit == Constants.SLAConstants.Days)
                finalMinutes = value * 60 * workingHoursInADay;
            else if (unit == Constants.SLAConstants.Hours)
                finalMinutes = value * 60;
            else
                finalMinutes = value;

            return finalMinutes;
        }
        #endregion Method to Calculate Minutes for RequestTypeByLocation SLA
    }
}

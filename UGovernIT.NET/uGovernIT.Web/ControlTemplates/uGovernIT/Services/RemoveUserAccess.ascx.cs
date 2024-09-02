using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using System.Linq;
using uGovernIT.Utility;
using uGovernIT.Manager;

namespace uGovernIT.Web
{
    public class RemoveAccessList
    {
        public int UserId { get; set; }
        public List<ModuleRoleRelation> ModuleRoleRelationList { get; set; }
        public List<ServiceMatrixData> ServiceMatrixDataList { get; set; }
        public string SelectionType { get; set; }
    }
    public class ModuleRoleRelation
    {
        public string ApplicationModule { get; set; }
        public string ApplicationRole { get; set; }
        public string ApplicationId { get; set; }
    }
    public partial class RemoveUserAccess : UserControl
    {
        public string QuestionId { get; set; }
        int userID = 0;
        public RemoveAccessList RemoveAccessList { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsShowFieldSet { get; set; }
        public bool IsShowRemoveAllAccessControl { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
           GenerateCntrolInReadOnlyMode();
        }

        protected void rdRemoveAccess_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            GetUserId();
            if (userID > 0)
            {
                if (rdRemoveAccess.SelectedItem.Value.ToLower() == "removespecificaccess")
                {
                    pnlUserAccess.Visible = true;
                    btnAllApplications.Visible = false;
                    GenerateApplicationAccessControl(true, null);
                    //Control cntr = pnlUserAccess.FindControl("rptApplModuleRole");
                    //if (cntr == null)
                    //{
                    //    lblNoApp.Visible = true;
                    //}
                    //else
                    //{
                    //    lblNoApp.Visible = false;
                    //}
                }
                else
                {
                    lblNoApp.Visible = false;
                    pnlUserAccess.Visible = false;
                    btnAllApplications.Visible = true;
                }
            }
        }

        private void GetUserId()
        {
            //SPFieldUserValue userRoleAssignee = new SPFieldUserValue();
           
            //ArrayList userPRPGroupList = pplUser.ResolvedEntities;
            //if (userPRPGroupList != null && userPRPGroupList.Count > 0)
            //{
                //SPFieldUserValueCollection ursVals = pplUser.GetUserValueCollection(); //uHelper.GetFieldUserValueCollection(userPRPGroupList, SPContext.Current.Web);
                //if (ursVals != null && ursVals.Count > 0)
                //{
                //    userRoleAssignee.LookupId = ursVals[0].User.ID;
                //}
            //}
            //else
            {
                // lblErrorMessage.Visible = true;
            }
        }

        private void GenerateApplicationAccessControl(bool IsNoteEnabled, List<ServiceMatrixData> serviceMatrixDataList)
        {
            string id = string.Format("QuestionId");
            Control cntrl = pnlUserAccess.FindControl(id);
            if (cntrl == null)
            {
                ServiceMatrix serviceMatrix = (ServiceMatrix)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceMatrix.ascx");

                if (serviceMatrixDataList == null)
                {
                    serviceMatrixDataList = new List<ServiceMatrixData>();

                    //SPQuery query = new SPQuery();
                    //query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ApplicationRoleAssign, userID);
                    //query.ViewFields = string.Concat(
                    //                    string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Id),
                    //                    string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Title),
                    //                    string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleLookup),
                    //                    string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.APPTitleLookup),
                    //                    string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationModulesLookup),
                    //                    string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleAssign));
                    //SPListItemCollection applicationListColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplModuleRoleRelationship, query);
                    //if (applicationListColl != null && applicationListColl.Count > 0)
                    //{
                    //    IEnumerable distinctApplicationsList = applicationListColl.Cast<SPListItem>().Select(itm => itm[DatabaseObjects.Columns.APPTitleLookup]).Distinct();
                    //    if (distinctApplicationsList != null)
                    //    {
                    //        IEnumerator enumerator = distinctApplicationsList.GetEnumerator();
                    //        while (enumerator.MoveNext())
                    //        {
                    //            ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
                    //            SPFieldLookupValue spFieldApplLookup = new SPFieldLookupValue(Convert.ToString(enumerator.Current));
                    //            int applicationId = spFieldApplLookup.LookupId;
                    //            serviceMatrixData.Name = spFieldApplLookup.LookupValue;
                    //            serviceMatrixData.ID = Convert.ToString(applicationId);
                    //            serviceMatrixData.RoleAssignee = Convert.ToString(userID);
                    //            SPQuery queryApp = new SPQuery();
                    //            queryApp.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.APPTitleLookup, applicationId);

                    //            SPQuery queryAppRole = new SPQuery();
                    //            queryAppRole.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='True'/><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.APPTitleLookup, applicationId, DatabaseObjects.Columns.ApplicationRoleAssign, userID);
                    //            queryAppRole.ViewFields = string.Concat(
                    //                                                string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Id),
                    //                                                string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Title),
                    //                                                string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleLookup),
                    //                                                string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.APPTitleLookup),
                    //                                                string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationModulesLookup),
                    //                                                string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleAssign));
                    //            List<ServiceData> lstServiceData = new List<ServiceData>();
                    //            SPListItemCollection spListItemColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplModuleRoleRelationship, queryAppRole);
                    //            if (spListItemColl != null && spListItemColl.Count > 0)
                    //            {
                    //                foreach (SPListItem spItem in spListItemColl)
                    //                {
                    //                    ServiceData serviceData = new ServiceData();
                    //                    SPFieldLookupValue spFieldLookupValueRoles = new SPFieldLookupValue(Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationRoleLookup]));
                    //                    serviceData.ColumnName = spFieldLookupValueRoles.LookupValue;
                    //                    SPFieldLookupValue spFieldLookupValueModules = new SPFieldLookupValue(Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationModulesLookup]));
                    //                    serviceData.RowName = spFieldLookupValueModules.LookupValue;
                    //                    serviceData.Value = Convert.ToString(spItem[DatabaseObjects.Columns.Id]);
                    //                    lstServiceData.Add(serviceData);
                    //                }
                    //                serviceMatrixData.lstGridData = lstServiceData;
                    //            }
                    //            queryApp.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
                    //            queryApp.ViewFieldsOnly = true;
                    //            SPListItemCollection splistAppModules = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplicationModules, queryApp);
                    //            List<string> lstRowNames = new List<string>();
                    //            foreach (SPListItem row in splistAppModules)
                    //            {
                    //                lstRowNames.Add(Convert.ToString(row[DatabaseObjects.Columns.Title]));
                    //            }
                    //            serviceMatrixData.lstRowsNames = lstRowNames;
                    //            SPListItemCollection splistAppRoles = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplicationRole, queryApp);
                    //            List<string> lstColumnNames = new List<string>();
                    //            foreach (SPListItem column in splistAppRoles)
                    //            {
                    //                lstColumnNames.Add(Convert.ToString(column[DatabaseObjects.Columns.Title]));
                    //            }
                    //            serviceMatrixData.lstColumnsNames = lstColumnNames;

                    //            if (splistAppModules != null && splistAppModules.Count > 0 && splistAppRoles != null && splistAppRoles.Count > 0)
                    //                serviceMatrixDataList.Add(serviceMatrixData);
                    //        }
                    //    }
                    //}
                }
                if (serviceMatrix != null)
                {
                    //serviceMatrix.ControlType = "CheckBox";
                    serviceMatrix.IsReadOnly = IsReadOnly;
                    serviceMatrix.ControlIDPrefix = QuestionId;
                    serviceMatrix.IsNoteEnabled = IsNoteEnabled;
                  //serviceMatrix.IsShowFieldSet = IsShowFieldSet;
                    serviceMatrix.ID = string.Format("QuestionId");
                    pnlUserAccess.Controls.Add(serviceMatrix);
                }
                if (serviceMatrixDataList == null || serviceMatrixDataList.Count == 0)
                {
                    lblNoApp.Visible = true;
                }
                else
                {
                    lblNoApp.Visible = false;
                }
            }
        }


        //public void GetSelectedValues()
        //{
        //    GetUserId();
        //    RemoveAccessList removeAccessList = new RemoveAccessList();
        //    List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
        //    List<ModuleRoleRelation> moduleRoleRelationList = new List<ModuleRoleRelation>();
        //    if (rdRemoveAccess.SelectedItem.Value.ToLower() == "removespecificaccess")
        //    {
        //        string ID = string.Format("QuestionId");
        //        ServiceMatrix serviceMatrix = (ServiceMatrix)pnlUserAccess.FindControl(ID);
        //        serviceMatrix.SaveState();
        //        if (serviceMatrix.ServiceMatrixDataList != null && serviceMatrix.ServiceMatrixDataList.Count > 0)
        //        {
        //            serviceMatrixDataList = serviceMatrix.ServiceMatrixDataList;
        //            foreach (ServiceMatrixData serviceMatrixData in serviceMatrix.ServiceMatrixDataList)
        //            {
        //                foreach (ServiceData serviceData in serviceMatrixData.lstGridData)
        //                {
        //                    ModuleRoleRelation moduleRelation = new ModuleRoleRelation();
        //                    moduleRelation.ApplicationModule = serviceData.RowName;
        //                    moduleRelation.ApplicationRole = serviceData.ColumnName;
        //                    moduleRelation.ApplicationId = serviceData.ID;
        //                    moduleRoleRelationList.Add(moduleRelation);
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        SPQuery query = new SPQuery();
        //        query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ApplicationRoleAssign, userID);
        //        SPListItemCollection applicationListColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplModuleRoleRelationship, query);
        //        if (applicationListColl != null && applicationListColl.Count > 0)
        //        {
        //            foreach (SPListItem item in applicationListColl)
        //            {
        //                ModuleRoleRelation moduleRelation = new ModuleRoleRelation();
        //                SPFieldLookupValue spFieldLookupValueModules = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.ApplicationModulesLookup]));
        //                moduleRelation.ApplicationModule = spFieldLookupValueModules.LookupValue;
        //                SPFieldLookupValue spFieldLookupValueRoles = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.ApplicationRoleLookup]));
        //                moduleRelation.ApplicationRole = spFieldLookupValueRoles.LookupValue;
        //                SPFieldLookupValue spFieldLookupValueApplication = new SPFieldLookupValue(Convert.ToString(item[DatabaseObjects.Columns.APPTitleLookup]));
        //                moduleRelation.ApplicationId = Convert.ToString(spFieldLookupValueApplication.LookupId);
        //                moduleRoleRelationList.Add(moduleRelation);
        //            }

        //        }
        //    }
        //    removeAccessList.UserId = userID;
        //    removeAccessList.ModuleRoleRelationList = moduleRoleRelationList;
        //    removeAccessList.SelectionType = rdRemoveAccess.SelectedItem.Value;
        //    removeAccessList.ServiceMatrixDataList = serviceMatrixDataList;
        //    RemoveAccessList = removeAccessList;

        //}

        public void GenerateCntrolInReadOnlyMode()
        {
            bool isNoteEnabled = true;
            if (IsReadOnly == true)
            {
                lblUserReadOnly.Visible = true;
                pplUser.Visible = false;
                rdRemoveAccess.Visible = false;
                pnlSelection.Visible = true;
                isNoteEnabled = true;
                btnAllApplications.Visible = false;
            }
            if (IsShowFieldSet == false)
            {
                rdRemoveAccess.Visible = false;
                pplUser.Visible = false;
                pnlSelection.Visible = true;
                lblUserReadOnly.Visible = true;
                lblselectionType.Visible = true;
                btnAllApplications.Visible = false;
            }
            ListItem item = null;
            string strRemoveAccessSelectionId = string.Empty;
            List<ServiceMatrixData> serviceMatrixDataList = null;
            if (RemoveAccessList != null)
            {
                userID = RemoveAccessList.UserId;
                if (userID > 0)
                {
                    if (!string.IsNullOrEmpty(RemoveAccessList.SelectionType))
                    {
                        strRemoveAccessSelectionId = Convert.ToString(RemoveAccessList.SelectionType.ToLower());
                        serviceMatrixDataList = RemoveAccessList.ServiceMatrixDataList;
                    }
                }
                else
                {
                    //  lblErrorMessage.Visible = true;
                }
            }
            else
            {
                strRemoveAccessSelectionId = Convert.ToString(Request.Form[rdRemoveAccess.UniqueID]);
                if (!string.IsNullOrEmpty(strRemoveAccessSelectionId))
                {
                    GetUserId();
                }
            }
            if (!string.IsNullOrEmpty(strRemoveAccessSelectionId))
            {
                item = rdRemoveAccess.Items.FindByValue(strRemoveAccessSelectionId);
            }
            if (userID > 0)
            {
                if (strRemoveAccessSelectionId == "removespecificaccess" || IsShowRemoveAllAccessControl)
                {
                    pnlUserAccess.Visible = true;
                    btnAllApplications.Visible = false;
                    GenerateApplicationAccessControl(isNoteEnabled, serviceMatrixDataList);
                }
                else
                {
                    pnlUserAccess.Visible = false;
                }
                //SPFieldUserValue arrRoleAssignee = new SPFieldUserValue(SPContext.Current.Web, Convert.ToString(userID));
                //pplUser.UpdateEntities(uHelper.getUsersListFromCollection(arrRoleAssignee));
                //pplUser.Value =Convert.ToString(arrRoleAssignee);
            }
            if (item != null)
            {
                rdRemoveAccess.SelectedValue = item.Value;
                lblselectionType.Text = item.Text;
            }

        }

        public void GetRemoveAllAccessControl()
        {
            if (RemoveAccessList != null && RemoveAccessList.ModuleRoleRelationList != null && RemoveAccessList.ModuleRoleRelationList.Count() > 0)
            {
                UpdateRemoveAccessList(RemoveAccessList);
            }
        }

        public void UpdateRemoveAccessList(RemoveAccessList removeAccessList)
        {
            //List<ServiceMatrixData> serviceMatrixDataList = new List<ServiceMatrixData>();
            //string applicationId = removeAccessList.ModuleRoleRelationList[0].ApplicationId;
            //ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
            //SPList applicationList = SPListHelper.GetSPList(DatabaseObjects.Lists.Applications);
            //DataTable dtApplications = applicationList.Items.GetDataTable();
            //DataRow[] drApps = dtApplications.Select(string.Format("{0}={1}", DatabaseObjects.Columns.Id, applicationId));

            //serviceMatrixData.Name = Convert.ToString(drApps[0][DatabaseObjects.Columns.Title]);
            //serviceMatrixData.ID = applicationId;
            //SPQuery query = new SPQuery();
            ////query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.APPTitleLookup, applicationId);
            //query.Query = string.Format("<Where><And><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq><Eq><FieldRef Name='{2}' LookupId='True'/><Value Type='Lookup'>{3}</Value></Eq></And></Where>", DatabaseObjects.Columns.APPTitleLookup, applicationId, DatabaseObjects.Columns.ApplicationRoleAssign, removeAccessList.UserId);
            //query.ViewFields = string.Concat(
            //                            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Id),
            //                            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Title),
            //                            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleLookup),
            //                            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.APPTitleLookup),
            //                            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationModulesLookup),
            //                            string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.ApplicationRoleAssign));
            //List<ServiceData> lstServiceData = new List<ServiceData>();
            //SPListItemCollection spListItemColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplModuleRoleRelationship, query);
            //if (spListItemColl != null && spListItemColl.Count > 0)
            //{
            //    foreach (SPListItem spItem in spListItemColl)
            //    {
            //        ServiceData serviceData = new ServiceData();
            //        SPFieldLookupValue spFieldLookupValueRoles = new SPFieldLookupValue(Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationRoleLookup]));
            //        serviceData.ColumnName = spFieldLookupValueRoles.LookupValue;
            //        SPFieldLookupValue spFieldLookupValueModules = new SPFieldLookupValue(Convert.ToString(spItem[DatabaseObjects.Columns.ApplicationModulesLookup]));
            //        serviceData.RowName = spFieldLookupValueModules.LookupValue;
            //        serviceData.Value = Convert.ToString(spItem[DatabaseObjects.Columns.Id]);

            //        lstServiceData.Add(serviceData);

            //    }
            //    serviceMatrixData.lstGridData = lstServiceData;

            //    SPQuery spquery = new SPQuery();
            //    spquery.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.APPTitleLookup, applicationId);
            //    spquery.ViewFields = string.Format("<FieldRef Name='{0}'/>", DatabaseObjects.Columns.Title);
            //    spquery.ViewFieldsOnly = true;
            //    SPListItemCollection splistAppModules = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplicationModules, spquery);
            //    List<string> lstRowNames = new List<string>();
            //    foreach (SPListItem row in splistAppModules)
            //    {
            //        lstRowNames.Add(Convert.ToString(row[DatabaseObjects.Columns.Title]));
            //    }
            //    serviceMatrixData.lstRowsNames = lstRowNames;
            //    SPListItemCollection splistAppRoles = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplicationRole, spquery);
            //    List<string> lstColumnNames = new List<string>();
            //    foreach (SPListItem column in splistAppRoles)
            //    {
            //        lstColumnNames.Add(Convert.ToString(column[DatabaseObjects.Columns.Title]));
            //    }
            //    serviceMatrixData.lstColumnsNames = lstColumnNames;

            //    if (lstRowNames.Count > 0 && lstColumnNames.Count > 0)
            //    {
            //        serviceMatrixData.RoleAssignee = Convert.ToString(removeAccessList.UserId);
            //        serviceMatrixData.Note = string.Format("This remove all selected access of {0}", serviceMatrixData.Name);
            //        serviceMatrixDataList.Add(serviceMatrixData);
            //        removeAccessList.ServiceMatrixDataList = serviceMatrixDataList;
            //    }
            //}
        }

        protected void btnAllApplication_Click(object sender, EventArgs e)
        {
            //if (userID > 0)
            //{
            //    SPQuery query = new SPQuery();
            //    query.Query = string.Format("<Where><Eq><FieldRef Name='{0}' LookupId='True'/><Value Type='Lookup'>{1}</Value></Eq></Where>", DatabaseObjects.Columns.ApplicationRoleAssign, userID);
            //    query.ViewFields = string.Concat(
            //                        string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.Id),
            //                        string.Format("<FieldRef Name='{0}' Nullable='TRUE' />", DatabaseObjects.Columns.APPTitleLookup));
            //    query.ViewFieldsOnly = true;
            //    List<ServiceData> lstServiceData = new List<ServiceData>();
            //    SPListItemCollection spListItemColl = SPListHelper.GetSPListItemCollection(DatabaseObjects.Lists.ApplModuleRoleRelationship, query);
            //    DataTable distinctValues = null;
            //    if (spListItemColl != null && spListItemColl.Count > 0)
            //    {
            //        DataTable dt = spListItemColl.GetDataTable();
            //        DataView dv = dt.DefaultView;
            //        dv.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.APPTitleLookup);
            //        distinctValues = dv.ToTable(true, DatabaseObjects.Columns.APPTitleLookup);
            //    }
            //    gvApplications.DataSource = distinctValues;
            //    gvApplications.DataBind();
            //    if (distinctValues == null)
            //    {
            //        lblPopUpMessage.Text = "No application exists.";
            //    }
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "ApplicationPopup", "<script>ShowApplicationDialog();</script>");

            //}
        }
    }
}

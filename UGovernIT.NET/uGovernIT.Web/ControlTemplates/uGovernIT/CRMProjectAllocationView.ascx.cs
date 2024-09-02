using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.HtmlControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using System.Linq;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class CRMProjectAllocationView : System.Web.UI.UserControl
    {
        public string ticketID { get; set; }
        public string FrameId;
        public bool ReadOnly;
        DataRow spListItem;
        string addNewItem;

        private string absoluteUrlEdit = "/Layouts/uGovernIT/DelegateControl.aspx?control={0}&ID={1}&ticketId={2}";
        private string formTitle = "Project Allocation";
        private string editParam = "crmprojectallocationaddedit";

        public bool IsReadOnly { get; set; }
        public string ModuleName { get; set; }

        public string projectStartDate { get; set; }
        public string projectEndDate { get; set; }


        protected string findResourceUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=crmprojectallocationaddedit&filterMode=CPRTeamAllocation&Mode=RMMProjectAgent");
        protected string findProjectRoleUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=projectrolesview");
        protected string importAllocationTemplateUrl = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=importallocationtemplate&ProjectID={0}");
        //protected string ProjectExternalTeamUrl = uHelper.GetAbsoluteURL("/_layouts/15/uGovernIT/DelegateControl.aspx?control=projectexternalteam");

        private ApplicationContext _context = null;
        protected ApplicationContext ApplicationContext
        {
            get
            {
                if (_context == null)
                {
                    _context = HttpContext.Current.GetManagerContext();
                }
                return _context;
            }

        }



        protected override void OnInit(EventArgs e)
        {
            string title = string.Format("{0} - New Item", formTitle);
            string module = string.Empty;
            if (!string.IsNullOrEmpty(ticketID))
            {
                //module = UGITUtility.getModuleNameByTicketId(ticketID);
                //spListItem = Ticket.getCurrentTicket(module, ticketID);

                spListItem =  Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(ticketID), ticketID);

                if (spListItem != null)
                {
                    if (UGITUtility.IfColumnExists(spListItem,DatabaseObjects.Columns.CRMProjectID) && spListItem[DatabaseObjects.Columns.CRMProjectID] != null)
                        title = string.Format("{0} {1}", Convert.ToString(spListItem[DatabaseObjects.Columns.CRMProjectID]), Convert.ToString(spListItem[DatabaseObjects.Columns.Title]));
                    else
                        title = string.Format("{0}", Convert.ToString(spListItem[DatabaseObjects.Columns.Title]));
                }
            }

            addNewItem = UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, "0", ticketID));
            aAddItem.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{2}','600','480',0,'{1}','true')", addNewItem, Server.UrlEncode(Request.Url.AbsolutePath), UGITUtility.ReplaceInvalidCharsInURL(title)));
            importAllocationTemplateUrl = UGITUtility.GetAbsoluteURL(string.Format(importAllocationTemplateUrl, ticketID));
            aImportTemplate.Attributes.Add("href", string.Format("javascript:window.parent.UgitOpenPopupDialog('{0}','','{1}','600','420',0,'{2}','true')", importAllocationTemplateUrl, "Select Project Team Template", Server.UrlEncode(Request.Url.AbsolutePath)));
            saveAsTemplateCtrl.ModuleName = ModuleName;
            saveAsTemplateCtrl.ProjectID = ticketID;
            saveAsTemplateCtrl.PopupID = pcSaveAsTemplate.ClientID;

            if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.EstimatedConstructionStart))
            {
                projectStartDate = Convert.ToString(spListItem[DatabaseObjects.Columns.EstimatedConstructionStart]);
            }
            else if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.PreconStartDate))
            {
                projectStartDate = Convert.ToString(spListItem[DatabaseObjects.Columns.PreconStartDate]);
            }
            else if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.ContractStartDate))
            {
                projectStartDate = Convert.ToString(spListItem[DatabaseObjects.Columns.ContractStartDate]);
            }

            if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.EstimatedConstructionEnd))
                projectEndDate = Convert.ToString(spListItem[DatabaseObjects.Columns.EstimatedConstructionEnd]);
            else if (UGITUtility.IsSPItemExist(spListItem, DatabaseObjects.Columns.ContractExpirationDate))
            {
                projectEndDate = Convert.ToString(spListItem[DatabaseObjects.Columns.ContractExpirationDate]);
            }

            string strfindResourceUrl = string.Format("{0}&ticketId={1}&startDate={2}&endDate={3}", findResourceUrl, ticketID, projectStartDate, projectEndDate);
            aFindResource.Attributes.Add("href", string.Format("javascript:OpenFindingResource('{0}')", strfindResourceUrl));

            aProjectRole.Visible = false;
            if (module == "CPR" || module == "CNS")
            {
                string strFindProjectRole = string.Format("{0}&ticketId={1}", findProjectRoleUrl, ticketID);
                aProjectRole.Attributes.Add("href", string.Format("javascript:OpenFindProjectRole('{0}')", strFindProjectRole));
                aProjectRole.Visible = true;
            }
            
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            // Check whether current logged in user is authorise to edit item or not.
            if (!ReadOnly)
            {
                int readWriteID = UGITUtility.StringToInt(Request["rwID"]);
               //// UGITModule moduleObj = uGITCache.ModuleConfigCache.GetCachedModule(HttpContext.Current.GetManagerContext(), ModuleName);
                ////if (!Ticket.HasFieldLevelAccess(moduleObj, spListItem, readWriteID, HttpContext.Current.GetManagerContext()))
                ////{
                ////    IsReadOnly = true;
                ////}
            }


            if (IsReadOnly)
            {
                aAddItem.Visible = false;
                aFindResource.Visible = false;
                grdAllocation.Columns[0].Visible = false;
                grdAllocation.Columns[1].Visible = false;
            }

            BindGrid();
            base.OnLoad(e);
        }

        void BindGrid()
        {

            string viewFields = string.Format("{0} = '{1}'",
                                    DatabaseObjects.Columns.TicketId, ticketID);

            ProjectEstimatedAllocationManager cRMProjectAllocationManager = new ProjectEstimatedAllocationManager(_context);
            //List<CRMProjectAllocation> collection = cRMProjectAllocationManager.Load(x => x.TicketId == ticketID);
            DataTable collection =  GetTableDataManager.GetTableData(DatabaseObjects.Tables.CRMProjectAllocation, $"{viewFields.ToString()} and TenantID='{ApplicationContext.TenantID}'"); 


            DataTable dtResult = null;
            if (collection != null && collection.Rows.Count > 0)
            {
                //var sorted
                dtResult = collection;
                dtResult.DefaultView.Sort = DatabaseObjects.Columns.AllocationStartDate + " ASC";

                grdAllocation.DataSource = dtResult;
                grdAllocation.DataBind();
                grdAllocation.ExpandAll();
            }
        }

        protected void grdAllocation_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {


            if (e.DataColumn.FieldName == DatabaseObjects.Columns.Type)
            {
                string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));


                UserRoleManager roleManager = new UserRoleManager(ApplicationContext);
                List<Role> sRoles = roleManager.GetRoleList();

                foreach (Role oGroup in sRoles)
                {
                    if(oGroup.Id == values)
                    {
                        e.Cell.Text = oGroup.Title;
                    }

                }

                //fmanger = new FieldConfigurationManager(context);
                //if (fmanger.GetFieldByFieldName(e.DataColumn.FieldName) != null)
                //{
                //string value = fmanger.GetFieldConfigurationData(e.DataColumn.FieldName, values);
                //e.Cell.Text = value;
                //}
            }

            if (e.DataColumn.FieldName == DatabaseObjects.Columns.AssignedTo)
            {
                string values = Convert.ToString(e.GetValue(e.DataColumn.FieldName));


                UserProfileManager UserProfileManager = new UserProfileManager(ApplicationContext);
                UserProfile UserProfile = UserProfileManager.GetUserById(values);
                if(UserProfile != null)
                    e.Cell.Text = UserProfile.Name;
                    
            }


        }



        protected void grdAllocation_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data || e.KeyValue == null)
                return;

            if (!IsReadOnly)
            {
                DataRow currentRow = grdAllocation.GetDataRow(e.VisibleIndex);
                string func = string.Empty;
                string ID = string.Empty;
                string reAllocationFunc = string.Empty;

                DateTime allocationStartDate, allocationEndDate, prjStartDate, prjEndDate;

                if (DateTime.TryParse(Convert.ToString(currentRow[DatabaseObjects.Columns.AllocationStartDate]), out allocationStartDate) &&
                DateTime.TryParse(Convert.ToString(currentRow[DatabaseObjects.Columns.AllocationEndDate]), out allocationEndDate) &&
                DateTime.TryParse(projectStartDate, out prjStartDate) &&
                DateTime.TryParse(projectEndDate, out prjEndDate))
                {
                    if (!(prjStartDate <= allocationStartDate && prjEndDate >= allocationEndDate))
                    {
                        e.Row.ForeColor = Color.Red;
                        e.Row.ToolTip = "Resource allocation is outside the project start/end dates!";
                    }
                }



                string title = string.Format("{0} - Edit Item", formTitle);
                if (spListItem != null)
                {
                    if (UGITUtility.IfColumnExists(spListItem,DatabaseObjects.Columns.CRMProjectID) && spListItem[DatabaseObjects.Columns.CRMProjectID] != null)
                        title = string.Format("{0} {1}", Convert.ToString(spListItem[DatabaseObjects.Columns.CRMProjectID]), Convert.ToString(spListItem[DatabaseObjects.Columns.Title]));
                    else
                        title = string.Format("{0}", Convert.ToString(spListItem[DatabaseObjects.Columns.Title]));
                }


                if (currentRow.Table.Columns.Contains(DatabaseObjects.Columns.Id) && Convert.ToString(currentRow[DatabaseObjects.Columns.Id]) != string.Empty)
                {
                    ID = currentRow[DatabaseObjects.Columns.Id].ToString().Trim();
                }
                func = string.Format("openDialog('{0}','{1}','{2}')", UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, ID, ticketID)), "", UGITUtility.ReplaceInvalidCharsInURL(title));
                HtmlImage img = (HtmlImage)grdAllocation.FindRowCellTemplateControl(e.VisibleIndex, null, "editLink");
                img.Attributes.Add("onClick", func);

                reAllocationFunc = string.Format("openReAllocationDialog('{0}','{1}','{2}')", UGITUtility.GetAbsoluteURL(string.Format(absoluteUrlEdit, editParam, ID, ticketID)), "", UGITUtility.ReplaceInvalidCharsInURL(title));
                HtmlImage imgReAllocation = (HtmlImage)grdAllocation.FindRowCellTemplateControl(e.VisibleIndex, null, "reAllocationLink");
                imgReAllocation.Attributes.Add("onClick", reAllocationFunc);
            }
        }

        
    }
}
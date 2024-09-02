
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using System.Linq;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ApplModuleRoleMapEdit : UserControl
    {
        public int id { get; set; }
        public int AppId { get; set; }
        public string userID { get; set; }
        public string AppMode { get; set; }

        protected override void OnInit(EventArgs e)
        {
            pnlEditMode.Style.Add("display", "block");            
            txtRoleAssignee.UserTokenBoxAdd.ClientSideEvents.ValueChanged = "function(s, e){ LoadApplications(s, e); }";
            LoadApplicationMatrixControl();
            if (string.IsNullOrEmpty(userID))
            {
                pnlServiceMatrix.Visible = false;
            }
            else
            {
                pnlServiceMatrix.Visible = true;
                divNoUser.Visible = false;
            }
            base.OnInit(e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        private void SaveData()
        {
            ServiceMatrix serviceMatrix = (ServiceMatrix)pnlServiceMatrix.Controls[0];
            serviceMatrix.SaveState();           
            List<ServiceMatrixData> serviceMatricsData = serviceMatrix.GetSavedState();
            if (serviceMatrix != null)
            {
                if (!string.IsNullOrEmpty(AppMode) && AppMode.ToLower() == "add" && !string.IsNullOrEmpty(serviceMatrix.RoleAssignee))
                {
                    userID = serviceMatrix.RoleAssignee;
                 }
                if (!string.IsNullOrEmpty(userID)  && serviceMatricsData != null && serviceMatricsData.Count > 0)
                {
                    lblErrorMessage.Style.Add("display", "none");
                    ApplicationHelperManager appHelperManager = new ApplicationHelperManager(HttpContext.Current.GetManagerContext());
                    appHelperManager.UpdateApplicationSpecificAccess(userID, serviceMatricsData[0]);                   
                    uHelper.ClosePopUpAndEndResponse(Context, true);
                }
                else
                {
                    lblErrorMessage.Style.Add("display", "block");
                }
            }
        }
        private void LoadApplicationMatrixControl()
        {
            List<ServiceMatrixData> serviceMatrixDataList = null;
            string id = string.Format("QuestionId");          
            Control cntrl = pnlServiceMatrix.FindControl(id);
            if (cntrl == null)
            {
                ServiceMatrix serviceMatrix = (ServiceMatrix)Page.LoadControl("~/CONTROLTEMPLATES/uGovernIT/Services/ServiceMatrix.ascx");
                serviceMatrix.ParentControl = "Application";
                if (serviceMatrixDataList == null)
                {
                    serviceMatrixDataList = new List<ServiceMatrixData>();
                }
                ServiceMatrixData serviceMatrixData = new ServiceMatrixData();
                if (AppId > 0)
                {
                    DataRow spListItem = GetTableDataManager.GetDataRow(DatabaseObjects.Tables.Applications, DatabaseObjects.Columns.ID, AppId)[0]; // SPListHelper.GetSPListItem(DatabaseObjects.Lists.Applications, AppId);
                    if (spListItem != null)
                    {
                        serviceMatrixData.Name = Convert.ToString(spListItem[DatabaseObjects.Columns.Title]);
                        serviceMatrixData.ID = Convert.ToString(AppId);
                    }
                }
                txtRoleAssignee.Visible = true;
                lblRoleAssignee.Visible = true;
                divNoUser.Visible = true;
                if (!string.IsNullOrEmpty(userID))//edit mode
                {
                    serviceMatrixData.RoleAssignee = Convert.ToString(userID);
                    txtRoleAssignee.Visible = false;
                    lblRoleAssignee.Visible = false;
                    divNoUser.Visible = false;
                }
                else
                {
                    string assignee = Convert.ToString(Request[hdnAssignee.UniqueID]);
                    if (!string.IsNullOrEmpty(assignee))
                    {
                        userID = assignee;
                        serviceMatrixData.RoleAssignee = Convert.ToString(userID);
                    }
                }
                serviceMatrixData = serviceMatrix.GetAppRolesModules(ref serviceMatrixData);
                if (serviceMatrixData.lstRowsNames != null&&  serviceMatrixData.lstRowsNames.Count == 0)
                {                  
                    ApplicationRole spitem = new ApplicationRole();
                    spitem.Title = "User";
                    spitem.APPTitleLookup = AppId;
                    ApplicationRoleManager appRoleMGR = new ApplicationRoleManager(HttpContext.Current.GetManagerContext());
                    appRoleMGR.Insert(spitem);  
                    LoadApplicationMatrixControl();
                    return;
                }
                if (serviceMatrix != null)
                {
                    List<int> lstApplicationIds = new List<int>();
                    lstApplicationIds.Add(AppId);
                    if (lstApplicationIds.Count > 0)
                        serviceMatrix.Applications = lstApplicationIds;
                    serviceMatrix.AccessType = "add";
                    if (!string.IsNullOrEmpty(userID))
                    {
                        serviceMatrix.RoleAssignee = Convert.ToString(userID);
                    }
                    serviceMatrix.IsReadOnly = false;
                    serviceMatrix.ControlIDPrefix = string.Format("QuestionId");
                    serviceMatrix.IsNoteEnabled = false;
                    serviceMatrix.ID = string.Format("QuestionId");
                    pnlServiceMatrix.Controls.Add(serviceMatrix);
                }
            }
        }

        protected void btnGetAccess_Click(object sender, EventArgs e)
        {
            userID = txtRoleAssignee.GetValues();
            userID = Convert.ToString(hdnSelectedUser["Id"]);
            if (!string.IsNullOrEmpty(userID))
            {
                pnlServiceMatrix.Visible = true;
                divNoUser.Visible = false;
                ServiceMatrix serviceMatrix = (ServiceMatrix)pnlServiceMatrix.Controls[0];
                if (serviceMatrix != null)
                {
                    serviceMatrix.RoleAssignee = Convert.ToString(userID);
                    serviceMatrix.Reload();
                    hdnAssignee.Value = Convert.ToString(userID);
                }
            }
            else
            {
                pnlServiceMatrix.Visible = false;
                divNoUser.Visible = true;
            }

        }
        private int GetRoleAssignee()
        {
            int UserId = 0;
            UserProfileManager userManager = HttpContext.Current.GetUserManager();
            List<UserProfile> userPRPGroupList = userManager.GetUserInfosById(txtRoleAssignee.GetValues());
            foreach (UserProfile userEntity in userPRPGroupList)
            {
                //UserId = userEntity.id;
                //if (userManager.CheckUserIsGroup(userEntity.Id))
                //{

                //}
                //else
                //{

                //}
                //// int userID = 0;
                //PickerEntity entity = (PickerEntity)userEntity;
                //if (entity.EntityData["PrincipalType"].ToString() == "User")
                //{
                //    UserProfile user = UserProfile.LoadByLoginName(Convert.ToString(entity.EntityData["AccountName"]));
                //    if (user != null)
                //    {
                //        UserId = user.ID;
                //    }
                //    userRoleAssignee.LookupId = UserId;
                //}
                //else
                //{
                //    int.TryParse(entity.EntityData["SPGroupID"].ToString(), out UserId);
                //    UserId = userRoleAssignee.LookupId = UserId;
                //}
            }
            return UserId;
        }

    }
}

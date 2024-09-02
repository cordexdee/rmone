using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using DevExpress.Web;
using System.Collections;
using System.Data;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Web;
using uGovernIT.Utility.Entities;
using Microsoft.AspNet.Identity.Owin;
using uGovernIT.Util.Log;

namespace uGovernIT.Web
{
    public partial class TicketReAssignment : UserControl
    {
        //public string Module { get; set; }
        public string TicketId { get; set; }
        DataRow spLItem;
        DataRow ticket;
        protected const string Varies = "<Value Varies>";
        UserProfile userProfile = null;
        UserProfileManager UserManager;
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager ObjModuleViewManager = new ModuleViewManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            cvUser.ControlToValidate = pePRP.UserTokenBoxAdd.ID;
            userProfile = HttpContext.Current.CurrentUser();
            UserManager = HttpContext.Current.GetOwinContext().Get<UserProfileManager>();

            if (!IsPostBack)
            {
                TicketValidationCheck();

                BindSkill(cbSkill, context.TenantID);
                cbSkill.SelectedIndex = 0;

                List<string> ticketIds = TicketId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (ticketIds.Count > 1)
                {
                    dvPRP.Style.Add("display", "block");
                    dvPRPEditor.Style.Add("display", "none");
                    lblPRP.Text = Server.HtmlEncode(Varies);
                    hndPRP.Value = Varies;

                    dvORP.Style.Add("display", "block");
                    dvORPEditor.Style.Add("display", "none");
                    lblORP.Text = Server.HtmlEncode(Varies);
                    hdnORP.Value = Varies;

                    //cvUser.Visible
                }
                else
                {

                    spLItem = uHelper.getModuleItemByTicketID(ticketIds[0], context.TenantID);
                    if (spLItem != null)
                    {
                        //ticket = GetTableDataManager.GetTableData(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), DatabaseObjects.Columns.TicketId+"='"+ ticketIds[0]+"'").Rows[0];
                        ticket = GetTableDataManager.GetTableData(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), $"{DatabaseObjects.Columns.TicketId}='{ticketIds[0]}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Rows[0];
                    }


                    if (ticket != null)
                    {
                        try
                        {
                            peORP.SetValues(Convert.ToString(ticket[DatabaseObjects.Columns.TicketORP]));
                            pePRP.SetValues(Convert.ToString(ticket[DatabaseObjects.Columns.TicketPRP]));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (hndPRP.Value == Varies)
            {
                dvPRP.Style.Add("display", "block");
                dvPRPEditor.Style.Add("display", "none");
                lblPRP.Text = Server.HtmlEncode(Varies);
            }
            else
            {
                dvPRP.Style.Add("display", "none");
                dvPRPEditor.Style.Add("display", "block");
                lblPRP.Text = Server.HtmlEncode(Varies);

                // cvUser.Visible = true;
            }

            if (hdnORP.Value == Varies)
            {
                dvORP.Style.Add("display", "block");
                dvORPEditor.Style.Add("display", "none");
                lblORP.Text = Server.HtmlEncode(Varies);
            }
            else
            {
                dvORP.Style.Add("display", "none");
                dvORPEditor.Style.Add("display", "block");
                lblORP.Text = Server.HtmlEncode(Varies);
            }
            base.OnPreRender(e);
        }

        public class ObjUserSkill
        {
            public string SkillName { get; set; }
            public Int32 SkillID { get; set; }
        }

        public static void BindSkill(ASPxComboBox ddl, string TenantID)
        {
            ddl.Items.Clear();
            List<ObjUserSkill> skills = new List<ObjUserSkill>();

            DataTable userSkillList = GetTableDataManager.GetTableData(DatabaseObjects.Tables.UserSkills, $"{DatabaseObjects.Columns.TenantID} = '{TenantID}'");
          
            DataRow[] userSkillCol = userSkillList.Select();
            if (userSkillCol != null)
            {
                foreach (DataRow item in userSkillCol)
                {
                    skills.Add(new ObjUserSkill { SkillName = Convert.ToString(item[DatabaseObjects.Columns.Title]), SkillID =Convert.ToInt32(item[DatabaseObjects.Columns.ID]) });
                }
            }

            ddl.DataSource = skills;
            ddl.TextField = "SkillName";
            ddl.ValueField = "SkillID";
            ddl.DataBind();
        }
        protected void btnPRP_Click(object sender, ImageClickEventArgs e)
        {
            List<UserProfile> selectedUsers = new List<UserProfile>();
            if (((ImageButton)sender).ID == "btnPRP")
            {
                hndUserType.Value = "PRP";
                selectedUsers = UserManager.GetUserInfosById(pePRP.GetValues());
            }
            else
            {
                hndUserType.Value = "ORP";
                selectedUsers = UserManager.GetUserInfosById(peORP.GetValues());
            }

            lblSkillErrorMessage.Visible = false;


            foreach (var user in selectedUsers)
                gridAllocation.Selection.SelectRowByKey(user.Id);

            grdSkill.ShowOnPageLoad = true;
            gridAllocation.DataBind();

            if (gridAllocation.VisibleRowCount > 0)
            {
                dxUpdateSkill.Visible = true;
            }
            else
            {
                dxUpdateSkill.Visible = false;
            }
        }


        private List<Allocation> GetSkillUserData()
        {
            List<Allocation> listAllocation = new List<Allocation>();

            List<string> userIds = new List<string>();
            List<UserProfile> lstUserProfile = UserManager.GetUsersProfile();

            string selectedSkill = cbSkill.SelectedItem == null ? "-1" : cbSkill.SelectedItem.Text;

            foreach (UserProfile user in lstUserProfile)
            {
                if (user.Skills != null)
                {
                    string[] skills = UGITUtility.SplitString(user.Skills, Constants.Separator6); // user.Skills.Select(x => x.Value).ToArray();
                    foreach (string item in skills)
                    {
                        if (item.Contains(selectedSkill) || selectedSkill.Contains(item))
                        {
                            userIds.Add(user.Id);
                        }
                    }
                }
            }
            userIds = userIds.Distinct().ToList();

            foreach (string userID in userIds)
            {
                Allocation dataAllocation = new Allocation();
                double totalPctAllocation = 0.0;//ResourceAllocation.AllocationPercentage(userID, 4);

                dataAllocation.ResourceId = userID;
                UserProfile profile = UserManager.GetUserById(userID);
                if (profile != null)
                    dataAllocation.Resource = profile.Name;
                dataAllocation.UserSkill = cbSkill.SelectedItem.Text;
                dataAllocation.UserFullAllocation = totalPctAllocation;
                dataAllocation.FullAllocation = CreateAllocationBar(totalPctAllocation);

                listAllocation.Add(dataAllocation);
            }

            return listAllocation.OrderBy(x => x.UserFullAllocation).ToList();
        }

        protected void dxUpdateSkill_Click(object sender, EventArgs e)
        {
            //var selectedResource = gridAllocation.GetSelectedFieldValues(DatabaseObjects.Columns.Resource);
            var selectedResource = gridAllocation.GetSelectedFieldValues(DatabaseObjects.Columns.ResourceId);
            string users = string.Empty;

            foreach (var usrItm in selectedResource)
            {
                //SPUser newUser = SPContext.Current.Web.EnsureUser(usrItm.ToString());
                UserProfile newUser = UserManager.GetUserById(Convert.ToString(usrItm));
                if (!string.IsNullOrEmpty(newUser.Id))
                {
                    users += newUser.Id + ",";
                }
            }
            if (!string.IsNullOrWhiteSpace(users) && users.Length>1)
            {
                users= users.Remove(users.LastIndexOf(","), 1);
            }

            if (selectedResource.Count != 0)
            {
                if (hndUserType.Value == "PRP")
                {
                    pePRP.SetValues(users);
                }
                else
                {
                    peORP.SetValues(users);
                }
            }
            else
            {
                pePRP.SetValues("");
            }
            grdSkill.ShowOnPageLoad = false;
        }

        private string CreateAllocationBar(double pctAllocation)
        {
            StringBuilder bar = new StringBuilder();
            double percentage = 0;
            string progressBarClass = "progressbar";
            string empltyProgressBarClass = "emptyProgressBar";
            percentage = pctAllocation;
            if (percentage > 100)
            {
                progressBarClass = "progressbarhold";
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:95%;text-align:center;top:1px;'>{2}%</strong><div class='{0}' style='float:left; width:95%;'><div class='{1}' style='float:left; width:100%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }
            else
            {
                bar.AppendFormat("<div style='position:relative;'><strong style='position:absolute;font-size:9px;left:2px;width:95%;text-align:center;top:1px;'>{2}%</strong><div class='{0}' style='float:left; width:95%;'><div class='{1}' style='float:left; width:{2}%;'><b>&nbsp;</b></div></div></div>", empltyProgressBarClass, progressBarClass, percentage);
            }

            return bar.ToString();
        }

        protected void gridAllocation_DataBinding(object sender, EventArgs e)
        {
            if (gridAllocation.DataSource == null)
            {
                gridAllocation.DataSource = GetSkillUserData();
            }
        }

    
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!cvUser.IsValid)
            {
                cvUser.ErrorMessage = "Single User Only";
                return;
            }

            if (!string.IsNullOrEmpty(TicketId))
            {
                List<string> ticketIds = TicketId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (ticketIds.Count > 0)
                {
                    int moduleId = uHelper.getModuleIdByTicketID(HttpContext.Current.GetManagerContext(),ticketIds[0]);
                    Ticket ticketRequest = new Ticket(context,moduleId);
                   
                    UGITModule moduleDetail = ObjModuleViewManager.GetByID(moduleId);
                    string moduleName=moduleDetail.ModuleName;
                    //DataRow[] moduleStagesRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, DatabaseObjects.Columns.ModuleNameLookup+"='"+ moduleName+"'").Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();
                    DataRow[] moduleStagesRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleNameLookup}='{moduleName}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();

                    foreach (string ticketId in ticketIds)
                    {
                        //spLItem = uHelper.getModuleItemByTicketID(ticketId);
                        spLItem = uHelper.getModuleItemByTicketID(ticketId, context.TenantID);
                        if (spLItem != null)
                        {
                            //ticket = GetTableDataManager.GetTableData(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), DatabaseObjects.Columns.TicketId+"='"+ ticketId+"'").Rows[0];
                            ticket = GetTableDataManager.GetTableData(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), $"{DatabaseObjects.Columns.TicketId}='{ticketId}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Rows[0];
                        }

                        if (ticket != null)
                        {
                            // SPFieldUserValueCollection selectedORP = new SPFieldUserValueCollection();
                            List<string> selectedORP = new List<string>();
                            try
                            {
                                if (hndPRP.Value != Varies)
                                    ticket[DatabaseObjects.Columns.TicketPRP] = pePRP.GetValues();
                                if (hdnORP.Value != Varies)
                                    ticket[DatabaseObjects.Columns.TicketORP] = peORP.GetValues();

                                string currentStage = UGITUtility.SplitString(ticket[DatabaseObjects.Columns.ModuleStepLookup].ToString(), Constants.Separator, 1);
                                //SPFieldLookupValue currentStageLookup = new SPFieldLookupValue(Convert.ToString(ticket[DatabaseObjects.Columns.ModuleStepLookup]));
                                DataRow currentStageRow = null;
                                if (!String.IsNullOrEmpty(Convert.ToString(ticket[DatabaseObjects.Columns.ModuleStepLookup])))
                                {
                                    currentStageRow = moduleStagesRow.FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.Id) == Convert.ToInt64(ticket[DatabaseObjects.Columns.ModuleStepLookup]));
                                }
                                else
                                {
                                    break;
                                }
                                Dictionary<string, string> dicCustomProperties = UGITUtility.GetCustomProperties(Convert.ToString(currentStageRow[DatabaseObjects.Columns.CustomProperties]), Constants.Separator);

                                bool IsSelfAssign = false;
                                if (dicCustomProperties.ContainsKey(CustomProperties.SelfAssign))
                                {
                                    IsSelfAssign = Convert.ToBoolean(dicCustomProperties[CustomProperties.SelfAssign]);
                                }


                                ticket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(context,Convert.ToString(ticket[DatabaseObjects.Columns.TicketStageActionUserTypes]), ticket);
                                if (IsSelfAssign)
                                {
                                    //ticketRequest.Approve(moduleId, ticket, true);
                                    ticketRequest.Approve(userProfile, ticket, true);
                                }
                                //ticket.Update();

                                ticketRequest.CommitChanges(ticket,"");
                                ticketRequest.SendEmailToActionUsers(Convert.ToString(ticket[DatabaseObjects.Columns.ModuleStepLookup]), ticket, moduleName,"",null);
                                //Spdelta 11(1.Enhancements to TicketEvents tracking for SVC (supports improved Lifecycle Tab information).2.Database changes for ticket events of SVC.3.Code configuration to display lifecyle tab for Svc.)
                                //List<UserProfile> newPRPValue = context.UserManager.GetUserInfosById(Convert.ToInt32(entity.LookupId), entity.User.LoginName);
                                string newPRPValue = HttpContext.Current.GetManagerContext().CurrentUser.Id;
                                List<UserProfile> oldPRPValue = context.UserManager.GetUserInfosById(Convert.ToString(ticket[DatabaseObjects.Columns.TicketPRP]));
                                string oldPRPValues = string.Join<string>(",", oldPRPValue.Select(x => x.Id));
                                LifeCycleStage currentLifeCycleStage = ticketRequest.GetTicketCurrentStage(ticket);
                                if (oldPRPValues != newPRPValue)
                                {
                                    TicketEventManager eventHelper = new TicketEventManager(context, ticketRequest.Module.ModuleName, ticketId);
                                    eventHelper.LogEvent(Constants.TicketEventType.Assigned, currentLifeCycleStage,  false, "", "", null, newPRPValue );
                                }
                                //
                            }
                            catch (Exception ex)
                            {
                                ULog.WriteException(ex, "Error re-assigning ticket");
                            }

                            //Update change after updating ticket
                            //uGITCache.ModuleDataCache.UpdateOpenTicketsCache(Convert.ToInt16(spLItem[DatabaseObjects.Columns.ModuleId]), ticket);
                        }
                    }
                }
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void cvUser_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (hndPRP.Value != Varies)
            {
                ArrayList userList = new ArrayList(pePRP.GetValuesAsList());
                if (userList.Count == 1)
                {
                    args.IsValid = true;
                }
                else
                    args.IsValid = false;
            }
            else
                args.IsValid = true;
        }

        private void TicketValidationCheck()
        {
            bool IsAdmin = UserManager.IsUGITSuperAdmin(userProfile) || UserManager.IsTicketAdmin(userProfile);

            List<string> ticketIds = TicketId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (ticketIds.Count > 0)
            {
                string moduleName;
                DataRow[] moduleStagesRow;

                int moduleId = uHelper.getModuleIdByTicketID(HttpContext.Current.GetManagerContext(),ticketIds[0]);

                UGITModule moduleDetail = ObjModuleViewManager.GetByID(moduleId);
                moduleName = moduleDetail.ModuleName;
                //moduleStagesRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, DatabaseObjects.Columns.ModuleNameLookup+"='"+ moduleName+"'").Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();
                moduleStagesRow = GetTableDataManager.GetTableData(DatabaseObjects.Tables.ModuleStages, $"{DatabaseObjects.Columns.ModuleNameLookup}='{moduleName}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Select().OrderBy(x => x.Field<int>(DatabaseObjects.Columns.ModuleStep)).ToArray();

                foreach (string ticketId in ticketIds)
                {
                    //spLItem = uHelper.getModuleItemByTicketID(ticketId);
                    spLItem = uHelper.getModuleItemByTicketID(ticketId, context.TenantID);
                    if (spLItem != null)
                    {
                        //ticket = GetTableDataManager.GetTableData(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), DatabaseObjects.Columns.TicketId+"='"+ticketId+"'").Rows[0];
                        ticket = GetTableDataManager.GetTableData(Convert.ToString(spLItem[DatabaseObjects.Columns.ModuleTicketTable]), $"{DatabaseObjects.Columns.TicketId}='{ticketId}' and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").Rows[0];
                    }

                    if (ticket != null)
                    {
                        string currentStage = UGITUtility.SplitString(ticket[DatabaseObjects.Columns.ModuleStepLookup].ToString(), Constants.Separator, 1);
                       // SPFieldLookupValue currentStageLookup = new SPFieldLookupValue();
                        DataRow currentStageRow = null;
                        if (!String.IsNullOrEmpty(Convert.ToString(ticket[DatabaseObjects.Columns.ModuleStepLookup])))
                        {
                            currentStageRow = moduleStagesRow.FirstOrDefault(x => x.Field<long>(DatabaseObjects.Columns.Id) == Convert.ToInt64(ticket[DatabaseObjects.Columns.ModuleStepLookup]));
                        }    
                        else
                        {
                            break;
                        }
                        Dictionary<string, string> dicCustomProperties = UGITUtility.GetCustomProperties(Convert.ToString(currentStageRow[DatabaseObjects.Columns.CustomProperties]), Constants.Separator);
                       
                        bool IsSelfAssign = false;
                        if (dicCustomProperties.ContainsKey(CustomProperties.SelfAssign))
                        {
                            IsSelfAssign = Convert.ToBoolean(dicCustomProperties[CustomProperties.SelfAssign]);
                        }
                        if (Convert.ToString(currentStageRow[DatabaseObjects.Columns.StageTypeChoice]) != "Assigned" && !IsSelfAssign)

                            //if (Convert.ToString(currentStageRow[DatabaseObjects.Columns.StageTypeLookup]) != "Assigned" && !IsSelfAssign)
                        {
                            if (!UGITUtility.StringToBoolean(Convert.ToString(currentStageRow[DatabaseObjects.Columns.AllowReassignFromList])))
                            {
                                tblMain.Visible = false;
                                tblErrorMessage.Visible = true;
                                lblErrorMessage.Text = "The selected tickets cannot be re-assigned in the current stage";
                                break;
                            }
                        }
                       // if (!uHelper.IsActionUser(ticket, moduleName, SPContext.Current.Web) && !IsAdmin)
                            if (!UserManager.IsActionUser(ticket,userProfile) && !IsAdmin)
                        {
                            tblMain.Visible = false;
                            tblErrorMessage.Visible = true;
                            lblErrorMessage.Text = "You don't have permission to re-assign the selected tickets";
                            break;
                        }
                    }
                }
            }
        }
    }

    
}

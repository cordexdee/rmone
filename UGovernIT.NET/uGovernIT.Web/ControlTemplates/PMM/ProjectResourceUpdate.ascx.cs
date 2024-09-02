using System;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ProjectResourceUpdate : UserControl
    {
        public string Module { get; set; }
        public string PublicID { get; set; }
        public int ItemID { get; set; }
        List<LookupValue> selectedUsers = new List<LookupValue>();
        List<RResourceAllocation> projectAllocations = null;
        RResourceAllocation resourceAllocation = null;
        bool autoCreateRMMProjectAllocation;
        public bool ReadOnly;

        ConfigurationVariableManager ConfigVariableManager;
        ApplicationContext AppContext;
        ResourceAllocationManager ResourceAllocationMGR;
        
        protected override void OnInit(EventArgs e)
        {
            AppContext = HttpContext.Current.GetManagerContext();
            ConfigVariableManager = new ConfigurationVariableManager(AppContext);
            ResourceAllocationMGR = new ResourceAllocationManager(AppContext);
            autoCreateRMMProjectAllocation = ConfigVariableManager.GetValueAsBool(ConfigConstants.AutoCreateRMMProjectAllocation);

            projectAllocations = ResourceAllocationMGR.LoadByWorkItem(Constants.RMMLevel1PMMProjectsType, PublicID, null, 4, false, true);
            resourceAllocation = projectAllocations.FirstOrDefault(x => x.ID == ItemID);
            if (!IsPostBack)
            {
                if (resourceAllocation != null)
                {
                    lbResource.Visible = true;
                    panelEditResource.Visible = false;
                    lbResource.Text = AppContext.UserManager.GetUserInfoById(resourceAllocation.Resource).Name;
                    txtAllocation.Text = resourceAllocation.PctAllocation.ToString();
                    startDate.Date = UGITUtility.StringToDateTime( resourceAllocation.AllocationStartDate);
                    endDate.Date = UGITUtility.StringToDateTime( resourceAllocation.AllocationEndDate);
                }
                else
                {
                    List<UserProfile> userList = new List<UserProfile>();
                    string usersName = string.Empty;
                    if (!string.IsNullOrEmpty(Request["users"]))
                    {
                        usersName = Uri.UnescapeDataString(Request["users"]);
                        List<string> users = UGITUtility.ConvertStringToList(usersName, ",");

                        foreach (string u in users)
                        {
                            UserProfile uf = AppContext.UserManager.GetUserById(u);
                            if (uf != null)
                            {
                                userList.Add(uf);
                                selectedUsers.Add(new LookupValue(uf.Id, uf.Name));
                            }
                        }
                    }
                    
                    DataRow project = Ticket.GetCurrentTicket(AppContext, Module, PublicID);
                    DateTime sstartDate = DateTime.MinValue;
                    DateTime sendDate = DateTime.MinValue;
                    if (UGITUtility.IsSPItemExist(project, DatabaseObjects.Columns.TicketActualStartDate))
                        sstartDate = (DateTime)project[DatabaseObjects.Columns.TicketActualStartDate];

                    if (UGITUtility.IsSPItemExist(project, DatabaseObjects.Columns.TicketActualCompletionDate))
                        sendDate = (DateTime)project[DatabaseObjects.Columns.TicketActualCompletionDate];

                    startDate.Date = sstartDate;
                    endDate.Date = sendDate;
                    txtAllocation.Text = "100";

                    if (selectedUsers.Count > 0 && autoCreateRMMProjectAllocation)
                    {
                        UpdateAllocation();
                        lbInformationMsg.Text = string.Format("No allocation was found for {0} for this project. A tentative project allocation has been created as shown below, please edit if needed.",
                            string.Join(", ", selectedUsers.Select(x => x.Value).ToArray()));
                        peditResource.SetValues(usersName);
                    }
                }
            }
            else
            {
                if (resourceAllocation != null)
                {
                    selectedUsers.Add(new LookupValue(resourceAllocation.Resource, resourceAllocation.Resource));
                }
            }

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnInformation.Set("Module", Module);
            hdnInformation.Set("PublicID", PublicID);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            UpdateAllocation();
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context);
        }

        private void UpdateAllocation()
        {
            foreach (LookupValue userVal in selectedUsers)
            {
                bool isNewAllocation = false;
                RResourceAllocation rAllocation = projectAllocations.FirstOrDefault(x => x.Resource == userVal.ID);
                if (rAllocation == null)
                {
                    rAllocation = new RResourceAllocation();
                    rAllocation.ResourceWorkItems = new ResourceWorkItems(userVal.ID);
                    rAllocation.ResourceWorkItems.WorkItemType = Module;
                    rAllocation.ResourceWorkItems.WorkItem = PublicID;
                    rAllocation.ResourceWorkItems.SubWorkItem = string.Empty;
                    rAllocation.Resource = userVal.ID;
                    isNewAllocation = true;
                }

                int pctAllocation = 0;
                int.TryParse(txtAllocation.Text.Trim(), out pctAllocation);
                rAllocation.PctAllocation = pctAllocation;
                rAllocation.AllocationStartDate = startDate.Date;
                rAllocation.AllocationEndDate = endDate.Date;
                ResourceAllocationMGR.Save(rAllocation);

                DataRow project = Ticket.GetCurrentTicket(AppContext, Module, PublicID);
                
                string mailFooter = uHelper.GetTicketDetailsForEmailFooter(AppContext, project, "PMM", true, false);

                //Send mail about new and update allocation
                if (isNewAllocation)
                {
                    UserProfile userObj = AppContext.UserManager.LoadById(rAllocation.Resource);
                    if (userObj != null && !string.IsNullOrEmpty(userObj.Email))
                    {
                        StringBuilder taskEmail = new StringBuilder();
                        taskEmail.AppendFormat("A new project allocation has been created for you from <b>{0} to {1}</b> for project <b>{2}: {3}</b>.<br/>",
                                                uHelper.GetDateStringInFormat(AppContext, UGITUtility.StringToDateTime(rAllocation.AllocationStartDate), false), 
                                                uHelper.GetDateStringInFormat(AppContext, UGITUtility.StringToDateTime(rAllocation.AllocationEndDate), false),
                                                UGITUtility.ObjectToString(project[DatabaseObjects.Columns.TicketId]), UGITUtility.ObjectToString( project[DatabaseObjects.Columns.Title]));
                        taskEmail.Append(mailFooter);
                        MailMessenger mailMessage = new MailMessenger(AppContext);
                        mailMessage.SendMail(userObj.Email, string.Format("New Project Allocation in {0}: {1}", project[DatabaseObjects.Columns.TicketId], UGITUtility.ObjectToString(project[DatabaseObjects.Columns.Title])),
                                             "", taskEmail.ToString(), true, new string[] { }, true);
                    }

                    if (userObj != null && !string.IsNullOrEmpty(userObj.ManagerID))
                    {
                        UserProfile userManagerObj = AppContext.UserManager.GetUserInfoById(userObj.ManagerID);
                        if (userManagerObj != null)
                        {
                            StringBuilder taskEmail = new StringBuilder();
                            taskEmail.AppendFormat("Project Manager <b>{0}</b> has requested a new allocation for <b>{1}</b> from <b>{2} to {3}</b> for project <b>{4}: {5}.</b>",
                                                    AppContext.CurrentUser.Name, userObj.Name,
                                                    uHelper.GetDateStringInFormat(AppContext, UGITUtility.StringToDateTime(rAllocation.AllocationStartDate), false), 
                                                    uHelper.GetDateStringInFormat(AppContext, UGITUtility.StringToDateTime(rAllocation.AllocationEndDate), false),
                                                    UGITUtility.ObjectToString(project[DatabaseObjects.Columns.TicketId]), UGITUtility.ObjectToString(project[DatabaseObjects.Columns.Title]));
                            taskEmail.Append("<br/><br/>");
                            taskEmail.AppendFormat("A tentative allocation has already been created, please edit it as needed.<br/>");
                            taskEmail.Append(mailFooter);

                            MailMessenger mailMessage = new MailMessenger(AppContext);
                            mailMessage.SendMail(userManagerObj.Email,
                                                string.Format("New Project Allocation for {0} in {1}: {2}", userObj.Name, project[DatabaseObjects.Columns.TicketId], UGITUtility.ObjectToString(project[DatabaseObjects.Columns.Title])),
                                                "", taskEmail.ToString(), true, new string[] { }, true);
                        }
                    }
                }

                if (rAllocation.ResourceWorkItemLookup > 0)
                {
                    
                    //Start Thread to update rmm summary list w.r.t current workitem
                    ThreadStart threadStartMethod = delegate () { RMMSummaryHelper.UpdateRMMSummaryAndMonthDistribution(AppContext, rAllocation.ResourceWorkItemLookup); };
                    Thread sThread = new Thread(threadStartMethod);
                    sThread.IsBackground = true;
                    sThread.Start();
                }
            }
        }

        protected void cvPeditResource_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //Creates uservaluecollection object from peoplepicker control
            //if (peditResource.Visible && peditResource.Accounts.Count > 0)
            //{
            //    for (int i = 0; i < peditResource.Accounts.Count; i++)
            //    {
            //        PickerEntity entity = (PickerEntity)peditResource.Entities[i];
            //        if (entity != null && entity.Key != null)
            //        {
            //            SPUser user = UserProfile.GetUserByName(entity.Key, SPPrincipalType.User);
            //            if (user != null)
            //            {
            //                selectedUsers.Add(new LookupValue(user.ID, user.Name));
            //            }
            //        }
            //    }
            //}

            //SPFieldUserValueCollection userValueCollection = peditResource.GetUserValueCollection();
            //if (userValueCollection != null && userValueCollection.Count > 0)
            //{
            //    foreach (SPFieldUserValue entity in userValueCollection)
            //    {
            //        SPUser user = UserProfile.GetUserByName(entity.LoginName, SPPrincipalType.User);
            //        if (user != null)
            //        {
            //            selectedUsers.Add(new LookupValue(user.ID, user.Name));
            //        }
            //    }
            //}


            //if (peditResource != null && peditResource.GetUserValueCollection().Count <= 0)
            //{
            //    cvPeditResource.ErrorMessage = "Please enter resource";
            //    args.IsValid = false;
            //}

            //List<string> exitingAllocations = new List<string>();
            //if (projectAllocations != null && projectAllocations.Count > 0)
            //{
            //    foreach (LookupValue lk in selectedUsers)
            //    {
            //        ResourceAllocation row = projectAllocations.AsEnumerable().FirstOrDefault(x => x.ResourceId == lk.ID);
            //        if (row != null)
            //        {
            //            exitingAllocations.Add(lk.Value);
            //        }
            //    }
            //}

            //if (exitingAllocations.Count > 0)
            //{
            //    cvPeditResource.ErrorMessage = string.Format("\"{0}\" already assigned in project", string.Join("; ", exitingAllocations.ToArray()));
            //    args.IsValid = false;
            //}
        }

        protected void cvEndDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //if (startDate.IsDateEmpty)
            //    startDate.SelectedDate = DateTime.Now.Date;
            //if (endDate.IsDateEmpty)
            //    endDate.SelectedDate = DateTime.Now.Date;

            //if (startDate.SelectedDate.Date > endDate.SelectedDate.Date)
            //{
            //    args.IsValid = false;
            //}
        }
    }
}

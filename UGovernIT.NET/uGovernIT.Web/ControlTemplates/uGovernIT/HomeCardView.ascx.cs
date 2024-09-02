using DevExpress.Web;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;
namespace uGovernIT.Web
{
    public partial class HomeCardView : System.Web.UI.UserControl
    {
        private ApplicationContext _context = null;
        //private ConfigurationVariableManager _configurationVariableManager;

        //protected string TicketTemplateURL = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=TicketTemplatelist");

        public string viewTicketsPath;
        public string viewTasksPath;
        public string viewRMMPath;
        public string viewTasksPage;
        public string viewGridPath;
        public string viewProjAllocPath;
        public string viewResourceUtilizationPath;
        public string UserRole;
        public string UserType { get; set; }

        public TicketStatus MTicketStatus { get; set; }

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
            _context = HttpContext.Current.GetManagerContext();
            //_configurationVariableManager = new ConfigurationVariableManager(_context);
            //viewTicketsPath = UGITUtility.GetAbsoluteURL(_configurationVariableManager.GetValue("FilterTicketsPageUrl"));

            viewTicketsPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=homedashboardtickets");
            viewTasksPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=homedashboardtasks");
            viewRMMPath = UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=customgroupsandusersinfo");
            viewProjAllocPath = UGITUtility.GetAbsoluteURL(string.Format("/layouts/uGovernIT/DelegateControl.aspx?frameObjId={0}&control=resourceallocationgrid&AllocationViewType=RMMAllocation&isdlg=1&isudlg=1", Guid.NewGuid()));
            viewResourceUtilizationPath = UGITUtility.GetAbsoluteURL(string.Format("/layouts/ugovernit/delegatecontrol.aspx?frameObjId={0}&control=resourceavailability&AllocationViewType=RMMAllocation&isdlg=1&isudlg=1", Guid.NewGuid()));
            viewTasksPage = UGITUtility.GetAbsoluteURL("/Pages/HomeTasks");
            viewGridPath = UGITUtility.GetAbsoluteURL("/Pages/HomeGrid");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UserDashboardCardView.SettingsBehavior.EnableCustomizationWindow = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            BindAspxGridView();

            ////if (!string.IsNullOrEmpty(ModuleName))
            ////{
            ////    string title = "My Projects";
            ////    ugitUserProfileCardView.OnClientClick = string.Format("window.parent.UgitOpenPopupDialog('{0}', 'Module={5}&Status={4}&UserType={2}&showalldetail=false;showglobalfilter=true', '{1}', 90, 90, 0, '{3}')", viewTicketsPath, title, UserType, Server.UrlEncode(Request.Url.AbsolutePath), MTicketStatus.ToString(), ModuleName);
            ////}
            ////else
            ////{
            ////    ugitUserProfileCardView.OnClientClick = string.Format("window.parent.UgitOpenPopupDialog('{0}', 'Module=All&Status={4}&UserType={2}&showalldetail=false;showglobalfilter=true', '{1}', 90, 90, 0, '{3}')", viewTicketsPath, title, UserType, Server.UrlEncode(Request.Url.AbsolutePath), MTicketStatus.ToString());
            ////}

        }

        private void BindAspxGridView()
        {
            UserDashboardCardView.DataSource = getDataSource("Card View");
            UserDashboardCardView.DataBind();
        }

        public DataTable getDataSource(string loadGrid)
        {
            //DataTable CardData = new DataTable();
            var dtCardData = new DataTable();
            dtCardData.Columns.Add("Picture", typeof(string));
            dtCardData.Columns.Add("Description", typeof(string));

            DataRow row1 = dtCardData.NewRow();
            DataRow row2 = dtCardData.NewRow();
            DataRow row3 = dtCardData.NewRow();
            DataRow row4 = dtCardData.NewRow();
            DataRow row5 = dtCardData.NewRow();
            DataRow row6 = dtCardData.NewRow();
            DataRow row7 = dtCardData.NewRow();

            //string query = "";

            if (loadGrid == "Card View")
            {
                ////var ticketManager = new TicketManager(HttpContext.Current.GetManagerContext());
                ////TicketStore ticketStore = new TicketStore(HttpContext.Current.GetManagerContext());
                //// string query = string.Format("{0} = {1} ", "Id", 0);
                ////long ProjectCount = ticketManager.GetAllTicketsCount("CRMProject");

                _context = HttpContext.Current.GetManagerContext();

                var userId = _context.CurrentUser.Id;
                var userProfileManager = new UserProfileManager(_context);
                var user = userProfileManager.GetUserById(userId);

                if (string.IsNullOrEmpty(user.UserRoleId))
                {
                    UserDashboardCardView.Visible = false;
                    return dtCardData;
                }

                //UserRoles userDetails = userProfileManager.GetUserRoleById(user.UserRoleId);

                //// currentTicket[DatabaseObjects.Columns.TicketStageActionUsers] = uHelper.GetUsersAsString(_context, Convert.ToString(currentTicket[DatabaseObjects.Columns.TicketStageActionUserTypes]), currentTicket);
                //string query = string.Format("{0} = {1} ", "Id", DatabaseObjects.Columns.TicketStageActionUsers);

                //if (userDetails == null)
                //    return dtCardData;

                //if (userDetails != null)
                //    userRoleGlobal = userDetails.Name;

                string SelectedUserGroup = Convert.ToString(Session["SelectedGroup"]);
                SelectedUserGroup = "PM";

                DataTable dtCardViewData = null;
                 dtCardViewData = GetTableDataManager.GetTableDataUsingQuery($"GetDashboardCardViewData @TenantId='{ApplicationContext.TenantID}', @UserId='{userId}', @Group='{SelectedUserGroup}'");

                //if (dtCardViewData == null || dtCardViewData.Rows.Count == 0)
                //    return dtCardData;

                var defaultImage = "/Content/images/BCCI_logo.jpg";
                var userImage = !string.IsNullOrEmpty(user.Picture) ? user.Picture : defaultImage;

                 var rowData = dtCardViewData.Rows[0];
               // DataRow rowData = null;

                UserRole = Convert.ToString(dtCardViewData.Rows[0][0]);
               // UserRole = "CRM";

                var desc1 = "";
                //var desc2 = "";
                //var desc3 = "";
                //var desc4 = "";
                //var desc5 = "";
                //var desc6 = "";
                //var desc7 = "";

                row1["Picture"] = userImage;
                row2["Picture"] = userImage;
                row3["Picture"] = userImage;

                row4["Picture"] = defaultImage;
                row5["Picture"] = defaultImage;
                row6["Picture"] = defaultImage;

                row7["Picture"] = defaultImage;



                if ("PM".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                {
                    desc1 = "My Tasks"; row1["Picture"] = userImage;
                    //desc2 = "Contacts"; row2["Picture"] = defaultImage;
                    //desc3 = "Companies"; row3["Picture"] = defaultImage;
                    //desc4 = "My Leads"; row4["Picture"] = userImage;
                    //desc5 = "My Opportunities"; row5["Picture"] = userImage;
                    //desc6 = "Opportunities"; row6["Picture"] = defaultImage;
                }
                //else if ("APM".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "Contacts"; row2["Picture"] = defaultImage;
                //    desc3 = "Companies"; row3["Picture"] = defaultImage;
                //    desc4 = "My Projects"; row4["Picture"] = userImage;
                //    desc5 = "All Projects"; row5["Picture"] = defaultImage;
                //    desc6 = "My Projects Due in 4 Weeks"; row6["Picture"] = userImage;
                //}
                //else if ("Estimator".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "My Projects"; row2["Picture"] = userImage;
                //    desc3 = "All Projects"; row3["Picture"] = defaultImage;
                //    desc4 = "My Opportunities"; row4["Picture"] = userImage;
                //    desc5 = "Opportunities"; row5["Picture"] = defaultImage;
                //}
                //else if ("Core User".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "My Contacts"; row2["Picture"] = userImage;
                //    desc3 = "My Companies"; row3["Picture"] = userImage;
                //    desc4 = "My Leads"; row4["Picture"] = userImage;
                //    desc5 = "My Opportunities"; row5["Picture"] = userImage;
                //    desc6 = "My Projects"; row6["Picture"] = userImage;
                //}
                //else if ("CRM Admin".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "Contacts"; row2["Picture"] = defaultImage;
                //    desc3 = "Companies"; row3["Picture"] = defaultImage;
                //    desc4 = "Leads"; row4["Picture"] = defaultImage;
                //    desc5 = "My Opportunities"; row5["Picture"] = userImage;
                //    desc6 = "Projects"; row6["Picture"] = defaultImage;
                //}
                //else if ("PE".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase) || "PM".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "Contacts"; row2["Picture"] = defaultImage;
                //    desc3 = "Companies"; row3["Picture"] = defaultImage;
                //    desc4 = "My Projects"; row4["Picture"] = userImage;
                //    desc5 = "All Projects"; row5["Picture"] = defaultImage;
                //    desc6 = "My Projects Due in 4 Weeks"; row6["Picture"] = userImage;
                //}
                //else if ("PM Admin".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "My Opportunities"; row2["Picture"] = userImage;
                //    desc3 = "All Opportunities"; row3["Picture"] = defaultImage;
                //    desc4 = "My Projects"; row4["Picture"] = userImage;
                //    desc5 = "All Projects"; row5["Picture"] = defaultImage;
                //    desc6 = "Projects Due in 4 Weeks"; row6["Picture"] = defaultImage;
                //}
                //else if ("Executive".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "Opportunities"; row2["Picture"] = defaultImage;
                //    desc3 = "Pipeline"; row3["Picture"] = defaultImage;
                //    desc4 = "Live Projects"; row4["Picture"] = defaultImage;
                //    desc5 = "Projects Started this Month"; row5["Picture"] = defaultImage;
                //    desc6 = "Closed this Year"; row6["Picture"] = defaultImage;
                //    desc7 = "Resources"; row7["Picture"] = defaultImage;
                //}
                //else if ("Project Executive".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "My Projects"; row2["Picture"] = userImage;
                //    desc3 = "My Action Items"; row3["Picture"] = userImage;
                //    desc4 = "Live Projects"; row4["Picture"] = defaultImage;
                //    desc5 = "Projects Closed"; row5["Picture"] = defaultImage;
                //    desc6 = "Pipeline"; row6["Picture"] = defaultImage;
                //}
                //else if ("Superintendent".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase) || "Field Operations".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "Projects Ready To Start"; row2["Picture"] = defaultImage;
                //    desc3 = "My Projects"; row3["Picture"] = userImage;
                //    desc4 = "All Projects"; row4["Picture"] = defaultImage;
                //    desc5 = "Project Allocation"; row5["Picture"] = defaultImage;
                //    desc6 = "Active SubCons"; row6["Picture"] = defaultImage;
                //    desc7 = "All SubCons"; row7["Picture"] = defaultImage;
                //}
                //else if ("PreCon Admin".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    desc1 = "My Tasks"; row1["Picture"] = userImage;
                //    desc2 = "All Opportunities"; row2["Picture"] = defaultImage;
                //    desc3 = "My Projects"; row3["Picture"] = userImage;
                //    desc4 = "Projects"; row4["Picture"] = defaultImage;
                //    desc5 = "Projects Ready To Start"; row5["Picture"] = defaultImage;
                //    desc6 = "Resource Utilization"; row6["Picture"] = defaultImage;
                //}
                //else if ("Admin".Equals(UserRole, StringComparison.CurrentCultureIgnoreCase))
                //{
                //    row2["Picture"] = defaultImage;
                //    row3["Picture"] = defaultImage;

                //    desc1 = "My Tasks";
                //    desc2 = "# of Companies";
                //    desc3 = "# of Contacts";
                //    desc4 = "# of Leads";
                //    desc5 = "# of Opportunities";
                //    desc6 = "# of Projects";
                //}

                row1["Description"] = $"{desc1} ({rowData[1]})";
                //row2["Description"] = $"{desc2} ({rowData[2]})";
                //row3["Description"] = $"{desc3} ({rowData[3]})";
                //row4["Description"] = $"{desc4} ({rowData[4]})";

                //row1["Description"] = "My Task (33)";
                //row2["Description"] = "My Task (33)";
                //row3["Description"] = "My Task (33)";
                //row4["Description"] = "My Task (33)";

                dtCardData.Rows.Add(row1);
                //dtCardData.Rows.Add(row2);
                //dtCardData.Rows.Add(row3);
                //dtCardData.Rows.Add(row4);

                //if (5 < rowData.ItemArray.Length)
                //{
                //    row5["Description"] = Convert.ToString(rowData[5]) != "-1" ? $"{desc5} ({rowData[5]})" : $"{desc5}";
                //    dtCardData.Rows.Add(row5);
                //}

                //if (6 < rowData.ItemArray.Length)
                //{
                //    row6["Description"] = Convert.ToString(rowData[6]) != "-1" ? $"{desc6} ({rowData[6]})" : $"{desc6}";
                //    dtCardData.Rows.Add(row6);
                //}

                //if (dtCardViewData.Columns.Count == 8)
                //{
                //    if (7 < rowData.ItemArray.Length)
                //    {
                //        row7["Description"] = Convert.ToString(rowData[7]) != "-1" ? $"{desc7} ({rowData[7]})" : $"{desc7}";
                //        dtCardData.Rows.Add(row7);
                //    }
                //}

                CreateDvxCarddView();
                Session["SelectedGroup"] = null;
            }
            return dtCardData;
        }

        protected void innerProjectData_CustomCallback(object sender, ASPxCardViewCustomCallbackEventArgs e)
        {
        }

        private void CreateDvxCarddView()
        {
            CardViewLayoutGroup cvLayoutGroup = new CardViewLayoutGroup();

            if (UserDashboardCardView.Columns.Count > 0)
                return;

            ////if (selectedColumnsForCardView.Count() > 0) //From MyModuleColumns
            ////{
            ////    DataRow[] filterRows = selectedColumnsForCardView.Where(x => x.Field<string>(DatabaseObjects.Columns.FieldName) != DatabaseObjects.Columns.Id && x.Field<bool>(DatabaseObjects.Columns.IsDisplay) == true).OrderBy(x => x.Field<double>(DatabaseObjects.Columns.FieldSequence)).ToArray();

            ////    foreach (DataRow modulecolumn in filterRows)
            ////    {

            ////        if (Convert.ToString(modulecolumn[DatabaseObjects.Columns.FieldName]) == DatabaseObjects.Columns.Picture)
            ////        {
            ////            CardViewImageColumn Col = new CardViewImageColumn();
            ////            Col.FieldName = Convert.ToString(modulecolumn[DatabaseObjects.Columns.FieldName]);
            ////            Col.Caption = "";
            ////            CardViewColumnLayoutItem CitemLay = new CardViewColumnLayoutItem();
            ////            CitemLay.ColumnName = DatabaseObjects.Columns.Picture;
            ////            CitemLay.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
            ////            CitemLay.VerticalAlign = FormLayoutVerticalAlign.Top;
            ////            ugitUserProfileCardView.CardLayoutProperties.Items.AddColumnItem(CitemLay);

            ////            if (resultedTable != null && resultedTable.Columns.Contains(Convert.ToString(modulecolumn[DatabaseObjects.Columns.FieldName])))
            ////                ugitUserProfileCardView.Columns.Add(Col);
            ////        }
            ////        else
            ////        {
            ////            CardViewColumn Ccol = new CardViewColumn();
            ////            Ccol.FieldName = Convert.ToString(modulecolumn[DatabaseObjects.Columns.FieldName]);

            ////            CardViewLay.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
            ////            CardViewLay.VerticalAlign = FormLayoutVerticalAlign.Middle;
            ////            CardViewLay.SettingsItemCaptions.Location = LayoutItemCaptionLocation.Top;
            ////            CardViewLay.GroupBoxDecoration = GroupBoxDecoration.None;

            ////            CardViewColumnLayoutItem CitemLay = new CardViewColumnLayoutItem();
            ////            CitemLay.ColumnName = Convert.ToString(modulecolumn[DatabaseObjects.Columns.FieldName]);

            ////            CardViewLay.Items.AddColumnItem(CitemLay);

            ////            if (resultedTable != null && resultedTable.Columns.Contains(Convert.ToString(modulecolumn[DatabaseObjects.Columns.FieldName])))
            ////                ugitUserProfileCardView.Columns.Add(Ccol);
            ////        }

            ////    }
            ////    ugitUserProfileCardView.CardLayoutProperties.ColCount = 2;
            ////    ugitUserProfileCardView.CardLayoutProperties.Items.AddGroup(CardViewLay);

            ////}
            ////else
            ////{
            ///

            CardViewImageColumn cvImageColumn = new CardViewImageColumn();
            cvImageColumn.FieldName = DatabaseObjects.Columns.Picture;
            cvImageColumn.Caption = "";
            cvImageColumn.PropertiesImage.ImageHeight = Unit.Pixel(40);
            //cvImageColumn.PropertiesImage.ImageWidth = Unit.Pixel(50);
            cvImageColumn.Width = Unit.Percentage(30);

            CardViewColumnLayoutItem cvLayoutItem = new CardViewColumnLayoutItem();
            cvLayoutItem.ColumnName = DatabaseObjects.Columns.Picture;
            cvLayoutItem.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
            cvLayoutItem.VerticalAlign = FormLayoutVerticalAlign.Top;

            UserDashboardCardView.CardLayoutProperties.Items.AddColumnItem(cvLayoutItem);
            UserDashboardCardView.Columns.Add(cvImageColumn);

            //For Layout Group
            cvLayoutGroup.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
            cvLayoutGroup.VerticalAlign = FormLayoutVerticalAlign.Middle;
            cvLayoutGroup.SettingsItemCaptions.Location = LayoutItemCaptionLocation.Left;
            cvLayoutGroup.GroupBoxDecoration = GroupBoxDecoration.None;


            //CardViewColumn ccol = new CardViewColumn();
            ////ccol.FieldName = DatabaseObjects.Columns.Name;
            //////ccol.Caption = DatabaseObjects.Columns.Name;
            ////ccol.Caption = "";

            ////CardViewLay.Items.AddColumnItem(ccol.FieldName);
            ////ugitUserProfileCardView.Columns.Add(ccol);

            //ccol = new CardViewColumn();
            ////ccol.FieldName = DatabaseObjects.Columns.JobProfile;
            ////ccol.Caption = "Job Title";
            //ccol.Caption = "";

            ////CardViewLay.Items.AddColumnItem(ccol.FieldName);
            ////ugitUserProfileCardView.Columns.Add(ccol);


            var cvCol = new CardViewColumn();
            cvCol.FieldName = "Description";
            cvCol.Caption = "Description";

            //ccol.Caption = "";

            cvLayoutGroup.Items.AddColumnItem(cvCol.FieldName);
            UserDashboardCardView.Columns.Add(cvCol);

            ////ccol = new CardViewColumn();
            ////ccol.FieldName = DatabaseObjects.Columns.PhoneNumber;
            ////ccol.Caption = DatabaseObjects.Columns.Phone;
            //ccol.Caption = "";

            ////CardViewLay.Items.AddColumnItem(ccol.FieldName);
            ////ugitUserProfileCardView.Columns.Add(ccol);

            UserDashboardCardView.CardLayoutProperties.ColCount = 2;
            UserDashboardCardView.CardLayoutProperties.Items.AddGroup(cvLayoutGroup);

            ////}
        }
    }
}
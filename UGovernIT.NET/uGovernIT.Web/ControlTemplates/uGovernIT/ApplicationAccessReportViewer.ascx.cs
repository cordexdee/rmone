
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Helpers;
using uGovernIT.Manager;
using System.Linq;
using uGovernIT.Utility;
using System.Web;
using System.Collections.Generic;
using uGovernIT.Manager.Reports;
using uGovernIT.Utility.Entities;

namespace uGovernIT.Web
{
    public partial class ApplicationAccessReportViewer : UserControl
    {
        public int ApplicationId { get; set; }
        public string TicketId { get; set; }
        ApplicationRoleManager appRoleMGR;
        ApplicationModuleManager aapModuleMGR;
        ApplicationAccessManager appAccessMGR;
        ApplicationContext _context;
        DataTable dtModuleRole = null;
        UserProfileManager userProfileManager;
        //string reportUrl;
        protected override void OnInit(EventArgs e)
        {
            _context = HttpContext.Current.GetManagerContext();
            appRoleMGR = new ApplicationRoleManager(_context);
            appAccessMGR = new ApplicationAccessManager(_context);
            aapModuleMGR = new ApplicationModuleManager(_context);
            userProfileManager = new UserProfileManager(_context);
            List<ApplicationRole> roles = appRoleMGR.Load(x => x.APPTitleLookup == ApplicationId).ToList();
            DataTable dtRoles = UGITUtility.ToDataTable<ApplicationRole>(roles);
            dtModuleRole = new DataTable();
            if (dtRoles != null && dtRoles.Rows.Count > 0)
            {
                dtModuleRole.Columns.Add(new DataColumn("Modules"));
                dtModuleRole.Columns.Add(new DataColumn("Assignees"));
                foreach (DataRow dr in dtRoles.Rows)
                {
                    int colIndex = 1;
                    DataColumn dc = new DataColumn(Convert.ToString(dr[DatabaseObjects.Columns.Title]), typeof(bool));
                    while (dtModuleRole.Columns.Contains(dc.ColumnName))
                    {
                        dc = new DataColumn(Convert.ToString(dr[DatabaseObjects.Columns.Title]) + "_"+ colIndex, typeof(bool));
                        colIndex++;
                    }
                    
                    dc.DefaultValue = false;
                    dtModuleRole.Columns.Add(dc);
                }

                List<ApplicationAccess> accessList = appAccessMGR.Load(x => x.APPTitleLookup == ApplicationId).OrderBy(x => x.ApplicationModulesLookup).ToList();
                DataTable dt = UGITUtility.ToDataTable<ApplicationAccess>(accessList);
                if (dt != null && dt.Rows.Count > 0)
                {
                    #region reduce multiple hit to get common data
                    List<string> mappedAppModule = dt.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.ApplicationModulesLookup)).Distinct().ToList();
                    List<string> mappedAppRoles = dt.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.ApplicationRoleLookup)).Distinct().ToList();
                    List<string> lstOfRoleAssign = dt.AsEnumerable().Select(x => x.Field<string>(DatabaseObjects.Columns.ApplicationRoleAssign)).Distinct().ToList();
                    List<ApplicationModule> lstOfFilterModules = aapModuleMGR.Load(x => mappedAppModule.Any(y => x.ID ==UGITUtility.StringToLong(y)));
                    List<ApplicationRole> lstOfFilterRoles = appRoleMGR.Load(x => mappedAppRoles.Any(y => x.ID == UGITUtility.StringToLong(y)));
                    List<UserProfile> lstOfFilterAssignee = userProfileManager.Load(x => lstOfRoleAssign.Contains(x.Id));
                    ApplicationRole applicationRole = null;
                    ApplicationModule applicationModule = null;
                    UserProfile userProfile = null;
                    #endregion

                    foreach (DataRow dr in dt.Rows)
                    {
                        string assignee = Convert.ToString(dr[DatabaseObjects.Columns.ApplicationRoleAssign]).Replace("'", "''");
                        userProfile = lstOfFilterAssignee.FirstOrDefault(x => x.Id == assignee);
                        if (userProfile != null)
                            assignee = userProfile.Name;

                        long modulelookup = UGITUtility.StringToLong(dr[DatabaseObjects.Columns.ApplicationModulesLookup]);

                        applicationModule = lstOfFilterModules.FirstOrDefault(x => x.ID == modulelookup);
                        // Check if access record already exists
                        DataRow[] drExisting = dtModuleRole.Select(string.Format("Assignees='{0}' AND  Modules='{1}'",
                                                                        assignee, applicationModule.Title));

                        string roleLookup = Convert.ToString(dr[DatabaseObjects.Columns.ApplicationRoleLookup]);
                        applicationRole = lstOfFilterRoles.FirstOrDefault(x => x.ID == UGITUtility.StringToLong(roleLookup));
                        if (applicationRole != null) // Make sure role exists
                        {
                            DataRow drModuleRole;
                            if (drExisting != null && drExisting.Length > 0)
                            {
                                // Update existing
                                drModuleRole = drExisting[0];
                                drModuleRole[applicationRole.Title] = true;
                            }
                            else
                            {
                                // Else add new record
                                drModuleRole = dtModuleRole.NewRow();
                                
                                drModuleRole["Modules"] = modulelookup;
                                if (applicationModule != null)
                                    drModuleRole["Modules"] = applicationModule.Title;

                                drModuleRole["Assignees"] = dr[DatabaseObjects.Columns.ApplicationRoleAssign];
                                if (userProfile != null)
                                    drModuleRole["Assignees"] = userProfile.Name;
                                drModuleRole[applicationRole.Title] = true;
                                dtModuleRole.Rows.Add(drModuleRole);
                            }
                        }
                    }

                    DataView view = new DataView(dtModuleRole);
                    view.Sort = "Modules ASC, Assignees ASC";
                    dtModuleRole = view.ToTable();
                }
            }

            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool ignoreAdditionalSetting = false;
            if (dtModuleRole != null && dtModuleRole.Columns.Count < 10)
            {
                RptAccessReport.AutoSize = false;
                ignoreAdditionalSetting = true;
            }

            ApplicationAccessReport accessReport = new ApplicationAccessReport(dtModuleRole, TicketId,ignoreAdditionalSetting);
            RptAccessReport.Report = accessReport;
        }
    }
}

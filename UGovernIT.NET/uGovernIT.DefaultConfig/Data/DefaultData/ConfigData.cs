using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uGovernIT.Manager;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Utility.Entities.Common;
using Microsoft.AspNet.Identity;
using uGovernIT.Utility.Entities.DB.uGovernIT.Utility;
using uGovernIT.Utility.Entities.DB;
using uGovernIT.Manager.Core;
using uGovernIT.Manager.Managers;
using uGovernIT.Util.Log;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    public class ConfigData : IDefault
    {
        // user fields imported from user class
        public static string userID = "";
        //public static string userName = "";

        private ApplicationContext _applicationContext { get; set; }


        public ConfigData(ApplicationContext Context) { _applicationContext = Context; }

        //public List<Tuple<string, string, string, bool, RoleType, string>> roles = new List<Tuple<string, string, string, bool, RoleType, string>>()
        //{
        //    new Tuple<string, string, string, bool, RoleType, string>("6D11509F-4261-497B-AD59-DDB992A4CD2F", "SAdmin", "Super Admin", true, RoleType.SAdmin, "Super Admin"),
        //    new Tuple<string, string, string, bool, RoleType, string>("6D14F1B1-B913-42E5-BB1B-A18277316FE4", "Admin", "Can Manage", false, RoleType.SAdmin, "uGovernIT Admins"),
        //    new Tuple<string, string, string, bool, RoleType, string>("6D14F1B1-B913-42E5-BB1B-A18277316FE5", "ResourceAdmin", "Can Manage", false, RoleType.ResourceAdmin, "Resource Admin"),
        //    new Tuple<string, string, string, bool, RoleType, string>("6D14F1B1-B913-42E5-BB1B-A18277316FE6", "UGITSuperAdmin", "UGIT Super Admin", false, RoleType.UGITSuperAdmin, "UGIT Super Admin")
        //};

        //Configuration values.

        public static string userId = "";
        public static string managerUserId = "";
        public static string userName = "";// Obtained from userId in UGovernITDefault.Initialize()
        public static string Password = "password@123";


        public static string[] HideInTicketTemplate = { "Attachments", "AssetLookup", "TicketComment", "TicketCreationDate", "TicketInitiator", "TicketRequestTypeCategory", "TicketStatus" };

        /// Default URL used when none passed in
        //public static string url = "http://demo.ugovernit.com/";  // Demo site
        public static string url = "http://winserver/";

        public static string adminGroupName = "uGovernIT Admins";
        public static string membersGroupName = "uGovernIT Members";
        public static int totalNumberOfTickets = 0;
        public static string guideMeUrl = "/SitePages/GuideMe.aspx";

        public static bool loadRequestTypes = true;
        public static bool loadDepartments = true;
        public static bool loadLocations = true;

        public static bool loadBudgetCategories = true;
        public static bool loadVNDModules = true;

        public static bool userFieldsEditable = true;
        public static bool loadModuleStages = true;

        // Used to track current stage when assigning stages
        public static int currStageID = 0;

        public static Hashtable moduleStartStages = new Hashtable();
        public static Hashtable moduleAssignedStages = new Hashtable();
        public static Hashtable moduleResolvedStages = new Hashtable();
        public static Hashtable moduleTestedStages = new Hashtable();
        public static Hashtable moduleClosedStages = new Hashtable();

        // Used to track certain stages that are used later in ChartFormula
        public static int nprMgrApprovalStageID;
        public static int nprPMOStageID;
        public static int nprITGovernanceStageID;
        public static int nprITSteeringeStageID;
        public static int nprApprovedStageID;

        public static int pmmStartStageID;
        public static int pmmClosedStageID;

        public void InsertProjectLifeCycles(ApplicationContext context)
        {
            List<LifeCycle> lstLifeCycles = new List<LifeCycle>()
            {
                new LifeCycle()
                {
                    Name="Waterfall 5-Stage",
                    ModuleNameLookup="PMM",
                    Description="5-Stage shortened waterfall cycle",
                    ItemOrder=1,
                    TenantID = context.TenantID
                },
                new LifeCycle()
                {
                    Name="Waterfall 7-Stage",
                    ModuleNameLookup="PMM",
                    Description="7-Stage long waterfall cycle",
                    ItemOrder=2,
                    TenantID = context.TenantID
                }
            };
            InsertProjectLifeCycleStages(context, lstLifeCycles);
        }

        private void InsertProjectLifeCycleStages(ApplicationContext context, List<LifeCycle> lstLifeCycles)
        {
            LifeCycleManager lcHelper = new LifeCycleManager(context);
            LifeCycleStageManager stageManager = new LifeCycleStageManager(context);
            foreach (LifeCycle lc in lstLifeCycles)
            {
                if (lc.Name == "Waterfall 5-Stage")
                {
                    long i = lcHelper.Insert(lc);
                    List<LifeCycleStage> lstStages = new List<LifeCycleStage>() {
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Identification", StageTitle= "Identification", Description="Identification",StageStep=1,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Evaluation", StageTitle= "Evaluation", Description="Evaluation",StageStep=1,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Initiation", StageTitle= "Initiation", Description="Initiation",StageStep=1,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Execution", StageTitle= "Execution", Description="Execution",StageStep=1,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Completion", StageTitle= "Completion", Description="Completion",StageStep=1,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID }
                    };

                    stageManager.InsertItems(lstStages);
                }
                if (lc.Name == "Waterfall 7-Stage")
                {
                    long i = lcHelper.Insert(lc);
                    List<LifeCycleStage> lstStages = new List<LifeCycleStage>() {
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Concept", StageTitle= "Concept", Description="Concept",StageStep=1,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Plan", StageTitle= "Plan", Description="Plan",StageStep=2,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Design", StageTitle= "Design", Description="Design",StageStep=3,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Build", StageTitle= "Build", Description="Build",StageStep=4,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Test", StageTitle= "Test", Description="Test",StageStep=5,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Deploy", StageTitle= "Deploy", Description="Deploy",StageStep=6,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID },
                    new LifeCycleStage() { LifeCycleName = lc.ID, Name= "Closeout", StageTitle= "Closeout", Description="Closeout",StageStep=7,StageWeight=1,ModuleNameLookup="PMM", TenantID=context.TenantID }
                    };
                    stageManager.InsertItems(lstStages);
                }
            }
        }

        public List<ConfigurationVariable> GetConfigurationVariable(ApplicationContext context)
        {
            List<ConfigurationVariable> mList = new List<ConfigurationVariable>();

            // Console.WriteLine("  ConfigurationVariable");

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "SMSHomePage", KeyName = "SMSHomePage", KeyValue = "/SitePages/Home.aspx", Description = "SMS Home Page URL", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AdminPage", KeyName = "AdminPage", KeyValue = "/SitePages/Admin.aspx", Description = "Admin Page URL", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "FilterTicketsPageUrl", KeyName = "FilterTicketsPageUrl", KeyValue = "/Layouts/ugovernit/delegatecontrol.aspx?control=cutomfilterticket", Description = "Ticket List webpart page URL", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ModulesRequiredCallLog", KeyName = "ModulesRequiredCallLog", KeyValue = "1", Description = "Used to control whether call log statistics are shown for PRS", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "GuideMeTitle", KeyName = "GuideMeTitle", KeyValue = "Guide Me!", Description = "Title of Wizard sidebar panel", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "GuideMeDescription", KeyName = "GuideMeDescription", KeyValue = "Start here if you are not sure which module you need.", Description = "Description of Wizard panel", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ThemedBackground", KeyName = "ThemedBackground", KeyValue = "False", Description = "Use accent color for background of module list pages", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "DivisionLabel", KeyName = "DivisionLabel", KeyValue = "Executing Office", Description = "Terminology for Divison" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "StudioLabel", KeyName = "StudioLabel", KeyValue = "Project Executive", Description = "Terminology for Divison" });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnableSimiarityFunction", KeyName = "EnableSimiarityFunction", KeyValue = "True", Description = "This enables find similar projects functionality", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnableLockUnlockAllocation", KeyName = "EnableLockUnlockAllocation", KeyValue = "True", Description = "This enable/disable the lock functionality on team tab", Type = string.Empty });
            //dataList.Add(new string[] { "General", "uGovernITLogo", "/Content/images/ugovernit/ugovernit_logo.png", "Use to override uGovernIT logo in footer" ,Type= string.Empty});
            //dataList.Add(new string[] { "General", "uGovernITLogoLink", "http://ugovernit.com", "Use to override logo link in footer" ,Type= string.Empty});

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "FooterText", KeyName = "FooterText", KeyValue = "<a href=\"http://ugovernit.com\" target=\"_blank\">uGovernIT.com</a> 2010-2018 v7.0", Description = "Product Footer text shown on each page", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "HeaderLogo", KeyName = "HeaderLogo", KeyValue = "/Content/Images/Service_Prime_Logo.svg", Description = "Company Logo used in Header, Homepage, etc. - add here as attachment", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ReportLogo", KeyName = "ReportLogo", KeyValue = "/Content/Images/Service_Prime_Logo.svg", Description = "Company Logo used in reports, etc. - add here as attachment", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "SignitureLogo", KeyName = "SignitureLogo", KeyValue = "/Content/Images/sp-logo.png", Description = "Signiture Logo used in mail", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AccountName", KeyName = "AccountName", KeyValue = context.TenantAccountId, Description = "Tenant name used show in page header", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ApplicationAccessPageSize", KeyName = "ApplicationAccessPageSize", KeyValue = "10", Description = "Set the default Application Access Page Size", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "Password Policy", KeyName = "PasswordPolicy", KeyValue = "Passwords must not contain the user's account name or parts of the user's full name that exceed two consecutive characters, Be at least 7 characters in length, AND Contain characters from three of the following four categories: English uppercase characters(A through Z), English lowercase characters(a through z), Base 10 digits(0 through 9), Non - alphabetic characters(for example, !, $, #, %)", Description = "Set the Password Policy", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ModuleStageAlternateLabel", KeyName = "ModuleStageAlternateLabel", KeyValue = "True", Description = "ModuleStageAlternateLabel" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ShowBudgetImport", KeyName = "ShowBudgetImport", KeyValue = "True", Description = "this is used to hide and show import button for budget import" });
            // Name of Help Desk group
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "HelpDeskGroup", KeyName = "HelpDeskGroup", KeyValue = "Help Desk", Description = "Name of Help Desk group", Type = string.Empty });

            //Resource Hourly Default Rate
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ResourceHourlyRate", KeyName = "ResourceHourlyRate", KeyValue = "100", Description = "Default resource hourly rate in RMM", Type = string.Empty });

            //Should budgets include Staffing Budget
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "StaffingBudget", KeyName = "StaffingBudget", KeyValue = "True", Description = "Include Staffing in Budgets?", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnablePowerSearch", KeyName = "EnablePowerSearch", KeyValue = "True", Description = "Enable Power Search in ticket/project lists", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "DisableRequestListBanding", KeyName = "DisableRequestListBanding", KeyValue = "True", Description = "If set to true then .Net banding else sharepoint banding!", Type = string.Empty });


            // Alternate label in module stage graphics
            mList.Add(new ConfigurationVariable() { CategoryName = "ModuleStage", Title = "AlternateLabel", KeyName = "AlternateLabel", KeyValue = "True", Description = "If enabled (True) puts alternate label in module stage graphics", Type = string.Empty });

            // Super-admin groups
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AdminGroupName", KeyName = "AdminGroupName", KeyValue = adminGroupName, Description = "Super Admin Group name", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "TicketAdminGroup", KeyName = "TicketAdminGroup", KeyValue = adminGroupName, Description = "Ticket Admin Group name", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "ResourceAdminGroup", KeyName = "ResourceAdminGroup", KeyValue = adminGroupName, Description = "Resource Admin Group (Names separated by ;#), can see all RMM allocations and hours and edit resource info", Type = string.Empty });

            //No of browse buttons to be shown on new ticket form
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "NewTicketMaxFiles", KeyName = "NewTicketMaxFiles", KeyValue = "5", Description = "Max No Of files that can be attached while creating a new ticket", Type = string.Empty });

            // Only show first username in "Waiting On" column of ticket lists?
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "OnlyShowFirstActionUser", KeyName = "OnlyShowFirstActionUser", KeyValue = "False", Description = "Only show first username in 'Waiting On' column of ticket lists", Type = string.Empty });

            // For showing  the query control.
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ShowQueryReports", KeyName = "ShowQueryReports", KeyValue = "True", Description = "Configure the visibility of Query Reports in ticket lists", Type = string.Empty });

            // Enable Divisions
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnableDivision", KeyName = "EnableDivision", KeyValue = "False", Description = "If enabled, uses Company-Division-Department hierarchy, else uses Company-Department hierarchy", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ShowDepartmentDetail", KeyName = "ShowDepartmentDetail", KeyValue = "True", Description = "Show Department Detail like Company > Division > Department", Type = string.Empty });

            // View ID of "TicketSummary" view in DashboardPanelView - adjust if necessary
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "OpenTicketsChart", KeyName = "OpenTicketsChart", KeyValue = "10", Description = "View ID for Charts shown in Open Tickets report", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ClosedTicketsChart", KeyName = "ClosedTicketsChart", KeyValue = "10", Description = "View ID for Charts shown in Closed Tickets report", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "OpenProjectsChart", KeyName = "OpenProjectsChart", KeyValue = "10", Description = "View ID for Charts shown in Open Projects report", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ClosedProjectsChart", KeyName = "ClosedProjectsChart", KeyValue = "10", Description = "View ID for Charts shown in Closed Projects report", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "HomePageHelpUrl", KeyName = "HomePageHelpUrl", KeyValue = "", Description = "Sets the help URL for Home Page", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "CommentsNewestFirst", KeyName = "CommentsNewestFirst", KeyValue = "False", Description = "Set to true to order comments newest first, else shows oldest first", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnableAppModuleRoles", KeyName = "EnableAppModuleRoles", KeyValue = "False", Description = "Controls whether Owner & Support Group can be specified at Application Module level", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "MessageBoardHideIfEmpty", KeyName = "MessageBoardHideIfEmpty", KeyValue = "False", Description = "Hide control if no messages to display", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AutoRedirectInvalidURL", KeyName = "AutoRedirectInvalidURL", KeyValue = "False", Description = "Auto-redirect invalid URLs not configured in alternate access mappings collection to valid url. Else error is shown with official URL.", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ShowGroupTicketsTab", KeyName = "ShowGroupTicketsTab", KeyValue = "False", Description = "Enables display of 'Group Tickets' tab in PRS, etc. to display list of tickets where the user is part of a group named on the ticket.", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnableUGITLogging", KeyName = "EnableUGITLogging", KeyValue = "False", Description = "Enables logging of user profile changes and other events to uGovernIT internal logs.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AllowCommentDelete", KeyName = "AllowCommentDelete", KeyValue = "True", Description = "Allow admin to delete comments.", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AutoADSyncEnabled", KeyName = "AutoADSyncEnabled", KeyValue = "False", Description = "Enable auto-sync of users from Active Directory", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AttachTaskCalendarEntry", KeyName = "AttachTaskCalendarEntry", KeyValue = "False", Description = "Attach task calendar entry in to task notification", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ShowSPRibbon", KeyName = "ShowSPRibbon", KeyValue = "False", Description = "Set to true to add web-parts, etc.", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnableRequestTypeSubCategory", KeyName = "EnableRequestTypeSubCategory", KeyValue = "True", Description = "Enable request type sub category", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "VIPGroup", KeyName = "VIPGroup", KeyValue = "VIP", Description = "VIP Group to mark ticket as VIP", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ScheduleActionArchiveExpiration", KeyName = "ScheduleActionArchiveExpiration", KeyValue = "90", Description = "Schedule Action Archive Expiration in days.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "WaitingOnMeTabName", KeyName = "WaitingOnMeTabName", KeyValue = "My Queue", Description = "Defaults to [Waiting On Me]", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "TSRAppCategory", KeyName = "TSRAppCategory", KeyValue = "Application Support", Description = "Name of top-level category for TSR request types synced from APP module", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AutoRefreshFrequency", KeyName = "AutoRefreshFrequency", KeyValue = "0", Description = "Frequency (in minutes) to refresh My Tickets part of home page, set to non-zero to enable auto-refresh", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "FileUploadHandlers", KeyName = "FileUploadHandlers", KeyValue = "html5,html4", Description = "The document uploader will try to use these upload handlers in this order. Defaults to 'html5,silverlight,flash,html4' if not set.", Type = string.Empty });
            //Get completion date from target or desire date to show ticket age
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AgeColorByTargetCompletion", KeyName = "AgeColorByTargetCompletion", KeyValue = "False", Description = "Get completion date from target or desire date to show ticket age.", Type = string.Empty });
            //Message Board Extended For Another group
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "MessageBoardAdmins", KeyName = "MessageBoardAdmins", KeyValue = "", Description = "Members can edit Message Board items", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AutoCloseChildTickets", KeyName = "AutoCloseChildTickets", KeyValue = "True", Description = "If set to true, prompts to also close any child tickets when parent ticket is closed", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "UgitTheme", KeyName = "UgitTheme", KeyValue = "<?xml version=\"1.0\" encoding=\"utf - 16\"?><UGITTheme xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><ThemeName>UGITNavyBlueDevEx</ThemeName><DevExTheme>UGITNavyBlueDevEx</DevExTheme><SPThemeName>Navy Blue</SPThemeName><Photo>/Content/Images/UGITNavyBlue.png</Photo><FontName>Verdana</FontName><MenuHighlightColor>#000000</MenuHighlightColor></UGITTheme>", Description = "uGovernIT Theme (will be auto-set by Theme selector)", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ModuleAgentButtonsEnable", KeyName = "ModuleAgentButtonsEnable", KeyValue = "True", Description = "Enable Module Agents as Buttons", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnableTicketReopenByRequestor", KeyName = "EnableTicketReopenByRequestor", KeyValue = "True", Description = "Allow ticket requestors to re-open closed ticket from home page", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "DisableAdminModules", KeyName = "DisableAdminModules", KeyValue = "a2;amanageInterface;atemplate;agovernance", Description = "Use ID for Disable Admin modules : - aresourceConfig-->User Management;acoreSetUp-->Configuration;aworkflow->Workflows;aKnowledge -->Knowledge;asystemResource -->System;a2 -->Reports;amanageInterface -->Interfaces;atemplate -->Templates;agovernance -->Governance;", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AutoFillTicket", KeyName = "AutoFillTicket", KeyValue = "True", Description = "Auto fill Request type,PRP.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AllowZeroResolutionHrs", KeyName = "AllowZeroResolutionHrs", KeyValue = "True", Description = "Allow zero actual hours when resolving tickets.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "TicketAgeExcludesHoldTime", KeyName = "TicketAgeExcludesHoldTime", KeyValue = "False", Description = "Set to true if you want to exclude hold time in ticket age, else will include hold time", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AdfsDomainName", KeyName = "AdfsDomainName", KeyValue = "@bcciconst.com", Description = "adfs settings", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "PreConUserGroups", KeyName = "PreConUserGroups", KeyValue = "Admin", Description = "Comma (,) seperated User Groups, for Assign to PreCon, eg., Admin,Preconstruction", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "showDefaultHelpPageButton", KeyName = "showDefaultHelpPageButton", KeyValue = "True", Description = "redirect to default help page.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "widgetsHelpCardCategory", KeyName = "widgetsHelpCardCategory", KeyValue = "widgets", Description = "category of help card to be shown in guide me page", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "DefaultPageUrl", KeyName = "DefaultPageUrl", KeyValue = "/SitePages/GuideMe", Description = "path of guide me page", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "CompletedTasksDisplayDuration", KeyName = "CompletedTasksDisplayDuration", KeyValue = "30", Description = "Sets, How long 'Recently Completed Tasks' stay on the homepage, in days", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnableStudioDivisionHierarchy", KeyName = "EnableStudioDivisionHierarchy", KeyValue = "True", Description = "To enable hierarchy between Studios and Division(Parent).", Type = "Bool" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ModuleTypes", KeyName = "ModuleTypes", KeyValue = "CPR:Project;OPM:Opportunity;CNS:Service", Description = "this will work new tenant will register and import projects", Type = "Text" });

            // Escalations
            mList.Add(new ConfigurationVariable() { CategoryName = "Escalation", Title = "EnableEscalations", KeyName = "EnableEscalations", KeyValue = "False", Description = "Enable SLA Escalations", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Escalation", Title = "RequestTypeSLAWarningTime", KeyName = "RequestTypeSLAWarningTime", KeyValue = "2", Description = "Request Type SLA Warning Time - # of hrs prior to SLA Expiration", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Escalation", Title = "RequestTypeSLAWarningBody", KeyName = "RequestTypeSLAWarningBody", KeyValue = "", Description = "Request Type SLA Warning email body - leave blank for default", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Escalation", Title = "RequestTypeSLAEscalationRoles", KeyName = "RequestTypeSLAEscalationRoles", KeyValue = "PRPGroup;RequestTypeEscalationManager;RequestTypeBackupEscalationManager", Description = "Request Type SLA Escalation Roles", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Escalation", Title = "RequestTypeSLAEscalationBody", KeyName = "RequestTypeSLAEscalationBody", KeyValue = "", Description = "Request Type SLA Escalation email body - leave blank for default", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Escalation", Title = "RequestTypeSLAEscalationRecurrance", KeyName = "RequestTypeSLAEscalationRecurrance", KeyValue = "24", Description = "Recurring interval in hours for Request Type SLA Escalations", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Escalation", Title = "RequestTypeSLAWarningSubject", KeyName = "RequestTypeSLAWarningSubject", KeyValue = "", Description = "Request Type SLA Warning email subject - leave blank for default", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Escalation", Title = "RequestTypeSLAEscalationSubject", KeyName = "RequestTypeSLAEscalationSubject", KeyValue = "", Description = "Request Type SLA Escalation email subject - leave blank for default", Type = string.Empty });

            // For default working day & time
            //dataList.Add(new string[] { "Calendar", "UseCalendar", "HolidaysAndWorkDaysCalendar", "Holidays and Workdays calendar. Working hours must be shown using a category of 'Work hours'" ,Type= string.Empty});
            mList.Add(new ConfigurationVariable() { CategoryName = "Calendar", Title = "UseCalendar", KeyName = "UseCalendar", KeyValue = "", Description = "Holidays and Workdays calendar. Working hours must be shown using a category of 'Work hours'", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Calendar", Title = "WorkdayStartTime", KeyName = "WorkdayStartTime", KeyValue = "1/1/1900 9:00:00 AM", Description = "Workday start time (ignores date) ONLY USED IF NO CALENDAR", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Calendar", Title = "WorkdayEndTime", KeyName = "WorkdayEndTime", KeyValue = "1/1/1900 5:00:00 PM", Description = "Workday end time (ignores date) ONLY USED IF NO CALENDAR", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Calendar", Title = "WeekendDays", KeyName = "WeekendDays", KeyValue = "Saturday,Sunday", Description = "Weekend days ONLY USED IF NO CALENDAR", Type = string.Empty });
            //Fiscal Year - Ignores the Year in code.
            mList.Add(new ConfigurationVariable() { CategoryName = "Calendar", Title = "FiscalStartDate", KeyName = "FiscalStartDate", KeyValue = "4/1/2011 9:00:00 AM", Description = "Fiscal Calendar Start Date (ignores time)", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Calendar", Title = "ResourceWorkingHours", KeyName = "ResourceWorkingHours", KeyValue = "8", Description = "Resource Hours Calculation", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "Greeting", KeyName = "Greeting", KeyValue = "<b>*** Please do not reply to this email ***</b><br /><br />Hi", Description = "Email Greeting", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "Signature", KeyName = "Signature", KeyValue = "Thanks, <br /><b>Help Desk</b><br />[$Logo$]", Description = "Email Signature", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "SendEmail", KeyName = "SendEmail", KeyValue = "1", Description = "Enable/disable e-mail notifications", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "DisplayName", KeyName = "DisplayName", KeyValue = "Service Prime", Description = "Display Name in From EmailId", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "CreateUserMailSubject", KeyName = "CreateUserMailSubject", KeyValue = "Your registration is successful!", Description = "Subject in New User invitation mail.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "CreateUserMailBody", KeyName = "CreateUserMailBody", KeyValue = "<html><head></head><body><p>Dear [$name$],<br>You are set up as a user of Service Prime.<br><br>You can create IT requests using a simple portal that is now set up for you.<br><br>This email contains your credentials: <strong>[$userCredentials$]</strong><br><br>Please <a href='[$SiteUrl$]'>click here</a> to activate your account.</p><p><br>Thanks,<br>[$CurrentUserName$]<br>Service Prime.</p></body></html>", Description = "Body in New User invitation mail.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "SupportEmail", KeyName = "SupportEmail", KeyValue = "support@ugovernit.com",Description= "Support Email id which appears on reset password email body.", Type = string.Empty });

            // Email To Ticket
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "EnableEmailToTicket", KeyName = "EnableEmailToTicket", KeyValue = "False", Description = "Enable Email-to-Ticket functionality", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "EmailToTicketCredentials", KeyName = "EmailToTicketCredentials", KeyValue = "", Description = "Credentials used to connect to IMAP server for Email-to-Ticket functionality", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "SurveyEmailBody", KeyName = "SurveyEmailBody", KeyValue = "", Description = "Survey Email Body", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "NotifyRequestorOnTargetDateChange", KeyName = "NotifyRequestorOnTargetDateChange", KeyValue = "False", Description = "Send notification to requestor if estimated target completion date changed.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "AllowedEmailToTicketSenders", KeyName = "AllowedEmailToTicketSenders", KeyValue = "", Description = "Email list of senders", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "NotifyNewPRPORPOnly", KeyName = "NotifyNewPRPORPOnly", KeyValue = "True", Description = "Send mail to newly added PRP or ORP only", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "EnableMigrate", KeyName = "EnableMigrate", KeyValue = "False", Description = "Enable migrate from stage to target", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "EnableMigrateCredential", KeyName = "EnableMigrateCredential", KeyValue = "", Description = "store credential to migrate stage to production", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "EmailToTicketIgnoreAttachments", KeyName = "EmailToTicketIgnoreAttachments", KeyValue = "False", Description = "Ignores/strips out attachments when converting emails into tickets", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "EmailToTicketAllowedAttachmentsExtn", KeyName = "EmailToTicketAllowedAttachmentsExtn", KeyValue = "", Description = "Semicolon-separated list of allowed email attachment extensions. Note: EmailToTicketIgnoreAttachments must also be set to False.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "EmailToTicketUsesOAuth2", KeyName = "EmailToTicketUsesOAuth2", KeyValue = "False", Description = "this can be used for two factor authentication instead of basic", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-Mail", Title = "JobStatusEmailTo", KeyName = "JobStatusEmailTo", KeyValue = "support@ugovernit.com", Description = "Email account that the status of timer jobs are sent to. Leave blank if no email notification is needed.", Type = string.Empty });



            // For Mobile Page Header & Footer
            mList.Add(new ConfigurationVariable() { CategoryName = "Mobile", Title = "EnableMobileRequest", KeyName = "EnableMobileRequest", KeyValue = "False", Description = "Enables redirection of requests from mobile devices (like iPhone, Android, etc.) to mobile versions of web pages.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Mobile", Title = "HomePageHeader", KeyName = "HomePageHeader", KeyValue = "uGovernIT Home", Description = "Mobile Homepage Header", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Mobile", Title = "PageHeader", KeyName = "PageHeader", KeyValue = "", Description = "Mobile Page Header", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Mobile", Title = "PageFooter", KeyName = "PageFooter", KeyValue = "© uGovernIT.com 2010-2018", Description = "Mobile Page Footer", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Mobile", Title = "DefaultAvatar", KeyName = "DefaultAvatar", KeyValue = "/Content/images/uGovernIT/default-avatar.png", Description = "Default avatar for mobile version.", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "WelcomeHeading", KeyName = "WelcomeHeading", KeyValue = "Welcome to uGovernIT - IT Governance made easy!", Description = "SMS Home Page Welcome Message", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "WelcomeDescription", KeyName = "WelcomeDescription", KeyValue = "uGovernIT is used to log, manage and track IT activities and requests for service. This is an application comprised of several modules with each section logging and tracking different types of requests. Click on a module below, or the links above to get started. Not sure where to start? Click on <b>Guide Me</b> on the left.", Description = "SMS Home Page Welcome Description", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "HideWelcomeHeading", KeyName = "HideWelcomeHeading", KeyValue = "True", Description = "Welcome Heading show/hide", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "HideWelcomeDesc", KeyName = "HideWelcomeDesc", KeyValue = "True", Description = "Welcome Description show/hide", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "HideMyTicketPanel", KeyName = "HideMyTicketPanel", KeyValue = "False", Description = "My Tickets Panel show/hide", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "HideMyTicketRolesLinks", KeyName = "HideMyTicketRolesLinks", KeyValue = "False", Description = "My Tickets Roles show/hide", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "HideMyticketPreview", KeyName = "HideMyticketPreview", KeyValue = "False", Description = "My Tickets Preview show/hide", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "HideModulePanel", KeyName = "HideModulePanel", KeyValue = "False", Description = "Module Panels show/hide", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "HideGuideMePanel", KeyName = "HideGuideMePanel", KeyValue = "False", Description = "GuideMe Panel show/hide", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "SMS Home Page", Title = "NoOfPreviewTickets", KeyName = "NoOfPreviewTickets", KeyValue = "6", Description = "# of Preview tickets in My Tickets section", Type = string.Empty });

            // For dashboard RMM
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMTypeActiveProjects", KeyName = "RMMTypeActiveProjects", KeyValue = "Projects (PMM)", Description = "Shows all current projects (PMM Projects)", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMTypeTasklists", KeyName = "RMMTypeTasklists", KeyValue = "Task Lists (TSK)", Description = "Shows all task lists (TSK Projects)", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMTypeProjectsPendingApproval", KeyName = "RMMTypeProjectsPendingApproval", KeyValue = "Project Requests (NPR)", Description = "Shows all projects pending approval (NPR Requests)", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMPastWeeksAllowed", KeyName = "RMMPastWeeksAllowed", KeyValue = "4", Description = "How many weeks back can the user enter hours (does not apply to user's manager)", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMFutureWeeksAllowed", KeyName = "RMMFutureWeeksAllowed", KeyValue = "4", Description = "How many weeks in the future can the user enter hours (applies to everyone)", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMLevel1Name", KeyName = "RMMLevel1Name", KeyValue = "Type", Description = "The label used for the highest level in RMM Allocations", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMLevel2Name", KeyName = "RMMLevel2Name", KeyValue = "Work Item", Description = "The label used for the second level in RMM Allocations", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMLevel3Name", KeyName = "RMMLevel3Name", KeyValue = "Sub Item", Description = "The label used for the third level in RMM Allocations", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMShowOriginalTaskName", KeyName = "RMMShowOriginalTaskName", KeyValue = "0", Description = "In case a PMM Task is renamed, show original name in RMM saved at time of allocation", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMTypeOther", KeyName = "RMMTypeOther", KeyValue = "Other", Description = "Name of Other/Admin category in RMM", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "MembersGroup", KeyName = "MembersGroup", KeyValue = membersGroupName, Description = "Group used to administer RMM users - members will appear in the RMM user dropdown for super-admins", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "DefaultGroup", KeyName = "DefaultGroup", KeyValue = membersGroupName, Description = "All users must be part of this group (typically uGovernIT Members) so the right permissions are set", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMDashboardView", KeyName = "RMMDashboardView", KeyValue = "RMM by Manager;#1090px*500px", Description = "Dashboard view to be show when you click on chart icon in RMM Allocations tab", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "AutoCreateRMMProjectAllocation", KeyName = "AutoCreateRMMProjectAllocation", KeyValue = "True", Description = "Auto-creates project allocation in RMM when new user is assigned a task in a project", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "UpdateResourceAllocation", KeyName = "UpdateResourceAllocation", KeyValue = "True", Description = "Key to allow Update ResourceAllocation table from ProjectEstimatedAllocation, from Admin -> Utility -> Resource Data Refresh, by Show/hide of Update Resource Allocation  button.", Type = "Bool" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "JobType", KeyName = "JobType", KeyValue = "Billable;#Overhead", Description = "Items used for JobType in JobTitle", Type = "Text" });
            
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "ERPJobIDName", KeyName = "ERPJobIDName", KeyValue = "True", Description = "if ShowERPJobID is False, show TicketID and call the column “ID”", Type = "Bool" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "ERPJobIDName", KeyName = "ERPJobIDName", KeyValue = "CMIC #", Description = "ERPJobIDName label", Type = "Text" });

            // For Mail Parser
            // Members group
            mList.Add(new ConfigurationVariable() { CategoryName = "Mail Parser", Title = "MailTokenStart", KeyName = "MailTokenStart", KeyValue = "<$", Description = "Token Start symbol for email parser", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Mail Parser", Title = "MailTokenValueSeparator", KeyName = "MailTokenValueSeparator", KeyValue = ":", Description = "Token Separator symbol for email parser", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Mail Parser", Title = "MailTokenEnd", KeyName = "MailTokenEnd", KeyValue = "$>", Description = "Token End symbol for email parser", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "Validation", Title = "DesiredCompletionDateValidation", KeyName = "DesiredCompletionDateValidation", KeyValue = "False", Description = "Validate if the Desired Completion Date can be in past", Type = string.Empty });

            // For ITG
            mList.Add(new ConfigurationVariable() { CategoryName = "ITG", Title = "ShowCompanyNameInBudgetList", KeyName = "ShowCompanyNameInBudgetList", KeyValue = "False", Description = "Show company name in ITG Budget with department", Type = string.Empty });

            // Import / Export control
            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "EnableFactTableImportExport", KeyName = "EnableFactTableImportExport", KeyValue = "False", Description = "This value decides Whether to show Import button or not on dashboard fact tables", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "EnableITGBudgetImportExport", KeyName = "EnableITGBudgetImportExport", KeyValue = "False", Description = "This value decides Whether to show Import button or not on ITG Budget Management", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "EnablePMMBudgetImportExport", KeyName = "EnablePMMBudgetImportExport", KeyValue = "False", Description = "This value decides Whether to show Import button or not on PMM Budget ", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "EnableTicketListPDFExport", KeyName = "EnableTicketListPDFExport", KeyValue = "False", Description = "Configure the visibility of PDF Export option in ticket lists", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "AllowZeroHoursOnImport", KeyName = "AllowZeroHoursOnImport", KeyValue = "False", Description = "If set to true, disables recalculation of zero estimated task hours based on duration during MS Project Import", Type = string.Empty });
            //Import Export project task
            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "EnableProjectExportImport", KeyName = "EnableProjectExportImport", KeyValue = "True", Description = "If set to true,Show Import task and Export task button during project import and export", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "MSProjectImportExportEnabled", KeyName = "MSProjectImportExportEnabled", KeyValue = "False", Description = "Controls whether Project Import/Export uses MS Project - requires MS Project to be installed on the server", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "ADReportEmail", KeyName = "ADReportEmail", KeyValue = "", Description = "Email for Received AD User Import Report", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "EnableAdminImport", KeyName = "EnableAdminImport", KeyValue = "True", Description = "Enable Import button in Admin", Type = string.Empty });

            //Import Excel for Asset
            mList.Add(new ConfigurationVariable() { CategoryName = "ImportExport", Title = "AssetAdmin", KeyName = "AssetAdmin", KeyValue = "Asset Managers", Description = "To provide import excel for asset to the particular group.", Type = string.Empty });

            // Project Management items
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "PMOGroup", KeyName = "PMOGroup", KeyValue = "PMO", Description = "Name of Project Management Group", Type = string.Empty });
            //mList.Add(new ConfigurationVariable() { CategoryName= "Project Mgt", "CarryOverNPRIdtoPMM", "True", "Keep the same ID for PMM" ,Type= string.Empty});
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "NPRBudgetMandatory", KeyName = "NPRBudgetMandatory", KeyValue = "False", Description = "If set to true requires at least one budget item in NPR request before submitting for IT Governance review", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "CreateBaselineTaskThreshold", KeyName = "CreateBaselineTaskThreshold", KeyValue = "10", Description = "If more than these many tasks are being marked as complete, then user will be prompted for create baseline", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "PMMBudgetNeedsApproval", KeyName = "PMMBudgetNeedsApproval", KeyValue = "True", Description = "If set to true, then new budget items added to PMM require approval", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", KeyName = "PMMBudgetAllowEdit", Title = "PMMBudgetAllowEdit", KeyValue = "True", Description = "If set to true, allows editing of existing (approved) budget items in PMM", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", KeyName = "PMMBudgetApprover", Title = "PMMBudgetApprover", KeyValue = "IT Steering Committee", Description = "If PMMBudgetNeedsApproval is true, this group needs to approve new items", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", KeyName = "NPRITGovApprover", Title = "NPRITGovApprover", KeyValue = "IT Governance", Description = "This groups approves NPR Requests in IT Governance Review stage", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", KeyName = "NPRITSCApprover", Title = "NPRITSCApprover", KeyValue = "IT Steering Committee", Description = "This groups approves NPR Requests in IT Steering Committee Review stage", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", KeyName = "DefaultLifeCycle", Title = "DefaultLifeCycle", KeyValue = "Agile Stages", Description = "This variable will help us to set default life cycle while creating new PMM", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "LinkActualFromRMMActual", KeyName = "LinkActualFromRMMActual", KeyValue = "False", Description = "TSK actual hour comes from RMM actual hour", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "NotifyProjectManager", KeyName = "NotifyProjectManager", KeyValue = "False", Description = "Notify Project Manager when Assignee changes the status of the task", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "OverallProjectScoreColor", KeyName = "OverallProjectScoreColor", KeyValue = "Red=50;#Yellow=90", Description = "Red Icon if <= 50, Yellow Icon if between 50 and 90, else Green", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "IncludeAllProjectMonitors", KeyName = "IncludeAllProjectMonitors", KeyValue = "False", Description = "If set to true, ALL project monitors are always included in all projects. Else Project Manager can choose monitors in PMM Import Wizard.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "ProjectCompletionUsingEstHours", KeyName = "ProjectCompletionUsingEstHours", KeyValue = "True", Description = "Calculate project % completion based on estimated hours or duration of task", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "DefaultSprintDuration", KeyName = "DefaultSprintDuration", KeyValue = "10", Description = "Default sprint duration", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "AllowAddFromMyTasks", KeyName = "AllowAddFromMyTasks", KeyValue = "True", Description = "Allow addition of new tasks from My Tasks on home page", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "SprintChartView", KeyName = "SprintChartView", KeyValue = "SprintChart", Description = "Sprint Burndown Chart View", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "PreventEstimatedHoursEdit", KeyName = "PreventEstimatedHoursEdit", KeyValue = "False", Description = "Prevent editing of Estimated Hours field once it has non-zero hours", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "NotifyPMMManagerOnStart", KeyName = "NotifyPMMManagerOnStart", KeyValue = "True", Description = "Notify PMM Manager on Project start.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "AttachTaskCalenderEntry", KeyName = "AttachTaskCalenderEntry", KeyValue = "True", Description = "Attach calendar entry to task assignment notification.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "TaskReminderDefaultDays", KeyName = "TaskReminderDefaultDays", KeyValue = "1", Description = "Set default task reminder days before due date.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "TaskReminderDefaultInterval", KeyName = "TaskReminderDefaultInterval", KeyValue = "7", Description = "Set default task reminder interval.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "TaskReminderDefaultEnable", KeyName = "TaskReminderDefaultEnable", KeyValue = "True", Description = "Enable task reminders by default", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "ProjectHealthAlertsEnable", KeyName = "ProjectHealthAlertsEnable", KeyValue = "False", Description = "Send alerts to monitor project health", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "ProjectSimilarityColorScale", KeyName = "ProjectSimilarityColorScale", KeyValue = "Red=40;#Yellow=70", Description = "Red Icon if <= 40, Yellow Icon if between 40 and 70, else Green.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "OnePagerMilestonesOnly", KeyName = "OnePagerMilestonesOnly", KeyValue = "False", Description = "Show milestone only if set to true", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "ProjectComparisonMatrixColor", KeyName = "ProjectComparisonMatrixColor", KeyValue = "#FF5757=20;#FFEF5F=40;#96DF56=80;#6BA538=100", Description = "Each item is separated by `;` It is the combimation of Hex Color and Score Max Value (Min value can be 0 OR the previous item Max Value).", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "Project Mgt", Title = "MinimumSimilarityScore", KeyName = "MinimumSimilarityScore", KeyValue = "30", Description = "Filter the projects by equal to OR greater than Minimum score.", Type = "Text" });
            // Services
            mList.Add(new ConfigurationVariable() { CategoryName = "Services", Title = "ShowServiceControl", KeyName = "ShowServiceControl", KeyValue = "True", Description = "Configure the visibility of the Service control", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Services", Title = "ServiceLookupLists", KeyName = "ServiceLookupLists", KeyValue = "Applications,TicketImpact,TicketSeverity,TicketPriority,Department,Location,ACRTypes,FunctionalAreas,ProjectClass,ProjectInitiative", Description = "Lookup Lists used in Services", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Services", Title = "NumServicesPerCategory", KeyName = "NumServicesPerCategory", KeyValue = "10", Description = "Number of top services shown in each category in the service catalog", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Services", Title = "ServiceChoiceQuestionPickLists", KeyName = "ServiceChoiceQuestionPickLists", KeyValue = "ACRTicket,TSRTicket,NPRRequest", Description = "Selected lists used in Single choice question", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Services", Title = "NotifyTaskAssignee", KeyName = "NotifyTaskAssignee", KeyValue = "True", Description = "Notify Assignee when SVC task is assigned to resource", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Services", Title = "EnableSVCTaskHold", KeyName = "EnableSVCTaskHold", KeyValue = "True", Description = "Enable hold functionality for SVC tasks", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Services", Title = "DisableSVCNewSubTasks", KeyName = "DisableSVCNewSubTasks", KeyValue = "False", Description = "Disable addition of SVC sub-tasks" });
            mList.Add(new ConfigurationVariable() { CategoryName = "Services", Title = "DisableSVCNewSubTasks", KeyName = "DisableSVCNewSubTickets", KeyValue = "False", Description = "Disable addition of SVC new item and sub-tickets" });
            mList.Add(new ConfigurationVariable() { CategoryName = "Services", Title = "NewTenantServiceTicket", KeyName = "NewTenantServiceTicket", KeyValue = "False", Description = "Enable Service Ticket for new Tenant Creation", Type = string.Empty });

            // IT Analytics
            //mList.Add(new ConfigurationVariable() { CategoryName= "Analytics", "AnalyticUrl", "http://track.ugovernit.com:85", "Analytics Url" ,Type= string.Empty});
            //mList.Add(new ConfigurationVariable() { CategoryName= "Analytics", "AnalyticAuth", "", "Analytics Credential" ,Type= string.Empty});

            //Wiki
            mList.Add(new ConfigurationVariable() { CategoryName = "Wiki", Title = "WikiCreators", KeyName = "WikiCreators", KeyValue = "Admins", Description = "Configure the wiki creator", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Wiki", Title = "WikiOwners", KeyName = "WikiOwners", KeyValue = "Admins", Description = "Configure the wiki owners", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Wiki", Title = "WikiSurveyScore", KeyName = "WikiSurveyScore", KeyValue = "True", Description = "Configure whether the score is  based on survey or it is net promoter score.", Type = string.Empty });

            //Document Management
            mList.Add(new ConfigurationVariable() { CategoryName = "Document Mgmt", Title = "DownloadOnly", KeyName = "DownloadOnly", KeyValue = "True", Description = "Configure the DownloadOnly option", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Document Mgmt", Title = "PageSize", KeyName = "PageSize", KeyValue = "15", Description = "Sets the page size for number of documents to show.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Document Mgmt", Title = "CompanyName", KeyName = "CompanyName", KeyValue = "", Description = "Sets the company name to show in reports", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Document Mgmt", Title = "LogoImagePath", KeyName = "LogoImagePath", KeyValue = "~/Content/images/uGovernIT/EDM_32x32.png", Description = "Sets the logo of EDM Module", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Document Mgmt", Title = "LogoLinkUrl", KeyName = "LogoLinkUrl", KeyValue = "", Description = "Sets the link url of EDM, else defaults to showing help.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Document Mgmt", Title = "SequenceNo", KeyName = "SequenceNo", KeyValue = "1", Description = "Sets the sequence for assigning it to document and later update the config SequenceNo..", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Document Mgmt", Title = "DefaultFolders", KeyName = "DefaultFolders", KeyValue = "Interview;#Proposal;#RFP", Description = "Default Folders (;# seperated) to be created.", Type = string.Empty });

            // User Management Group
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "FBAMembershipProvider", KeyName = "FBAMembershipProvider", KeyValue = "FBAMembership", Description = "FBA Membership Provider Name", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "UserAccountCreationFormatToken", KeyName = "UserAccountCreationFormatToken", KeyValue = "[$FirstInitial$][$None$][$LastName$]", Description = "Format for AD User Account", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "AutoCreateFBAUser", KeyName = "AutoCreateFBAUser", KeyValue = "False", Description = "Allows to manage FBA User", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "EnableImportUsers", KeyName = "EnableImportUsers", KeyValue = "False", Description = "This value decides Whether to show User Import button or not on RMM, Only for Admin", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "DefaultPassword", KeyName = "DefaultPassword", KeyValue = ConfigData.Password, Description = "Default user password, leave blank to generate random password", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "DefaultUserRole", KeyName = "DefaultUserRole", KeyValue = "", Description = "New users added from RMM default to this role. Example: End Users", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "ADAdminCredential", KeyName = "ADAdminCredential", KeyValue = "", Description = "Active Directory Admin Credentials. ", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "ADUserCredential", KeyName = "ADUserCredential", KeyValue = "", Description = "User Name & Password Details for AD.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "ImportADUserGroup", KeyName = "ImportADUserGroup", KeyValue = "uGovernIT Members", Description = "Import AD User into Group.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "AutoCreateNewUser", KeyName = "AutoCreateNewUser", KeyValue = "False", Description = "True value will create user in either location machine or AD", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "EnableADPasswordReset", KeyName = "EnableADPasswordReset", KeyValue = "true", Description = "Enable password reset functionality for admins", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "PasswordExpirationPeriod", KeyName = "PasswordExpirationPeriod", KeyValue = "60", Description = "Password expiration interval in days", Type = string.Empty });

            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", KeyName = "ForcePasswordChangeForNewUsers", Title = "ForcePasswordChangeForNewUsers", KeyValue = "False", Description = "Set to True to force new users to change password.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "User Management", Title = "DisableChangePassword", KeyName = "DisableChangePassword", KeyValue = "False", Description = "Set to True to disable user password change functionality for on-prem installations using AD integration. NOTE: this is always disabled when using ADFS authentication.", Type = string.Empty });

            //FTP Credential Group
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "FTPBaseUrl", KeyName = "FTPBaseUrl", KeyValue = "ftps://ftp2.caloptima.org:22/users/ugovernit/Outgoing/", Description = "Base url for FTP Location. ", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "FTPCredential", KeyName = "FTPCredential", KeyValue = "", Description = "Comma-separated User Name & Password for FTP.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "UserFile", KeyName = "UserFile", KeyValue = "PhonebookUGovernItExtract.csv", Description = "User file name with extension.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "DepartmentFile", KeyName = "DepartmentFile", KeyValue = "DepartmentUGovernItExtract.csv", Description = "Department file name with extension.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "DefaultLocation", KeyName = "DefaultLocation", KeyValue = "", Description = "Users default locations.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "FTPUserImportMapLocationToDesk", KeyName = "FTPUserImportMapLocationToDesk", KeyValue = "False", Description = "Maps Location column from import file to Desk Location instead of to Location", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "ITDepartmentNames", KeyName = "ITDepartmentNames", KeyValue = "Information Technology", Description = "IT department names separated by ';#' - the user import process will set IT flag for anyone in these departments.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "EnableUserSyncTimerJob", KeyName = "EnableUserSyncTimerJob", KeyValue = "False", Description = "Set to True to enable BCS user sync timer job.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "AssetFile", KeyName = "AssetFile", KeyValue = "assets", Description = "Prefix of asset file", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "EnableAssetSyncTimerJob", KeyName = "EnableAssetSyncTimerJob", KeyValue = "False", Description = "Set to True to enable asset sync timer job", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "FTPArchiveFolderUrl", KeyName = "FTPArchiveFolderUrl", KeyValue = "Processed", Description = "Archived folder name used to save processed files after import from ftp", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "FTPFolderUrl", KeyName = "FTPArchiveFolderUrl", KeyValue = "/ftproot/UGovernUpload", Description = "FTP Folder URL", Type = string.Empty });


            



            //Groups
            mList.Add(new ConfigurationVariable() { CategoryName = "Group", Title = "SetUserGroup", KeyName = "SetUserGroup", KeyValue = "User;", Description = "Set User Group", Type = string.Empty });

            //Task Workflow
            mList.Add(new ConfigurationVariable() { CategoryName = "Task Workflow", Title = "NameOfWorkflowStepToShowTask", KeyName = "NameOfWorkflowStepToShowTask", KeyValue = "In Progress;", Description = "Work flow on which u need to set task", Type = string.Empty });

            //contract
            mList.Add(new ConfigurationVariable() { CategoryName = "Contract", Title = "RemoveContractExpiryAfter", KeyName = "RemoveContractExpiryAfter", KeyValue = "30", Description = "Remove Contract After Expiry form the Message board.", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Contract", Title = "ShowContractExpiryBefore", KeyName = "ShowContractExpiryBefore", KeyValue = "30", Description = "Show Contract before the Expiry on message board.", Type = string.Empty });

            // Vendor Management
            mList.Add(new ConfigurationVariable() { CategoryName = "Vendor Management", Title = "VNDDocumentAuthors", KeyName = "VNDDocumentAuthors", KeyValue = "ITMS", Description = "Vendor Management Document Library Authors", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Vendor Management", Title = "VNDDocumentReaders", KeyName = "VNDDocumentReaders", KeyValue = "ITMS", Description = "Vendor Management Document Library Readers", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Vendor Management", Title = "VNDDocumentOwners", KeyName = "VNDDocumentOwners", KeyValue = "MGS", Description = "Vendor Management Document Library Owners", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "Vendor Management", Title = "VendorSLADashboard", KeyName = "VendorSLADashboard", KeyValue = "Vendor SLA Performance", Description = "Dashboard used to show Vendor SLA Performance", Type = string.Empty });

            //Account uniquesness through email and username
            mList.Add(new ConfigurationVariable() { CategoryName = "Account Uniqueness", Title = "UniqueaccountforUser", KeyName = "UniqueAccountForUser", KeyValue = "username,email", Description = "Set value same as (username,email) and separated by comma", Type = string.Empty });


            // New Keys for ucoreM
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "CompanyEmailKey", KeyName = "CompanyEmailKey", KeyValue = "BCCI", Description = "", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "E-mail", Title = "EnableCommentNotification", KeyName = "EnableCommentNotification", KeyValue = "False", Description = "Enable notification to action users when comment is added to a ticket", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "ProcoreBaseUrl", KeyName = "ProcoreBaseUrl", KeyValue = "https://app.procore.com/vapid/", Description = "ProcoreBaseUrl", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "ProcoreClientId", KeyName = "ProcoreClientId", KeyValue = "", Description = "Add Procore client id", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "ProcoreClientSecretId", KeyName = "ProcoreClientSecretId", KeyValue = "", Description = "Add Procore client app secret Id", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "ProcoreRefreshToken", KeyName = "ProcoreRefreshToken", KeyValue = "", Description = "Add procore refresh token here", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "FTP Credential", Title = "ProcoreToken", KeyName = "ProcoreToken", KeyValue = "", Description = "Add procore token here", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ProcoreCompanyId", KeyName = "ProcoreCompanyId", KeyValue = "4272", Description = "ProcoreCompanyId", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ProcoreProjectId", KeyName = "ProcoreProjectId", KeyValue = "205958", Description = "ProcoreProjectId", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "EnableUpdateTicketsOnWorkflowChange", KeyName = "EnableUpdateTicketsOnWorkflowChange", KeyValue = "True", Description = "Enable button to update existing tickets after workflow change", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "NotifyAuthorBeforeDeletion", KeyName = "NotifyAuthorBeforeDeletion", KeyValue = "False", Description = "", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "NotifyOwnerBeforeDeletion", KeyName = "NotifyOwnerBeforeDeletion", KeyValue = "False", Description = "", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "NotifyOwnerOnDocUpload", KeyName = "NotifyOwnerOnDocUpload", KeyValue = "False", Description = "", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "CacheOn", KeyName = "CacheOn", KeyValue = "True", Description = "Turns Global Cache on/off - keep on for better performance", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "CloseLeadWhileCreatingOpportunity", KeyName = "CloseLeadWhileCreatingOpportunity", KeyValue = "True", Description = "", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "AgentDefaultGroup", KeyName = "AgentDefaultGroup", KeyValue = "APM, Project Manager, Superintendent", Description = "APM, Project Manager, Superintendent", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "AllocationTimeLineColor", KeyName = "AllocationTimeLineColor", KeyValue = "LEM;##CCFFCC,OPM;##00FF00,PMM;##808080,CPR;##93E9FF,CNS;##800080,TSK;##333333,Time Off;##FF0000,Shared Services;##999999,Lights-On Support;##C0C0C0,Other;##FFFFFF", Description = "", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "EnablePMMTaskAllocation", KeyName = "EnablePMMTaskAllocation", KeyValue = "True", Description = "Allow allocating at the project task level (as opposed to the project level) in RMM", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "ProjectCapacityDescriptor", KeyName = "ProjectCapacityDescriptor", KeyValue = "#", Description = "Project Capacity Descriptor", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "ResourceAllocationColor", KeyName = "ResourceAllocationColor", KeyValue = "Gray=0;#Green=80;#Yellow=119;#Red=120", Description = "Red if >= 120, Yellow if between 50 and 119, else Green and if there is 0 then Gray.", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RevenueCapacityDescriptor", KeyName = "RevenueCapacityDescriptor", KeyValue = "$M", Description = "Revenue Capacity Descriptor", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RevenueCapacityScaleFactor", KeyName = "RevenueCapacityScaleFactor", KeyValue = "1000000", Description = "Revenue Capacity Scale Factor", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMUpload-Category", KeyName = "RMMUpload-Category", KeyValue = "Category", Description = "RMMUpload-Category => Category", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMUpload-EndDate", KeyName = "RMMUpload-EndDate", KeyValue = "End Date", Description = "RMMUpload-EndDate => End Date", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMUpload-FirstName", KeyName = "RMMUpload-FirstName", KeyValue = "First Name", Description = "RMMUpload-FirstName =>First Name", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMUpload-Hours", KeyName = "RMMUpload-Hours", KeyValue = "Hours", Description = "RMMUpload-Hours", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMUpload-LastName", KeyName = "RMMUpload-LastName", KeyValue = "Last Name", Description = "RMMUpload-LastName => Last Name", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMUpload-StartDate", KeyName = "RMMUpload-StartDate", KeyValue = "Start Date", Description = "RMMUpload-StartDate => Start Date", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "RMMUpload-SubCategory", KeyName = "RMMUpload-SubCategory", KeyValue = "Description", Description = "RMMUpload-SubCategory => Description", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "ResourceSelectFilter", KeyName = "ResourceSelectFilter", KeyValue = "projecttype=true;#Capacity=true;#Complexity=true;#Opportunities=false", Description = "Items used for filter resource", Type = "" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "EnableRMMAssignment", KeyName = "EnableRMMAssignment", KeyValue = "True", Description = "Show/Hide Assignment radio button under RMM Resource Utilization tab", Type = "Bool" });
            mList.Add(new ConfigurationVariable() { CategoryName = "RMM", Title = "DefaultRMMDisplayMode", KeyName = "DefaultRMMDisplayMode", KeyValue = "Weekly", Description = "This is used to set default display mode either monthly or weekly." });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "SuccessChance", KeyName = "SuccessChance", KeyValue = "Cold;#Warm;#Hot", Description = "Items used for Lead Priority", Type = "Text" });

            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "LastSequenceProjectID", KeyName = "LastSequenceProjectID", KeyValue = "0", Description = "Last number used to create Project ID. Based on this new Project ID is generated.", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ProjectIDFormat", KeyName = "ProjectIDFormat", KeyValue = "YY-NNNN", Description = "YY-NNNN  YY- Last 2 digits of current Year  NNNN - Sequence number format.", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "DeletedChoices", KeyName = "DeletedChoices", KeyValue = "CRMBusinessUnit;#1-41 Service", Description = "Hides Choices for New Records separated with ;#  and ,  eg., CRMBusinessUnit;#1-41 Service;#1-31 Structures,  OpportunityType;#Negotiated Project;#Qualifications    (  Choice1;#Item1;#Item2,Choice2;#Item1;#Item2,Choice3;#Item1;#Item2  )", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "FilterClosedFields", KeyName = "FilterClosedFields", KeyValue = "ApplicationMultiLookup;#CRMCompanyLookup;#LeadSource", Description = "Removes Closed Items for New Records, for Fields separated with ;#", Type = "Text" });

            mList.Add(new ConfigurationVariable() { CategoryName = "Asset Management", Title = "AssetUniqueTag", KeyName = "AssetUniqueTag", KeyValue = "AssetTagNum", Description = "Unique identifying column in Assets table.", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AccessTokenExpiration", KeyName = "AccessTokenExpiration", KeyValue = "14", Description = "It will manage access token expiration", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "APIAllowPartialUpdate", KeyName = "APIAllowPartialUpdate", KeyValue = "True", Description = "API- It will allow user to partially create and update record", Type = "bool" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "ShortNameCharacters", KeyName = "ShortNameCharacters", KeyValue = "20", Description = "This is used set a limit that how many chars are allowed in shortname field, this only applicable for CPR,CNS and OPM", Type = string.Empty });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AllocationImportEnabled", KeyName = "AllocationImportEnabled", KeyValue = "False", Description = "Hide/Show ImportAllocation button", Type = "bool" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "AllowRecreatingExperienceTags", KeyName = "AllowRecreatingExperienceTags", KeyValue = "False", Description = "Allow Recreating Experience Tags", Type = "bool" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "DefaultAllocationPercentage", KeyName = "DefaultAllocationPercentage", KeyValue = "10", Description = "Default Allocation Percentage.", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "DefaultNormalizedScore", KeyName = "DefaultNormalizedScore", KeyValue = "100", Description = "Default Normalized Score for Project Comparison.", Type = "Text" });
            mList.Add(new ConfigurationVariable() { CategoryName = "General", Title = "DefaultComparisonMetricType", KeyName = "DefaultComparisonMetricType", KeyValue = "Similarity", Description = "Default Metric Type for Project Comparison.", Type = "Text" });

            var configurationVariableManager = new ConfigurationVariableManager(context);

            mList.ForEach(x => x.TenantID = context.TenantID);

            configurationVariableManager.InsertItems(mList);
            //  uGITDAL.InsertItem(mList);

            //configurationVariableManager.ExecuteStoredProcedure(context.TenantID, context.TenantAccountId);

            return mList;
        }

        internal void ExecuteStoredProcedures(ApplicationContext context)
        {
            var configurationVariableManager = new ConfigurationVariableManager(context);
            configurationVariableManager.ExecuteStoredProcedure(context.TenantID, context.TenantAccountId);
        }

        public List<State> GetStates(ApplicationContext context)
        {
            List<State> mList = new List<State>();

            // Console.WriteLine("  States");
            mList.Add(new State() { Title = "Alabama", StateCode = "AL", Country = "USA" });
            mList.Add(new State() { Title = "Alaska", StateCode = "AK", Country = "USA" });
            mList.Add(new State() { Title = "American Samoa", StateCode = "AS", Country = "USA" });
            mList.Add(new State() { Title = "Arizona", StateCode = "AZ", Country = "USA" });
            mList.Add(new State() { Title = "Arkansas", StateCode = "AR", Country = "USA" });
            mList.Add(new State() { Title = "California", StateCode = "CA", Country = "USA" });
            mList.Add(new State() { Title = "Colorado", StateCode = "CO", Country = "USA" });
            mList.Add(new State() { Title = "Connecticut", StateCode = "CT", Country = "USA" });
            mList.Add(new State() { Title = "Delaware", StateCode = "DE", Country = "USA" });
            mList.Add(new State() { Title = "Dist. of Columbia", StateCode = "DC", Country = "USA" });
            mList.Add(new State() { Title = "Florida", StateCode = "FL", Country = "USA" });
            mList.Add(new State() { Title = "Georgia", StateCode = "GA", Country = "USA" });
            mList.Add(new State() { Title = "Guam", StateCode = "GU", Country = "USA" });
            mList.Add(new State() { Title = "Hawaii", StateCode = "HI", Country = "USA" });
            mList.Add(new State() { Title = "Idaho", StateCode = "ID", Country = "USA" });
            mList.Add(new State() { Title = "Illinois", StateCode = "IL", Country = "USA" });
            mList.Add(new State() { Title = "Indiana", StateCode = "IN", Country = "USA" });
            mList.Add(new State() { Title = "Iowa", StateCode = "IA", Country = "USA" });
            mList.Add(new State() { Title = "Kansas", StateCode = "KS", Country = "USA" });
            mList.Add(new State() { Title = "Kentucky", StateCode = "KY", Country = "USA" });
            mList.Add(new State() { Title = "Louisiana", StateCode = "LA", Country = "USA" });
            mList.Add(new State() { Title = "Maine", StateCode = "ME", Country = "USA" });
            mList.Add(new State() { Title = "Maryland", StateCode = "MD", Country = "USA" });
            mList.Add(new State() { Title = "Marshall Islands", StateCode = "MH", Country = "USA" });
            mList.Add(new State() { Title = "Massachusetts", StateCode = "MA", Country = "USA" });
            mList.Add(new State() { Title = "Michigan", StateCode = "MI", Country = "USA" });
            mList.Add(new State() { Title = "Micronesia", StateCode = "FM", Country = "USA" });
            mList.Add(new State() { Title = "Minnesota", StateCode = "MN", Country = "USA" });
            mList.Add(new State() { Title = "Mississippi", StateCode = "MS", Country = "USA" });
            mList.Add(new State() { Title = "Missouri", StateCode = "MO", Country = "USA" });
            mList.Add(new State() { Title = "Montana", StateCode = "MT", Country = "USA" });
            mList.Add(new State() { Title = "Nebraska", StateCode = "NE", Country = "USA" });
            mList.Add(new State() { Title = "Nevada", StateCode = "NV", Country = "USA" });
            mList.Add(new State() { Title = "New Hampshire", StateCode = "NH", Country = "USA" });
            mList.Add(new State() { Title = "New Jersey", StateCode = "NJ", Country = "USA" });
            mList.Add(new State() { Title = "New Mexico", StateCode = "NM", Country = "USA" });
            mList.Add(new State() { Title = "New York", StateCode = "NY", Country = "USA" });
            mList.Add(new State() { Title = "North Carolina", StateCode = "NC", Country = "USA" });
            mList.Add(new State() { Title = "North Dakota", StateCode = "ND", Country = "USA" });
            mList.Add(new State() { Title = "Northern Marianas", StateCode = "MP", Country = "USA" });
            mList.Add(new State() { Title = "Ohio", StateCode = "OH", Country = "USA" });
            mList.Add(new State() { Title = "Oklahoma", StateCode = "OK", Country = "USA" });
            mList.Add(new State() { Title = "Oregon", StateCode = "OR", Country = "USA" });
            mList.Add(new State() { Title = "Palau", StateCode = "PW", Country = "USA" });
            mList.Add(new State() { Title = "Pennsylvania", StateCode = "PA", Country = "USA" });
            mList.Add(new State() { Title = "Puerto Rico", StateCode = "PR", Country = "USA" });
            mList.Add(new State() { Title = "Rhode Island", StateCode = "RI", Country = "USA" });
            mList.Add(new State() { Title = "South Carolina", StateCode = "SC", Country = "USA" });
            mList.Add(new State() { Title = "South Dakota", StateCode = "SD", Country = "USA" });
            mList.Add(new State() { Title = "Tennessee", StateCode = "TN", Country = "USA" });
            mList.Add(new State() { Title = "Texas", StateCode = "TX", Country = "USA" });
            mList.Add(new State() { Title = "Utah", StateCode = "UT", Country = "USA" });
            mList.Add(new State() { Title = "Vermont", StateCode = "VT", Country = "USA" });
            mList.Add(new State() { Title = "Virginia", StateCode = "VA", Country = "USA" });
            mList.Add(new State() { Title = "Virgin Islands", StateCode = "VI", Country = "USA" });
            mList.Add(new State() { Title = "Washington", StateCode = "WA", Country = "USA" });
            mList.Add(new State() { Title = "West Virginia", StateCode = "WV", Country = "USA" });
            mList.Add(new State() { Title = "Wisconsin", StateCode = "WI", Country = "USA" });
            mList.Add(new State() { Title = "Wyoming", StateCode = "WY", Country = "USA" });

            StateManager stateManager = new StateManager(context);
            stateManager.InsertItems(mList);

            return mList;
        }

        public List<CRMRelationshipType> GetCRMRelationshipTypes(ApplicationContext context)
        {
            List<CRMRelationshipType> mList = new List<CRMRelationshipType>();

            mList.Add(new CRMRelationshipType() { Title = "Architect", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Broker", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Building Rep", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Client", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Construction Manager​", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Consultant", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Developer", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Engineer", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "General Contractor​", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Individuals", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Inspection Agency", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Investor", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Municipality​", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Property Manager", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Prospect​", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Real Estate Agent", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Subcontractor", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Supplier​", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Vendor", Description = "" });
            mList.Add(new CRMRelationshipType() { Title = "Other", Description = "" });

            CRMRelationshipTypeManager crmRelationshipTypeManager = new CRMRelationshipTypeManager(context);
            crmRelationshipTypeManager.InsertItems(mList);

            return mList;
        }

        public List<ModuleImpact> GetModuleImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            // Console.WriteLine("TicketImpact");
            return mList;
        }

        public List<ModulePrioirty> GetModulePrioirty()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            // Console.WriteLine("  TicketPriority");
            return mList;
        }

        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            // Console.WriteLine("  RequestPriority");
            return mList;
        }

        public List<ModuleSeverity> GetModuleSeverity()
        {
            // Console.WriteLine("  TicketSeverity");
            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            return mList;

        }

        public List<Module_StageType> GetModuleStageType(ApplicationContext context)
        {
            List<Module_StageType> mList = new List<Module_StageType>();
            // Console.WriteLine("  StageType");

            mList.Add(new Module_StageType() { Title = "Initiated", ModuleStageType = "Initiated" });
            mList.Add(new Module_StageType() { Title = "Assigned", ModuleStageType = "Assigned" });
            mList.Add(new Module_StageType() { Title = "Resolved", ModuleStageType = "Resolved" });
            mList.Add(new Module_StageType() { Title = "Tested", ModuleStageType = "Tested" });
            mList.Add(new Module_StageType() { Title = "Closed", ModuleStageType = "Closed" });

            // uGITDAL.InsertItem(mList);
            return mList;
        }

        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();
            // Console.WriteLine("  ModuleUserTypes");
            return mList;
        }

        public List<UGITModule> GetUGITModule(ApplicationContext context)
        {
            List<UGITModule> mList = new List<UGITModule>();
            // Console.WriteLine("  Modules");
            return mList;
        }

        public List<BudgetCategory> GetBudgetCategories(ApplicationContext context)
        {
            List<BudgetCategory> mList = new List<BudgetCategory>();

            // Console.WriteLine("  BudgetCategories");

            mList.Add(new BudgetCategory() { Title = "Infrastructure-Data Center", BudgetCategoryName = "Infrastructure", BudgetSubCategory = "Data Center", BudgetAcronym = "1000", BudgetCOA = "1000-001", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Infrastructure-Security", BudgetCategoryName = "Infrastructure", BudgetSubCategory = "Security", BudgetAcronym = "1000", BudgetCOA = "1000-002", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Infrastructure-Data Network", BudgetCategoryName = "Infrastructure", BudgetSubCategory = "Data Network", BudgetAcronym = "1000", BudgetCOA = "1000-003", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Infrastructure-Servers", BudgetCategoryName = "Infrastructure", BudgetSubCategory = "Servers", BudgetAcronym = "1000", BudgetCOA = "1000-004", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Infrastructure-System Software", BudgetCategoryName = "Infrastructure", BudgetSubCategory = "System Software", BudgetAcronym = "1000", BudgetCOA = "1000-005", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Infrastructure-Miscellaneous", BudgetCategoryName = "Infrastructure", BudgetSubCategory = "Miscellaneous", BudgetAcronym = "1000", BudgetCOA = "1000-006", IncludesStaffing = false, BudgetType = "Lights On" });

            mList.Add(new BudgetCategory() { Title = "Communications-Voice", BudgetCategoryName = "Communications", BudgetSubCategory = "Voice", BudgetAcronym = "1100", BudgetCOA = "1100-001", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Communications-Mobile", BudgetCategoryName = "Communications", BudgetSubCategory = "Mobile", BudgetAcronym = "1100", BudgetCOA = "1100-002", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Communications-Equipment", BudgetCategoryName = "Communications", BudgetSubCategory = "Equipment", BudgetAcronym = "1100", BudgetCOA = "1100-003", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Communications-Miscellaneous", BudgetCategoryName = "Communications", BudgetSubCategory = "Miscellaneous", BudgetAcronym = "1100", BudgetCOA = "1100-004", IncludesStaffing = false, BudgetType = "Lights On" });

            mList.Add(new BudgetCategory() { Title = "User Devices-Computer", BudgetCategoryName = "User Devices", BudgetSubCategory = "Computer", BudgetAcronym = "1200", BudgetCOA = "1200-001", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "User Devices-Mobile", BudgetCategoryName = "User Devices", BudgetSubCategory = "Mobile", BudgetAcronym = "1200", BudgetCOA = "1200-002", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "User Devices-Peripherals", BudgetCategoryName = "User Devices", BudgetSubCategory = "Peripherals", BudgetAcronym = "1200", BudgetCOA = "1200-003", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "User Devices-OfficeSoftware", BudgetCategoryName = "User Devices", BudgetSubCategory = "Office Software", BudgetAcronym = "1200", BudgetCOA = "1200-004", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "User Devices-Miscellaneous", BudgetCategoryName = "User Devices", BudgetSubCategory = "Miscellaneous", BudgetAcronym = "1200", BudgetCOA = "1200-005", IncludesStaffing = false, BudgetType = "Lights On" });

            mList.Add(new BudgetCategory() { Title = "Applications-Software License", BudgetCategoryName = "Applications", BudgetSubCategory = "Software License", BudgetAcronym = "1300", BudgetCOA = "1300-001", IncludesStaffing = false, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Applications-Miscellaneous", BudgetCategoryName = "Applications", BudgetSubCategory = "Miscellaneous", BudgetAcronym = "1300", BudgetCOA = "1300-002", IncludesStaffing = false, BudgetType = "Lights On" });

            mList.Add(new BudgetCategory() { Title = "Staffing-HelpDesk", BudgetCategoryName = "Staffing", BudgetSubCategory = "HelpDesk", BudgetAcronym = "1400", BudgetCOA = "1400-001", IncludesStaffing = true, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Staffing-Systems", BudgetCategoryName = "Staffing", BudgetSubCategory = "Systems", BudgetAcronym = "1400", BudgetCOA = "1400-002", IncludesStaffing = true, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Staffing-Application Support", BudgetCategoryName = "Staffing", BudgetSubCategory = "Application Support", BudgetAcronym = "1400", BudgetCOA = "1400-003", IncludesStaffing = true, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Staffing-Application Development", BudgetCategoryName = "Staffing", BudgetSubCategory = "Application Development", BudgetAcronym = "1400", BudgetCOA = "1400-004", IncludesStaffing = true, BudgetType = "Lights On" });
            mList.Add(new BudgetCategory() { Title = "Staffing-Miscellaneous", BudgetCategoryName = "Staffing", BudgetSubCategory = "Miscellaneous", BudgetAcronym = "1400", BudgetCOA = "1400-005", IncludesStaffing = true, BudgetType = "Lights On" });

            BudgetCategoryViewManager budgetCategoryViewManager = new BudgetCategoryViewManager(context);
            budgetCategoryViewManager.InsertItems(mList);
            //  uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<Location> GetLocation(ApplicationContext context)
        {
            List<Location> mList = new List<Location>();

            // Console.WriteLine("  Location");

            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "All Locations", LocationDescription = "All Locations" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Albert Lea", LocationDescription = "Albert Lea" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Bangalore", LocationDescription = "Bangalore" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Birmingham", LocationDescription = "Birmingham" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Boston", LocationDescription = "Boston" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Brea", LocationDescription = "Brea" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Chambersburg", LocationDescription = "Chambersburg" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Chicago", LocationDescription = "Chicago" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "City of Industry", LocationDescription = "City of Industry" });
            mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Los Angeles", LocationDescription = "Los Angeles" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Mumbai", LocationDescription = "Mumbai" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "New Delhi", LocationDescription = "New Delhi" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "New York", LocationDescription = "New York" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Ontario", LocationDescription = "Ontario" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Opelousas - Lou Ana", LocationDescription = "Opelousas - Lou Ana" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Philadelphia", LocationDescription = "Philadelphia" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Portland", LocationDescription = "Portland" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Saginaw", LocationDescription = "Saginaw" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Salem", LocationDescription = "Salem" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "San Jose", LocationDescription = "San Jose" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Seattle", LocationDescription = "Seattle" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "St. Joseph", LocationDescription = "St. Joseph" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Thornton", LocationDescription = "Thornton" });
            //mList.Add(new Location() { Country = "", Region = "", State = "", Title = "Waukesha", LocationDescription = "Waukesha" });

            LocationManager locationManager = new LocationManager(context);
            locationManager.InsertItems(mList);
            //uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<ModuleMonitorOption> GetModuleMonitorOptions(ApplicationContext context, List<ModuleMonitor> ModuleMonitorList)
        {
            List<ModuleMonitorOption> mList = new List<ModuleMonitorOption>();
            // Console.WriteLine("  ModuleMonitorOptions");

            var LookupId = ModuleMonitorList.ElementAt(0).ID;

            // Issues
            mList.Add(new ModuleMonitorOption() { Title = "Critical Issues-GreenLED", ModuleMonitorOptionName = "No major issues", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "GreenLED", ModuleMonitorMultiplier = 100 });
            mList.Add(new ModuleMonitorOption() { Title = "Critical Issues-YellowLED", ModuleMonitorOptionName = "Some issues needing attention", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "YellowLED", ModuleMonitorMultiplier = 50 });
            mList.Add(new ModuleMonitorOption() { Title = "Critical Issues-RedLED", ModuleMonitorOptionName = "One or more critical issues", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "RedLED", ModuleMonitorMultiplier = 0 });

            LookupId = ModuleMonitorList.ElementAt(1).ID;
            // Scope
            mList.Add(new ModuleMonitorOption() { Title = "Within Scope-GreenLED", ModuleMonitorOptionName = "Minimal scope changes", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "GreenLED", ModuleMonitorMultiplier = 100 });
            mList.Add(new ModuleMonitorOption() { Title = "Within Scope-YellowLED", ModuleMonitorOptionName = "Moderate scope changes not affecting cost or schedule", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "YellowLED", ModuleMonitorMultiplier = 50 });
            mList.Add(new ModuleMonitorOption() { Title = "Within Scope-RedLED", ModuleMonitorOptionName = "Major scope changes with significant impact on cost or schedule", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "RedLED", ModuleMonitorMultiplier = 0 });

            LookupId = ModuleMonitorList.ElementAt(2).ID;
            // Budget
            mList.Add(new ModuleMonitorOption() { Title = "On Budget-GreenLED", ModuleMonitorOptionName = "Budget variance within 10%", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "GreenLED", ModuleMonitorMultiplier = 100 });
            mList.Add(new ModuleMonitorOption() { Title = "On Budget-YellowLED", ModuleMonitorOptionName = "Budget variance between 10-20%", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "YellowLED", ModuleMonitorMultiplier = 50 });
            mList.Add(new ModuleMonitorOption() { Title = "On Budget-RedLED", ModuleMonitorOptionName = "Budget variance over 20%", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "RedLED", ModuleMonitorMultiplier = 0 });

            LookupId = ModuleMonitorList.ElementAt(3).ID;
            // On-Time
            mList.Add(new ModuleMonitorOption() { Title = "On Time-GreenLED", ModuleMonitorOptionName = "On schedule (within 10% variance)", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "GreenLED", ModuleMonitorMultiplier = 100 });
            mList.Add(new ModuleMonitorOption() { Title = "On Time-YellowLED", ModuleMonitorOptionName = "Some schedule lag (between 10-15%)", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "YellowLED", ModuleMonitorMultiplier = 50 });
            mList.Add(new ModuleMonitorOption() { Title = "On Time-RedLED", ModuleMonitorOptionName = "Behind schedule (variance over 15%)", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "RedLED", ModuleMonitorMultiplier = 0 });

            LookupId = ModuleMonitorList.ElementAt(4).ID;
            // Risk
            mList.Add(new ModuleMonitorOption() { Title = "GreenLED-Risk Level", ModuleMonitorOptionName = "Minimal risk", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "GreenLED", ModuleMonitorMultiplier = 100 });
            mList.Add(new ModuleMonitorOption() { Title = "YellowLED-Risk Level", ModuleMonitorOptionName = "Moderate risk but contingency mechanism in place", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "YellowLED", ModuleMonitorMultiplier = 50 });
            mList.Add(new ModuleMonitorOption() { Title = "RedLED-Risk Level", ModuleMonitorOptionName = "Significant risk with inadequate contingency mechanism", ModuleMonitorNameLookup = LookupId, ModuleMonitorOptionLEDClass = "RedLED", ModuleMonitorMultiplier = 0 });

            ModuleMonitorOptionManager moduleMonitorOptionManager = new ModuleMonitorOptionManager(context);
            moduleMonitorOptionManager.InsertItems(mList);
            // uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<ModuleMonitor> GetModuleMonitors(ApplicationContext context)
        {
            List<ModuleMonitor> mList = new List<ModuleMonitor>();
            // Console.WriteLine("  ModuleMonitors");

            // NOTE: Title is used in PMM listing (custom filter only!)
            mList.Add(new ModuleMonitor() { Title = "Iss", MonitorName = "Critical Issues", ModuleNameLookup = "PMM", IsDefault = true }); // 1
            mList.Add(new ModuleMonitor() { Title = "Sco", MonitorName = "Within Scope", ModuleNameLookup = "PMM", IsDefault = true }); // 2
            mList.Add(new ModuleMonitor() { Title = "$$$", MonitorName = "On Budget", ModuleNameLookup = "PMM", IsDefault = true }); // 3
            mList.Add(new ModuleMonitor() { Title = "Tme", MonitorName = "On Time", ModuleNameLookup = "PMM", IsDefault = true }); // 4
            mList.Add(new ModuleMonitor() { Title = "Rsk", MonitorName = "Risk Level", ModuleNameLookup = "PMM", IsDefault = true }); // 5

            ModuleMonitorManager moduleMonitorManager = new ModuleMonitorManager(context);
            moduleMonitorManager.InsertItems(mList);
            //uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<Company> GetCompany(ApplicationContext context)
        {
            List<Company> mList = new List<Company>();

            // Console.WriteLine("  Company");

            mList.Add(new Company() { Title = "Company1", GLCode = "101" });

            CompanyManager companyManager = new CompanyManager(context);
            companyManager.InsertItems(mList);
            //  uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<Department> GetDepartment(ApplicationContext context, List<Company> companyList)
        {
            var companyID = companyList.SingleOrDefault(x => x.TenantID == context.TenantID).ID;
            List<Department> mList = new List<Department>();

            // Console.WriteLine("  Department");

            mList.Add(new Department() { Title = "All Departments", DepartmentDescription = "All Departments", CompanyIdLookup = companyID, GLCode = "00" });
            mList.Add(new Department() { Title = "Information Technology", DepartmentDescription = "Information Technology", CompanyIdLookup = companyID, GLCode = "04" });
            mList.Add(new Department() { Title = "Corporate", DepartmentDescription = "Corporate", CompanyIdLookup = companyID, GLCode = "02" });
            mList.Add(new Department() { Title = "Human Resources", DepartmentDescription = "Human Resources", CompanyIdLookup = companyID, GLCode = "05" });
            mList.Add(new Department() { Title = "Marketing", DepartmentDescription = "Marketing", CompanyIdLookup = companyID, GLCode = "07" });
            mList.Add(new Department() { Title = "Sales", DepartmentDescription = "Sales", CompanyIdLookup = companyID, GLCode = "06" });
            mList.Add(new Department() { Title = "Finance and Accounting", DepartmentDescription = "Professional Services", CompanyIdLookup = companyID, GLCode = "03" });
            mList.Add(new Department() { Title = "Executive Management", DepartmentDescription = "Executive Management", CompanyIdLookup = companyID, GLCode = "01" });
            mList.Add(new Department() { Title = "Business Operations", DepartmentDescription = "Business Operations", CompanyIdLookup = companyID, GLCode = "08" });
            mList.Add(new Department() { Title = "Research and Development", DepartmentDescription = "R&D functions", CompanyIdLookup = companyID, GLCode = "09" });

            /*
            mList.Add(new Department() { Title = "Professional Services", DepartmentDescription = "Professional Services", CompanyIdLookup = companyID, GLCode = "" });
            mList.Add(new Department() { Title = "Executive Management", DepartmentDescription = "Executive Management", CompanyIdLookup = companyID, GLCode = "" });
            mList.Add(new Department() { Title = "Field Operations", DepartmentDescription = "Field Operations", CompanyIdLookup = companyID, GLCode = "" });
            mList.Add(new Department() { Title = "Project Management", DepartmentDescription = "Project Management", CompanyIdLookup = companyID, GLCode = "" });
            */

            DepartmentManager departmentManager = new DepartmentManager(context);
            departmentManager.InsertItems(mList);
            // uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<FunctionalArea> GetFunctionalAreas(ApplicationContext context, List<Department> departmentList)
        {
            List<FunctionalArea> mList = new List<FunctionalArea>();
            // Console.WriteLine("  FunctionalAreas");
            long DeptId = 0;
            var dept = departmentList.First(x => x.Title.EqualsIgnoreCase("Information Technology"));
            if (dept != null)
                DeptId = dept.ID;

            string groupId = string.Empty;
            UserProfile group = new UserProfileManager(context).GetUserByUserName("Admin");
            if (group != null)
                groupId = group.Id;

            mList.Add(new FunctionalArea() { Title = "Business Systems Support", DepartmentLookup = DeptId, Owner = groupId, FunctionalAreaDescription = "Application Support services" });
            mList.Add(new FunctionalArea() { Title = "Admimistration", DepartmentLookup = DeptId, Owner = groupId, FunctionalAreaDescription = "Includes IT Leadership and IT Admin" });
            mList.Add(new FunctionalArea() { Title = "Production Services", DepartmentLookup = DeptId, Owner = groupId, FunctionalAreaDescription = "Data Center, Backups, etc" });
            mList.Add(new FunctionalArea() { Title = "Technical Services", DepartmentLookup = DeptId, Owner = groupId, FunctionalAreaDescription = "Infrastructure, etc" });
            mList.Add(new FunctionalArea() { Title = "User Support", DepartmentLookup = DeptId, Owner = groupId, FunctionalAreaDescription = "HelpDesk, Desktop Support, etc" });
            mList.Add(new FunctionalArea() { Title = "PMO", DepartmentLookup = DeptId, Owner = groupId, FunctionalAreaDescription = "Project Management Office" });

            /*
            mList.Add(new FunctionalArea() { Title = "Architectural and Design", DepartmentLookup = 7, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Business Development", DepartmentLookup = 8, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Construction Management", DepartmentLookup = 7, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Construction Services", DepartmentLookup = 9, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Infrastructure Services", DepartmentLookup = 2, Owner = userId, FunctionalAreaDescription = "Data Center, Networking, etc." });
            mList.Add(new FunctionalArea() { Title = "IT Steering Committee", DepartmentLookup = 2, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Onboarding and Training", DepartmentLookup = 4, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Professional Services", DepartmentLookup = 7, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Project Management", DepartmentLookup = 10, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Quality Control", DepartmentLookup = 9, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Safety Management", DepartmentLookup = 9, Owner = userId, FunctionalAreaDescription = "" });
            mList.Add(new FunctionalArea() { Title = "Shared Services", DepartmentLookup = 2, Owner = userId, FunctionalAreaDescription = "Will provide the core shared services for BCCI" });
            */

            FunctionalAreasManager functionalAreasManager = new FunctionalAreasManager(context);
            functionalAreasManager.InsertItems(mList);
            //uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<MailTokenColumnName> GetMailTokenColumnName(ApplicationContext context)
        {
            List<MailTokenColumnName> mList = new List<MailTokenColumnName>();
            // Console.WriteLine("  MailTokenColumnName");

            mList.Add(new MailTokenColumnName() { KeyName = "RequestType;#Request Type;#", KeyValue = "RequestTypeLookup", Title = "RequestType;#Request Type;# RequestTypeLookup" });
            mList.Add(new MailTokenColumnName() { KeyName = "Severity;#", KeyValue = "SeverityLookup", Title = "Severity;# SeverityLookup" });
            mList.Add(new MailTokenColumnName() { KeyName = "Impact;#", KeyValue = "ImpactLookup", Title = "Impact;#ImpactLookup" });
            mList.Add(new MailTokenColumnName() { KeyName = "Location;#", KeyValue = "LocationLookup", Title = "Location;# LocationLookup" });
            mList.Add(new MailTokenColumnName() { KeyName = "DesiredCompletionDate;#Desired Completion Date;#Complete By;#", KeyValue = "DesiredCompletionDate", Title = "DesiredCompletionDate;#Desired Completion Date;#Complete By;# - DesiredCompletionDate" });
            mList.Add(new MailTokenColumnName() { KeyName = "Manager;#", KeyValue = "BusinessManager", Title = "Manager;# BusinessManager" });

            MailTokenColumnNameManager mailTokenColumnNameManager = new MailTokenColumnNameManager(context);
            mailTokenColumnNameManager.InsertItems(mList);
            // uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<ModuleColumn> GetModuleColumns(ApplicationContext context)
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            // Console.WriteLine("  ModuleColumns");

            int seqNum = 0;

            // My Tasks on home page
            mList.Add(new ModuleColumn() { Title = "MyTaskTab > Type", CategoryName = "MyTaskTab", FieldName = "ActionType", FieldDisplayName = "Type", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyTaskTab > Title", CategoryName = "MyTaskTab", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyTaskTab > Status", CategoryName = "MyTaskTab", FieldName = "Status", FieldDisplayName = "Status", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyTaskTab > % Complete", CategoryName = "MyTaskTab", FieldName = "PercentComplete", FieldDisplayName = "% Complete", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyTaskTab > Due Date", CategoryName = "MyTaskTab", FieldName = "DueDate", FieldDisplayName = "Due Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyTaskTab > Assigned To", CategoryName = "MyTaskTab", FieldName = "AssignedTo", FieldDisplayName = "Assigned To", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyTaskTab > Type", CategoryName = "MyTaskTab", FieldName = "ActionType", FieldDisplayName = "Type", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });

            // My s on home page
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Ticket ID", CategoryName = "MyHomeTab", FieldName = "TicketId", FieldDisplayName = "Ticket ID", IsDisplay = true, IsUseInWildCard = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Title", CategoryName = "MyHomeTab", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, IsUseInWildCard = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Priority", CategoryName = "MyHomeTab", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, IsUseInWildCard = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Progress", CategoryName = "MyHomeTab", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, IsUseInWildCard = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Stage", CategoryName = "MyHomeTab", FieldName = "ModuleStage", FieldDisplayName = "Stage", IsDisplay = true, IsUseInWildCard = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Age", CategoryName = "MyHomeTab", FieldName = "Age", FieldDisplayName = "Age", IsDisplay = true, IsUseInWildCard = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Closed", CategoryName = "MyHomeTab", FieldName = "CloseDate", FieldDisplayName = "Closed", IsDisplay = false, IsUseInWildCard = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum }); // Not displayed
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Target Date", CategoryName = "MyHomeTab", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, IsUseInWildCard = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Requested By", CategoryName = "MyHomeTab", FieldName = "Requestor", FieldDisplayName = "Requested By", IsDisplay = false, IsUseInWildCard = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum }); // Not displayed
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Location", CategoryName = "MyHomeTab", FieldName = "LocationLookup", FieldDisplayName = "Location", IsDisplay = false, IsUseInWildCard = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > PRP Group", CategoryName = "MyHomeTab", FieldName = "PRPGroup", FieldDisplayName = "PRP Group", IsDisplay = false, IsUseInWildCard = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum }); // Not displayed
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Waiting On", CategoryName = "MyHomeTab", FieldName = "StageActionUsers", FieldDisplayName = "Waiting On", IsDisplay = false, IsUseInWildCard = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum }); // Not displayed
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > PRP", CategoryName = "MyHomeTab", FieldName = "PRP", FieldDisplayName = "PRP", IsDisplay = false, IsUseInWildCard = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum });

            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Company", CategoryName = "MyHomeTab", FieldName = "CRMCompanyLookup", FieldDisplayName = "Company", IsDisplay = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum, TextAlignment = "Left", IsUseInWildCard = true });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > StreetAddress1", CategoryName = "MyHomeTab", FieldName = "StreetAddress1", FieldDisplayName = "Street Address", IsDisplay = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum, TextAlignment = "Left", IsUseInWildCard = true });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > Estimator", CategoryName = "MyHomeTab", FieldName = "Estimator", FieldDisplayName = "Estimator", IsDisplay = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum, TextAlignment = "Left", IsUseInWildCard = true });
            mList.Add(new ModuleColumn() { Title = "MyHomeTab > ProjectManager", CategoryName = "MyHomeTab", FieldName = "ProjectManager", FieldDisplayName = "Project Manager", IsDisplay = true, ShowInMobile = true, DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum, TextAlignment = "Left", IsUseInWildCard = true });

            // Dashboard tickets drilldown
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Ticket ID", CategoryName = "MyDashboard", FieldName = "TicketId", FieldDisplayName = "Ticket ID", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Title", CategoryName = "MyDashboard", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Priority", CategoryName = "MyDashboard", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Progress", CategoryName = "MyDashboard", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Stage", CategoryName = "MyDashboard", FieldName = "ModuleStage", FieldDisplayName = "Stage", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Age", CategoryName = "MyDashboard", FieldName = "TicketAge", FieldDisplayName = "Age", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Created", CategoryName = "MyDashboard", FieldName = "CreationDate", FieldDisplayName = "Created", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum }); // Not displayed
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Target Date", CategoryName = "MyDashboard", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Requested By", CategoryName = "MyDashboard", FieldName = "Requestor", FieldDisplayName = "Requested By", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboard > PRP Group", CategoryName = "MyDashboard", FieldName = "PRPGroup", FieldDisplayName = "PRP Group", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboard > Waiting On", CategoryName = "MyDashboard", FieldName = "StageActionUsers", FieldDisplayName = "Waiting On", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "MultiUser", FieldSequence = ++seqNum });

            // My Projects
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Attachments", CategoryName = "MyProjectTab", FieldName = "Attachments", FieldDisplayName = " ", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Project ID", CategoryName = "MyProjectTab", FieldName = "TicketId", FieldDisplayName = "Project ID", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum }); // Sort 1
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Title", CategoryName = "MyProjectTab", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Project Type", CategoryName = "MyProjectTab", FieldName = "RequestTypeLookup", FieldDisplayName = "Project Type", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Priority", CategoryName = "MyProjectTab", FieldName = "PriorityLookup", FieldDisplayName = "Priority", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Progress", CategoryName = "MyProjectTab", FieldName = "Status", FieldDisplayName = "Progress", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "ProgressBar", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Proj Mgr(s)", CategoryName = "MyProjectTab", FieldName = "ProjectManager", FieldDisplayName = "Proj Mgr(s)", IsDisplay = false, ShowInMobile = false, DisplayForClosed = true, ColumnType = "MultiUser", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > % Comp", CategoryName = "MyProjectTab", FieldName = "PctComplete", FieldDisplayName = "% Comp", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Target Date", CategoryName = "MyProjectTab", FieldName = "DesiredCompletionDate", FieldDisplayName = "Target Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Project Score", CategoryName = "MyProjectTab", FieldName = "ProjectScore", FieldDisplayName = "Project Score", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "IndicatorLight", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Project Initiative", CategoryName = "MyProjectTab", FieldName = "ProjectInitiativeLookup", FieldDisplayName = "Project Initiative", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Beneficiaries", CategoryName = "MyProjectTab", FieldName = "Beneficiaries", FieldDisplayName = "Beneficiaries", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyProjectTab > Monitor", CategoryName = "MyProjectTab", FieldName = "ProjectMonitors", FieldDisplayName = "Monitor", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum }); // Placeholder for monitors, Title is ignored

            // Dashboard Issues from panel/chart drilldown
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Title", CategoryName = "MyDashboardIssues", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Priority", CategoryName = "MyDashboardIssues", FieldName = "Priority", FieldDisplayName = "Priority", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Impact", CategoryName = "MyDashboardIssues", FieldName = "IssueImpact", FieldDisplayName = "Impact", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Status", CategoryName = "MyDashboardIssues", FieldName = "Status", FieldDisplayName = "Status", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > % Complete", CategoryName = "MyDashboardIssues", FieldName = "PercentComplete", FieldDisplayName = "% Complete", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Created", CategoryName = "MyDashboardIssues", FieldName = "Created", FieldDisplayName = "Created", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Assigned To", CategoryName = "MyDashboardIssues", FieldName = "AssignedTo", FieldDisplayName = "Assigned To", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });

            // ResourceTab
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Name", CategoryName = "ResourceTab", FieldName = "Name", FieldDisplayName = "Name", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Login Name", CategoryName = "ResourceTab", FieldName = "LoginName", FieldDisplayName = "Login Name", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Job Title", CategoryName = "ResourceTab", FieldName = "JobProfile", FieldDisplayName = "Job Title", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Email", CategoryName = "ResourceTab", FieldName = "Email", FieldDisplayName = "Email", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Phone", CategoryName = "ResourceTab", FieldName = "PhoneNumber", FieldDisplayName = "Phone", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Department", CategoryName = "ResourceTab", FieldName = "DepartmentLookup", FieldDisplayName = "Department", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Functional Area", CategoryName = "ResourceTab", FieldName = "FunctionalAreaLookup", FieldDisplayName = "Functional Area", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Location", CategoryName = "ResourceTab", FieldName = "LocationLookup", FieldDisplayName = "Location", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Desk", CategoryName = "ResourceTab", FieldName = "DeskLocation", FieldDisplayName = "Desk", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Manager", CategoryName = "ResourceTab", FieldName = "ManagerUser", FieldDisplayName = "Manager", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > IT", CategoryName = "ResourceTab", FieldName = "IsIT", FieldDisplayName = "IT", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Cons", CategoryName = "ResourceTab", FieldName = "IsConsultant", FieldDisplayName = "Cons", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Mgr", CategoryName = "ResourceTab", FieldName = "IsManager", FieldDisplayName = "Mgr", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Role", CategoryName = "ResourceTab", FieldName = "RoleName", FieldDisplayName = "Role", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > Skills", CategoryName = "ResourceTab", FieldName = "UserSkillLookup", FieldDisplayName = "Skills", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            // Internal columns not displayed but required
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > ID", CategoryName = "ResourceTab", FieldName = "ID", FieldDisplayName = "ID", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > TitleLink", CategoryName = "ResourceTab", FieldName = "TitleLink", FieldDisplayName = "TitleLink", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > ManagerLink", CategoryName = "ResourceTab", FieldName = "ManagerLink", FieldDisplayName = "ManagerLink", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyDashboardIssues > UserRole", CategoryName = "ResourceTab", FieldName = "UserRoleIdLookup", FieldDisplayName = "User Role", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });

            //Added MyAsset Category for UserProfile
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "MyAssets > Asset ID", CategoryName = "MyAssets", FieldName = "AssetTagNum", FieldDisplayName = "Asset ID", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyAssets > Asset Name", CategoryName = "MyAssets", FieldName = "AssetName", FieldDisplayName = "Asset Name", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyAssets > Asset Model", CategoryName = "MyAssets", FieldName = "AssetModelLookup", FieldDisplayName = "Asset Model", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyAssets > Hostname", CategoryName = "MyAssets", FieldName = "HostName", FieldDisplayName = "Hostname", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "MyAssets > Status", CategoryName = "MyAssets", FieldName = "Status", FieldDisplayName = "Status", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });


            //Added NPRBudget Category for NPRBudget
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "NPRBudget > Category", CategoryName = "NPRBudget", FieldName = "BudgetCategory", FieldDisplayName = "Category", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NPRBudget > SubCategory", CategoryName = "NPRBudget", FieldName = "BudgetSubCategory", FieldDisplayName = "Sub Category", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NPRBudget > BudgetItem", CategoryName = "NPRBudget", FieldName = "BudgetItem", FieldDisplayName = "Budget Item", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NPRBudget > Amount", CategoryName = "NPRBudget", FieldName = "BudgetAmount", FieldDisplayName = "Amount", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NPRBudget > StartDate", CategoryName = "NPRBudget", FieldName = "AllocationStartDate", FieldDisplayName = "Start Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NPRBudget > End Date", CategoryName = "NPRBudget", FieldName = "AllocationEndDate", FieldDisplayName = "End Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NPRBudget > Notes", CategoryName = "NPRBudget", FieldName = "BudgetDescription", FieldDisplayName = "Notes", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });


            //Added NPRBudget Category for PMMBudget
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "PMMBudget > Category", CategoryName = "PMMBudget", FieldName = "BudgetCategory", FieldDisplayName = "Category", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMBudget > SubCategory", CategoryName = "PMMBudget", FieldName = "BudgetSubCategory", FieldDisplayName = "Sub Category", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMBudget > BudgetItem", CategoryName = "PMMBudget", FieldName = "BudgetItem", FieldDisplayName = "Budget Item", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMBudget > Amount", CategoryName = "PMMBudget", FieldName = "BudgetAmount", FieldDisplayName = "Amount", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMBudget > StartDate", CategoryName = "PMMBudget", FieldName = "AllocationStartDate", FieldDisplayName = "Start Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMBudget > End Date", CategoryName = "PMMBudget", FieldName = "AllocationEndDate", FieldDisplayName = "End Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });

            //Added PmmbudgetActuals for PmmActuals
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "PMMActuals > ModuleBudgetLookup", CategoryName = "PMMActuals", FieldName = "ModuleBudgetLookup", FieldDisplayName = "Budget Item", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMActuals > BudgetDescription", CategoryName = "PMMActuals", FieldName = "BudgetDescription", FieldDisplayName = "Description", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMActuals > VendorLookup", CategoryName = "PMMActuals", FieldName = "VendorLookup", FieldDisplayName = "Vendor", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMActuals > InvoiceNumber", CategoryName = "PMMActuals", FieldName = "InvoiceNumber", FieldDisplayName = "PO / Invoive#", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMActuals > BudgetAmount", CategoryName = "PMMActuals", FieldName = "BudgetAmount", FieldDisplayName = "Amount", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMActuals > AllocationStartDate", CategoryName = "PMMActuals", FieldName = "AllocationStartDate", FieldDisplayName = "Start Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "PMMActuals > AllocationEndDate", CategoryName = "PMMActuals", FieldName = "AllocationEndDate", FieldDisplayName = "End Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });



            //Added  NprResource
            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "NprResource > UserSkillLookup", CategoryName = "NPRResource", FieldName = "UserSkillLookup", FieldDisplayName = "Skill", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NprResource > BudgetType", CategoryName = "NPRResource", FieldName = "BudgetType", FieldDisplayName = "Staff Type", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NprResource > _ResourceType", CategoryName = "NPRResource", FieldName = "_ResourceType", FieldDisplayName = "Description", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NprResource > NoOfFTEs", CategoryName = "NPRResource", FieldName = "NoOfFTEs", FieldDisplayName = "FTEs", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NprResource > AllocationStartDate", CategoryName = "NPRResource", FieldName = "AllocationStartDate", FieldDisplayName = "Start Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NprResource > AllocationEndDate", CategoryName = "NPRResource", FieldName = "AllocationEndDate", FieldDisplayName = "End Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NprResource > RequestedResources", CategoryName = "NPRResource", FieldName = "RequestedResources", FieldDisplayName = "Req. Resource(s)", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "NprResource > RoleName", CategoryName = "NPRResource", FieldName = "RoleNameChoice", FieldDisplayName = "Role Name", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });



            seqNum = 0;
            mList.Add(new ModuleColumn() { Title = "#", CategoryName = "TSKTask", FieldName = "ItemOrder", FieldDisplayName = "#", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "ID", CategoryName = "TSKTask", FieldName = "ID", FieldDisplayName = "#", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Title", CategoryName = "TSKTask", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Task Type", CategoryName = "TSKTask", FieldName = "Behaviour", FieldDisplayName = "Task Type", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "% Comp", CategoryName = "TSKTask", FieldName = "PercentComplete", FieldDisplayName = "% Comp", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Status", CategoryName = "TSKTask", FieldName = "Status", FieldDisplayName = "Status", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Assigned To", CategoryName = "TSKTask", FieldName = "AssignedTo", FieldDisplayName = "Assigned To", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Pred.", CategoryName = "TSKTask", FieldName = "Predecessors", FieldDisplayName = "Pred.", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Est. Hrs", CategoryName = "TSKTask", FieldName = "TaskEstimatedHours", FieldDisplayName = "Est. Hrs", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Act Hrs", CategoryName = "TSKTask", FieldName = "TaskActualHours", FieldDisplayName = "Act Hrs", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "ERH", CategoryName = "TSKTask", FieldName = "EstimatedRemainingHours", FieldDisplayName = "ERH", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Start Date", CategoryName = "TSKTask", FieldName = "StartDate", FieldDisplayName = "Start Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Due Date", CategoryName = "TSKTask", FieldName = "DueDate", FieldDisplayName = "Due Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Duration", CategoryName = "TSKTask", FieldName = "Duration", FieldDisplayName = "Duration", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "IsMileStone", CategoryName = "TSKTask", FieldName = "IsMileStone", FieldDisplayName = "IsMileStone", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "StageStep", CategoryName = "TSKTask", FieldName = "StageStep", FieldDisplayName = "StageStep", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "CompletionDate", CategoryName = "TSKTask", FieldName = "CompletionDate", FieldDisplayName = "CompletionDate", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Act Hrs", CategoryName = "TSKTask", FieldName = "ActualHours", FieldDisplayName = "Act Hrs", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Est. Hrs", CategoryName = "TSKTask", FieldName = "EstimatedHours", FieldDisplayName = "Est. Hrs", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });

            //columns in task list for svc
            mList.Add(new ModuleColumn() { Title = "#", CategoryName = "SVCTask", FieldName = "ItemOrder", FieldDisplayName = "#", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "ID", CategoryName = "SVCTask", FieldName = "ID", FieldDisplayName = "#", IsDisplay = false, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Title", CategoryName = "SVCTask", FieldName = "Title", FieldDisplayName = "Title", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "TicketID", CategoryName = "SVCTask", FieldName = "RelatedTicketID", FieldDisplayName = "TicketID", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Task Type", CategoryName = "SVCTask", FieldName = "Behaviour", FieldDisplayName = "Task Type", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "% Comp", CategoryName = "SVCTask", FieldName = "PercentComplete", FieldDisplayName = "% Comp", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Status", CategoryName = "SVCTask", FieldName = "Status", FieldDisplayName = "Status", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Assigned To", CategoryName = "SVCTask", FieldName = "AssignedTo", FieldDisplayName = "Assigned To", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Pred.", CategoryName = "SVCTask", FieldName = "Predecessors", FieldDisplayName = "Pred.", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Due Date", CategoryName = "SVCTask", FieldName = "DueDate", FieldDisplayName = "Due Date", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "Est. Hours", CategoryName = "SVCTask", FieldName = "EstimatedHours", FieldDisplayName = "Est. Hours", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });
            mList.Add(new ModuleColumn() { Title = "StageStep", CategoryName = "SVCTask", FieldName = "StageStep", FieldDisplayName = "StageStep", IsDisplay = true, ShowInMobile = false, DisplayForClosed = false, ColumnType = "", FieldSequence = ++seqNum });


            ModuleColumnManager moduleColumnManager = new ModuleColumnManager(context);
            moduleColumnManager.InsertItems(mList);
            //   uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<MessageBoard> GetMessageBoard(ApplicationContext context)
        {
            List<MessageBoard> mList = new List<MessageBoard>();
            // Console.WriteLine("  MessageBoard");

            // Overall Status
            mList.Add(new MessageBoard() { MessageType = "Warning", Body = "[$OverallStatus$]", Title = "[$OverallStatus$]" });
            // Messages
            mList.Add(new MessageBoard() { MessageType = "Warning", Body = "WAN traffic congestion in Santa Barbara community. Expect slow performance from 11 AM to 2 PM this weekend.", Title = "WAN traffic congestion in Santa Barbara community. Expect slow performance from 11 AM to 2 PM this weekend." });
            mList.Add(new MessageBoard() { MessageType = "Critical", Body = "E-mail services will not be available from 11 PM to midnight today.", Title = "E-mail services will not be available from 11 PM to midnight today." });
            mList.Add(new MessageBoard() { MessageType = "Ok", Body = "All SAP systems are back up and functional.", Title = "All SAP systems are back up and functional." });
            mList.Add(new MessageBoard() { MessageType = "Information", Body = "uGovernIT is going live Dec 1st.", Title = "uGovernIT is going live Dec 1st." });

            MessageBoardManager messageBoardManager = new MessageBoardManager(context);
            messageBoardManager.InsertItems(mList);
            // uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<AssetVendor> GetAssetVendor(ApplicationContext context)
        {
            long vendorType = 0;

            List<AssetVendor> mList = new List<AssetVendor>();

            VendorTypeManager _vendorTypeManager = new VendorTypeManager(context);

            //var lstVendortype = _vendorTypeManager.Load().Where(x=>x.Title.ToLower().Contains("computer")).FirstOrDefault();
            var lstVendortype = _vendorTypeManager.Load($"{DatabaseObjects.Columns.Title}='computer'and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").FirstOrDefault();

            if (lstVendortype != null)
            {
                vendorType = lstVendortype.id;
            }

            mList.Add(new AssetVendor() { VendorName = "Dell", VendorTypeLookup = vendorType });
            mList.Add(new AssetVendor() { VendorName = "Lenovo", VendorTypeLookup = vendorType });
            mList.Add(new AssetVendor() { VendorName = "Apple", VendorTypeLookup = vendorType });
            mList.Add(new AssetVendor() { VendorName = "HP", VendorTypeLookup = vendorType });

            AssetVendorViewManager assetVendorViewManager = new AssetVendorViewManager(context);

            assetVendorViewManager.InsertItems(mList);
            // uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<AssetModel> GetAssetModel(ApplicationContext context)
        {
            List<AssetModel> mList = new List<AssetModel>();
            long assetVendorId = 0;

            AssetVendorViewManager assetVendorViewManager = new AssetVendorViewManager(context);

            AssetVendor assetVendor = assetVendorViewManager.Load($"{DatabaseObjects.Columns.VendorName}='apple'and {DatabaseObjects.Columns.TenantID}='{context.TenantID}'").FirstOrDefault();

            if (assetVendor != null)
            {
                assetVendorId = assetVendor.ID;
            }

            mList.Add(new AssetModel() { ModelName = "Inspiron", Title = "Inspiron" });
            mList.Add(new AssetModel() { ModelName = "Thinkpad", Title = "Thinkpad", VendorLookup = assetVendorId });
            mList.Add(new AssetModel() { ModelName = "Mackbook Pro", Title = "Mackbook Pro", VendorLookup = assetVendorId });
            mList.Add(new AssetModel() { ModelName = "iPad", Title = "iPad", VendorLookup = assetVendorId });
            mList.Add(new AssetModel() { ModelName = "iMac", Title = "iMac", VendorLookup = assetVendorId });
            mList.Add(new AssetModel() { ModelName = "Mac mini", Title = "Mac mini", VendorLookup = assetVendorId });

            AssetModelViewManager assetModelViewManager = new AssetModelViewManager(context);

            assetModelViewManager.InsertItems(mList);
            // uGITDAL.InsertItem(mList);
            return mList;
        }

        public List<VendorType> GetVendorType(ApplicationContext context)
        {
            List<VendorType> mList = new List<VendorType>();

            mList.Add(new VendorType() { Title = "Application", Deleted = false });
            mList.Add(new VendorType() { Title = "Computer", Deleted = false });
            mList.Add(new VendorType() { Title = "Infrastructor", Deleted = false });
            mList.Add(new VendorType() { Title = "Software", Deleted = false });

            VendorTypeManager vendorTypeManager = new VendorTypeManager(context);
            vendorTypeManager.InsertItems(mList);

            return mList;
        }

        public List<ClientAdminConfigurationList> GetClientAdminConfigurationLists(ApplicationContext context, List<ClientAdminCategory> ClientAdminCategories)
        {
            List<ClientAdminConfigurationList> mList = new List<ClientAdminConfigurationList>();

            // Console.WriteLine("ClientAdminConfigurationLists");

            int seqNum = 0;
            var ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(0).ID;

            // Initial Setup
            mList.Add(new ClientAdminConfigurationList() { Title = "Configuration Variables", ListName = "Config_ConfigurationVariable", Description = "Miscellaneous Configuration Variables", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Modules", ListName = "Config_Modules", Description = "Module Configuration", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Form Layout", ListName = "Config_Module_FormLayout", Description = "Form Layout Configuration", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Request Lists", ListName = "Config_Module_ModuleColumns", Description = "Request List Columns Configuration", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "User Types", ListName = "Config_Module_ModuleUserTypes", Description = "User Type configuration for each module", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Departments", ListName = "Department", Description = "List of Departments", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Functional Areas", ListName = "FunctionalAreas", Description = "List of Functional Areas within each deplartment", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Locations", ListName = "Location", Description = "List of locations/sites/facilities", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Module Defaults", ListName = "Config_Module_DefaultValues", Description = "List of Module Default Values", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Edit Choices", ListName = "addchoices", Description = "Edit Choice Field Options", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Phrases", ListName = "phrasesview", Description = "List of phrases for similar Ticket search", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Widgets", ListName = "widgets", Description = "create and edit widgets", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            ///need to work mList.Add(new ClientAdminConfigurationLists() { Title =  "Migrate", "EnableMigrate", "Migrate to production", TabSequence =++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });

            ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(1).ID;
            // Workflows & Request Types
            mList.Add(new ClientAdminConfigurationList() { Title = "Request Types", ListName = "Config_Module_RequestType", Description = "Request Types & Owners", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "ACR Types", ListName = "ACRTypes", Description = "ACR Type values", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "DRQ Rapid Types", ListName = "DRQRapidTypes", Description = "DRQ Rapid Request Types", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "DRQ System Areas", ListName = "DRQSystemAreas", Description = "DRQ System Areas", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Workflows", ListName = "Config_Module_ModuleStages", Description = "Module Workflow Configuration", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Stage Exit Criteria", ListName = "ModuleStageConstraintTemplates", Description = "Configure stage exit criteria", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            //mList.Add(new ClientAdminConfigurationLists() { Title =  "Stage Skip Criteria ", "modulestagerules", "Configure stage skipping rules", TabSequence =++seqNum, ClientAdminCategoryLookup = 2 });
            mList.Add(new ClientAdminConfigurationList() { Title = "Environment", ListName = "Environment", Description = "List of Application Environments", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Sub Location", ListName = "SubLocation", Description = "List of Sub Locations", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Quick Tickets / Macros", ListName = "Templates", Description = "Create & edit Ticket Templates for Quick Tickets & Macros", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            //mList.Add(new ClientAdminConfigurationList() { Title = "Event Categories", ListName = "Config_EventCategories", Description = "List of Application Environments", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });

            //need to work mList.Add(new ClientAdminConfigurationLists() { Title =  "User Management", "ResourceManagement", "Manage Users & Groups", TabSequence =++seqNum, "2" });
            ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(2).ID;

            // Priorities & SLAs
            mList.Add(new ClientAdminConfigurationList() { Title = "Impact", ListName = "Config_Module_Impact", Description = "Impact values for each module (used to calculate priority)", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Severity", ListName = "Config_Module_Severity", Description = "Severity values for each module (used to calculate priority)", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Priority", ListName = "Config_Module_Priority", Description = "Priority values for each module", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Priority Mapping", ListName = "Config_Module_RequestPriority", Description = "Impact-Severity-Priority Mapping for each module", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "SLA Rules", ListName = "Config_Module_SLARule", Description = "SLAs for each module", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "SLA Escalations", ListName = "Config_Module_EscalationRule", Description = "SLA Escalation Rules & E-mail formats", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Holiday Calendar", ListName = "HolidaysAndWorkDaysCalendar ", Description = "Company Holiday Calendar", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Email-to-Ticket", ListName = "Emails", Description = "IMAP server credentials for email-to-ticket", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Email Notifications", ListName = "Config_Module_TaskEmails", Description = "Email Notification formats for each module step", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "SMTP Credentials", ListName = "SmtpMail", Description = "SMTP server credentials for email", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });

            ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(3).ID;
            // Scheduler & Surveys
            mList.Add(new ClientAdminConfigurationList() { Title = "Scheduled Actions", ListName = "SchedulerActions", Description = "Scheduled Actions", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Survey", ListName = "Survey", Description = "Show Surveys", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Survey Feedback", ListName = "SurveyFeedbackLink", Description = "Show feedback against survey", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });


            ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(4).ID;

            // Information
            mList.Add(new ClientAdminConfigurationList() { Title = "Message Board", ListName = "MessageBoard", Description = "Show message list", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "uGovernIT Logs", ListName = "Log", Description = "Show uGovernIT log entries", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Menu Navigation", ListName = "Config_MenuNavigation", Description = "Configure Top Menu", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Page Editor", ListName = "Config_PageConfiguration", Description = "Page Editor", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Delete Items", ListName = "DeleteTickets", Description = "Delete tickets & dependent entries", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Wiki Categories", ListName = "Config_WikiLeftNavigation", Description = "Configure Wiki Categories", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Scheduler Jobs", ListName = "SchedulerJob", Description = "show scheduled jobs dashboard", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });

            ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(5).ID;

            // Document Management
            mList.Add(new ClientAdminConfigurationList() { Title = "Document Categories", ListName = "DMDocInfoType", Description = "Set all the allowed documents extensions by category", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Document Types", ListName = "DMDocumentTypeList", Description = "Document Types, such as Templates, SOWs, Specs, etc.", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            //mList.Add(new ClientAdminConfigurationList() { Title = "Wiki Categories", ListName = "Config_WikiLeftNavigation", Description = "Configure Wiki Categories", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Manage Help", ListName = "HelpCard", Description = "create and edit help cards", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            //mList.Add(new ClientAdminConfigurationList() { Title = "Wiki Categories", ListName = "Config_WikiLeftNavigation", Description = "Configure Wiki Categories", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });

            //dataList.Add(new string[] { "Project Types", "DMProjects", "List of Projects.", TabSequence =++seqNum, "9" });
            //dataList.Add(new string[] { "Subcontractor", "DMVendor", "List of Subcontractors", TabSequence =++seqNum, "9" });

            ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(6).ID;
            // Templates
            /*
            mList.Add(new ClientAdminConfigurationList() { Title = "Quick Tickets / Macros", ListName = "Templates", Description = "Create & edit Ticket Templates for Quick Tickets & Macros", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Task Templates", ListName = "TaskTemplates", Description = "Project Task Templates", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "CheckList Templates", ListName = "CheckListTemplates", Description = "CheckList Templates", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Ranking Criteria", ListName = "RankingCriteria", Description = "RankingCriteria", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Lead Criteria", ListName = "LeadCriteria", Description = "Lead Priority Criteria", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Project Complexity", ListName = "ProjectComplexity", Description = "Project Complexity Criteria", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            */

            // Projects
            mList.Add(new ClientAdminConfigurationList() { Title = "Project Lifecycles", ListName = "Config_ProjectLifeCycles", Description = "Project Lifecycles", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Business Initiatives", ListName = "Config_ProjectInitiative", Description = "Project Initiatives values", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Project Class", ListName = "Config_ProjectClass", Description = "Project Class values", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Task Templates", ListName = "TaskTemplates", Description = "Project Task Templates", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Project Standards", ListName = "ProjectStandardWorkItems", Description = "Project Standards Templates", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Project Similarity", ListName = "ProjectSimilarityConfig", Description = "Project Similarity", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });

            //need to check the table name      dataList.Add(new string[] { "Event Categories", "EventCategories", "Project Calendar Event Categories", TabSequence =++seqNum, "5" });
            ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(7).ID;

            // Budgets & Assets
            mList.Add(new ClientAdminConfigurationList() { Title = "Budget Categories", ListName = "Config_BudgetCategories", Description = "Budget Categories & Sub-Categories", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Vendors", ListName = "AssetVendors", Description = "Vendors for Assets, Budget Item, Contracts, etc.", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Asset Models", ListName = "AssetModels", Description = "Asset Models for PCs, PDAs, etc", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "User Skills", ListName = "UserSkills", Description = "User Skills", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "User Roles", ListName = "Roles", Description = "User Roles", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Landing Pages", ListName = "LandingPages", Description = "Landing Pages", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Job Title", ListName = "JobTitle", Description = "Job Title", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Employee Type", ListName = "EmployeeTypes", Description = "Employee Types", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            //dataList.Add(new string[] { "Vendor Resource Category", "VendorResourceCategory", "Vendor Resource Category", TabSequence =++seqNum, "6" });             

            ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(8).ID;

            // Governance
            mList.Add(new ClientAdminConfigurationList() { Title = "Dashboard Buttons", ListName = "LinkItems", Description = "Configure dashboard buttons", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Configuration", ListName = "LinkCategory", Description = "Configure links to pdf's etc", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Link Views", ListName = "LinkView", Description = "Help/Documentation Links", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            //need to check the table name   dataList.Add(new string[] { "Analytic Auth", "analyticauth", "Analytic Credentials", TabSequence =++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            //need to check the table name  dataList.Add(new string[] { "AD Admin Auth", "adminauth", "AD Admin Credential", TabSequence =++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "AD User Sync", ListName = "ADUserMapping", Description = "Active Directory Sync", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "User Management", ListName = "ResourceManagement", Description = "User Management", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });

            // ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(9).ID;

            // Procore API
            /*
            mList.Add(new ClientAdminConfigurationList() { Title = "Procore Credentials", ListName = "ProcoreCredentials", Description = "Configure Procore Credentials", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Procore Utility", ListName = "ProcoreUtility", Description = "Configure Procore Utility", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Procore Mapping", ListName = "ProcoreMapping", Description = "Configure Procore Mapping", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Token Info", ListName = "TokenInfo", Description = "Token Info", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            */
            //ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(10).ID;

            // User Management
            /*
            mList.Add(new ClientAdminConfigurationList() { Title = "User Roles", ListName = "Roles", Description = "User Roles", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "User Skills", ListName = "UserSkills", Description = "User Skills", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Landing Pages", ListName = "LandingPages", Description = "Landing Pages", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "AD Admin Auth", ListName = "ADAdminAuth", Description = "AD Admin Auth", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "User Management", ListName = "ResourceManagement", Description = "User Management", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Job Title", ListName = "JobTitle", Description = "Job Title", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            */

            //ClientAdminCategoryLookupId = ClientAdminCategories.ElementAt(11).ID;

            // Utility
            //mList.Add(new ClientAdminConfigurationList() { Title = "Resource Data Refresh", ListName = "DataRefresh", Description = "Resource Data Refresh", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });
            mList.Add(new ClientAdminConfigurationList() { Title = "Application Health", ListName = "ApplicationHealth", Description = "This will help us to show many allocations are corrupt", TabSequence = ++seqNum, ClientAdminCategoryLookup = ClientAdminCategoryLookupId });

            AdminConfigurationListManager adminConfigurationListManager = new AdminConfigurationListManager(context);
            adminConfigurationListManager.InsertItems(mList);
            // uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<ClientAdminCategory> GetClientAdminCategory(ApplicationContext context)
        {
            // Console.WriteLine("ClientAdminCategory");
            List<ClientAdminCategory> mList = new List<ClientAdminCategory>();
            /*
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Initial Setup", ItemOrder = 1, ImageUrl = "/Content/ButtonImages/ApplicationSettings.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Request Types & Workflows", ItemOrder = 2, ImageUrl = "/Content/ButtonImages/SetupUser.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Priorities & SLAs", ItemOrder = 3, ImageUrl = "/Content/ButtonImages/Clock.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Scheduler & Surveys", ItemOrder = 4, ImageUrl = "/Content/ButtonImages/Survey.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Templates", ItemOrder = 5, ImageUrl = "/Content/ButtonImages/Configproject.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Budgets & Assets", ItemOrder = 6, ImageUrl = "/Content/ButtonImages/ConfigAssets.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Governance", ItemOrder = 7, ImageUrl = "/Content/ButtonImages/ITPolicy.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Information, etc.", ItemOrder = 8, ImageUrl = "/Content/ButtonImages/Alert_YellowPerspective.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Document Management", ItemOrder = 9, ImageUrl = "/Content/ButtonImages/DocumentManagement.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Procore API", ItemOrder = 10, ImageUrl = "/Content/Images/procore_logo.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "User Management", ItemOrder = 11, ImageUrl = "/Content/ButtonImages/RMM_32x32.png" });
            mList.Add(new ClientAdminCategory() { Title = "uCorem Configuration", CategoryName = "Utility", ItemOrder = 12, ImageUrl = "/Content/Images/utility.png" });
            */

            mList.Add(new ClientAdminCategory() { Title = "Service Management", CategoryName = "Initial Setup", ItemOrder = 1, ImageUrl = "/Content/ButtonImages/ApplicationSettings.png" });
            mList.Add(new ClientAdminCategory() { Title = "Service Management", CategoryName = "Request Types & Workflows", ItemOrder = 2, ImageUrl = "/Content/ButtonImages/SetupUser.png" });
            mList.Add(new ClientAdminCategory() { Title = "Service Management", CategoryName = "Priorities & SLAs", ItemOrder = 3, ImageUrl = "/Content/ButtonImages/Clock.png" });
            mList.Add(new ClientAdminCategory() { Title = "Service Management", CategoryName = "Scheduler & Surveys", ItemOrder = 4, ImageUrl = "/Content/ButtonImages/Survey.png" });
            mList.Add(new ClientAdminCategory() { Title = "Service Management", CategoryName = "Information, etc.", ItemOrder = 5, ImageUrl = "/Content/ButtonImages/Alert_YellowPerspective.png" });
            mList.Add(new ClientAdminCategory() { Title = "Service Management", CategoryName = "Document Management", ItemOrder = 6, ImageUrl = "/Content/ButtonImages/DocumentManagement.png" });
            mList.Add(new ClientAdminCategory() { Title = "Project & Resource Management", CategoryName = "Projects", ItemOrder = 7, ImageUrl = "/Content/ButtonImages/Configproject.png" });

            //mList.Add(new ClientAdminCategory() { Title = "Service Management", CategoryName = "Templates", ItemOrder = 5, ImageUrl = "/Content/ButtonImages/Configproject.png" });
            mList.Add(new ClientAdminCategory() { Title = "Project & Resource Management", CategoryName = "Budgets & Assets", ItemOrder = 8, ImageUrl = "/Content/ButtonImages/ConfigAssets.png" });
            mList.Add(new ClientAdminCategory() { Title = "Project & Resource Management", CategoryName = "Governance", ItemOrder = 9, ImageUrl = "/Content/ButtonImages/ITPolicy.png" });

            /*
            mList.Add(new ClientAdminCategory() { Title = "Project & Resource Management", CategoryName = "Procore API", ItemOrder = 10, ImageUrl = "/Content/Images/procore_logo.png" });
            mList.Add(new ClientAdminCategory() { Title = "Project & Resource Management", CategoryName = "User Management", ItemOrder = 11, ImageUrl = "/Content/ButtonImages/RMM_32x32.png" });
            mList.Add(new ClientAdminCategory() { Title = "Project & Resource Management", CategoryName = "Utility", ItemOrder = 12, ImageUrl = "/Content/Images/utility.png" });
            */

            AdminCategoryManager adminCategoryManager = new AdminCategoryManager(context);
            adminCategoryManager.InsertItems(mList);
            //uGITDAL.InsertItem(mList);

            return mList;
        }

        public List<GenericTicketStatus> GetGenericTicketStatus(ApplicationContext context)
        {
            // Console.WriteLine("  GenericTicketStatus");
            List<GenericTicketStatus> mList = new List<GenericTicketStatus>();
            return mList;
        }

        public void UpdateTenantScheduler(ApplicationContext context)
        {
            List<TenantScheduler> mList = new List<TenantScheduler>();
            // Console.WriteLine("..Tenant Scheduler..");

            TenantScheduler rootItem = null;
            rootItem = new TenantScheduler() { CronExpression = "*/5 * * * *", JobType = "AgentJobScheduler", TenantID = context.TenantID };
            mList.Add(rootItem);

            rootItem = new TenantScheduler() { CronExpression = "0 2 * * *", JobType = "BudgetDistributionSchedular", TenantID = context.TenantID };
            mList.Add(rootItem);

            rootItem = new TenantScheduler() { CronExpression = "*/20 * * * *", JobType = "CleanupJobScheduler", TenantID = context.TenantID };
            mList.Add(rootItem);

            rootItem = new TenantScheduler() { CronExpression = "*/30 * * * *", JobType = "EmailToTicketScheduler", TenantID = context.TenantID };
            mList.Add(rootItem);

            rootItem = new TenantScheduler() { CronExpression = "0 23 * * * *", JobType = "DailyTicketCounts", TenantID = context.TenantID };
            mList.Add(rootItem);

            rootItem = new TenantScheduler() { CronExpression = "0 2 * * * *", JobType = "UpdateTicketAgeJob", TenantID = context.TenantID };
            mList.Add(rootItem);

            rootItem = new TenantScheduler() { CronExpression = "*/20 * * * *", JobType = "AssetImportJobScheduler", TenantID = context.TenantID };
            mList.Add(rootItem);

            rootItem = new TenantScheduler() { CronExpression = "0 4 * * *", JobType = "CleanupJobScheduler", TenantID = context.TenantID };
            mList.Add(rootItem);

            TenantSchedulerManager tenantSchedulerManager = new TenantSchedulerManager(context);
            tenantSchedulerManager.InsertItems(mList);
        }

        public void UpdateMenuNavigations(ApplicationContext context)
        {
            List<MenuNavigation> mList = new List<MenuNavigation>();
            // Console.WriteLine("  MenuNavigation");

            MenuNavigation rootItem = null;

            rootItem = new MenuNavigation() { MenuName = "Default", MenuParentLookup = 0, Title = "Home", MenuDisplayType = "Both", NavigationUrl = "/", IconUrl = "/Content/images/adminHome.png", NavigationType = "Navigate" };
            mList.Add(rootItem);

            rootItem = new MenuNavigation() { MenuName = "Default", MenuParentLookup = 0, Title = "Service Management", MenuDisplayType = "Both", NavigationUrl = "", IconUrl = "/Content/images/service-management.png", NavigationType = "Navigate" };
            mList.Add(rootItem);

            rootItem.Children = new List<MenuNavigation>();
            /*---------------------------------------------------------Service Management---------------------------------------------------------------------*/

            rootItem.Children = new List<MenuNavigation>();
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Services", MenuDisplayType = "Both", NavigationUrl = "/Pages/SVCRequests", IconUrl = "/Content/images/Menu/SubMenu/SVC_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Problem Resolution", MenuDisplayType = "Both", NavigationUrl = "/Pages/PRSTickets", IconUrl = "/Content/images/Menu/SubMenu/PRS_32x32.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Technical Service Request", MenuDisplayType = "Both", NavigationUrl = "/Pages/TSRTickets", IconUrl = "/Content/images/Menu/SubMenu/TSR_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Application Change Request", MenuDisplayType = "Both", NavigationUrl = "/Pages/ACRTickets", IconUrl = "/Content/images/Menu/SubMenu/ACR_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Change Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/DRQTickets", IconUrl = "/Content/images/Menu/SubMenu/DRQ_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Bug Tracking", MenuDisplayType = "Both", NavigationUrl = "/Pages/BTSTickets", IconUrl = "/Content/images/Menu/SubMenu/BTS_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Outage Incidents", MenuDisplayType = "Both", NavigationUrl = "/Pages/Incidents", IconUrl = "/Content/images/Menu/SubMenu/INC_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Root Cause Analysis", MenuDisplayType = "Both", NavigationUrl = "/Pages/RCATickets", IconUrl = "/Content/images/Menu/SubMenu/RCA_32x32.svg", NavigationType = "Navigate" });


            //root menu item
            /*---------------------------------------------------------Project Management---------------------------------------------------------------------*/

            rootItem = new MenuNavigation() { MenuName = "Default", MenuParentLookup = 0, Title = "Project Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/PMMProjects", IconUrl = "/Content/images/project-management.png", NavigationType = "Navigate", IsDisabled = false };
            mList.Add(rootItem);
            rootItem.Children = new List<MenuNavigation>();
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "New Project Requests", MenuDisplayType = "Both", NavigationUrl = "/Pages/NPRRequests", IconUrl = "/Content/images/Menu/SubMenu/NPR_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Current Projects", MenuDisplayType = "Both", NavigationUrl = "/Pages/PMMProjects", IconUrl = "/Content/images/Menu/SubMenu/PMM_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Task Lists", MenuDisplayType = "Both", NavigationUrl = "/Pages/TSKProjects", IconUrl = "/Content/images/Menu/SubMenu/TSK_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Business Initiatives", MenuDisplayType = "Both", NavigationUrl = "/Pages/Initiatives", IconUrl = "/Assets/analytics.png", NavigationType = "Navigate" });


            /*---------------------------------------------------------Resource Management---------------------------------------------------------------------*/

            rootItem = new MenuNavigation() { MenuName = "Default", Title = "Resource Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/RMM", IconUrl = "/Content/images/recource-management.png", NavigationType = "Navigate" };
            mList.Add(rootItem);

            rootItem.Children = new List<MenuNavigation>();
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Budget Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/Budgets", IconUrl = "/Content/images/Menu/SubMenu/VFM_32x32.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "People Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/RMM", IconUrl = "/Content/images/Menu/SubMenu/RMM_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Application Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/Applications", IconUrl = "/Content/images/Menu/SubMenu/APP_32x32.svg", NavigationType = "Navigate", IsDisabled = false });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Asset Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/CMDB", IconUrl = "/Content/images/Menu/SubMenu/CMDB_32x32.svg", NavigationType = "Navigate", IsDisabled = false });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Contract Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/Contracts", IconUrl = "/Content/images/Menu/SubMenu/CMT_32x32.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Document Management", MenuDisplayType = "Both", NavigationUrl = "/DocumentManagement/Repository", IconUrl = "/Content/images/Menu/SubMenu/EDM_32x32.svg", NavigationType = "Navigate", IsDisabled = true });


            /*---------------------------------------------------------Vendor Management---------------------------------------------------------------------*/

            rootItem = new MenuNavigation() { MenuName = "Default", Title = "Vendor Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/VNDRequests", IconUrl = "/Content/images/VND_32x32.png", NavigationType = "Navigate", IsDisabled = true };
            mList.Add(rootItem);

            rootItem.Children = new List<MenuNavigation>();
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Vendor MSAs", MenuDisplayType = "Both", NavigationUrl = "/Pages/VNDRequests", IconUrl = "/Content/images/Menu/SubMenu/VND_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Performance Reports", MenuDisplayType = "Both", NavigationUrl = "/Pages/VPMRequests", IconUrl = "/Content/images/Menu/SubMenu/VPM_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Invoices", MenuDisplayType = "Both", NavigationUrl = "/Pages/VFMRequests", IconUrl = "/Content/images/Menu/SubMenu/VFM_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Contract Changes", MenuDisplayType = "Both", NavigationUrl = "/Pages/VCCRequests", IconUrl = "/Content/images/Menu/SubMenu/CMT_32x32.svg", NavigationType = "Navigate" });


            rootItem = new MenuNavigation() { MenuName = "Default", MenuParentLookup = 0, Title = "Projects", MenuDisplayType = "Both", NavigationUrl = "/Pages/CRMProject", IconUrl = "/Content/images/ITService.png", NavigationType = "Navigate", IsDisabled = true };
            mList.Add(rootItem);

            rootItem.Children = new List<MenuNavigation>();

            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Construction Projects", MenuDisplayType = "Both", NavigationUrl = "/Pages/CRMProject", IconUrl = "/Content/images/Menu/SubMenu/SVC_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Service Projects", MenuDisplayType = "Both", NavigationUrl = "/Pages/CRMServices", IconUrl = "/Content/images/Menu/SubMenu/PRS_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Permits", MenuDisplayType = "Both", NavigationUrl = "/Pages/Permits", IconUrl = "/Content/images/Menu/SubMenu/TSR_32x32.svg", NavigationType = "Navigate", IsDisabled = true });

            rootItem = new MenuNavigation() { MenuName = "Default", Title = "Resources", MenuDisplayType = "Both", NavigationUrl = "/Pages/RMM", IconUrl = "/Content/images/RMM_32x32.png", NavigationType = "Navigate" };
            mList.Add(rootItem);


            rootItem = new MenuNavigation() { MenuName = "Default", MenuParentLookup = 0, Title = "CRM", MenuDisplayType = "Both", NavigationUrl = "/Pages/Company", IconUrl = "/Content/images/ITService.png", NavigationType = "Navigate", IsDisabled = true };
            mList.Add(rootItem);

            rootItem.Children = new List<MenuNavigation>();

            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Contact Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/Contacts", IconUrl = "/Content/images/Menu/SubMenu/SVC_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Company Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/Company", IconUrl = "/Content/images/Menu/SubMenu/PRS_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Lead Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/Lead", IconUrl = "/Content/images/Menu/SubMenu/TSR_32x32.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Opportunities", MenuDisplayType = "Both", NavigationUrl = "/Pages/Opportunity", IconUrl = "/Content/images/Menu/SubMenu/TSR_32x32.svg", NavigationType = "Navigate", IsDisabled = true });

            /*
            rootItem = new MenuNavigation() { MenuName = "Default", MenuParentLookup = 0, Title = "Concierge", MenuDisplayType = "Both", NavigationUrl = "/Pages/TSRTickets", IconUrl = "/Content/images/Menu/SubMenu/TSR_32x32.svg", NavigationType = "Navigate", IsDisabled = true };
            mList.Add(rootItem);

            rootItem.Children = new List<MenuNavigation>();

            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Technical Service Request", MenuDisplayType = "Both", NavigationUrl = "/Pages/TSRTickets", IconUrl = "/Content/images/Menu/SubMenu/TSR_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Asset Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/CMDB", IconUrl = "/Content/images/Menu/SubMenu/PRS_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Document Management", MenuDisplayType = "Both", NavigationUrl = "/DocumentManagement/Repository", IconUrl = "//Content/ButtonImages/DocumentManagement.png", NavigationType = "Navigate" });
            */

            rootItem = new MenuNavigation() { MenuName = "Default", MenuParentLookup = 0, Title = "Enterprise Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/PMMProjects", IconUrl = "/Content/images/Menu/SubMenu/PMM_32x32.svg", NavigationType = "Navigate", IsDisabled = true };
            mList.Add(rootItem);

            rootItem.Children = new List<MenuNavigation>();

            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Contract Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/Contracts", IconUrl = "/Content/images/Menu/SubMenu/SVC_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Budget Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/Budgets", IconUrl = "/Content/images/Menu/SubMenu/VFM_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Task Lists", MenuDisplayType = "Both", NavigationUrl = "/Pages/TSKProjects", IconUrl = "/Content/images/Menu/SubMenu/TSK_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "New Program Requests", MenuDisplayType = "Both", NavigationUrl = "/Pages/NPRRequests", IconUrl = "/Content/images/Menu/SubMenu/NPR_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Programs", MenuDisplayType = "Both", NavigationUrl = "/Pages/PMMProjects", IconUrl = "/Content/images/Menu/SubMenu/PMM_32x32.svg", NavigationType = "Navigate" });

            /*
            rootItem = new MenuNavigation() { MenuName = "Default", MenuParentLookup = 0, Title = "Technology", MenuDisplayType = "Both", NavigationUrl = "/Pages/SVCRequests", IconUrl = "/Content/images/Menu/SubMenu/SVC_32x32.svg", NavigationType = "Navigate", IsDisabled = true };
            mList.Add(rootItem);

            rootItem.Children = new List<MenuNavigation>();

            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Shared Services", MenuDisplayType = "Both", NavigationUrl = "/Pages/SVCRequests", IconUrl = "/Content/images/Menu/SubMenu/SVC_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Application Change Request", MenuDisplayType = "Both", NavigationUrl = "/Pages/ACRTickets", IconUrl = "/Content/images/Menu/SubMenu/ACR_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Change Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/DRQTickets", IconUrl = "/Content/images/Menu/SubMenu/DRQ_32x32.svg", NavigationType = "Navigate" });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Outage Incidents", MenuDisplayType = "Both", NavigationUrl = "/Pages/Incidents", IconUrl = "/Content/images/Menu/SubMenu/INC_32x32.svg", NavigationType = "Navigate" });
            */

            //rootItem = new MenuNavigation() { MenuName = "Default", Title = "Dashboards & Reports", MenuDisplayType = "Both", NavigationUrl = "/", IconUrl = "/Content/images/Dashboard_32x32.png", NavigationType = "Navigate" };
            //mList.Add(rootItem);

            //rootItem.Children = new List<MenuNavigation>();
            //rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Query Reports", MenuDisplayType = "Both", NavigationUrl = "/Pages/QueryReports", IconUrl = "/Content/images/Menu/SubMenu/Reports_16x16.svg", NavigationType = "Navigate" });

            /*---------------------------------------------------------Dashboards & Reports---------------------------------------------------------------------*/

            rootItem = new MenuNavigation() { MenuName = "Default", Title = "Dashboards & Reports", MenuDisplayType = "Both", NavigationUrl = "/", IconUrl = "/Content/images/dashboards-reports.png", NavigationType = "Navigate", IsDisabled = false };
            mList.Add(rootItem);

            rootItem.Children = new List<MenuNavigation>();
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "IT Governance", MenuDisplayType = "Both", NavigationUrl = "/Pages/ITG", IconUrl = "/Content/images/Menu/SubMenu/Governance.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "CIO Dashboard", MenuDisplayType = "Both", NavigationUrl = "/Pages/DashboardGov", IconUrl = "/Content/images/Menu/SubMenu/Dashboard_32x32.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Service Trends", MenuDisplayType = "Both", NavigationUrl = "/Pages/MonthlyTrends", IconUrl = "/Content/images/Menu/SubMenu/ChartIcon.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Service Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/DashboardSMS", IconUrl = "/Content/images/Menu/SubMenu/SVC_32x32.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "SLA Performance", MenuDisplayType = "Both", NavigationUrl = "/Pages/DashboardSLA", IconUrl = "/Content/images/Menu/SubMenu/ChartIcon.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Service Performance", MenuDisplayType = "Both", NavigationUrl = "/Pages/DashboardTickets", IconUrl = "/Content/images/Menu/SubMenu/SVC_32x32.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Project Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/DashboardPMO", IconUrl = "/Content/images/Menu/SubMenu/PMM_32x32.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Resource Management", MenuDisplayType = "Both", NavigationUrl = "/Pages/DashboardRMM", IconUrl = "/Content/images/Menu/SubMenu/RMM_32x32.svg", NavigationType = "Navigate", IsDisabled = true });
            rootItem.Children.Add(new MenuNavigation() { MenuName = "Default", Title = "Query Reports", MenuDisplayType = "Both", NavigationUrl = "/Pages/QueryReports", IconUrl = "/Content/images/Menu/SubMenu/Reports_16x16.svg", NavigationType = "Navigate" });


            rootItem = new MenuNavigation() { MenuName = "Default", Title = "Admin Classic", MenuDisplayType = "Both", NavigationUrl = "/Admin/Admin.aspx", IconUrl = "/Assets/Admin03.png", NavigationType = "Navigate" };
            mList.Add(rootItem);

            MenuNavigationManager menuNavigationManager = new MenuNavigationManager(context);
            foreach (MenuNavigation nav in mList)
            {
                menuNavigationManager.Insert(nav);
                if (nav.Children != null && nav.Children.Count > 0 && nav.ID > 0)
                {
                    nav.Children.ForEach(x => x.MenuParentLookup = nav.ID);
                    menuNavigationManager.InsertItems(nav.Children);
                }
            }
        }

        public void UpdateFieldConfigData(ApplicationContext context)
        {
            List<FieldConfiguration> fieldConfigData = new List<FieldConfiguration>();

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LocationLookup", ParentTableName = "Location", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Description", ParentTableName = "", ParentFieldName = "Title", Datatype = "NoteField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestorUser", ParentTableName = "", ParentFieldName = "Title", Datatype = "UserField", Data = null, DisplayChoicesControl = null, SelectionSet = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "Contracts" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "Customers" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "DashboardSummary" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "INC" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "INC_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "Investors" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "PLCRequest" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "PRS" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "PRS_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "TSR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "TSR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "UPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "UPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestSourceChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "E-mail;#Instant Message;#Phone;#Self;#Walk-up;#Wizard", DisplayChoicesControl = "DropDownMenu", TableName = "VCCRequest" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DiverseCertificationChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "SBE;#MBE;#WBE;#DBE;#VBE;#LBE;#Other", DisplayChoicesControl = null, TableName = "CRMProject" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DiverseCertificationChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "SBE;#MBE;#WBE;#DBE;#VBE;#LBE;#Other", DisplayChoicesControl = null, TableName = "Opportunity" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ContractStatusChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Awaiting Client Response;#Cancelled;#Closed;#Budgeting Negotiated", DisplayChoicesControl = null, TableName = "CRMProject" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ContractStatusChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Awaiting Client Response;#Cancelled;#Closed;#Budgeting Negotiated", DisplayChoicesControl = null, TableName = "Opportunity" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RetainageChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "0%;#5%;#10%;#Other", DisplayChoicesControl = null, TableName = "CRMProject" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RetainageChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "0%;#5%;#10%;#Other", DisplayChoicesControl = null, TableName = "Opportunity" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssetLookup", ParentTableName = "Assets", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestTypeLookup", ParentTableName = "Config_Module_RequestType", ParentFieldName = "RequestType", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, TemplateType = "RequestTypeDropdownTemplate" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OwnerUser", ParentTableName = "", ParentFieldName = "Title", Datatype = "UserField", Data = null, DisplayChoicesControl = null, SelectionSet = "User", Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "BusinessManagerUser", ParentTableName = "", ParentFieldName = "Title", Datatype = "UserField", Data = null, DisplayChoicesControl = null, SelectionSet = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "ACR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "ACR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "BTS" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "BTS_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "DashboardSummary" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "DRQ" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "DRQ_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "PRS" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "PRS_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "TSR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "TSR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "UPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "UPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorResolvedChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "VCCRequest" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "IsPrivate", ParentTableName = "", ParentFieldName = "Title", Datatype = "", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TicketImpactLookup", ParentTableName = "", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SeverityLookup", ParentTableName = "Config_Module_Severity", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PriorityLookup", ParentTableName = "Config_Module_Priority", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CustomUGChoice01", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "None", DisplayChoicesControl = "DropDownMenu" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CustomUGChoice02", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "None", DisplayChoicesControl = "DropDownMenu" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CustomUGChoice03", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "None", DisplayChoicesControl = "DropDownMenu" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CustomUGChoice04", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "None", DisplayChoicesControl = "DropDownMenu" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "ACR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "ACR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "BTS" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "BTS_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "Contracts" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "CRMCompany" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "CRMCompany_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "CRMContact" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "CRMContact_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "CRMProject" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "CRMProject_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "CRMServices" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "CRMServices_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "Customers" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "DRQ" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "DRQ_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "INC" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "INC_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "ITGovernance" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "ModuleTasks" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "ModuleTasksHistory" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "NPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "Opportunity" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "PLCRequest" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "PMM" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "PMM_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "PMMHistory" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "PmmProjectHistory" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "PRS" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "PRS_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "RCA" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "RCA_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "SVCRequests" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "TSK" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "TSK_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "TSR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "TSR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "VCCRequest" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "VendorMSA" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "VendorSLA" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "VendorSOW" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "VendorSOWInvoices" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OnHoldReasonChoice", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Waiting on User;#Waiting on Purchase;#Waiting on Vendor;#Other", DisplayChoicesControl = "DropDownMenu", TableName = "VendorVPM" });

            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionType", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Code Change;#Configuration Change;#Infrastructure Change;#Not An Issue;#Other", DisplayChoicesControl = "DropDownMenu" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactLookup", ParentTableName = "Config_Module_Impact", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "FunctionalAreaLookup", ParentTableName = "FunctionalAreas", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "APPTitleLookup", ParentTableName = "Applications", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionComments", ParentTableName = "", ParentFieldName = "Title", Datatype = "NoteField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PRPUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, SelectionSet = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ORPUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, SelectionSet = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DepartmentLookup", ParentTableName = "Department", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, TemplateType = "DepartmentDropDownTemplate" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ModuleType", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "SMS;#Governance;#Project", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OwnerBindingChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Auto;#Disabled;#ClientSide", DisplayChoicesControl = null, TableName = "Config_Modules" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "StageActionUsersUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "MultiRoles", ParentTableName = "aspnetroles", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = "GridLookUp", Multi = true, SelectionSet = "Group" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SecurityManagerUser", ParentTableName = "", ParentFieldName = "Title", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false, SelectionSet = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TextAlignment", ParentTableName = "", ParentFieldName = "Title", Datatype = "Choices", Data = "Center;#Left;#Right", DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ManagerUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            // fieldConfigData.Add(new FieldConfiguration() { FieldName = "BusinessManager", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false, SelectionSet = "User" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "StageType", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Initiated;#Assigned;#Resolved;#Tested;#Closed", DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TesterUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InitiatorUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "BudgetIdLookup", ParentTableName = "Config_BudgetCategories", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "BudgetLookup", ParentTableName = "Config_BudgetCategories", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PRPGroupUser", ParentTableName = "Config_Module_RequestType", ParentFieldName = "Title", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false, SelectionSet = "User" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestTypeOwnerUser", ParentTableName = "", ParentFieldName = "Title", Datatype = "UserField", Data = null, DisplayChoicesControl = null, SelectionSet = "User" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestTypeEscalationManagerUser", ParentTableName = "", ParentFieldName = "Title", Datatype = "UserField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CreationDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DesiredCompletionDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ACRTypeTitleLookup", ParentTableName = "ACRTypes", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "Category", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "System Issue;#Performance Issue;#UI Issue;#Data Issue;#Notification Issue;#Enhancement", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RCATypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Application;#Hardware;#Infrastructure;#Network;#Other;#Process", DisplayChoicesControl = null, TableName = "RCA" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RCATypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Application;#Hardware;#Infrastructure;#Network;#Other;#Process", DisplayChoicesControl = null, TableName = "RCA_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRBRImpactChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "yes;#No", DisplayChoicesControl = null, TableName = "DRQ" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRBRImpactChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "yes;#No", DisplayChoicesControl = null, TableName = "DRQ_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRQChangeTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New Application/Server;#Change to Application;#Change to Server;#Change to Both;#No Change", DisplayChoicesControl = null, TableName = "DRQ" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRQChangeTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New Application/Server;#Change to Application;#Change to Server;#Change to Both;#No Change", DisplayChoicesControl = null, TableName = "DRQ_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RiskChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "DRQ" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RiskChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "DRQ_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RiskChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "VendorMSA" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRReplicationChangeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Elements Being Added;#Elements Being Removed;#Other;#No Change", DisplayChoicesControl = null, TableName = "DRQ" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRReplicationChangeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Elements Being Added;#Elements Being Removed;#Other;#No Change", DisplayChoicesControl = null, TableName = "DRQ_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TestingDoneChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "yes;#No", DisplayChoicesControl = null, TableName = "DRQ" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TestingDoneChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "yes;#No", DisplayChoicesControl = null, TableName = "DRQ_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DeploymentResponsibleUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProductionVerifyResponsibleUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RollbackResponsibleUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "UsersAffectedUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AffectedUsersUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectManagerUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SponsorsUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RCAType", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Application;#Hardware;#Infrastructure;#Network;#Other;#Process", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRBRImpact", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "yes;#No", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRQChangeType", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New Application/Server;#Change to Application;#Change to Server;#Change to Both;#No Change", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Risk", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRReplicationChange", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Elements Being Added;#Elements Being Removed;#Other;#No Change", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TestingDone", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "yes;#No", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DeploymentResponsible", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProductionVerifyResponsible", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RollbackResponsible", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "UsersAffected", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AffectedUsers", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Sponsors", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectInitiativeLookup", ParentTableName = "Config_ProjectInitiative", ParentFieldName = "Title", Datatype = "Lookup", DisplayChoicesControl = null, });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "StakeHoldersUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New;#Enhancement;#Replacement;#Upgrade;#Other", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New;#Enhancement;#Replacement;#Upgrade;#Other", DisplayChoicesControl = null, TableName = "NPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New;#Enhancement;#Replacement;#Upgrade;#Other", DisplayChoicesControl = null, TableName = "PMM" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New;#Enhancement;#Replacement;#Upgrade;#Other", DisplayChoicesControl = null, TableName = "PMM_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New;#Enhancement;#Replacement;#Upgrade;#Other", DisplayChoicesControl = null, TableName = "PMMHistory" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New;#Enhancement;#Replacement;#Upgrade;#Other", DisplayChoicesControl = null, TableName = "PmmProjectHistory" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Big Data/Analytics;#Cloud Hosted Application;#Data Center Improvements;#Internally Hosted Application;#Mobile Application;#Social Media", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Big Data/Analytics;#Cloud Hosted Application;#Data Center Improvements;#Internally Hosted Application;#Mobile Application;#Social Media", DisplayChoicesControl = null, TableName = "NPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Big Data/Analytics;#Cloud Hosted Application;#Data Center Improvements;#Internally Hosted Application;#Mobile Application;#Social Media", DisplayChoicesControl = null, TableName = "PMM" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Big Data/Analytics;#Cloud Hosted Application;#Data Center Improvements;#Internally Hosted Application;#Mobile Application;#Social Media", DisplayChoicesControl = null, TableName = "PMM_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Big Data/Analytics;#Cloud Hosted Application;#Data Center Improvements;#Internally Hosted Application;#Mobile Application;#Social Media", DisplayChoicesControl = null, TableName = "PMMHistory" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Big Data/Analytics;#Cloud Hosted Application;#Data Center Improvements;#Internally Hosted Application;#Mobile Application;#Social Media", DisplayChoicesControl = null, TableName = "PmmProjectHistory" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationScopeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Well Defined;#Somewhat Understood;#Need To Define", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClassificationScopeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Well Defined;#Somewhat Understood;#Need To Define", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectComplexityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "CRMProject" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectComplexityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectComplexityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectComplexityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "PMM" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectComplexityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "PMMHistory" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectComplexityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "PmmProjectHistory" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OrganizationalImpactChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OrganizationalImpactChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OrganizationalImpactChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "PMM" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OrganizationalImpactChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "PMMHistory" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TechnologyUsabilityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "No Change;#Minor Improvement;#Moderate Improvement;#Major Improvement", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TechnologyUsabilityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "No Change;#Minor Improvement;#Moderate Improvement;#Major Improvement", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "OrganizationalImpact", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TechnologyReliabilityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "No Change;#Minor Improvement;#Moderate Improvement;#Major Improvement", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TechnologyReliabilityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "No Change;#Minor Improvement;#Moderate Improvement;#Major Improvement", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TechnologySecurityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "No Change;#Minor Improvement;#Moderate Improvement;#Major Improvement", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TechnologySecurityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "No Change;#Minor Improvement;#Moderate Improvement;#Major Improvement", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InternalCapabilityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Weak;#Acceptable;#Strong", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InternalCapabilityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Weak;#Acceptable;#Strong", DisplayChoicesControl = null, TableName = "NPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InternalCapabilityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Weak;#Acceptable;#Strong", DisplayChoicesControl = null, TableName = "PMM" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InternalCapabilityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Weak;#Acceptable;#Strong", DisplayChoicesControl = null, TableName = "PMMHistory" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "VendorSupportChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Poor;#Acceptable;#Excellent", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "VendorSupportChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Poor;#Acceptable;#Excellent", DisplayChoicesControl = null, TableName = "NPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "VendorSupportChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Poor;#Acceptable;#Excellent", DisplayChoicesControl = null, TableName = "PMM" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "VendorSupportChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Poor;#Acceptable;#Excellent", DisplayChoicesControl = null, TableName = "PMMHistory" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AdoptionRiskChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AdoptionRiskChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "PMM" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AdoptionRiskChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AdoptionRiskChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "PMMHistory" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactRevenueIncreaseChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactRevenueIncreaseChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactIncreasesProductivityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactIncreasesProductivityChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactBusinessGrowthChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactBusinessGrowthChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactReducesExpensesChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactReducesExpensesChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactReducesRiskChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactReducesRiskChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactDecisionMakingChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImpactDecisionMakingChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "BTS" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "BTS_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "CRMCompany" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "CRMCompany_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "CRMContact" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "CRMContact_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "CRMProject" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "CRMProject_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "CRMServices" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "CRMServices_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "DashboardSummary" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "Opportunity" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "PRS" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "PRS_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "RCA" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "RCA_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "TSR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "TSR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "VCCRequest" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "VendorMSA" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "VendorSLA" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "VendorSOW" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "VendorSOWInvoices" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResolutionTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Asset Management;#Code Change;#Configuration Change;#Infrastructure Change;#Security Management;#Not An Issue", DisplayChoicesControl = null, TableName = "VendorVPM" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LicenseBasisChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "CPUs;#In-House: NA;#Installed Instances;#Other;#Users", DisplayChoicesControl = null, TableName = "Applications" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AccessAdminUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApproverUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApprovalTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "None;#Any One;#All", DisplayChoicesControl = null, TableName = "ApplicationModules" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApprovalTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "None;#Any One;#All", DisplayChoicesControl = null, TableName = "Applications" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApprovalTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "None;#Any One;#All", DisplayChoicesControl = null, TableName = "Relationship" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "HostingTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Colocation;#Hosted;#In-House;#NA", DisplayChoicesControl = null, TableName = "Applications" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "FrequencyOfUpgradesChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Ad Hoc;#Annual;#Monthly;#None in Last Year;#Quarterly;#Semi-Annual", DisplayChoicesControl = null, TableName = "Applications" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "VendorTypeLookup", ParentTableName = "VendorType", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "VendorLookup", ParentTableName = "AssetVendors", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CategoryName", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Cloud Software;#Desktop Software;#ERP", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SupportedByUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ScheduleStatusChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Future;#In Progress;#Completed;#Postponed;#N/A", DisplayChoicesControl = null, TableName = "Assets" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ScheduleStatusChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Future;#In Progress;#Completed;#Postponed;#N/A", DisplayChoicesControl = null, TableName = "Assets_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SupplierChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Barracuda;#None", DisplayChoicesControl = null, TableName = "Assets" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SupplierChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Barracuda;#None", DisplayChoicesControl = null, TableName = "Assets_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssetDispositionChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New;#Setup;#In Use;#Warranty Repair;#Broken;#Retired;#Disposed", DisplayChoicesControl = null, TableName = "Assets" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssetDispositionChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "New;#Setup;#In Use;#Warranty Repair;#Broken;#Retired;#Disposed", DisplayChoicesControl = null, TableName = "Assets_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OwnerUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PreviousOwner1User", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PreviousOwner2User", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PreviousOwner3User", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RegisteredByUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InstalledByUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SetupCompletedByUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PreviousUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssetModelLookup", ParentTableName = "AssetModels", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ManufacturerChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "CANON;#APC;#Barracuda;#Brocade;#BROTHER;#CANON;#CISCO;#DELL;#EPSON;#EVAULT;#fujitsu;#GETAC;#HP;#ITHACA;#LENOVO;#LEXMARK;#MAC", DisplayChoicesControl = null, TableName = "Assets" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ManufacturerChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "CANON;#APC;#Barracuda;#Brocade;#BROTHER;#CANON;#CISCO;#DELL;#EPSON;#EVAULT;#fujitsu;#GETAC;#HP;#ITHACA;#LENOVO;#LEXMARK;#MAC", DisplayChoicesControl = null, TableName = "Assets_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImageOptionLookup", ParentTableName = "ImageSoftware", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ReplacementTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "None", DisplayChoicesControl = null, TableName = "Assets" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ReplacementTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "None", DisplayChoicesControl = null, TableName = "Assets_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "UpgradeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "", DisplayChoicesControl = null, TableName = "Assets" });
            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestTypeLookup", ParentTableName = "Config_Module_RequestType", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, TemplateType = "RequestTypeDropdownTemplate" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ReplacementAsset_SNLookup", ParentTableName = "	Assets", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AcquisitionDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SaleDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RetiredDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SetupCompletedDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RegistrationDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ReplacementOrderedDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ReplacementDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RenewalDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InstalledDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ImageInstallDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "WarrantyExpirationDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ActualReplacementDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TransferDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "StatusChangeDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "UninstallDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ReplacementDeliveryDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "DesiredCompletionDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Attachments", ParentTableName = "Documents", ParentFieldName = "Name", Datatype = "Attachments", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RoleNameChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Administrator;#Architect;#Business Analyst;#Developer;#Executive;#L&D Analyst;#Project Manager;#SME;#Stakeholder;#UAT User", DisplayChoicesControl = null, TableName = "NPRResources" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RoleNameChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Administrator;#Architect;#Business Analyst;#Developer;#Executive;#L&D Analyst;#Project Manager;#SME;#Stakeholder;#UAT User", DisplayChoicesControl = null, TableName = "UserProfile" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RequestedResourcesUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "BudgetTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Staff;#On-Site Consultants;#Off-Site Consultants", DisplayChoicesControl = null, TableName = "NPRResources" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "UserSkillLookup", ParentTableName = "UserSkills", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ModuleBudgetLookup", ParentTableName = "ModuleBudget", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "UserSkillMultiLookup", ParentTableName = "UserSkills", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssignedToUser", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SLACategoryChoice", ParentTableName = "", ParentFieldName = null, Datatype = "Choices", Data = "Assignment;#Requestor Contact;#Resolution;#Close", DisplayChoicesControl = null, TableName = "Config_Module_SLARule" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SLACategoryChoice", ParentTableName = "", ParentFieldName = null, Datatype = "Choices", Data = "Assignment;#Requestor Contact;#Resolution;#Close", DisplayChoicesControl = null, TableName = "WorkflowSLASummary" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SLADaysRoundUpDownChoice", ParentTableName = "", ParentFieldName = null, Datatype = "Choices", Data = "No-RoundOff;#Round Up;#Round Down", DisplayChoicesControl = null, TableName = "Config_Module_SLARule" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectClassLookup", ParentTableName = "Config_ProjectClass", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PctComplete", ParentTableName = "", ParentFieldName = null, Datatype = "Percentage", Data = "", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ContractValue", ParentTableName = "", ParentFieldName = null, Datatype = "Currency", Data = "", DisplayChoicesControl = null, Notation = "en-US" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DashboardPermissionUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ModuleNameLookup", ParentTableName = "Config_Modules", ParentFieldName = "Title", Datatype = "Lookup", Data = "", DisplayChoicesControl = null, Notation = null, TableName = "BaselineDetails" });


            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CategoryLookup", ParentTableName = "Config_Service_ServiceCategories", ParentFieldName = "Title", Datatype = "Lookup", Data = "", DisplayChoicesControl = null, Notation = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssetsLookup", ParentTableName = "Assets", ParentFieldName = null, Datatype = "Lookup", Data = "", DisplayChoicesControl = null, Notation = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApplicationModulesLookup", ParentFieldName = "Title", ParentTableName = "ApplicationModules", Datatype = "Lookup", Data = "", DisplayChoicesControl = null, Notation = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ServerFunctionsChoice", Data = "Application Server;#Database Server;#File Server;#Job Server;#Print Server;#Service Server;#Web Server;#Other", Datatype = "Choices", TableName = "ApplicationServers" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApplicationRoleAssignUser", Datatype = "UserField", Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApplicationModulesLookup", ParentTableName = "ApplicationModules", ParentFieldName = "Title", Datatype = "Lookup", DisplayChoicesControl = null, Notation = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApplicationRoleLookup", ParentTableName = "ApplicationRole", ParentFieldName = "Title", Datatype = "Lookup", DisplayChoicesControl = null, Notation = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssetsTitleLookup", ParentTableName = "Assets", ParentFieldName = "Title", Datatype = "Lookup", DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApplicationRoleModuleLookup", ParentTableName = "ApplicationModules", ParentFieldName = "Title", Datatype = "Lookup", Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "EscalationManagerUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "BeneficiariesLookup", ParentTableName = "Department", ParentFieldName = "Title", Datatype = "Lookup", Multi = true, TemplateType = "DepartmentDropDownTemplate" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ManagerUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CreatedByUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ModifiedByUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "FinanceManagerUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PurchasingUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LegalUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ReminderToUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "IssueTypeChoice", Datatype = "Choices", Data = "Access;#Installation;#Issue;#Others;#Uninstall;#Upgrade", TableName = "ACR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "IssueTypeChoice", Datatype = "Choices", Data = "Access;#Installation;#Issue;#Others;#Uninstall;#Upgrade", TableName = "ACR_Archive" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "IssueTypeChoice", Datatype = "Choices", Data = "Access;#Installation;#Issue;#Others;#Uninstall;#Upgrade", TableName = "TSR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "IssueTypeChoice", Datatype = "Choices", Data = "Access;#Installation;#Issue;#Others;#Uninstall;#Upgrade", TableName = "TSR_Archive" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RepeatIntervalChoice", Datatype = "Choices", Data = "Every 1 Week;#Every 2 Weeks;#Every 1 Month;#Every Quarter;#Every 6 Month", TableName = "Contracts" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "NeedReviewChoice", Datatype = "Choices", Data = "Yes;#No", TableName = "Contracts" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TermTypeChoice", Datatype = "Choices", Data = "Monthly;#Quaterly;#Yearly;#2 Year;#3 Year;#Other", TableName = "Contracts" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ReminderBody", Datatype = "NoteField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResourceUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "GroupUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "BusinessStrategyLookup", ParentTableName = "BusinessStrategy", ParentFieldName = "Title", Datatype = "Lookup" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "VendorTimeZoneChoice", Datatype = "Choices", Data = "CST;#EST;#MST;#PST;#Other", TableName = "AssetVendors" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DefaultUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "GroupsUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRQSystemsLookup", ParentTableName = "DRQSystemAreas", ParentFieldName = "Title", Datatype = "Lookup", Multi = false, TemplateType = "" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRQRapidTypeLookup", ParentTableName = "DRQRapidTypes", ParentFieldName = "Title", Datatype = "Lookup", Multi = false, TemplateType = "" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ActualStartDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TargetCompletionDate", ParentTableName = "", ParentFieldName = "", Datatype = "DateTime", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ActualCompletionDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "UserGroup", ParentTableName = "", ParentFieldName = "", Datatype = "GroupField", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "MultiModules", ParentTableName = "Config_Modules", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = "GridLookUp", Multi = true, SelectionSet = "Group" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ModuleStepLookup", ParentTableName = "Config_Module_ModuleStages", ParentFieldName = "Name", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = false });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CompanyTitleLookup", ParentTableName = "Company", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CompanyMultiLookup", ParentTableName = "Company", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DivisionLookup", ParentTableName = "CompanyDivisions", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DivisionMultiLookup", ParentTableName = "CompanyDivisions", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LocationMultiLookup", ParentTableName = "Location", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ServiceLookUp", ParentTableName = "Config_Services", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApplicationMultiLookup", ParentTableName = "Applications", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectLifeCycleLookup", ParentTableName = "Config_ModuleLifeCycles", ParentFieldName = "Name", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "NPRIdLookup", ParentTableName = "NPR", ParentFieldName = "TicketId", Datatype = "Lookup" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PMMIdLookup", ParentTableName = "PMM", ParentFieldName = "TicketId", Datatype = "Lookup" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ResourceNameUser", Datatype = "UserField" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "FunctionalAreaTitleLookup", ParentTableName = "FunctionalAreas", ParentFieldName = "Title", Datatype = "Lookup", TableName = "ResourceUsageSummaryMonthWise" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Division", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Masonry;#Concrete;#Photographer;#Recruiter​", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OwnershipType", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "LLC;#Corporation", DisplayChoicesControl = null });
            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "MasterAgreement", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TypesofWork", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Multi = true, Data = "Mixed Use;#Housing;#Tenant Improvement", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RegionsofWork", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Multi = true, Data = "Los Angeles;#San Francisco;#Oregon", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Certifications", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Customer;#Potential Vendor", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "StateLookup", ParentTableName = "State", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RelationshipTypeLookup", ParentTableName = "CRMRelationshipType", ParentFieldName = "Title", Datatype = "Lookup", Multi = true });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ContactMethod", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Any;#Email;#Bulk-Email;#Phone;#Fax;#Social Media​;#None", DisplayChoicesControl = null });
            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRMCompanyTitleLookup", ParentTableName = "CRMCompany", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ContactLookup", ParentTableName = "CRMContact", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LeadSource", ParentTableName = "CRMContact", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "LeadSource", Datatype = "UserField", Multi = false, SelectionSet = "User" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRMBusinessUnit", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "1-11 San Francisco;#1-21 South Bay;#1-31 Structures;#1-41 Service", DisplayChoicesControl = null });
            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "SuccessChance", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Cold;#Warm;#Hot", DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "EstimatedConstructionStart", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "EstimatedConstructionEnd", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "BidDueDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRMOpportunityStatus", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Awarded;#Cancelled;#Declined;#In Progress;#Lost;#On Hold;#Precon", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ChanceOfSuccess", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "(1) Less Than 30%;#(2) 30 to 50%;#(3) 50% to 80%;#(4) More Than 80%", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "WELLLevels", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Silver;#Gold;#Platinum;#TBD", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LeadLevel", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Certified;#Silver;#Gold;#Platinum;#TBD", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OpportunityType", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Budget;#Competitive Bid;#GC and Fee;#Negotiated Project;#Qualifications", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRMProjectStatus", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Awaiting Client Response;#Awaiting Drawings;#Awarded in PreCon;#Awarded Final Pricing Approved;#Bidding Competitive;#Bidding Negotiated;#Budgeting Competitive;#Budgeting Negotiated;#Cancelled;#Change Order;#Closed;#Close-Out;#In Design;#In Progress;#Lost;#On-Hold;#Passed;#Pre-Construction;#Re-Bid Competitive;#Re-Budgeting;#Re-Design;#ROM;#Service Opportunity;#Under Construction;#Value Engineering;#Service Lead", DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OwnerUser", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRMProjectComplexity", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "1;#2;#3;#4;#5", DisplayChoicesControl = null });


            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LEMIdLookup", ParentTableName = "Lead", ParentFieldName = "TicketId", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OPMIdLookup", ParentTableName = "Opportunity", ParentFieldName = "TicketId", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProposalRecipient", ParentTableName = "CRMContact", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AdditionalRecipients", ParentTableName = "CRMContact", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null, Multi = true });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PreconStartDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "UninstallDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SectorChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Aviation;#Commercial Development (Dev & Reposition);#Corporate Base Building (Infrastructure);#Corporate Interiors;#Cultural/Religious;#Education;#Government;#Healthcare;#Hospitality/Hotel;#Industrial Warehouse/Manufacturing;#Life Sciences (Pharma/BioTech);#Mission Critical/Data Centers;#Multi-Unit Residential;#Retail;#Sports & Entertainment;#Technology;#Other (New Sector)", DisplayChoicesControl = null, TableName = "CRMProject" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SectorChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Aviation;#Commercial Development (Dev & Reposition);#Corporate Base Building (Infrastructure);#Corporate Interiors;#Cultural/Religious;#Education;#Government;#Healthcare;#Hospitality/Hotel;#Industrial Warehouse/Manufacturing;#Life Sciences (Pharma/BioTech);#Mission Critical/Data Centers;#Multi-Unit Residential;#Retail;#Sports & Entertainment;#Technology;#Other (New Sector)", DisplayChoicesControl = null, TableName = "Lead" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SectorChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Aviation;#Commercial Development (Dev & Reposition);#Corporate Base Building (Infrastructure);#Corporate Interiors;#Cultural/Religious;#Education;#Government;#Healthcare;#Hospitality/Hotel;#Industrial Warehouse/Manufacturing;#Life Sciences (Pharma/BioTech);#Mission Critical/Data Centers;#Multi-Unit Residential;#Retail;#Sports & Entertainment;#Technology;#Other (New Sector)", DisplayChoicesControl = null, TableName = "Opportunity" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "QualityControlCoordinator", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LaborerSuperintendent", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Architect", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "BusinessDevelopmentManager", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ManagerServiceGroup", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "MEPCoordinator", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProgramManager", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectManagerArchitecture", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectManagerServiceGroup", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SeniorManagerAccounting", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ServiceCoordinator", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SustainbilityAssociate", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SustainbilityCoordinator", ParentTableName = "", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ClosedDateOnly", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, Multi = false, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AwardedorLossDate", ParentTableName = "", ParentFieldName = "", Datatype = "Date", Data = null, Multi = false, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRMUrgency", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Cold – 4+ Months Out;#Warm – 2-3 Months Out;#Hot – 1 Month Out", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "OwnerContractType", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Arch Only;#Preconstruction;#Design Build;#Lump Sum/Stipulated Sum;#GMP;#GMP conv to LS;#T&M;#TBD", DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Studio", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "SF Studio 1;#SF Studio 2;#SF Studio 3;#SB Studio 1;#SB Studio 2;#ST Studio 1;#ST Studio 2;#Serv Studio 1;#Serv Studio 2", DisplayChoicesControl = null });

            //Records for ParentTableName = "CRMProject"
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssistantProjectManager", ParentTableName = "CRMProject", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Superintendent", ParentTableName = "CRMProject", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "QalityControlCoordinator", ParentTableName = "CRMProject", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectExecutive", ParentTableName = "CRMProject", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectManager", ParentTableName = "CRMProject", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Estimator", ParentTableName = "CRMProject", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRM", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRMAdmin", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PreConAdmin", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "APM", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectEngineer", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "FieldOperationsManager", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SafetyManager", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "FieldEngineer", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Carpenter", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CarpenterApprentice", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CarpenterForeman", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssistantSuperintendent", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LaborerForeman", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Laborer", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PreconLeadUser", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApprenticeArchitectUser", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectAuditorUser", ParentTableName = "CRMProject", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });


            //Records for ParentTableName = "Opportunity"
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssistantProjectManager", ParentTableName = "Opportunity", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Superintendent", ParentTableName = "Opportunity", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "QalityControlCoordinator", ParentTableName = "Opportunity", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectExecutive", ParentTableName = "Opportunity", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectManager", ParentTableName = "Opportunity", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Estimator", ParentTableName = "Opportunity", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRM", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRMAdmin", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PreConAdmin", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "APM", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectEngineer", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "FieldOperationsManager", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SafetyManager", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "FieldEngineer", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Carpenter", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CarpenterApprentice", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CarpenterForeman", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "AssistantSuperintendent", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LaborerForeman", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Laborer", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PreconLeadUser", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApprenticeArchitectUser", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectAuditorUser", ParentTableName = "Opportunity", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApprenticeArchitectUser", ParentTableName = "CRMServices", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectAuditorUser", ParentTableName = "CRMServices", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApprenticeArchitectUser", ParentTableName = "PMM", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectAuditorUser", ParentTableName = "NPR", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApprenticeArchitectUser", ParentTableName = "PMM", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ProjectAuditorUser", ParentTableName = "NPR", ParentFieldName = null, Datatype = "UserField", Multi = true, Data = null, DisplayChoicesControl = null });


            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SubstatusLookup", ParentTableName = "Substatus", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "BusinessUnitLookup", ParentTableName = "BusinessUnits", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ServiceLookup", ParentTableName = "Config_Services", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "SubCategoryLookup", ParentTableName = "Config_Module_RequestType", ParentFieldName = "SubCategory", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DivisionLookup", ParentTableName = "CompanyDivisions", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            //fieldConfigData.Add(new FieldConfiguration() { FieldName = "CompanyTitleLookup", ParentTableName = "Company", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CompanyLookup", ParentTableName = "Company", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CRMCompanyLookup", ParentTableName = "CRMCompany", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DecisionMakerUser", ParentTableName = "UserField" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ContributionToStrategyChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "PaybackCostSavingsChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "CustomerBenefitChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "High;#Medium;#Low", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "RegulatoryChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "NPR" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ITLifecycleRefreshChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Yes;#No", DisplayChoicesControl = null, TableName = "NPR" });

            fieldConfigData.Add(new FieldConfiguration() { FieldName = "JobTitleLookup", ParentTableName = "JobTitle", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "UserRoleIdLookup", ParentTableName = "LandingPages", ParentFieldName = "Name", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "LocalAdminUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ITManager", ParentTableName = "", ParentFieldName = "Title", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = true });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "DRBRManagerUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "InfrastructureManagerUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ApplicationManagerUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "StageTypeChoice", ParentTableName = "", ParentFieldName = "", Datatype = "Choices", Data = "Initiated;#Assigned;#Resolved;#Tested;#Closed;#Approved", DisplayChoicesControl = null, Multi = false, TableName= "Config_Module_ModuleStages" });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "Tester2User", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false, TableName= null});
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "ITManagerUser", ParentTableName = "", ParentFieldName = "", Datatype = "UserField", Data = null, DisplayChoicesControl = null, Multi = false, TableName = null });
            fieldConfigData.Add(new FieldConfiguration() { FieldName = "TagMultiLookup", ParentTableName = "ExperiencedTags", ParentFieldName = "Title", Datatype = "Lookup", Data = null, DisplayChoicesControl = null });
            FieldConfigurationManager fieldConfigurationManager = new FieldConfigurationManager(context);
            fieldConfigurationManager.InsertItems(fieldConfigData);
            //  uGITDAL.InsertItem(fieldConfigData);
        }

        // Default Configuration for Reports//
        public void UpdateReportConfigData(ApplicationContext context)
        {
            List<ReportConfigData> reportConfigData = new List<ReportConfigData>();

            reportConfigData.Add(new ReportConfigData() { ReportType = "TicketSummary", ReportTitle = "Ticket Summary Report", FilterWidth = "600px", FilterHeight = "300px", FilterTitle = "Ticket Summary" });
            reportConfigData.Add(new ReportConfigData() { ReportType = "SummaryByTechnician", ReportTitle = "Summary By Technician", FilterWidth = "600px", FilterHeight = "300px", FilterTitle = "Ticket Summary By Technician" });
            reportConfigData.Add(new ReportConfigData() { ReportType = "WeeklyTeamReport", ReportTitle = "Weekly Team Report(TSR)", FilterWidth = "600px", FilterHeight = "300px", FilterTitle = "Weekly Team Performance" });
            reportConfigData.Add(new ReportConfigData() { ReportType = "NonPeakHoursPerformance", ReportTitle = "Non-Peak Hours Performance (TSR)", FilterWidth = "600px", FilterHeight = "300px", FilterTitle = "Non-Peak Hours Performance" });
            reportConfigData.Add(new ReportConfigData() { ReportType = "SurveyFeedbackReport", ReportTitle = "Survey Feedback Report", FilterWidth = "600px", FilterHeight = "300px", FilterTitle = "Survey Feedback Report Filter" });
            reportConfigData.Add(new ReportConfigData() { ReportType = "TSKProjectReport", ReportTitle = "Project Report (TSK)", FilterWidth = "90", FilterHeight = "90", FilterTitle = "TSK Project Report" });
            reportConfigData.Add(new ReportConfigData() { ReportType = "TaskSummary", ReportTitle = "Task Summary", FilterWidth = "830px", FilterHeight = "100", FilterTitle = "Task Summary" });
            reportConfigData.Add(new ReportConfigData() { ReportType = "ProjectReport", ReportTitle = "Project Report (PMM)", FilterWidth = "95", FilterHeight = "95", FilterTitle = "Project Report (PMM)" });
            reportConfigData.Add(new ReportConfigData() { ReportType = "OnePagerReport", ReportTitle = "One Pager Report", FilterWidth = "90", FilterHeight = "90", FilterTitle = "One Pager Report" });
            reportConfigData.Add(new ReportConfigData() { ReportType = "ProjectStatusReport", ReportTitle = "Project Status Report", FilterWidth = "90", FilterHeight = "90", FilterTitle = "Project Status Report" });

            ReportConfigManager reportConfigManager = new ReportConfigManager(context);
            reportConfigManager.InsertItems(reportConfigData);
            //uGITDAL.InsertItem(reportConfigData);
        }

        public List<ProjectSimilarityConfig> GetProjectSimilarityConfig(ApplicationContext context)
        {
            List<ProjectSimilarityConfig> mList = new List<ProjectSimilarityConfig>();
            // Console.WriteLine("  Project Similarity Config");
            mList.Add(new ProjectSimilarityConfig() { Title = "ProjectClassLookup", ColumnName = "ProjectClassLookup", StageWeight = 5, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "ClassificationType", ColumnName = "ClassificationTypeChoice", StageWeight = 5, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "Classification", ColumnName = "ClassificationChoice", StageWeight = 5, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "ClassificationSize", ColumnName = "ClassificationSizeChoice", StageWeight = 10, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "TicketClassificationScope", ColumnName = "ClassificationScopeChoice", StageWeight = 5, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "ProjectComplexity", ColumnName = "ProjectComplexityChoice", StageWeight = 5, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "OrganizationalImpact", ColumnName = "OrganizationalImpactChoice", StageWeight = 5, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "TechnologyUsability", ColumnName = "TechnologyUsabilityChoice", StageWeight = 2, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "TechnologyReliability", ColumnName = "TechnologyReliabilityChoice", StageWeight = 2, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "TechnologySecurity", ColumnName = "TechnologySecurityChoice", StageWeight = 6, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "InternalCapability", ColumnName = "InternalCapabilityChoice", StageWeight = 2, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "VendorSupport", ColumnName = "VendorSupportChoice", StageWeight = 2, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "AdoptionRisk", ColumnName = "AdoptionRiskChoice", StageWeight = 6, ColumnType = "MatchValue" });
            mList.Add(new ProjectSimilarityConfig() { Title = "ProjectEstDurationMinDays", ColumnName = "ProjectEstDurationMinDays", StageWeight = 5, ColumnType = "Ratio" });
            mList.Add(new ProjectSimilarityConfig() { Title = "ProjectEstDurationMaxDays", ColumnName = "ProjectEstDurationMaxDays", StageWeight = 5, ColumnType = "Ratio" });
            mList.Add(new ProjectSimilarityConfig() { Title = "TicketDuration", ColumnName = "Duration", StageWeight = 10, ColumnType = "Ratio" });
            mList.Add(new ProjectSimilarityConfig() { Title = "ProjectEstSizeMinHrs", ColumnName = "ProjectEstSizeMinHrs", StageWeight = 5, ColumnType = "Ratio" });
            mList.Add(new ProjectSimilarityConfig() { Title = "ProjectEstSizeMaxHrs", ColumnName = "ProjectEstSizeMaxHrs", StageWeight = 5, ColumnType = "Ratio" });
            mList.Add(new ProjectSimilarityConfig() { Title = "EstimatedHours", ColumnName = "EstimatedHours", StageWeight = 10, ColumnType = "Ratio" });

            ProjectSimilarityConfigManager projectSimilarityConfigManager = new ProjectSimilarityConfigManager(context);
            projectSimilarityConfigManager.InsertItems(mList);

            return mList;
        }

        internal void GetHelpCards(ApplicationContext context)
        {
            HelpCardContentManager helpCardContentManager = new HelpCardContentManager(context);
            HelpCardManager helpCardManager = new HelpCardManager(context);

            string ticketId = string.Empty;
            List<HelpCardContent> helpCardContents = new List<HelpCardContent>();
            List<HelpCard> helpCards = new List<HelpCard>();

            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Modify Functional Areas", Content = "<p>In this video we will show you how to configure departments for Service Prime. </p><p>Functional Areas provide a structure to break down Information technology resources, budgets and efforts (Project Request and Projects) to a lower level than department such as IT Administration, Business Systems Support, Lights On and User Support.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Set up Locations", Content = "<p>In this video we will show you how to configure Locations for Service Prime. </p><p>Locations are used to identify the physical location of a user or asset. Locations can be classified with Region/Country/State hierarchy. Locations are assigned to items such as Resources (users), tickets and Request Types. Request Type assignment can provide for workflow and resource assignment/responsibility differences for Requests.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Modify Departments", Content = "<p>In this video we will show you how to configure departments for Service Prime. </p><p>Departments are used to classify resources (users and assets) and to identify beneficiaries of task/project efforts. Generally they are the departments defined in your financial system.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "User Role Maintenance", Content = "<p>In this video we will show you how to configure User Roles for Service Prime. </p><p>User Roles provide role standardization for resource availability and assignment to project/task list efforts. User Roles are related to Job Titles in Job Title Set up. User Roles are linked to system fields, if any, through User Role Maintenance.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Job Title Maintenance", Content = "<p>In this video we will show you how to configure Job Titles for Service Prime. </p><p>Job Titles for the organization can be set up at a department level and assigned to users in User Management. Role Maintenance allows Job Titles to be linked to User Roles to provide resource availability and assignment to project/task list efforts. User Roles are set up in User Role Maintenance.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Skill Maintenance", Content = "<p>In this video we will show you how to configure User Skills for Service Prime. </p><p>User Skills can be set up and assigned to users in User Management. Skills can be assigned to Skill categories to assist in locating a skill when assigning to a user. Multiple skills can be assigned to a user.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "User Groups", Content = "<p>In this video we will show you how to configure User Groups for Service Prime. </p><p>A system user can belong to multiple User Groups. User Groups provide exclusive access to system functions (e.g. Menus, form tabs, fields) or administrative capabilities.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "User Management", Content = "<p>In this video we will show you how to set up users in Service Prime. Users will have access to system features based on User attributes and configuration of Menu Navigation and User Groups.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Request Type Overview", Content = "<p>In this wiki, we share links to a short videos where we will introduce you to configuration of Request Types. </p><p>Request Types are established to provide functions such as module ticket defaults, classification of tickets for reporting and Request List list configuration purposes.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Introduction to Governance", Content = "<p>In this wiki, we share links to a short video where we will introduce you to IT Governance and and provide a brief overview of the four established frame works: ITSM, PMO, SAFE, TBM. </p><p>We show that IT Governance is not just related to managing risk and compliance, rather it is all about leveraging Information Technology to help your enterprise achieve its goals. We will also show you how automation using Service Prime can help you implement IT governance without any onerous process to strifle your plans to innovate and transform the enterprise digitally.  This wiki will also help you share your thoughts and suggestions to other users of Service Prime.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Introduction to ITSM", Content = "<p>In this video we will show you how provides Service Prime provides IT Governance that is not cumbersome, and will help you implement service management, project management, resource and budget management to IT analytics that are needed to facilitate insights to run the technology department in order to meet business objectives. </p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Introduction to PMO", Content = "<p>In this video we will show you how provides Service Prime the tools that helps you manage the Project Management Office including individual projects as well as portfolio of projects that is aligned to meet your enterprise needs. </p><p>Projects encompass both the existing Applications and the projects that transform the business. We will also briefly cover projects analytics that are needed to facilitate insights to run the technology department in order to meet business objectives. </p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Introduction to SAFe", Content = "<p>In this video we will give a brief overview of SAFe (Scaled Agile Framework). </p><p>SAFe is a set of organization and workflow patterns for implementing agile practices based on structured guidance on roles and responsibilities, how to plan and manage the work, and values to uphold. SAFe facilitates alignment, collaboration, and delivery across large numbers of agile teams. It was formed around three primary bodies of knowledge: agile software development, lean product development, and systems architecture. We will also provide a brief overview of how Service Prime helps you implement SAFe with simple configuration.</p>" });
            helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Introduction to TBM", Content = "<p>In this video, we will provide you an overview of TBM (Technology Business Management ) and how Service Prime helps you achieve TBM. </p><p>TBM is a collaborative framework that helps businesses align their IT departments with overall business goals, an essential practice for today’s digital enterprise. TBM is not prescriptive and supports ITSM, PMBOK and SAFe.</p>" });

            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Modify Functional Areas", Category = "Admin" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Set up Locations", Category = "Admin" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Modify Departments", Category = "Admin" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "User Role Maintenance", Category = "Admin" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Job Title Maintenance", Category = "Admin" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Skill Maintenance", Category = "Admin" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "User Groups", Category = "Admin" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "User Management", Category = "Admin" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Request Type Overview", Category = "Admin" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Introduction to Governance", Category = "General" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Introduction to ITSM", Category = "General" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Introduction to PMO", Category = "General" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Introduction to SAFe", Category = "General" });
            helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Introduction to TBM", Category = "General" });

            AgentsManager agentsManager = new AgentsManager(context);
            var agent = agentsManager.Load(x => x.Title.EqualsIgnoreCase("Add Asset Type")).FirstOrDefault();
            if (agent != null)
            {
                helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Add Asset Type", Content = "<p>Add Asset Type<p>", AgentContent = $"<div class='row'> <div class='col-md-6' title='create a new project' onclick='handleWidget({agent.Id})'><a id=9 ><img src='/Content/Images/agent-icon.png' class='agentBtnImg'/> </a></div> </div>" });
                helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Add Asset Type", Category = "Widgets", AgentLookUp = agent.Id.ToString() });
            }

            agent = agentsManager.Load(x => x.Title.EqualsIgnoreCase("Add a resource to a project")).FirstOrDefault();
            if (agent != null)
            {
                helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Add a resource to a project", Content = "<p>Add a resource to a project<p>", AgentContent = $"<div class='row'> <div class='col-md-6' title='create a new project' onclick='handleWidget({agent.Id})'><a id=9 ><img src='/Content/Images/agent-icon.png' class='agentBtnImg'/> </a></div> </div>" });
                helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Add a resource to a project", Category = "Widgets", AgentLookUp = agent.Id.ToString() });
            }

            agent = agentsManager.Load(x => x.Title.EqualsIgnoreCase("Add a Request type")).FirstOrDefault();
            if (agent != null)
            {
                helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Add a Request type", Content = "<p>Add a Request type<p>", AgentContent = $"<div class='row'> <div class='col-md-6' title='create a new project' onclick='handleWidget({agent.Id})'><a id=9 ><img src='/Content/Images/agent-icon.png' class='agentBtnImg'/> </a></div> </div>" });
                helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Add a Request type", Category = "Widgets", AgentLookUp = agent.Id.ToString() });
            }

            agent = agentsManager.Load(x => x.Title.EqualsIgnoreCase("Create a project")).FirstOrDefault();
            if (agent != null)
            {
                helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "Create a project", Content = "<p>Create a project<p>", AgentContent = $"<div class='row'> <div class='col-md-6' title='create a new project' onclick='handleWidget({agent.Id})'><a id=9 ><img src='/Content/Images/agent-icon.png' class='agentBtnImg'/> </a></div> </div>" });
                helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false, Title = "Create a project", Category = "Widgets", AgentLookUp = agent.Id.ToString() });
            }
            agent = agentsManager.Load(x => x.Title.EqualsIgnoreCase("create new application request")).FirstOrDefault();
            if (agent != null)
            {
                helpCardContents.Add(new HelpCardContent { Attachments = string.Empty, Deleted = false, Title = "create new application request", Content = "<p>create new application request<p>", AgentContent = $"<div class='row'> <div class='col-md-6' title='create a new project' onclick='handleWidget({agent.Id})'><a id=9 ><img src='/Content/Images/agent-icon.png' class='agentBtnImg'/> </a></div> </div>" });
                helpCards.Add(new HelpCard { Attachments = string.Empty, Deleted = false,  Title = "create new application request", Category = "Widgets", AgentLookUp = agent.Id.ToString() });
            }

            for (int i = 0; i < helpCardContents.Count; i++)
            {
                Ticket obj = new Ticket(context, "HLP");
                ticketId = obj.GetNewTicketId();
                helpCardContents[i].TicketId = ticketId;
                helpCardContentManager.Insert(helpCardContents[i]);
                helpCards[i].HelpCardContentID = helpCardContents[i].ID;
                helpCards[i].TicketId = ticketId;
                helpCardManager.Insert(helpCards[i]);
            }

        }

        public List<Phrases> GetPhrases(ApplicationContext context)
        {
            List<Phrases> phrasesList = new List<Phrases>();
            try
            {
                RequestTypeManager requestTypeManager = new RequestTypeManager(context);
                ModuleViewManager moduleViewManager = new ModuleViewManager(context);
                ServicesManager servicesManager = new ServicesManager(context);

                //long requestTypeId = 0;
                long serviceId = 0;

                //string tsr = Convert.ToString(moduleViewManager.LoadByName("TSR", false).ID);
                //string acr = Convert.ToString(moduleViewManager.LoadByName("ACR", false).ID);
                //string svc = Convert.ToString(moduleViewManager.LoadByName("SVC", false).ID);

                string tsr = Convert.ToString(moduleViewManager.Load(x => x.ModuleName.EqualsIgnoreCase("TSR")).FirstOrDefault().ID);
                string acr = Convert.ToString(moduleViewManager.Load(x => x.ModuleName.EqualsIgnoreCase("ACR")).FirstOrDefault().ID);
                string svc = Convert.ToString(moduleViewManager.Load(x => x.ModuleName.EqualsIgnoreCase("SVC")).FirstOrDefault().ID);

                var requestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'TSR' and {DatabaseObjects.Columns.Category} = 'User Devices' and {DatabaseObjects.Columns.SubCategory} = 'Laptop' and {DatabaseObjects.Columns.RequestType} = 'Issue' ");

                if (requestType != null)
                    phrasesList.Add(new Phrases() { Phrase = "Laptop not Booting", AgentType = 2, TicketType = tsr, RequestType = requestType.ID });

                requestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'TSR' and {DatabaseObjects.Columns.Category} = 'Network' and {DatabaseObjects.Columns.SubCategory} = 'LAN' and {DatabaseObjects.Columns.RequestType} = 'Connectivity' ");
                if (requestType != null)
                    phrasesList.Add(new Phrases() { Phrase = "Cannot connect to Internet", AgentType = 2, TicketType = tsr, RequestType = requestType.ID });

                requestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'TSR' and {DatabaseObjects.Columns.Category} = 'Desktop Software' and {DatabaseObjects.Columns.SubCategory} = 'MS Office' and {DatabaseObjects.Columns.RequestType} = 'Install/Uninstall' ");
                if (requestType != null)
                    phrasesList.Add(new Phrases() { Phrase = "Word stopped working", AgentType = 2, TicketType = tsr, RequestType = requestType.ID });

                phrasesList.Add(new Phrases() { Phrase = "Unable to login", AgentType = 1 });
                phrasesList.Add(new Phrases() { Phrase = "Reset Password", AgentType = 1 });
                phrasesList.Add(new Phrases() { Phrase = "iPhone", AgentType = 2 });
                phrasesList.Add(new Phrases() { Phrase = "Device", AgentType = 2 });

                requestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'TSR' and {DatabaseObjects.Columns.Category} = 'User Devices' and {DatabaseObjects.Columns.SubCategory} = 'Printer' and {DatabaseObjects.Columns.RequestType} = 'Broken' ");
                if (requestType != null)
                    phrasesList.Add(new Phrases() { Phrase = "Printer is Broken", AgentType = 2, TicketType = tsr, RequestType = requestType.ID });

                requestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'TSR' and {DatabaseObjects.Columns.Category} = 'Network' and {DatabaseObjects.Columns.SubCategory} = 'Router' and {DatabaseObjects.Columns.RequestType} = 'Connectivity' ");
                if (requestType != null)
                    phrasesList.Add(new Phrases() { Phrase = "Router cable is not connected", AgentType = 2, TicketType = tsr, RequestType = requestType.ID });

                requestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'TSR' and {DatabaseObjects.Columns.Category} = 'User Devices' and {DatabaseObjects.Columns.SubCategory} = 'Printer' and {DatabaseObjects.Columns.RequestType} = 'Hardware Upgrades' ");
                if (requestType != null)
                    phrasesList.Add(new Phrases() { Phrase = "Printer bluetooth is not working", AgentType = 2, TicketType = tsr, RequestType = requestType.ID });

                requestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'ACR' and {DatabaseObjects.Columns.Category} = 'Governance' and {DatabaseObjects.Columns.SubCategory} = '' and {DatabaseObjects.Columns.RequestType} = 'Governance' ");
                if (requestType != null)
                    phrasesList.Add(new Phrases() { Phrase = "Governance", AgentType = 2, TicketType = acr, RequestType = requestType.ID });

                requestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'TSR' and {DatabaseObjects.Columns.Category} = 'User Devices' and {DatabaseObjects.Columns.SubCategory} = 'Phone' and {DatabaseObjects.Columns.RequestType} = 'Purchase' ");
                if (requestType != null)
                    phrasesList.Add(new Phrases() { Phrase = "Need iPhone", AgentType = 2, TicketType = tsr, RequestType = requestType.ID });

                //if (requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'ACR' and {DatabaseObjects.Columns.Category} = 'Application Software' and {DatabaseObjects.Columns.SubCategory} = 'Oracle' and {DatabaseObjects.Columns.RequestType} = 'Upgrade' ").ID;
                //phrasesList.Add(new Phrases() { Phrase = "Upgrade Oracle to latest version", AgentType = 2, TicketType = acr, RequestType = requestType.ID });

                phrasesList.Add(new Phrases() { Phrase = "Broken Printer", AgentType = 2, TicketType = tsr });

                requestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID} = '{context.TenantID}' and {DatabaseObjects.Columns.ModuleNameLookup} = 'TSR' and {DatabaseObjects.Columns.Category} = 'Network' and {DatabaseObjects.Columns.SubCategory} = 'LAN' and {DatabaseObjects.Columns.RequestType} = 'Performance' ");
                if (requestType != null)
                    phrasesList.Add(new Phrases() { Phrase = "Internet is slow", AgentType = 2, TicketType = tsr, RequestType = requestType.ID });


                var service = servicesManager.LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Title.EqualsIgnoreCase("Employee on-boarding") && x.Deleted == false).FirstOrDefault();
                if (service != null)
                {
                    serviceId = service.ID;
                    phrasesList.Add(new Phrases() { Phrase = "Employee onboarding", AgentType = 3, TicketType = svc, Services = serviceId });
                }

                service = servicesManager.LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Title.EqualsIgnoreCase("Password Issue") && x.Deleted == false).FirstOrDefault();
                if (service != null)
                {
                    serviceId = service.ID;
                    phrasesList.Add(new Phrases() { Phrase = "Password issue", AgentType = 3, TicketType = svc, Services = serviceId });
                }

                service = servicesManager.LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Title.EqualsIgnoreCase("Need Laptop") && x.Deleted == false).FirstOrDefault();
                if (service != null)
                {
                    serviceId = service.ID;
                    phrasesList.Add(new Phrases() { Phrase = "Need Laptop", AgentType = 3, TicketType = svc, Services = serviceId });
                }

                service = servicesManager.LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Title.EqualsIgnoreCase("Report a Problem") && x.Deleted == false).FirstOrDefault();
                if (service != null)
                {
                    serviceId = service.ID;
                    phrasesList.Add(new Phrases() { Phrase = "Report a problem", AgentType = 3, TicketType = svc, Services = serviceId });
                }

                service = servicesManager.LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Title.EqualsIgnoreCase("Printer Issue") && x.Deleted == false).FirstOrDefault();
                if (service != null)
                {
                    serviceId = service.ID;
                    phrasesList.Add(new Phrases() { Phrase = "Printer issue", AgentType = 3, TicketType = svc, Services = serviceId });
                }

                PhraseManager phraseManager = new PhraseManager(context);
                phraseManager.InsertItems(phrasesList);
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }

            return phrasesList;
        }


        public List<Agents> GetWidgets(ApplicationContext context)
        {
            List<Agents> agentsList = new List<Agents>();
            long serviceId = 0;
            try
            {
                ServicesManager servicesManager = new ServicesManager(context);
                agentsList.Add(new Agents() { Title = "Add Asset Type", Name = "addassettype", Description = "This will open add asset Type page", Control = "requestedit", Url = "/Layouts/uGovernIT/DelegateControl.aspx", Parameters = "ID=0&module=ACR", ServiceLookUp = 0, Width = "600", Height = "800" });
                agentsList.Add(new Agents() { Title = "Add a resource to a project", Name = "addassettype", Description = "Add a resource to a project", Control = "homedashboardtickets", Url = "/Layouts/uGovernIT/DelegateControl.aspx", Parameters = "Module=PMM&Status=all&UserType=Admin&showalldetail=false&showglobalfilter=true&ShowResourceAllocationBtn=true", ServiceLookUp = 0, Width = "1500", Height = "800" });
                agentsList.Add(new Agents() { Title = "Add a Request type", Name = "addrequesttype", Description = "This will open request type page", Control = "requesttype", Url = "/Layouts/uGovernIT/uGovernITConfiguration.aspx", Parameters = "", ServiceLookUp = 0, Width = "1200", Height = "800" });
                agentsList.Add(new Agents() { Title = "Resource Utilization", Name = "resourceutilization", Description = "This will open resource utilization tab", Control = "resourceavailability", Url = "/Layouts/uGovernIT/DelegateControl.aspx", Parameters = "AllocationViewType=RMMAllocation&isdlg=1&isudlg=1", ServiceLookUp = 0, Width = "1200", Height = "1000" });

                var service = servicesManager.LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Title.EqualsIgnoreCase("Create a project") && x.Deleted == false).FirstOrDefault();
                if (service != null)
                {
                    serviceId = service.ID;
                    agentsList.Add(new Agents() { Title = "Create a project", Name = "createproject", Description = "This will run create project service", Control = "", Url = "", Parameters = "", ServiceLookUp = serviceId, Width = "1200", Height = "800" });

                }
                service = servicesManager.LoadAllServices().Where(x => x.IsActivated == true && x.ServiceType.EqualsIgnoreCase("Service") && x.Title.EqualsIgnoreCase("Create new application request") && x.Deleted == false).FirstOrDefault();
                if (service != null)
                {
                    serviceId = service.ID;
                    agentsList.Add(new Agents() { Title = "create new application request", Name = "createapplicationrequest", Description = "This will run create new application request", Control = "", Url = "", Parameters = "", ServiceLookUp = serviceId, Width = "1200", Height = "850" });

                }
                AgentsManager agentsManager = new AgentsManager(context);
                agentsManager.InsertItems(agentsList);

            }
            catch (Exception ex)
            {
                ULog.WriteException(ex);
            }
            return agentsList;
        }

        public List<ServiceCategory> GetServiceCategory(ApplicationContext context)
        {
            List<ServiceCategory> ServiceCategoryList = new List<ServiceCategory>();

            ServiceCategoryList.Add(new ServiceCategory() { CategoryName = "I have an Issue", ItemOrder = 1, Title = "I have an Issue", ImageUrl = "/content/images/SCissue.png" });
            ServiceCategoryList.Add(new ServiceCategory() { CategoryName = "Employee Management", ItemOrder = 1, Title = "Employee Management", ImageUrl = "/content/images/SCmanagement.png" });
            ServiceCategoryList.Add(new ServiceCategory() { CategoryName = "Purchase", ItemOrder = 1, Title = "Purchase", ImageUrl = "/Content/Images/SCpurchase.png" });


            ServiceCategoryManager ServiceCategoryManager = new ServiceCategoryManager(context);
            ServiceCategoryManager.InsertItems(ServiceCategoryList);

            return ServiceCategoryList;
        }


        public void GetWikiContents(ApplicationContext context)
        {
            Ticket ticket = new Ticket(context, "WIKI");

            WikiArticlesManager wikiArticlesManager = new WikiArticlesManager(context);

            RequestTypeManager requestTypeManager = new RequestTypeManager(context);


            WikiContents acrWikiContents = new WikiContents();
            WikiContents tsrWikiContents = new WikiContents();
            WikiContents drqWikiContents = new WikiContents();
            WikiContents svcWikiContents = new WikiContents();
            WikiContents btsWikiContents = new WikiContents();
            WikiContents incWikiContents = new WikiContents();

            //ACR 
            acrWikiContents.Content = "<div style='font - family: verdana, verdana, helvetica, sans - serif; font - size: 11px; float: left; width: 1159.14px; padding - bottom: 30px; '><span id='ctl00_PlaceHolderMain_ctl00_ASPxSplitterWiki_lblHeading' style='font - family: verdana, verdana, helvetica, sans - serif; position: absolute; padding - top: 5px; font - weight: bold; '>Related To: WIK - Service Management - Application Change Request</span></div><div id='ctl00_PlaceHolderMain_ctl00_ASPxSplitterWiki_taWikiDescriptionSection' style='font - family: verdana, verdana, helvetica, sans - serif; font - size: 11px; width: 1142.14px; overflow: auto; height: 372px; '><p style='font - family: &quot; Times New Roman & quot;, serif; '><span style='font - family: Arial, sans - serif; '>An Application Change Request (ACR) tracks desired changes or enhancements to a specific application (software) within the company’s portfolio of business systems. An ACR may have been created as a result of another request such as a Technical Service Request or New Project Request.&nbsp; The ACR is used to track the work activity and resolution of the request and follows a workflow with pre-defined stages which can be viewed when selecting the request. The company’s portfolio of business systems is maintained in the Application Management module.</span></p><p style='font - family: &quot; Times New Roman & quot;, serif; '><span style='font - family: Arial, sans - serif; '>&nbsp;</span><strong style='font - family: verdana, verdana, helvetica, sans - serif; '><span style='font - family: Arial, sans - serif; '>Creating a New Request</span></strong></p><p style='font - family: Arial, sans - serif; font - size: 14.6667px; margin: 14.6667px 0px; '>An Application Change Request (ACR) is created automatically from a Shared Services Request (SVC)&nbsp;or directly from the Application Change Request portal.&nbsp;Values for certain fields may be pre-defined&nbsp;from defaults or templates to assign tasks and resources and initiate workflow steps when a new request is created.&nbsp;<em style='font - family: verdana, verdana, helvetica, sans - serif; '>See the Shared Services Help Guide for more information on creating an ACR from a Service Request.</em></p><p style='font - family: Calibri, sans - serif; font - size: 14.6667px; margin: 14.6667px 0px; '><span style='font - family: Arial, sans - serif; '>Required fields are designated with an asterisk (*) which may be a drop-down selection, checkbox, date, or direct entry.&nbsp;Documents may be attached to the request and are viewable as needed.&nbsp;&nbsp;</span></p><p style='font - family: Arial, sans - serif; font - size: 14.6667px; margin: 14.6667px 0px; '>New requests are automatically added to one or more tabs on the Application Change Request portal and the requestor&#39;s Home page, depending on a user&#39;s role. &nbsp;The current step of the request is highlighted in the workflow.&nbsp;&nbsp;A request may require a manager&#39;s approval depending on the role of the requestor and request type.&nbsp;</p><p style='font - family: Calibri, sans - serif; margin: 0in 0in 0.0001pt; font - size: 14.6667px; '><strong style='font - family: verdana, verdana, helvetica, sans - serif; '><span style='font - family: Arial, sans - serif; '>Viewing and Working with a Request</span></strong></p><p style='font - family: Calibri, sans - serif; font - size: 14.6667px; margin: 14.6667px 0px; '><span style='font - family: Arial, sans - serif; '>You can view information about the request by clicking on the tabs.&nbsp; You may be able to perform certain limited actions such as adding a comment or printing the request. If you are a supervisor or manager you may have an approval or other action available if a request is pending your review. Generally, designated IT staff are responsible for updating and closing a request.</span></p><p style='font - family: Calibri, sans - serif; font - size: 14.6667px; margin: 14.6667px 0px; '><span style='font - family: Arial, sans - serif; '>As a request is updated, it may generate automatic emails to the requestor, managers, person’s with assigned tasks, or other parties as pre-defined by the workflow steps.</span></p><p style='font - family: Calibri, sans - serif; font - size: 14.6667px; margin: 14.6667px 0px; '><strong style='font - family: verdana, verdana, helvetica, sans - serif; '><span style='font - family: Arial, sans - serif; '>Sample Requests</span></strong></p><p style='font - family: Calibri, sans - serif; font - size: 14.6667px; margin: 14.6667px 0px; '><span style='font - family: Arial, sans - serif; '>See the Related Requests panel for sample requests. Click on a request to view the detail.&nbsp;<em style='font - family: verdana, verdana, helvetica, sans - serif; '>See Basic Navigation Help Guide on how to access the Related Requests panel</em>.</span></p></div>";

            acrWikiContents.TicketId = ticket.GetNewTicketId();

            acrWikiContents.Title = "Application Change Request Help Guide";

            acrWikiContents = AddFetchWikiContent(acrWikiContents, context);

            ModuleRequestType moduleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{context.TenantID}' and { DatabaseObjects.Columns.TicketRequestType }='Application Change Request' ");

            if (moduleRequestType != null)
            {
                WikiArticles wikiArticles = new WikiArticles() { Title = "Application Change Request Help Guide", ModuleNameLookup = "WIKI", WikiContentID = acrWikiContents.ID, TicketId = acrWikiContents.TicketId, WikiSnapshot = "Related To: WIK - Service Management - Application Change Request    An Application Change Request (ACR) tracks desired changes or enhancements to a s", RequestTypeLookup = moduleRequestType.ID };

                wikiArticlesManager.Insert(wikiArticles);
            }


            //tsr 

            tsrWikiContents.Content = "<div style='font - family: verdana, verdana, helvetica, sans - serif; font - size: 11px; float: left; width: 1159.14px; padding - bottom: 30px; '><span id='ctl00_PlaceHolderMain_ctl00_ASPxSplitterWiki_lblHeading' style='font - family: verdana, verdana, helvetica, sans - serif; position: absolute; padding - top: 5px; font - weight: bold; '>Related To: WIK - Service Management - Technical Service Request</span></div><div id='ctl00_PlaceHolderMain_ctl00_ASPxSplitterWiki_taWikiDescriptionSection' style='font - family: verdana, verdana, helvetica, sans - serif; font - size: 11px; width: 1142.14px; overflow: auto; height: 343px; '><p style='font - family: '><span style='font - family: Arial, sans - serif; '>A Technical Service Request (or ticket) is a record of a specific issue as submitted by a user which tracks the work activity and resolution of the issue. Each request follows a workflow with pre-defined stages which can be viewed when selecting the request.</span></p><p style='font - family: '><strong style='font - family: verdana, verdana, helvetica, sans - serif; '><span style='font - family: Arial, sans - serif; '>Creating a New Request</span></strong></p><p style='font - family: '><span style='font - family: Arial, sans - serif; '>A Technical Service</span><span style='font - family: Arial, sans - serif; '>&nbsp;</span><span style='font - family: Arial, sans - serif; '>Request (TSR) is created automatically from a Shared Services Request (SVC), from an email to the support desk, or directly from the Technical Service Request portal. Values for certain fields may be pre-defined&nbsp;from defaults or templates to assign tasks and resources and initiate workflow steps when a new request is created.</span><span style='font - family: Arial, sans - serif; '>&nbsp;</span><em style='font - family: Arial, sans - serif; '>See the Shared Services Help Guide for more information on creating a TSR from a Service Request.</em></p><p style='font - family: '><span style='font - family: Arial, sans - serif; '>Required fields are designated with an asterisk (*) which may be a drop-down selection, checkbox, date, or direct entry. Documents may be attached to the request and are viewable as needed.</span></p><p style='font - family: '><span style='font - family: Arial, sans - serif; '>New requests are automatically added to one or more tabs on the Technical Service Request portal and the requestor&#39;s Home page, depending on a user&#39;s role. &nbsp;The current step of the request is highlighted in the workflow. &nbsp;A request may require a manager&#39;s approval depending on the role of the requestor and request type.</span></p><strong><span style='font - family: Arial, sans - serif; '>Viewing and Working with a Request</span></strong><p><span style='font - family: Arial, sans - serif; '>You can view information about the request by clicking on the tabs.&nbsp; You may be able to perform certain limited actions such as adding a comment or printing the request. If you are a supervisor or manager you may have an approval or other action available if a request is pending your review. Generally, the Support/Help Desk staff are responsible for updating and closing a request.</span></p><p><span style='font - family: Arial, sans - serif; '>As a request is updated, it may generate automatic emails to the requestor, managers, person’s with assigned tasks, or other parties as defined by the workflow steps.</span></p><p><strong><span style='font - family: Arial, sans - serif; '>Sample Requests</span></strong></p><p><span style='font - family: Arial, sans - serif; '>See the Related Requests panel for sample requests. Click on a request to view.&nbsp;</span><em style='font - family: Arial, sans - serif; '>See Basic Navigation Help Guide on how to access the Related Requests panel</em><span style='font - family: Arial, sans - serif; '>.</span></p><p><span style='font - family: Arial, sans - serif; '><img src='data: image / png; base64,iVBORw0KGgoAAAANSUhEUgAABG0AAAEpCAYAAAApn98aAAAgAElEQVR4AeydWZBlVZnvebwvvvRDR / hww2g7Ogyjw8vta3s1tAkxNMBWwykc8DrbIE7thNoqKIoTIIhCi4wyFjNVxQxFQc1VFNRAzUVBTdQ8UfMEFOvGb6XfYdVm5xkyT57MyvrtiJP75N5r / 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'";

            tsrWikiContents.TicketId = ticket.GetNewTicketId();

            tsrWikiContents.Title = "Technical Service Request Help Guide";

            tsrWikiContents = AddFetchWikiContent(tsrWikiContents, context);

            moduleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{context.TenantID}' and { DatabaseObjects.Columns.TicketRequestType }='Technical Service Request' ");
            if (moduleRequestType != null)
            {
                var tsrWikiArticles = new WikiArticles() { Title = "Technical Service Request Help Guide", ModuleNameLookup = "WIKI", WikiContentID = tsrWikiContents.ID, TicketId = tsrWikiContents.TicketId, WikiSnapshot = "Related To: WIK - Service Management - Application Change Request    An Application Change Request (ACR) tracks desired changes or enhancements to a s", RequestTypeLookup = moduleRequestType.ID };
                wikiArticlesManager.Insert(tsrWikiArticles);
            }

            //drq

            drqWikiContents.Content = "<div style='font - family: verdana, verdana, helvetica, sans - serif; font - size: 11px; float: left; width: 1159.14px; padding - bottom: 30px; '><span id='ctl00_PlaceHolderMain_ctl00_ASPxSplitterWiki_lblHeading' style='font - family: verdana, verdana, helvetica, sans - serif; position: absolute; padding - top: 5px; font - weight: bold; '>Related To: WIK - Service Management - Change Management</span></div><div id='ctl00_PlaceHolderMain_ctl00_ASPxSplitterWiki_taWikiDescriptionSection' style='font - family: verdana, verdana, helvetica, sans - serif; font - size: 11px; width: 1142.14px; overflow: auto; height: 372px; '><p style='font - family: &quot; Times New Roman & quot;, serif; '><span style='font - family: Arial, sans - serif; '>Change Management tracks updates to production systems including application, infrastructure, communications, and other systems. It serves as the production control for reviewing, approving, scheduling, and managing system changes. With a calendar view of scheduled changes, requests can be managed with a holistic view to help prevent conflicts on deployments.&nbsp; The systems serving a company may be catalogued and managed in the Application Management or Asset Management modules.</span></p><p style='font - family: &quot; Times New Roman & quot;, serif; '><strong style='font - family: verdana, verdana, helvetica, sans - serif; '><span style='font - family: Arial, sans - serif; '>Creating a New Request</span></strong></p><p style='font - family: &quot; Times New Roman & quot;, serif; '><span style='font - family: Arial, sans - serif; '>A Change Management (DRQ) request is created directly from the Change Management portal. Values for certain fields may be pre-defined&nbsp;from defaults or templates to assign tasks and resources and initiate workflow steps when a new request is created.</span></p><p style='font - family: &quot; Times New Roman & quot;, serif; '><span style='font - family: Arial, sans - serif; '>Required fields are designated with an asterisk (*) which may be a drop-down selection, checkbox, date, or direct entry. Documents may be attached to the request and are viewable as needed.</span></p><p style='font - family: &quot; Times New Roman & quot;, serif; '><span style='font - family: Arial, sans - serif; '>New requests are automatically added to one or more tabs on the Change Management portal and the requestor&#39;s Home page, depending on a user&#39;s role. &nbsp;The current step of the request is highlighted in the workflow. &nbsp;A request may require a manager&#39;s approval depending on the role of the requestor and request type.</span></p><strong><span style='font - family: Arial, sans - serif; '>Viewing and Working with a Request</span></strong><p><span style='font - family: Arial, sans - serif; '>You can view information about the request by clicking on the tabs.&nbsp; You may be able to perform certain limited actions such as adding a comment or printing the request. If you are a supervisor or manager you may have an approval or other action available if a request is pending your review.&nbsp; Generally, the designated IT staff are responsible for updating and closing a request.&nbsp;</span></p><p><span style='font - family: Arial, sans - serif; '>As a request is updated, it may generate automatic emails to the requestor, managers, person’s with assigned tasks, or other parties as pre-defined by the workflow steps.</span></p><p><strong><span style='font - family: Arial, sans - serif; '>Sample Requests</span></strong></p><p><span style='font - family: Arial, sans - serif; '>See the Related Requests panel for sample requests. Click on a request to view the detail.&nbsp;</span><em style='font - family: Arial, sans - serif; '>See Basic Navigation Help Guide on how to access the Related Requests panel.</em></p></div>";

            drqWikiContents.TicketId = ticket.GetNewTicketId();

            drqWikiContents.Title = "Change Management Help Guide";

            drqWikiContents = AddFetchWikiContent(drqWikiContents, context);

            moduleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{context.TenantID}' and { DatabaseObjects.Columns.TicketRequestType }='Change Management' ");
            if (moduleRequestType != null)
            {
                var drqWikiArticles = new WikiArticles() { Title = "Change Management Help Guide", ModuleNameLookup = "WIKI", WikiContentID = drqWikiContents.ID, TicketId = drqWikiContents.TicketId, WikiSnapshot = "Related To: WIK - Service Management - Application Change Request    An Application Change Request (ACR) tracks desired changes or enhancements to a s", RequestTypeLookup = moduleRequestType.ID };

                wikiArticlesManager.Insert(drqWikiArticles);
            }
            //BTS
            btsWikiContents.Content = "<p><br /></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><span style='font-family: Arial, sans-serif;'>Bug Tracking requests track defects in company systems, most notably for software applications. The request may reference related requests from Technical Service Requests, Incidents, or Root Cause Analysis, and may lead to an associated Application Change Request to repair the bug.&nbsp;&nbsp;<em>A request</em>&nbsp;may be converted directly into an Application Change Request.&nbsp;<em>See the General&nbsp;Services Module Help Guide for information on converting tickets.</em></span></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><strong><span style='font-family: Arial, sans-serif;'>Creating a New Request</span></strong></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><span style='font-family: Arial, sans-serif;'>A Bug Tracking (BTS) request is created directly from the Bug Tracking portal. Values for certain fields may be pre-defined&nbsp;from defaults or templates to assign tasks and resources and initiate workflow steps when a new request is created.</span></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><span style='font-family: Arial, sans-serif;'>Required fields are designated with an asterisk (*) which may be a drop-down selection, checkbox, date, or direct entry. Documents may be attached to the request and are able to view as needed.</span></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><span style='font-family: Arial, sans-serif;'>New requests are automatically added to one or more tabs on the Bug Tracking portal and the requester&#39;s Home page, depending on a user&#39;s role. &nbsp;The current step of the request is highlighted in the workflow.&nbsp; A request may require a manager&#39;s approval depending on the role of the requester and request type.</span></p><strong><span style='font-family: Arial, sans-serif;'>Viewing and Working with a Request</span></strong><p><span style='font-family: Arial, sans-serif;'>You can view information about the request by clicking on the tabs.&nbsp; You may be able to perform certain limited actions such as adding a comment or printing the request. If you are a supervisor or manager you may have an approval or other action available if a request is pending your review.&nbsp; Generally, designated IT staff are responsible for updating and closing a request.&nbsp;</span></p><p><span style='font-family: Arial, sans-serif;'>As a request is updated, it may generate automatic emails to the requester, managers, person’s with assigned tasks, or other parties as pre-defined by the workflow steps.</span></p><p><strong><span style='font-family: Arial, sans-serif;'>Sample Requests</span></strong></p><p><span style='font-family: Arial, sans-serif;'>See the Related Requests panel for sample requests. Click on a request to view the detail.&nbsp;</span><em style='font-family: Arial, sans-serif;'>See Basic Navigation Help Guide on how to access the Related Requests panel</em><span style='font-family: Arial, sans-serif;'>.</span></p><p><span style='font-family: Arial, sans-serif;'><br /></span></p><img src='http://demo.ugovernit.com/_layouts/15/images/uGovernIT/UploadedFiles/BTS%20Header%20View%20for%20Help%20Guide.png' alt=''/>";

            btsWikiContents.TicketId = ticket.GetNewTicketId();

            btsWikiContents.Title = "Bug Tracking Help Guide";

            btsWikiContents = AddFetchWikiContent(btsWikiContents, context);

            moduleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{context.TenantID}' and { DatabaseObjects.Columns.TicketRequestType }='Bug Tracking' ");
            if (moduleRequestType != null)
            {
                var btsWikiArticles = new WikiArticles() { Title = "Bug Tracking Help Guide", ModuleNameLookup = "WIKI", WikiContentID = btsWikiContents.ID, TicketId = btsWikiContents.TicketId, WikiSnapshot = "Bug Tracking requests track defects in company systems, most notably for software applications. The request may reference related requests from Technical", RequestTypeLookup = moduleRequestType.ID };

                wikiArticlesManager.Insert(btsWikiArticles);
            }


            //INC
            incWikiContents.Content = "<p style='font-family: &quot;Times New Roman&quot;, serif;'><span style='font-family: Arial, sans-serif;'><br /></span></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><span style='font-family: Arial, sans-serif;'>Outage Incidents record widespread issues with systems and are used to track the activity of the incident. Outage Incidents requests are usually created as a result of multiple Technical Service Requests for a similar issue, and may lead to requests created for Root Cause Analysis, Application Change Request, or a Change Management request to address the issue.</span></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><strong><span style='font-family: Arial, sans-serif;'>Creating a New Request</span></strong></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><span style='font-family: Arial, sans-serif;'>An Outage Incident (INC) request is created directly from the Outage Incident portal. Values for certain fields may be pre-defined&nbsp;from defaults or templates to assign tasks and resources and initiate workflow steps when a new request is created.</span></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><span style='font-family: Arial, sans-serif;'>Required fields are designated with an asterisk (*) which may be a drop-down selection, checkbox, date, or direct entry. Documents may be attached to the request and are able to view as needed.</span></p><p style='font-family: &quot;Times New Roman&quot;, serif;'><span style='font-family: Arial, sans-serif;'>New requests are automatically added to one or more tabs on the Outage Incident portal and the requester&#39;s Home page, depending on a user&#39;s role. &nbsp;The highlighted step in the workflow indicates the current step. A request may require a manager&#39;s approval depending on the role of the requester and request type.</span></p><strong><span style='font-family: Arial, sans-serif;'>Viewing and Working with a Request</span></strong><p><span style='font-family: Arial, sans-serif;'>You can view information about the request by clicking on the tabs.&nbsp; You may be able to perform certain limited actions such as adding a comment or printing the request. If you are a supervisor or manager you may have an approval or other action available if a request is pending your review.&nbsp; Generally, designated IT staff are responsible for updating and closing a request.&nbsp;</span></p><p><span style='font-family: Arial, sans-serif;'>As a request is updated, it may generate automatic emails to the requester, managers, person’s with assigned tasks, or other parties as pre-defined by the workflow steps.</span></p><p><strong><span style='font-family: Arial, sans-serif;'>Sample Requests</span></strong></p><p><span style='font-family: Arial, sans-serif;'>See the Related Requests panel for sample requests. Click on a request to view the detail.&nbsp;</span><em style='font-family: Arial, sans-serif;'>See Basic Navigation Help Guide on how to access the Related Requests panel</em><span style='font-family: Arial, sans-serif;'>.</span></p>";

            incWikiContents.TicketId = ticket.GetNewTicketId();

            incWikiContents.Title = "Outage Incidents Help Guide";

            incWikiContents = AddFetchWikiContent(incWikiContents, context);

            moduleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{context.TenantID}' and { DatabaseObjects.Columns.TicketRequestType }='Outage Incidents' ");
            if (moduleRequestType != null)
            {
                var incWikiArticles = new WikiArticles() { Title = "Outage Incidents Help Guide", ModuleNameLookup = "WIKI", WikiContentID = incWikiContents.ID, TicketId = incWikiContents.TicketId, WikiSnapshot = "Outage Incidents record widespread issues with systems and are used to track the activity of the incident. Outage Incidents requests are usually create", RequestTypeLookup = moduleRequestType.ID };

                wikiArticlesManager.Insert(incWikiArticles);
            }
            //Service which is Employee on-boarding

            svcWikiContents.Content = "<div style='font-family: verdana, verdana, helvetica, sans-serif; font-size: 11px; float: left; width: 1363.81px; padding-bottom: 30px;'><span id='ctl00_PlaceHolderMain_ctl00_ASPxSplitterWiki_lblHeading' style='font-family: verdana, verdana, helvetica, sans-serif; position: absolute; padding-top: 5px; font-weight: bold;'>Related To: WIK - General - IT Internal</span></div><div id='ctl00_PlaceHolderMain_ctl00_ASPxSplitterWiki_taWikiDescriptionSection' style='font-family: verdana, verdana, helvetica, sans-serif; font-size: 11px; width: 1363.81px; overflow: auto; height: 462px;'><p><img src='data:image/png;base64,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'></p></div>";
            svcWikiContents.TicketId = ticket.GetNewTicketId();
            svcWikiContents.Title = "Employee on-boarding";

            svcWikiContents = AddFetchWikiContent(svcWikiContents, context);
            //Need to add request type
            moduleRequestType = requestTypeManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{context.TenantID}' and { DatabaseObjects.Columns.ModuleNameLookup }='acr' ");
            if (moduleRequestType != null)
            {
                var svcWikiArticles = new WikiArticles() { Title = "Employee on-boarding", ModuleNameLookup = "SVC", WikiContentID = svcWikiContents.ID, TicketId = svcWikiContents.TicketId, WikiSnapshot = "Related To: WIK - Service Management - Application Change Request    An Application Change Request (ACR) tracks desired changes or enhancements to a s", RequestTypeLookup = moduleRequestType.ID };
                wikiArticlesManager.Insert(svcWikiArticles);
            }
        }

        //Default data for Service Categories
        //// public List<> GetServiceCategories(ApplicationContext context)
        // {

        // }

        public void UpdateTabView(ApplicationContext context)
        {
            List<TabView> tabs = new List<TabView>();

            tabs.Add(new TabView() { TabName = "waitingonme", TabDisplayName = "Waiting On Me", ViewName = "Home", TabOrder = 1, ModuleNameLookup = "", ColumnViewName = "" });
            tabs.Add(new TabView() { TabName = "myopentickets", TabDisplayName = "My Request", ViewName = "Home", TabOrder = 2, ModuleNameLookup = "", ColumnViewName = "" });
            tabs.Add(new TabView() { TabName = "mytask", TabDisplayName = "My Task", ViewName = "Home", TabOrder = 3, ModuleNameLookup = "", ColumnViewName = "" });
            tabs.Add(new TabView() { TabName = "departmentticket", TabDisplayName = "My Department", ViewName = "Home", TabOrder = 4, ModuleNameLookup = "", ColumnViewName = "" });
            tabs.Add(new TabView() { TabName = "myproject", TabDisplayName = "My Project", ViewName = "Home", TabOrder = 5, ModuleNameLookup = "", ColumnViewName = "" });
            tabs.Add(new TabView() { TabName = "documentpendingapprove", TabDisplayName = "Document Pending Approve", ViewName = "Home", TabOrder = 6, ModuleNameLookup = "", ColumnViewName = "" });
            tabs.Add(new TabView() { TabName = "myclosedtickets", TabDisplayName = "My Closed Tickets", ViewName = "Home", TabOrder = 7, ModuleNameLookup = "", ColumnViewName = "" });

            TabViewManager tabViewManager = new TabViewManager(context);
            tabViewManager.InsertItems(tabs);
            //  uGITDAL.InsertItem(tabs);
        }

        public void GetGlobalRoles(ApplicationContext context)
        {
            List<GlobalRole> globalRoles = new List<GlobalRole>();
            globalRoles.Add(new GlobalRole() { Id = Guid.NewGuid().ToString(), Name = "Assistant Project manager" });
            globalRoles.Add(new GlobalRole() { Id = Guid.NewGuid().ToString(), Name = "Business Analyst" });
            globalRoles.Add(new GlobalRole() { Id = Guid.NewGuid().ToString(), Name = "Director of Projects" });
            globalRoles.Add(new GlobalRole() { Id = Guid.NewGuid().ToString(), Name = "Product Owner" });
            globalRoles.Add(new GlobalRole() { Id = Guid.NewGuid().ToString(), Name = "Project Managerr" });
            globalRoles.Add(new GlobalRole() { Id = Guid.NewGuid().ToString(), Name = "Software Engineer" });
            globalRoles.Add(new GlobalRole() { Id = Guid.NewGuid().ToString(), Name = "Tech Lead" });
            globalRoles.Add(new GlobalRole() { Id = Guid.NewGuid().ToString(), Name = "Tech Support" });
            globalRoles.Add(new GlobalRole() { Id = Guid.NewGuid().ToString(), Name = "Vice President of Engineering" });

            GlobalRoleManager rolesManager = new GlobalRoleManager(context);
            rolesManager.InsertItems(globalRoles);
        }

        public void GetLeadCriteria(ApplicationContext context)
        {
            List<LeadCriteria> mList = new List<LeadCriteria>();

            mList.Add(new LeadCriteria() { Priority = "Cold", MinValue = 0.00M, MaxValue = 2.00M });
            mList.Add(new LeadCriteria() { Priority = "Warm", MinValue = 2.01M, MaxValue = 4.00M });
            mList.Add(new LeadCriteria() { Priority = "Hot", MinValue = 4.01M, MaxValue = 100.00M });

            LeadCriteriaManager LeadCriteriaManager = new LeadCriteriaManager(context);
            LeadCriteriaManager.InsertItems(mList);
        }

        public void GetProjectComplexity(ApplicationContext context)
        {
            List<ProjectComplexity> mList = new List<ProjectComplexity>();

            mList.Add(new ProjectComplexity() { CRMProjectComplexity = "1", MinValue = 0, MaxValue = 500000 });
            mList.Add(new ProjectComplexity() { CRMProjectComplexity = "2", MinValue = 500001, MaxValue = 2000000 });
            mList.Add(new ProjectComplexity() { CRMProjectComplexity = "3", MinValue = 2000001, MaxValue = 5000000 });
            mList.Add(new ProjectComplexity() { CRMProjectComplexity = "4", MinValue = 5000001, MaxValue = 15000000 });
            mList.Add(new ProjectComplexity() { CRMProjectComplexity = "5", MinValue = 15000001, MaxValue = 99999999999 });

            ProjectComplexityManager projectComplexityManager = new ProjectComplexityManager(context);
            projectComplexityManager.InsertItems(mList);
        }

        public void UpdateRoles(ApplicationContext context, string accountId, bool createSuperAdmin)
        {
            List<Role> roles = new List<Role>();

            //We need only one superadmin (UGITSuperadmin) that will handle all tenants and each tenant should have only admin access rights, so below SAdmin role has been commentted.
            //roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "SAdmin", Title = "Super Admin", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "Admin", Title = "Admin", IsSystem = true, LandingPage = "/Pages/UserHomePage" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "TicketAdmin", Title = "Ticket Admin", IsSystem = true, LandingPage = "/Pages/UserHomePage" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "ResourceAdmin", Title = "Resource Admin", IsSystem = true, LandingPage = "/Pages/UserHomePage" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "User", Title = "User", IsSystem = true, LandingPage = "/Pages/userhomepage" });
            //roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "ResourceAdmin", Title = "Resource Admin", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "PMO", Title = "PMO", IsSystem = true, LandingPage = "/Pages/HomePMO" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "uGovernIT Members", Title = "uGovernIT Members", IsSystem = true, LandingPage = "/Pages/UserHomePage" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "Data Management", Title = "Data Management", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "IT Network Management", Title = "IT Network Management", IsSystem = true, LandingPage = "/Pages/UserHomePage" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "Application Management", Title = "Application Management", IsSystem = true, LandingPage = "/Pages/UserHomePage" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "IT Security Management", Title = "IT Security Management", IsSystem = true, LandingPage = "/Pages/UserHomePage" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "Executive Management", Title = "Executive Management", IsSystem = true, LandingPage = "/Pages/UserHomePage" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "TrialUser", Title = "Trial User", IsSystem = true, LandingPage = "/Pages/TrialUser" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "ITSM", Title = "ITSM", IsSystem = true, LandingPage = "/Pages/ITSM" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "PMM", Title = "PMM", IsSystem = true, LandingPage = "/Pages/HomePMM" });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "Asset Managers", Title = "Asset Managers", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "TicketAdmin", Title = "Core Admin", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "User", Title = "User", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "IT Desktop", Title = "IT Desktop", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "IT Help Desk", Title = "IT Help Desk", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "IT Infrastructure", Title = "IT Infrastructure", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "ITSM", Title = "ITSM", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "PMM", Title = "PMM", IsSystem = true });
            roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "uGovernIT Members", Title = "uGovernIT Members", IsSystem = true });


            if (createSuperAdmin)
            {
                roles.Add(new Role() { Id = Guid.NewGuid().ToString(), Name = "UGITSuperAdmin", Title = $"{accountId} Super Admin", IsSystem = true, LandingPage = "/Pages/HomeSuperAdmin" });
            }

            var rolemanager = new UserRoleManager(context);
            foreach (Role r in roles)
            {
                rolemanager.AddOrUpdate(r);
            }
            // Console.WriteLine("Roles Created");
        }

        public void UpdateUsers(ApplicationContext context, string accountId, AccountInfo accountInfo, bool createSuperAdmin)
        {
            accountInfo = SetAdminAccountInfo(accountInfo, accountId);

            UserProfileManager manager = new UserProfileManager(context);
            List<Tuple<UserProfile, string, List<string>>> users = new List<Tuple<UserProfile, string, List<string>>>();
            LandingPages landingPage = new LandingPages();
            LandingPages landingPagetrail = new LandingPages();
            LandingPagesManager ObjLandingPagesManager = new LandingPagesManager(context);

            landingPage = ObjLandingPagesManager.GetRoleByName("Admin");
            if (string.IsNullOrEmpty(accountInfo.Name))
                accountInfo.Name = "Admin";

            users.Add(new Tuple<UserProfile, string, List<string>>(new UserProfile() { Id = DBConstant.SystemAdminID, UserName = accountInfo.UserName, Email = accountInfo.Email, Enabled = true, Name = accountInfo.Name, UserRoleId = landingPage.Id, IsDefaultAdmin = 1 }, accountInfo.Password, new List<string> { RoleType.Admin.ToString(), "ITSM" }));

            if (createSuperAdmin)
            {
                users.Add(new Tuple<UserProfile, string, List<string>>(new UserProfile() { UserName = $"SuperAdmin_{accountId}", Email = $"superadmin@{accountId}.com", Enabled = true, Name = $"{accountId} Super Admin" }, ConfigData.Password, new List<string> { RoleType.UGITSuperAdmin.ToString() })); //ugit super admin
            }

            foreach (Tuple<UserProfile, string, List<string>> item in users)
            {
                //foreach (Tenant x in list)
                //{
                item.Item1.Id = Guid.NewGuid().ToString();
                item.Item1.TenantID = context.TenantID.ToString();
                //   context.TenantID = x.TenantID.ToString();
                IdentityResult result = manager.Create(item.Item1, item.Item2);
                if (result.Succeeded)
                {
                    foreach (string role in item.Item3)
                    {
                        manager.AddToRole(item.Item1.Id, role);
                    }
                }
                //  }
            }

            // Console.WriteLine("User Created");
        }

        public List<PageConfiguration> GetPages()
        {
            List<PageConfiguration> pages = new List<PageConfiguration>();

            pages.Add(new PageConfiguration() { Name = "RequestList", Title = "Request List", RootFolder = "SitePages" });
            pages.Add(new PageConfiguration() { Name = "Request", Title = "Request", RootFolder = "SitePages" });
            //pages.Add(new PageConfiguration() { Name= "ACR", Title= "ACR", LeftMenuName="",LeftMenuType="",HideLeftMenu=true,HideTopMenu=true,HideHeader=true,TopMenuName="",TopMenuType="",HideSearch=true,HideFooter=true,ControlInfo= "<?xml version="1.0" encoding="utf - 16"?><ArrayOfDockPanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi"'http://www.w3.org/2001/XMLSchema-instance"><DockPanelSetting xsi:type="ModuleWebpartDockPanelSetting"><AssemblyName>uGovernIT.Web.ControlTemplates.DockPanels.ModuleWebPartDockPanel</AssemblyName><ControlID>182bb72a-abe7-4d41-a20c-71023a794f22_DockPanel_</ControlID><Name>ACR Ticket</Name><Title>Create ACR Ticket</Title><ShowTitle>false</ShowTitle><ModuleNameLookup>ACR</ModuleNameLookup></DockPanelSetting></ArrayOfDockPanelSetting>",LayoutInfo= "{'182bb72a-abe7-4d41-a20c-71023a794f22_DockPanel_':[true,'All','LeftZone','722px','63px',0,0,0]}", RootFolder= "Pages", });

            return pages;
        }

        public List<LandingPages> listOfLandingPages()
        {
            List<LandingPages> landingPages = new List<LandingPages>();

            landingPages.Add(new LandingPages() { Name = "Admin", Title = "Admin", LandingPage = "/Pages/UserHomePage" });
            landingPages.Add(new LandingPages() { Name = "User", Title = "User", LandingPage = "/Pages/UserHomePage" });
            landingPages.Add(new LandingPages() { Name = "PMO", Title = "ITSM", LandingPage = "/Pages/HomePMO" });
            landingPages.Add(new LandingPages() { Name = "UGITSuperAdmin", Title = "SuperAdmin", LandingPage = "/Pages/HomeSuperAdmin" });
            landingPages.Add(new LandingPages() { Name = "NetworkManagement", Title = "Network Management", LandingPage = "/Pages/UserHomePage" });
            landingPages.Add(new LandingPages() { Name = "ApplicationManagement", Title = "Application Management", LandingPage = "/Pages/UserHomePage" });
            landingPages.Add(new LandingPages() { Name = "TicketAdmin", Title = "Ticket Admin", LandingPage = "/Pages/UserHomePage" });
            landingPages.Add(new LandingPages() { Name = "ResourceAdmin", Title = "Resource Admin", LandingPage = "/Pages/UserHomePage" });
            landingPages.Add(new LandingPages() { Name = "uGovernITMembers", Title = "uGovernIT Members", LandingPage = "/Pages/UserHomePage" });
            landingPages.Add(new LandingPages() { Name = "SecurityManagement", Title = "Security Management", LandingPage = "/Pages/UserHomePage" });
            landingPages.Add(new LandingPages() { Name = "ExecutiveManagement", Title = "Executive Management", LandingPage = "/Pages/UserHomePage" });
            landingPages.Add(new LandingPages() { Name = "Trial User", Title = "TrialUser", LandingPage = "/Pages/TrialUser" });
            landingPages.Add(new LandingPages() { Name = "ITSM", Title = "ITSM", LandingPage = "/Pages/ITSM" });
            landingPages.Add(new LandingPages() { Name = "PMM", Title = "PMM", LandingPage = "/Pages/HomePMM" });
            return landingPages;
        }

        public AccountInfo SetAdminAccountInfo(AccountInfo accountInfo, string accountId)
        {
            var adminEmail = $"Admin@{accountId}.com";
            var adminUserName = $"Administrator_{accountId}";

            if (accountInfo == null)
            {
                accountInfo = new AccountInfo();
            }

            if (string.IsNullOrEmpty(accountInfo.AccountID))
            {
                accountInfo.AccountID = accountId;
            }
            if (string.IsNullOrEmpty(accountInfo.Email))
            {
                accountInfo.Email = adminEmail;
            }
            if (string.IsNullOrEmpty(accountInfo.UserName))
            {
                accountInfo.UserName = adminUserName;
            }
            if (string.IsNullOrEmpty(accountInfo.Password))
            {
                accountInfo.Password = ConfigData.Password;
            }

            return accountInfo;
        }

        public static AccountInfo GetAdminAccountInfo(string accountId, string email, string password = null)
        {
            return new AccountInfo
            {
                AccountID = accountId,
                Email = email,
                UserName = $"Administrator_{accountId}",
                Password = password == null ? ConfigData.Password : password
            };
        }

        public class Variables
        {
            private static string name = DBConstant.SystemAdminID;

            public static string Name
            {
                get { return name; }
                set { name = userId; }

            }
        }

        public WikiContents AddFetchWikiContent(WikiContents wikiContent, ApplicationContext context)
        {
            WikiContentsManager wikiContentsManager = new WikiContentsManager(context);

            WikiArticlesManager wikiArticlesManager = new WikiArticlesManager(context);

            wikiContentsManager.Insert(wikiContent);

            WikiContents wikiContents = wikiContentsManager.Get($"Where {DatabaseObjects.Columns.TenantID}='{context.TenantID}' and {DatabaseObjects.Columns.TicketId}='{wikiContent.TicketId}'");

            return wikiContents;
        }


    }
}

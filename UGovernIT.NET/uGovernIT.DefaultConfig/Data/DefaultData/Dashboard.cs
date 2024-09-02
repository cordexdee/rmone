using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
using System.Data.SqlTypes;
using uGovernIT.DefaultConfig;

namespace uGovernIT.DefaultConfig.Data.DefaultData
{
    public class Dashboard : IDashboard
    {
        public static string userId = "1";
        public static string managerUserId = "1";
        public static string userName = ""; // Obtained from userId in UGovernITDefault.Initialize()

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

        public UGITModule Module
        {
            get
            {
                return new UGITModule();
            }
        }

        public string ModuleName
        {
            get
            {
                return string.Empty;
            }
        }

        public List<ACRType> GetACRTypes()
        {
            List<ACRType> mList = new List<ACRType>();
            return mList;
        }

        public List<ChartFilter> GetChartFilters()
        {
            List<ChartFilter> mList = new List<ChartFilter>();
            // Console.WriteLine("  ChartFilters");

            List<string[]> dataList = new List<string[]>();
            dataList.Add(new string[] { "Priority", "TicketPriorityLookup", "DashboardSummary", "SMS", "", "0", "1" });//1
            dataList.Add(new string[] { "Owner", "OwnerUser", "DashboardSummary", "SMS", "", "0", "1" });//2
            dataList.Add(new string[] { "PRP", "PRPUser", "DashboardSummary", "SMS", "", "0", "1" });//3
            dataList.Add(new string[] { "Status", "GenericStatusLookup", "DashboardSummary", "SMS", "", "0", "1" });//4
            dataList.Add(new string[] { "Modules", "ModuleNameLookup", "DashboardSummary", "SMS", "", "0", "1" });//5
            dataList.Add(new string[] { "Intiation Date", "InitiatedDate", "DashboardSummary", "SMS", "", "0", "1" });//6

            // For custom dashboard by Manish
            dataList.Add(new string[] { "User Info", "", "Users", "Governance", "8", "1", "1" });//7
            dataList.Add(new string[] { "ITStaff", "IT", "Users", "Governance", "8", "1", "1" });//8
            dataList.Add(new string[] { "Closed Date", "ClosedDate", "DateTime", "SMS", "", "1", "1" });//9
            dataList.Add(new string[] { "User Types", "UserTypes", "ModuleUserTypes", "SMS", "", "1", "1" });//10
            dataList.Add(new string[] { "Stage", "StageTitle", "ModuleStages", "Governance", "6", "1", "1" });//11
            dataList.Add(new string[] { "Configuration", "KeyName", "ConfigurationVariable", "", "", "1", "1" });//12

            // by Manish
            dataList.Add(new string[] { "Asset Owner", "OwnerUser", "Assets", "Governance", "10", "1", "1" });//13
            dataList.Add(new string[] { "NPR Requests", "", "NPRRequest", "Governance", "6", "1", "1" });//14
            dataList.Add(new string[] { "PMM Projects", "", "PMM", "Governance", "7", "1", "1" });//15
            dataList.Add(new string[] { "Assets", "", "Assets", "Governance", "7", "1", "1" });//16
            dataList.Add(new string[] { "User Statistics", "", "ModuleUserStatistics", "SMS", "", "1", "1" });//17

            // For Stage skipping
            dataList.Add(new string[] { "Category", "TicketRequestTypeCategory", "", "", "2", "0", "0" });//18          
            dataList.Add(new string[] { "IT Steering Committee Approval Required", "IsSteeringApprovalRequired", "", "", "6", "0", "0" });//19
            dataList.Add(new string[] { "DRQ Rapid Request", "TicketRapidRequest", "DRQTicket", "SMS", "4", "0", "0" });//20
            dataList.Add(new string[] { "Workflow Type", "TicketRequestTypeWorkflow", "", "", "", "0", "0" });//21   
            dataList.Add(new string[] { "SVC Owner Approval Required", "OwnerApprovalRequired", "SVCRequests", "SMS", "13", "0", "0" });//22

            foreach (string[] data in dataList)
            {
                ChartFilter chartFilters = new ChartFilter();
                chartFilters.Title = data[0];
                chartFilters.ColumnName = data[1];
                chartFilters.ListName = data[2];
                chartFilters.ModuleType = data[3];
                chartFilters.ModuleNameLookup = data[4];
                chartFilters.IsDefault = Convert.ToBoolean(Convert.ToInt32(data[5]));
                chartFilters.ValueAsId = data[6];
                mList.Add(chartFilters);
            }
            return mList;
        }

        public List<ChartFormula> GetChartFormula()
        {
            List<ChartFormula> mList = new List<ChartFormula>();
            // Console.WriteLine("  ChartFormula");

            List<string[]> dataList = new List<string[]>();

            dataList.Add(new string[] { "Previous Month Tickets", "(Initiated Date = \"Previous Month\")", "<And><Geq><FieldRef Name='InitiatedDate' /><Value Type='' >[#2-Now-2-Day(s)#]</Value></Geq><Leq><FieldRef Name='InitiatedDate' /><Value Type='' >[#1-Now-2-Day(s)#]</Value></Leq></And>", "2", "SMS", "", "0" }); // 1
            dataList.Add(new string[] { "PRS Tickets", "(Modules = \"PRS\") ", "<Eq><FieldRef Name='ModuleNameLookup' /><Value Type='' >PRS</Value></Eq>", "3", "SMS", "", "0" }); // 2
            dataList.Add(new string[] { "PRS Closed Requests", "((Modules = \"PRS\") AND (Status = \"Closed\"))", "<And><Eq><FieldRef Name='GenericStatusLookup' /><Value Type='' >Closed</Value></Eq><Eq><FieldRef Name='ModuleNameLookup' /><Value Type='' >PRS</Value></Eq></And>", "4", "SMS", "", "0" }); // 3
            dataList.Add(new string[] { "TSR Tickets", "(Modules = \"TSR\")", "<Eq><FieldRef Name='ModuleNameLookup' /><Value Type='' >TSR</Value></Eq>", "5", "SMS", "", "0" }); // 4
            dataList.Add(new string[] { "TSR Closed Requests", "((Modules = \"TSR\") AND (Status = \"Closed\"))", "<And><Eq><FieldRef Name='GenericStatusLookup' /><Value Type='' >Closed</Value></Eq><Eq><FieldRef Name='ModuleNameLookup' /><Value Type='' >TSR</Value></Eq></And>", "6", "SMS", "", "0" }); // 5

            // For dashboard panel by Manish
            dataList.Add(new string[] { "ITConsultant", "", "##^^7~Eq~IsConsultant=1^^$#$AND##^^7~Eq~IT=1^^", "", "Governance", "8", "1" }); // 6
            dataList.Add(new string[] { "ITStaff", "", "##^^7~Eq~IsConsultant=0^^$#$AND##^^7~Eq~IT=1^^", "", "Governance", "8", "1" }); // 7
            dataList.Add(new string[] { "Open Tickets", "", "##^^4~Eq~Open^^$#$OR##^^4~Eq~Unassigned^^$#$OR##^^4~Eq~On-Hold^^", "", "SMS", "", "1" }); // 8
            dataList.Add(new string[] { "Previous Month Closed Tickets", "", "##^^4~Eq~Closed^^$#$AND##^^9~Eq~Previous_Month^^", "", "SMS", "", "1" }); // 9
            dataList.Add(new string[] { "Open Ticket Of High Priority", "", "##^^4~Eq~Open^^$#$AND##^^1~Eq~High^^", "", "SMS", "", "1" }); // 10
            dataList.Add(new string[] { "My Tickets", "", "##^^17~Eq~TicketUser=my^^", "", "SMS", "", "1" }); // 11

            // NOTE: This has to be NPR start + 1 to NPR Approved -1
            // Need something like this: ##^^11~Eq~38^^$#$OR##^^11~Eq~39^^$#$OR##^^11~Eq~40^^$#$OR##^^11~Eq~41^^
            // This doesn't work: ##^^14~Neq~TicketStatus=Approved^^$#$AND##^^14~Neq~TicketStatus=Closed^^$#$AND##^^14~Neq~TicketStatus=On Hold^^
            string expression = string.Empty;
            for (int stageID = nprMgrApprovalStageID; stageID < nprApprovedStageID; stageID++)
            {
                if (expression != string.Empty)
                    expression += "$#$OR";
                expression += "##^^11~Eq~" + stageID + "^^";
            }
            dataList.Add(new string[] { "NPR Pending Requests", "", expression, "", "Governance", "6", "1" }); // 12

            // NOTE: This has to be PMM start to PMM Closed -1 
            // Need something like this: ##^^11~Eq~44^^$#$OR##^^11~Eq~45^^$#$OR##^^11~Eq~46^^$#$OR##^^11~Eq~47^^$#$OR##^^11~Eq~48^^$#$OR##^^11~Eq~49^^
            expression = string.Empty;
            for (int stageID = pmmStartStageID; stageID < pmmClosedStageID; stageID++)
            {
                if (expression != string.Empty)
                    expression += "$#$OR";
                expression += "##^^11~Eq~" + stageID + "^^";
            }
            dataList.Add(new string[] { "PMM Active Projects", "", expression, "", "Governance", "7", "1" }); // 13

            // PMM Start +3 & +4
            dataList.Add(new string[] { "Pending Governance Review", "", "##^^11~Eq~" + nprITGovernanceStageID.ToString() + "^^", "", "Governance", "6", "1" }); // 14
            dataList.Add(new string[] { "Pending IT Steering Committee Review", "", "##^^11~Eq~" + nprITSteeringeStageID.ToString() + "^^", "", "Governance", "6", "1" }); // 15

            // This is WRONG (hard-coded), need to replace with actual data from BCS link
            dataList.Add(new string[] { "# of Active Dashboards", "", "##^^12~Eq~ActiveDashbaordCount^^", "", "", "", "1" }); // 16
            dataList.Add(new string[] { "# of Active Analytics", "", "##^^12~Eq~ActiveAnalyticCount^^", "", "", "", "1" }); // 17

            // NPR Approved but not yet started
            dataList.Add(new string[] { "NPR Ready to Start", "", "##^^14~Eq~TicketStatus=Approved^^$#$AND##^^14~IsNull~TicketPMMIdLookup^^", "", "Governance", "6", "1" }); // 18

            //For asset dashboard by Manish
            dataList.Add(new string[] { "All Assets", "", "##^^13~Eq~all^^", "", "Governance", "10", "1" }); // 19
            dataList.Add(new string[] { "My Assets", "", "##^^13~Eq~my^^", "", "Governance", "10", "1" }); // 20
            dataList.Add(new string[] { "All Consultant", "", "##^^7~Eq~IsConsultant=1^^", "", "Governance", "8", "1" }); // 21
            dataList.Add(new string[] { "All Staff", "", "##^^7~Eq~IsConsultant=0^^", "", "Governance", "8", "1" }); // 22
            dataList.Add(new string[] { "Assets By SubCategory", "", "##^^15~IsNotNull~BudgetSubCategory^^$#$AND##^^15~Group~BudgetSubCategory^^", "", "Governance", "10", "1" }); // 23

            //For Stage Skipping
            dataList.Add(new string[] { "Category is NOT Account Setup", "", @"##^^18~Neq~Account Setup^^", "", "", "2", "0" }); // 24
            dataList.Add(new string[] { "IT Steering Approval Required", "", @"##^^19~Eq~0^^", "", "", "6", "0" }); // 25

            //All SMS Open tickets
            dataList.Add(new string[] { "Open Tickets", "(Status != \"Closed\"))", "<Neq><FieldRef Name='GenericStatusLookup' /><Value Type='' >Closed</Value></Neq>", "", "SMS", "", "0" }); // 26

            // To allow urgent DRQs to skip stages
            dataList.Add(new string[] { "DRQ Rapid Request", "", @"##^^20~Eq~Yes^^", "", "", "4", "0" }); // 27

            // Skipping testing stage if Workflow Type is set to NoTest
            dataList.Add(new string[] { "Skip Testing Phase", "", @"##^^21~Eq~NoTest^^", "", "", "", "0" }); // 28

            // Skip Owner approval stages in SVC module if OwnerApprovalRequired is false
            dataList.Add(new string[] { "SVC Owner Approval Required", "", @"##^^22~Eq~No^^", "", "", "13", "0" }); // 29

            // Skipping approval stages stage if Workflow Type is set to NoTest
            dataList.Add(new string[] { "Skip Approval Stages", "", @"##^^21~Eq~SkipApprovals^^", "", "", "", "0" }); // 30

            foreach (string[] data in dataList)
            {
                ChartFormula chartFormula = new ChartFormula();
                chartFormula.Title = data[0];
                chartFormula.Formula = data[1];
                chartFormula.FormulaValue = data[2];
                chartFormula.ChartTemplateIds = data[3];
                chartFormula.ModuleType = data[4];
                chartFormula.ModuleNameLookup = data[5];
                chartFormula.IsDefault = Convert.ToBoolean(Convert.ToInt32(data[6]));
                mList.Add(chartFormula);
            }
            return mList;
        }

        public List<ChartTemplate> GetChartTemplates()
        {

            List<ChartTemplate> mList = new List<ChartTemplate>();
            // Console.WriteLine("  ChartTemplates");

            List<string[]> dataList = new List<string[]>();
            string dashboard = string.Empty;

            // Dashboard panels template
            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""PanelSetting""><type>Panel</type><ChartContainerTitle /><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><DashboardID>113a512a-afd3-496b-82b6-61ded51762d7</DashboardID><Expressions><DashboardPanelLink><LinkID>7179b331-09f1-4d73-837b-1df48c6a6364</LinkID><PanelModuleType>SMS</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>8</FormulaId><Title>All Open Tickets</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>Open Tickets: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>daa1ffd8-01b2-4760-9867-b359742fb05f</LinkID><PanelModuleType>SMS</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>10</FormulaId><Title>High Priority Tickets</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>High Priority: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>1</Order></DashboardPanelLink><DashboardPanelLink><LinkID>0be65923-68f4-4468-acfa-98a295ee7880</LinkID><PanelModuleType>SMS</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>11</FormulaId><Title>All My Tickets</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>My Tickets: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>2</Order></DashboardPanelLink><DashboardPanelLink><LinkID>9668faa1-698e-416d-b5fe-a5f48e53d794</LinkID><PanelModuleType>SMS</PanelModuleType><Title>Go to SMS Home Page</Title><LinkUrl>/SitePages/SMS.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>0</ModuleName><ExpressionFormat /><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>2</Order></DashboardPanelLink></Expressions><PanelID>113a512a-afd3-496b-82b6-61ded51762d7</PanelID><PanelDetail><LinkID>7179b331-09f1-4d73-837b-1df48c6a6364</LinkID><PanelModuleType>SMS</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>8</FormulaId><Title>All Open Tickets</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>Open Tickets: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></PanelDetail><IconUrl>/Content/images/ugovernit/TSR_32x32.png</IconUrl><ColumnViewType>0</ColumnViewType></panel><Order>1</Order><Title>Service Management System</Title><Description /><PanelType>Panel</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>true</IsHideDescription><Width>200</Width><Height>175</Height><SPTheme>Accent2</SPTheme></UDashboard>";
            dataList.Add(new string[] { "Service Management System", dashboard, "0" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""PanelSetting""><type>Panel</type><ChartContainerTitle /><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><DashboardID>5aded5c0-88fc-444e-8732-ce93a9af6373</DashboardID><Expressions><DashboardPanelLink><LinkID>0be65923-68f4-4468-acfa-98a295ee7880</LinkID><PanelModuleType>SMS</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>11</FormulaId><Title>All My Tickets</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>My Tickets: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>d641cdb3-25ca-454d-9099-7818e43542ea</LinkID><PanelModuleType>SMS</PanelModuleType><Title>Go to SMS Home Page</Title><LinkUrl>/SitePages/SMS.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>0</ModuleName><ExpressionFormat /><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>0</Order></DashboardPanelLink></Expressions><PanelID>5aded5c0-88fc-444e-8732-ce93a9af6373</PanelID><IconUrl>Content/images/ugovernit/PRS_32x32.png</IconUrl><PanelDetail><LinkID>0be65923-68f4-4468-acfa-98a295ee7880</LinkID><PanelModuleType>SMS</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>11</FormulaId><Title>All My Tickets</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>My Tickets: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></PanelDetail><ColumnViewType>0</ColumnViewType></panel><Order>2</Order><Title>All My Tickets</Title><Description /><PanelType>Panel</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>true</IsHideDescription><Width>200</Width><Height>175</Height><SPTheme>Accent2</SPTheme></UDashboard>";
            dataList.Add(new string[] { "All My Tickets", dashboard, "0" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""PanelSetting""><type>Panel</type><ChartContainerTitle /><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><DashboardID>8021aa1e-a39a-4e83-b005-a517eb10da9a</DashboardID><Expressions><DashboardPanelLink><LinkID>9905554c-93ed-4c05-9abe-5dccfd53a821</LinkID><PanelModuleType>All</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>17</FormulaId><Title>All Active Analytics</Title><LinkUrl>http://track.ugovernit.com:83</LinkUrl><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>Active Analytics: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>35ad2270-ae51-4fcd-8a20-626f82f7d6de</LinkID><PanelModuleType>All</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>16</FormulaId><Title>All Active Dashboards</Title><LinkUrl>http://track.ugovernit.com:83</LinkUrl><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>Active Dashboards: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>e2e25061-7470-44a1-9252-fef7f0a14ff9</LinkID><PanelModuleType>SMS</PanelModuleType><Title>Go to IT Analytics</Title><LinkUrl>http://track.ugovernit.com:83</LinkUrl><ViewType>0</ViewType><ModuleName>0</ModuleName><ExpressionFormat /><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>0</Order></DashboardPanelLink></Expressions><PanelID>8021aa1e-a39a-4e83-b005-a517eb10da9a</PanelID><IconUrl>Content/images/ugovernit/ITAnalytics_32x32.png</IconUrl><PanelDetail><LinkID>9905554c-93ed-4c05-9abe-5dccfd53a821</LinkID><PanelModuleType>All</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>17</FormulaId><Title>All Active Analytics</Title><LinkUrl>http://track.ugovernit.com:83</LinkUrl><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>Active Analytics: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></PanelDetail><ColumnViewType>0</ColumnViewType></panel><Order>4</Order><Title>IT Analytics</Title><Description /><PanelType>Panel</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>true</IsHideDescription><Width>200</Width><Height>175</Height><SPTheme>Accent2</SPTheme></UDashboard>";
            dataList.Add(new string[] { "IT Analytics", dashboard, "0" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""PanelSetting""><type>Panel</type><ChartContainerTitle /><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><DashboardID>b0da8ed6-275a-4346-a8c0-a2f372cd1193</DashboardID><Expressions><DashboardPanelLink><LinkID>18fbd56f-6c57-4560-8924-613bb80bc176</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>14</FormulaId><Title>Projects Pending Governance Review</Title><LinkUrl>/SitePages/ITG.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>6</ModuleName><ExpressionFormat>Projects Pending IT Governance Review: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>f212330e-0a34-43cb-87dc-fc3fb3befa02</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>15</FormulaId><Title>Projects Pending Steering Committee Review</Title><LinkUrl>/SitePages/ITG.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>6</ModuleName><ExpressionFormat>Projects Pending Steering Committee Review: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>a739d84c-6001-41b2-b904-2ce982d5b798</LinkID><PanelModuleType>Governance</PanelModuleType><Title>Go to IT Governance</Title><LinkUrl>/SitePages/ITG.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>0</ModuleName><ExpressionFormat /><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>3</Order></DashboardPanelLink></Expressions><PanelID>b0da8ed6-275a-4346-a8c0-a2f372cd1193</PanelID><IconUrl>Content/images/ugovernit/ITG_32x32.png</IconUrl><PanelDetail><LinkID>18fbd56f-6c57-4560-8924-613bb80bc176</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>14</FormulaId><Title>Projects Pending Governance Review</Title><LinkUrl>/SitePages/ITG.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>6</ModuleName><ExpressionFormat>Projects Pending IT Governance Review: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></PanelDetail><ColumnViewType>0</ColumnViewType></panel><Order>3</Order><Title>Virtual PMO</Title><Description /><PanelType>Panel</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>true</IsHideDescription><Width>200</Width><Height>175</Height><SPTheme>Accent2</SPTheme></UDashboard>";
            dataList.Add(new string[] { "Virtual PMO", dashboard, "0" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""PanelSetting""><type>Panel</type><ChartContainerTitle /><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><DashboardID>faf8d472-ccf6-4e5a-84ac-ce534d873a33</DashboardID><Expressions><DashboardPanelLink><LinkID>d42a3678-404b-43f1-9379-8f5a163e589d</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>12</FormulaId><Title>Pending Approval</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>6</ModuleName><ExpressionFormat>Pending Approval: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>b812c328-2405-4f29-9f06-2e5734fbacc5</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>18</FormulaId><Title>Ready to Start</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>6</ModuleName><ExpressionFormat>Ready to Start: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>3cbcf69a-4855-4550-a9c5-66794540011b</LinkID><PanelModuleType>All</PanelModuleType><Title>Go to NPR</Title><LinkUrl>/SitePages/NPRRequests.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>0</ModuleName><ExpressionFormat /><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>5</Order></DashboardPanelLink></Expressions><PanelID>faf8d472-ccf6-4e5a-84ac-ce534d873a33</PanelID><IconUrl>Content/images/ugovernit/NPR_32x32.png</IconUrl><PanelDetail><LinkID>d42a3678-404b-43f1-9379-8f5a163e589d</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>12</FormulaId><Title>Pending Approval</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>6</ModuleName><ExpressionFormat>Pending Approval: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></PanelDetail><ColumnViewType>0</ColumnViewType></panel><Order>5</Order><Title>NPR Requests</Title><Description /><PanelType>Panel</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>true</IsHideDescription><Width>200</Width><Height>175</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "NPR Requests", dashboard, "0" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""PanelSetting""><type>Panel</type><ChartContainerTitle /><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><DashboardID>da0ded5d-1540-4b95-a457-7dfd44b4afe5</DashboardID><Expressions><DashboardPanelLink><LinkID>ee465c1e-98ea-49c9-829f-b29460d3b179</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>13</FormulaId><Title>All Active Projects</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>7</ModuleName><ExpressionFormat>Active Projects: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>ac93815d-736f-4105-9a1b-3e14945348d2</LinkID><PanelModuleType>All</PanelModuleType><Title>Go to PMM</Title><LinkUrl>/SitePages/PMMProjects.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>0</ModuleName><ExpressionFormat /><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>0</Order></DashboardPanelLink></Expressions><PanelID>da0ded5d-1540-4b95-a457-7dfd44b4afe5</PanelID><IconUrl>Content/images/ugovernit/PMM_32x32.png</IconUrl><PanelDetail><LinkID>ee465c1e-98ea-49c9-829f-b29460d3b179</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>13</FormulaId><Title>All Active Projects</Title><LinkUrl /><ViewType>1</ViewType><ModuleName>7</ModuleName><ExpressionFormat>Active Projects: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></PanelDetail><ColumnViewType>0</ColumnViewType></panel><Order>6</Order><Title>Active PMM Projects</Title><Description /><PanelType>Panel</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>true</IsHideDescription><Width>200</Width><Height>175</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "Active PMM Projects", dashboard, "0" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""PanelSetting""><type>Panel</type><ChartContainerTitle /><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><DashboardID>9611de55-e9fd-440f-b7d7-1c06c50d4df0</DashboardID><Expressions><DashboardPanelLink><LinkID>630751fa-de1d-45b0-9820-cb5dd358c3f9</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>7</FormulaId><Title>All IT Staff</Title><LinkUrl>/sitePages/RMM.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>8</ModuleName><ExpressionFormat>IT Staff: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>c6c09a6f-5d19-4e88-8be7-809e04e2d4f9</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>6</FormulaId><Title>All IT Consultant</Title><LinkUrl>/sitePages/RMM.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>8</ModuleName><ExpressionFormat>IT Consultants: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>3a9b63d9-046e-49fa-a374-b75dc8403d3f</LinkID><PanelModuleType>All</PanelModuleType><Title>Go to RMM</Title><LinkUrl>/SitePages/RMM.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>0</ModuleName><ExpressionFormat /><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>0</Order></DashboardPanelLink></Expressions><PanelID>9611de55-e9fd-440f-b7d7-1c06c50d4df0</PanelID><IconUrl>Content/images/ugovernit/RMM_32x32.png</IconUrl><PanelDetail><LinkID>630751fa-de1d-45b0-9820-cb5dd358c3f9</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>7</FormulaId><Title>All IT Staff</Title><LinkUrl>/sitePages/RMM.aspx</LinkUrl><ViewType>0</ViewType><ModuleName>8</ModuleName><ExpressionFormat>IT Staff: $exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></PanelDetail><ColumnViewType>0</ColumnViewType></panel><Order>7</Order><Title>RMM Resource Counts</Title><Description /><PanelType>Panel</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>true</IsHideDescription><Width>200</Width><Height>175</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "RMM Resource Counts", dashboard, "0" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""PanelSetting""><type>Panel</type><ChartContainerTitle /><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><DashboardID>11f837b8-108e-4568-9f41-359dec588164</DashboardID><StartFromNewLine>false</StartFromNewLine><Expressions><DashboardPanelLink><LinkID>0e4e1e5e-dacd-4cc9-820e-fdbaa57378c3</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>5</ExpressionID><FormulaId>23</FormulaId><Title>Top 5 Categories</Title><LinkUrl>/SitePages/cmdb.aspx</LinkUrl><ViewType>1</ViewType><ModuleName>10</ModuleName><ExpressionFormat>$exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>ecb95467-de36-4699-96cb-13414e43f994</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>19</FormulaId><Title>Assets</Title><LinkUrl>/SitePages/filteredtickets.aspx</LinkUrl><ViewType>1</ViewType><ModuleName>10</ModuleName><ExpressionFormat>$exp$</ExpressionFormat><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>0</Order></DashboardPanelLink></Expressions><PanelID>11f837b8-108e-4568-9f41-359dec588164</PanelID><PanelDetail><LinkID>ecb95467-de36-4699-96cb-13414e43f994</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>19</FormulaId><Title>Assets</Title><LinkUrl>/SitePages/filteredtickets.aspx</LinkUrl><ViewType>1</ViewType><ModuleName>10</ModuleName><ExpressionFormat>$exp$</ExpressionFormat><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>0</Order></PanelDetail><IconUrl>/Content/images/ugovernit/CMDB_32x32.png</IconUrl><ColumnViewType>0</ColumnViewType></panel><Order>0</Order><Title>Top 5 Categories</Title><Description /><PanelType>Panel</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>200</Width><Height>175</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "Top 5 Asset Categories", dashboard, "0" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""PanelSetting""><type>Panel</type><ChartContainerTitle /><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><DashboardID>9b7b3e2a-4534-441b-a42d-b3989183a859</DashboardID><StartFromNewLine>false</StartFromNewLine><Expressions><DashboardPanelLink><LinkID>9913f046-d7be-4d6b-a248-9cf4031015cd</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>6</ExpressionID><FormulaId>23</FormulaId><Title>Top 5 Problem Categories</Title><LinkUrl>/SitePages/cmdb.aspx</LinkUrl><ViewType>1</ViewType><ModuleName>0</ModuleName><ExpressionFormat>$exp$</ExpressionFormat><UseAsPanel>false</UseAsPanel><IsHide>false</IsHide><Order>0</Order></DashboardPanelLink><DashboardPanelLink><LinkID>9d2a34a2-7729-49ed-a941-ee70e1db5904</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>23</FormulaId><Title>Assets</Title><LinkUrl>/SitePages/filteredtickets.aspx</LinkUrl><ViewType>1</ViewType><ModuleName>10</ModuleName><ExpressionFormat>$exp$</ExpressionFormat><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>0</Order></DashboardPanelLink></Expressions><PanelID>9b7b3e2a-4534-441b-a42d-b3989183a859</PanelID><PanelDetail><LinkID>9d2a34a2-7729-49ed-a941-ee70e1db5904</LinkID><PanelModuleType>Governance</PanelModuleType><ExpressionID>1</ExpressionID><FormulaId>23</FormulaId><Title>Assets</Title><LinkUrl>/SitePages/filteredtickets.aspx</LinkUrl><ViewType>1</ViewType><ModuleName>10</ModuleName><ExpressionFormat>$exp$</ExpressionFormat><UseAsPanel>true</UseAsPanel><IsHide>true</IsHide><Order>0</Order></PanelDetail><IconUrl>/Content/images/ugovernit/PRS_32x32.png</IconUrl><ColumnViewType>0</ColumnViewType></panel><Order>0</Order><Title>Top 5 Problem Categories</Title><Description /><PanelType>Panel</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>200</Width><Height>175</Height><SPTheme>Accent2</SPTheme></UDashboard>";
            dataList.Add(new string[] { "Top 5 Asset Problem Categories", dashboard, "0" });

            // Dashboard Chart Templates
            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>Active Requests by Priority</ContainerTitle><Description>Active Requests by Priority</Description><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>4efdfe92-6fbc-48de-88fa-df65c5c156c5</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>9a332aca-b420-456f-920f-2a13279ae109</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Modules</Title><SelectedField>ModuleNameLookup</SelectedField><Sequence>1</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>Detail</DataPointClickEvent><DateViewType /></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Priorities</Title><FactTable>DashboardSummary</FactTable><GroupByField>TicketPriorityLookup</GroupByField><Operator>Count</Operator><ExpressionFormula /><Order>1</Order><ChartType>StackedColumn</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>true</HideLabel><LabelStyle>Top</LabelStyle><LabelText>$Exp$</LabelText><YAsixType>Primary</YAsixType><DataPointClickEvent>Detail</DataPointClickEvent><FunctionExpression>ID</FunctionExpression></ChartExpression></Expressions><FactTable>DashboardSummary</FactTable><BasicDateFitlerStartField /><BasicDateFitlerEndField /><BasicDateFilterDefaultView /><BasicFilter /><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>true</HideLabel><LabelStyle>Top</LabelStyle><LabelText>$Exp$</LabelText></panel><Order>1</Order><Title>Active Requests by Priority</Title><Description>Active Requests by Priority</Description><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>300</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "Active Requests by Priority", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>$Date$: Tickets Created</ContainerTitle><Description>Monthly Report</Description><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>c58bd28f-858c-4090-84b8-b8460ad0e9a7</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>1fdf5485-74aa-4e5e-8faa-1219ebb45f37</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Modules</Title><SelectedField>ModuleNameLookup</SelectedField><Sequence>1</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>Detail</DataPointClickEvent><DateViewType /></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Total Tickets</Title><FactTable>DashboardSummary</FactTable><GroupByField /><Operator>Count</Operator><ExpressionFormula /><Order>1</Order><ChartType>Bar</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>ID</FunctionExpression></ChartExpression></Expressions><FactTable>DashboardSummary</FactTable><BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField><BasicDateFitlerEndField /><BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView><BasicFilter /><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>true</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /></panel><Order>2</Order><Title>$Date$: Tickets Created</Title><Description>Monthly Report</Description><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>300</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "$Date$: Tickets Created", dashboard, "1" });


            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>PRS Avg Days to Resolve by Priority</ContainerTitle><Description>PRS Avg Days to Resolve by Priority</Description><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>86dacaba-9cdb-447d-a25e-8187263453ba</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>0f9dcb76-0624-4c24-955a-aa52cb300df7</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Year</Title><SelectedField>InitiatedDate</SelectedField><Sequence>1</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType>year</DateViewType></ChartDimension><ChartDimension><Title>Month</Title><SelectedField>InitiatedDate</SelectedField><Sequence>2</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType>month</DateViewType></ChartDimension><ChartDimension><Title>Days</Title><SelectedField>InitiatedDate</SelectedField><Sequence>3</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>Detail</DataPointClickEvent><DateViewType>day</DateViewType></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Priorities</Title><FactTable>DashboardSummary</FactTable><GroupByField>TicketPriorityLookup</GroupByField><Operator>avg</Operator><ExpressionFormula /><Order>1</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>Daysdiff(ResolvedDate,InitiatedDate)</FunctionExpression></ChartExpression></Expressions><FactTable>DashboardSummary</FactTable><BasicDateFitlerStartField /><BasicDateFitlerEndField /><BasicDateFilterDefaultView /><BasicFilter>ModuleNameLookup = 'PRS'</BasicFilter><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>2</BorderWidth><HideLabel>false</HideLabel><LabelStyle /><LabelText /></panel><Order>3</Order><Title>PRS Avg Days to Resolve by Priority</Title><Description>PRS Avg Days to Resolve by Priority</Description><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>300</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "PRS Avg Days to Resolve by Priority", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>PRS Response Trends</ContainerTitle><Description>PRS Response Trends</Description><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>319c62b9-c3d4-43b7-9b35-760e1568804a</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>9a3d7936-ecd6-42e0-80ba-2e6fd24ac488</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Year</Title><SelectedField>InitiatedDate</SelectedField><Sequence>1</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType>year</DateViewType></ChartDimension><ChartDimension><Title>Month</Title><SelectedField>InitiatedDate</SelectedField><Sequence>2</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType>month</DateViewType></ChartDimension><ChartDimension><Title>Days</Title><SelectedField>InitiatedDate</SelectedField><Sequence>3</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>Detail</DataPointClickEvent><DateViewType>day</DateViewType></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Closed Requests</Title><FactTable>DashboardSummary</FactTable><GroupByField /><Operator>Count</Operator><ExpressionFormula /><Order>1</Order><ChartType>Column</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>ID</FunctionExpression></ChartExpression><ChartExpression><Title>Days to Resolve</Title><FactTable>DashboardSummary</FactTable><GroupByField /><Operator>avg</Operator><ExpressionFormula /><Order>2</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>true</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /><YAsixType>Secondary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>Daysdiff(ResolvedDate,InitiatedDate</FunctionExpression></ChartExpression><ChartExpression><Title>Days to Respond</Title><FactTable>DashboardSummary</FactTable><GroupByField /><Operator>avg</Operator><ExpressionFormula /><Order>3</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>true</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /><YAsixType>Secondary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>Daysdiff(AssignedDate,InitiatedDate)</FunctionExpression></ChartExpression><ChartExpression><Title>Days to Close</Title><FactTable>DashboardSummary</FactTable><GroupByField /><Operator>avg</Operator><ExpressionFormula /><Order>4</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>true</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /><YAsixType>Secondary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>Daysdiff(ClosedDate,InitiatedDate)</FunctionExpression></ChartExpression></Expressions><FactTable>DashboardSummary</FactTable><BasicDateFitlerStartField /><BasicDateFitlerEndField /><BasicDateFilterDefaultView /><BasicFilter>ModuleNameLookup = 'PRS' AND GenericStatusLookup = 'Closed'</BasicFilter><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>true</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /></panel><Order>4</Order><Title>PRS Response Trends</Title><Description>PRS Response Trends</Description><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>300</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "PRS Response Trends", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>TSR Avg Days to Resolve by Priority</ContainerTitle><Description>TSR Avg Days to Resolve by Priority</Description><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>0b55fa95-8034-4923-bd48-329efbf16bfd</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>34cbae5f-49af-4aec-9509-e732195c02e2</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Year</Title><SelectedField>InitiatedDate</SelectedField><Sequence>1</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType>year</DateViewType></ChartDimension><ChartDimension><Title>Month</Title><SelectedField>InitiatedDate</SelectedField><Sequence>2</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType>month</DateViewType></ChartDimension><ChartDimension><Title>Days</Title><SelectedField>InitiatedDate</SelectedField><Sequence>3</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>Detail</DataPointClickEvent><DateViewType>day</DateViewType></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Priorities</Title><FactTable>DashboardSummary</FactTable><GroupByField>TicketPriorityLookup</GroupByField><Operator>avg</Operator><ExpressionFormula /><Order>1</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>Daysdiff(ResolvedDate,InitiatedDate)</FunctionExpression></ChartExpression></Expressions><FactTable>DashboardSummary</FactTable><BasicDateFitlerStartField /><BasicDateFitlerEndField /><BasicDateFilterDefaultView /><BasicFilter>ModuleNameLookup = 'TSR'</BasicFilter><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>2</BorderWidth><HideLabel>false</HideLabel><LabelStyle /><LabelText /></panel><Order>5</Order><Title>TSR Avg Days to Resolve by Priority</Title><Description>TSR Avg Days to Resolve by Priority</Description><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>300</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "TSR Avg Days to Resolve by Priority", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>TSR Response Trends</ContainerTitle><Description>TSR Response Trends</Description><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>6bd9875a-8276-4378-844e-e88343f917d9</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>9a3d7936-ecd6-42e0-80ba-2e6fd24ac488</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Year</Title><SelectedField>InitiatedDate</SelectedField><Sequence>1</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType>year</DateViewType></ChartDimension><ChartDimension><Title>Month</Title><SelectedField>InitiatedDate</SelectedField><Sequence>2</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType>month</DateViewType></ChartDimension><ChartDimension><Title>Days</Title><SelectedField>InitiatedDate</SelectedField><Sequence>3</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>DashboardSummary</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType>day</DateViewType></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Closed Requests</Title><FactTable>DashboardSummary</FactTable><GroupByField /><Operator>Count</Operator><ExpressionFormula /><Order>1</Order><ChartType>Column</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>true</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>ID</FunctionExpression></ChartExpression><ChartExpression><Title>Days to Resolve</Title><FactTable>DashboardSummary</FactTable><GroupByField /><Operator>avg</Operator><ExpressionFormula /><Order>2</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>true</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>Daysdiff(ResolvedDate,InitiatedDate)</FunctionExpression></ChartExpression><ChartExpression><Title>Days to Respond</Title><FactTable>DashboardSummary</FactTable><GroupByField /><Operator>avg</Operator><ExpressionFormula /><Order>3</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>true</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>Daysdiff(AssignedDate,InitiatedDate</FunctionExpression></ChartExpression><ChartExpression><Title>Days to Close</Title><FactTable>DashboardSummary</FactTable><GroupByField /><Operator>avg</Operator><ExpressionFormula /><Order>4</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>true</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>Daysdiff(ClosedDate,InitiatedDate</FunctionExpression></ChartExpression></Expressions><FactTable>DashboardSummary</FactTable><BasicDateFitlerStartField /><BasicDateFitlerEndField /><BasicDateFilterDefaultView /><BasicFilter>GenericStatusLookup = 'Closed' AND ModuleNameLookup = 'TSR'</BasicFilter><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>true</HideLabel><LabelStyle>Auto</LabelStyle><LabelText /></panel><Order>4</Order><Title>TSR Response Trends</Title><Description>TSR Response Trends</Description><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>300</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "TSR Response Trends", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>NPR Metrics</ContainerTitle><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>78a77e3f-eeac-4e6e-83f0-86f85c159494</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>25252af7-d2bd-4372-938f-23691934c884</ChartId><IsComulative>false</IsComulative><Dimensions /><Expressions><ChartExpression><Title># of NPR Pending Approval</Title><FactTable>NPRRequest</FactTable><GroupByField /><Operator>Count</Operator><ExpressionFormula>TicketStatus = 'IT Governance Review' OR TicketStatus = 'IT Steering Committee Review'</ExpressionFormula><Order>1</Order><ChartType>Bar</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Detail</DataPointClickEvent><FunctionExpression>TicketId</FunctionExpression></ChartExpression><ChartExpression><Title># of NPRs Ready to Start</Title><FactTable>NPRRequest</FactTable><GroupByField /><Operator>Count</Operator><ExpressionFormula>TicketStatus = 'Approved' AND TicketPMMIdLookup = ''</ExpressionFormula><Order>2</Order><ChartType>Bar</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Detail</DataPointClickEvent><FunctionExpression>TicketId</FunctionExpression></ChartExpression><ChartExpression><Title># of NPR Rejected</Title><FactTable>NPRRequest</FactTable><GroupByField /><Operator>Count</Operator><ExpressionFormula>TicketStatus = 'Rejected'</ExpressionFormula><Order>3</Order><ChartType>Bar</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Detail</DataPointClickEvent><FunctionExpression>TicketId</FunctionExpression></ChartExpression><ChartExpression><Title># of Current Projects</Title><FactTable>NPRRequest</FactTable><GroupByField /><Operator>Count</Operator><ExpressionFormula>TicketPMMIdLookup &lt;&gt; '' AND TicketStatus &lt;&gt; 'Closed'</ExpressionFormula><Order>4</Order><ChartType>Bar</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Detail</DataPointClickEvent><FunctionExpression>TicketId</FunctionExpression></ChartExpression></Expressions><FactTable>NPRRequest</FactTable><BasicDateFitlerStartField>TicketCreationDate</BasicDateFitlerStartField><BasicDateFitlerEndField /><BasicDateFilterDefaultView /><BasicFilter /><HideGrid>true</HideGrid><Palette>Excel</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>false</HideLabel><LabelStyle /><LabelText /></panel><Order>1</Order><Title>NPR Metrics</Title><Description /><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>500</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "NPR Metrics", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>$Date$ :Top 10 Projects</ContainerTitle><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>8017c3a0-b2f3-4dc3-aa15-634468bbbd42</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>990dfbdb-deb3-4fe3-937c-c4424700cec1</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Projects</Title><SelectedField>WorkItem</SelectedField><Sequence>1</Sequence><PickTopDataPoint>10</PickTopDataPoint><Operator>Sum</Operator><OperatorField>AllocationHour</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>ResourceUsageSummaryMonthWise</FactTable><FilterID>0</FilterID><DataPointClickEvent>Detail</DataPointClickEvent><DateViewType /></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Allocation Hours</Title><FactTable>ResourceUsageSummaryMonthWise</FactTable><GroupByField /><Operator>Sum</Operator><ExpressionFormula /><Order>1</Order><ChartType>Bar</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText>$Exp$</LabelText><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>AllocationHour</FunctionExpression></ChartExpression><ChartExpression><Title>Actual Hours</Title><FactTable>ResourceUsageSummaryMonthWise</FactTable><GroupByField /><Operator>Sum</Operator><ExpressionFormula /><Order>2</Order><ChartType>Bar</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText>$Exp$</LabelText><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>ActualHour</FunctionExpression></ChartExpression></Expressions><FactTable>ResourceUsageSummaryMonthWise</FactTable><BasicDateFitlerStartField /><BasicDateFitlerEndField /><BasicDateFilterDefaultView /><BasicFilter>WorkItemType = 'PMM'</BasicFilter><HideGrid>true</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText>$Exp$</LabelText></panel><Order>1</Order><Title>$Date$ :Top 10 Projects</Title><Description /><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>400</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "$Date$ :Top 10 Projects", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>$Date$ :10 Production Support</ContainerTitle><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>2becc8f9-7f5f-4c10-8e6b-04ddb56c106d</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>4d80fd89-6c5c-4260-897a-113e61ac29bd</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Production Support</Title><SelectedField>WorkItem</SelectedField><Sequence>1</Sequence><PickTopDataPoint>10</PickTopDataPoint><Operator>Sum</Operator><OperatorField>PctActual</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>ResourceUsageSummaryMonthWise</FactTable><FilterID>0</FilterID><DataPointClickEvent>Detail</DataPointClickEvent><DateViewType /></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Allocation Hours</Title><FactTable>ResourceUsageSummaryMonthWise</FactTable><GroupByField /><Operator>Sum</Operator><ExpressionFormula /><Order>1</Order><ChartType>Bar</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText>$Exp$</LabelText><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>AllocationHour</FunctionExpression></ChartExpression><ChartExpression><Title>Actual Hours</Title><FactTable>ResourceUsageSummaryMonthWise</FactTable><GroupByField /><Operator>Sum</Operator><ExpressionFormula /><Order>1</Order><ChartType>Bar</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText>$Exp$</LabelText><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>ActualHour</FunctionExpression></ChartExpression></Expressions><FactTable>ResourceUsageSummaryMonthWise</FactTable><BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField><BasicDateFitlerEndField /><BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView><BasicFilter>WorkItemType = 'Ticketing Support'</BasicFilter><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText>$Exp$</LabelText></panel><Order>2</Order><Title>$Date$ :10 Production Support</Title><Description /><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>400</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "$Date$ :10 Production Support", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>$Date$:% Allocation</ContainerTitle><Description /><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>e6f2018f-190d-4d45-9540-6ae4d93bd26a</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>a69d78c9-98a2-4bf2-a4c8-285907e9dc92</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Level1</Title><SelectedField>WorkItemType</SelectedField><Sequence>1</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>ResourceUsageSummaryMonthWise</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType /></ChartDimension><ChartDimension><Title>Level2</Title><SelectedField>WorkItem</SelectedField><Sequence>2</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>ResourceUsageSummaryMonthWise</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType /></ChartDimension><ChartDimension><Title>Level3</Title><SelectedField>SubWorkItem</SelectedField><Sequence>3</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>ResourceUsageSummaryMonthWise</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType /></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Allocations</Title><FactTable>ResourceUsageSummaryMonthWise</FactTable><GroupByField /><Operator>Sum</Operator><ExpressionFormula /><Order>1</Order><ChartType>Pie</ChartType><ShowInPercentage>true</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText>$Exp$ %</LabelText><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>AllocationHour</FunctionExpression></ChartExpression></Expressions><FactTable>ResourceUsageSummaryMonthWise</FactTable><BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField><BasicDateFitlerEndField /><BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView><BasicFilter /><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>false</HideLabel><LabelStyle>Auto</LabelStyle><LabelText>$Exp$ %</LabelText></panel><Order>3</Order><Title>$Date$:% Allocation</Title><Description /><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>400</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "$Date$:% Allocation", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>$Date$ Actual hours versus allocated</ContainerTitle><Description>Actual hours versus allocated by category and sub-category</Description><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>1d1f3e8e-67c4-4e2d-b510-8114f1d2fd55</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>5027c5b8-2da5-4db1-8443-f1bca4bd02f7</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Category</Title><SelectedField>WorkItemType</SelectedField><Sequence>1</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>ResourceUsageSummaryMonthWise</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType /></ChartDimension><ChartDimension><Title>Sub-Category</Title><SelectedField>WorkItem</SelectedField><Sequence>2</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>ResourceUsageSummaryMonthWise</FactTable><FilterID>0</FilterID><DataPointClickEvent>Detail</DataPointClickEvent><DateViewType /></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Allocation Hours</Title><FactTable>ResourceUsageSummaryMonthWise</FactTable><GroupByField /><Operator>Sum</Operator><ExpressionFormula /><Order>1</Order><ChartType>StepLine</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>PctAllocation</FunctionExpression></ChartExpression><ChartExpression><Title>Actual Hours</Title><FactTable>ResourceUsageSummaryMonthWise</FactTable><GroupByField /><Operator>Sum</Operator><ExpressionFormula /><Order>2</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>ActualHour</FunctionExpression></ChartExpression></Expressions><FactTable>ResourceUsageSummaryMonthWise</FactTable><BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField><BasicDateFitlerEndField /><BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView><BasicFilter /><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>false</HideLabel><LabelStyle /><LabelText /></panel><Order>6</Order><Title>$Date$ Actual hours versus allocated</Title><Description>Actual hours versus allocated by category and sub-category</Description><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>300</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "$Date$ Actual hours versus allocated", dashboard, "1" });

            dashboard = @"<?xml version=""1.0"" encoding=""utf-16""?><UDashboard xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""><ID>0</ID><panel xsi:type=""ChartSetting""><type>Chart</type><ContainerTitle>$Date$ Actual hours versus allocated</ContainerTitle><Description>Actual hours versus allocated by manager/staff</Description><CreatedOn>9999-12-31T23:59:59.9999999</CreatedOn><Order>0</Order><IsShowInSideBar>false</IsShowInSideBar><HeightForSideBar>0</HeightForSideBar><DashboardID>8ff924f8-8064-4ccf-9b45-c19e4d8f78b5</DashboardID><StartFromNewLine>false</StartFromNewLine><HideZoomView>false</HideZoomView><HideTableView>false</HideTableView><HidewDownloadView>false</HidewDownloadView><Id /><ChartId>10610d93-f879-450c-81d2-e6a88bcb50f1</ChartId><IsComulative>false</IsComulative><Dimensions><ChartDimension><Title>Manager</Title><SelectedField>Manager</SelectedField><Sequence>1</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>ResourceUsageSummaryMonthWise</FactTable><FilterID>0</FilterID><DataPointClickEvent>NextDimension</DataPointClickEvent><DateViewType /></ChartDimension><ChartDimension><Title>Resource</Title><SelectedField>Resource</SelectedField><Sequence>2</Sequence><PickTopDataPoint>0</PickTopDataPoint><Operator>Count</Operator><OperatorField>ID</OperatorField><DataPointOrder>Ascending</DataPointOrder><IsCumulative>false</IsCumulative><FactTable>ResourceUsageSummaryMonthWise</FactTable><FilterID>0</FilterID><DataPointClickEvent>Detail</DataPointClickEvent><DateViewType /></ChartDimension></Dimensions><Expressions><ChartExpression><Title>Allocated Hours</Title><FactTable>ResourceUsageSummaryMonthWise</FactTable><GroupByField /><Operator>Sum</Operator><ExpressionFormula /><Order>1</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>AllocationHour</FunctionExpression></ChartExpression><ChartExpression><Title>Actual Hours</Title><FactTable>ResourceUsageSummaryMonthWise</FactTable><GroupByField /><Operator>Sum</Operator><ExpressionFormula /><Order>2</Order><ChartType>Spline</ChartType><ShowInPercentage>false</ShowInPercentage><HideLabel>false</HideLabel><LabelStyle /><LabelText /><YAsixType>Primary</YAsixType><DataPointClickEvent>Inherit</DataPointClickEvent><FunctionExpression>ActualHour</FunctionExpression></ChartExpression></Expressions><FactTable>ResourceUsageSummaryMonthWise</FactTable><BasicDateFitlerStartField>MonthStartDate</BasicDateFitlerStartField><BasicDateFitlerEndField /><BasicDateFilterDefaultView>Previous Month</BasicDateFilterDefaultView><BasicFilter /><HideGrid>false</HideGrid><Palette>None</Palette><HideLegend>false</HideLegend><LegendAlignment>Near</LegendAlignment><LegendDocking>Top</LegendDocking><BorderWidth>1</BorderWidth><HideLabel>false</HideLabel><LabelStyle /><LabelText /></panel><Order>7</Order><Title>$Date$ Actual hours versus allocated</Title><Description>Actual hours versus allocated by manager/staff</Description><PanelType>Chart</PanelType><IsShowInSideBar>false</IsShowInSideBar><Icon /><IsHideTitle>false</IsHideTitle><IsHideDescription>false</IsHideDescription><Width>300</Width><Height>300</Height><SPTheme>Accent1</SPTheme></UDashboard>";
            dataList.Add(new string[] { "$Date$ Actual hours versus allocated", dashboard, "1" });

            foreach (string[] data in dataList)
            {
                ChartTemplate chartTemplates = new ChartTemplate();
                chartTemplates.Title = data[0];
                chartTemplates.ChartObject = data[1];
                chartTemplates.TemplateType = data[2];
                mList.Add(chartTemplates);
            }
            return mList;
        }

        public List<DashboardFactTables> GetDashboardFactTables()
        {
            List<DashboardFactTables> mList = new List<DashboardFactTables>();
            // Console.WriteLine("  DashboardFactTables");

            List<string[]> dataList = new List<string[]>();
            dataList.Add(new string[] { "DashboardSummary", "60", "0", DateTime.Now.ToString("MM/dd/yyyy"), "1", "Scheduled", "All", "" });
            dataList.Add(new string[] { "ResourceUsageSummaryWeekWise", "60", "0", DateTime.Now.ToString("MM/dd/yyyy"), "1", "Scheduled", "All", "" });
            dataList.Add(new string[] { "ResourceUsageSummaryMonthWise", "60", "0", DateTime.Now.ToString("MM/dd/yyyy"), "1", "Scheduled", "All", "" });
            dataList.Add(new string[] { "UserInformation", "60", "0", DateTime.Now.ToString("MM/dd/yyyy"), "1", "Scheduled", "All", "22" });
            dataList.Add(new string[] { "NPRRequest", "60", "0", DateTime.Now.ToString("MM/dd/yyyy"), "1", "Scheduled", "All", "" });
            dataList.Add(new string[] { "PMMProjects", "60", "0", DateTime.Now.ToString("MM/dd/yyyy"), "1", "Scheduled", "All", "" });
            dataList.Add(new string[] { "TSK", "60", "0", DateTime.Now.ToString("MM/dd/yyyy"), "1", "Scheduled", "All", "" });
            dataList.Add(new string[] { "TicketWorkflowSLASummary", "60", "0", DateTime.Now.ToString("MM/dd/yyyy"), "1", "Scheduled", "All", "" });

            foreach (string[] data in dataList)
            {
                DashboardFactTables dashboardFactTables = new DashboardFactTables();
                dashboardFactTables.Title = data[0];
                dashboardFactTables.CacheAfter = Convert.ToInt32(data[1]);       // Minutes
                dashboardFactTables.CacheThreshold = Convert.ToInt32(data[2]);
                dashboardFactTables.ExpiryDate = Convert.ToDateTime(data[3]);
                dashboardFactTables.CacheTable = Convert.ToBoolean(Convert.ToInt32(data[4]));   // Boolean
                dashboardFactTables.CacheMode = data[5];    // On-Demand or Scheduled
                dashboardFactTables.RefreshMode = data[6];  // All or ChangesOnly
                dashboardFactTables.DashboardPanelInfo = data[7];
                mList.Add(dashboardFactTables);
            }
            return mList;
        }

        public List<DashboardSummary> GetDashboardSummary()
        {
            List<DashboardSummary> mList = new List<DashboardSummary>();
            return mList;
        }

        public List<DRQRapidType> GetDRQRapidTypes()
        {
            List<DRQRapidType> mList = new List<DRQRapidType>();
            return mList;
        }

        public List<DRQSystemArea> GetDRQSystemAreas()
        {
            List<DRQSystemArea> mList = new List<DRQSystemArea>();
            return mList;
        }

        public List<ModuleFormTab> GetFormTabs()
        {
            List<ModuleFormTab> mList = new List<ModuleFormTab>();
            return mList;
        }

        public List<ModuleImpact> GetImpact()
        {
            List<ModuleImpact> mList = new List<ModuleImpact>();
            return mList;
        }

        public List<LifeCycleStage> GetLifeCycleStage()
        {
            List<LifeCycleStage> mList = new List<LifeCycleStage>();
            return mList;
        }

        public List<ModuleColumn> GetModuleColumns()
        {
            List<ModuleColumn> mList = new List<ModuleColumn>();
            return mList;
        }

        public List<ModuleDefaultValue> GetModuleDefaultValue()
        {
            List<ModuleDefaultValue> mList = new List<ModuleDefaultValue>();
            return mList;
        }
        public List<ModuleSeverity> GetModuleSeverity()
        {

            List<ModuleSeverity> mList = new List<ModuleSeverity>();
            return mList;
        }
        public List<ModuleUserType> GetModuleUserType()
        {
            List<ModuleUserType> mList = new List<ModuleUserType>();

            return mList;
        }

        public List<ModulePriorityMap> GetModulePriorityMap()
        {
            List<ModulePriorityMap> mList = new List<ModulePriorityMap>();
            return mList;
        }
        public List<ModulePrioirty> GetModulePriority()
        {
            List<ModulePrioirty> mList = new List<ModulePrioirty>();
            return mList;
        }

   
        public List<ModuleEscalationRule> GetModuleEscalationRule()
        {
            List<ModuleEscalationRule> mList = new List<ModuleEscalationRule>();
            // Console.WriteLine("  EscalationRule");

            string escalationTo = "";
            string subject = "Ticket [$TicketId$] escalation";

            List<string[]> dataList = new List<string[]>();

            // TSR Assignment
            string body = "Ticket ID <b>[$TicketId$]</b> has exceeded the SLA for ticket assignment and needs your attention.";
            dataList.Add(new string[] { "1", "240", "TicketOwner;#RequestTypeEscalationManager", "1440", escalationTo, body, "Description", subject });
            dataList.Add(new string[] { "2", "480", "TicketOwner;#RequestTypeEscalationManager", "1440", escalationTo, body, "Description", subject });
            dataList.Add(new string[] { "3", "480", "TicketOwner;#RequestTypeEscalationManager", "1440", escalationTo, body, "Description", subject });

            // TSR Resolution
            body = "Ticket ID <b>[$TicketId$]</b> has exceeded the SLA for ticket resolution and needs your attention.";
            dataList.Add(new string[] { "4", "240", "TicketOwner;#TicketPRP;#RequestTypeEscalationManager", "1440", escalationTo, body, "Description", subject });
            dataList.Add(new string[] { "5", "240", "TicketOwner;#TicketPRP;#RequestTypeEscalationManager", "1440", escalationTo, body, "Description", subject });
            dataList.Add(new string[] { "6", "480", "TicketOwner;#TicketPRP;#RequestTypeEscalationManager", "1440", escalationTo, body, "Description", subject });

            //SLARules = web.Lists["SLARule.Items;


            foreach (string[] data in dataList)
            {
                ModuleEscalationRule mEscalationRule = new ModuleEscalationRule();
                int SLARuleID = Convert.ToInt32(data[0]);
                //mEscalationRule.Title = Convert.ToString(SLARules[SLARuleID - 1]["Title"]);
                mEscalationRule.SLARuleIdLookup = Convert.ToInt32(data[0]);

                mEscalationRule.EscalationMinutes = Convert.ToInt32(data[1]);

                mEscalationRule.EscalationToRoles = data[2];

                mEscalationRule.EscalationFrequency = Convert.ToInt32(data[3]);

                mEscalationRule.EscalationToEmails = data[4];

                mEscalationRule.EscalationEmailBody = data[5];

                mEscalationRule.EscalationDescription = data[6];

                mEscalationRule.EscalationMailSubject = data[7];
                mList.Add(mEscalationRule);
            }
            return mList;
        }

        public List<ModuleFormLayout> GetModuleFormLayout()
        {
            List<ModuleFormLayout> mList = new List<ModuleFormLayout>();
            return mList;
        }

        public List<ModuleRequestType> GetModuleRequestType()
        {
            List<ModuleRequestType> mList = new List<ModuleRequestType>();
            return mList;
        }

        public List<ModuleRoleWriteAccess> GetModuleRoleWriteAccess()
        {
            List<ModuleRoleWriteAccess> mList = new List<ModuleRoleWriteAccess>();
            return mList;
        }

        public List<ModuleSLARule> GetModuleSLARule()
        {

            List<ModuleSLARule> mList = new List<ModuleSLARule>();
            // Console.WriteLine("  SLARule");

            List<string[]> dataList = new List<string[]>();

            // TSR
            //string moduleId = "2";
            string ModuleName = "TSR";
            string startStageID = (++currStageID).ToString();
            moduleStartStages.Add(ModuleName + "-" + GlobalVar.TenantID, startStageID);
            string startStage = moduleStartStages[ModuleName + "-" + GlobalVar.TenantID].ToString();
            string assignedStageID = currStageID.ToString();
            moduleAssignedStages.Add(ModuleName + "-" + GlobalVar.TenantID, assignedStageID);
            string assignedStage = moduleAssignedStages[ModuleName + "-" + GlobalVar.TenantID].ToString();
            moduleResolvedStages.Add(ModuleName + "-" + GlobalVar.TenantID, currStageID);
            string resolutionStage = moduleResolvedStages[ModuleName + "-" + GlobalVar.TenantID].ToString();

            dataList.Add(new string[] { "Assignment", ModuleName, "4", "2", "No-RoundOff", "100", startStage, assignedStage,
                                            "TSR Assignment SLA for High-Priority Tickets (2 hrs)" });
            dataList.Add(new string[] { "Assignment", ModuleName, "5", "4", "No-RoundOff", "100", startStage, assignedStage,
                                            "TSR Assignment SLA for Medium-Priority Tickets (4 hrs)" });
            dataList.Add(new string[] { "Assignment", ModuleName, "6", "8", "No-RoundOff", "100", startStage, assignedStage,
                                            "TSR Assignment SLA for Low-Priority Tickets (8 hrs)" });

            dataList.Add(new string[] { "Resolution", ModuleName, "4", "8", "No-RoundOff", "100", startStage, resolutionStage,
                                            "TSR Resolution SLA for High-Priority Tickets (1 day)" });
            dataList.Add(new string[] { "Resolution", ModuleName, "5", "16", "No-RoundOff", "100", startStage, resolutionStage,
                                            "TSR Resolution SLA for Medium-Priority Tickets (2 days)" });
            dataList.Add(new string[] { "Resolution", ModuleName, "6", "56", "No-RoundOff", "100", startStage, resolutionStage,
                                            "TSR Resolution SLA for Low-Priority Tickets (7 days)" });


            // Lookup lists for constructing title in format: "PRS - Assignment Time - High"
            //SPList modules = web.Lists["Modules;

            //SPList priorities = web.Lists["TicketPriority;

            foreach (string[] data in dataList)
            {
                ModuleSLARule mSLARule = new ModuleSLARule();
                //priorityItem = priorities.GetItemById(int.Parse(data[2]));

                // mSLARule.Title = data[0] + " - " + priorityItem["Title'];
                mSLARule.SLACategoryChoice = data[0];

                mSLARule.ModuleNameLookup = (data[1]);

                mSLARule.PriorityLookup = Convert.ToInt32(data[2]);

                mSLARule.SLAHours = Convert.ToInt32(data[3]);

                mSLARule.SLADaysRoundUpDownChoice = data[4];

                mSLARule.SLATarget = Convert.ToInt32(data[5]);

                mSLARule.StageTitleLookup = Convert.ToInt32(data[6]);

                mSLARule.EndStageTitleLookup = Convert.ToInt32(data[7]);

                mSLARule.ModuleDescription = data[8];
                mList.Add(mSLARule);
            }
            return mList;
        }

        public List<ModuleStatusMapping> GetModuleStatusMapping()
        {
            List<ModuleStatusMapping> mList = new List<ModuleStatusMapping>();
            return mList;
        }

        public List<ModuleTaskEmail> GetModuleTaskEmail()
        {
            List<ModuleTaskEmail> mList = new List<ModuleTaskEmail>();
            return mList;
        }

        public List<TabView> UpdateTabView()
        {
            List<TabView> tabs = new List<TabView>();
            return tabs;
        }
        public void GetPriorityMapping(ref List<ModuleImpact> impacts, ref List<ModuleSeverity> serverities, ref List<ModulePrioirty> priorities, ref List<ModulePriorityMap> mapping)
        {

        }
        public List<ModulePriorityMap> requestPriorities(List<ModuleImpact> impacts, List<ModuleSeverity> serverities, List<ModulePrioirty> priorities)
        {
            List<ModulePriorityMap> rp = new List<ModulePriorityMap>();
            return rp;
        }
    }
}


using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;
using uGovernIT.Utility.Entities;
using uGovernIT.Web.ControlTemplates;

namespace uGovernIT.Web
{
    public partial class RelatedTicketPicker : UserControl
    {
        public string Module { get; set; }
        public string ParentTicketId { get; set; }
        public string Type { get; set; }
        public string selectedTicketId { get; set; }
        public string ParentModule { get; set; }
        public string ControlId { get; set; }
        public string Title { get; set; }
        public string RequestType { get; set; }
        public string CurrentModulePagePath { get; set; }
        public string TabId = "4";
        public bool LookAhead { get; set; }
        public int AssetId { get; set; }
        public DataTable FilteredTable { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        TicketRelationshipHelper RelationshipHelper = null;
        ModuleViewManager ObjModuleViewManager = null;

        protected override void OnInit(EventArgs e)
        {
            RelationshipHelper = new TicketRelationshipHelper(context);
            ObjModuleViewManager = new ModuleViewManager(context);

            if (Type == "TicketRelation")
            {
                helpCardListPicker.Visible = false;
                wikiListPicker.Visible = false;
                lstPicker.Visible = true;
                ListPicker customListPicker = ((ListPicker)lstPicker);
                customListPicker.ExcludedTickets = new List<string>();
                customListPicker.EnableModuleDropdown = true;
                customListPicker.Module = Module;
                customListPicker.IsMultiSelect = true;
                customListPicker.LookAhead = LookAhead;
                customListPicker.Title = Title;
                customListPicker.RequestType = RequestType;

                BtnSaveLink.Style.Add("display", "block");

                if (!string.IsNullOrEmpty(Module) && !string.IsNullOrEmpty(ParentTicketId))
                {
                    customListPicker.ExcludedTickets = RelationshipHelper.GetParentTickets(Module, ParentTicketId);
                }
            }
            else if (Type == "ServiceSubTicket")
            {
                helpCardListPicker.Visible = false;
                wikiListPicker.Visible = false;
                lstPicker.Visible = true;
                ListPicker customListPicker = ((ListPicker)lstPicker);
                customListPicker.ExcludedTickets = new List<string>();
                customListPicker.EnableModuleDropdown = true;
                customListPicker.Module = Module;
                customListPicker.IsMultiSelect = true;
                BtnSaveLink.Style.Add("display", "block");
                if (!string.IsNullOrEmpty(Module) && !string.IsNullOrEmpty(ParentTicketId))
                {
                    customListPicker.ExcludedTickets = RelationshipHelper.GetDependentTickets(Module, ParentTicketId);
                }
            }
            else if (Type == "RelatedService" || Type == "ScheduleAddTicket" || Type == "PMMRelateTicket" || Type == "RelatedRequestTicket")
            {
                helpCardListPicker.Visible = false;
                wikiListPicker.Visible = false;
                lstPicker.Visible = true;
                ListPicker customListPicker = ((ListPicker)lstPicker);
                customListPicker.ExcludedTickets = new List<string>();
                customListPicker.EnableModuleDropdown = true;
                customListPicker.Module = Module;
                if (!string.IsNullOrEmpty(Module) && !string.IsNullOrEmpty(ParentTicketId))
                {
                    customListPicker.ExcludedTickets = RelationshipHelper.GetDependentTickets(Module, ParentTicketId);
                }
            }
            else if (Type == "FormLayout")
            {
                helpCardListPicker.Visible = false;
                wikiListPicker.Visible = true;
                lstPicker.Visible = false;
                WikiListPicker customWikiListPicker = ((WikiListPicker)wikiListPicker);
                customWikiListPicker.ExcludedTickets = new List<string>();
                customWikiListPicker.EnableModuleDropdown = true;
                customWikiListPicker.ExcludedTickets = GetExcludedTickets();
                //customWikiListPicker.SelectedModule = Module;
                customWikiListPicker.Module = ModuleNames.WIKI;
                customWikiListPicker.ParentTicketId = ParentTicketId;
            }
            //Added by mudassir 19 march 2020
            else if (Type == "AssetRelatedWithTickets")
            {
                wikiListPicker.Visible = false;
                helpCardListPicker.Visible = false;
                lstPicker.Visible = true;
                ListPicker customListPicker = ((ListPicker)lstPicker);
                customListPicker.ExcludedTickets = new List<string>();
                customListPicker.EnableModuleDropdown = true;
                customListPicker.Module = Module;
                customListPicker.IsMultiSelect = true;

                BtnSaveLink.Style.Add("display", "block");
                if (!string.IsNullOrEmpty(Module) && !string.IsNullOrEmpty(ParentTicketId))
                {
                    customListPicker.ExcludedTickets = RelationshipHelper.GetParentTickets(Module, ParentTicketId);
                }
            }
            else if (Type == "AssetRelatedWithAssets")
            {
                wikiListPicker.Visible = false;
                lstPicker.Visible = true;
                ListPicker customListPicker = ((ListPicker)lstPicker);
                customListPicker.ExcludedTickets = new List<string>();
                customListPicker.EnableModuleDropdown = false;
                customListPicker.Module = Module;
                customListPicker.IsMultiSelect = true;
                customListPicker.Type = Type;
                BtnSaveLink.Style.Add("display", "block");
                helpCardListPicker.Visible = false;
                if (!string.IsNullOrEmpty(Module) && !string.IsNullOrEmpty(ParentTicketId))
                {
                    customListPicker.ExcludedTickets = RelationshipHelper.GetParentTickets(Module, ParentTicketId);
                }
            }
            //
            else if (Type == "EmptyTitle")
            {
                helpCardListPicker.Visible = false;
                wikiListPicker.Visible = false;
                lstPicker.Visible = true;
            }
            else if (Type == "HelpCardList")
            {

                helpCardListPicker.Visible = true;
                wikiListPicker.Visible = false;
                lstPicker.Visible = false;
                HelpCardListPicker customHelpCardListPicker = ((HelpCardListPicker)helpCardListPicker);
                customHelpCardListPicker.ExcludedTickets = new List<string>();
                customHelpCardListPicker.ExcludedTickets = GetExcludedTicketsHelpCard();
                customHelpCardListPicker.Module = "HLP";
                customHelpCardListPicker.ParentTicketId = ParentTicketId;
            }
            else
            {
                helpCardListPicker.Visible = false;
                wikiListPicker.Visible = true;
                lstPicker.Visible = false;
                WikiListPicker customWikiListPicker = ((WikiListPicker)wikiListPicker);
                customWikiListPicker.ExcludedTickets = new List<string>();
                customWikiListPicker.EnableModuleDropdown = true;
                customWikiListPicker.ExcludedTickets = GetExcludedTickets();
                customWikiListPicker.Module = ModuleNames.WIKI;
                customWikiListPicker.ParentTicketId = ParentTicketId;
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Convert.ToString(Request["TabId"]) != null)
            {
                TabId = Convert.ToString(Request["TabId"]);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            List<string> selectedVals;
            //Added by mudassir 19 march 2020
            if (Type == "AssetRelatedWithAssets" || Type == "AssetRelatedWithTickets" || Type == "RelatedService" || Type == "TicketRelation" || Type == "ScheduleAddTicket" || Type == "PMMRelateTicket" || Type == "ServiceSubTicket" || Type == "RelatedRequestTicket")
            {
                ListPicker customListPicker = ((ListPicker)lstPicker);
                selectedVals = customListPicker.selectedTicketIds;
            }
            else if (Type == "HelpCardList")
            {
                HelpCardListPicker customHelpCardListPicker = ((HelpCardListPicker)helpCardListPicker);
                selectedVals = customHelpCardListPicker.selectedTicketIds;
            }
            else
            {
                WikiListPicker customWikiListPicker = ((WikiListPicker)wikiListPicker);
                selectedVals = customWikiListPicker.selectedTicketIds;
            }

            if (selectedVals == null)
                return;

            if (LookAhead && Type == "TicketRelation")
            {
                HttpContext.Current.Session.Add("relatedTicket", selectedVals);
                uHelper.ClosePopUpAndEndResponse(Context, false);
                return;
            }

            switch (Type)
            {
                //Added by mudassir 19 march 2020
                case "AssetRelatedWithAssets":
                    {
                        foreach (string item in selectedVals)
                        {
                            TicketRelationshipHelper tRelation = new TicketRelationshipHelper(context, ParentTicketId, item);
                            int rowEffected = tRelation.CreateRelation(context);


                        }
                    }
                    break;
                case "AssetRelatedWithTickets":
                    {
                        AssetsManger assetsManger = new AssetsManger(context);
                        foreach (string item in selectedVals)
                        {
                            if (!string.IsNullOrEmpty(Request.QueryString["AssetId"]))
                                AssetId = Convert.ToInt32(Request.QueryString["AssetId"]);
                                assetsManger.CreateRelationWithIncident(Convert.ToString(AssetId), item);
                                assetsManger.CreateAssetHistory(AssetId, item);
                        }
                    }
                    break;
                case "RelatedService":
                    foreach (string val in selectedVals)
                    {
                        //ModuleInstanceDependency moduleInstance = new ModuleInstanceDependency();
                        //moduleInstance.ParentInstance = val;
                        //moduleInstance.ChildInstance = ParentTicketId;
                        DataRow splistitem = Ticket.GetCurrentTicket(context, Module, val);
                        if (splistitem != null)//&& splistitem.Count() > 0)
                        {
                            //moduleInstance.Title = Convert.ToString(splistitem[DatabaseObjects.Columns.Title]);
                        }

                        UGITModule dataRow = ObjModuleViewManager.GetByName(ModuleNames.WIKI);
                        if (dataRow != null)
                        {
                            //SPFieldLookupValue spFieldLookupModule = new SPFieldLookupValue(Convert.ToInt32(dataRow[DatabaseObjects.Columns.Id]), Convert.ToString(dataRow[DatabaseObjects.Columns.ModuleName]));
                            //moduleInstance. = spFieldLookupModule;
                        }
                        // bool isSave = moduleInstance.Save();
                        //DataTable dtRelations = moduleInstance.LoadTableByChildTicketId(ParentTicketId);
                        //if (dtRelations != null && dtRelations.Rows.Count > 0)
                        //{
                        //SPList wikiArticlesSPList = SPListHelper.GetSPList(DatabaseObjects.Lists.WikiArticles);
                        //SPQuery query = new SPQuery();
                        //query.Query = "<Where><Eq><FieldRef Name=\"TicketId\" /><Value Type=\"Text\">" + ParentTicketId + "</Value></Eq></Where>";
                        //query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.WikiServiceRequestCount);
                        //int requestCount = dtRelations.Rows.Count;
                        //SPListItemCollection spCollArticles = wikiArticlesSPList.GetItems(query);
                        //SPListItem spListItemArticle = spCollArticles[0];
                        //spListItemArticle[DatabaseObjects.Columns.WikiServiceRequestCount] = requestCount;
                        //spListItemArticle.Update();
                        // }
                    }
                    break;

                case "TicketWiki":

                    foreach (string val in selectedVals)
                    {
                        string title = string.Empty;

                        DataRow splistitem = Ticket.GetCurrentTicket(context, ParentModule, ParentTicketId);
                        if (splistitem != null)
                        {
                            title = Convert.ToString(splistitem[DatabaseObjects.Columns.Title]);
                        }
                        //ModuleInstanceDependency moduleInstance = new ModuleInstanceDependency();
                        //moduleInstance.ParentInstance = ParentTicketId.Trim();
                        //moduleInstance.ChildInstance = val;
                        //moduleInstance.Title = title;

                        UGITModule dataRow = ObjModuleViewManager.GetByName(ModuleNames.WIKI);
                        if (dataRow != null)
                        {
                            //SPFieldLookupValue spFieldLookupModule = new SPFieldLookupValue(Convert.ToInt32(dataRow[DatabaseObjects.Columns.Id]), Convert.ToString(dataRow[DatabaseObjects.Columns.ModuleName]));
                            //moduleInstance.Module = spFieldLookupModule;
                        }
                        //bool isSave = moduleInstance.Save(SPContext.Current.Web);
                        //UpdateRelatedServiceCount(Convert.ToString(val));
                    }
                    break;

                case "TicketRelation":
                    foreach (string val in selectedVals)
                    {
                        if (Module != null && Module.Trim() != string.Empty && !string.IsNullOrEmpty(val))
                        {
                            TicketRelationshipHelper tRelation = new TicketRelationshipHelper(context, ParentTicketId, val);
                            int rowEffected = tRelation.CreateRelation(context);
                        }
                    }
                    // if(LookAhead)btnRelatedTitle////CL
                    //{
                    //string newModuleTicketTitle = string.Format("{0}: {1}", ParentTicketId, UGITUtility.ReplaceInvalidCharsInURL(UGITUtility.TruncateWithEllipsis(Convert.ToString(Title), 100, string.Empty)));
                    //string newModuleTicketLink = string.Format("<a href=\"javascript:\" id='myAnchor' onclick=\"PopupCloseBeforeOpenUgit('{1}','ticketId={0}','{2}', 90, 90, 0, '{3}' );  \">{0}</a>",ParentTicketId, CurrentModulePagePath, newModuleTicketTitle, Request["source"]);
                    //string typeName = UGITUtility.newTicketTitle(Module);
                    //string informationMsg = string.Format("<div class='informationMsg'><span>{0} Created: <b>{1}</b></span></div>",
                    //                                        typeName, newModuleTicketLink);
                    //string header = string.Format("{0} Created", typeName);
                    //TicketRequest.UpdateTicketCache(currentTicket, currentModuleName); Need to ask 
                    //uHelper.InformationPopup(Context, Server.UrlEncode(informationMsg), header);
                    //uHelper.ClosePopUpAndEndResponse(Context, false, Convert.ToString(Request.Url));



                    break;

                case "ServiceSubTicket":
                    if (string.IsNullOrEmpty(ParentModule))
                    {
                        ParentModule = uHelper.getModuleNameByTicketId(ParentTicketId);
                    }

                    if (ParentModule == "SVC")
                    {
                        UGITTaskManager taskManager = new UGITTaskManager(context);
                        List<UGITTask> instDependencies = taskManager.LoadByProjectID(ParentTicketId); // ModuleInstanceDependency.LoadByPublicID(SPContext.Current.Web, ParentTicketId);

                        foreach (string val in selectedVals)
                        {
                            if (Module != null && Module.Trim() != string.Empty && !string.IsNullOrEmpty(val))
                            {
                                DataRow splistitem = Ticket.GetCurrentTicket(context, Module, val.Trim());
                                if (splistitem != null)
                                {

                                    UGITTask moduleInstance = new UGITTask();
                                    moduleInstance.ParentInstance = ParentTicketId.Trim();
                                    moduleInstance.ChildInstance = val;
                                    moduleInstance.Level = 0;
                                    moduleInstance.ParentTaskID = 0;
                                    moduleInstance.RelatedModule = uHelper.getModuleNameByTicketId(val);
                                    moduleInstance.RelatedTicketID = val;
                                    moduleInstance.DueDate = new DateTime(1800, 1, 1);
                                    moduleInstance.StartDate = new DateTime(1800, 1, 1);
                                    moduleInstance.TicketId = ParentTicketId;
                                    moduleInstance.ItemOrder = 1;
                                    if (instDependencies.Count > 0)
                                        moduleInstance.ItemOrder = instDependencies[instDependencies.Count - 1].ItemOrder + 1;

                                    moduleInstance.ModuleNameLookup = uHelper.getModuleNameByTicketId(ParentTicketId);

                                    string name = Convert.ToString(splistitem[DatabaseObjects.Columns.TicketStageActionUsers]);
                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        string[] assignees = UGITUtility.SplitString(name, Constants.Separator6);

                                        //moduleInstance.AssignedTo = new SPFieldUserValueCollection();
                                        foreach (string assignee in assignees)
                                        {

                                            UserProfile spUser = HttpContext.Current.GetUserManager().GetUserById(assignee);     //  UserProfile.GetUserByName(assignee);
                                            if (spUser != null)
                                            {
                                                string userId = Convert.ToString(spUser.Id);
                                                moduleInstance.AssignedTo += Constants.Separator6 + userId;  //moduleInstance.AssignedTo.Add(new SPFieldUserValue(SPContext.Current.Web, userId));
                                            }
                                        }
                                    }

                                    moduleInstance.Behaviour = "Ticket";
                                    moduleInstance.Status = Convert.ToString(splistitem[DatabaseObjects.Columns.TicketStatus]);
                                    moduleInstance.Title = Convert.ToString(splistitem[DatabaseObjects.Columns.Title]);

                                    taskManager.Save(moduleInstance);

                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            if (Type == "WikiHelp" || Type == "FormLayout")
            {
                ClosePopUpAndReturnValue(selectedVals[0]);
            }
            else if (Type == "ScheduleAddTicket" || Type == "PMMRelateTicket" || Type == "RelatedRequestTicket")
            {
                ClosePopUpAndReturnValue(selectedVals[0], Type);
            }
            else if (Type == "HelpCardList")
            {
                ClosePopUpAndReturnValue(selectedVals[0], Type);
            }
            else
            {
                // Setting Selected TabId, and storing it in session, so that Related Ticket Tab should be focussed when Picker List is closed (after Relating to Parent Ticket).
                Session["selectedTab"] = TabId;
                uHelper.ClosePopUpAndEndResponse(Context, true);
            }
        }

        private void ClosePopUpAndReturnValue(string retValue)
        {
            ClosePopUpAndReturnValue(retValue, "");
        }

        private void ClosePopUpAndReturnValue(string retValue, string Type)
        {
            string sourceUrl = string.Empty;
            if (Context.Request["source"] != null && Context.Request["source"].Trim() != string.Empty)
            {
                sourceUrl = Context.Request["source"].Trim();
            }

            Dictionary<string, string> dict = new Dictionary<string, string>();
            //Adding viewControlId to dictionary to set value on a hyperlink for DRQ Related Request ID in case of Edit/New ticket 
            string viewControlId = string.Empty;
            if (!string.IsNullOrEmpty(this.ControlId))
            {
                string[] strMultipleControlId = null;
                strMultipleControlId = UGITUtility.SplitString(this.ControlId, Constants.Separator1);
                if (strMultipleControlId.Length > 1)
                {
                    this.ControlId = strMultipleControlId[0];
                    viewControlId = strMultipleControlId[1];
                }
            }
            dict.Add("ViewControlId", viewControlId);
            dict.Add("ControlId", this.ControlId);
            if (Type == "ScheduleAddTicket" || Type == "RelatedRequestTicket")
            {
                string url = Ticket.GenerateTicketURL(context, Module, retValue, true);
                DataRow item = Ticket.GetCurrentTicket(context, Module, retValue);
                if (item != null)
                    dict.Add("ScheduleTickeTitle", UGITUtility.Truncate(UGITUtility.ObjectToString(item[DatabaseObjects.Columns.Title]), 20));

                dict.Add("ScheduleTicketUrl", url);
                dict.Add("ScheduleTicketId", retValue);

                if (Type == "RelatedRequestTicket")
                {
                    HyperLink viewTicket = new HyperLink();
                    int moduleId = uHelper.getModuleIdByTicketID(context, retValue);
                    viewTicket = uHelper.GetHyperLinkControlForTicketID(context, moduleId, retValue);

                    string viewTicketDetails = string.Format("{0};#{1};#{2}", viewTicket.ToolTip, UGITUtility.TruncateWithEllipsis(viewTicket.ToolTip, 20),
                        HttpUtility.UrlEncode(viewTicket.NavigateUrl));

                    dict.Add("ViewTicketDetails", viewTicketDetails);
                }
            }
            else if (Type == "PMMRelateTicket")
            {
                DataRow ticketInfo = Ticket.GetCurrentTicket(context, Module, retValue);
                string url = Ticket.GenerateTicketURL(context, Module, retValue, true);
                dict.Add("TicketUrl", url);
                dict.Add("TicketInfo", string.Format("{0}: {1}", retValue, ticketInfo[DatabaseObjects.Columns.Title]));
            }
            else if (Type == "HelpCardList")
            {
                dict.Add("HelpCardId", retValue);
            }
            else
            {
                dict.Add("WikiId", retValue);
            }
            dict.Add("frameUrl", sourceUrl);
            var vals = UGITUtility.GetJsonForDictionary(dict);

            uHelper.ClosePopUpAndEndResponse(Context, false, vals);
        }

        private List<string> GetExcludedTickets()
        {
            List<string> selectedTicket = new List<string>();
            // bool isOwner = false;
            //bool isCreator = false;
            DataTable dtWikiTickets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.WikiArticles, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");    //SPListHelper.GetDataTable(DatabaseObjects.Lists.WikiArticles);
            if (dtWikiTickets != null && dtWikiTickets.Rows.Count > 0)
            {
                //isOwner = WikiArticleHelper.IsWikiOwner();
                // isCreator = WikiArticleHelper.GetPermissions("add", "");
                //DataTable dt = UGITTaskManager.LoadTableByPublicID(ParentTicketId);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    DataRow[] dr = dt.Select(string.Format("ModuleName= '{0}'", "WIK"));
                //    if (dr != null && dr.Length > 0)
                //    {
                //        dt = dr.CopyToDataTable();

                //        string ticketIds = string.Empty;
                //        foreach (DataRow row in dt.Rows)
                //        {
                //            ticketIds = ticketIds + "'" + Convert.ToString(row[DatabaseObjects.Columns.ChildTicketId]) + "',";
                //            selectedTicket.Add(Convert.ToString(row[DatabaseObjects.Columns.ChildTicketId]));
                //        }
                //    }
                //}
            }

            return selectedTicket;
        }


        private List<string> GetExcludedTicketsHelpCard()
        {
            List<string> selectedTicket = new List<string>();
            // bool isOwner = false;
            //bool isCreator = false;
            DataTable dtHelpCardTickets = GetTableDataManager.GetTableData(DatabaseObjects.Tables.HelpCard, $"{DatabaseObjects.Columns.TenantID}='{context.TenantID}'");    //SPListHelper.GetDataTable(DatabaseObjects.Lists.WikiArticles);
            if (dtHelpCardTickets != null && dtHelpCardTickets.Rows.Count > 0)
            {
                //isOwner = WikiArticleHelper.IsWikiOwner();
                // isCreator = WikiArticleHelper.GetPermissions("add", "");
                //DataTable dt = UGITTaskManager.LoadTableByPublicID(ParentTicketId);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    DataRow[] dr = dt.Select(string.Format("ModuleName= '{0}'", "WIK"));
                //    if (dr != null && dr.Length > 0)
                //    {
                //        dt = dr.CopyToDataTable();

                //        string ticketIds = string.Empty;
                //        foreach (DataRow row in dt.Rows)
                //        {
                //            ticketIds = ticketIds + "'" + Convert.ToString(row[DatabaseObjects.Columns.ChildTicketId]) + "',";
                //            selectedTicket.Add(Convert.ToString(row[DatabaseObjects.Columns.ChildTicketId]));
                //        }
                //    }
                //}
            }

            return selectedTicket;
        }

        private void UpdateRelatedServiceCount(string TicketID)
        {
            //    ModuleInstanceDependency moduleInstance = new ModuleInstanceDependency();
            //    DataTable dtRelations = moduleInstance.LoadTableByChildTicketId(TicketID);
            //    SPList wikiArticlesSPList = SPListHelper.GetSPList(DatabaseObjects.Lists.WikiArticles);
            //    if (wikiArticlesSPList != null && wikiArticlesSPList.ItemCount > 0)
            //    {
            //        SPQuery query = new SPQuery();
            //        query.Query = "<Where><Eq><FieldRef Name=\"TicketId\" /><Value Type=\"Text\">" + TicketID + "</Value></Eq></Where>";
            //        query.ViewFields = string.Format("<FieldRef Name='{0}'/><FieldRef Name='{1}'/>", DatabaseObjects.Columns.Id, DatabaseObjects.Columns.WikiServiceRequestCount);
            //        SPListItemCollection spCollArticles = wikiArticlesSPList.GetItems(query);
            //        if (spCollArticles != null && spCollArticles.Count > 0)
            //        {
            //            SPListItem spListItemArticle = spCollArticles[0];
            //            if (dtRelations != null && dtRelations.Rows.Count > 0)
            //                spListItemArticle[DatabaseObjects.Columns.WikiServiceRequestCount] = dtRelations.Rows.Count;
            //            else
            //                spListItemArticle[DatabaseObjects.Columns.WikiServiceRequestCount] = 0;
            //            spListItemArticle.Update();
            //        }
            //    }
        }

    }
}

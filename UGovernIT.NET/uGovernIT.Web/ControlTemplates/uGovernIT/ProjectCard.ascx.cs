using DevExpress.Web;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ProjectCard : UserControl
    {
        public string TicketIds { get; set; }

        private string module = string.Empty;
        private string companyLogo = string.Empty;
        private ApplicationContext _context = null;

        private ModuleViewManager _moduleViewManager = null;

        private TicketManager _ticketManager = null;
        private FieldConfigurationManager _fieldConfigurationManager = null;

        ApplicationContext context = HttpContext.Current.GetManagerContext();
        private ConfigurationVariableManager _configurationVariableManager = null;

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

        protected ModuleViewManager ModuleViewManager
        {
            get
            {
                if (_moduleViewManager == null)
                {
                    _moduleViewManager = new ModuleViewManager(ApplicationContext);
                }
                return _moduleViewManager;
            }
        }

        protected TicketManager TicketManager
        {
            get
            {
                if (_ticketManager == null)
                {
                    _ticketManager = new TicketManager(ApplicationContext);
                }
                return _ticketManager;
            }
        }

        protected FieldConfigurationManager FieldConfigurationManager
        {
            get
            {
                if (_fieldConfigurationManager == null)
                {
                    _fieldConfigurationManager = new FieldConfigurationManager(ApplicationContext);
                }
                return _fieldConfigurationManager;
            }
        }

        protected ConfigurationVariableManager ConfigurationVariableManager
        {
            get
            {
                if (_configurationVariableManager == null)
                {
                    _configurationVariableManager = new ConfigurationVariableManager(ApplicationContext);

                }
                return _configurationVariableManager;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            string[] values = TicketIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (!IsPostBack)
            {
                chkCancelled.Checked = true;
                chkClosed.Checked = true;
                chkConCPR.Checked = true;
                chkLead.Checked = true;
                chkLead.InputAttributes["class"] = "card-chkbox";
                chkLost.Checked = true;
                chkPiplelineCPROpenOMP.Checked = true;
                chkClientContracts.Checked = true;
            }

            if (values != null && values.Length > 0)
            {
                module = uHelper.getModuleNameByTicketId(values[0]);
            }

            #region Generate Cardview columns


            CardViewImageColumn ccol1 = new CardViewImageColumn();
            ccol1.FieldName = DatabaseObjects.Columns.DocIcon;
            ccol1.Caption = DatabaseObjects.Columns.DocIcon;
            ccol1.PropertiesImage.ImageWidth = 60;
            projectCardView.Columns.Add(ccol1);

            CardViewColumn ccol = new CardViewColumn();
            ccol.FieldName = DatabaseObjects.Columns.Title;
            ccol.Caption = DatabaseObjects.Columns.Name;
            projectCardView.Columns.Add(ccol);

            ccol = new CardViewColumn();
            ccol.FieldName = DatabaseObjects.Columns.CRMCompanyLookup;
            ccol.Caption = "Company";
            projectCardView.Columns.Add(ccol);
            int rowSpan = 5;

            if (module == "CON")
            {
                rowSpan = 7;
                ccol = new CardViewColumn();
                ccol.FieldName = "Contact";
                ccol.Caption = "Contact";
                projectCardView.Columns.Add(ccol);

                ccol = new CardViewColumn();
                ccol.FieldName = "ContactRole";
                ccol.Caption = "Role";
                projectCardView.Columns.Add(ccol);
                projectCardView.Styles.FlowCard.Height = Unit.Pixel(145);
            }

            ccol = new CardViewColumn();
            ccol.FieldName = DatabaseObjects.Columns.ApproxContractValue;
            ccol.Caption = "Contract Value";
            projectCardView.Columns.Add(ccol);

            ccol = new CardViewColumn();
            ccol.FieldName = DatabaseObjects.Columns.EstimatedConstructionStart;
            ccol.Caption = "Est. Const. Start";
            projectCardView.Columns.Add(ccol);

            projectCardView.CardLayoutProperties.ColCount = 2;
            CardViewColumnLayoutItem lItem = new CardViewColumnLayoutItem();
            lItem.ColumnName = "DocIcon";
            lItem.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
            lItem.RowSpan = rowSpan;
            lItem.CssClass = "docIcon";
            lItem.Width = new Unit(25, UnitType.Percentage);
            projectCardView.CardLayoutProperties.Items.AddColumnItem(lItem);

            lItem = new CardViewColumnLayoutItem();
            lItem.ColumnName = "Title";
            lItem.ShowCaption = DevExpress.Utils.DefaultBoolean.True;
            lItem.Width = new Unit(75, UnitType.Percentage);
            projectCardView.CardLayoutProperties.Items.AddColumnItem(lItem);
            lItem = new CardViewColumnLayoutItem();
            lItem.ColumnName = DatabaseObjects.Columns.CRMCompanyLookup; //"CRMCompanyLookup";
            lItem.ShowCaption = DevExpress.Utils.DefaultBoolean.True;
            projectCardView.CardLayoutProperties.Items.AddColumnItem(lItem);

            if (module == "CON")
            {
                lItem = new CardViewColumnLayoutItem();
                lItem.ColumnName = "Contact";
                lItem.ShowCaption = DevExpress.Utils.DefaultBoolean.True;
                projectCardView.CardLayoutProperties.Items.AddColumnItem(lItem);
                lItem = new CardViewColumnLayoutItem();
                lItem.ColumnName = "ContactRole";
                lItem.ShowCaption = DevExpress.Utils.DefaultBoolean.True;
                projectCardView.CardLayoutProperties.Items.AddColumnItem(lItem);
            }

            lItem = new CardViewColumnLayoutItem();
            lItem.ColumnName = "ApproxContractValue";
            lItem.ShowCaption = DevExpress.Utils.DefaultBoolean.True;
            projectCardView.CardLayoutProperties.Items.AddColumnItem(lItem);
            lItem = new CardViewColumnLayoutItem();
            lItem.ColumnName = "EstimatedConstructionStart";
            lItem.ShowCaption = DevExpress.Utils.DefaultBoolean.True;
            projectCardView.CardLayoutProperties.Items.AddColumnItem(lItem);



            #endregion

            #region Generate Gridview columns



            GridViewDataTextColumn colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.Title;
            colId.Caption = DatabaseObjects.Columns.Name;
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(150);
            grdProject.Columns.Add(colId);


            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.CRMCompanyLookup;
            colId.Caption = "Company";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(90);
            grdProject.Columns.Add(colId);






            if (module == "CON")
            {
                colId = new GridViewDataTextColumn();
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.FieldName = "Contact";
                colId.Caption = "Contact";
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                //colId.Width = Unit.Pixel(100);
                grdProject.Columns.Add(colId);


                colId = new GridViewDataTextColumn();
                colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                colId.PropertiesTextEdit.EncodeHtml = false;
                colId.FieldName = "ContactRole";
                colId.Caption = "Role";
                colId.HeaderStyle.Font.Bold = true;
                colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
                colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
                //colId.Width = Unit.Pixel(100);
                grdProject.Columns.Add(colId);


            }

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Right;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.ApproxContractValue;
            colId.Caption = "Contract Value";
            colId.PropertiesEdit.DisplayFormatString = "{0:C0}";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(100);
            grdProject.Columns.Add(colId);


            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.EstimatedConstructionStart;
            colId.Caption = "Const. Start";
            colId.HeaderStyle.Font.Bold = true;
            colId.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(50);
            grdProject.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.EstimatedConstructionEnd;
            colId.Caption = "Const. End";
            colId.HeaderStyle.Font.Bold = true;
            colId.PropertiesEdit.DisplayFormatString = "{0:MMM-dd-yyyy}";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(50);
            grdProject.Columns.Add(colId);


            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.UsableSqFt;
            colId.Caption = "Net Rt. Sq Ft";
            colId.HeaderStyle.Font.Bold = true;
            colId.PropertiesEdit.DisplayFormatString = "{0:N0}";
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(50);
            grdProject.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Left;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.StreetAddress1;
            colId.Caption = "Street";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(150);
            grdProject.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.City;
            colId.Caption = DatabaseObjects.Columns.City;
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(100);
            grdProject.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.StateLookup;
            colId.Caption = "State";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(70);
            grdProject.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.Zip;
            colId.Caption = DatabaseObjects.Columns.Zip;
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(20);
            grdProject.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.Module;
            colId.Caption = DatabaseObjects.Columns.Module;
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(30);
            grdProject.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.DivisionLookup;
            colId.Caption = "Business Unit";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(30);
            grdProject.Columns.Add(colId);

            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.TicketStatus;
            colId.Caption = "Stage";
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(30);
            grdProject.Columns.Add(colId);



            colId = new GridViewDataTextColumn();
            colId.CellStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            colId.PropertiesTextEdit.EncodeHtml = false;
            colId.FieldName = DatabaseObjects.Columns.Status;
            colId.Caption = DatabaseObjects.Columns.Status;
            colId.HeaderStyle.Font.Bold = true;
            colId.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.Default;
            colId.SettingsHeaderFilter.Mode = GridHeaderFilterMode.CheckedList;
            //colId.Width = Unit.Pixel(30);
            grdProject.Columns.Add(colId);





            #endregion

            base.OnInit(e);
        }

        private DataTable GetProjectByCompany()
        {
            //String[] values = TicketIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            DataTable dtResult = new DataTable();

           

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Title))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMCompanyLookup))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.CRMCompanyLookup, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ApproxContractValue))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.ApproxContractValue, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionStart))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionStart, typeof(DateTime));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionEnd))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionEnd, typeof(DateTime));
            }

            if (!dtResult.Columns.Contains("ContactRole"))
            {
                dtResult.Columns.Add("ContactRole", typeof(string));
            }

            if (!dtResult.Columns.Contains("Contact"))
            {
                dtResult.Columns.Add("Contact", typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.StageStep))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.StageStep, typeof(int));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketStatus))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.TicketStatus, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketId))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMProjectStatus))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.CRMProjectStatus, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMOpportunityStatus))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.CRMOpportunityStatus, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.LeadStatus))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.LeadStatus, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Module))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.Module, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.UsableSqFt))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.UsableSqFt, typeof(double));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.StreetAddress1))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.StreetAddress1, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.City))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.City, typeof(string));
            }

            //if (!dtResult.Columns.Contains(DatabaseObjects.Columns.UGITStateLookup))
            //{
            //    dtResult.Columns.Add(DatabaseObjects.Columns.UGITStateLookup, typeof(string));
            //}

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.StateLookup))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.StateLookup, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Zip))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.Zip, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Status))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.Status, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ReasonType))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.ReasonType, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.DivisionLookup, typeof(string));
            }

            #region Related Companies filter

            // SPQuery spQuery = new SPQuery();
            //string qry = "<Where><In><FieldRef Name=\"CRMCompanyTitleLookup\"  /><Values>";
            string filterExpression = string.Empty;
            string LookupID = string.Empty;
            /*
            List<string> valueLookUp = new List<string>();

            foreach (string val in values)
            {
                if (!string.IsNullOrEmpty(val))
                {
                    var spListItem = Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(val), val);
                    if (spListItem != null)
                    {
                        // qry += string.Format("<Value Type=\"Lookup\">{0}</Value>", Convert.ToString(spListItem[DatabaseObjects.Columns.Title]));
                        valueLookUp.Add(Convert.ToString(spListItem[DatabaseObjects.Columns.Id]));

                    }
                }            
            }

            LookupID = string.Join(",", valueLookUp);
            */

            LookupID = string.Join(",", TicketIds.Split(',').Select(x => $"'{x}'").ToList()); 
            filterExpression = $"{DatabaseObjects.Columns.CRMCompanyLookup} in({LookupID}) and {DatabaseObjects.Columns.TenantID }= '{context.TenantID}' ";
            List<string> lstTicketIds = TicketIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //qry += "</Values></In></Where>";
            // spQuery.Query = qry;
            // DataTable dtRelatedCompaniesData = SPListHelper.GetDataTable(DatabaseObjects.Lists.RelatedCompanies, spQuery);
            //  var dtRelatedCompaniesData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RelatedCompanies, $"{DatabaseObjects.Columns.CRMCompanyTitleLookup} in'{spListItem[DatabaseObjects.Columns.Title]}'");
            var dtRelatedCompaniesData = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RelatedCompanies, filterExpression);

            if ((dtRelatedCompaniesData != null && dtRelatedCompaniesData.Rows.Count > 0) || (lstTicketIds.Count > 0))
            {
                List<string> lstDistinctTickets = new List<string>();
                DataTable dtDistinct = dtRelatedCompaniesData.AsDataView().ToTable(true, new string[] { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.RelationshipTypeLookup, DatabaseObjects.Columns.CRMCompanyLookup });

                if (dtDistinct != null && dtDistinct.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDistinct.Rows)
                    {
                        lstDistinctTickets.Add(Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));
                    } 
                }

                DataTable dtCPRs = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.CPR));
                DataTable dtCNSs = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.CNS));
                DataTable dtOPMs = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.OPM));
                DataTable dtLEMs = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.LEM));

                var data = dtCPRs.AsEnumerable().Where(x => lstDistinctTickets.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId)) || lstTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.CRMCompanyLookup)));

                DataTable drsCPR = null;
                if (data != null && data.AsDataView().Count > 0)
                {
                    drsCPR = data.CopyToDataTable();
                    // If application is using BusinessUnitLookup instead CRMBusinessUnit
                    if (drsCPR != null && drsCPR.Columns.Contains(DatabaseObjects.Columns.BusinessUnitLookup) && !drsCPR.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                    {
                        drsCPR.Columns[DatabaseObjects.Columns.BusinessUnitLookup].ColumnName = DatabaseObjects.Columns.DivisionLookup;
                    }
                }

                if (dtResult == null)
                {
                    dtResult = drsCPR;
                }
                else if (drsCPR != null)
                {
                    dtResult.Merge(drsCPR, false, MissingSchemaAction.Ignore);
                }

                data = dtCNSs.AsEnumerable().Where(x => lstDistinctTickets.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId)) || lstTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.CRMCompanyLookup)));

                DataTable drsCNS = null;
                if (data != null && data.AsDataView().Count > 0)
                {
                    drsCNS = data.CopyToDataTable();
                    // If application is using BusinessUnitLookup instead CRMBusinessUnit
                    if (drsCNS != null && drsCNS.Columns.Contains(DatabaseObjects.Columns.BusinessUnitLookup) && !drsCNS.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                    {
                        drsCNS.Columns[DatabaseObjects.Columns.BusinessUnitLookup].ColumnName = DatabaseObjects.Columns.DivisionLookup;
                    }
                }

                if (dtResult == null)
                {
                    dtResult = drsCNS;
                }
                else if (drsCNS != null)
                {
                    dtResult.Merge(drsCNS, false, MissingSchemaAction.Ignore);
                }

                data = dtOPMs.AsEnumerable().Where(x => lstDistinctTickets.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId)) || lstTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.CRMCompanyLookup)));
                DataTable drsOPM = null;
                if (data != null && data.AsDataView().Count > 0)
                {
                    drsOPM = data.CopyToDataTable();
                    // If application is using BusinessUnitLookup instead CRMBusinessUnit
                    if (drsOPM != null && drsOPM.Columns.Contains(DatabaseObjects.Columns.BusinessUnitLookup) && !drsOPM.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                    {
                        drsOPM.Columns[DatabaseObjects.Columns.BusinessUnitLookup].ColumnName = DatabaseObjects.Columns.DivisionLookup;
                    }
                }

                if (dtResult == null)
                {
                    dtResult = drsCPR;
                }
                else if (drsOPM != null)
                {
                    dtResult.Merge(drsOPM, false, MissingSchemaAction.Ignore);
                }

                data = dtLEMs.AsEnumerable().Where(x => lstDistinctTickets.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId)) || lstTicketIds.Contains(x.Field<string>(DatabaseObjects.Columns.CRMCompanyLookup)));
                DataTable drsLEM = null;
                if (data != null && data.AsDataView().Count > 0)
                {
                    drsLEM = data.CopyToDataTable();
                    // If application is using BusinessUnitLookup instead CRMBusinessUnit
                    if (drsLEM != null && drsLEM.Columns.Contains(DatabaseObjects.Columns.BusinessUnitLookup) && !drsLEM.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                    {
                        drsLEM.Columns[DatabaseObjects.Columns.BusinessUnitLookup].ColumnName = DatabaseObjects.Columns.DivisionLookup;
                    }
                }

                if (dtResult == null)
                {
                    dtResult = drsLEM;
                }
                else if (drsLEM != null)
                {
                    dtResult.Merge(drsLEM, false, MissingSchemaAction.Ignore);
                }

            }

            #endregion




            if (dtResult != null)
            {
                string colName = "CardType";
                if (!dtResult.Columns.Contains(colName))
                {
                    dtResult.Columns.Add(colName, typeof(int));
                }

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Module))
                {
                    dtResult.Columns.Add(DatabaseObjects.Columns.Module, typeof(string));
                }


                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Status))
                {
                    dtResult.Columns.Add(DatabaseObjects.Columns.Status, typeof(string));
                }




                DataTable dtTemp = dtResult.Clone();

                foreach (DataRow dr in dtResult.Rows)
                {
                    int step = Convert.ToString(dr[DatabaseObjects.Columns.StageStep]) == "" ? 0 : Convert.ToInt16(dr[DatabaseObjects.Columns.StageStep]);
                    string tStatus = Convert.ToString(dr[DatabaseObjects.Columns.TicketStatus]);
                    string ticketID = Convert.ToString(dr[DatabaseObjects.Columns.TicketId]);

                    if (!string.IsNullOrEmpty(ticketID))
                    {
                        string module = uHelper.getModuleNameByTicketId(ticketID);

                        string status = string.Empty;

                        if (module == "CPR")
                            status = Convert.ToString(dr[DatabaseObjects.Columns.CRMProjectStatus]);
                        else if (module == "OPM")
                            status = Convert.ToString(dr[DatabaseObjects.Columns.CRMOpportunityStatus]);
                        else if (module == "LEM")
                            status = Convert.ToString(dr[DatabaseObjects.Columns.ReasonType]);
                        else
                            status = Convert.ToString(dr[DatabaseObjects.Columns.TicketStatus]);


                        dr[DatabaseObjects.Columns.Module] = module;
                        dr[DatabaseObjects.Columns.Status] = status;
                        if (UGITUtility.ObjectToString(cmbViewType.Value) == "Card View")
                        {
                            if (chkLead.Checked && module == "LEM")
                            {
                                dr[colName] = 1;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkClientContracts.Checked && module == "CCM")
                            {
                                dr[colName] = 7;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkLost.Checked && ((module == "CPR" && status == "Lost") || (module == "OPM" && status == "Lost")))
                            {
                                dr[colName] = 6;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkCancelled.Checked && (tStatus == "Cancelled" || tStatus == "Cancel"))
                            {
                                dr[colName] = 5;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkPiplelineCPROpenOMP.Checked && ((module == "CPR" && step < 7) || (module == "OPM" && tStatus != "Closed")))
                            {
                                dr[colName] = 2;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkConCPR.Checked && (module == "CPR" && (step >= 7 && step <= 10)))
                            {
                                dr[colName] = 3;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkClosed.Checked && tStatus == "Closed")
                            {
                                dr[colName] = 4;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                        }
                        else
                        {
                            dtTemp.Rows.Add(dr.ItemArray);
                        }
                    }
                }


                DataView dView = dtTemp.AsDataView();
                string sortBy = UGITUtility.GetCookie(Request, "SortBy") != null ? UGITUtility.GetCookieValue(Request, "SortBy") : string.Empty;

                string controlName = uHelper.GetPostBackControlId(this.Page);

                if (controlName == "imgbtnToggleView")
                {

                    if (sortBy == "Title")
                    {
                        UGITUtility.CreateCookie(Response, "SortBy", "Color");
                        dView.Sort = string.Format("{0} ASC", "CardType");
                    }
                    else
                    {
                        UGITUtility.CreateCookie(Response, "SortBy", "Title");
                        dView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
                    }
                }
                else
                {
                    if (sortBy == "Title")
                    {
                        dView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
                    }
                    else
                    {
                        dView.Sort = string.Format("{0} ASC", "CardType");
                    }
                }

                dtResult = dView.ToTable();
            }

            if (dtResult != null && !dtResult.Columns.Contains(DatabaseObjects.Columns.DocIcon))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.DocIcon, typeof(string), "'/content/images/company-avatar.jpg'");

            }


            var temp = dtResult.DefaultView.ToTable();
            foreach (DataRow row in temp.Rows)
            {
                foreach (DataColumn dc in temp.Columns)
                {
                    string val = Convert.ToString(row[dc]);
                    //if (!string.IsNullOrEmpty(val) && val.Contains(Constants.Separator))
                    //{
                    //    row[dc] = UGITUtility.RemoveIDsFromLookupString(val);
                    //}

                    if (FieldConfigurationManager.GetFieldByFieldName(dc.ColumnName) != null && val !=null && val!="")
                    {
                        string value = FieldConfigurationManager.GetFieldConfigurationData(dc.ColumnName, val);
                        row[dc] = value;
                    }

                    //if (Convert.ToString(fmanger.GetFieldByFieldName(dc.ColumnName)) == "Comment")
                    //{
                    //    string value = fmanger.GetFieldConfigurationData(dc.ColumnName, val);

                    //    row[dc] = value.Split(";#");

                    //}


                }
            }
            dtResult = temp;
            return dtResult;
        }

        private DataTable GetProjectByContact()
        {

            //String[] values = TicketIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            DataTable dtResult = new DataTable();

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Title))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.Title, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMCompanyLookup))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.CRMCompanyLookup, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ApproxContractValue))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.ApproxContractValue, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionStart))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionStart, typeof(DateTime));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionEnd))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.EstimatedConstructionEnd, typeof(DateTime));
            }

            if (!dtResult.Columns.Contains("ContactRole"))
            {
                dtResult.Columns.Add("ContactRole", typeof(string));  //, "'Broker'"
            }

            if (!dtResult.Columns.Contains("Contact"))
            {
                dtResult.Columns.Add("Contact", typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.StageStep))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.StageStep, typeof(int));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketStatus))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.TicketStatus, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.TicketId))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.TicketId, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMProjectStatus))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.CRMProjectStatus, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.CRMOpportunityStatus))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.CRMOpportunityStatus, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.LeadStatus))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.LeadStatus, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Module))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.Module, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.UsableSqFt))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.UsableSqFt, typeof(double));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.StreetAddress1))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.StreetAddress1, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.City))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.City, typeof(string));
            }

            //if (!dtResult.Columns.Contains(DatabaseObjects.Columns.UGITStateLookup))
            //{
            //    dtResult.Columns.Add(DatabaseObjects.Columns.UGITStateLookup, typeof(string));
            //}

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.StateLookup))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.StateLookup, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Zip))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.Zip, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Status))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.Status, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.ReasonType))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.ReasonType, typeof(string));
            }

            if (!dtResult.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.DivisionLookup, typeof(string));
            }

            if (!dtResult.Columns.Contains("ContactRole"))
            {
                dtResult.Columns.Add("ContactRole", typeof(string));
            }

            if (!dtResult.Columns.Contains("Contact"))
            {
                dtResult.Columns.Add("Contact", typeof(string));
            }

            if (dtResult != null && !dtResult.Columns.Contains(DatabaseObjects.Columns.DocIcon))
            {
                dtResult.Columns.Add(DatabaseObjects.Columns.DocIcon, typeof(string), "'/content/images/company-avatar.jpg'");

            }

            #region Related Companies filter

            // SPQuery spQuery = new SPQuery();
            //string qry = "<Where><In><FieldRef Name=\"ContactLookup\"  /><Values>";
            //string filterExpression = string.Empty;

            //string LookupID ="";
            /*
            foreach (string val in values)
            {
                if (!string.IsNullOrEmpty(val))
                {
                    // SPListItem spListItem = Ticket.getCurrentTicket(uHelper.getModuleNameByTicketId(val), val);
                    var spListItem = Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(val), val);
                    if (spListItem != null)
                    {
                        //qry += string.Format("<Value Type=\"LookupMulti\">{0}</Value>", Convert.ToString(spListItem[DatabaseObjects.Columns.Title]));
                        LookupID += "'" + Convert.ToString(spListItem[DatabaseObjects.Columns.Id]) + "',";

                        // LookupID.Add(Convert.ToString(spListItem[DatabaseObjects.Columns.Id]));

                    }
                }

            }
            LookupID = LookupID.Length > 0 ? LookupID.Substring(0, LookupID.Length - 1) : LookupID;
            */
            //TicketIds
            /*
            LookupID = string.Join(",", TicketIds.Split(',').Select(x => $"'{x}'").ToList());
            if (LookupID != null )
                filterExpression = $"{DatabaseObjects.Columns.ContactLookup} in ({ LookupID}) and {DatabaseObjects.Columns.TenantID} = '{ApplicationContext.TenantID}'";
            else
                filterExpression = $"{DatabaseObjects.Columns.TenantID} = '{ApplicationContext.TenantID}'";
            */

            List<string> lstTicketIds = TicketIds.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            DataTable dtRelatedCompanies = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RelatedCompanies, $"{DatabaseObjects.Columns.TenantID} = '{ApplicationContext.TenantID}'");
            DataTable dtRelatedCompaniesData = new DataView(dtRelatedCompanies, "1=2", null, DataViewRowState.CurrentRows).ToTable();
            DataRow[] drRelatedContact = null;
            foreach (var item in lstTicketIds)
            {
                drRelatedContact = dtRelatedCompanies.Select($"{DatabaseObjects.Columns.ContactLookup} like '%{item}%'");
                if (drRelatedContact.Length > 0)
                    dtRelatedCompaniesData.Merge(drRelatedContact.CopyToDataTable(), false, MissingSchemaAction.Ignore);
            }

            //if ((dtRelatedCompaniesData != null && dtRelatedCompaniesData.Rows.Count > 0) || (lstTicketIds.Count > 0))
            //{
                List<string> lstDistinctTickets = new List<string>();
                DataTable dtDistinct = dtRelatedCompaniesData.AsDataView().ToTable(true, new string[] { DatabaseObjects.Columns.TicketId, DatabaseObjects.Columns.RelationshipTypeLookup, DatabaseObjects.Columns.ContactLookup });

                if (dtDistinct != null && dtDistinct.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtDistinct.Rows)
                    {
                        lstDistinctTickets.Add(Convert.ToString(dr[DatabaseObjects.Columns.TicketId]));
                    }
                }

                DataTable dtCPRs = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.CPR));
                DataTable dtCNSs = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.CNS));
                DataTable dtOPMs = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.OPM));
                DataTable dtLEMs = TicketManager.GetAllTickets(ModuleViewManager.GetByName(ModuleNames.LEM));

                var data = dtCPRs.AsEnumerable().Where(x => lstDistinctTickets.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId)));

                DataTable drsCPR = null;
                if (data != null && data.AsDataView().Count > 0)
                {
                    drsCPR = data.CopyToDataTable();
                    // If application is using BusinessUnitLookup instead CRMBusinessUnit
                    if (drsCPR != null && drsCPR.Columns.Contains(DatabaseObjects.Columns.BusinessUnitLookup) && !drsCPR.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                    {
                        drsCPR.Columns[DatabaseObjects.Columns.BusinessUnitLookup].ColumnName = DatabaseObjects.Columns.DivisionLookup;
                    }
                }

                if (dtResult == null)
                {
                    dtResult = drsCPR;
                }
                else if (drsCPR != null)
                {
                    dtResult.Merge(drsCPR, false, MissingSchemaAction.Ignore);
                }

                data = dtCNSs.AsEnumerable().Where(x => lstDistinctTickets.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId)));

                DataTable drsCNS = null;
                if (data != null && data.AsDataView().Count > 0)
                {
                    drsCNS = data.CopyToDataTable();
                    // If application is using BusinessUnitLookup instead CRMBusinessUnit
                    if (drsCNS != null && drsCNS.Columns.Contains(DatabaseObjects.Columns.BusinessUnitLookup) && !drsCNS.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                    {
                        drsCNS.Columns[DatabaseObjects.Columns.BusinessUnitLookup].ColumnName = DatabaseObjects.Columns.DivisionLookup;
                    }
                }

                if (dtResult == null)
                {
                    dtResult = drsCNS;
                }
                else if (drsCNS != null)
                {
                    dtResult.Merge(drsCNS, false, MissingSchemaAction.Ignore);
                }

                data = dtOPMs.AsEnumerable().Where(x => lstDistinctTickets.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId)));
                DataTable drsOPM = null;
                if (data != null && data.AsDataView().Count > 0)
                {
                    drsOPM = data.CopyToDataTable();
                    // If application is using BusinessUnitLookup instead CRMBusinessUnit
                    if (drsOPM != null && drsOPM.Columns.Contains(DatabaseObjects.Columns.BusinessUnitLookup) && !drsOPM.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                    {
                        drsOPM.Columns[DatabaseObjects.Columns.BusinessUnitLookup].ColumnName = DatabaseObjects.Columns.DivisionLookup;
                    }
                }

                if (dtResult == null)
                {
                    dtResult = drsCPR;
                }
                else if (drsOPM != null)
                {
                    dtResult.Merge(drsOPM, false, MissingSchemaAction.Ignore);
                }

                data = dtLEMs.AsEnumerable().Where(x => lstDistinctTickets.Contains(x.Field<string>(DatabaseObjects.Columns.TicketId)));
                DataTable drsLEM = null;
                if (data != null && data.AsDataView().Count > 0)
                {
                    drsLEM = data.CopyToDataTable();
                    // If application is using BusinessUnitLookup instead CRMBusinessUnit
                    if (drsLEM != null && drsLEM.Columns.Contains(DatabaseObjects.Columns.BusinessUnitLookup) && !drsLEM.Columns.Contains(DatabaseObjects.Columns.DivisionLookup))
                    {
                        drsLEM.Columns[DatabaseObjects.Columns.BusinessUnitLookup].ColumnName = DatabaseObjects.Columns.DivisionLookup;
                    }
                }

                if (dtResult == null)
                {
                    dtResult = drsLEM;
                }
                else if (drsLEM != null)
                {
                    dtResult.Merge(drsLEM, false, MissingSchemaAction.Ignore);
                }

            //}


            DataRow[] drContact = null;
            foreach (var item in lstTicketIds)
            {
                drContact = dtCPRs.Select($"{DatabaseObjects.Columns.ContactLookup} like '%{item}%'");
                if (drContact.Length > 0)
                    dtResult.Merge(drContact.CopyToDataTable(), false, MissingSchemaAction.Ignore);

                drContact = dtCNSs.Select($"{DatabaseObjects.Columns.ContactLookup} like '%{item}%'");
                if (drContact.Length > 0)
                    dtResult.Merge(drContact.CopyToDataTable(), false, MissingSchemaAction.Ignore);

                drContact = dtOPMs.Select($"{DatabaseObjects.Columns.ContactLookup} like '%{item}%'");
                if (drContact.Length > 0)
                    dtResult.Merge(drContact.CopyToDataTable(), false, MissingSchemaAction.Ignore);

                drContact = dtLEMs.Select($"{DatabaseObjects.Columns.ContactLookup} like '%{item}%'");
                if (drContact.Length > 0)
                    dtResult.Merge(drContact.CopyToDataTable(), false, MissingSchemaAction.Ignore);
            }

            #endregion

            if (dtResult != null)
            {

                string colName = "CardType";
                if (!dtResult.Columns.Contains(colName))
                {
                    dtResult.Columns.Add(colName, typeof(int));
                }

                if (!dtResult.Columns.Contains(DatabaseObjects.Columns.Module))
                {
                    dtResult.Columns.Add(DatabaseObjects.Columns.Module, typeof(string));
                }

                DataTable dtTemp = dtResult.Clone();

                foreach (DataRow dr in dtResult.Rows)
                {
                    int step = Convert.ToInt16(dr[DatabaseObjects.Columns.StageStep]);
                    string tStatus = Convert.ToString(dr[DatabaseObjects.Columns.TicketStatus]);
                    string ticketID = Convert.ToString(dr[DatabaseObjects.Columns.TicketId]);

                    if (!string.IsNullOrEmpty(ticketID))
                    {
                        DataRow[] drs = dtRelatedCompaniesData.Select(string.Format("{0} = '{1}'", DatabaseObjects.Columns.TicketId, ticketID));
                        if (drs != null && drs.Length > 0)
                        {
                            string contacts = Convert.ToString(drs[0][DatabaseObjects.Columns.ContactLookup]);
                            if (!string.IsNullOrEmpty(contacts))
                            {
                                /*
                                string[] vals = uHelper.GetMultiLookupValue(contacts);
                                if (vals.Length > 0)
                                    dr["Contact"] = string.Join(", ", vals);
                                */
                                dr["Contact"] = FieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.ContactLookup, contacts);
                            }

                            if (drs[0][DatabaseObjects.Columns.RelationshipTypeLookup] != DBNull.Value)
                            {
                                dr["ContactRole"] = FieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.RelationshipTypeLookup, Convert.ToString(drs[0][DatabaseObjects.Columns.RelationshipTypeLookup]));
                            }
                            else
                            dr["ContactRole"] = "";
                        }
                        else
                        {
                            DataRow drTicket =  Ticket.GetCurrentTicket(ApplicationContext, uHelper.getModuleNameByTicketId(ticketID), ticketID);
                            if (drTicket != null)
                            {
                                string contacts = Convert.ToString(drTicket[DatabaseObjects.Columns.ContactLookup]);
                                if (!string.IsNullOrEmpty(contacts))
                                {
                                    dr["Contact"] = FieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.ContactLookup, contacts);
                                }

                                if (UGITUtility.IsSPItemExist(drTicket, DatabaseObjects.Columns.RelationshipTypeLookup))
                                {
                                    dr["ContactRole"] = FieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.RelationshipTypeLookup, Convert.ToString(drTicket[DatabaseObjects.Columns.RelationshipTypeLookup]));
                                }
                                else
                                    dr["ContactRole"] = "";
                            }
                        }

                        string module = uHelper.getModuleNameByTicketId(ticketID);

                        string status = string.Empty;
                        if (module == "CPR" || module == "CNS")
                            status = Convert.ToString(dr[DatabaseObjects.Columns.CRMProjectStatus]);
                        else if (module == "OPM")
                            status = Convert.ToString(dr[DatabaseObjects.Columns.CRMOpportunityStatus]);
                        else if (module == "LEM")
                            status = Convert.ToString(dr[DatabaseObjects.Columns.ReasonType]);
                        else
                            status = Convert.ToString(dr[DatabaseObjects.Columns.TicketStatus]);


                        dr[DatabaseObjects.Columns.Status] = status;

                        dr[DatabaseObjects.Columns.Module] = module;
                        if (UGITUtility.ObjectToString(cmbViewType.Value) == "Card View")
                        {
                            if (chkLead.Checked && module == "LEM")
                            {
                                dr[colName] = 1;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            if (chkClientContracts.Checked && module == "CCM")
                            {
                                dr[colName] = 7;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkLost.Checked && (((module == "CPR" || module == "CNS") && status == "Lost") || (module == "OPM" && status == "Lost")))
                            {
                                dr[colName] = 6;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkCancelled.Checked && (tStatus == "Cancelled" || tStatus == "Cancel"))
                            {
                                dr[colName] = 5;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkPiplelineCPROpenOMP.Checked && (((module == "CPR" || module == "CNS") && step < 7) || (module == "OPM" && tStatus != "Closed")))
                            {
                                dr[colName] = 2;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkConCPR.Checked && (module == "CPR" || module == "CNS") && (step >= 7 && step <= 10))
                            {
                                dr[colName] = 3;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                            else if (chkClosed.Checked && tStatus == "Closed")
                            {
                                dr[colName] = 4;
                                dtTemp.Rows.Add(dr.ItemArray);
                            }
                        }
                        else
                        {
                            dtTemp.Rows.Add(dr.ItemArray);
                        }
                    }
                }

                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {

                    DataView dView = dtTemp.AsDataView();
                    string sortBy = UGITUtility.GetCookie(Request, "SortBy") != null ? UGITUtility.GetCookieValue(Request, "SortBy") : string.Empty;

                    string controlName = uHelper.GetPostBackControlId(this.Page);

                    if (controlName == "imgbtnToggleView")
                    {

                        if (sortBy == "Title")
                        {
                            UGITUtility.CreateCookie(Response, "SortBy", "Color");
                            dView.Sort = string.Format("{0} ASC", "CardType");
                        }
                        else
                        {
                            UGITUtility.CreateCookie(Response, "SortBy", "Title");
                            dView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
                        }
                    }
                    else
                    {
                        if (sortBy == "Title")
                        {
                            dView.Sort = string.Format("{0} ASC", DatabaseObjects.Columns.Title);
                        }
                        else
                        {
                            dView.Sort = string.Format("{0} ASC", "CardType");
                        }
                    }
                    dtResult = dView.ToTable();
                }
                else
                {
                    dtResult = new DataTable();
                }
            }
            var temp = dtResult.DefaultView.ToTable();
            foreach (DataRow row in temp.Rows)
            {
                foreach (DataColumn dc in temp.Columns)
                {
                    string val = Convert.ToString(row[dc]);
                   
                    if (FieldConfigurationManager.GetFieldByFieldName(dc.ColumnName) != null && val != null && val != "")
                    {
                        string value = FieldConfigurationManager.GetFieldConfigurationData(dc.ColumnName, val);
                        row[dc] = value;
                    }

                    
                }
            }
            dtResult = temp;


            return dtResult;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            companyLogo = ConfigurationVariableManager.GetValue(ConfigConstants.ReportLogo);
            DataTable dtResult = module == "COM" ? GetProjectByCompany() : GetProjectByContact();
            grdProject.DataSource = dtResult;
            grdProject.DataBind();
            projectCardView.DataSource = dtResult;
            projectCardView.DataBind();
        }

        protected void projectCardView_HtmlCardPrepared(object sender, ASPxCardViewHtmlCardPreparedEventArgs e)
        {

            string tStatus = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketStatus));
            string ticketID = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketId));
            string title = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));

            if (!string.IsNullOrEmpty(ticketID))
            {
                string module = uHelper.getModuleNameByTicketId(ticketID);

                string status = string.Empty;
                if (module == "CPR")
                    status = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.CRMProjectStatus));
                else if (module == "OPM")
                    status = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.CRMOpportunityStatus));
                else if (module == "CCM")
                    status = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketStatus));
                else
                    status = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.LeadStatus));


                int step = module == "LEM" ? 0 : Convert.ToInt16(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.StageStep));



                e.Card.BorderWidth = Unit.Pixel(2);
                if (module == "LEM")
                {
                    e.Card.BorderColor = Color.Black;
                }
                else if (module == "CCM")
                {
                    e.Card.BorderColor = Color.Orange;
                }
                else if ((module == "CPR" && status == "Lost") || (module == "OPM" && status == "Lost"))
                {
                    e.Card.BorderColor = Color.MediumVioletRed; // System.Drawing.ColorTranslator.FromHtml("#800020");                 
                }
                else if (tStatus == "Cancelled" || tStatus == "Cancel")
                {
                    e.Card.BorderColor = Color.Teal;
                }
                else if ((module == "CPR" && step < 7) || (module == "OPM" && tStatus != "Closed"))
                {
                    e.Card.BorderColor = Color.MediumSeaGreen;
                }
                else if (module == "CPR" && (step >= 7 && step <= 10))
                {
                    e.Card.BorderColor = Color.RoyalBlue;
                }


                // DataRow moduleDetail = uGITCache.GetModuleDetails(module);
                var moduleDetail = ModuleViewManager.GetByName(module);

                e.Card.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 90, 90, 0, '{3}')", UGITUtility.GetAbsoluteURL(moduleDetail.StaticModulePagePath), string.Format("TicketId={0}", ticketID), title, Server.UrlEncode(Request.Url.AbsolutePath)));
                e.Card.Style.Add("cursor", "pointer");

            }

        }

        protected void projectCardView_CustomColumnDisplayText(object sender, ASPxCardViewColumnDisplayTextEventArgs e)
        {
            decimal currencyValue;
            if (Decimal.TryParse(e.Value.ToString(), out currencyValue))
            {
                e.DisplayText = string.Format("{0:c0}", currencyValue);
            }
        }

        protected void grdProject_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName.Replace(" ", "") != DatabaseObjects.Columns.ApproxContractValue || e.Value == null) return;

            var contractValue = Convert.ToString(e.Value);
            if (string.IsNullOrEmpty(contractValue)) return;

            if (decimal.TryParse(contractValue, out var dContractValue))
                e.DisplayText = string.Format("{0:C}", dContractValue);
        }

        protected void imgbtnToggleView_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void chk_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtResult = module == "COM" ? GetProjectByCompany() : GetProjectByContact();
            projectCardView.DataSource = dtResult;
            projectCardView.DataBind();
            grdProject.DataSource = dtResult;
            grdProject.DataBind();

        }

        protected void cmbViewType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void CardViewExporter_RenderBrick(object sender, ASPxCardViewExportRenderingEventArgs e)
        {

        }

        protected void imgExportToPDF_Click(object sender, ImageClickEventArgs e)
        {
            if (Convert.ToString(cmbViewType.Value) == "Card View")
            {
                CardViewExporter.WritePdfToResponse();
            }
            else
            {

                // ReportEntity.ReportGenerationHelper reportHelper = new ReportEntity.ReportGenerationHelper();
                ReportGenerationHelper reportHelper = new ReportGenerationHelper();
                reportHelper.companyLogo = companyLogo;
                ReportQueryFormat qFormat = new ReportQueryFormat();
                qFormat.ShowDateInFooter = true;
                qFormat.ShowCompanyLogo = true;


                XtraReport report = reportHelper.GenerateReport(grdProject, (DataTable)grdProject.DataSource, "Project View", 6.75F, "pdf", null, qFormat);
                reportHelper.WritePdfToResponse(Response, "ProjectView" + ".pdf", System.Net.Mime.DispositionTypeNames.Attachment.ToString());

            }
        }

        protected void imgExportToExcel_Click(object sender, ImageClickEventArgs e)
        {
            if (Convert.ToString(cmbViewType.Value) == "Card View")
            {
                XlsExportOptionsEx options = new XlsExportOptionsEx(TextExportMode.Text);
                options.ExportType = DevExpress.Export.ExportType.WYSIWYG;
                options.ShowGridLines = true;

                CardViewExporter.WriteXlsToResponse();
            }
            else
            {
                ReportGenerationHelper reportHelper = new ReportGenerationHelper();
                reportHelper.companyLogo = companyLogo;
                ReportQueryFormat qFormat = new ReportQueryFormat();
                qFormat.ShowDateInFooter = true;
                qFormat.ShowCompanyLogo = true;

                XtraReport report = reportHelper.GenerateReport(grdProject, (DataTable)grdProject.DataSource, "Project View", 8F, "xls", null, qFormat);
                reportHelper.WriteXlsToResponse(Response, "ProjectView" + ".xls", System.Net.Mime.DispositionTypeNames.Attachment.ToString());
            }
        }

        protected void grdProject_HtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            //string tStatus = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketStatus));
            string ticketID = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.TicketId));
            string title = Convert.ToString(projectCardView.GetCardValues(e.VisibleIndex, DatabaseObjects.Columns.Title));

            if (!string.IsNullOrEmpty(ticketID))
            {
                string module = uHelper.getModuleNameByTicketId(ticketID);

                // DataRow moduleDetail = uGITCache.GetModuleDetails(module);
                var moduleDetail = ModuleViewManager.GetByName(module);
                //  UGITModule moduleDetails = ObjModuleViewManager.LoadByName(moduleName); // UGITUtility.GetModuleDetails(moduleName);

                e.Row.Attributes.Add("onclick", string.Format("window.parent.UgitOpenPopupDialog('{0}', '{1}', '{2}', 90, 90, 0, '{3}')", UGITUtility.GetAbsoluteURL(moduleDetail.StaticModulePagePath), string.Format("TicketId={0}", ticketID), title, Server.UrlEncode(Request.Url.AbsolutePath)));
                e.Row.Style.Add("cursor", "pointer");

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class CPRProjectTitleControl : UserControl
    {
        public string ticketID { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleFormLayoutManager moduleFormLayoutManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            moduleFormLayoutManager = new ModuleFormLayoutManager(context);

            LoadLabels();
            LoadValues();
        }

        private void LoadLabels()
        {
            if (uHelper.getModuleNameByTicketId(ticketID) == "OPM")
            {
                trOPMProject.Visible = true;
                trOPMProjectDetail.Visible = true;
                trProjectrow.Visible = false;
                trProjectrow2.Visible = false;

                //List<ModuleFormLayout> mFormLayouts = moduleFormLayoutManager.Load($"{DatabaseObjects.Columns.ModuleNameLookup} = 'OPM' and FieldName in ('{DatabaseObjects.Columns.Title}', '{DatabaseObjects.Columns.CRMCompanyTitleLookup}', '{DatabaseObjects.Columns.TicketDescription}')");
                List<ModuleFormLayout> mFormLayouts = moduleFormLayoutManager.Load(x => x.ModuleNameLookup == "OPM" && (x.FieldName == DatabaseObjects.Columns.Title || x.FieldName == DatabaseObjects.Columns.CRMCompanyLookup || x.FieldName == DatabaseObjects.Columns.TicketDescription));

                if (mFormLayouts != null)
                {
                    /*
                    ModuleFormLayout mFLTitle = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.Title);
                    if (mFLTitle != null)
                        lblOPMProjectName.InnerText = mFLTitle.FieldDisplayName;

                    ModuleFormLayout mFLCRMCompanyTitleLookup = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.CRMCompanyTitleLookup);
                    if (mFLCRMCompanyTitleLookup != null)
                        lblOPMProjectClient.InnerText = mFLCRMCompanyTitleLookup.FieldDisplayName;

                    ModuleFormLayout mFLTickeDescription = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.TicketDescription);
                    if (mFLTickeDescription != null)
                        lblOPMProjectDescription.InnerText = mFLTickeDescription.FieldDisplayName;

                    */

                    ModuleFormLayout formLayoutItem = null;
                    formLayoutItem = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.Title);
                    if (formLayoutItem != null)
                        lblOPMProjectName.InnerText = formLayoutItem.FieldDisplayName;

                    formLayoutItem = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.CRMCompanyLookup);
                    if (formLayoutItem != null)
                        lblOPMProjectClient.InnerText = formLayoutItem.FieldDisplayName;

                    formLayoutItem = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.TicketDescription);
                    if (formLayoutItem != null)
                        lblOPMProjectDescription.InnerText = formLayoutItem.FieldDisplayName;
                }
            }
            else
            {

                trOPMProject.Visible = false;
                trOPMProjectDetail.Visible = false;
                trProjectrow.Visible = true;
                trProjectrow2.Visible = true;

                /*
                List<ModuleFormLayout> mFormLayouts = moduleFormLayoutManager.Load($"{DatabaseObjects.Columns.ModuleNameLookup} = 'CPR'");
                if (mFormLayouts != null)
                {
                    ModuleFormLayout mFLEstimateNo = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.EstimateNo);
                    if (mFLEstimateNo != null)
                        lblEstimateNo.InnerText = mFLEstimateNo.FieldDisplayName;

                    ModuleFormLayout mFLTitle = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.Title);
                    if (mFLTitle != null)
                        lblProjectName.InnerText = mFLTitle.FieldDisplayName;

                    ModuleFormLayout mFLCRMCompanyTitleLookup = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.CRMCompanyTitleLookup);
                    if (mFLCRMCompanyTitleLookup != null)
                        lblClient.InnerText = mFLCRMCompanyTitleLookup.FieldDisplayName;

                    ModuleFormLayout mFLTicketRequestTypeLookup = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.TicketRequestTypeLookup);
                    if (mFLTicketRequestTypeLookup != null)
                        lblProjectType.InnerText = mFLTicketRequestTypeLookup.FieldDisplayName;

                    ModuleFormLayout mFLCRMProjectID = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.CRMProjectID);
                    if (mFLCRMProjectID != null)
                        lblProjectNo.InnerText = mFLCRMProjectID.FieldDisplayName;
                }
                */

                var fieldFilter = new string[] { DatabaseObjects.Columns.EstimateNo, DatabaseObjects.Columns.Title, DatabaseObjects.Columns.CRMCompanyLookup, DatabaseObjects.Columns.TicketRequestTypeLookup, DatabaseObjects.Columns.CRMProjectID };
                List<ModuleFormLayout> mFormLayouts = moduleFormLayoutManager.Load(x => x.ModuleNameLookup == "CPR" && fieldFilter.Contains(x.FieldName));

                if (mFormLayouts != null)
                {
                    ModuleFormLayout formLayoutItem = null;

                    formLayoutItem = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.EstimateNo);
                    if (formLayoutItem != null)
                        lblEstimateNo.InnerText = formLayoutItem.FieldDisplayName;

                    formLayoutItem = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.Title);
                    if (formLayoutItem != null)
                        lblProjectName.InnerText = formLayoutItem.FieldDisplayName;

                    formLayoutItem = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.CRMCompanyLookup);
                    if (formLayoutItem != null)
                        lblClient.InnerText = formLayoutItem.FieldDisplayName;

                    formLayoutItem = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.TicketRequestTypeLookup);
                    if (formLayoutItem != null)
                        lblProjectType.InnerText = formLayoutItem.FieldDisplayName;

                    //formLayoutItem = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.CRMProjectID);
                    //if (formLayoutItem != null)
                    //    lblProjectNo.InnerText = formLayoutItem.FieldDisplayName;

                    formLayoutItem = mFormLayouts.Find(x => x.FieldName == DatabaseObjects.Columns.ERPJobID);
                    if (formLayoutItem != null)
                        lblERPJobID.InnerText = formLayoutItem.FieldDisplayName;
                }
            }
        }

        private void LoadValues()
        {
            if (uHelper.getModuleNameByTicketId(ticketID) == "OPM")
            {
                //List<string> viewFields = new List<string>(){
                //                    DatabaseObjects.Columns.Id,
                //                    DatabaseObjects.Columns.Title,
                //                    DatabaseObjects.Columns.CRMCompanyTitleLookup,
                //                    DatabaseObjects.Columns.TicketRequestTypeCategory,
                //                    DatabaseObjects.Columns.TicketRequestTypeSubCategory,
                //                    DatabaseObjects.Columns.TicketRequestTypeLookup,
                //                     DatabaseObjects.Columns.TicketDescription};

                //SPListItem spOPMProjectitem = uHelper.getCurrentTicket("OPM", ticketID, viewFields,true);
                DataRow spOPMProjectitem = Ticket.GetCurrentTicket(context, "OPM", ticketID);

                if (spOPMProjectitem != null)
                {
                    lblOPMProjectNameval.InnerText = UGITUtility.TruncateWithEllipsis(Convert.ToString(spOPMProjectitem[DatabaseObjects.Columns.Title]), 30);

                    //lblOPMProjectClientval.InnerText = Convert.ToString(spOPMProjectitem[DatabaseObjects.Columns.CRMCompanyTitleLookup]);
                    if (!string.IsNullOrEmpty(Convert.ToString(spOPMProjectitem[DatabaseObjects.Columns.CRMCompanyLookup])))
                        lblOPMProjectClientval.InnerText = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMCompany, DatabaseObjects.Columns.Title, Convert.ToString(spOPMProjectitem[DatabaseObjects.Columns.CRMCompanyLookup]), context.TenantID));

                    //lblOPMProjectDescriptionval.InnerText = uHelper.ConvertTextAreaStringToHtml(Convert.ToString(spOPMProjectitem[DatabaseObjects.Columns.TicketDescription]));
                    lblOPMProjectDescriptionval.InnerHtml = uHelper.ConvertTextAreaStringToHtml(Convert.ToString(spOPMProjectitem[DatabaseObjects.Columns.TicketDescription]));
                }
            }
            else
            {
                //List<string> viewFields = new List<string>(){
                //     DatabaseObjects.Columns.Id,
                //     DatabaseObjects.Columns.CRMProjectID,
                //     DatabaseObjects.Columns.EstimateNo,
                //     DatabaseObjects.Columns.Title,
                //     DatabaseObjects.Columns.CRMCompanyTitleLookup,
                //     DatabaseObjects.Columns.TicketRequestTypeCategory,
                //     DatabaseObjects.Columns.TicketRequestTypeSubCategory,
                //     DatabaseObjects.Columns.TicketRequestTypeLookup,
                //      DatabaseObjects.Columns.TicketRequestTypeLookup};

                DataRow spCRMProjectitem = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(ticketID), ticketID);

                if (spCRMProjectitem != null)
                {
                    lblEstimateNoVal.InnerText = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.EstimateNo]);
                    lblProjectNameVal.InnerText = UGITUtility.TruncateWithEllipsis(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.Title]), 30);
                    //lblClientVal.InnerText = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.CRMCompanyTitleLookup]);

                    if (!string.IsNullOrEmpty(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.CRMCompanyLookup])))
                        lblClientVal.InnerText = Convert.ToString(GetTableDataManager.GetSingleValueByTicketId(DatabaseObjects.Tables.CRMCompany, DatabaseObjects.Columns.Title, Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.CRMCompanyLookup]), context.TenantID));

                    //List<string> projectType = new List<string>();
                    //projectType.Add(uHelper.GetLookupValue(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.TicketRequestTypeCategory])));
                    //projectType.Add(uHelper.GetLookupValue(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.TicketRequestTypeSubCategory])));
                    //projectType.Add(uHelper.GetLookupValue(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.TicketRequestTypeLookup])));

                    //lblProjectTypeVal.InnerText = string.Join(" > ", projectType.ToArray());
                    if (!string.IsNullOrEmpty(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.TicketRequestTypeLookup])))
                        lblProjectTypeVal.InnerText = Convert.ToString(GetTableDataManager.GetSingleValueByID(DatabaseObjects.Tables.RequestType, DatabaseObjects.Columns.Title, Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.TicketRequestTypeLookup]), context.TenantID));

                    //lblProjectNoVal.InnerText = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.CRMProjectID]);

                    if (!string.IsNullOrEmpty(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ERPJobID])))
                        lblERPJobIDVal.InnerText = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ERPJobID]);

                }
            }
        }
    }
}

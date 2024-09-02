using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Web;
using uGovernIT.Manager;
using uGovernIT.Utility;
using System.Collections.Generic;
using System.Data.SqlClient;
using Utils;
using DevExpress.Web;
using System.Linq;
using System.IO;

namespace uGovernIT.Web
{
    public partial class RequestTypeByLocation : UserControl
    {

        public string Location { get; set; }
        public int ReqId { get; set; }
        public string RequestTypeIDs { private get; set; }
        public bool Use24x7Calendar { get; set; }
        string[] requestTypes;
        protected const string Varies = "<Value Varies>";
        RequestTypeByLocationManager objRequestTypeByLocationManager;
        RequestTypeManager requestTypeManager;
        ApplicationContext applicationContext = HttpContext.Current.GetManagerContext();
        List<ModuleRequestTypeLocation> reqLocationCollection;
        List<ModuleRequestType> requesetTypeCollection;
        LocationManager locationManager;
        protected override void OnInit(EventArgs e)
        {
            objRequestTypeByLocationManager = new RequestTypeByLocationManager(applicationContext);
            requestTypeManager = new RequestTypeManager(applicationContext);
            locationManager = new LocationManager(applicationContext);
            requestTypes = UGITUtility.SplitString(RequestTypeIDs, ",", StringSplitOptions.RemoveEmptyEntries);
            if (requestTypes.Length == 0)
                return;
            requesetTypeCollection = requestTypeManager.Load(x => requestTypes.Contains(UGITUtility.ObjectToString(x.ID)));
            //Full request type list based on request type ids
            if (!string.IsNullOrWhiteSpace(Location))
            {
                reqLocationCollection = objRequestTypeByLocationManager.Load(x => requestTypes.Contains(UGITUtility.ObjectToString(x.RequestTypeLookup)) && x.LocationLookup == UGITUtility.StringToLong(Location));
            }

            BindLocation();
            if (reqLocationCollection != null && reqLocationCollection.Count > 0)
            {
                if (!IsPostBack)
                {
                    Fill();
                }
                LnkbtnDelete.Visible = true;
            }
            else
            {
                LnkbtnDelete.Visible = false;
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (requesetTypeCollection != null && requesetTypeCollection.Count > 1)
            {
                lbOwner.Text = Server.HtmlEncode(Varies);
                lbPRPGroup.Text = Server.HtmlEncode(Varies);
                lbORP.Text = Server.HtmlEncode(Varies);
                lbExecManager.Text = Server.HtmlEncode(Varies);
                lbBackupMan.Text = Server.HtmlEncode(Varies);

                if (!UGITUtility.StringToBoolean(hdnOwnerVariesEnable.Value))
                {
                    pViewPPEOwner.Style.Add(HtmlTextWriterStyle.Display, "none");
                    pEditPPEOwner.Style.Add(HtmlTextWriterStyle.Display, "block");
                }

                if (!UGITUtility.StringToBoolean(hdnPRPVariesEnable.Value))
                {
                    pViewPPEPrpGroup.Style.Add(HtmlTextWriterStyle.Display, "none");
                    pEditPPEPrpGroup.Style.Add(HtmlTextWriterStyle.Display, "block");
                }

                if (!UGITUtility.StringToBoolean(hdnORPVariesEnable.Value))
                {
                    pViewPPEORP.Style.Add(HtmlTextWriterStyle.Display, "none");
                    pEditPPEORP.Style.Add(HtmlTextWriterStyle.Display, "block");
                }

                if (!UGITUtility.StringToBoolean(hdnExecManagerVariesEnable.Value))
                {
                    pViewPPEExecManeger.Style.Add(HtmlTextWriterStyle.Display, "none");
                    pEditPPEExecManeger.Style.Add(HtmlTextWriterStyle.Display, "block");
                }

                if (!UGITUtility.StringToBoolean(hdnBackupManVariesEnable.Value))
                {
                    pViewPPEBackUpMan.Style.Add(HtmlTextWriterStyle.Display, "none");
                    pEditPPEBackUpMan.Style.Add(HtmlTextWriterStyle.Display, "block");
                }
            }

            base.OnLoad(e);
        }

        private void BindLocation()
        {
            List<Location> listLocation = locationManager.Load().OrderBy(x => x.Title).ToList();
            if (listLocation != null && listLocation.Count > 0)
            {
                ddlLocation.DataSource = listLocation;
                ddlLocation.DataTextField = DatabaseObjects.Columns.Title;
                ddlLocation.DataValueField = DatabaseObjects.Columns.ID;
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, new ListItem("None", "0"));
            }
        }
        private void Fill()
        {
            List<ModuleRequestTypeLocation> reqLocationData = reqLocationCollection;
            if (reqLocationData == null)
                return;
            int numberOfIDs = reqLocationData.Count;
            if (numberOfIDs > 1)
                ddlLocation.Enabled = false;
            ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(Convert.ToString(Location)));
            bool valueVaries = false;
            // For Owner With Multiuser
            if (numberOfIDs > 1 && reqLocationData.Select(x => x.Owner).Distinct().ToList().Count > 1)
            {
                valueVaries = true;
                hdnOwnerVariesEnable.Value = "true";
                pViewPPEOwner.Style.Add(HtmlTextWriterStyle.Display, "block");
                pEditPPEOwner.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            if (!valueVaries)
            {
                ppeOwner.SetValues(Convert.ToString(reqLocationData[0].Owner));
            }
            valueVaries = false;
            if (numberOfIDs > 1 && reqLocationData.Select(x => x.PRPGroup).Distinct().ToList().Count > 1)//.ToTable(true, DatabaseObjects.Columns.PRPGroup).Rows.Count > 1)
            {
                valueVaries = true;
                hdnPRPVariesEnable.Value = "true";
                pViewPPEPrpGroup.Style.Add(HtmlTextWriterStyle.Display, "block");
                pEditPPEPrpGroup.Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            // For prp group sinlge user
            if (!valueVaries)
            {
                ppePrpGroup.SetValues(Convert.ToString(reqLocationData[0].PRPGroup));
            }
            if (!valueVaries)
            {
                ppePRP.SetValues(Convert.ToString(reqLocationData[0].PRPUser));
            }
            valueVaries = false;
            if (numberOfIDs > 1 && reqLocationData.Select(x => x.ORP).Distinct().ToList().Count > 1)
            {
                valueVaries = true;
                hdnORPVariesEnable.Value = "true";
                pViewPPEORP.Style.Add(HtmlTextWriterStyle.Display, "block");
                pEditPPEORP.Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            if (!valueVaries)
            {
                ppeORP.SetValues(Convert.ToString(reqLocationData[0].ORP));
            }
            // For Escalation Manager   
            valueVaries = false;
            if (numberOfIDs > 1 && reqLocationData.Select(x => x.EscalationManager).Distinct().ToList().Count > 1)//(true, DatabaseObjects.Columns.RequestTypeEscalationManager).Rows.Count > 1)
            {
                valueVaries = true;
                hdnExecManagerVariesEnable.Value = "true";
                pViewPPEExecManeger.Style.Add(HtmlTextWriterStyle.Display, "block");
                pEditPPEExecManeger.Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            if (!valueVaries)
            {
                ppeExecManeger.SetValues(Convert.ToString(reqLocationData[0].EscalationManager));
            }

            // For Backup Escalation Manager
            valueVaries = false;
            if (numberOfIDs > 1 && reqLocationData.Select(x => x.BackupEscalationManager).Distinct().ToList().Count > 1)//(true, DatabaseObjects.Columns.RequestTypeBackupEscalationManager).Rows.Count > 1)
            {
                valueVaries = true;
                hdnBackupManVariesEnable.Value = "true";
                pViewPPEBackUpMan.Style.Add(HtmlTextWriterStyle.Display, "block");
                pEditPPEBackUpMan.Style.Add(HtmlTextWriterStyle.Display, "none");
            }

            if (!valueVaries)
            {
                ppeBackUpMan.SetValues(Convert.ToString(reqLocationCollection[0].BackupEscalationManager));
            }

            //fill sla of request type location.
            if (numberOfIDs == 1)
            {
                FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(reqLocationCollection[0].RequestorContactSLA), txtRequestorContactSLA, ddlRequestorContactSLAType);
                FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(reqLocationCollection[0].AssignmentSLA), txtAssignmentSLA, ddlAssignmentSLAType);
                FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(reqLocationCollection[0].ResolutionSLA), txtResolutionSLA, ddlResolutionSLAType);
                FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(reqLocationCollection[0].CloseSLA), txtCloseSLA, ddlCloseSLAType);
            }
            else
            {
                if (reqLocationData.Select(x => x.RequestorContactSLA).Distinct().ToList().Count == 1)//.ToTable(true, DatabaseObjects.Columns.RequestorContactSLA).Rows.Count == 1)
                    FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(reqLocationCollection[0].RequestorContactSLA), txtRequestorContactSLA, ddlRequestorContactSLAType);
                else
                    txtRequestorContactSLA.Text = Varies;

                if (reqLocationData.Select(x => x.AssignmentSLA).Distinct().ToList().Count == 1)
                    FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(reqLocationCollection[0].AssignmentSLA), txtAssignmentSLA, ddlAssignmentSLAType);
                else
                    txtAssignmentSLA.Text = Varies;

                if (reqLocationData.Select(x => x.ResolutionSLA).Distinct().ToList().Count == 1)
                    FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(reqLocationCollection[0].ResolutionSLA), txtResolutionSLA, ddlResolutionSLAType);
                else
                    txtResolutionSLA.Text = Varies;

                if (reqLocationData.Select(x => x.CloseSLA).Distinct().ToList().Count == 1)
                    FillSLADayHourAndMinutesFormat(UGITUtility.StringToDouble(reqLocationCollection[0].CloseSLA), txtCloseSLA, ddlCloseSLAType);
                else
                    txtCloseSLA.Text = Varies;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModuleRequestType moduleRequestType = new ModuleRequestType();
            Dictionary<string, object> values = new Dictionary<string, object>();
            ModuleRequestTypeLocation moduleRequestTypeLocation = new ModuleRequestTypeLocation();
            if (ddlLocation.SelectedValue == "0")
            {
                cvLocation.ErrorMessage = "Please choose location!";
                cvLocation.IsValid = false;
                return;
            }
            // Check if existing location
            if (requestTypes.Length == 1)
            {
                int requestTypeID = Convert.ToInt32(requestTypes[0]);
                moduleRequestType = requestTypeManager.LoadByID(Convert.ToInt64(requestTypeID));
                moduleRequestTypeLocation = objRequestTypeByLocationManager.LoadByID(ReqId);
                if (moduleRequestTypeLocation == null)
                    moduleRequestTypeLocation = new ModuleRequestTypeLocation();
                List<ModuleRequestTypeLocation> coll = objRequestTypeByLocationManager.Load(x => x.LocationLookup == UGITUtility.StringToLong(ddlLocation.SelectedValue) && x.RequestTypeLookup == requestTypeID);
                if (coll.Count > 0)
                {
                    if (reqLocationCollection == null || Convert.ToInt64(reqLocationCollection[0].ID) != Convert.ToInt64(coll[0].ID))
                    {
                        cvLocation.ErrorMessage = "Location already exists!";
                        cvLocation.IsValid = false;
                        return;
                    }
                }
            }
            string owners = ppeOwner.GetValues();
            if (!UGITUtility.StringToBoolean(hdnOwnerVariesEnable.Value) && (owners == null || owners.Length == 0))
            {
                cvOwner.ErrorMessage = "Please enter Owner!";
                cvOwner.IsValid = false;
                return;
            }

            if (!Page.IsValid)
                return;
            //load request location based for selectedd location 
            //This block will be executed only in case on add multip entries for location.
            if ((reqLocationCollection == null || reqLocationCollection.Count == 0) && ddlLocation.SelectedItem != null && ddlLocation.SelectedItem.Text != Location)
            {

                //DataTable dtRequestLocation = GetTableDataManager.GetTableData(DatabaseObjects.Tables.RequestTypeByLocation);
                //reqLocationCollection = dtRequestLocation.Select(string.Format("{0} In ({1}) And {2} ='{3}'", DatabaseObjects.Columns.TicketRequestTypeLookup, string.Join(",", requestTypes.Select(x => string.Format("'{0}'", x))), DatabaseObjects.Columns.LocationLookup, ddlLocation.SelectedItem.Text));
            }

            //List<SPListItem> reqLocationList = new List<SPListItem>();
            //if (reqLocationCollection != null && reqLocationCollection.Count > 0)
            //    reqLocationList = reqLocationCollection.Cast<SPListItem>().ToList();

            //SPList reqeustLocationList = SPListHelper.GetSPList(DatabaseObjects.Lists.RequestTypeByLocation);
            //SPListItem spRtypeLoc = null;
            foreach (string requestType in requestTypes)
            {
                int requestID = UGITUtility.StringToInt(requestType);
                if (requestID == 0)
                    continue;

                // SPListItem requestTypeItem = SPListHelper.GetItemByID(requesetTypeCollection, requestID);
                //if (requestTypeItem == null)
                //    continue;


                //spRtypeLoc = null;
                //if (reqLocationList.Count > 0)
                //    spRtypeLoc = reqLocationList.FirstOrDefault(x => (new SPFieldLookupValue(Convert.ToString(x[DatabaseObjects.Columns.TicketRequestTypeLookup]))).LookupId == requestID);



                moduleRequestTypeLocation.LocationLookup = UGITUtility.StringToLong(ddlLocation.SelectedValue);
                moduleRequestTypeLocation.RequestTypeLookup = Convert.ToInt64(requestID);
                moduleRequestTypeLocation.ModuleNameLookup = moduleRequestType.ModuleNameLookup;
                if (!UGITUtility.StringToBoolean(hdnOwnerVariesEnable.Value))
                {
                    moduleRequestTypeLocation.Owner = owners;
                }

                if (!UGITUtility.StringToBoolean(hdnPRPVariesEnable.Value))
                {
                    moduleRequestTypeLocation.PRPGroup = ppePrpGroup.GetValues();
                }

                if (!UGITUtility.StringToBoolean(hdnORPVariesEnable.Value))
                {
                    moduleRequestTypeLocation.ORP = ppeORP.GetValues();
                }

                if (!UGITUtility.StringToBoolean(hdnExecManagerVariesEnable.Value))
                {
                    moduleRequestTypeLocation.EscalationManager = ppeExecManeger.GetValues();
                }

                if (!UGITUtility.StringToBoolean(hdnBackupManVariesEnable.Value))
                {
                    moduleRequestTypeLocation.BackupEscalationManager = ppeBackUpMan.GetValues();
                }
                //save sla value for request type location.
                moduleRequestTypeLocation.RequestorContactSLA = SetSLAValueSPListItem(txtRequestorContactSLA, ddlRequestorContactSLAType);
                moduleRequestTypeLocation.AssignmentSLA = SetSLAValueSPListItem(txtAssignmentSLA, ddlAssignmentSLAType);
                moduleRequestTypeLocation.ResolutionSLA = SetSLAValueSPListItem(txtResolutionSLA, ddlResolutionSLAType);
                moduleRequestTypeLocation.CloseSLA = SetSLAValueSPListItem(txtCloseSLA, ddlCloseSLAType);
                if (!UGITUtility.StringToBoolean(hdnPRPUserVariesEnable.Value))
                {
                    moduleRequestTypeLocation.PRPUser = ppePRP.GetValues();
                }
                if (ReqId > 0)
                {
                    objRequestTypeByLocationManager.Update(moduleRequestTypeLocation);

                }
                else
                {
                    objRequestTypeByLocationManager.Insert(moduleRequestTypeLocation);
                }


            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, false);
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ModuleRequestTypeLocation moduleRequestTypeLocation = objRequestTypeByLocationManager.LoadByID(ReqId);
                if (moduleRequestTypeLocation != null)
                    objRequestTypeByLocationManager.Delete(moduleRequestTypeLocation);

            }
            catch (Exception ex)
            {
                Util.Log.ULog.WriteException(ex);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        private void FillSLADayHourAndMinutesFormat(double minutes, TextBox textcontrol, DropDownList dropdown)
        {
            int workingHoursInADay = 0;

            if (!Use24x7Calendar)
                workingHoursInADay = uHelper.GetWorkingHoursInADay(HttpContext.Current.GetManagerContext(), true);
            else
                workingHoursInADay = 24;
            
            if (minutes % (workingHoursInADay * 60) == 0)
            {
                textcontrol.Text = string.Format("{0:0.##}", minutes / (workingHoursInADay * 60));
                dropdown.SelectedValue = Constants.SLAConstants.Days;
            }
            else if (minutes % 60 == 0)
            {
                textcontrol.Text = string.Format("{0:0.##}", minutes / 60);
                dropdown.SelectedValue = Constants.SLAConstants.Hours;
            }
            else
            {
                textcontrol.Text = string.Format("{0:0.##}", minutes);
                dropdown.SelectedValue = Constants.SLAConstants.Minutes;
            }
        }

        private double SetSLAValueSPListItem(TextBox textcontrol, DropDownList dropdown)
        {
            double calculateTime = 0.0;
            if (textcontrol.Text != Varies)
            {
                int workingHoursInADay = 0;

                if (!Use24x7Calendar)
                    workingHoursInADay = uHelper.GetWorkingHoursInADay(HttpContext.Current.GetManagerContext(), true);
                else
                    workingHoursInADay = 24;
                // Converting days,hours into minutes
                if (dropdown.SelectedValue == Constants.SLAConstants.Days)
                {
                    calculateTime = Convert.ToDouble(textcontrol.Text.Trim()) * 60 * workingHoursInADay;
                }
                else if (dropdown.SelectedValue == Constants.SLAConstants.Hours)
                {
                    calculateTime = Convert.ToDouble(textcontrol.Text.Trim()) * 60;
                }
                else
                {
                    calculateTime = Convert.ToDouble(textcontrol.Text.Trim());
                }
            }
            return calculateTime;
        }
    }
}

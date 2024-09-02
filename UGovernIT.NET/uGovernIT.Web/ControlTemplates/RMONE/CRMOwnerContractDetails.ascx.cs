using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.RMONE
{
    public partial class CRMOwnerContractDetails : System.Web.UI.UserControl
    {
        public string TicketId { get; set; }
        public string ContractType { get; set; }
        public string FeePct { get; set; }
        public string ConstructionStart { get; set; }
        public string ConstructionEnd { get; set; }
        public string RequestType { get; set; }
        public string ApproxContractValue { get; set; }
        public string Insurance { get; set; }
        public string GLInsurance { get; set; }
        public string SDI { get; set; }
        public string Bond { get; set; }

        protected FieldConfigurationManager fieldConfigurationManager;
        protected string ModuleName
        {
            get
            {
                return uHelper.getModuleNameByTicketId(TicketId);
            }
        }

        ApplicationContext AppContext = HttpContext.Current.GetManagerContext();
        protected DataRow CurrentTicket
        {
            get
            {
                Ticket ticket = new Ticket(AppContext, ModuleName);
                List<string> viewFields = new List<string>();
                viewFields.Add(DatabaseObjects.Columns.OwnerContractTypeChoice);
                viewFields.Add(DatabaseObjects.Columns.FeePct);
                viewFields.Add(DatabaseObjects.Columns.EstimatedConstructionStart);
                viewFields.Add(DatabaseObjects.Columns.EstimatedConstructionEnd);
                viewFields.Add(DatabaseObjects.Columns.ApproxContractValue);
                viewFields.Add(DatabaseObjects.Columns.RequestTypeLookup);
                viewFields.Add(DatabaseObjects.Columns.Insurance);
                viewFields.Add(DatabaseObjects.Columns.GLInsurance);
                viewFields.Add(DatabaseObjects.Columns.SDI);
                viewFields.Add(DatabaseObjects.Columns.Bond);
                return Ticket.GetCurrentTicket(AppContext, ModuleName, TicketId, viewFields);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            fieldConfigurationManager = new FieldConfigurationManager(AppContext);
            if (this.CurrentTicket != null)
            {
                this.ApproxContractValue = !string.IsNullOrWhiteSpace(this.CurrentTicket[DatabaseObjects.Columns.ApproxContractValue].ToString()) ?
                    Convert.ToInt32(this.CurrentTicket[DatabaseObjects.Columns.ApproxContractValue]).ToString("N0") : "0";
                this.RequestType = fieldConfigurationManager.GetFieldConfigurationData(DatabaseObjects.Columns.RequestTypeLookup, UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.RequestTypeLookup]));
                this.ContractType = UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.OwnerContractTypeChoice]);
                this.FeePct = UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.FeePct]);
                this.FeePct = !string.IsNullOrWhiteSpace(this.FeePct) ? this.FeePct + "%" : "-";
                this.ConstructionStart = UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket[DatabaseObjects.Columns.EstimatedConstructionStart]), false);
                this.ConstructionEnd = UGITUtility.GetDateStringInFormat(Convert.ToString(CurrentTicket[DatabaseObjects.Columns.EstimatedConstructionEnd]), false);
                this.Insurance = !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.Insurance])) 
                    ? UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.Insurance])
                    : "-";
                if(UGITUtility.IfColumnExists(DatabaseObjects.Columns.GLInsurance, this.CurrentTicket.Table))
                    this.GLInsurance = !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.GLInsurance]))
                    ? "$" + UGITUtility.StringToInt(this.CurrentTicket[DatabaseObjects.Columns.GLInsurance]).ToString("#,##0") : "";
                if(UGITUtility.IfColumnExists(DatabaseObjects.Columns.SDI, this.CurrentTicket.Table))
                    this.SDI = !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.SDI]))
                    ? "$" + UGITUtility.StringToInt(this.CurrentTicket[DatabaseObjects.Columns.SDI]).ToString("#,##0") : "";
                if(UGITUtility.IfColumnExists(DatabaseObjects.Columns.Bond, this.CurrentTicket.Table))
                    this.Bond = !string.IsNullOrWhiteSpace(UGITUtility.ObjectToString(this.CurrentTicket[DatabaseObjects.Columns.Bond]))
                    ? "$" + UGITUtility.StringToInt(this.CurrentTicket[DatabaseObjects.Columns.Bond]).ToString("#,##0") : "";
            }
        }
    }
}
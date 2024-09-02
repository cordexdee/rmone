using System;
using System.Data;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Manager.Managers;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class TimeLineControl : UserControl
    {
        public string TicketId { get; set; }
        public int CPRId { get; set; }
        public string Module { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();

        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(TicketId))
            {
                return;
            }

            if (string.IsNullOrEmpty(Module))
            {
                return;
            }

            ///DataRow dataRow = uGITCache.ModuleDataCache.GetTicketData(Module, TicketId);
            DataRow dataRow = Ticket.GetCurrentTicket(context, Module, TicketId);
            //SPListItem currentTicket = SPListHelper.GetSPListItem(DatabaseObjects.Lists.CRMProject, CPRId);

            DateTime projectStartDate = dataRow.Table.Columns.Contains(DatabaseObjects.Columns.PreconStartDate) && dataRow[DatabaseObjects.Columns.PreconStartDate] != null && !(dataRow[DatabaseObjects.Columns.PreconStartDate] is DBNull) ? Convert.ToDateTime(dataRow[DatabaseObjects.Columns.PreconStartDate]) : DateTime.MinValue;
            DateTime constStartDate = dataRow.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionStart) && dataRow[DatabaseObjects.Columns.EstimatedConstructionStart] != null && !(dataRow[DatabaseObjects.Columns.EstimatedConstructionStart] is DBNull) ? Convert.ToDateTime(dataRow[DatabaseObjects.Columns.EstimatedConstructionStart]) : DateTime.MinValue;
            DateTime constEndDate = dataRow.Table.Columns.Contains(DatabaseObjects.Columns.EstimatedConstructionEnd) && dataRow[DatabaseObjects.Columns.EstimatedConstructionEnd] != null && !(dataRow[DatabaseObjects.Columns.EstimatedConstructionEnd] is DBNull) ? Convert.ToDateTime(dataRow[DatabaseObjects.Columns.EstimatedConstructionEnd]) : DateTime.MinValue;
            DateTime projectCloseDate = dataRow.Table.Columns.Contains(DatabaseObjects.Columns.TicketCloseDate) && dataRow[DatabaseObjects.Columns.TicketCloseDate] != null && !(dataRow[DatabaseObjects.Columns.TicketCloseDate] is DBNull) ? Convert.ToDateTime(dataRow[DatabaseObjects.Columns.TicketCloseDate]) : DateTime.MinValue;



            if (string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.PreconStartDate])))
            {
                tdconprojectstartgraph.Visible = false;
                tdconsstartmidpipe.Visible = false;
                tdconprojectstartlabel.ColSpan = 1;
                tdconprojectstartlabel.Visible = false;
                lblProjectStartDt.Text = "N/A";
            }
            else
            {
                lblProjectStartDt.Text = string.Format("{0:MMM-dd-yyyy}", projectStartDate);
                
                if (projectStartDate <= DateTime.Today)
                {
                    graphdiv1.Attributes["class"] = "midpipe completegraph";
                } 
            }


                ///Construction Start Date
            if (string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.EstimatedConstructionStart])))
                {
                    tdconsstartmidpipe.Visible = false;
                    tdconsstartgraph.Visible = false;
                    tdconststartlabel.ColSpan = 1;
                    tdconststartlabel.Visible = false;
                    lblConstStartDt.Text = "N/A";
                }
                else
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.PreconStartDate])))
                    {
                        lblConstStartDt.CssClass = "newdatelabel";
                    }
                    lblConstStartDt.Text = string.Format("{0:MMM-dd-yyyy}", constStartDate);
                    if (constStartDate <= DateTime.Today)
                    {
                        graphdiv1.Attributes["class"] = "midpipe completegraph";
                    }                    
                }

                ///Construction End Date
            if (string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.EstimatedConstructionEnd])))
                {
                    tdconsendmidpipe.Visible = false;
                    tdconsendgraph.Visible = false;
                    tdconstendlabel.ColSpan = 1;
                    tdconstendlabel.Visible = false;
                    lblConstEndDt.Text = "N/A";
                }
                else
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.PreconStartDate])) && string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.EstimatedConstructionStart])))
                    {
                        lblConstEndDt.CssClass = "newdatelabel";
                    }
                    else if (string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.PreconStartDate])) && !string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.EstimatedConstructionStart])))
                    {
                        lblConstEndDt.CssClass = "newdatelabel";
                    }

                    lblConstEndDt.Text = string.Format("{0:MMM-dd-yyyy}", constEndDate);
                    if (string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.EstimatedConstructionStart])))
                    {
                        tdconsendmidpipe.Visible = false;
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.PreconStartDate])))
                    {
                        tdconsendmidpipe.Visible = true;
                    }
                        if (constEndDate <= DateTime.Today)
                        {
                            graphdiv2.Attributes["class"] = "midpipe completegraph";
                        }
                    
                }


                ///Project End Date
            if (string.IsNullOrEmpty(Convert.ToString(dataRow[DatabaseObjects.Columns.TicketCloseDate])))
                {
                    tdproclosemidpipe.Visible = false;
                    tdproclosegraph.Visible = false;
                    tdconstendlabel.ColSpan = 1;
                    tdprocloselabel.Visible = false;
                    lblProjectEndDt.Text = "N/A";
                }
                else
                {
                    lblProjectEndDt.Text = string.Format("{0:MMM-dd-yyyy}", projectCloseDate);
                    if (projectCloseDate <= DateTime.Today)
                    {
                        graphdiv3.Attributes["class"] = "midpipe completegraph";
                    }
                }
            
            base.OnInit(e);
        }



    }
}

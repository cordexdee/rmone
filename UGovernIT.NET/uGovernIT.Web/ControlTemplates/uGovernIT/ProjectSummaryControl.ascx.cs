using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using uGovernIT.Manager;
using uGovernIT.Utility;

namespace uGovernIT.Web
{
    public partial class ProjectSummaryControl : UserControl
    {
        public string ticketID { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        ModuleViewManager moduleManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            moduleManager = new ModuleViewManager(context);
            FillData();
        }

        void FillData()
        {
            string module = uHelper.getModuleNameByTicketId(ticketID);
            DataTable dt = new DataTable();
            dt.Columns.Add("FieldName", typeof(string));
            dt.Columns.Add("FieldValue", typeof(string));


            if (module == "CPR" || module == "CNS")
            {
                List<string> viewFields = new List<string>(){
                    DatabaseObjects.Columns.Id, 
                    DatabaseObjects.Columns.TicketId, 
                    DatabaseObjects.Columns.RFIs, 
                    DatabaseObjects.Columns.CCOs, 
                    DatabaseObjects.Columns.CurrentBudget, 
                    //DatabaseObjects.Columns.OriginalCommitments,
                    DatabaseObjects.Columns.ProjectCost, 
                    DatabaseObjects.Columns.CostVariance,
                     DatabaseObjects.Columns.OrignalStartDate, 
                     DatabaseObjects.Columns.OrignalEndDate, 
                     DatabaseObjects.Columns.ApproxContractValue, 
                     DatabaseObjects.Columns.PctPlannedProfit,
                     DatabaseObjects.Columns.PreconStartDate,
                     DatabaseObjects.Columns.EstimatedConstructionStart,
                     DatabaseObjects.Columns.EstimatedConstructionEnd,
                     DatabaseObjects.Columns.TicketCloseDate
                };

                DataRow spCRMProjectitem = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(ticketID), ticketID);

                if (spCRMProjectitem[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.PreconStartDate]) != "")
                {
                    DataRow dr9 = dt.NewRow();
                    dr9["FieldName"] = "Precon Start";
                    dr9["FieldValue"] = string.Format("{0:MMM-dd-yyyy}", spCRMProjectitem[DatabaseObjects.Columns.PreconStartDate]);
                    dt.Rows.Add(dr9);
                }

                if (spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionStart] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionStart]) != "")
                {
                    DataRow dr10 = dt.NewRow();
                    dr10["FieldName"] = "Construction Start";
                    dr10["FieldValue"] = string.Format("{0:MMM-dd-yyyy}", spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                    dt.Rows.Add(dr10);
                }

                if (spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionEnd] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionEnd]) != "")
                {
                    DataRow dr11 = dt.NewRow();
                    dr11["FieldName"] = "Construction End";
                    dr11["FieldValue"] = string.Format("{0:MMM-dd-yyyy}", spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                    dt.Rows.Add(dr11);
                }
                
                if (spCRMProjectitem[DatabaseObjects.Columns.TicketCloseDate] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.TicketCloseDate]) != "")
                {
                    DataRow dr12 = dt.NewRow();
                    dr12["FieldName"] = "Project close";
                    dr12["FieldValue"] = string.Format("{0:MMM-dd-yyyy}", spCRMProjectitem[DatabaseObjects.Columns.TicketCloseDate]);
                    dt.Rows.Add(dr12);
                }

                DataRow dr1 = dt.NewRow();
                dr1["FieldName"] = "# of RFIs";
                dr1["FieldValue"] = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.RFIs]);
                dt.Rows.Add(dr1);

                DataRow dr2 = dt.NewRow();
                dr2["FieldName"] = "# of Change Orders";
                dr2["FieldValue"] = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.CCOs]);
                dt.Rows.Add(dr2);

                DataRow dr3 = dt.NewRow();
                dr3["FieldName"] = "Budget";
                double budget = 0.0;
                if (spCRMProjectitem[DatabaseObjects.Columns.CurrentBudget] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.CurrentBudget]) != "")
                {
                    if (Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.CurrentBudget]) > 0)
                    {
                        budget = Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.CurrentBudget]);
                    }
                }
                else if (spCRMProjectitem[DatabaseObjects.Columns.ApproxContractValue] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ApproxContractValue]) != "")
                {
                    if(Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.ApproxContractValue]) > 0)
                    {
                        budget = Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.ApproxContractValue]);
                    }
                }

                dr3["FieldValue"] = string.Format("{0:C}", budget);
                dt.Rows.Add(dr3);

                DataRow dr4 = dt.NewRow();
                dr4["FieldName"] = "Commitments";
                dr4["FieldValue"] = (spCRMProjectitem[DatabaseObjects.Columns.OriginalCommitments] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.OriginalCommitments]) != "") ?
                                    string.Format("{0:C}", Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.OriginalCommitments])) :
                                    Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.OriginalCommitments]);
                dt.Rows.Add(dr4);

                DataRow dr5 = dt.NewRow();
                dr5["FieldName"] = "Actual Cost";
                dr5["FieldValue"] = (spCRMProjectitem[DatabaseObjects.Columns.ProjectCost] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ProjectCost]) != "") ?
                                    string.Format("{0:C}", Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.ProjectCost])) :
                                    Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ProjectCost]);
                dt.Rows.Add(dr5);

                DataRow dr6 = dt.NewRow();
                dr6["FieldName"] = "Cost Variance";
                dr6["FieldValue"] = (spCRMProjectitem[DatabaseObjects.Columns.CostVariance] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.CostVariance]) != "") ?
                                    string.Format("{0:###,###.00}", Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.CostVariance])) :
                                    Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.CostVariance]);
                dt.Rows.Add(dr6);

                DataRow dr7 = dt.NewRow();
                dr7["FieldName"] = "Duration";
                string duration = "0";
                if (!string.IsNullOrEmpty(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.OrignalStartDate])) && string.IsNullOrEmpty(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.OrignalEndDate])))
                    duration = Convert.ToString(uHelper.GetTotalWorkingDateBetween(context, Convert.ToDateTime(spCRMProjectitem[DatabaseObjects.Columns.OrignalStartDate]), Convert.ToDateTime(spCRMProjectitem[DatabaseObjects.Columns.OrignalEndDate])));
                dr7["FieldValue"] = duration;
                dt.Rows.Add(dr7);

                DataRow dr8 = dt.NewRow();
                dr8["FieldName"] = "Planned Profit(%)";
                string pProfit = "0";
                if (!string.IsNullOrEmpty(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.PctPlannedProfit])))
                    pProfit = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.PctPlannedProfit]);
                dr8["FieldValue"] = string.Format("{0}%", pProfit);

                dt.Rows.Add(dr8);

            }
            else if (module == "CPO")
            {
                //UGITModule moduleObj = uGITCache.ModuleConfigCache.GetCachedModule(SPContext.Current.Web, "CPO");
                UGITModule moduleObj = moduleManager.LoadByName("CPO");
                if (moduleObj != null)
                {

                    List<string> viewFields = new List<string>(){
                        DatabaseObjects.Columns.Id, 
                        DatabaseObjects.Columns.TicketId, 
                        DatabaseObjects.Columns.PunchListItems, 
                        DatabaseObjects.Columns.ClientChangeOrders, 
                        DatabaseObjects.Columns.CurrentBudget, 
                        DatabaseObjects.Columns.OriginalCommitments,
                        DatabaseObjects.Columns.ProjectCost, 
                        DatabaseObjects.Columns.ProjectBillings,
                         DatabaseObjects.Columns.ActualStartDate, 
                         DatabaseObjects.Columns.RevisedEndDate, 
                         DatabaseObjects.Columns.ApproxContractValue, 
                         DatabaseObjects.Columns.PctPlannedProfit};

                    DataRow spCRMProjectitem = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(ticketID), ticketID);



                    ModuleFormLayout formLayoutItem = moduleObj.List_FormLayout.Find(x => x.FieldName == DatabaseObjects.Columns.PunchListItems);
                    if (formLayoutItem != null)
                    {

                        DataRow dr1 = dt.NewRow();
                        dr1["FieldName"] = formLayoutItem.FieldDisplayName;
                        dr1["FieldValue"] = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.PunchListItems]);
                        dt.Rows.Add(dr1);
                    }


                    formLayoutItem = moduleObj.List_FormLayout.Find(x => x.FieldName == DatabaseObjects.Columns.ClientChangeOrders);
                    if (formLayoutItem != null)
                    {
                        DataRow dr2 = dt.NewRow();
                        dr2["FieldName"] = formLayoutItem.FieldDisplayName;
                        dr2["FieldValue"] = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ClientChangeOrders]);
                        dt.Rows.Add(dr2);
                    }


                    formLayoutItem = moduleObj.List_FormLayout.Find(x => x.FieldName == DatabaseObjects.Columns.CurrentBudget);
                    if (formLayoutItem != null)
                    {
                        DataRow dr3 = dt.NewRow();
                        dr3["FieldName"] = formLayoutItem.FieldDisplayName;
                        double budget = 0.0;
                        if (spCRMProjectitem[DatabaseObjects.Columns.CurrentBudget] != null && Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.CurrentBudget]) > 0)
                        {
                            budget = Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.CurrentBudget]);
                        }

                        dr3["FieldValue"] = string.Format("{0:C0}", budget);
                        dt.Rows.Add(dr3);
                    }

                    formLayoutItem = moduleObj.List_FormLayout.Find(x => x.FieldName == DatabaseObjects.Columns.ProjectBillings);
                    if (formLayoutItem != null)
                    {
                        DataRow dr4 = dt.NewRow();
                        dr4["FieldName"] = formLayoutItem.FieldDisplayName;
                        dr4["FieldValue"] = spCRMProjectitem[DatabaseObjects.Columns.ProjectBillings] != null ?
                                            string.Format("{0:C0}", Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.ProjectBillings])) :
                                            Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ProjectBillings]);
                        dt.Rows.Add(dr4);
                    }


                    formLayoutItem = moduleObj.List_FormLayout.Find(x => x.FieldName == DatabaseObjects.Columns.ProjectCost);
                    if (formLayoutItem != null)
                    {
                        DataRow dr5 = dt.NewRow();
                        dr5["FieldName"] = formLayoutItem.FieldDisplayName;
                        dr5["FieldValue"] = spCRMProjectitem[DatabaseObjects.Columns.ProjectCost] != null ?
                                            string.Format("{0:C0}", Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.ProjectCost])) :
                                            Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ProjectCost]);
                        dt.Rows.Add(dr5);
                    }


                    DataRow dr7 = dt.NewRow();
                    dr7["FieldName"] = "Duration";
                    string duration = "0";
                    if (!string.IsNullOrEmpty(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ActualStartDate])) && !string.IsNullOrEmpty(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.RevisedEndDate])))
                        duration = Convert.ToString(uHelper.GetTotalWorkingDaysBetween(context, Convert.ToDateTime(spCRMProjectitem[DatabaseObjects.Columns.ActualStartDate]), Convert.ToDateTime(spCRMProjectitem[DatabaseObjects.Columns.RevisedEndDate])));
                    dr7["FieldValue"] = duration;
                    dt.Rows.Add(dr7);


                    formLayoutItem = moduleObj.List_FormLayout.Find(x => x.FieldName == DatabaseObjects.Columns.PctPlannedProfit);
                    if (formLayoutItem != null)
                    {
                        DataRow dr8 = dt.NewRow();
                        dr8["FieldName"] = formLayoutItem.FieldDisplayName;
                        string pProfit = "0";
                        if (!string.IsNullOrEmpty(Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.PctPlannedProfit])))
                            pProfit = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.PctPlannedProfit]);
                        dr8["FieldValue"] = string.Format("{0}%", pProfit);

                        dt.Rows.Add(dr8);
                    }
                }

            }
            else if (module == "OPM")
            {
                List<string> viewFields = new List<string>() {
                        DatabaseObjects.Columns.Id,
                        DatabaseObjects.Columns.TicketId, 
                        DatabaseObjects.Columns.ApproxContractValue, 
                        DatabaseObjects.Columns.ChanceOfSuccess, 
                        "Competition",
                        DatabaseObjects.Columns.PreconStartDate,
                        DatabaseObjects.Columns.EstimatedConstructionStart,
                        DatabaseObjects.Columns.EstimatedConstructionEnd,
                        DatabaseObjects.Columns.TicketCloseDate
                };

                DataRow spCRMProjectitem = Ticket.GetCurrentTicket(context, uHelper.getModuleNameByTicketId(ticketID), ticketID);

                if (spCRMProjectitem[DatabaseObjects.Columns.PreconStartDate] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.PreconStartDate]) != "")
                {
                    DataRow dr4 = dt.NewRow();
                    dr4["FieldName"] = "Precon Start";
                    dr4["FieldValue"] = string.Format("{0:MMM-dd-yyyy}", spCRMProjectitem[DatabaseObjects.Columns.PreconStartDate]);
                    dt.Rows.Add(dr4);
                }

                if (spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionStart] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionStart]) != "")
                {
                    DataRow dr5 = dt.NewRow();
                    dr5["FieldName"] = "Construction Start";
                    dr5["FieldValue"] = string.Format("{0:MMM-dd-yyyy}", spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionStart]);
                    dt.Rows.Add(dr5);
                }

                if (spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionEnd] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionEnd]) != "")
                {
                    DataRow dr6 = dt.NewRow();
                    dr6["FieldName"] = "Construction End";
                    dr6["FieldValue"] = string.Format("{0:MMM-dd-yyyy}", spCRMProjectitem[DatabaseObjects.Columns.EstimatedConstructionEnd]);
                    dt.Rows.Add(dr6);
                }

                if (spCRMProjectitem[DatabaseObjects.Columns.TicketCloseDate] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.TicketCloseDate]) != "")
                {
                    DataRow dr12 = dt.NewRow();
                    dr12["FieldName"] = "Project close";
                    dr12["FieldValue"] = string.Format("{0:MMM-dd-yyyy}", spCRMProjectitem[DatabaseObjects.Columns.TicketCloseDate]);
                    dt.Rows.Add(dr12);
                }

                DataRow dr1 = dt.NewRow();
                dr1["FieldName"] = "Estimated Contract Value";
                double budget = 0.0;
                if (spCRMProjectitem[DatabaseObjects.Columns.ApproxContractValue] != DBNull.Value && Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ApproxContractValue]) != "")
                {
                    if (Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.ApproxContractValue]) > 0)
                    {
                        budget = Convert.ToDouble(spCRMProjectitem[DatabaseObjects.Columns.ApproxContractValue]);
                    }
                }

                dr1["FieldValue"] = string.Format("{0:C}", budget);
                dt.Rows.Add(dr1);

                DataRow dr2 = dt.NewRow();
                dr2["FieldName"] = "Chance of Success";
                dr2["FieldValue"] = Convert.ToString(spCRMProjectitem[DatabaseObjects.Columns.ChanceOfSuccess]);
                dt.Rows.Add(dr2);



                DataRow dr3 = dt.NewRow();
                dr3["FieldName"] = "Competition";
                dr3["FieldValue"] = Convert.ToString(spCRMProjectitem["Competition"]);
                dt.Rows.Add(dr3);
            }

            grdProjectSummary.DataSource = dt;
            grdProjectSummary.DataBind();
        }
    }
}

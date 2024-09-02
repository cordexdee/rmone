<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskGraph.ascx.cs" Inherits="uGovernIT.Web.TaskGraph" %>

 <div class="col-md-12 col-sm-12 col-xs-12 summaryCPRdb_tableWrap paddingNo">
        <div class="row">
            <div class="summaryCPRdb_header">
                <asp:Label ID="lblSummaryHeader" runat="server" Text="Task Management Summary" CssClass="summaryCPRdb_title"></asp:Label>
            </div>
        </div>

       <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 paddingNo">        
                <div class="row taskManSummary_dataRow">
                    <div class="col-md-6 col-sm-6 col-xs-6 taskSummary_lable">
                        No of overdue tasks
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 adjustableWidth" style="width:49%;">
                        <div style="background-color:#EB517A;width:<% =pctOverdue%>%;text-align: center;margin-top:5px; border-radius:10px;"><asp:Label ID="lblOverdueTasks"  runat="server" Text="0"></asp:Label></div>
                    </div>
                </div>
                <div class="row taskManSummary_dataRow">
                    <div class="col-md-6 col-sm-6 col-xs-6 taskSummary_lable">
                       No of pending tasks
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 adjustableWidth" style="width:49%;">
                        <div style="background-color:#4A6EE2;width:<% =pctPending%>%;text-align: center;margin-top:5px; border-radius:10px;""> <asp:Label ID="lblPendingTasks"  runat="server" Text="0"></asp:Label></div>
                    </div>
                </div>
                <div class="row taskManSummary_dataRow">
                    <div class="col-md-6 col-sm-6 col-xs-6 taskSummary_lable">
                       No of completed tasks
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6 adjustableWidth" style="width:49%;">
                        <div style="background-color:#45B493;width:<% =pctCompleted%>%;text-align: center;margin-top:5px; border-radius:10px;""> <asp:Label ID="lblCompletedTasks"  runat="server" Text="0"></asp:Label></div>
                    </div>
                </div>
            </div>
        </div>
   </div>

     

    
       

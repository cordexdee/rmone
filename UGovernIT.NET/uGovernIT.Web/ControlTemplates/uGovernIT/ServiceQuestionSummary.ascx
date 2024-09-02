<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceQuestionSummary.ascx.cs" Inherits="uGovernIT.Web.ServiceQuestionSummary" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<link href="<%= ResolveUrl(@"~/Content/uGITWizard.css") + "?v=" + UGITUtility.AssemblyVersion %>" rel="stylesheet" />
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .col_b {
        width: 100% !important;
    }
    
</style>
<%if (rSummaryTable.Items.Count <= 0)
    {%>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ques-answer {
        background-color: #eeeeee !important;
        text-align: center !important;
    }
</style>
<%} %>
<%--<asp:Panel ID="pStep2SectionSContainer" runat="server" CssClass="col_b">
    <div id="parentdiv" runat="server" class="ques-answer">
        <asp:Repeater runat="server" ID="rSummaryTable" OnItemDataBound="RSummaryTable_ItemDataBound">
            <ItemTemplate>
                <div class="summary-section">
                    <b>&nbsp;&nbsp;&nbsp;Section:
                        <asp:Label ID="lbSection" runat="server"></asp:Label></b>
                </div>
                <div class="summary-sectiondetail">
                    <asp:Repeater runat="server" ID="rSummaryioQuest" OnItemDataBound="RSummaryioQuest_ItemDataBound">
                        <ItemTemplate>
                            <div class="question" runat="server" id="summaryQuestionDiv">
                                <span>
                                    <b class="questionlb"></b>
                                    <span class="qwidth">
                                        <asp:Label ID="lbQuestion" runat="server"></asp:Label>
                                    </span>
                                </span>
                                <div class="select_product">
                                    <asp:Label ID="lbQuestionVal" runat="server"></asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    
</asp:Panel>--%>


    <asp:Panel ID="pStep2SectionSContainer" runat="server" CssClass="col_b">
        <div id="parentdiv" runat="server" class="ques-answer">
            <asp:Repeater runat="server" ID="rSummaryTable" OnItemDataBound="RSummaryTable_ItemDataBound">
            <ItemTemplate>
                <div class="summary-section">
                    <b>&nbsp;&nbsp;&nbsp;Section: <asp:Label ID="lbSection" runat="server"></asp:Label></b>
                </div>
                <div class="summary-sectiondetail">
                    <asp:Repeater runat="server" ID="rSummaryioQuest" OnItemDataBound="RSummaryioQuest_ItemDataBound">
                        <ItemTemplate>
                            <div class="question" runat="server" id="summaryQuestionDiv">
                                <span>
                                    <b class="questionlb"></b>
                                    <span class="qwidth">
                                        <asp:Label ID="lbQuestion" runat="server"></asp:Label>
                                    </span>
                                </span>
                                <div class="select_product">
                                    <asp:Label ID="lbQuestionVal" runat="server"></asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
            <%--<asp:Repeater runat="server" ID="rSummaryTable" OnItemDataBound="RSummaryTable_ItemDataBound">
                <ItemTemplate>                    
                    <div class="summaryTab_xsNoPadding col-xl-6 col-12 col-md-<%=summaryColWidth%>">
                        <div class="info-content-wrap ml-3 mb-5">
                            <div class="info-subtitle">                                
                                <h3><asp:Label ID="lbSection" runat="server"></asp:Label></h3>
                            </div>
                            <div class="row">
                                <asp:Repeater runat="server" ID="rSummaryioQuest" OnItemDataBound="RSummaryioQuest_ItemDataBound">
                                    <ItemTemplate>
                                        <panel runat="server" id="summaryQuestionDiv" class="col-md-6 question-wrap">
                                            <span>
                                                <span class="question-name">
                                                    <asp:Label ID="lbQuestion" runat="server"></asp:Label>
                                                </span>
                                            </span>
                                            <div class="question-value">
                                                <asp:Label ID="lbQuestionVal" runat="server"></asp:Label>
                                            </div>
                                        </panel>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>                
            </asp:Repeater>--%>
        </div>
    </asp:Panel>






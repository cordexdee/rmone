<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExecutiveSummary.ascx.cs" Inherits="uGovernIT.Web.ExecutiveSummary" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/HtmlEditorControl.ascx" TagPrefix="ugit" TagName="HtmlEditorControl" %>


<div id="projectStatus" class="project-status-container row">
            <div style="width: 100%;">
                <div id="trShowLatestSummary" runat="server" class="ms-alternatingstrong">
                    <div class="project-status-header"  style="padding: 10px 6px;">
                        <div style="width: 100%;">
                            <div class="row">
                                <div class="pmmStatus_lable">
                                    <b>Executive Summary:&nbsp;</b>
                                </div>
                                <div class="pmmStatus_editIcon">
                                    <ugit:HtmlEditorControl runat="server" ID="HtmlEditorControl" isHideInlineHtml="true"
                                        PopupTitle="Executive Summary" />
                                </div>
                                <div class="pmmStatus_summaryHistory">
                                    <asp:Image CssClass="btprojectsummaryhistory" ID="btProjectSummaryHistory" runat="server" ImageUrl="/content/images/summaryHistory.png" ToolTip="Summary History" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="trAddSummaryBox" class="row" runat="server" visible="false">
                    <div>
                        <div style="float: left; width: 98%;">
                            <asp:TextBox ID="txtProjectSummary" runat="server" TextMode="MultiLine" Rows="4" Width="99.5%"></asp:TextBox>
                        </div>
                        <div style="float: left; width: 98%;">
                            <asp:LinkButton ID="btCancelAddSummary" Visible="true" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;"
                                            ToolTip="Cancel" Style="float: right; padding-top: 10px;" OnClick="btCancelAddSummary_Click">
                                <span class="button-bg">
                                 <b style="float: left; font-weight: normal;">
                                 Cancel</b>
                                 <i
                                style="float: left; position: relative; top: -2px;left:2px">
                                <img src="/Content/ButtonImages/cancel.png"  style="border:none;" title="" alt=""/></i> 
                                 </span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btAddProjectSummary" Visible="true" runat="server" Text="&nbsp;&nbsp;Submit&nbsp;&nbsp;"
                                        ToolTip="Save" Style="float: right; padding-top: 10px;" OnClick="btAddProjectSummary_Click1">
                            <span class="button-bg">
                                <b style="float: left; font-weight: normal;">
                                Save</b>
                                <i style="float: left; position: relative; top: -2px;left:2px">
                                <img src="/Content/ButtonImages/save.png"  style="border:none;" title="" alt=""/></i> 
                                </span>
                        </asp:LinkButton>
                        </div>
                    </div>
                </div>
                <div id="tr1" runat="server" class="row ms-alternatingstrong">
                    <div align="left" valing="top" style="padding: 15px;">
                        <span style="float: left; width: 97%;">
                            <%--<b>Executive Summary:&nbsp;</b>--%>
                            <asp:Literal ID="lbReadOnlyStatusSummary" runat="server" Text="No summary entered"></asp:Literal>
                        </span>
                    </div>
                </div>
            </div>
        </div>

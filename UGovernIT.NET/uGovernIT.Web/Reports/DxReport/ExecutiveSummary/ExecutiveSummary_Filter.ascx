<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExecutiveSummary_Filter.ascx.cs" Inherits="uGovernIT.DxReport.ExecutiveSummary_Filter" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<style type="text/css">
    /*#wrapper_1 { clear: left; float: left; position: relative; left: 50%; }
    #container_1 { display: block; float: left; position: relative; right: 50%; }*/
</style>

<script type="text/javascript">
    function GetCoreServiceReportPopup(obj) {
        //debugger;
        LoadingPanel.Show();
        var moduleName = "<%= ModuleName %>";
        var type = $('#<%=rbReportType.ClientID %> input:checked').val()
        var url = '<%= CoreServiceReportURL %>' + '&moduleName=' + moduleName + '&Type=' + type;
        window.location.href = url;
        return false;
    }
</script>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." CssClass="customeLoader" ClientInstanceName="LoadingPanel" Image-Url="~/Content/IMAGES/ajax-loader.gif" ImagePosition="Top"
    Modal="True">
</dx:ASPxLoadingPanel>

<div id="wrapper_1" class="col-md-12 col-sm-12 col-xs-12">
    <div id="container_1">
        <div id="dvCoreServiceReport" class="projectsummary-reportWrap">
            <fieldset class="projectsummary-reportContainer">
                <legend class="summary-reportHeading excutive-sumReport-heading">Executive Summary</legend>
                <div class="row">
                        <div style="padding:0 0 15px;">
                            <asp:Label ID="Label4" runat="server" Text="Type" CssClass="summary-reportLabel"> </asp:Label>
                        </div>
                        <div class="col-md-2 col-sm-3 col-xs-12 noPadding">
                            <asp:RadioButtonList ID="rbReportType" runat="server" Width="100%" RepeatDirection="Horizontal" CssClass="reportRadio-btnWrap">
                                <asp:ListItem Text="Summary" Value="Summary" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Detailed" Value="Detailed"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                   </div>
                    <div class="row summaryReport-btnWrap">
                        <div class="summaryReport-btnContainer col-md-2 col-sm-3 col-xs-12 noPadding">
                            <ul class="summaryReport-ul">
                                <li runat="server" id="Li2">
                                    <%--<asp:LinkButton runat="server" ID="lnkCRRP" Style="color: white" Text="Build Report" CssClass="ganttImg" OnClientClick="javascript:return GetCoreServiceReportPopup(this);" />--%>
                                    <dx:ASPxButton ID="LinkButton2" runat="server" Text="Build Report" AutoPostBack="false" CssClass="buildReport-btn">
                                        <ClientSideEvents Click="function(s,e){GetCoreServiceReportPopup(s);}" />
                                    </dx:ASPxButton>
                                </li>
                                <li runat="server" id="Li1">
                                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false" OnClick="btnCancel_Click" CssClass="cancelReport-btn">
                                    </dx:ASPxButton>
                                </li>
                                
                            </ul>
                        </div>
                    </div>
            </fieldset>
        </div>
    </div>
</div>


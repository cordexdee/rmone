<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GovernanceReview.ascx.cs" Inherits="uGovernIT.Web.GovernanceReview" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css">
    fieldset {
        border: 1px solid silver;
        margin: 0 2px;
        padding: 0.35em 0.625em 0.75em;
    }

    legend {
        display: unset;
        width: unset;
        padding: unset;
        margin-bottom: unset;
        font-size: unset;
        line-height: unset;
        color: black;
        border: unset;
        border-bottom: unset;
    }
    .action-container {
        background: none repeat scroll 0 0 #FFFFAA;
        border: 1px solid #FFD47F;
        float: left;
        padding: 1px 5px 0;
        position: absolute;
        z-index: 800;
        margin-top: -16px;
        margin-left: 3px;
        left: 5px;
    }

    .hidenew {
        display: none;
    }
    
    .makeMeHidden {
        display: none;
    }
</style>

<script type="text/javascript">
    //Action button hide show: start
    function showTasksActions(trObj) {
    
        $(trObj).find(".action-container").show();
    }

    function hideTasksActions(trObj) {
        $(trObj).find(".action-container").hide();
    }
    //Action button hide show: end

    function actionBtnHandle(s, moduleName, action, title, ticketPublicID) {
         
        var param = "moduleName=" + moduleName + "&command=" + action + "&ticketPublicId=" + ticketPublicID;
        var width = "450px";
        var height = "250px";
        if (action == "approve") {
            width = "390px";
            height = "150px";
        }
        window.parent.UgitOpenPopupDialog('<%=TicketActionUrl%>', param, ticketPublicID + ':' + title, width, height, 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
</script>

 <fieldset id="fldsetNPRResource" runat="server">
                    <legend style="font-weight: bold;">Pending Review</legend>
    <asp:Panel runat="server" ID="portfolioContainer" CssClass="MaxHeight" Width="100%">
        <dx:ASPxGridView ID="itgGrid" EnableCallBacks="true" runat="server" AutoGenerateColumns="False" Images-HeaderActiveFilter-Url="/Content/images/Filter_Red_16.png"
              ClientInstanceName="itgGridClientInstance" OnDataBinding="itgGrid_DataBinding" OnHtmlRowPrepared="itgGrid_HtmlRowPrepared"
            Theme="DevEx" Width="100%" SettingsText-EmptyHeaders="&nbsp;" KeyFieldName="TicketId">
            <Columns>
            </Columns>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong" AlternatingRow-Enabled="True" Footer-Font-Bold="true"></Styles>

            <SettingsLoadingPanel Mode="ShowAsPopup" />
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
            <SettingsBehavior AllowSort="true" AllowSelectByRowClick="false" ProcessSelectionChangedOnServer="true" />
            <Settings ShowFooter="false" ShowHeaderFilterButton="true" />

            <ClientSideEvents />

        </dx:ASPxGridView>
    </asp:Panel>
</fieldset>
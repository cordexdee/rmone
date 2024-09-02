<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateProjectTags.ascx.cs" Inherits="uGovernIT.Web.CreateProjectTags" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style>
    .crm-checkWrap {
        display: flex;
        margin-top: 2px;
    }
        .crm-checkWrap label::before {
            content: "";
            appearance: none;
            background-color: transparent;
            box-shadow: none;
            display: inline-block;
            position: relative;
            vertical-align: middle;
            cursor: pointer;
            margin-right: 0px !important;
            border: none;
            padding: 0px !important;
        }

    .ms-formlabel {
        white-space: nowrap;
        font-weight: normal;
        padding-left: 0px !important;
    }
    .crmLbl {
        font-size: 13px;
        font-family: 'Roboto', sans-serif !important;
        font-weight: 500;
        color: #4b4b4b;
    }

    .addEditPopup-btnWrap {
        float: right;
        text-align: right;
        padding: 0px;
    }

    .dxichCellSys{
        padding:0px !important;
    }
    
    .dxlpLoadingPanel_UGITNavyBlueDevEx
    .dxlp-loadingImage, 
    .dxcaLoadingPanel_UGITNavyBlueDevEx 
    .dxlp-loadingImage, 
    .dxlpLoadingPanelWithContent_UGITNavyBlueDevEx 
    .dxlp-loadingImage,
    .dxeImage_UGITNavyBlueDevEx.dxe-loadingImage {
        background-image: url(/Content/Images/ajaxloader.gif);
        height: 32px;
        width: 32px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var allowServerClick = false;
    function createTags(s, e) {
        e.processOnServer = allowServerClick;
        if (allowServerClick == false) {
            var result = DevExpress.ui.dialog.confirm("<i>Are you sure you want to delete existing and create New Tags?</i>", "Warning!");
            result.done(function (dialogResult) {
                if (dialogResult) {
                    allowServerClick = true;
                    waitPanel.Show();
                    btnNewTags.DoClick();
                } else {
                    allowServerClick = false;
                    return false;
                }
            });
        }
    }
    function updateTags(s, e) {
        e.processOnServer = allowServerClick;
        if (allowServerClick == false) {
            var result = DevExpress.ui.dialog.confirm("<i>Are you sure you want to update existing Tags?</i>", "Warning!");
            result.done(function (dialogResult) {
                if (dialogResult) {
                    allowServerClick = true;
                    waitPanel.Show();
                    btnUpdateTags.DoClick();
                } else {
                    allowServerClick = false;
                    return false;
                }
            });
        }
    }
</script>

<dx:ASPxLoadingPanel ID="waitPanel" ClientInstanceName="waitPanel" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup ">
        <div class="row mt-2">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel" style="white-space: pre-line; line-height: 17px;">
                    <asp:Label ID="lblMsg" runat="server"> </asp:Label></h3>
            </div>
            <div class="row" id="divModuleSelector" runat="server" style="padding-top: 15px" visible="true">
                Select Modules: 
                <div class="crm-checkWrap">
                    <asp:CheckBox ID="chkCPR" Text="CPR"  runat="server" Checked="true"/>
                    <asp:CheckBox ID="chkCNS" Text="CNS" runat="server" />
                    <asp:CheckBox ID="chkOPM" Text="OPM" runat="server" />
                </div>
            </div>
            <div class="row" id="divRequestTypeSelector" runat="server" style="padding-top: 15px" visible="true">
                Select Request Type: 
                <div class="crm-checkWrap">
                    <asp:CheckBox ID="chkOpenRecords" Text="Open"  runat="server" Checked="true"/>
                    <asp:CheckBox ID="chkClosedRecords" Text="Closed" runat="server" Checked="false" />
                </div>
            </div>
        </div>
        <div style="text-align: center !important; padding-top: 25px !important; font-family: Roboto sans-serif !important; font-size: 15px !important; font-weight: bold !important;">
            <dx:ASPxLabel runat="server" ID="refreshData" ClientInstanceName="refreshData"></dx:ASPxLabel>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnNewTags" runat="server" CssClass="primary-blueBtn" Text="Delete Existing and Create New Tags" ToolTip="Delete Existing and Create New Tags"
                ClientInstanceName="btnNewTags" OnClick="btnNewTags_Click">
                <ClientSideEvents Click="function(s, e){return createTags(s,e);}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnUpdateTags" runat="server" Text="Update Existing Tags" CssClass="primary-blueBtn" ToolTip="Update Existing Tags" ClientInstanceName="btnUpdateTags" OnClick="btnUpdateTags_Click">
                <ClientSideEvents Click="function(s, e){return updateTags(s,e);}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="Cancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="Cancel_Click"></dx:ASPxButton>
        </div>
    </div>
</div>

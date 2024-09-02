

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadTaskTemplate.ascx.cs" Inherits="uGovernIT.Web.LoadTaskTemplate" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .pctcomplete {
        text-align: right;
    }

    .estimatedhours {
        text-align: right;
    }

    .actualhours {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .ms-formlabel {
        width: 100px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .fleft {
        float: left;
    }

    .proposeddatelb {
        padding-top: 5px;
        padding-right: 4px;
        float: left;
    }

    .warning-message {
        float: left;
        padding: 7px 0px;
    }
</style>


<table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
    width="100%">
    <tr>
        <td colspan="2">
            <asp:Label CssClass="warning-message" ForeColor="Red" ID="lbMessage" runat="server"></asp:Label>
        </td>
    </tr>

    <tr>
        <td colspan="2">
            <ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="grid"
                 Width="100%" KeyFieldName="ID">

                <Columns></Columns>
                <SettingsBehavior AutoExpandAllGroups="true" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <Styles>
                    <GroupRow Font-Bold="true"></GroupRow>
                    <SelectedRow BackColor="#AAD4FF"></SelectedRow>
                </Styles>
                <SettingsPager Position="TopAndBottom" PageSize="15">
                    <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
                </SettingsPager>
            </ugit:ASPxGridView>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="right" style="padding-top: 5px;">
            <div style="float:right;">
                       
        <a href="javascript:void(0);" onclick="window.frameElement.commitPopup();"
                title="Cancel" Style="float: right; padding-top: 10px;">
                <span class="button-bg">
                    <b style="float: left; font-weight: normal;">
                    Cancel</b>
                    <i
                style="float: left; position: relative; top: -2px;left:2px">
                <img src="/Content/ButtonImages/cancel.png"  style="border:none;" title="" alt=""/></i> 
                    </span>
                </a>

        <asp:LinkButton ValidationGroup="Task" ID="btLoadFromTemplate" Visible="true" runat="server" Text="&nbsp;&nbsp;Load From Template&nbsp;&nbsp;"
        ToolTip="Load From Tempate" Style="float: right; padding-top: 10px; " OnClick="btLoadFromTemplate_Click">
        <span class="button-bg">
            <b style="float: left; font-weight: normal;">
            Load</b>
            <i
        style="float: left; position: relative; top: -2px;left:2px">
        <img src="/Content/ButtonImages/save.png"  style="border:none;" title="" alt=""/></i> 
            </span>
            </asp:LinkButton>

            </div>
        </td>
    </tr>
</table>

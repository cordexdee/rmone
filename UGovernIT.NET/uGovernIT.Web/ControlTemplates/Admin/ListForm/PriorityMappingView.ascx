<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriorityMappingView.ascx.cs" Inherits="uGovernIT.Web.PriorityMappingView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .tdrowhead {
        font-weight: bold;
        padding: 0px;
        text-align: center;
        background-color: #F2F2F2;
        color: #3E4F50;
    }

    .canvascell_0 {
        /*background-image:url('/_layouts/15/images/uGovernIT/pm_gridtopcorner.png');*/
        background-color: #F2F2F2;
        height: 32px;
        width: 107px;
        /*border:1px solid #d3d3d3;*/
    }

    
    .divcell_0 {
        color: #F2F2F2;
        width: 107px;
    }

    .cell_0_0 {
        background-color: #3E4F50;
        padding: 0px;
        height:32px;
        width:107px;
    }
    .header {
        padding: 0px 9px;
        font-weight: bold;
        text-align: left;
        background-color: #F2F2F2;
        color: #3E4F50;
        position: relative !important;
        width: auto !important;
    }

   /*#gridview th {
        font-weight: bold;
        padding: 0px;
        text-align: left;
        background-color: #F2F2F2;
        color: #3E4F50;
    }*/
    #buttontd ul li {
        color: #FFFFFF;
        cursor: pointer;
        display: inline;
        float: left;
        list-style: none outside none;
        margin: 0 1px;
        overflow: hidden;
        padding: 0;
        text-align: center;
        width: auto;
    }
    
    #gridview td, #gridview th {
        border: 1px solid #3E4F50;
    }
    a:hover {
        text-decoration:underline;
    }
    #gridview td {
        padding:3px !important;
    }
</style>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" id="header">
        <div class="formLayout-dropDownWrap col-md-6 col-sm-6 col-xs-6 noLeftPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModule" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" CssClass="itsmDropDownList aspxDropDownList">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noRightPadding" style="padding-bottom:5px">
            <div class="headerContent-right">
                <div class="menuNav-applyChngBtn" style="padding-top:20px !important;">
                    <dx:ASPxButton ID="btnApplyChanges" CausesValidation="false" Text="Apply Changes" runat="server" OnClick="btnApplyChanges_Click"
                        CssClass="primary-blueBtn">
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>

    <div class="row" id="content">
        <table style="width:100%;">
        <tbody>
            <tr>
                <td colspan="2" id="gridview">
                    
                    <asp:GridView ID="gridPriority" runat="server" EnableModelValidation="True" ForeColor="#333333" Width="100%" GridLines="Both"
                        OnRowDataBound="gridPriority_RowDataBound" CellPadding="4" BorderColor="#3E4F50">
                        <HeaderStyle BackColor="#F2F2F2" Font-Bold="True" ForeColor="#3E4F50" />
                        <RowStyle BackColor="#FFFFFF" ForeColor="#3E4F50" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:ValidationSummary ID="vsmessage" runat="server" ValidationGroup="save" DisplayMode="SingleParagraph" EnableClientScript="false" /> 
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Blue"></asp:Label>
                </td>
                <td id="buttontd">
                    <div style="padding-top: 15px; float: right;" >
                         <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click">
                         </dx:ASPxButton>
                         <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn">
                         </dx:ASPxButton>
                    </div>
                </td>

            </tr>
        </tbody>
    </table>
    </div>
</div>

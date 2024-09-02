<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportCheckListTemplate.ascx.cs" Inherits="uGovernIT.Web.ImportCheckListTemplate" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    body {
        overflow-y: auto !important;
    }

    #s4-leftpanel {
        display: none;
    }

    .s4-ca {
        margin-left: 0px !important;
    }

    #s4-ribbonrow {
        height: auto !important;
        min-height: 0px !important;
    }

    #s4-ribboncont {
        display: none;
    }

    #s4-titlerow {
        display: none;
    }

    .s4-ba {
        width: 100%;
        min-height: 0px !important;
    }

    #s4-workspace {
        float: left;
        width: 100%;
        overflow: auto !important;
    }

    body #MSO_ContentTable {
        min-height: 0px !important;
        position: inherit;
    }

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

    /*.ms-formlabel {
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        width: 160px;
    }*/

    .ms-standardheader {
        text-align: right;
    }

    .fleft {
        float: left;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">    
    function ValidateChecklisttemplate() {
        var ddlCount = document.getElementById("<%=ddlCheckListTemplate.ClientID%>").length;
        if (ddlCount <= 0) {
            alert('Checklist Templates not available for this module.')
            return false;
        }

        return true;
    }
</script>

<div id="tb1" class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12 noPadding" >
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding" id="trModule" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">CheckList Template<b style="color: Red;">*</b>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlCheckListTemplate" runat="server" AppendDataBoundItems="true"
                    CssClass="tsmDropDownList + aspxDropDownList" >
                </asp:DropDownList>

                <asp:RequiredFieldValidator InitialValue="0" ID="ReqCheckListTemplate" Display="Dynamic" runat="server" ControlToValidate="ddlCheckListTemplate"
                     Text="Field Required." ErrorMessage="ErrorMessage"></asp:RequiredFieldValidator>
            </div>
        </div> 
    </div>
    <div class="row addEditPopup-btnWrap">
        <dx:ASPxButton runat="server" ID="btnCancel" OnClick="btnCancel_Click" AutoPostBack="true" Text="Cancel"
            CssClass="secondary-cancelBtn">
		</dx:ASPxButton>
		<dx:ASPxButton runat="server" ID="btnImportCheckListTemplate" OnClick="btnImportCheckListTemplate_Click" AutoPostBack="true" Text="Import CheckList Template" 
            ToolTip="Import CheckList Template"  CssClass="primary-blueBtn">
			<ClientSideEvents Click="function(s,e){return ValidateChecklisttemplate();}" />
		</dx:ASPxButton>
    </div>
</div>
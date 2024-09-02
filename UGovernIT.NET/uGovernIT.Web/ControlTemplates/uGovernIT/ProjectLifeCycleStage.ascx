<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectLifeCycleStage.ascx.cs" Inherits="uGovernIT.Web.ProjectLifeCycleStage" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%--<%@ Register Src="~/ControlTemplates/Shared/LifeCycleGUI.ascx" TagPrefix="uc1" TagName="LifeCycleGUI" %>--%>
<%@ Import Namespace="uGovernIT.Utility" %>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Please Wait ..." ClientInstanceName="LoadingPanel" Modal="True">
</dx:ASPxLoadingPanel>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .ms-panelcontainerText {
        font-family: Verdana,sans-serif;
        font-size: 12px;
        text-align: center;
        height: 100px;
        border-width: 1px;
        border-bottom-style: solid;
        border-left-style: solid;
        border-right-style: solid;
        border-color: lightgray;
    }

    .checkboxstyle input {
        margin-left: 0px !important;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function confirmTaskDeleteMessage() {
        var status = $('#<%= chkResetProjectPlantoDefault.ClientID %>').is(':checked');
        var message;
        if (status)
            message = "This will DELETE all tasks for this project.\nThis will also delete ALL resource allocations for this project.\n\nAre you sure want to proceed?";
        else
            message = "This will change the project lifecycle and remove any links from the tasks to the stages.\n\nAre you sure want to proceed?";
        if (confirm(message)) {

            LoadingPanel.Show();
            return true;
        }
        else {
            return false;
        }
    }
</script>

<asp:Panel ID="pLifeCycleStageGUI" runat="server" class="pmmitem-container2 pmmProjectLifeCycle_container"></asp:Panel>

<br />
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup ">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Project Lifecycle:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlLifeCycleModel" runat="server" OnSelectedIndexChanged="ddlLifeCycleModel_SelectedIndexChanged" 
                    AutoPostBack="true" OnInit="ddlLifeCycleModel_Init" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
            </div>
        </div>
        <div class="row" id="trResetProjectPlantoDefault">
            <%--<div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Reset Project Plan to Default:</h3>
            </div>--%>
            <div class="ms-formbody accomp_inputField crm-checkWrap">
                <asp:CheckBox ID="chkResetProjectPlantoDefault" runat="server" Text="Reset Project Plan to Default" Checked="false" CssClass="checkboxstyle" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap" id="tr2" runat="server">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn" ToolTip="Cancel" 
                CausesValidation="false"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" CssClass="primary-blueBtn" runat="server" Text="Save" ToolTip="Save" OnClick="btnSave_Click" >
                <ClientSideEvents Click="confirmTaskDeleteMessage" />
            </dx:ASPxButton>
        </div>
    </div>
</div>

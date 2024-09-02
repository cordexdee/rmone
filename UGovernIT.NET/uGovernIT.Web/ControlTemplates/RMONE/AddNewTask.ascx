<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddNewTask.ascx.cs" Inherits="uGovernIT.Web.AddNewTask" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">

    .CRMDueDate_inputField tr td {
        padding: 3px 8px 3px 4px !important;
    }

    .ms-formlabel {
        text-align: right;
        width: 170px;
        vertical-align: top;
        padding-top: 7px;
    }
    /*.ms-standardheader 
    {
        text-align: right;
        padding-left: 9px;
    }*/
    .ms-standardheader .lblsubitem {
        font: inherit;
    }

    .width25 {
        width: 25px;
        text-align: right
    }

    .rightpos {
        /*float:right;*/
    }

    .allocationmargin {
        /*margin-left: 10px;*/
    }

    .dxeEditAreaSys {
        color: inherit;
    }

    .dxeCalendarFooter_UGITNavyBlueDevEx {
        padding-left: 67px;
    }

    body input.dxeEditArea_UGITNavyBlueDevEx {
        color: inherit;
    }
    .dxICheckBox_CustomImage {
        margin: 1px;
    }
    .dxICBFocused_CustomImage {
        margin: 0px;
        border: 1px dotted Orange;
    }
    .budget_fieldLabel {
        color: black;
    }
    .accomp_inputField {
        color:black;
    }
    input[type=password][disabled], input[type=text][disabled], input[type=file][disabled], textarea[disabled], select[disabled], .sp-peoplepicker-topLevelDisabled, .ms-inputBoxDisabled {
    background-color: #f2f2f2 !important;
    }

</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function CreateNewTask(s, e) {
        var moduleName = $("#<%=ddlLevel1.ClientID %>").val();
        var ticketID = cbLevel2.GetValue();
        var params = "module=" + moduleName + "&ticketId=" + ticketID + "&conditionType=ModuleTaskCT&ID=0&type=NewConstraint";
        window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl?control=modulestagetask', params, 'New Task', '700px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
</script>

        <div style="width: 100%;" class="AddTask">
        <div class="ms-formtable row accomp-popup">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlLevel1" runat="server" OnLoad="ddlLevel1_Load" AutoPostBack="true" CssClass="aspxDropDownList" OnSelectedIndexChanged="ddlLevel1_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="workitem" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Work Item<b style="color: Red;">*</b>
                    </h3>

                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxComboBox ID="cbLevel2"  runat="server" AutoPostBack="true" ClientInstanceName="cbLevel2"
                        DropDownStyle="DropDownList" ValueField="LevelTitle" TextField="LevelTitle" TextFormatString="{0}"
                        ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True"
                        CallbackPageSize="10" CssClass="comboBox-dropDown CRMDueDate_inputField" PopupHorizontalAlign="RightSides">
                        <Columns></Columns>
                    </dx:ASPxComboBox>
                </div>
            </div>
            <div class="row fieldWrap" id="tr4" runat="server">
                <div class="ms-formlabel errorMsg-wrap">
                    <asp:Label ID="lbMessage" runat="server" Text="" Visible="true" CssClass="error-msg" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <div class="row addEditPopup-btnWrap">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" AutoPostBack="true" OnClick="btnCancel_Click" />                </dx:ASPxButton>
                <dx:ASPxButton ID="btnCreate" runat="server" Text="Create" ToolTip="Create New Task" ValidationGroup="Save" CssClass="primary-blueBtn" >
                    <ClientSideEvents Click="function(s, e){ CreateNewTask(); }" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>
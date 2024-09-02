<%--<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>--%>
<%--<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>--%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedCompanyAddEdit.ascx.cs" Inherits="uGovernIT.Web.RelatedCompanyAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        padding: 3px 6px 4px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function cmbCompany_SelectedIndexChanged(s, e) {
        CallbackPanel.PerformCallback(s.GetValue());
    }

    function GridLookup_EndCallback(s, e) {
        LoadingPanel.Hide();
    }

    function Panel_EndCallback(s, e) {
        LoadingPanel.Hide();
    }

    function closeFrame() {
        window.frameElement.commitPopup();
    }
    $(document).ready(function () {
        $('.fetch-popupParent').parent().addClass('popupUp-mainContainer');
    });
</script>
<%--<script type="text/javascript">
    function OnDropDown(comboBox) {
        SetDropDownWidth(comboBox, "256px");
    }
    function SetDropDownWidth(comboBox, width) {
        var listBox = comboBox.GetListBoxControl();
        var scrollDiv = listBox.GetScrollDivElement();
        scrollDiv.style.overflowX = "auto";
        scrollDiv.style.width = width;

        var popupControl = comboBox.GetPopupControl();
        popupControl.SetSize("0", "0");
    }
    </script>--%>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True"></dx:ASPxLoadingPanel>

<div class="col-md-12 col-sm-12 col-xs-12  fetch-popupParent" style="width: 100%; padding-left: 5px;">
    <fieldset class="externalTeam-content">
        <legend class="activity-title">Project Directory</legend>
        <dx:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="CallbackPanel" OnCallback="ASPxCallbackPanel1_Callback">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent3" runat="server">
                    <div class="ms-formtable accomp-popup row">
                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader popupField-label">Company</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField" id="reltedCom_dropDown">
                                <dx:ASPxComboBox ID="cmdCompany" runat="server" Width="100%" ValueType="System.String" ListBoxStyle-CssClass="aspxComboBox-listBox"
                                    IncrementalFilteringMode="StartsWith" DropDownStyle="DropDownList" ClientSideEvents-SelectedIndexChanged="cmbCompany_SelectedIndexChanged"
                                    CssClass="aspxComBox-dropDown" DropDownRows="5" ItemStyle-Wrap ="True">
                                    <ClientSideEvents DropDown="function(s, e) { OnDropDown(s); }" />
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader popupField-label">Contact</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <dx:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple" ClientInstanceName="gridLookup"
                                    KeyFieldName="TicketId" TextFormatString="{0}" MultiTextSeparator=", " OnDataBinding="GridLookup_DataBinding" 
                                    GridViewProperties-EnableCallBacks="true" CssClass="CRMDueDate_inputField dropDown-feild"
                                    GridViewStyles-FilterRow-CssClass="lookupDropDown-filterWrap" GridViewStyles-Row-CssClass="lookupDropDown-contentRow" 
                                    DropDownWindowStyle-CssClass="aspxGridLookup-dropDown">
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True"/>
                                        <dx:GridViewDataColumn FieldName="Title"  Width="210px"/>   
                                    </Columns>
                                    <GridViewProperties>
                                        <Settings ShowFilterRow="True" ShowStatusBar="Hidden" ShowColumnHeaders="false" VerticalScrollBarMode="Visible" 
                                            VerticalScrollableHeight="100"/>
                                    </GridViewProperties>
                                    <ClientSideEvents EndCallback="GridLookup_EndCallback" />
                                </dx:ASPxGridLookup>
                            </div>
                        </div>

                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader popupField-label">Type</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <dx:ASPxComboBox ID="cmbType" runat="server" ValueType="System.String" 
                                    IncrementalFilteringMode="StartsWith" DropDownStyle="DropDownList" CssClass="CRMDueDate_inputField dropDown-feild">
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="row addEditPopup-btnWrap">
                        <dx:ASPxButton ID="LnkbtnDelete" runat="server" CssClass="secondary-cancelBtn" Text="Delete" ToolTip="Delete" OnClick="LnkbtnDelete_Click">
                            <ClientSideEvents Click="function(){return confirm('Are you sure you want to delete?');}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnCancel" CssClass="secondary-cancelBtn"  Text="Cancel" runat="server" ToolTip="Cancel" 
                            OnClick="btnCancel_Click"></dx:ASPxButton>
                        <dx:ASPxButton ID="btnSave" runat="server" CssClass="primary-blueBtn" Text="Save" ToolTip="Save" ValidationGroup="SaveContact" OnClick="btnSave_Click"></dx:ASPxButton>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="Panel_EndCallback" />
        </dx:ASPxCallbackPanel>
    </fieldset>
    <%--<div class="externalTeam-popupBtn">
        <div class="activityDelete_btnWrap">
            <asp:LinkButton  Text="&nbsp;&nbsp;&nbsp;&nbsp;" 
                OnClientClick="" >
                    <span class="activityDelete_btn">
                        <b>Delete</b>
                    </span>
            </asp:LinkButton>
        </div>
         <div class="CancelBtn_wrap">
            <asp:LinkButton >
                <span class="activityCancel_btn">
                    <b>Cancel</b>
                </span>
            </asp:LinkButton>
        </div>
        <div class="activitySave_btnWrap">
            <asp:LinkButton >
                <span class="activitySave_btn">
                    <b>Save</b>
                </span>
            </asp:LinkButton>
        </div>
    </div>--%>
</div>

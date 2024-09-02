
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectInitView.ascx.cs" Inherits="uGovernIT.Web.ProjectInitView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function hidepopup(s, e) {
        addnewstrategy.Hide();
    }
    function showpopup() {
        addnewstrategy.Show();
        return false;
    }
    function showeditpopup(key) {
        
        if (key != "") {
            hdnBSKey.Set("BSKey", key);
            ccpEditBS.Show();
            ccpEditBS.PerformCallback();

            return false;
        }
        else {
            ccpEditBS.Hide();
        }
    }
    function seteditpopupvalues() {
        if (ASPxClientEdit.ValidateGroup('EditBS')) {
            hdnBSKey.Set("BSEditTitle", txtEditTitle.GetText());
            hdnBSKey.Set("BSEditDecription", txtEditDescription.GetText());
            lnkupdateBS.DoClick();
            ccpEditBS.Hide();
        }
    }
    function onbusinessstrategychanged(s) {
        var key = s.GetRowKey(s.focusedRowIndex)
        if ($.isNumeric(key)) {
            hdnbskeyvalue.Set("keyvalue", key);
        }
        else {
            hdnbskeyvalue.Set("keyvalue", 0);
        }
        _gridView.PerformCallback(s.GetFocusedRowIndex());
    }
</script>


<div class="col-md-12 col-sm-12 col-xs-12" style="padding-top:10px">
    <div class="row">
        <div style="float:right; padding-bottom:10px;">
            <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn" Text="Apply Changes" ToolTip="Apply Changes" 
                OnClick="btnApplyChanges_Click"></dx:ASPxButton>
            <dx:ASPxHiddenField ID="hdnbskeyvalue" ClientInstanceName="hdnbskeyvalue" runat="server"></dx:ASPxHiddenField>
        </div>
    </div>
    <div class="row">
        <dx:ASPxSplitter ID="business_initiative" runat="server" ClientInstanceName="business_initiative" AllowResize="false" >
            <Panes>
                <dx:SplitterPane Name="Business Strategy" Size="100%" ScrollBars="Auto">
                    <ContentCollection>
                        <dx:SplitterContentControl>
                            <div class="col-md-12 col-sm-12 col-xs-12" style="padding: 0px 0px 6px 0px;">
                                <b><%=BStitle %>:</b>
                                <b style="text-align: right;">
                                    <asp:ImageButton ID="btAddBusinessStrategy" OnClientClick="return showpopup();" runat="server" ToolTip="Add Business Strategy" 
                                        ImageUrl="/Content/images/plus-cicle.png" Width="16px" />
                                </b>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                <ugit:ASPxGridView ID="grdBS" runat="server" CssClass="customgridview homeGrid" Width="100%" EnableViewState="false" Styles-AlternatingRow-BackColor="WhiteSmoke"
                                    KeyFieldName="ID" Settings-GridLines="None" OnHtmlRowPrepared="grdBS_HtmlRowPrepared" ClientInstanceName="grdBS">
                                    <SettingsPager Visible="false" Mode="ShowAllRecords"></SettingsPager>
                                    <SettingsBehavior AllowFocusedRow="True" />
                                    <ClientSideEvents FocusedRowChanged="function(s, e) {onbusinessstrategychanged(s,e);}" />
                                    <Styles>
                                        <Row CssClass="homeGrid_dataRow"></Row>
                                        <Header CssClass="homeGrid_headerColumn"></Header>
                                    </Styles>
                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="" Width="20px">
                                            <DataItemTemplate>
                                                <img id="imgBSEdit" style="cursor: pointer; width:16px" title="Edit" 
                                                    onclick='<%# string.Format("return showeditpopup({0})",Eval("ID")) %>' runat="server" src="/Content/images/editNewIcon.png" />
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataColumn FieldName="Title" Width="100px" Caption="Title"></dx:GridViewDataColumn>
                                        <dx:GridViewDataColumn Caption="Description" FieldName="Description"></dx:GridViewDataColumn>
                                    </Columns>
                                </ugit:ASPxGridView>
                            </div>
                        </dx:SplitterContentControl>
                    </ContentCollection>
                </dx:SplitterPane>
                <dx:SplitterPane Name="Initiative" Size="100%" ScrollBars="Auto">
                    <ContentCollection>
                        <dx:SplitterContentControl>
                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                <div class="fleft">
                                    <b><%=iniTitle %>:</b>
                                    <b style="text-align: right;">
                                        <a id="aAddItem_Top" title="Add Initiative" runat="server" href="" style="padding-left: 5px">
                                            <img id="Img2" runat="server" src="/Content/images/plus-cicle.png" width="16" />
                                        </a>
                                    </b>
                                </div>
                                <div class="fright crm-checkWrap">
                                    <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted" runat="server" TextAlign="Right"
                                        AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged" />
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                <div id="content" style="padding-top:5px;">
                                    <ugit:ASPxGridView ID="_gridView" ClientInstanceName="_gridView" runat="server" Width="100%" EnableViewState="false" 
                                        Styles-AlternatingRow-BackColor="WhiteSmoke" CssClass="customgridview homeGrid"
                                        KeyFieldName="ID" Settings-GridLines="None" OnDataBinding="_gridView_DataBinding" OnHtmlRowPrepared="_gridView_HtmlRowPrepared" 
                                        OnCustomCallback="_gridView_CustomCallback" >
                                        <SettingsPager Visible="false" Mode="ShowAllRecords"></SettingsPager>
                                        <Styles>
                                            <Row CssClass="homeGrid_dataRow"></Row>
                                            <Header CssClass="homeGrid_headerColumn"></Header>
                                        </Styles>
                                        <Columns>
                                            <dx:GridViewDataTextColumn Caption=" " Width="20px">
                                                <DataItemTemplate>
                                                    <a id="aEdit" runat="server" href="">
                                                        <img id="Imgedit" runat="server" style="width:16px" src="/Content/images/editNewIcon.png" />
                                                    </a>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataColumn FieldName="Title" Width="100px" Caption="Title"></dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="ProjectNote" Caption="Description"></dx:GridViewDataColumn>
                                            </Columns>
                                    </ugit:ASPxGridView>
                                </div>
                            </div>
                        </dx:SplitterContentControl>
                    </ContentCollection>
                </dx:SplitterPane>
            </Panes>
        </dx:ASPxSplitter>
    </div>
</div>

<dx:ASPxPopupControl ClientInstanceName="addnewstrategy" ID="addnewstrategy" runat="server" Modal="True" Width="450px"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssClass="aspxPopup"
    HeaderText="Add New Business Strategy" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
    <ContentCollection>
        <dx:PopupControlContentControl ID="pccBusinessStrategy" runat="server">
            <dx:ASPxPanel ID="pnladdstrategy" ClientInstanceName="pnladdstrategy" runat="server">
                <PanelCollection>
                    <dx:PanelContent>
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="ms-formtable accomp-popup">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxTextBox ID="txtBSTitle" Width="100%" runat="server" ValidationSettings-RequiredField-IsRequired="true"
                                            ValidationSettings-RequiredField-ErrorText="Please enter title" CssClass="asptextBox-input"
                                            ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-Display="Dynamic" 
                                            ClientInstanceName="txtBSTitle" ValidationSettings-ValidationGroup="BSSave"></dx:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxMemo ID="txtBSDescription" CssClass="aspxMemo-linkBox" Width="100%" runat="server" ClientInstanceName="txtBSDescription"></dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row addEditPopup-btnWrap">
                             <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false" CssClass="secondary-cancelBtn">
                                <ClientSideEvents Click="function(s, e){ 
                                    addnewstrategy.Hide();
                                    }" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btnSaveBS" ClientInstanceName="btnSaveBS" runat="server" Text="Save" ToolTip="Save" 
                                OnClick="btnSaveBS_Click" ValidationGroup="BSSave" AutoPostBack="false" CssClass="primary-blueBtn">
                                <ClientSideEvents Click="function(s, e){
                                     if( ASPxClientEdit.ValidateGroup('BSSave')){
                                          btnSaveBS.DoClick();
                                          addnewstrategy.Hide();
                                     }
                                    }" />
                            </dx:ASPxButton>
                           
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ClientInstanceName="ccpEditBS" ID="ccpEditBS" CloseAction="CloseButton" LoadContentViaCallback="OnPageLoad" runat="server" Modal="True" Width="450px"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssClass="aspxPopup"
    HeaderText="Edit Business Strategy" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <dx:ASPxPanel ID="dvpnlEditBS" ClientInstanceName="dvpnlEditBS" runat="server">
                <PanelCollection>
                    <dx:PanelContent>
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <dx:ASPxHiddenField ID="hdnBSKey" ClientInstanceName="hdnBSKey" runat="server"></dx:ASPxHiddenField>
                            <div class="ms-formtable accomp-popup">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxTextBox ID="txtEditTitle" runat="server" ValidationSettings-RequiredField-IsRequired="true" 
                                            ValidationSettings-RequiredField-ErrorText="Please enter title" CssClass="asptextBox-input" Width="100%"
                                            ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-ValidationGroup="EditBS"
                                            ValidationSettings-Display="Dynamic" ClientInstanceName="txtEditTitle"></dx:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxMemo ID="txtEditDescription" Width="100%" CssClass="aspxMemo-linkBox" runat="server" ClientInstanceName="txtEditDescription"></dx:ASPxMemo>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row addEditPopup-btnWrap">
                             <dx:ASPxButton ID="btnEditBSCancel" runat="server" Text="Cancel"  CssClass=" secondary-cancelBtn">
                                <ClientSideEvents Click="function(s,e){ ccpEditBS.Hide(); }" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="lnkupdateBS" ClientInstanceName="lnkupdateBS"  CssClass="primary-blueBtn" runat="server" Text="Save" ValidationGroup="EditBS" AutoPostBack="false" OnClick="lnkupdateBS_Click">
                                <ClientSideEvents Click="seteditpopupvalues" />
                            </dx:ASPxButton>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

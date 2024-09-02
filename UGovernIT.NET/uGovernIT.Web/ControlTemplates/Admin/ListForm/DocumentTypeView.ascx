<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentTypeView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.DocumentTypeView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx"%>
<%@ Import Namespace="uGovernIT.Utility" %>


<script data-v="<%=UGITUtility.AssemblyVersion %>">

    function editvendortype(mode) {
        hdnKeepkeyValue.Set('mode', mode);
        if (ASPxCallbackPanel1 != null)
            ASPxCallbackPanel1.PerformCallback();
    }
    function edit(obj, mode) {
        popupedit.Show();
        hdnKeepkeyValue.Set('KeyValue', obj);
        //hdnKeepkeyValue.Set('mode', mode);
        if (ASPxCallbackPanel1 != null)
            ASPxCallbackPanel1.PerformCallback(mode);

    }
    function closepopup() {
        if (popupedit.cpCloseEdit)
            popupedit.Hide();
        else if (popupnew.cpCloseNew)
            popupnew.Hide();

        gridview.Refresh();
    }

 </script>

<dx:ASPxHiddenField ID="hdnKeepkeyValue" ClientInstanceName="hdnKeepkeyValue" runat="server"></dx:ASPxHiddenField>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" style="padding-top:15px;">
        <div style="float:right;"></div>
        <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link" onclick="popupnew.Show()">
            <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" />
            <asp:Label ID="Label1" runat="server" Text="Add New Item"></asp:Label>
        </a>
    </div> 
    <div class="row" style="padding-top:15px;">
        <dx:ASPxGridView ID="gridview" runat="server" Width="100%" CssClass="customgridview homeGrid" ClientInstanceName="gridview" KeyFieldName="ID" AutoGenerateColumns="false">
            <Columns>
                <dx:GridViewDataTextColumn Caption="" Width="20px">
                    <DataItemTemplate>
                        <img src="/Content/Images/editNewIcon.png" style="cursor: pointer; width:16px;" title="Edit" onclick="edit(<%# Container.KeyValue %>,'edit')" />
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Title" FieldName="Title">
                    <DataItemTemplate>
                        <a href="javascript:void(0);" onclick="javascript:event.cancelBubble=true;edit(<%# Container.KeyValue %>,'edit')">
                            <%# Eval("Title") %>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Description" FieldName="DMSDescription">
                </dx:GridViewDataTextColumn>
            </Columns>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass=" homeGrid_headerColumn"></Header>
            </Styles>
            <Settings ShowHeaderFilterButton="true" />
            <SettingsBehavior AllowSelectByRowClick="true" />
            <SettingsPopup>
                <HeaderFilter MinHeight="250px" />
            </SettingsPopup>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[Deleted]=1" Format="Custom" RowStyle-CssClass="formatcolor" ApplyToRow="true" />
            </FormatConditions>
            <SettingsPager Visible="true" AlwaysShowPager="true" Mode="ShowPager" Position="TopAndBottom" PageSize="15">
                <PageSizeItemSettings Items="15, 20, 25, 50, 75, 100" />
            </SettingsPager>
        </dx:ASPxGridView>
    </div>
    <div class="row" style="padding-top:15px;">
         <a id="aAddItem" runat="server" href="" onclick="popupnew.Show()" class="primary-btn-link">
            <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
            <asp:Label ID="LblAddItem" runat="server" Text="Add New Item"></asp:Label>
        </a>
    </div>
</div>


<dx:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" Height="250px" OnCallback="ASPxCallbackPanel1_Callback" ClientInstanceName="ASPxCallbackPanel1" RenderMode="Table">
    <ClientSideEvents EndCallback="function(s,e){closepopup();}" />
    <PanelCollection>
        <dx:PanelContent>
            <dx:ASPxPopupControl ID="popupedit" ClientInstanceName="popupedit" HeaderText="Edit Document Type" CloseAction="OuterMouseClick" AllowDragging="true" 
                Width="500px" Height="200px" runat="server" EnableViewState="false" CssClass="aspxPopup" PopupHorizontalAlign="WindowCenter" Modal="true" 
                PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                           <div class="ms-formtable accomp-popup">
                               <div class="row">
                                   <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxTextBox ID="txttitleedit" Width="100%" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-Display="Dynamic" 
                                            ValidationSettings-ErrorDisplayMode="ImageWithText" ValidationSettings-ErrorText="Title is mandatory" CssClass="asptextBox-input"
                                            runat="server" ClientInstanceName="txttitleedit" EnableViewState="false">
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                               <div class="row">
                                   <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxMemo ID="txtdesedit" CssClass="aspxMemo-linkBox" runat="server" ClientInstanceName="txtdesedit" 
                                            CaptionSettings-ShowColon="false" EnableViewState="false"></dx:ASPxMemo>
                                    </div>
                               </div>
                               <div class="row addEditPopup-btnWrap">
                                   <dx:ASPxButton ID="btnrecycle" runat="server" Visible="false" OnClick="popupdelete_Click" CssClass="secondary-cancelBtn" 
                                        ClientInstanceName="popupdelete" Text="Delete" ToolTip="Delete" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e){popupedit.Hide();}" />
                                    </dx:ASPxButton>
                                   <dx:ASPxButton ID="btncloseedit" runat="server" ClientInstanceName="btnclose" CssClass="secondary-cancelBtn" Text="Close" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e){popupedit.Hide();}" />
                                    </dx:ASPxButton>
                                   <dx:ASPxButton ID="btnsaveedit" ClientInstanceName="btnsaveedit" runat="server" CssClass="primary-blueBtn" Text="Save" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e){editvendortype('update');}" />
                                    </dx:ASPxButton>
                                </div>
                           </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="popupnew" ClientInstanceName="popupnew" HeaderText="New Document Type" AllowDragging="true" CloseAction="OuterMouseClick" Width="500px" Height="200px"
                runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" CssClass="aspxPopup" PopupVerticalAlign="WindowCenter" Modal="true" EnableHierarchyRecreation="True">
                <ClientSideEvents Closing="function(s,e){ASPxClientEdit.ClearEditorsInContainerById('contentDiv');}" />
                <HeaderStyle CssClass="setheadercss" />
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" id="newEditorClear">
                            <div class="ms-formtable accomp-popup">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxTextBox ID="txttitle" runat="server" Width="100%" ClientInstanceName="txttitle" ValidationSettings-RequiredField-IsRequired="true" 
                                            ValidationSettings-Display="Dynamic" ValidationSettings-ErrorDisplayMode="ImageWithText" CssClass="asptextBox-input"
                                            ValidationSettings-ErrorText="Title is mandatory" EnableViewState="false"></dx:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxMemo ID="txtdexcription"  runat="server" ClientInstanceName="txtdexcription" CaptionSettings-ShowColon="false"
                                            EnableViewState="false" CssClass="aspxMemo-linkBox"></dx:ASPxMemo>
                                    </div>
                                </div>
                               <div class="row addEditPopup-btnWrap">
                                    <dx:ASPxButton ID="btnClose" runat="server" ClientInstanceName="btnClose" CssClass="secondary-cancelBtn" 
                                        Text="Close" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e){ popupnew.Hide()}" />
                                    </dx:ASPxButton>
                                     <dx:ASPxButton ID="btnSave" ClientInstanceName="btnSave" CssClass="primary-blueBtn" runat="server" Text="Save" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e){editvendortype('new');}" />
                                    </dx:ASPxButton>
                               </div>
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

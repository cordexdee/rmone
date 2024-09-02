<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetVendorsEdit.ascx.cs" Inherits="uGovernIT.Web.AssetVendorsEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
   function closeOrgFrame() {
        var FromCtrl = ("<%=FromCtrl%>" == "True") ? true : false;
        if (FromCtrl)
            VendorAddEdit_PopUp.Hide();
        else
            window.frameElement.commitPopup();
    }

    function newvendortype() {

        var url = '<%=dialogUrl%>';
        if (url == '')
            return false;

        window.parent.UgitOpenPopupDialog(url, '', 'Add New Vendor Type', 600, 250, false, escape(window.location.href), true);

        }
 function editvendortype() {
        var url = '<%=dialogUrl%>';
        if (url == '')
            return false;
        var vendortypeid = $('#<%=ddlVendorType.ClientID%>').attr('value');
     
        if (vendortypeid == '')
        {
            alert('Please select vendor type to edit.')
            return false;
        }
        if (vendortypeid != '')
            url = url + '&ItemId=' + vendortypeid;
        window.parent.UgitOpenPopupDialog(url, '', 'Edit Vendor Type', 600, 250, false, escape(window.location.href), true);
    }
    function editvendortype(mode) {

        hdnKeepkeyValue.Set('mode', mode);
       var vendortypeid = $('#<%=ddlVendorType.ClientID%>').val();
        if (vendortypeid == '') {
            alert('Please select vendor type to edit.')
            return false;
        }
        popupedit.Show();
        hdnKeepkeyValue.Set('KeyValue', vendortypeid);
        if (ASPxCallbackPanel1 != null)
            ASPxCallbackPanel1.PerformCallback();
    }
    function closepopup() {
        if (popupedit.cpCloseEdit)
            popupedit.Hide();
        else if (popupnew.cpCloseNew)
            popupnew.Hide();

        if (refreshdropdown != null && ASPxCallbackPanel1.cpAllowCallback)
            refreshdropdown.PerformCallback(ASPxCallbackPanel1.cpPassDropDownValue);
        //grdvendortype.Refresh();
    }
    function newvendortype(mode) {
        hdnKeepkeyValue.Set('mode', mode);
        if (ASPxCallbackPanel1 != null)
            ASPxCallbackPanel1.PerformCallback(mode);
    }
    function setSelectedValue() {
        var dropdownCtr = $('#<%=ddlVendorType.ClientID%>');
        if (dropdownCtr != undefined && refreshdropdown.cpSelectedValue) {
            hdnKeepkeyValue.Set('VType', refreshdropdown.cpSelectedValue);
            dropdownCtr.val(refreshdropdown.cpSelectedValue);
        }
    }
</script>

<dx:ASPxHiddenField ID="hdnKeepkeyValue" ClientInstanceName="hdnKeepkeyValue" runat="server"></dx:ASPxHiddenField>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Vendor Name<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtVendorName" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvtxtVendorName" ErrorMessage="Enter Vendor Name" ControlToValidate="txtVendorName" runat="server" ValidationGroup="Save" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Vendor Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div class="col-md-10 col-sm-10 col-xs-10 noLeftPadding">
                        <dx:ASPxCallbackPanel ID="refreshdropdown" OnCallback="refreshdropdown_Callback" runat="server" ClientInstanceName="refreshdropdown" 
                            RenderMode="Table" Width="100%">
                            <ClientSideEvents EndCallback="function(s,e){setSelectedValue();}" />
                            <PanelCollection>
                                <dx:PanelContent>
                                    <asp:DropDownList ID="ddlVendorType" runat="server" CssClass=" itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>
                    </div>
                    <div class="col-md-2 col-sm-2 col-xs-2 noPadding">
                        <img id="Img2" runat="server" title="Add Vendor Type" onclick="popupnew.Show();" src="/Content/images/plus-blue.png" width="16" />
                        <img src="/Content/Images/editNewIcon.png" width="16" alt="Edit" title="Edit Vendor Type" onclick="editvendortype('edit')" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Product Service Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDescription" TextMode="MultiLine"  runat="server" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Location</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtVendorLocation" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
             <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Phone</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtPhone" runat="server" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Email</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtEmail" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Address</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtAddress" TextMode="MultiLine"  runat="server" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">WebSite URL</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtWebsiteUrl" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Time Zone</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlTimeZone" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                        <asp:ListItem Value="Select">Select</asp:ListItem>
                        <asp:ListItem Value="CST">CST</asp:ListItem>
                         <asp:ListItem Value="EST">EST</asp:ListItem>
                         <asp:ListItem Value="MST">MST</asp:ListItem>
                         <asp:ListItem Value="PST">PST</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Support Hours</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtsupporthours" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
             <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Support Credentials</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtSupportCredentials" runat="server" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Account Rep Name</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtAccountRepName" runat="server" />
                </div>
            </div>
        </div>

       <div class="row">
           <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Account Rep Phone</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtAccountRepPhone" runat="server"/>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Account Rep Mobile</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtAccountRepMobile" runat="server"/>
                </div>
            </div>
       </div>
        
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Account Rep Email</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtAccountRepEmail" runat="server" />
                </div>
            </div>

            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr11" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                </div>
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click1" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            </div>
        </div> 
    </div>
</div>

<dx:ASPxPanel ID="callback" runat="server" ClientInstanceName="callback"></dx:ASPxPanel>
<dx:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" OnCallback="ASPxCallbackPanel1_Callback" ClientInstanceName="ASPxCallbackPanel1" RenderMode="Table">
    <ClientSideEvents EndCallback="function(s,e){closepopup();}" />
    <PanelCollection>
        <dx:PanelContent>
            <dx:ASPxPopupControl ID="popupedit" ClientInstanceName="popupedit" HeaderText="Edit Vendor Type" CloseAction="CloseButton" Width="500px" Height="200px"
                runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssClass="aspxPopup" 
                EnableHierarchyRecreation="True" SettingsAdaptivity-Mode="Always">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                         <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                            <div class="ms-formtable accomp-popup">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxTextBox ID="txttitleedit" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-Display="Dynamic" 
                                            ValidationSettings-ErrorDisplayMode="ImageWithText" ValidationSettings-ErrorText="Title is mandatory" CssClass="asptextBox-input"
                                            runat="server" ClientInstanceName="txttitleedit" EnableViewState="false" Width="100%">
                                        </dx:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxMemo ID="txtdesedit" runat="server" ClientInstanceName="txtdesedit" CaptionSettings-ShowColon="false" 
                                            EnableViewState="false" CssClass="aspxMemo-linkBox"></dx:ASPxMemo>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField crm-checkWrap">
                                        <asp:CheckBox ID="chkdeleteedit" runat="server" EnableViewState="false" Text="(Prevent use for new item)" />
                                       <%-- <dx:ASPxCheckBox ID="chkdeleteedit" ClientInstanceName="chkdeleteedit" runat="server" EnableViewState="false" 
                                            Text="(Prevent use for new ticket)"></dx:ASPxCheckBox>--%>
                                    </div>
                                </div>
                                <div class="row addEditPopup-btnWrap">
                                    <dx:ASPxButton ID="btnrecycle" runat="server" Visible="false" OnClick="popupdelete_Click" ClientInstanceName="popupdelete" 
                                        Text="Delete" ToolTip="Delete" AutoPostBack="false" CssClass="secondary-cancelBtn">
                                    </dx:ASPxButton>
                                     <dx:ASPxButton ID="btncloseedit" CssClass="secondary-cancelBtn" runat="server" ClientInstanceName="btnclose" Text="Close" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e){popupedit.Hide();}" />
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="btnsaveedit" ClientInstanceName="btnsaveedit" CssClass="primary-blueBtn" runat="server" Text="Save" AutoPostBack="false">
                                        <ClientSideEvents Click="function(s,e){editvendortype('update');}" />
                                    </dx:ASPxButton>
                                </div>
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="popupnew" ClientInstanceName="popupnew" HeaderText="New Vendor Type" CloseAction="CloseButton"
                runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True" 
                CssClass="aspxPopup" SettingsAdaptivity-Mode="Always">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                       <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                            <div class="ms-formtable accomp-popup">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxTextBox ID="txttitle" runat="server" ClientInstanceName="txttitle" ValidationSettings-RequiredField-IsRequired="true"
                                            ValidationSettings-Display="Dynamic" ValidationSettings-ErrorDisplayMode="ImageWithText" CssClass="asptextBox-input" 
                                            ValidationSettings-ErrorText="Title is mandatory" EnableViewState="false" Width="100%"></dx:ASPxTextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxMemo ID="txtdexcription" runat="server" ClientInstanceName="txtdexcription" CaptionSettings-ShowColon="false"
                                            EnableViewState="false" CssClass="aspxMemo-linkBox"></dx:ASPxMemo>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField crm-checkWrap">
                                        <asp:CheckBox ID="chkdelete" runat="server" EnableViewState="false" Text="(Prevent use for new item)" />
                                        <%--<dx:ASPxCheckBox ID="chkdelete" ClientInstanceName="chkdelete" runat="server" EnableViewState="false" 
                                            Text="(Prevent use for new ticket)"></dx:ASPxCheckBox>--%>
                                    </div>
                                </div>
                                <div class="row addEditPopup-btnWrap">
                                    <dx:ASPxButton ID="btnClose" runat="server" ClientInstanceName="btnClose" Text="Close" AutoPostBack="false" CssClass="secondary-cancelBtn">
                                        <ClientSideEvents Click="function(s,e){ popupnew.Hide()}" />
                                    </dx:ASPxButton>
                                     <dx:ASPxButton ID="ASPxButton1" ClientInstanceName="btnSave" runat="server" Text="Save" AutoPostBack="false" CssClass="primary-blueBtn">
                                        <ClientSideEvents Click="function(s,e){newvendortype('new');}" />
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

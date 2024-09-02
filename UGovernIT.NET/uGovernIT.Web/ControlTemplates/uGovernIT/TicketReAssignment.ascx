<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketReAssignment.ascx.cs" Inherits="uGovernIT.Web.TicketReAssignment" %>
 <%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function editPRP() {
        $("[id$='hndPRP']").val('');
        $("[id$='dvPRP']").css('display', 'none');
        $("[id$='dvPRPEditor']").css('display', '')
    }

    function editORP() {
        $("[id$='hdnORP']").val('');
        $("[id$='dvORPEditor']").css('display', '');
        $("[id$='dvORP']").css('display', 'none')
    }
     $(document).ready(function () {
        $('#dvTktReassign').parent().addClass('popupUp-mainContainer reAssignePopup')
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12" id="dvTktReassign">
    <div class=" ms-formtable accomp-popup ">
        <div class="row" id="tblMain" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Skill</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div>
                    <asp:HiddenField ID="hndUserType" runat="server"></asp:HiddenField>
                    <dx:ASPxComboBox ID="cbSkill" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith" 
                        CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox" Width="100%"></dx:ASPxComboBox>
                    <asp:RequiredFieldValidator ID="rfvSkill" runat="server" ValidationGroup="AddResource" ControlToValidate="cbSkill"
                        Display="Dynamic" InitialValue="" CssClass="text-error">
                            <span>Please select a Skill.</span>
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel" style="display:inline-block;">PRP<span style="color:red">*</span></h3>
                    <div class="reassign_popup_gridOpen_icon">
                        <asp:ImageButton ID="btnPRP" runat="server" Style="padding-left: 7px;"
                                ImageUrl="~/Content/Images/Autocalculater.png" ToolTip="Choose Resource" Height="16px"
                                OnClick="btnPRP_Click" />
                    </div>
                </div>
                <div class="ms-formbody accomp_inputField"  id="dvPRPEditor" runat="server">
                    <ugit:UserValueBox ID="pePRP" runat="server" isMulti="false" SelectionSet="User" CssClass="userValueBox-dropDown" />
                    <div style="float: left;" class="">
                        <asp:CustomValidator ID="cvUser" ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" 
                            OnServerValidate="cvUser_ServerValidate" ErrorMessage="Enter User Name" Display="Dynamic" 
                            ValidationGroup="TicketReassignment"></asp:CustomValidator>
                    </div>
                </div>
                <div id="dvPRP" runat="server" style="display: none;">
                    <asp:Label ID="lblPRP" runat="server" ></asp:Label>

                    <asp:HiddenField ID="hndPRP" runat="server" />
                    <img id="imgEditItem" onclick="editPRP()" runat="server" src="~/Content/Images/edit-icon.png" />
                </div>
                <dx:ASPxPopupControl ID="grdSkill" runat="server" CloseAction="CloseButton" Modal="true" SettingsAdaptivity-Mode="Always"
                    PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ScrollBars="Auto"
                    ShowFooter="false"  HeaderText="Resource(s) with Skills:" ClientInstanceName="grdSkill"
                    CssClass="aspxPopup" width="500px" height="300px">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                <div class="row">
                                    <ugit:ASPxGridView ID="gridAllocation" CssClass="customgridview homeGrid"  runat="server" AutoGenerateColumns="False"
                                        OnDataBinding="gridAllocation_DataBinding" ClientInstanceName="gridAllocation"
                                        Width="100%" KeyFieldName="ResourceId"
                                        SettingsText-EmptyDataRow="No Resources Available">
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowSelectCheckbox="True"
                                                VisibleIndex="0">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn PropertiesTextEdit-EncodeHtml="false" Caption="Resource" FieldName="Resource" 
                                                ReadOnly="True"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Skill" FieldName="UserSkill" ReadOnly="True"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn PropertiesTextEdit-EncodeHtml="false" Caption="Total Allocation" 
                                                FieldName="FullAllocation" ReadOnly="True"></dx:GridViewDataTextColumn>
                                        </Columns>
                                        <Styles AlternatingRow-CssClass="ugitlight1lightest">
                                             <GroupRow Font-Bold="true"></GroupRow>
                                             <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                                             <Row cssClass="homeGrid_dataRow"></Row>
                                        </Styles>
                                        <Settings GroupFormat="{1}" VerticalScrollableHeight="150" VerticalScrollBarMode="Auto" />
                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsBehavior ConfirmDelete="true" AllowDragDrop="false" AutoExpandAllGroups="true" ColumnResizeMode="Disabled" />
                                        <SettingsPopup HeaderFilter-Height="150"></SettingsPopup>
                                    </ugit:ASPxGridView>
                                </div>
                                <div class="row">
                                    <asp:Label ID="lblSkillErrorMessage" runat="server" Text="Please select at least one resource." ForeColor="Red" Visible="false"></asp:Label>
                                </div>
                                    <div class="row addEditPopup-btnWrap">
                                         <dx:ASPxButton ID="btCancelResourceSkill" runat="server" ClientInstanceName="btCancelResourceSkill" 
                                             Text="Cancel" CssClass="secondary-cancelBtn" ToolTip="Cancel" CausesValidation="false">
                                            <ClientSideEvents Click="function(s, e) { grdSkill.Hide(); }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="dxUpdateSkill" Text="Add"  CssClass="primary-blueBtn" runat="server" 
                                            ClientInstanceName="dxUpdateSkill" OnClick="dxUpdateSkill_Click"></dx:ASPxButton>
                                    </div>
                                </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel" style="display:inline-block;">ORP</h3>
                <div class="reassign_popup_gridOpen_icon" >
                    <asp:ImageButton ID="btnORP" runat="server" Style="padding-left: 7px;"
                        ImageUrl="/Content/Images/Autocalculater.png" ToolTip="Choose Resource" Height="16px"
                        OnClick="btnPRP_Click" />
                </div>
            </div>
            <div class="ms-formbody accomp_inputField" id="dvORPEditor" runat="server">
                <ugit:UserValueBox ID="peORP" runat="server" isMulti="true" SelectionSet="User" CssClass="userValueBox-dropDown" />
                <div id="dvORP" runat="server" style="display: none;">
                    <asp:Label ID="lblORP" runat="server"></asp:Label>
                    <asp:HiddenField ID="hdnORP" runat="server" />
                    <img id="imgOPREdit" onclick="editORP()" runat="server" src="~/Content/Images/edit-icon.png" />
                </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnUpdate" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" OnClick="btnUpdate_Click" ValidationGroup="TicketReassignment">
            </dx:ASPxButton>
        </div>
    </div>
</div>


<div class="col-md-12 col-sm-12 col-xs-12 fieldClear" id="tblErrorMessage" runat="server" visible="false">
    <div class="row">
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Text="sfsdfs"></asp:Label>
    </div>
    <div class="row addEditPopup-btnWrap">
        <dx:ASPxButton ID="LinkButton1" runat="server" CssClass="secondary-cancelBtn" Text="Cancel" ToolTip="Cancel">
            <ClientSideEvents Click="function(){window.frameElement.commitPopup();}" />
        </dx:ASPxButton>
            <%--<asp:LinkButton ID="LinkButton1" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;"
                ToolTip="Cancel" OnClientClick="window.frameElement.commitPopup();">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">
                                Cancel</b>
                            <i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/Content/ButtonImages/cancelwhite.png"  style="border:none;" title="" alt=""/>
                            </i> 
                        </span>
            </asp:LinkButton>--%>
    </div>
</div>


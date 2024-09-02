<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkCategoryEdit.ascx.cs" Inherits="uGovernIT.Web.LinkCategoryEdit" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function AddLinkView() {
        $("#<%=txtTitle.ClientID %>").val('');
         $("#<%=ddlCategory.ClientID %>").get(0).selectedIndex = 0;
         $("#<%=lblErrorMsgCategory.ClientID %>").val('');
         $("#<%=txtLinkViewCategory.ClientID %>").val('');
         $(".ddlCategory").show();
         $("#<%=hdnCategory.ClientID %>").val('0');
        popupControlLinkView.Show();
    }

    function hideddlCategory() {
        $("#<%=ddlCategory.ClientID %>").get(0).selectedIndex = 0;
        $(".ddlCategory").hide();
        $("#<%=hdnCategory.ClientID %>").val('1');
    }
    function showddlCategory() {
        $(".ddlCategory").show();
        $("#<%=hdnCategory.ClientID %>").val('0');
    }

   

    //function hideddlCategory() {
    //    $("#ctl00_PlaceHolderMain_ctl00_ddlCategory").get(0).selectedIndex = 0;
    //    $(".ddlCategory").hide();
    //    $("#ctl00_PlaceHolderMain_ctl00_hdnCategory").val('1');

    //}

    //function showddlCategory() {
    //    $(".ddlCategory").show();
    //    $("#ctl00_PlaceHolderMain_ctl00_hdnCategory").val('0');
    //}
</script>

<%-- <asp:HiddenField ID="hdnCategory" runat="server" />--%>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">View<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField accomp_inputField_linkViewCus">
                <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                    <asp:DropDownList ID="ddlView" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                </div>
                 <div class="col-md-1 col-sm-1 col-xs-1 noPadding" style="margin-left: 10px">
                      <img alt="Add Link View" title="Add Link View" id="imviewcategory" src="/content/images/plus-cicle.png" style="cursor: pointer;width:16px;" 
                    onclick="AddLinkView()" />
                </div>
                <div>
                    <asp:RequiredFieldValidator ID="rfvddlVIew" runat="server" ErrorMessage="please select view " ControlToValidate="ddlView" ValidationGroup="Save" InitialValue="" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Section<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtCategoryName" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvCategoryName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtCategoryName" 
                        ErrorMessage="required link category" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="row" id="tr3" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Order<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtOrder" runat="server" />
                <div>
                    <asp:RegularExpressionValidator ID="regextxtOrder" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtOrder" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvtxtOrder" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtOrder"
                        ErrorMessage="order required." Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Image Url<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <asp:TextBox runat="server" ID="txtFile" />
                <br />
                 <asp:FileUpload runat="server" ID="fileUpload"  /> 
                <div>
                    <asp:CustomValidator ID="cvfileUpload" ErrorMessage="Select a file." ControlToValidate="fileUpload" runat="server" Enabled="true" OnServerValidate="cvfileUpload_ServerValidate" Display="Dynamic" ValidationGroup="Save"  />
                </div>
            </div>
        </div>

        <div class="row" id="tr5" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" row="6" Columns="44" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="lnkDelete" Visible="false" runat="server" Text="Delete" ToolTip="Delete" OnClick="lnkDelete_Click" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="function(){return confirm('Are you sure you want to delete?');}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>

    <dx:ASPxPopupControl ID="popupControlLinkView" runat="server" CloseAction="CloseButton" Modal="True" Width="450px" Height="200px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupControlLinkView" SettingsAdaptivity-Mode="Always"
        HeaderText="Add View Link" AllowDragging="false" PopupAnimationType="None" EnableViewState="true" CssClass="aspxPopup">
        <ContentCollection>
            <dx:PopupControlContentControl ID="pcccLinkView" runat="server">
                <dx:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnLinkViewSave">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                                <div id="tb1" class="ms-formtable accomp-popup">
                                    <div class="row" id="tr12" runat="server">
                                        <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                            <asp:TextBox ID="txtTitle" runat="server" />
                                            <div>
                                                <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                                                    ErrorMessage="Field required." Display="Dynamic" ValidationGroup="SaveLinkView"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="tr13" runat="server">
                                        <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">Category<b style="color: Red;">*</b></h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                            <div class="ddlCategory" id="divddlCategory" runat="server">
                                                <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass=" itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                                                    <img alt="Add Category Name" id="imcategory" src="/content/images/plus-cicle.png" style="cursor: pointer; margin-left:5px; width:16px;" 
                                                    onclick="javascript:$('.divCategory').attr('style','display:block');hideddlCategory()" />
                                                </div>
                                            </div>
                                            <div runat="server" id="divCategory" class="divCategory" style="display: none;">
                                                <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                                    <asp:TextBox runat="server" ID="txtLinkViewCategory" CssClass="txtCategory"></asp:TextBox>
                                                </div>
                                                <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                                                     <img alt="Cancel Category Name" width="16" src="/content/images/close-blue.png" class="cancelModule" 
                                                    onclick="javascript:$('.divCategory').attr('style','display:none');showddlCategory();" />
                                                </div>
                                            </div>
                                            <div style="float: left; width: 100%;">
                                                <asp:Label ID="lblErrorMsgCategory" runat="server" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row" id="tr9" runat="server">
                                        <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                            <asp:TextBox ID="TextBox2" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="3" cols="15" />
                                        </div>
                                    </div>
                                    <div class="row addEditPopup-btnWrap">
                                        <dx:ASPxButton ID="btnILinkViewCancel" runat="server" ClientInstanceName="btnILinkViewCancel" Text="Cancel" ToolTip="Cancel" AutoPostBack="false"
                                            CausesValidation="false" CssClass="secondary-cancelBtn">
                                            <ClientSideEvents Click="function(s, e) { popupControlLinkView.Hide(); }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btnLinkViewSave" ClientInstanceName="btnLinkViewSave" runat="server" ValidationGroup="SaveLinkView"
                                            Text="Save" ToolTip="Save" CssClass="primary-blueBtn"
                                            OnClick="btnLinkViewSave_Click">
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ContentStyle>
            <Paddings PaddingBottom="5px" />
        </ContentStyle>
    </dx:ASPxPopupControl>

    <asp:HiddenField ID="hdnCategory" runat="server" />
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkConfiguratorView.ascx.cs" Inherits="uGovernIT.Web.LinkConfiguratorView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .StaticMenuStyle a {
        border-width: 4px;
        font: menu 16px arial;
        height: 0;
        padding: 2px 40px;
        text-align: center;
        width: auto;
    }

    #header {
        text-align: center;
        /*height: 30px;*/
        float: left;
        padding: 0px 2px;
    }

    #content {
        width: 100%;
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: normal;
    }

    a:hover {
        text-decoration: underline;
    }

    a, img {
        border: 0px;
    }

    .seqNoHeader {
        width: 20px !important;
        text-align: center;
    }

    .seqNoItem {
        width: 20px !important;
        text-align: center;
    }
</style>

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

    .ms-formlabel {
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        width: 160px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .fleft {
        float: left;
    }

    .proposeddatelb {
        padding-top: 5px;
        padding-right: 4px;
        float: left;
    }

    .tskbehaviour-opt {
    }

        .tskbehaviour-opt td {
            padding-left: 15px;
        }

            .tskbehaviour-opt td:first-child {
                padding-left: 0px;
            }

        .tskbehaviour-opt input {
            float: left;
            position: relative;
            top: -2px;
        }

        .tskbehaviour-opt label {
            float: left;
        }

            .tskbehaviour-opt label > i {
                float: left;
            }

            .tskbehaviour-opt label > b {
                float: left;
                padding: 2px 0 0 2px;
            }

    .attachment-container {
        float: left;
        width: 100%;
        padding-top: 7px;
    }

        .attachment-container .label {
            float: left;
            width: 24%;
            padding-left: 23px;
        }

        .attachment-container .attachmentform {
            float: left;
            width: 100%;
        }

    .attachmentform .oldattachment, .attachmentform .newattachment {
        float: left;
        width: 100%;
    }

    .attachmentform .fileupload {
        float: left;
        width: 100%;
    }

    .fileupload span, .fileread span {
        float: left;
    }

    .fileupload label, .fileread label {
        float: left;
        padding-left: 5px;
        font-weight: bold;
        padding-top: 3px;
        cursor: pointer;
    }

    .attachmentform .fileread {
        float: left;
        width: 100%;
    }

    .attachment-container .addattachment {
        float: left;
    }

        .attachment-container .addattachment img {
            border: 1px outset;
            padding: 3px;
        }

    .overlay {
        display: none;
        position: absolute;
        left: 0%;
        top: 0%;
        padding: 25px;
        background-color: black;
        width: 93%;
        height: 740px;
        -moz-opacity: 0.3;
        opacity: .30;
        filter: alpha(opacity=30);
        z-index: 100;
    }
</style>--%>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        if ($("#<%=ddlView.ClientID %>").get(0).selectedIndex == 0) {
            $("#<%=ImgbtnEditViewLink.ClientID %>").hide();
        }
        else {
            $("#<%=ImgbtnEditViewLink.ClientID %>").show();
        }
    })

    function AddLinkView() {
        $("#<%=txtTitle.ClientID %>").val('');
        $("#<%=ddlCategory.ClientID %>").get(0).selectedIndex = 0;
        $("#<%=lblErrorMsgCategory.ClientID %>").val('');
        $("#<%=txtCategoryName.ClientID %>").val('');
        $(".ddlCategory").show();
        $("#<%=hdnCategory.ClientID %>").val('0');
        popupControlLinkView.Show();
        $("#<%=hdnLinkViewAddEdit.ClientID %>").val('0');
        $("#<%=lnkDelete.ClientID %>").hide();
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

    
        function openDialog(path, params, titleVal, width, height, stopRefresh) {
            window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, 0);
       
        }

        function OpenPopupDialog(path, params, titleVal, width, height, stopRefresh) {
            window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, 0);
        }
        
</script>

<div id="content" class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap linkViewContent">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div class="col-md-5 col-sm-5 col-xs-12 noPadding">
                <div class="ms-formlabel"> 
                    <h3 class=" ms-standardheade budget_fieldLabel">View</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div class="col-md-11 col-sm-11 col-xs-11" style="padding-left:0">
                        <asp:DropDownList ID="ddlView" runat="server" CssClass="itsmDropDownList aspxDropDownList"  OnSelectedIndexChanged="ddlView_SelectedIndexChanged" 
                        AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="col-md-1 col-sm-1 col-xs-1 noPadding" style="display: flex; justify-content:space-between">
                        <img alt="Add View Link" title="Add View Link" id="imviewcategory" src="/content/images/plus-cicle.png" style="cursor: pointer; width:16px;" onclick="AddLinkView()" />
                        <asp:ImageButton runat="server" ToolTip="Edit View Link" ID="ImgbtnEditViewLink" ImageUrl="/content/images/editNewIcon.png" Style="cursor: pointer; width:16px" 
                        OnClick="ImgbtnEditViewLink_Click" />
                    </div>
                </div>
            </div>
            <div class="col-md-7 col-sm-7 col-xs-12 noPadding">
                <div style="float:right; padding-top:15px;">
                    <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                        <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" title="Add New Item" />
                        <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                    </a>
                </div>
            </div>
        </div>
        <div class="row">
             <ugit:ASPxGridView ID="ASPxGridViewLinkView" AutoGenerateColumns="False" runat="server"  SettingsText-EmptyDataRow="No record found."
                KeyFieldName="ID" Width="100%" CssClass="customgridview homeGrid" ClientInstanceName="ASPxGridViewLinkView" 
                 OnHtmlRowPrepared="ASPxGridViewLinkView_HtmlRowPrepared">
                <Columns>
                    <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0" Width="30px">
                        <DataItemTemplate>
                            <img id="editLink" runat="server" src="/content/images/editNewIcon.png" alt="Edit" title="Edit" style="float: right; cursor:pointer; width:16px" />
                        </DataItemTemplate>
                        <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataDateColumn FieldName="ID" VisibleIndex="0" Visible="false" Caption="ID" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataDateColumn FieldName="ItemOrder" Width="30px" VisibleIndex="2" Caption="#" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                    </dx:GridViewDataDateColumn>

                    <%--  <dx:GridViewDataColumn FieldName="Title" VisibleIndex="3" Caption="Title" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>--%>

                        <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" VisibleIndex="3" >
                        <DataItemTemplate>
                            <a id="aTitle" runat="server" href="" onload="aTitle_Load"></a>
                        </DataItemTemplate>
                            <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataColumn FieldName="TargetType" VisibleIndex="4" Caption="Target Type" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="Description" VisibleIndex="5" Caption="Description" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="LinkCategoryLookup" Visible="false" VisibleIndex="6" Caption="Link Category" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="ItemOrder" VisibleIndex="6" Caption="Link Category Item Order" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" GroupIndex="0">
                    </dx:GridViewDataColumn>
                </Columns>

                <Templates>
                    <GroupRowContent>
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxLabel ID="lblCategoryGroupHeader" runat="server"></dx:ASPxLabel>
                                </td>
                                          
                                <td>
                                    <img id="imgEditCategorybutton" runat="server" src="/content/images/editNewIcon.png" alt="Edit Category" title="Edit Category" style="cursor:pointer; width: 16px; margin-left:10px" />
                                </td>
                            </tr>
                        </table>
                    </GroupRowContent>
                </Templates>
                <Settings ShowFooter="false" ShowHeaderFilterButton="true" GroupFormat="{0} {1}" />
                <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                    <GroupRow Font-Bold="true"></GroupRow>
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                    <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                    <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                </Styles>
            </ugit:ASPxGridView>
        </div>
        <div class="row" style="float:right; padding-top:15px;">
            <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" title="Add New Item" />
                <asp:Label ID="LblAddItem" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>


    <dx:ASPxPopupControl ID="popupControlLinkView" runat="server" CloseAction="CloseButton" Modal="True" CssClass="aspxPopup" Width="600px" Height="250px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupControlLinkView" SettingsAdaptivity-Mode="Always"
        HeaderText="Add View Link" AllowDragging="false" PopupAnimationType="None" EnableViewState="true">
        <ContentCollection>
            <dx:PopupControlContentControl ID="pcccLinkView" runat="server">
                <dx:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnLinkViewSave">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
                                <div id="tb1" class="ms-formtable accomp-popup">
                                    <div class="row">
                                        <div class="col-md-6 col-sm-6 col-xs-12" id="tr12" runat="server">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                            </div>
                                            <div class="ms-formbody accomp_inputField">
                                                <asp:TextBox ID="txtTitle" runat="server" />
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                                                        ErrorMessage="Field required." Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                         <div class="col-md-6 col-sm-6 col-xs-12" id="tr13" runat="server">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">Category<b style="color: Red;">*</b></h3>
                                            </div>
                                            <div class="ms-formbody accomp_inputField">
                                                <div class="ddlCategory row" id="divddlCategory" runat="server">
                                                    <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                                                        <img alt="Add Category Name" id="imcategory" src="/content/images/plus-cicle.png" style="cursor: pointer; width:16px; margin-left:5px" 
                                                    onclick="javascript:$('.divCategory').attr('style','display:block');hideddlCategory()" />
                                                    </div>
                                                </div>
                                                <div runat="server" id="divCategory" class="divCategory row" style="display: none;">
                                                    <div class="col-md-11 col-sm-11 col-xs-11 noPadding">
                                                        <asp:TextBox runat="server" ID="txtCategoryName" CssClass="txtCategory"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                                                        <img alt="Cancel Category Name" width="16" src="/content/images/close-blue.png" style="margin-left:5px;" class="cancelModule"
                                                        onclick="javascript:$('.divCategory').attr('style','display:none');showddlCategory();" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <asp:Label ID="lblErrorMsgCategory" runat="server" ForeColor="Red"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 col-xs-12" id="tr9" runat="server">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                                            </div>
                                            <div class="ms-formbody accomp_inputField">
                                                <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" Rows="6" cols="20" />
                                            </div>
                                        </div>
                                    </div>
                                   <div class="row addEditPopup-btnWrap">
                                       <div class="col-md-12 col-sm-12 col-xs-12">
                                           <dx:ASPxButton ID="lnkDelete" Visible="false" runat="server" Text="Delete" ToolTip="Delete" 
                                               OnClick="lnkDelete_Click" CssClass="secondary-cancelBtn">
                                               <ClientSideEvents Click="function(){return confirm('Are you sure you want to delete?');}" />
                                           </dx:ASPxButton>
                                            <dx:ASPxButton ID="btnILinkViewCancel" runat="server" ClientInstanceName="btnILinkViewCancel" Text="Cancel" ToolTip="Cancel" 
                                                AutoPostBack="false" CssClass="secondary-cancelBtn" CausesValidation="false" >
                                                <ClientSideEvents Click="function(s, e) { popupControlLinkView.Hide(); }" />
                                            </dx:ASPxButton>
                                           <dx:ASPxButton ID="btnLinkViewSave" ClientInstanceName="btnLinkViewSave" runat="server" ValidationGroup="Save"
                                                Text="Save" ToolTip="Save" CssClass="primary-blueBtn" OnClick="btnLinkViewSave_Click" AutoPostBack="true">
                                            </dx:ASPxButton>
                                       </div>
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
    <asp:HiddenField ID="hdnLinkViewAddEdit" runat="server" />
</div>

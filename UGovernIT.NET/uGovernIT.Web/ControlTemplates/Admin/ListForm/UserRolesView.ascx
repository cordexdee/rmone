<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRolesView.ascx.cs" Inherits="uGovernIT.Web.UserRolesView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function openuserRoleDialog(path, params, titleVal, width, height, stopRefresh) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh);
    }

    function NewUserRoleDialog() {
        window.parent.UgitOpenPopupDialog('<%=editUrl%>&RoleID=', "", "Add Landing Page", '400px', '350px', 0);        
    }
</script>
<%--<div style="margin-bottom: 5px;">
    <div style="float: right">--%>
       <%-- <asp:LinkButton ID="lnkNew" runat="server" Text="&nbsp;&nbsp;New&nbsp;&nbsp;" OnClientClick="NewUserRoleDialog()"
            ToolTip="Add New" Style="float: right;">
                <span class="button-bg">
                        <b style="float: left; font-weight: normal;">Add New</b>
                        <i style="float: right; position: relative; top: -1px;left:2px">
                            <img src="/Content/images/uGovernIT/add_icon.png"  style="border:none;" title="" alt=""/>
                        </i> 
                </span>
        </asp:LinkButton>--%>
<%--    </div>
</div>--%>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" style="padding-top:15px;">
    <div class="row">
        <ugit:ASPxGridView ID="aspxGridUserRoles" AutoGenerateColumns="False" runat="server" SettingsText-EmptyDataRow="No record found." 
            KeyFieldName="Id" Width="100%" ClientInstanceName="aspxGridUserRoles" OnHtmlRowPrepared="aspxGridUserRoles_HtmlRowPrepared"  
            OnHeaderFilterFillItems="aspxGridUserRoles_HeaderFilterFillItems" CssClas="customgridview homeGrid">

            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" VisibleIndex="0" Width="40px">
                    <DataItemTemplate>
                      <a id="editLink" runat="server" href=""> 
                           <img id="Imgedit" runat="server" style="width:16px" src="~/Content/Images/editNewIcon.png"/>
                        </a>
                    </DataItemTemplate>
                    <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                </dx:GridViewDataTextColumn>

                 <dx:GridViewDataDateColumn FieldName="Name" VisibleIndex="1" Caption="Page Title">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataDateColumn>

                <dx:GridViewDataColumn FieldName="Description" VisibleIndex="2"  Caption="Description" CellStyle-HorizontalAlign="Left">
                    <Settings HeaderFilterMode="CheckedList"  />
                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="LandingPage" VisibleIndex="3" Caption="Landing Page" CellStyle-HorizontalAlign="Left">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataColumn>
            </Columns>
            <Settings ShowFooter="False" ShowHeaderFilterButton="true"/>
            <Settings VerticalScrollableHeight="312" />
            <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
            <Settings VerticalScrollBarMode="Auto" />
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
            <SettingsPager PageSize="11"></SettingsPager>
            <Styles AlternatingRow-CssClass="ugitlight1lightest">
                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                <Row CssClass="homeGrid_dataRow"></Row>
                <AlternatingRow CssClass="ugitlight1lightest"></AlternatingRow>
                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
            </Styles>
        </ugit:ASPxGridView>
    </div>
    <div class="row" style="padding:15px 0;">
        <div style="float: right;">
            <a id="lnkNewuserRole" runat="server" href="" style="padding-left:15px" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserCertificatesView.ascx.cs" Inherits="uGovernIT.Web.UserCertificatesView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function openuserCertificateDialog(path, params, titleVal, width, height, stopRefresh) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh);
    }

    function NewUserCertificateDialog() {
        window.parent.UgitOpenPopupDialog('<%= editUrl%>&CertificateID=0', "", "Add User Certification", '450px', '330px', 0);
    }
</script>
<div class="col-md-12 col-sm-12 col-xs-12">
    <div style="margin:5px;">
        <div class="headerItem-addItemBtn" style="margin-bottom: 5px;">
            <a id="LinkButton1" runat="server" href="" class="primary-btn-link">
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
    <dx:ASPxGridView ID="aspxGridUserCertificates" AutoGenerateColumns="False" runat="server" SettingsText-EmptyDataRow="No record found."
        KeyFieldName="ID" Width="99%" ClientInstanceName="aspxGridUserCertificates" OnHtmlRowPrepared="aspxGridUserCertificates_HtmlRowPrepared">
        <Columns>
            <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0">
                <DataItemTemplate>
                    <img src="/Content/Images/editNewIcon.png" alt="Edit" style="width: 16px;" />
                </DataItemTemplate>
                <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataDateColumn FieldName="Title" VisibleIndex="1" Caption="Certificate">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="CategoryName" VisibleIndex="2" Caption="Category">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataColumn FieldName="Description" VisibleIndex="3" Caption="Description">
                <Settings HeaderFilterMode="CheckedList" />
            </dx:GridViewDataColumn>
        </Columns>

        <Settings ShowFooter="True" ShowHeaderFilterButton="true" />
        <Styles>
            <Row CssClass="homeGrid_dataRow"></Row>
            <Header CssClass="homeGrid_headerColumn"></Header>
        </Styles>
        <%--<Settings VerticalScrollableHeight="300" />
    <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
    <Settings VerticalScrollBarMode="Auto" />--%>
        <%--<SettingsPopup>
        <HeaderFilter Height="200" />
    </SettingsPopup>--%>
        <%--<Styles AlternatingRow-CssClass="ugitlight1lightest">
        <GroupRow Font-Bold="true"></GroupRow>
        <Header Font-Bold="true"></Header>
        <AlternatingRow CssClass="ugitlight1lightest"></AlternatingRow>
        <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
    </Styles>--%>
    </dx:ASPxGridView>

    <div style="margin-top: 5px;">
        <div style="float: right; padding:15px 0;" >
            <a id="lnkAddNewUserCertificate" runat="server" href="" style="padding-left: 15px" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
</div>
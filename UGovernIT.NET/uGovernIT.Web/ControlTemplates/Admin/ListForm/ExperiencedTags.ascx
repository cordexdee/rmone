<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExperiencedTags.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.ExperiencedTags" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style>
    .dxdvItem_UGITNavyBlueDevEx, .dxdvFlowItem_UGITNavyBlueDevEx {
        border: 0px solid #a9acb5;
        background-color: White;
        padding: 0px;
        height: 0px;
    }

    .dxdvControl_UGITNavyBlueDevEx {
        font: 11px Verdana, Geneva, sans-serif;
        color: #201f35;
        border: 0px solid #9da0aa;
    }

    .floatright {
        float: right;
        padding: 5px;
    }

    .paddingright {
        padding-right: 10%;
    }

    .paddingtop {
        padding-top: 10px;
    }

    .rowBackBgColor_0 {
        background: #e4e3e3;
    }

    .rowBackBgColor_1 {
        background: #F7D7DA;
    }

    .Hidden {
        display: none;
    }

    .displaynone {
        display: none;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function openProjectTagDialog(path, params, titleVal, width, height, stopRefresh) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh);
    }

    function NewProjectTagDialog() {
        debugger
        window.parent.UgitOpenPopupDialog('<%= editUrl%>&ProjectTagID=0', "", "Add Experience Tag", '450px', '330px', 0);
    }
</script>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Text="Please Wait ..."
    Modal="True">
</dx:ASPxLoadingPanel>


<div class="col-md-12 col-sm-12 col-xs-12">
    <div style="margin: 5px;">
        <div class="headerItem-addItemBtn" style="margin-bottom: 5px;">
            <a id="LinkButton1" runat="server" href="" class="primary-btn-link">
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>

    <dx:ASPxGridView ID="aspxGridProjectTags" AutoGenerateColumns="False" runat="server" SettingsText-EmptyDataRow="No record found."
        KeyFieldName="ID" Width="99%" ClientInstanceName="aspxGridProjectTags" OnHtmlRowPrepared="aspxGridProjectTags_HtmlRowPrepared">
        <Columns>
            <dx:GridViewDataTextColumn FieldName=" " Width="40" CellStyle-HorizontalAlign="Center" VisibleIndex="0">
                <DataItemTemplate>
                    <img src="/Content/Images/editNewIcon.png" alt="Edit" style="width: 16px;" />
                </DataItemTemplate>
                <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataTextColumn FieldName="Category" VisibleIndex="1" GroupIndex="0" Caption="Category">
            </dx:GridViewDataTextColumn>

            <dx:GridViewDataDateColumn FieldName="Title" VisibleIndex="2" Caption="Experience Tags">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="CreatedBy" VisibleIndex="3" Caption="Inserted By">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataDateColumn FieldName="Created" VisibleIndex="3" Caption="Created On">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="Modified" VisibleIndex="4" Caption="Modified On">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>
        </Columns>

        <Settings ShowFooter="True" ShowHeaderFilterButton="true" VerticalScrollableHeight="210" VerticalScrollBarMode="Visible" />
        <Styles>
            <Row CssClass="homeGrid_dataRow"></Row>
            <Header CssClass="homeGrid_headerColumn"></Header>
        </Styles>
        <SettingsBehavior AutoExpandAllGroups="true" />
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
        <div style="float: right; padding: 15px 0;">
            <a id="lnkAddNewExperiencedTags" runat="server" href="" style="padding-left: 15px" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
</div>



<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserProjectExperiences.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.UserProjectExperiences" %>
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

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Text="Please Wait ..."
    Modal="True">
</dx:ASPxLoadingPanel>


<div class="col-md-12 col-sm-12 col-xs-12">

    <dx:ASPxGridView ID="aspxGridUserProjectExperiences" AutoGenerateColumns="False" runat="server" SettingsText-EmptyDataRow="No record found."
        KeyFieldName="ID" Width="99%" ClientInstanceName="aspxGridUserProjectExperiences">
        <Columns>       
            <dx:GridViewDataDateColumn FieldName="ProjectID" VisibleIndex="0" Caption="Project ID">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="UserId" VisibleIndex="1" Caption="" Visible="false">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="ResourceUser" VisibleIndex="1" Caption="Resource User">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>

             <dx:GridViewDataDateColumn FieldName="TagLookup" VisibleIndex="2" Caption="Tags" Visible="false">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="Title" VisibleIndex="2" Caption="Tags">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>

            <dx:GridViewDataDateColumn FieldName="Created" VisibleIndex="3" Caption="Created On">
                <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
            </dx:GridViewDataDateColumn>
        </Columns>

        <Settings ShowFooter="True" ShowHeaderFilterButton="true" />
        <Styles>
            <Row CssClass="homeGrid_dataRow"></Row>
            <Header CssClass="homeGrid_headerColumn"></Header>
        </Styles>
    </dx:ASPxGridView>

    <%--<div style="margin-top: 5px;">
        <div style="float: right; padding:15px 0;" >
            <a id="lnkAddNewExperiencedTags" runat="server" href="" style="padding-left: 15px" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>--%>
</div>



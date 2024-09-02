<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeTypesView.ascx.cs" Inherits="uGovernIT.Web.EmployeeTypesView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function openuEmpTypeDialog(path, params, titleVal, width, height, stopRefresh) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh);
    }

    function NewEmpTypeDialog() {
        window.parent.UgitOpenPopupDialog('<%= editUrl%>&ID=0', "", "Add Employee Type", '450px', '350px', 0);
    } 
</script>
<div class="col-md-12 col-sm-12 col-xs-12">
<div style="margin-bottom: 5px;">
    <div style="float: right">
        <div class="headerItem-addItemBtn" style="margin-bottom: 5px; margin-top:10px;">
            <a id="LinkButton1" runat="server" href="" style="padding-left:15px" class="primary-btn-link">
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>

    </div>
</div>
<dx:ASPxGridView ID="aspxGridEmployeeType" AutoGenerateColumns="False" runat="server" SettingsText-EmptyDataRow="No record found."
    KeyFieldName="ID" Width="100%" ClientInstanceName="aspxGridEmployeeType" CssClass="customgridview homeGrid" 
    OnHtmlRowPrepared="aspxGridEmployeeType_HtmlRowPrepared">
    <Columns>
        <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0">
            <DataItemTemplate>
                <img src="/Content/Images/editNewIcon.png" alt="Edit" style="width: 16px;" />
            </DataItemTemplate>
            <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
        </dx:GridViewDataTextColumn>

        <dx:GridViewDataDateColumn FieldName="Title" VisibleIndex="1" Caption="Title">
            <Settings HeaderFilterMode="CheckedList" SortMode="DisplayText" />
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataColumn FieldName="Description" VisibleIndex="3" Caption="Description">
            <Settings HeaderFilterMode="CheckedList" />
        </dx:GridViewDataColumn>
    </Columns>
    <Styles>
        <Row CssClass="homeGrid_dataRow"></Row>
        <Header CssClass="homeGrid_headerColumn"></Header>
    </Styles>
    <Settings ShowFooter="True" ShowHeaderFilterButton="true" />
</dx:ASPxGridView>

<div style="margin-top: 5px;">
    <div style="float: right">
        <a id="lnkAddNewEmployeeType" runat="server" href="" style="padding-left: 15px" class="primary-btn-link">
            <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
            <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
        </a>
    </div>
</div>
    </div>
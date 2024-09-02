<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardFactTablesView.ascx.cs" Inherits="uGovernIT.Web.DashboardFactTablesView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function changeRequestType(s, e) {
        debugger;
        LoadingPanel.Show();
        var val = s.mainElement.attributes.anu.value;
        grdDashbaords.PerformCallback(val);
    }
    function endcallback() {
        LoadingPanel.Hide();
    }
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxbButton {
        color: #ffffff;
        font: 12px Tahoma, Geneva, sans-serif;
        border: 0px solid #7F7F7F;
        background: #ffffff url(/DXR.axd?r=1_166-Hc4el) repeat-x top;
        padding: 1px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    $(function () {
        $(".cachehmode").bind("change", function () {

            $(".scheduledcontainter").addClass("hide");
            if ($(this).val() == "Scheduled") {
                $(".scheduledcontainter").removeClass("hide");

            }

        });
    });

    function importExcel(obj) {
        var path = "<%= importExcelPagePath %>";
        UgitOpenPopupDialog(path, "", "Import Excel", 55, 35, false, escape(window.location.href));
        return false;
    }
    function OpenAdd(obj) {
        var path = "<%= openAddPath %>";
        UgitOpenPopupDialog(path, "", "Dashboard Fact Table - New Item", "700px", "500px", false, escape(window.location.href));
        return false;
    }
    function exportList(obj) {
        var path = "<%= exportListPagePath %>";
        UgitOpenPopupDialog(path, "", "Export List", 40, 20, false, escape(window.location.href));
        return false;
    }

</script>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." CssClass="customeLoader" ClientInstanceName="LoadingPanel" Image-Url="~/Content/IMAGES/ajax-loader.gif" ImagePosition="Top"
    Modal="True">
</dx:ASPxLoadingPanel>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" style="margin-top:10px; margin-bottom:10px;">
        <dx:ASPxButton ID="ASPxButton2" Visible="true" runat="server" Text="Refresh All" OnClick="BtRefreshAll_Click"
            ToolTip="Refresh All" Style="float:left;margin-right:16px;" CssClass="primary-blueBtn" >
        </dx:ASPxButton>
        <asp:ImageButton ID="btImport" runat="server" ImageUrl="/_layouts/15/Images/uGovernIT/import.png" Style="margin-top: 2px; margin-left: 5px;"
            alt="import" OnClientClick="return importExcel(this)" />
        <asp:ImageButton ID="btExport" runat="server" ImageUrl="/_layouts/15/Images/uGovernIT/export.png" Style="margin-top: 2px; margin-left: 5px;"
            alt="Export" OnClientClick="return exportList(this)" />
         <span style="float: right;">
            <dx:ASPxButton ID="ASPxButton1" Visible="true" runat="server" Text="Add" Image-Url="~/Content/Images/plus-symbol.png"
                ToolTip="Add" Style="float:left;margin-right:16px;" Image-Width="10px" CssClass="primary-blueBtn" AutoPostBack ="false" >
                <ClientSideEvents Click="function(s,e){return OpenAdd(s);}" />
            </dx:ASPxButton>
        </span>
    </div>
    <div class="row">
        <div>
            <asp:Label ID="lbErrorMessag" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <ugit:ASPxGridView ID="grdDashbaords" Width="100%" runat="server" KeyFieldName="ID" CssClass="customgridview homeGrid" 
            OnHtmlDataCellPrepared="grdDashbaords_HtmlDataCellPrepared" ClientInstanceName="grdDashbaords" 
            SettingsPager-Mode="ShowAllRecords">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Fact Table">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <%--<dx:GridViewDataTextColumn FieldName="Description" Caption="Description"></dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn FieldName="CacheTable" Caption="Cache Table"></dx:GridViewDataTextColumn>
                <%--<dx:GridViewDataTextColumn FieldName="CacheThreshold" Caption="CacheThreshold"></dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn FieldName="CacheMode" Caption="Cache Mode" ></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Status" Caption="Status"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ExpiryDate" Caption="Expiry Date"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CacheTable" Caption="Cache Table" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Name="aEdit" Width="5%">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" runat="server" style="width:16px" src="/Content/Images/editNewIcon.png"/>
                        </a>
                        <asp:ImageButton ID="btRefreshItem" runat="server" CommandArgument="<%# Eval(DatabaseObjects.Columns.Id) %>"
                            OnClick="BtRefreshItem_Click" AlternateText="Refresh" ImageUrl="/Content/images/refresh-icon.png" />
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
            <Settings ShowHeaderFilterButton="true" />
        </ugit:ASPxGridView>
    </div>
    <div class="row" style="margin-top:15px;margin-bottom:15px;">
         <span style="float: right;">
            <dx:ASPxButton ID="btnSave" Visible="true" runat="server" Text="Add" Image-Url="~/Content/Images/plus-symbol.png"
                ToolTip="Add" Style="float:left;margin-right:16px;" Image-Width="10px" CssClass="primary-blueBtn" AutoPostBack ="false" >
                    <ClientSideEvents Click="function(s,e){return OpenAdd(s);}" />
            </dx:ASPxButton>
        </span>
    </div>
</div>

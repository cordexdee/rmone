<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetRelatedWithTickets.ascx.cs" Inherits="uGovernIT.Web.AssetRelatedWithTickets" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Import Namespace="System.Data" %>

<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_beginRequest(BeginRequestHandler);

    prm.add_pageLoading(MyPageLoading);
    prm.add_endRequest(EndRequest);
    var btnId;

    function InitializeRequest(sender, args) {
        sender.abortPostBack();
    }

    function BeginRequestHandler(sender, args) {
        //  alert(sender.length);
    }

    function MyPageLoading(sender, args) {
    }

    function EndRequest(sender, args) {
        var s = sender;
        var a = args;
        var msg = null;
        if (a._error != null) {
            switch (args._error.name) {
                case "Sys.WebForms.PageRequestManagerServerErrorException":
                    msg = "PageRequestManagerServerErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerParserErrorException":
                    msg = "PageRequestManagerParserErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerTimeoutException":
                    msg = "PageRequestManagerTimeoutException";
                    break;
            }
            args._error.message = "My Custom Error Message " + msg;
            args.set_errorHandled(true);
        }
    }

    var hrefTreeNodeText = null;
    function OverOnDeleteButton(control) {
        hrefTreeNodeText = control.parentNode.parentNode.getAttribute("href");
        control.parentNode.parentNode.setAttribute("href", "javascript:");
    }

    function OverOutOnDeleteButton(control) {
        if (hrefTreeNodeText) {
            control.parentNode.parentNode.setAttribute("href", hrefTreeNodeText);
        }
    }


</script>
<asp:Panel runat="server" ID="AssetiwthIncidentPanel">


    <table cellpadding="0" cellspacing="0" border="0" style="padding-bottom: 5px;">
        <tr>
            <td id="addRelationPanel" runat="server" style="padding-top: 20px;">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="padding-bottom: 10px">
                            <a id="aAddItem" runat="server" href="">
                                <img id="Img1" runat="server" src="/Content/Images/add_icon.png" />
                                <asp:Label ID="LblAddItem" runat="server" Text="Add Child Ticket"></asp:Label>
                            </a>
                        </td>
                        <td>
                            <asp:UpdateProgress ID="addRelationUpdateProgress" runat="server">
                                <ProgressTemplate>
                                    <asp:Panel ID="updateProgressPanel" runat="server">
                                        <img alt="Loading.." style="vertical-align: middle;" src="/_layouts/15/images/loadingcirclests16.gif">
                                    </asp:Panel>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <asp:Panel ID="pnlRelatedTicket" runat="server">
        <div class="doc-subcontainer">
            <ugit:ASPxGridView ID="grdAssetRelatedTicket" runat="server" AutoGenerateColumns="false"  Images-HeaderActiveFilter-Url="/Content/images/uGovernIT/Filter_Red_16.png" ClientInstanceName="grdAssetRelatedTicket"
                Width="100%" KeyFieldName="TicketId" CssClass="customgridview   " OnRowDeleting="grdAssetRelatedTicket_RowDeleting" OnHtmlDataCellPrepared="grdAssetRelatedTicket_HtmlDataCellPrepared">
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Ticket" FieldName="TicketId" PropertiesTextEdit-EncodeHtml="false" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center" VisibleIndex="0" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" CellStyle-Wrap="True" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
                    
                    <dx:GridViewDataTextColumn Caption="Priority" FieldName="PriorityLookup" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Progress" FieldName="Status" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Age" FieldName="Age" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Created" FieldName="CreationDate" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                    <dx:GridViewCommandColumn Caption="#" ShowDeleteButton="true" ButtonType="Image">
                    </dx:GridViewCommandColumn>
                </Columns>
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsBehavior AllowSort="true" />
                <Settings ShowHeaderFilterButton="true" />
                <Styles>
                    <AlternatingRow Enabled="True" BackColor="#EAEAEA" />
                    <Header Font-Bold="true" />
                </Styles>
                <SettingsCommandButton>
                    <DeleteButton>
                        <Image Url="/Content/images/delete-icon-new.png"></Image>
                    </DeleteButton>
                </SettingsCommandButton>
                <SettingsBehavior ConfirmDelete="true" />
                <SettingsText ConfirmDelete="Are you sure you want to delete the relationship to this item?" EmptyDataRow="There are no items to show in this view." />
            </ugit:ASPxGridView>
        </div>
    </asp:Panel>
</asp:Panel>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiRelatedTickets.ascx.cs" Inherits="uGovernIT.Web.WikiRelatedTickets" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .rptRequestsTd td {
        height: 20px;
        padding-left: 10px;
    }

    .lblTitle {
        font-weight: bold;
        color: #000066;
    }

    .lblText {
        color: #000066;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">


    function openEditDialog(obj) {
        var ticketId = $.trim($(obj).attr('ticketId'));
        var url = '<%=detailsUrl %>' + "&ticketId=" + ticketId;
        window.parent.UgitOpenPopupDialog(url, "", 'View Wiki Article', '1200px', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }


    function adjustControlSize() {
        setTimeout(function () {
            try {
                grid.AdjustControl();
            } catch (ex) { }
        }, 10);
    }

</script>
<div>
    
    <div>

        <table cellpadding="0" cellspacing="0" class="rptRequests">

            <tr>
                <td>

                    <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="grid" EnableCallBacks="true" EnableViewState="false" 
                        Width="100%" KeyFieldName="ID">

                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="Ticket" VisibleIndex="0" HeaderStyle-Font-Bold="true">
                                <DataItemTemplate>
                                    <a id="lblTicketId" runat="server" ticketid='<%#Eval("TicketId") %>' onclick="openEditDialog(this);" style="cursor: pointer;"><%#Eval("TicketId") %></a>
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataTextColumn FieldName="Title" HeaderStyle-Font-Bold="true" />
                            <dx:GridViewDataTextColumn FieldName="RequestTypeLookup" Caption="Request Type" HeaderStyle-Font-Bold="true" />
                            <dx:GridViewDataTextColumn FieldName="WikiViews" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" />


                        </Columns>

                        <SettingsBehavior AutoExpandAllGroups="false" AllowGroup="true" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                        <Settings GridLines="Horizontal" VerticalScrollBarMode="Auto" VerticalScrollableHeight="80" />
                        <SettingsCookies Enabled="false" />
                        <SettingsPopup>
                            <HeaderFilter Height="200" />
                        </SettingsPopup>
                        <SettingsPager Mode="ShowAllRecords" Position="TopAndBottom">
                        </SettingsPager>
                    </dx:ASPxGridView>


                </td>
            </tr>
        </table>
    </div>

     <div style="padding-top: 4px;" id="divAddNewRelatedWiki" runat="server" visible="false">
        <div style="float: left;padding-right: 20px;">
            <a id="aAddItem" runat="server" href="">
                <img id="Img1" runat="server" src="/Content/Images/add_icon.png" style="border:none;" />
                <asp:Label ID="LblAddItem" runat="server" Text="Relate To Existing Wiki"></asp:Label>
            </a>
        </div>
        <div>
            <a id="aAddNew" runat="server" href="">
                <img id="Img2" runat="server" src="/Content/Images/add_icon.png" style="border:none;" />
                <asp:Label ID="lblAddNew" runat="server" Text="Add New Related Wiki"></asp:Label>
            </a>
        </div>
    </div>
</div>

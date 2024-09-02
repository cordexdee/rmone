<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectFolder.ascx.cs" Inherits="uGovernIT.Web.SelectFolder" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function onEditButtonClick(s, e) {
        popupSelectFolder.Show();
    }
    $(document).ready(function () {
        $('#<%= ddlPortal.ClientID%>').on('change', function () {
            var seletedvalue = $('#<%= ddlPortal.ClientID%>').val();
            var seletedtext = $("#<%= ddlPortal.ClientID%> option[value='" + seletedvalue + "']").html();
            $('#<%= hdnportalName.ClientID%>').val(seletedtext)
        });
    });
</script>

<dx:ASPxImage ID="edit" runat="server" ImageUrl="/Content/images/Admin/editNewIcon.png">
    <ClientSideEvents Click="onEditButtonClick" />
</dx:ASPxImage> 

<asp:HiddenField ID="hdnportalName" runat="server" />
<dx:ASPxPopupControl ID="popupSelectFolder" runat="server" ClientInstanceName="popupSelectFolder"
    Width="600px" Height="300px" MaxWidth="800px" MaxHeight="800px" MinHeight="150px" MinWidth="150px"
    HeaderText="Pick any folder" EnableViewState="false" PopupHorizontalAlign="WindowCenter" Modal="true"
    PopupVerticalAlign="WindowCenter" PopupAnimationType="Fade" EnableHierarchyRecreation="True" >
    <ContentCollection>
        <dx:PopupControlContentControl ID="popupContentControl" runat="server">
            <div>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 30%;">
                            <span>
                                <dx:ASPxImage runat="server" ID="imgExpand" Text="Expand All Rows"
                                    ImageUrl="/Content/images/expand-all-new.png"
                                    AutoPostBack="false">
                                    <ClientSideEvents Click="function(s,e) { treeview.ExpandAll(); }" />
                                </dx:ASPxImage>
                            </span>
                            <span>
                                <dx:ASPxImage runat="server" ID="imgCollapse" Text="Collapse All Rows"
                                    ImageUrl="/Content/images/collapse-all-new.png"
                                    AutoPostBack="false">
                                    <ClientSideEvents Click="function(s,e) { treeview.CollapseAll(); }" />
                                </dx:ASPxImage>
                            </span>

                        </td>
                        <td style="width: 70%;">

                            <div id="divSelectPortal" runat="server" style="float: right;">
                                <span>
                                    <b>
                                        <asp:Label ID="lblType" Text="Type:" runat="server" /></b>
                                    <asp:DropDownList ID="ddltype" runat="server" AutoPostBack="true" >
                                        <asp:ListItem Text="All Portals" Value="allportals"></asp:ListItem>
                                        <asp:ListItem Text="Project" Value="project"></asp:ListItem>
                                        <asp:ListItem Text="Generic" Value="nonproject"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                                <span>
                                    <b>
                                        <asp:Label ID="lblPortal" Text="Portal:" runat="server" /></b>
                                    <asp:DropDownList ID="ddlPortal" runat="server" AutoPostBack="true" >
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <dx:ASPxTreeView ID="aspxtvPortal" runat="server" ClientInstanceName="treeview"
                                EnableAnimation="false" EnableCallBacks="true" EnableClientSideAPI="true"
                                AllowSelectNode="true" Width="200px">
                                <Images NodeImage-Width="13px">
                                </Images>
                                <Styles NodeImage-Paddings-PaddingTop="2px">
                                </Styles>
                            </dx:ASPxTreeView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="float: right">
                                <dx:ASPxButton ID="btnDone" ValidationGroup="selectfolder" AutoPostBack="true" runat="server" Text="Done" OnClick="btnDone_Click"></dx:ASPxButton>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>


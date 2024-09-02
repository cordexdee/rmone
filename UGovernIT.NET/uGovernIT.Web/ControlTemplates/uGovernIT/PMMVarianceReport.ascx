<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMVarianceReport.ascx.cs" Inherits="uGovernIT.Web.PMMVarianceReport" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<asp:Panel ID="projectVarianceEditMode" runat="server">

    <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        .fleft {
            float: left;
        }

        .fullwidth {
            width: 99%;
        }
    </style>
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_pageLoading(MyPageLoading);
        prm.add_endRequest(EndRequest);
        var btnId;
        function InitializeRequest(sender, args) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
        }
        var notifyId = "";
        function AddNotification(msg) {
            if (notifyId == "") {
                notifyId = DevExpress.ui.notify({ message: "Processing...", width: 150, height: 45, shading: false }, "success", 700);
            }
        }
        function RemoveNotification() {
            //SP.UI.Notify.removeNotification(notifyId);
            notifyId = '';
            DevExpress.ui.notify({ message: "Processing...", width: 150, height: 45, shading: false }, "success", 700);
        }
        function BeginRequestHandler(sender, args) {
            //AddNotification("Processing ..");
            DevExpress.ui.notify({ message: "Processing...", width: 150, height: 45, shading: false }, "success", 700);
        }

        function EndRequest(sender, args) {
            window.parent.adjustIFrameWithHeight("<%=FrameId %>", $(".managementcontrol-main").height());
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
            else {
                RemoveNotification();
            }
        }

        function MyPageLoading(sender, args) {
        }

        function EditCostVarianceOnbdClick(rowIndex) {
            var listViewId = "<%= lvCostVariances.ClientID%>";
            var editBt = document.getElementById(listViewId + "_ctrl" + rowIndex + "_lnkEdit");
            if (editBt) {
                editBt.click();
            }
        }
    </script>
    <div class="mainblock">
        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
            <Triggers></Triggers>
            <ContentTemplate>
                <asp:Label runat="server" ID="costVarianceMessage" BackColor="#F4F4F4" Width="100%" Style="text-align: center;"></asp:Label>
                <asp:ListView ID="lvCostVariances" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1"
                    DataKeyNames='Title'
                    OnItemEditing="LvCostVariances_ItemEditing" OnItemCanceling="LvCostVariances_ItemCanceling"
                    OnItemUpdating="LvCostVariances_ItemUpdating">
                    <LayoutTemplate>
                        <table class="ms-listviewtable worksheettable" style="border-collapse: collapse" width="100%" cellpadding="0" cellspacing="0">
                            <tr class="worksheetheader ms-viewheadertr">
                                <th class="ms-vh2 paddingfirstcell" width="10%">
                                    <b>Item</b>
                                </th>
                                <th class="ms-vh2 alncenter" width="10%">
                                    <b>Plan</b>
                                </th>
                                <th class="ms-vh2 alncenter" width="10%">
                                    <b>Re-Plan</b>
                                </th>
                                <th class="ms-vh2 alncenter" width="10%">
                                    <b>Actual</b>
                                </th>
                                <th class="ms-vh2 alncenter" width="10%">
                                    <b>Variance 1</b>
                                </th>
                                <th class="ms-vh2 alncenter" width="10%">
                                    <b>Variance 2</b>
                                </th>
                                <th class="ms-vh2 alncenter" width="26%">
                                    <b>Note</b>
                                </th>
                                <th class="ms-vh2 alncenter" width="4%">&nbsp;
                                </th>
                            </tr>
                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr ondblclick="EditCostVarianceOnbdClick(<%# Container.DataItemIndex %>)">
                            <td class="ms-vb2 paddingfirstcell "><%# Eval("Title")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Plan")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Replan")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Actual")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Variance1")%></td>
                            <td class="ms-vb2 alncenter"><%#Eval("Variance2")%></td>
                            <td class="ms-vb2 alncenter"><%#Eval("Note")%></td>
                            <td class="ms-vb2 alncenter">
                                <asp:ImageButton runat="server" ID="lnkEdit" CommandName="Edit" ImageUrl="/Content/images/edit-icon.png" BorderWidth="0" ToolTip="Edit" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="ms-alternatingstrong" ondblclick="EditCostVarianceOnbdClick(<%# Container.DataItemIndex %>)">
                            <td class="ms-vb2 paddingfirstcell "><%# Eval("Title")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Plan")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Replan")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Actual")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Variance1")%></td>
                            <td class="ms-vb2 alncenter"><%#Eval("Variance2")%></td>
                            <td class="ms-vb2 alncenter"><%#Eval("Note")%></td>
                            <td class="ms-vb2 alncenter">
                                <asp:ImageButton runat="server" ID="lnkEdit" CommandName="Edit" ImageUrl="/Content/images/edit-icon.png" BorderWidth="0" ToolTip="Edit" />
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EditItemTemplate>
                        <tr>
                            <td class="ms-vb2 paddingfirstcell "><%# Eval("Title")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Plan")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Replan")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Actual")%></td>
                            <td class="ms-vb2 alncenter"><%# Eval("Variance1")%></td>
                            <td class="ms-vb2 alncenter"><%#Eval("Variance2")%></td>
                            <td class="ms-vb2 alncenter">
                                <asp:TextBox Height="20" ID="txtNote" CssClass="fullwidth" runat="server" Text='<%#Eval("Note")%>'></asp:TextBox></td>
                            <td class="ms-vb2 alncenter">
                                <span>
                                    <asp:LinkButton ID="btnUpdate" runat="server" Text="<img style='border:0px;' src='/Content/images/newsaveicon.png' alt='Update'/>" CommandName="Update" />
                                </span>
                                <asp:LinkButton ID="btnCancel" runat="server" Text="<img style='border:0px;' src='/Content/images/cancel.png' alt='Cancel'/>" CommandName="Cancel" />
                            </td>
                        </tr>
                    </EditItemTemplate>
                </asp:ListView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Panel>

<asp:Panel ID="projectVarianceReadMode" runat="server">


    <asp:ListView ID="lvRoCostVariances" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1">
        <LayoutTemplate>
            <table class="ro-table" style="border-collapse: collapse" width="100%" cellpadding="0" cellspacing="0">
                <tr class="ro-header">
                    <th class="ro-padding" width="20%">
                        <b>Item</b>
                    </th>
                    <th class="ro-padding" width="10%">
                        <b>Plan</b>
                    </th>
                    <th class="ro-padding" width="10%">
                        <b>Re-Plan</b>
                    </th>
                    <th class="ro-padding" width="10%">
                        <b>Actual</b>
                    </th>
                    <th class="ro-padding" width="10%">
                        <b>Variance 1</b>
                    </th>
                    <th class="ro-padding" width="10%">
                        <b>Variance 2</b>
                    </th>
                    <th class="ro-padding" width="30%">
                        <b>Note</b>
                    </th>
                </tr>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr class="ro-item">
                <td class="ro-padding"><%# Eval("Title")%></td>
                <td class="ro-padding"><%# Eval("Plan")%></td>
                <td class="ro-padding"><%# Eval("Replan")%></td>
                <td class="ro-padding"><%# Eval("Actual")%></td>
                <td class="ro-padding"><%# Eval("Variance1")%></td>
                <td class="ro-padding"><%#Eval("Variance2")%></td>
                <td class="ro-padding"><%#Eval("Note")%></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="ro-alternateitem">
                <td class="ro-padding"><%# Eval("Title")%></td>
                <td class="ro-padding"><%# Eval("Plan")%></td>
                <td class="ro-padding"><%# Eval("Replan")%></td>
                <td class="ro-padding"><%# Eval("Actual")%></td>
                <td class="ro-padding"><%# Eval("Variance1")%></td>
                <td class="ro-padding"><%#Eval("Variance2")%></td>
                <td class="ro-padding"><%#Eval("Note")%></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:ListView>
</asp:Panel>

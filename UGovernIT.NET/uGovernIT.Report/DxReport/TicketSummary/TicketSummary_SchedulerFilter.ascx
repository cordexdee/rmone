<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketSummary_SchedulerFilter.ascx.cs" Inherits="uGovernIT.Report.DxReport.TicketSummary_SchedulerFilter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<script type="text/javascript">
    function ShowHideCheckbox() {
        var moduleName = $("#<%=ddlModules.ClientID%>").val();
        if (moduleName.toLowerCase() == "all") {
            $("#<%=chkSortByModule.ClientID%>").parent().css("display", "block");
            $("#<%=chkSortByModule.ClientID%>").attr('checked', 'checked');
        }
        else {
            $("#<%=chkSortByModule.ClientID%>").parent().css("display", "none");
            $("#<%=chkSortByModule.ClientID%>").removeAttr('checked');
        }
    }
</script>

<div id="divTicketsReportsPopup" style="width: 100%">
    <fieldset>
        <legend class="reportTitle">Ticket Summary Options</legend>

        <table style="padding-top: 10px; width: 100%;">
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Modules</h3>
                </td>
                <td class="ms-formbody">
                    <asp:DropDownList ID="ddlModules" runat="server" onchange="ShowHideCheckbox();" Width="230px" CssClass="drp-module"></asp:DropDownList>
                    <asp:CheckBox ID="chkSortByModule" runat="server" Checked="false" CssClass="chk-shortbymodule" Text="Sort By Module First" />
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Ticket Status</h3>
                </td>
                <td class="ms-formbody">
                    <asp:RadioButtonList ID="rdbtnLstFilterCriteria" runat="server" RepeatDirection="Horizontal" CssClass="rdFilterCriteria filter">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="Open" Value="Open" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Sort By</h3>
                </td>
                <td class="ms-formbody">
                    <asp:RadioButtonList ID="rdSortCriteria" runat="server" RepeatDirection="Horizontal" CssClass="rdFilterCriteria sort">
                        <asp:ListItem Text="Oldest First" Value="oldesttonewest" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Newest First" Value="newesttooldest"></asp:ListItem>
                        <asp:ListItem Text="Waiting On" Value="waitingon"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <%-- <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">User<b style="color: Red;">*</b></h3>
                </td>
                <td class="ms-formbody">
                    <SharePoint:PeopleEditor PrincipalSource="UserInfoList"  ID="ppeCurrentUser" MaximumHeight="30" Width="300px" CssClass="peAssignedTo"
                        SelectionSet="User,SPGroup" runat="server" MultiSelect="false" PlaceButtonsUnderEntityEditor="false" AugmentEntitiesFromUserInfo="true" />
                    <asp:CustomValidator ID="cvOwner" runat="server" Enabled="true"></asp:CustomValidator>
                </td>
            </tr>--%>
            <%--<tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Date Created</h3>
                    <td class="ms-formbody">
                        <div class="top_right_nav" style="left: 0px; float: left;">
                            <span>
                                <span>
                                    <b style="padding-top: 2px; float: left; font-weight: normal;">From:</b>
                                    <span>
                                        <dx:ASPxDateEdit ID="dtFromTicketSummary" runat="server" ></dx:ASPxDateEdit>
                                      
                                    </span>
                                </span>

                                <span>
                                    <b style="padding-top: 2px; float: left; font-weight: normal;">To:</b>
                                    <span>
                                        <dx:ASPxDateEdit ID="dtToTicketSummary" runat="server"></dx:ASPxDateEdit>
                                    
                                    </span>
                                </span>
                            </span>
                        </div>
                    </td>
            </tr>--%>
            <tr>

                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Date Range</h3>

                    <%-- <asp:Label ID="Label6" runat="server" Text="Date Range" Font-Bold="true"> </asp:Label></td>--%>
                <td>
                    <div class="top_right_nav" style="left: 0px; float: left; margin-top: 6px; width: 100%;">
                        <span id="spanNoOfDays" runat="server" style="float: left; width: 100%; margin: 5px; margin-left: 0px">

                            <span style="float: left; padding-top: 5px;">From:</span>
                            <span style="float: left">
                                <dx:ASPxSpinEdit ID="txtValueFrom" MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit>
                            </span>

                            <span style="float: left; padding-top: 5px">To:</span>
                            <span style="float: left">
                                <dx:ASPxSpinEdit ID="txtValueTo" MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit>
                            </span>
                            <span style="float: left">
                                <dx:ASPxComboBox ID="ddlDateUnitsFrom" Width="60px" runat="server">
                                    <Items>
                                        <dx:ListEditItem Text="Days" Selected="true" Value="0" />
                                        <dx:ListEditItem Text="Weeks" Value="1" />
                                        <dx:ListEditItem Text="Months" Value="1" />
                                    </Items>
                                </dx:ASPxComboBox>

                            </span>
                        </span>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
    <table style="width: 100%;">
        <tr>
            <td style="width: 50%;">&nbsp;
            </td>
            <td style="text-align: right;">
                <div class="addEditPopup-btnWrap">
                    <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" OnClick="lnkCancel_Click" CssClass="secondary-cancelBtn" AutoPostBack="false">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="lnkSubmit" runat="server" Text="Schedule Report" OnClick="lnkSubmit_Click" CssClass="primary-blueBtn" AutoPostBack="false">
                    </dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
</div>

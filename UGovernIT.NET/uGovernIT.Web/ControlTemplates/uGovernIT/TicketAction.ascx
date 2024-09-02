<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketAction.ascx.cs" Inherits="uGovernIT.Web.TicketAction" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formlabel {
        width: 170px;
        text-align: right;
    }

    .full-width {
        width: 90%;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .text-error {
        color: red;
        font-weight: 500;
        margin-top: 5px;
    }

    div.ms-inputuserfield {
        height: 17px;
    }

    .btnDelete {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/_layouts/15/images/uGovernIT/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
    }

    .required-item:after {
        content: '*';
        color: red;
        font-weight: bold;
    }

    .alignUnauthorizedTicketLabel {
    }

    .first_tier_nav {
        width: 99%;
        float: left;
        height: 23px;
        padding: 10px;
        padding-left: 0px;
    }

    .buttoncell {
        height: 6px;
    }

    .alignIsAuthorizedlabel {
        top: 100px;
        bottom: 100px;
        left: 50px;
    }
</style>

<script type="text/javascript">
    function validate(obj) {

        var moduleName = '<%=this.ModuleName%>';


        if ($(obj).attr("Name") == "Hold") {
            if (aspxdtOnHoldDate.date == null) {
                $('#<%=lblHoldMessage.ClientID%>').html("Please enter the Hold Till date");
                return false;
            }
        }
        else if ($(obj).attr("Name") == "UnHold") {
            if ($('#<%=popedUnHoldComments.ClientID%>').val() == "") {
                $('#<%=lblUnHoldMessage.ClientID%>').html("Please enter comment");
                return false;
            }
        }
        else if ($(obj).attr("Name") == "Return") {
            if ($('#<%=popedReturnComments.ClientID%>').val() == "") {
                $('#<%=lblReturnMessage.ClientID%>').html("Please enter comment");
                return false;
            }
        }
        else if ($(obj).attr("Name") == "Reject") {
            if ($('#<%=popedRejectComments.ClientID%>').val() == "") {
                $('#<%=lblRejectMessage.ClientID%>').html("Please enter comment");
                return false;
            }
        }
        else if ($(obj).attr("Name") == "ApproveBudget") {
            if ($('#<%=popedApproveBudgetComments.ClientID%>').val() == "") {
                $('#<%=lblApproveBudgetMessage.ClientID%>').html("Please enter comment");
                return false;
            }
        }
        else if ($(obj).attr("Name") == "RejectBudget") {
            if ($('#<%=popedRejectBudgetComments.ClientID%>').val() == "") {
                $('#<%=lblRejectBudgetMessage.ClientID%>').html("Please enter comment");
                return false;
            }
        }

        alpLoading.Show();
        return true;
    }

</script>

<dx:ASPxLoadingPanel ID="alpLoading" ClientInstanceName="alpLoading" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>

<asp:Panel ID="putOnHold" runat="server" Visible="false">

    <div style="height: auto; width: auto;">

        <table style="width: 99%;">
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblHoldMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"> </asp:Label></td>
            </tr>
            <tr>
                <td style="text-align: right;">Hold Till<b style='color: red'>* </b></td>
                <td style="padding-left: 10px; padding-bottom: 10px;">
                    <dx:ASPxDateEdit ID="aspxdtOnHoldDate" ClientInstanceName="aspxdtOnHoldDate" runat="server" Width="99%"></dx:ASPxDateEdit>
                    <%-- <dx:ASPxDateEdit ID="aspxdtOnHoldDate" Width="250px" runat="server" ClientInstanceName="aspxdtOnHoldDate"></dx:ASPxDateEdit>--%>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">Reason</td>
                <td style="padding-left: 10px; padding-bottom: 10px;">
                    <asp:DropDownList ID="ddlOnHoldReason" runat="server" Width="99%" EnableViewState="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">Comment</td>
                <td style="padding-left: 10px; padding-bottom: 10px;">
                    <asp:TextBox runat="server" ID="popedHoldComments" Width="95.5%" Height="101px" Columns="30" Rows="5" TextMode="MultiLine" Text=""></asp:TextBox></td>
            </tr>

            <tr>
                <td colspan="2">
                    <div  class="first_tier_nav">
                    <ul style="float: right; padding-bottom:2px;">
                        <li runat="server" id="Li11" class="Red" onmouseover="this.className='tabhoverRed'" onmouseout="this.className='Red'">
                            <asp:LinkButton runat="server" ID="HoldButton" Style="color: white" CssClass="lock" Text="Hold" Name="Hold" OnClick="HoldButton_Click" OnClientClick="javascript:return validate(this)" />

                        </li>
                        <li runat="server" id="Li12" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <a id="LinkButton2" style="color: white"
                                onclick="window.frameElement.commitPopup();"
                                class="cancelwhite">Cancel</a>
                        </li>
                    </ul>
                        </div>
                </td>
            </tr>
        </table>

    </div>

</asp:Panel>
<%--hold section ::end--%>

<%--unhold section ::start--%>

<asp:Panel ID="unHold" runat="server" Visible="false">
    <div>
        <table style="width: 99%;">
            <tr>    
                <td>
                    <asp:Label ID="lblUnHoldMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="popedUnHoldComments" Width="425px" Height="150px" Columns="52" Rows="9" TextMode="MultiLine" Text=""></asp:TextBox></td>
            </tr>

            <tr>
                <td class="buttoncell">
                    <div class="first_tier_nav">
                    <ul style="float: right; padding-bottom:2px;">

                        <li runat="server" id="Li13" class="Green"  onmouseover="this.className='tabhoverGreen'" onmouseout="this.className='Green'">
                            <asp:LinkButton runat="server" ID="UnHoldButton" Style="color: white" Name="UnHold" Text="Remove hold" OnClick="UnHoldButton_Click"
                                OnClientClick="javascript:return validate(this)" CssClass="unlock" />
                        </li>
                        <li runat="server" id="Li14" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <a id="LinkButton3" style="color: white" class="cancelwhite" href="javascript:void(0);" onclick="window.frameElement.commitPopup();">Cancel</a>
                        </li>

                    </ul>
                        </div>
                </td>
            </tr>

        </table>
    </div>
</asp:Panel>

<%--unhold section ::end--%>


<%--return section ::start--%>
<asp:Panel ID="returnTicket" runat="server" Visible="false">
    <div>
        <table style="width: 96%;">
            <tr>
                <td>
                    <asp:Label ID="lblReturnMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label></td>
            </tr>

            <tr>
                <td>
                    <asp:TextBox runat="server" ID="popedReturnComments" Width="412px" Height="150px" Columns="6" Rows="9" TextMode="MultiLine" Text=""></asp:TextBox></td>
            </tr>

            <tr>
                <td class="buttoncell">
                    <div  class="first_tier_nav">
                    <ul style="float: right; padding-bottom:2px;">
                        <li runat="server" id="Li7" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="color: red">
                            <asp:LinkButton runat="server" ID="returnButton" Style="color: white" Name="Return" OnClick="returnButton_Click"
                                OnClientClick="javascript:return validate(this)"
                                Text="OK" CssClass="return" />
                        </li>
                        <li runat="server" id="Li8" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <a style="color: white" class="cancelwhite" onclick="window.frameElement.commitPopup();" href="javascript:void(0);">Cancel</a>
                        </li>
                    </ul>
                        </div>
                </td>
            </tr>
        </table>
    </div>

</asp:Panel>

<%--return section ::end--%>


<%--reject section ::start--%>
<asp:Panel ID="reject" runat="server" Visible="false">
    <div>
        <table style="width: 99%;">
            <tr>
                <td>This will close/cancel this workflow and move it to the last stage. If configured, a notification will also be sent.<br /><br />Are you sure you want to proceed? If so please enter the reason below.</td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRejectMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"> </asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="popedRejectComments" Width="390px" Height="100px" Columns="6" Rows="6" TextMode="MultiLine" Text=""></asp:TextBox></td>
            </tr>
            <tr>
                <td class="buttoncell">
                    <div class="first_tier_nav">
                    <ul style="float: right; padding-bottom:2px;">
                        <li runat="server" id="Li5" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <asp:LinkButton runat="server" CssClass="reject" ID="rejectButton" Style="color: white" Name="Reject" OnClick="rejectButton_Click" OnClientClick="javascript:return validate(this)"
                                Text="Yes, Proceed" />
                        </li>

                        <li runat="server" id="Li6" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <a style="color: white" onclick="window.frameElement.commitPopup();" href="javascript:void(0);" class="cancelwhite">No</a>
                        </li>
                    </ul>
                        </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
<%--reject section ::end--%>

<%--Approve section ::start--%>
<asp:Panel ID="approve" runat="server" Visible="false">
    <div>
        <table style="width: 99%;">
            <tr>
                <td style="text-align: center; padding: 37px 0px">Are you sure you want to approve this project request?
                </td>
            </tr>
            <tr>
                <td class="">
                    <div class="first_tier_nav" style="width: 99% !important">
                        <ul style="float: right; padding-bottom:2px;">
                            <li runat="server" id="Li1" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                <asp:LinkButton runat="server" CssClass="approve" ID="approveButton" Style="color: white" Name="Approve" OnClick="approveButton_Click"
                                    Text="Yes, Proceed" />
                            </li>

                            <li runat="server" id="Li2" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                <a style="color: white" onclick="window.frameElement.commitPopup();" href="javascript:void(0);" class="cancelwhite">No</a>
                            </li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
<%--Approve section ::end--%>


<asp:Panel ID="isAuthrizedPanel" runat="server" Visible="true">
    <div style="text-align: center;">
        <table style="width: 100%;">
            <tr>
                <td colspan="2">
                    <asp:Label ID="isAuthrizedMsg" runat="server" CssClass="alignIsAuthorizedlabel" Text="" Font-Size="Smaller" ForeColor="Red"> </asp:Label>

                </td>
            </tr>

        </table>
    </div>
</asp:Panel>


<%--Approve Budget section ::start--%>
<asp:Panel ID="approveBudget" runat="server" Visible="false">
    <div>
        <table style="width: 99%;">
            <tr>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblApproveBudgetMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"> </asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="popedApproveBudgetComments" Width="425px" Height="132px" Columns="6" Rows="9" TextMode="MultiLine" Text=""></asp:TextBox></td>
            </tr>
            <tr>
                <td class="buttoncell">
                    <div class="first_tier_nav">
                    <ul style="float: right; padding-bottom:2px;">
                        <li runat="server" id="Li1000" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <asp:LinkButton runat="server" CssClass="approve" ID="approveBudgetButton" Style="color: white" Name="ApproveBudget" OnClick="approveBudgetButton_Click" OnClientClick="javascript:return validate(this)"
                                Text="Approve" />
                        </li>

                        <li runat="server" id="Li3" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <a style="color: white" onclick="window.frameElement.commitPopup();" href="javascript:void(0);" class="cancelwhite">Cancel</a>
                        </li>
                    </ul>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
<%--Approve Budget section ::end--%>


<%--reject Budget section ::start--%>
<asp:Panel ID="rejectBudget" runat="server" Visible="false">
    <div>
        <table style="width: 99%;">
            <tr>
                <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRejectBudgetMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"> </asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="popedRejectBudgetComments" Width="425px" Height="132px" Columns="6" Rows="9" TextMode="MultiLine" Text=""></asp:TextBox></td>
            </tr>
            <tr>
                <td class="buttoncell">
                    <div class="first_tier_nav">
                    <ul style="float: right; padding-bottom:2px;">
                        <li runat="server" id="Li100" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <asp:LinkButton runat="server" CssClass="reject" ID="rejectBudgetButton" Style="color: white" Name="RejectBudget" OnClick="rejectBudgetButton_Click" OnClientClick="javascript:return validate(this)"
                                Text="Reject" />
                        </li>

                        <li runat="server" id="Li123" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <a style="color: white" onclick="window.frameElement.commitPopup();" href="javascript:void(0);" class="cancelwhite">Cancel</a>
                        </li>
                    </ul>
                        </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
<%--reject Budget section ::end--%>
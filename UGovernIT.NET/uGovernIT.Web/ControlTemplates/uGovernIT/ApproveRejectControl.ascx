
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApproveRejectControl.ascx.cs" Inherits="uGovernIT.Web.ApproveRejectControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    html .ui-btn {
        background-color: #373737;
        border-color: #1f1f1f;
        color: #fff;
        text-shadow: 0 1px 0 #111;
    }

    .ui-btn {
        font-size: 1em;
        line-height: 1.3;
        font-family: sans-serif /*{global-font-family}*/;
    }

    .ui-btn {
        font-size: 16px;
        margin: .5em 0;
        padding: .7em 1em;
        display: block;
        position: relative;
        text-align: center;
        text-overflow: ellipsis;
        overflow: hidden;
        white-space: nowrap;
        cursor: pointer;
        -webkit-user-select: none;
        -moz-user-select: none;
        -ms-user-select: none;
        user-select: none;
    }

    .ui-btn {
        font-size: 12.5px;
        display: inline-block;
        vertical-align: middle;
    }
</style>


<header>
    <h2 style="padding-left:5px;">
        <asp:Label ID="lblHeader" Text="Approval Completed" runat="server" />
    </h2>
</header>
<div data-role="content" style="padding: 1em 5px;">
    <span runat="server" id="spanSuccessMessage"></span>
    <br />
    <a runat="server" id="aRefOK" href="#" class="ui-btn" data-ajax="false">Open Request</a>
</div>
<dx:ASPxPopupControl ClientInstanceName="commentsRejectPopup" Modal="true"
        PopupElementID="btnRejectApp" ID="commentsRejectPopup"
        ShowFooter="false" ShowHeader="true" CssClass="departmentPopup" HeaderText="Reject Reason"
        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <div style="float: left; height: 200px; width: 400px;" class="first_tier_nav">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <h3 class="ms-standardheader" style="text-align:left !important;">Reason<b style="color: red;">*</b>
                            </h3>
                                </td>
                                <%--<asp:Label ID="lblComment" runat="server" Text="Reason *" Font-Size="Smaller" ></asp:Label></td>--%>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox runat="server" ID="txtTaskComment" Width="400px" Columns="52" Rows="9" TextMode="MultiLine" Text=""></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTaskComment" runat="server" ControlToValidate="txtTaskComment" Display="Dynamic" ErrorMessage="Field required!" ForeColor="Red" ValidationGroup="RejectReason"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="buttoncell padding-top5" align="right">

                                <dx:ASPxButton ToolTip="Reject Task" AutoPostBack="true" ImagePosition="Right" OnClick="aspxRejectbutton_Click"  ValidationGroup="RejectReason"
                                    ID="aspxRejectbutton" runat="server" Text="Reject">
                                    
                                    <Image Url="/images/ButtonImages/reject.png"></Image>
                                </dx:ASPxButton>


                                <dx:ASPxButton ToolTip="Cancel" AutoPostBack="false" ImagePosition="Right" 
                                    ID="ASPxButton4" runat="server" Text="Cancel">
                                    
                                    <Image Url="/images/ButtonImages/cancel.png"></Image>
                                    <ClientSideEvents Click="function(s,e){commentsRejectPopup.Hide();}" />
                                </dx:ASPxButton>

                            </td>
                        </tr>
                    </table>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
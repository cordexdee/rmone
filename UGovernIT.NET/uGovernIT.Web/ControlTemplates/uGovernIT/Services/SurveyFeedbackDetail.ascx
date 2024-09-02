
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyFeedbackDetail.ascx.cs" Inherits="uGovernIT.Web.SurveyFeedbackDetail" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<link href="<%= ResolveUrl(@"~/Content/uGITWizard.css") + "?v=" + UGITUtility.AssemblyVersion %>" rel="stylesheet" />

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .requestdetail {
        float: left;
        width: 100%;
    }

        .requestdetail .label1 {
            font-weight: bold;
        }

        .requestdetail a:hover {
            text-decoration: underline;
        }
</style>


<div class="requestdetail">
    <fieldset style="width: 98%;">
        <legend>Request Detail</legend>
        <div class="question" style="margin: 0px; float: left; width: 98%;">
            <div style="float: left; width: 50%;">
                <span class="label1">Ticket ID: </span>
                <span>
                    <asp:HyperLink ID="lbTicketID" runat="server" Text="Ticket ID"></asp:HyperLink>
                </span>
            </div>
            <div style="float: left; width: 50%;">
                <span class="label1">Submitted By: </span>
                <span>
                    <asp:HyperLink ID="lbSubmitBy" runat="server" Text="User Name"></asp:HyperLink>
                </span>
            </div>
        </div>

        <div class="question" style="margin: 0px; float: left; width: 98%;">
            <div style="float:left;">
                <span class="label1">Title: </span>
                <span>
                    <asp:Label ID="lbTitle" runat="server" Text="sdfasd fasdfasdf"></asp:Label>
                </span>
            </div>
            <div id="ticketdiv" runat="server" style="width: 100%; float: right;" visible="false">
                <div style="float: left; width: 20%;">
                    <span class="label1">PRP: </span>
                    <span>
                        <asp:Label ID="lblPRP" runat="server" />
                    </span>
                </div>
                <div style="float: left; width: 20%;">
                    <span class="label1">Owner: </span>
                    <span>
                        <asp:Label ID="lblOwner" runat="server" />
                    </span>
                </div>
                <div style="float: left; width: 20%;">
                    <span class="label1">Category: </span>
                    <span>
                        <asp:Label ID="lblCategory" runat="server" />
                    </span>
                </div>
            </div>
        </div>

    </fieldset>
</div>

<fieldset style="float: left; width: 98%;">
    <legend>Feedback Detail</legend>
    <asp:Repeater runat="server" ID="rFeedbackD">
        <ItemTemplate>
            <div class="question" style="margin: 0px;">
                <b class="questionlb"></b>
                <span style="font-weight: bold;">
                    <%#  Eval("Question")  %>
                </span>
                <div class="select_product">
                    <span><%#  Eval("Answer")  %></span>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</fieldset>

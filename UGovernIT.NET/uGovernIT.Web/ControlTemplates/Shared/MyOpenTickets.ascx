
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="MyOpenTickets.ascx.cs" Inherits="uGovernIT.Web.MyOpenTickets" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%--<%@ Import Namespace="uGovernIT.Helpers" %
<%@ Import Namespace="uGovernIT.Manager.Utilities" %>>--%>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .uborderdiv {
        float: left;
        cursor: pointer;
    }

    .ucontentdiv {
        float: left;
        height: 18px;
        float: left;
        padding: 1px 6px 0;
        margin: 2px 4px;
    }

    .uborderdicselected {
        position: relative;
        top: -1px;
        left: -1px;
    }

    .upanelbottomshadow {
        background: url("/Content/images/panel_bottomshadow.png") no-repeat;
        float: left;
        width: inherit;
    }

    .myticketpaneldiv {
        float: left;
    }

    .mytickettext {
        float: left;
        font-weight: bold;
        left: 4px;
        position: relative;
        top: 7px;
        background-position: 0 -5px;
        height: 16px;
        padding: 0px 2px;
        background: #fff;
    }

    .myticketcontent { /*border:1px solid #eeecea;*/
        float: left;
        padding: 9px 0;
    }
    /*.clickedTab {
    background-color:#0072c6;
    
    }
    .clickedTab a{
        color:#fff !important;
    }*/
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function SetSelectedMyTicketLink(val) {
        var setlink = document.getElementById("<%=selectedMyTicketLink.ClientID %>");
      
        set_cookie("mytab", "mytickets");
        if (setlink) {
            if (setlink.value == val) {
                setlink.value = "my";
            }
            else {
                setlink.value = val;
            }
            
            return true;
        }
    }
</script>
 <asp:HiddenField ID="selectedMyTicketLink" runat="server" />
<div class="col-md-12 col-sm-12 col-xs-12 noPadding">
    <div class="row homeDb-requestLinkWrap" id="cMyTicketLinkspanel" runat="server" visible="false">
        <div class="ugit-contentcontainer">
            <asp:Repeater ID="myTicketRepeater" runat="server">
                <ItemTemplate>
                    <div class="uborderdiv">
                        <div class="ucontentdiv ugitlinkbg" id="myTicketsLinks" runat="server">
                            <span>
                                <asp:LinkButton ID="myTicketLink" runat="server"></asp:LinkButton>
                            </span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="row" id="cPreviewTicketPanel" runat="server" style="background:#fff;">
        <div style="background-color:#fff;">
            <div class="ticketpreviewdiv" id="myTicketPreviewPanel" runat="server">
            </div>
        </div>
    </div>
</div>

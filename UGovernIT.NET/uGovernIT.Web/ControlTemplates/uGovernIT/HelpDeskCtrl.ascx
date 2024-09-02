<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpDeskCtrl.ascx.cs" Inherits="uGovernIT.Web.HelpDeskCtrl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    //$(document).ready(function () {
    //    $('.btn').removeClass('slectedBtn');
    //    $(".btn").click(function () {
    //        dvCallbackPanel.PerformCallback($(this).attr('id'))
    //        $('.btn').removeClass('slectedBtn');
    //        $(this).toggleClass("slectedBtn");
    //    });
    //});

<%--    $(document).ready(function () {
         var tsr= $.cookie("TSRClicked");
         var acr= $.cookie("ACRClicked");
         var drq= $.cookie("DRQClicked");
         var svc= $.cookie("SVCClicked");
        if (tsr == "True") {
            $('#<%=TSR.ClientID%>').toggleClass("slectedBtn");
        } else if (acr == "True") {
            $('#<%=ACR.ClientID%>').toggleClass("slectedBtn");
        } else if (drq == "True") {
            $('#<%=DRQ.ClientID%>').toggleClass("slectedBtn");
        } else if (svc == "True") {
            $('#<%=SVC.ClientID%>').toggleClass("slectedBtn");
        } else {
            $('#<%=TSR.ClientID%>').toggleClass("slectedBtn");
        }
            delete_cookie("TSRClicked");
            delete_cookie("ACRClicked");
            delete_cookie("DRQClicked");
            delete_cookie("SVCClicked");

        });--%>
    
    function OnSelectionChanged(s, e) {
    }
    //function OpneChangeTicketTypeDialog() { }
    //function OnEndTicketTypePopupCallback() { }
</script>

<div class="col-md-12 col-sm-12 col-xs-12">
  <div class="row">
<%--    <button type="button" class="btn btn-md ITSMButton-secondary" id="TSR">Incidents</button>
    <button type="button" class="btn btn-md ITSMButton-secondary" id="SVC">Services</button>
    <button type="button" class="btn btn-md ITSMButton-secondary" id="DRQ">Change Management</button>
    <button type="button" class="btn btn-md ITSMButton-secondary" id="ACR">Applications</button>--%>
<%--    <asp:Button ID="TSR"  class="btn btn-md ITSMButton-secondary" runat="server" onclick="TSR_Click" Text="Incidents" /> 
    <asp:Button ID="SVC" class="btn btn-md ITSMButton-secondary" runat="server" onclick="SVC_Click" Text="Services" /> 
    <asp:Button ID="DRQ" class="btn btn-md ITSMButton-secondary"  runat="server" onclick="DRQ_Click" Text="Change Management" /> 
    <asp:Button ID="ACR"  class="btn btn-md ITSMButton-secondary" runat="server" onclick="ACR_Click" Text="Applications" /> --%>
  </div>
</div>
    <div>
     <dx:PanelContent ID="dvTicket" runat="server"></dx:PanelContent>
    </div>
<%--<div class="row">
<div class="col-sm-12">   
    <dx:ASPxCallbackPanel ID="dvTicketCallbackPanel" ClientInstanceName="dvCallbackPanel" runat="server" OnCallback="TicketCallbackPanel_Callback">
        <PanelCollection>
            <dx:PanelContent ID="dvTicket" runat="server"></dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>
</div>    
</div>--%>



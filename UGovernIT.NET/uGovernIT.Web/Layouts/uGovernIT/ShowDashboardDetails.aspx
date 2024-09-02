
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowDashboardDetails.aspx.cs" MasterPageFile="~/master/Light.master" Inherits="uGovernIT.Web.ShowDashboardDetails"   %>
<%--DynamicMasterPageFile="~masterurl/default.master"--%>
<asp:Content ID="PageHead" ContentPlaceHolderID="HeaderContent" runat="server">
   <meta name="viewport" content="width=device-width">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
<script type="text/javascript">

    $(function () {
      try {
        //Remove top title block for printout
        <%if(printEnable){ %>
            $("div:hidden").remove();
             $("#s4-titlerow").remove();
        <%} %>

        <%if(startDownload){ %>
           startDownload();
        <%} %>
        }catch(ex){
        }
    });
 
    function startDownload()
    {
       window.location.href = window.location.href+"&startDownload=true";
    }
   </script>
     <asp:Panel ID="managementControls" runat="server" CssClass="managementcontrol-main"></asp:Panel>
</asp:Content>





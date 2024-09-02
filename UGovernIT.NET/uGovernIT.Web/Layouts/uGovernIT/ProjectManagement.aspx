<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectManagement.aspx.cs" Inherits="uGovernIT.Web.ProjectManagement" MasterPageFile="~/master/Root.Master" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
   <meta name="viewport" content="width=device-width" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function clickUpdateSize() {
        try {
            window.parent.adjustIFrameWithHeight("<%=frameId %>", $(".managementcontrol-main").height());
        }
        catch (ex) {
        }
    }
 
    var stopUpdateFrameSize = false;
    function addHeightToCalculateFrameHeight(control, height) {
        try {
            window.parent.adjustIFrameWithHeight("<%=frameId %>", $(".managementcontrol-main").height());
            stopUpdateFrameSize = true;
        }
        catch (ex) {
        }
    }


    $(function () {
        try {
            $(".managementcontrol-main").bind("click", function () {
                stopUpdateFrameSize = false;
                clickUpdateSize();
            });
            $(".managementcontrol-main").bind("hover", function (e) {
                if (!stopUpdateFrameSize) {
                    clickUpdateSize();
                }
            });

            //Remove top title block for printout
            <%if (printEnable)
              { %>
            $("div:hidden").remove();
            $("#s4-titlerow").remove();
            $(".readonlyblock table").attr("width", "1124px");
            $(".readonlyblock table").removeClass("ro-table");
            $(".ro-table").attr("width", "1130px");
            $(".ro-table").removeClass("ro-table");
            <%} %>


        } catch (ex) {
        }

        try {
            clickUpdateSize();
        }
        catch (ex) {
        }
    });


   </script>
<asp:Panel ID="managementControls" runat="server" CssClass="managementcontrol-main">
</asp:Panel>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server"  Visible="false">
</asp:Content>

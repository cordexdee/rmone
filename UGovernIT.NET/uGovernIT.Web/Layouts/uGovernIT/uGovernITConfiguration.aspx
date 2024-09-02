

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uGovernITConfiguration.aspx.cs"
    Inherits="uGovernIT.Web.uGovernITConfiguration"  MasterPageFile="~/master/Light.Master" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="PageHead" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
<%--<script type="text/javascript">
        function clickUpdateSize() {
            window.parent.autoFrameWithHeight($(".managementcontrol-main").height());
        }

        var stopUpdateFrameSize = false;
        function addHeightToCalculateFrameHeight(control, height) {
            var diff = $(".managementcontrol-main").height() - $(control).position().top;
            if (diff < 100 && height > diff) {
                var allocatedHeight = height - diff;
                window.parent.autoFrameWithHeight($(".managementcontrol-main").height() + allocatedHeight);
                stopUpdateFrameSize = true;
            }
        }

        $(function () {
            try {
                clickUpdateSize();
                $(".managementcontrol-main").bind("click", function () {
                    stopUpdateFrameSize = false;
                    clickUpdateSize();
                });
                //        $(".managementcontrol-main").bind("mouseover", function (e) {
                //            if (!stopUpdateFrameSize) {
                //                clickUpdateSize();
                //            }
                //        });
            }
            catch (ex)
            {

            }
        });

    </script>--%>
      <asp:Panel ID="contentPanel" runat="server" CssClass="managementcontrol-main marginZero_forMobile"></asp:Panel>
</asp:Content>



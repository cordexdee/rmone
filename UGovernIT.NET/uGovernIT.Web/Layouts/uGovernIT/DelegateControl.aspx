<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DelegateControl.aspx.cs" Inherits="uGovernIT.Web.DelegateControl" MasterPageFile="~/master/Light.master" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="HeaderContent" runat="server">
   <meta name="viewport" content="width=device-width">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server" >
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        var frameId = "<%=frameId %>";
        function clickUpdateSize() {
            try {
                var ctrlHeight = $(".managementcontrol-main").height();
                if (ctrlHeight == 0 && typeof ($(".managementcontrol-main")[0]) !== "undefined")
                    ctrlHeight = $(".managementcontrol-main")[0].scrollHeight;

                window.parent.adjustIFrameWithHeight("<%=frameId %>", ctrlHeight);
            }
            catch (ex) {
            }
        }

       var stopUpdateFrameSize = false;
       function addHeightToCalculateFrameHeight(control, height) {
           try {
                window.parent.adjustIFrameWithHeight(frameId, $(".managementcontrol-main").height());
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
                $(".managementcontrol-main").bind("mouseover", function (e) {
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

                var mandatoryVal = $('#<%=hdnMandatoryCheck.ClientID%>').val();
                setIframeControlChecks(frameId, mandatoryVal);


            } catch (ex) {
            }

            try {
                clickUpdateSize();
            }
            catch (ex) {
            }
        });

        //function clickUpdateSize() {
        //   autoFrameWithHeight($(".managementcontrol-main").height());
        //}

        //var stopUpdateFrameSize = false;
        //function addHeightToCalculateFrameHeight(control, height) {
        //    var diff = $(".managementcontrol-main").height() - $(control).position().top;
        //    if (diff < 100 && height > diff) {
        //        var allocatedHeight = height - diff;
        //        window.parent.autoFrameWithHeight($(".managementcontrol-main").height() + allocatedHeight);
        //        stopUpdateFrameSize = true;
        //    }
        //}

        //$(function () {
        //    
        //    try {
        //        clickUpdateSize();
        //        $(".managementcontrol-main").bind("click", function () {
        //            stopUpdateFrameSize = false;
        //            clickUpdateSize();
        //        });
        //                $(".managementcontrol-main").bind("mouseover", function (e) {
        //                    if (!stopUpdateFrameSize) {
        //                        clickUpdateSize();
        //                    }
        //                });
        //    } catch (ex) {
        //    }
        //});


        function autoFrameWithHeight(height) {
            $("#configurationframe").removeAttr("width");
            if ($(".rightside-container").width() > 100) {
                $("#configurationframe").attr("width", $(".rightside-container").width() + "px");
            }
            else {

                $("#configurationframe").attr("width", "800px");
            }

            if (height && height > 750) {
                $("#configurationframe").attr("height", eval(height + 10) + "px");
            }
            else {
                $("#configurationframe").attr("height", "650px");
            }
        }


        function setIframeControlChecks(controlID, val) {

            var hiddenCtr = $(".framepanel input:hidden[id $= '" + controlID + "_mandatory']");
            hiddenCtr.val(val);

        }
    </script>
    <asp:HiddenField ID="hdnMandatoryCheck" runat="server" />
    <asp:Panel ID="managementControls" runat="server" CssClass="managementcontrol-main managementcontrolMainCus px-3">
    </asp:Panel>
     <asp:Panel ID="contentPanel" runat="server" CssClass="managementcontrol-main">
    </asp:Panel>
       <div class="homeDashboard_leftcontent_wrap" style="Padding-left:12px">
                    <div >
                        <div>
                            <asp:Panel ID="MainPanel" runat="server" Width="100%" >
                            </asp:Panel>
                        </div>
                    </div>
                </div>

</asp:Content>




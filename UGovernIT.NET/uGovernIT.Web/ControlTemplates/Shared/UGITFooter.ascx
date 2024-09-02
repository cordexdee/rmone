<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UGITFooter.ascx.cs" Inherits="uGovernIT.Web.UGITFooter" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ugitfootercopyrightAnchor a:visited {
        color: white !important;
    }

    .ugit-setfooterposition {
        /*position: fixed;
        bottom: 0;*/
        height: 29px;
        width: 100%;
        /*z-index:999;*/
    }
    .dxnb-itemHover {
        color: white !important;
        background-color: #253746 !important;
        /*padding: 5px;*/
        /*z-index: 999;*/
        /*color: blue;*/
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    
    .dxnbLite_UGITNavyBlueDevEx .dxnb-itemHover a{
        background-color: #253746 !important;
    }
</style>

<asp:Panel ID="dvfooter" runat="server" CssClass="ugit-dialogHidden ugit-setfooterposition " Visible="false">
   <div id="messagefooter" class="dxnb-itemHover" style="padding:5px;z-index:999;color:white" >
        <%--style="float: left; padding-left: 5px; padding-right: 5px; padding-top: 11px;"--%>
        <span>
            <dx:ASPxImage ID="imgfooterlogo" runat="server" style="max-width:80px;height:19px;"></dx:ASPxImage>
            <%--<img style="max-width: 80px;" src="/Content/Images/ugovernit_logo.png" alt="uGovernIT" border="0" />--%>
        </span>
        
        <div runat="server" id="messagefooterdata" class="content-wrapper" style="display:none;">
            <div class="float-left">
                <p>
                    <img runat="server" id="imgMessage" style="width: 40px;"
                        src="/Content/Images/overallstatus_good.png" class="float-left" />
                    <span runat="server" id="spanbody" class="float-left" style="margin-top: 10px; padding-left:5px;">&nbsp;&nbsp;All systems operating normally</span>
                </p>
            </div>

            <p class="float-left" onclick="footerMessageClick()">
                <img style="width: 32px; height: 32px;" src="/Content/Images/zoomin_icon.png" />
            </p>
        </div>
        <span>&nbsp;<asp:Literal ID="lbCopywrite" runat="server"></asp:Literal>  <%--class="ugitfootercopyright"--%>
            <span id="spanMessageBoard">&nbsp;</span>
        </span>
    </div>
</asp:Panel>


<script data-v="<%=UGITUtility.AssemblyVersion %>">

    function footerMessageClick() {
        try {
            HideShowMessageBoard(this);
            var height = $(".ms-core-tableNoSpace.ms-webpartPage-root").height() - $(".dashboard-panel").last().position().top;
            $(".dashboard-panel").last().find("#linkID").css("height", height + "px");
        }
        catch (ex) {
        }
    }

</script>

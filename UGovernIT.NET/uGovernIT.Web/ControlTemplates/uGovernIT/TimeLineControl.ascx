<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeLineControl.ascx.cs" Inherits="uGovernIT.Web.TimeLineControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .verticallabel {
        display: inline-block;
        -webkit-transform: rotate(-30deg);
        -moz-transform: rotate(-30deg);
        filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=3);
    }
    .verticallabelrow {
        height:102px;
    }
    .datecell,.datelabel {
       margin-left: 4px;
    margin-top: -1px;
    position: absolute;
    text-align: left;
    vertical-align: top;
    }
    .timelinetable {
        width:80%;
        margin-left:100px;
    }
    /*.timelinetable td, .timelinetable tr {
       padding:0px

    }*/
    .timelinegraps img {
        z-index:100;
    }
    .timelinegraps img, .datelabel,verticallabel {
        float:left;
    }
    .timelinegraps {
        background-repeat: no-repeat;
        width: 37px;
        height: 62px;
    }
    .startpro {
        background-image: url('/Content/images/uGovernIT/prostarticon.png');
    }

    .endpro {
     background-image:url('/Content/images/uGovernIT/proendicon.png');
    }

    .startconst {
     background-image:url('/Content/images/uGovernIT/conststarticon.png');
    }

    .endconst {
     background-image:url('/Content/images/uGovernIT/constendicon.png');
    }

    .projectendlabel {
        float: left;
        margin-left: -39px;
        margin-top: -36px;
    }
    .projectstartlabel {
        float: left;
        margin-left: -39px;
        margin-top: -36px;
    }
    .conststartlabel {
        float: left;
        margin-left: -67px;
        margin-top: -27px;
    }
    .constendlabel {
        float: left;
        margin-left: -60px;
        margin-top: -29px;
    }
    .midpipe {
        float: left;
        height: 38px;
        margin-left: -2px;
       width: 105%;
        
    }
    .completegraph {
        background-image: url("/Content/images/uGovernIT/stepline_active.gif");
        background-repeat: repeat-x;
    }
     .inprocessgraph {
        background-image: url("/Content/images/uGovernIT/stepline.gif");
        background-repeat: repeat-x;
    }

      .newdatelabel {
       margin-left: 4px;
    margin-top: 50px;
    position: absolute;
    text-align: left;
    vertical-align: top;
    }
    
</style>


<table class="timelinetable" id="tbTimeline" runat="server" >
    <tr>
        <td class="timelinegraps" id="tdconprojectstartgraph" runat="server">
            <img  src="/Content/images/uGovernIT/prostarticon.png" />
            <asp:Label ID="lblProjectStartDt" CssClass="datelabel" Text="11-25-2015" runat="server" />
        </td>
        <td class="graphtd" runat="server" id="tdconsstartmidpipe" >
           <div id="graphdiv1" runat="server" class="midpipe inprocessgraph"></div>
        </td>
        <td class="timelinegraps" runat="server" id="tdconsstartgraph">
            <img src="/Content/images/uGovernIT/conststarticon.png" />
            <asp:Label ID="lblConstStartDt" CssClass="datelabel" Text="11-30-2015" runat="server" />
            
        </td>
        <td class="graphtd" runat="server" id="tdconsendmidpipe" >
            <div  id="graphdiv2"  runat="server" class="midpipe inprocessgraph">&nbsp;&nbsp;</div>
        </td>
        <td class="timelinegraps" runat="server" id="tdconsendgraph">
            <img src="/Content/images/uGovernIT/constendicon.png" />
            <asp:Label ID="lblConstEndDt" CssClass="datelabel" Text="12-30-2015" runat="server" />
        </td>
        <td class="graphtd"  runat="server" id="tdproclosemidpipe" >
            <div  id="graphdiv3"  runat="server" class="midpipe inprocessgraph"></div>
        </td>
        <td class="timelinegraps" runat="server" id="tdproclosegraph">
            <img   src="/Content/images/uGovernIT/proendicon.png" />
            <asp:Label ID="lblProjectEndDt" CssClass="datelabel" Text="1-10-2016" runat="server" />
        </td>
    </tr>
    <tr class="verticallabelrow">
        <td colspan="2" id="tdconprojectstartlabel" runat="server">
            <span class="verticallabel projectstartlabel">Precon Start</span>
        </td>
        <td colspan="2" runat="server" id="tdconststartlabel">
            <span class="verticallabel conststartlabel">Construction Start</span>
        </td>
        <td colspan="2" runat="server" id="tdconstendlabel" >
            <span class="verticallabel constendlabel">Construction End</span>
        </td>
        <td runat="server" id="tdprocloselabel">
            <span class="verticallabel projectendlabel">Project Close</span>
        </td>
    </tr>
</table>
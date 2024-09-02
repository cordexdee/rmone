<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uGovernITSideBarUserControl.ascx.cs" Inherits="uGovernIT.Web.uGovernITSideBarUserControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
  /* body #s4-titlerow {
        display: none;
    }

    body #s4-leftpanel {
        display: none;
    }

.bordercollapse{border-collapse:collapse;}
.sidebarcontentmain{ border-collapse:collapse;}
.sidebarmaintd{background:url("/_layouts/15/images/ugovernit/sidebar_shadow.png");}
.sidebarmaindiv{background:#FFF;float:left;padding:5px 4px 5px 5px;} 
.sidebarbottomdiv{height:3px;margin:0 3px;}
  .ms-cui-tts, .ms-cui-tts-scale-1, .ms-cui-tts-scale-2{display:none !important;}
  .ms-cui-TabRowLeft, .ms-cui-jewel-container{display:none !important;}
  .ms-WPBorder, .ms-WPBorderBorderOnly{border:0px !important;}
      
    body #MSO_ContentTable {
        margin-left: 0px !important;
    }
    .ms-WPBorder, .ms-WPBorderBorderOnly {
        border: 0px !important;
    }
      */ 

    .bordercollapse {
        border-collapse: collapse;
    }

    .sidebarcontentmain {
        border-collapse: collapse;
    }

    .sidebarmaintd {
        background: url("/_layouts/15/images/ugovernit/sidebar_shadow.png");
    }

    .sidebarmaindiv {
        float: left;
        padding: 0px 4px 5px 5px;
    }
    /*margin:0 2px 0 1px;border:1px solid #cacaca;*/
    .sidebarbottomdiv {
        height: 3px;
        margin: 0 3px;
    }
    
</style>

<asp:Panel ID="previewPanel" runat="server">
    <table cellpadding="0" cellspacing="0" class="bordercollapse">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" class="bordercollapse">
                    <tr>
                        <td>
                            <div class="sidebarmaindiv" runat="server" id="sidebarmaindiv">
                                <asp:Table ID="prviewTable" Width="140px" runat="server" CellPadding="0" CellSpacing="0" CssClass="bordercollapse">
                                </asp:Table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <div class="sidebarbottomdiv"></div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>



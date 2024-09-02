<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportList.ascx.cs" Inherits="uGovernIT.Web.ExportList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
       
    .downloadlink, .downloadlink:visited, .downloadlink:hover
    {
        color: Blue !important;
    }
    .style2
    {
        width: 91px;
    }
</style>
<script language="javascript" type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        <%if(startDownload){ %>
            startDownload();
            readyDownload();
        <%} else if(pageName == "ITGBudgetManagement" ||pageName =="PMMBudget"){ %>
        startDownload();
        readyDownload();
        <%} %>

    });
 
    function startDownload()
    {
  
    var list =  document.getElementById('<%= operationsDropDown.ClientID %>').value;
    window.location.href = window.location.href+"&startDownload=true&listName="+list;
    }

    function readyDownload()
    {

   $("#dropDownDiv").css("display", 'none');
   $("#downloadMessageDiv").css("display", 'block');
     return true;
    }
   
</script>
<asp:HiddenField ID="selectedList" Value="" runat="server" />
<div id="dropDownDiv" 
    
    style="display: block; position: relative; top: 0px; left: 0px; height: 155px; width: 632px;">
    <table style="width: 632px; height: 34px">
        <tr>
            <td style="font-weight: bold" class="style2">
             Choose list:
            </td>
            <td>
                <asp:DropDownList ID="operationsDropDown" runat="server" onchange="ShowOperationsDialog(true)">
                </asp:DropDownList>
            </td>
        </tr>
         </table >
       
        <table width="400" style="height: 87px">
        <tr>
        <td align="right" valign="bottom">
                <asp:Button ID="btDownload" Text="Export List" runat="server"  OnClientClick=" return startDownloadList()"
                    OnClick="btDownload_Click" />
          </td>
        </tr>
        </table>
            </div>

<div id="downloadMessageDiv"  style="display: none;position: relative; background; top: 0px; left: 0px; height: 124px;">
         
    <table width="100%" style="border-collapse: collapse; padding: 0px;" cellspacing="0"
        cellpadding="0">
        <tr>
            <td>
                If your download does not start in 30 seconds, Click <a class='downloadlink' href='javascript:void(0)'
                    onclick='startDownload()'>Start Download</a>
            </td>
        </tr>
          </table>
      
        <table width="400" style="height: 102px">
        <tr>
        <td align="right" valign="bottom">
         <asp:Button ID="btClose" Text="Close"   runat="server" onclick="btClose_Click" />
        </td>
        </tr>
        </table>
   
        
</div>

 <dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" Visible="false"></dx:ASPxSpreadsheet>
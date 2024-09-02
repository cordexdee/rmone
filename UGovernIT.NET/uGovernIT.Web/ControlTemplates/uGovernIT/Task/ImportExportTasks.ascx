<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportExportTasks.ascx.cs"
    Inherits="uGovernIT.Web.ImportExportTasks" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function getConfirm() {
        if (confirm('All existing tasks will be replaced from the imported file.\nThis will also delete ALL resource allocations for this project.\n\nAre you sure you want to proceed?')) {            document.getElementById('ImportBox').style.display = 'none';
            AddNotification("Processing ..");
            
            loadingScreen();
            return true;
        }
        else {
            return false;
        }
    }

    function closepopup() {
        document.getElementById('ImportBox').style.display = 'none';
    }

    function ShowImportBox(obj) {
        var importBox = $(document.getElementById('ImportBox'));
        var importImg = $(obj);
        var boxTop = 0;
        var boxLeft = importImg.position().left - 390;
        importBox.css({ "position": "absolute", "top": boxTop + "px", "left": boxLeft + "px" })
        importBox.show("slow");
        return false;
    }

    function setFormSubmitToFalse() {
        setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);
        return true;
    }
</script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .maindiv
    {
        background: #ced8d9;
        border: 1px solid black;
       
        height: 80px;
    }
    .headerRow
    {
        height: 20px;
        background: #191919;
        font-family: Verdana Arial Times New Roman;
        color: #fff;
    }
     .trImportDateOnly input {
        position:relative;
        top:3px;
    }
    .checkbox-span{float: left;position: relative; top: 3px;}
</style>


<div class="maindiv" id="ImportBox" style="display:none">
   <asp:HiddenField ID="hdnFilePath" runat="server" />
  
    <table width="100%">
        <tr class="headerRow">
            <td align="left">
                Import from MS Project
            </td>
            <td align="right">
                <img src="/Content/images/Crossicon.jpg" alt="Close"
                    id="close" onclick="closepopup()" />
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblSelectFile" runat="server" Text="Select File"></asp:Label>
                        </td>
                        <td>
                            <div style="float:left;overflow:hidden;">
                              <asp:FileUpload ID="fileUpload" runat="server" />
                            </div>
                        </td>
                        <td>
                            <asp:Button ID="btnImport" runat="server" Text="Import" OnClick="ImportTasks" OnClientClick="return getConfirm()" />
                        </td>
                    </tr>
                    <tr id="trImportDateOnly" class="trImportDateOnly" runat="server">
                        <td colspan="3">
                            <asp:CheckBox ID="chkImportDates" runat="server" CssClass="checkbox-span" Text="Don't Import Predecessors" TextAlign="Right" />
                            <div style="float:right;">
                                <asp:CheckBox ID="chkDontImportAssignee" CssClass="checkbox-span" runat="server" Text="Don't Import Assignees" TextAlign="Right" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</div>
<div id="importExportButtonPnl" runat="server">
    <span>
            <asp:ImageButton id="imgBtnExport" ImageUrl="/Content/images/exportTasks.png" OnClientClick="javascript:setFormSubmitToFalse()" style="float: right;" runat="server"  alt="Export Tasks" title="Export Tasks" OnClick="ExportTasks" />
    </span>
    <span>
            <asp:ImageButton  id="imgBtnImport" ImageUrl="/Content/images/importTasks.png" style="float: right;" runat="server" alt="Import Tasks" title="Import Tasks" OnClientClick="return ShowImportBox(this)"  />
    </span>
</div>

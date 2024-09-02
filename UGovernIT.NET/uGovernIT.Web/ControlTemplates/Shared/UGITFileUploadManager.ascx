<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UGITFileUploadManager.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Shared.UGITFileUploadManager" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxpc-contentWrapper {
        height:100%!important;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
      
    function UplodaControlWithID(id) {
         var popupCtrl = ASPxClientControl.GetControlCollection().GetByName(id + "_fileUploadManager");
            popupCtrl.PerformCallback("aUploadDocuments");
            popupCtrl.Show();
            fileUploadManager.SetHeaderText("Pick from Library");
            return false;
    }
    function showUploadControl(id) {
        setTimeout(UplodaControlWithID(id), 100);
    }
    function showWiki(id) {
        var popupCtrl = ASPxClientControl.GetControlCollection().GetByName(id + "_fileUploadManager");
        popupCtrl.PerformCallback("aShowWiki");
        popupCtrl.Show();
        fileUploadManager.SetHeaderText("Wiki Detail" + "(" + 'Click On a Row to Select it' + ")");
        return false;

    }
    function fileManager_SelectionChanged(s, e) {
        var files = "";
        var selectedFiles = s.GetSelectedItems();
        for (var i = 0; i < selectedFiles.length; i++) {
            files += selectedFiles[i].name + ",";           
        }
        if (files != null && files != undefined) {
           var fileList= files.substring(0, files.length - 1);
            var customCtrID = s.name.replace("_fileUploadManager_ASPxFileManager1", "");
            var textBoxCtrl = ASPxClientControl.GetControlCollection().GetByName(customCtrID + "_txtHelp");
            if (textBoxCtrl)
            {
                textBoxCtrl.SetText(fileList);
            }
        }
       // document.getElementById("filesCount").innerHTML = selectedFiles.length;
    }
    function FindImageName(s, e) {
        var cidValue = s.globalName;
        var editor = s;
        var customCtrID = s.name.replace("_fileUploadManager_ASPxFileManager1", "");
        var popupCtrl = ASPxClientControl.GetControlCollection().GetByName(customCtrID + "_fileUploadManager");
        var textBoxCtrl = ASPxClientControl.GetControlCollection().GetByName(customCtrID + "_txtHelp");
        var fileName = s.GetSelectedFile().GetFullName();
        if (cidValue != null && cidValue != undefined && cidValue!="")
            eval(cidValue).Refresh();
        if (fileName != null) {
            editor.SetVisible(false);
            fileName = '/'+ fileName.replace(/\\/g, "/");
            textBoxCtrl.SetText(fileName);
            popupCtrl.Hide();
        }
    }

   
    function WikiCallBack(s, e, data) {
        var editor = s;
        var customCtrID = s.name.replace("_fileUploadManager_wikiListPicker_cbpaneltestst_grid", "");
        var popupCtrl = ASPxClientControl.GetControlCollection().GetByName(customCtrID + "_fileUploadManager");
        var textBoxCtrl = ASPxClientControl.GetControlCollection().GetByName(customCtrID + "_txtHelp");
        var Url = "<%=this.detailsURL%>";
        if (data != null) {
            editor.SetVisible(false);
            textBoxCtrl.SetText(Url + "ticketId=" + data);
            popupCtrl.Hide();
        }
    }

    function fileUploadManager_CallBack(s, e) {
        var scripts = $("#" + s.name + "_PWC-1").find("#wikiScript")
        scripts.each(function (i, s) {
            $.globalEval($(s).text());
        });
    }

     function ChangeView(s, e) {
        if (e.commandName == "ChangeView-Details") {            
             ASPxFileManager1.PerformCallback("Details"); 
        }
        else if (e.commandName == "ChangeView-Thumbnails") {  
              ASPxFileManager1.PerformCallback("Thumbnails");            
        }        
    }
</script>

<div class="row feildRow"> 
    <div class="fileUpload-txtBoxWrap">
        <dx:ASPxTextBox ID="txtHelp" ClientInstanceName="txtHelp" CssClass="fileUpload-txtBox" Width="100%" runat="server"  />
    </div>
     <div class="mt-1">
        <a id="aShowWiki" runat="server" class="uploadFile-links">Add Wiki</a> 
        <a id="apipe" runat="server" class="uploadFile-links" >|</a>
        <a id="aUploadDocuments"  runat="server" class="uploadFile-links"><%= AnchorLabel %></a>
     </div>
 </div>

    


<dx:ASPxPopupControl ID="fileUploadManager" runat="server" ClientInstanceName="fileUploadManager" AllowResize="true" MinHeight="400" Width="100%" Modal="True" 
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Middle" SettingsAdaptivity-Mode="Always" CssClass="aspxPopup"
    AllowDragging="true" PopupAnimationType="Fade" EnableViewState="False" OnWindowCallback="cb_Callback" CloseAction="CloseButton" background="#000000;">    
    <ClientSideEvents EndCallback="function(s,e){fileUploadManager_CallBack(s,e);}" CloseUp="function(s,e){fileUploadManager.Hide();}" />
    <ClientSideEvents Closing="function(s) {s.SetContentHtml(null); }" />
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxHiddenField ID="hdnPopupContentInfo" runat="server"></dx:ASPxHiddenField>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>



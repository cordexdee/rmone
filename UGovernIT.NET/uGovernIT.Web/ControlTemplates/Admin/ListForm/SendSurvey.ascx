<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SendSurvey.ascx.cs" Inherits="uGovernIT.Web.SendSurvey" %>
<%@ Register Src="~/controltemplates/uGovernIT/BatchCreateWizard.ascx" TagPrefix="ugit" TagName="BatchCreateWizard" %>
<%@ Register Src="~/controltemplates/uGovernIT/AddTicketEmail.ascx" TagPrefix="uc1" TagName="AddTicketEmail" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .setalign {
        padding-top: 2px !important;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function HideEmailBody() {
        if ('<%=show%>' == "True") {
            $('#idhtmleditor').css("display", "none");
            $('#idbtnnext').css("display", "none");
            $('#idbtnprevious').css("display", "none");
            $('#idbtnfinish').css("display", "none");
            $('#idbatchcreatewizard').css("display", "none");
            $('#errordiv').css('display', 'block');
            $(".actionButtons .hidden :hidden").val("");
        }
        else {
            $('#idhtmleditor').css("display", "none");
            $('#idbtnnext').css("display", "none");
            $('#idbtnprevious').css("display", "block");
            $('#idbtnfinish').css("display", "block");
            $('#idbatchcreatewizard').css("display", "block");
            $(".actionButtons .hidden :hidden").val("true");
        }
        return false;
    }

    function ShowEmailBody() {

        $('#idhtmleditor').css("display", "block");
        $('#idbtnnext').css("display", "block");
        $('#idbtnprevious').css("display", "none");
        $('#idbtnfinish').css("display", "none");
        $('#idbatchcreatewizard').css("display", "none");
        $(".actionButtons .hidden :hidden").val("");
        return false;
    }

    function FinishSurvey() {
        btnBatchCreateFinish.DoClick();
        //$('.classBatchCreateFinish').get(0).click();
    }

</script>

<div>
    <div id="idhtmleditor" style="display: none;">
        <uc1:AddTicketEmail runat="server" ID="IdAddTicketEmail" />
    </div>
    <div id="idbatchcreatewizard" style="display: none; height:400px;">
        <ugit:BatchCreateWizard runat="server" ID="IdBatchCreateWizard" style="display: none;" ></ugit:BatchCreateWizard>
    </div>

    <div style="float: right; padding-top: 2px;" class="actionButtons addEditPopup-btnWrap">
        <div class="hidden">
            <asp:HiddenField ID="hdnSelectUsers" runat="server" />
        </div>
        <div id="idbtnnext" style="float: left; display: none;">
            <dx:ASPxButton ID="btnnext" runat="server" Text="Next >>" CssClass="primary-blueBtn" AutoPostBack="false">
                <ClientSideEvents Click="HideEmailBody" />
            </dx:ASPxButton>
        </div>
        <div id="idbtnprevious" style="float: left; display: none; padding-left: 5px;">
            <dx:ASPxButton ID="btnPrevious" runat="server" CssClass="primary-blueBtn" Text="<< Previous" AutoPostBack="false">
                <ClientSideEvents Click="ShowEmailBody" />
            </dx:ASPxButton>

        </div>
        <div id="idbtnfinish" style="float: left; display: none; padding-left: 5px;">
            <dx:ASPxButton ID="btnFinish" runat="server" CssClass="secondary-cancelBtn" Text="Finish" OnClick="btnFinish_Click"></dx:ASPxButton>
        </div>
    </div>
    <div id="errordiv" style="text-align: center; vertical-align: central; display: none;">
        <div>
            <span>You cannot send survey because it is mapped to module.</span>
        </div>
    </div>
</div>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    if ($(".actionButtons .hidden :hidden").val() == "true") {
        HideEmailBody();
    }
    else {
        ShowEmailBody();
    }

</script>

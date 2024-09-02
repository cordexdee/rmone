<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedTicketPicker.ascx.cs" Inherits="uGovernIT.Web.RelatedTicketPicker" %>
<%@ Register Src="~/CONTROLTEMPLATES/Admin/ListForm/ListPicker.ascx" TagPrefix="uGovernIT" TagName="ListPicker" %>
<%@ Register Src="~/CONTROLTEMPLATES/Admin/ListForm/WikiListPicker.ascx" TagPrefix="uGovernIT" TagName="WikiListPicker" %>
<%@ Register Src="~/CONTROLTEMPLATES/Admin/ListForm/HelpCardListPicker.ascx" TagPrefix="uGovernIT" TagName="helpCardListPicker" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .BtnSaveLink {
        /*display:none !important;*/
        /*commented by anurag*/
    }
</style>

<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <uGovernIT:ListPicker ID="lstPicker" runat="server" ShowModuleDetail="false"></uGovernIT:ListPicker>
            <uGovernIT:WikiListPicker ID="wikiListPicker" runat="server"></uGovernIT:WikiListPicker>
			<uGovernIT:HelpCardListPicker id="helpCardListPicker" runat="server"></uGovernIT:HelpCardListPicker>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="BtnSaveLink" runat="server" Text="Save" style="width:72px !important" CssClass="BtnSaveLink primary-blueBtn" 
                ValidationGroup="LinkView" OnClick="btnSave_Click" ImagePosition="Right">
            </dx:ASPxButton>
        </div>
    </div>
</div>

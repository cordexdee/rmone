

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceSectionEditor.ascx.cs" Inherits="uGovernIT.Web.ServiceSectionEditor" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
 .fleft{float:left;}
.ms-formbody
{
    background: none repeat scroll 0 0 #E8EDED;
    border-top: 1px solid #A5A5A5;
    padding: 3px 6px 4px;
    vertical-align: top;
}
.pctcomplete{ text-align:right;}
.estimatedhours{text-align:right;}
.actualhours{text-align:right;}
.full-width{width:98%;}
.ms-formlabel{width:160px;}

</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
 $(function () {
        $("input:text").bind("keypress", function (event) {
            return event.keyCode != 13;
        });
    });
    function deleteConfirmation() {
        if (confirm("This action will delete questions under section also, do you want to delete section?")) {
            return true;
        }
        return false;
    }
</script>
 <asp:HiddenField ID="hfSectionID" runat="server" />
 <asp:HiddenField ID="hfServiceID" runat="server" />
 <div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer noPadding">
    <div class="ms-formtable accomp-popup">
         <div class="row" id="trTitle" runat="server" >
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">Section<b style="color: Red;">*</b></h3>
             </div>
             <div class="ms-formbody accomp_inputField">
                 <asp:TextBox ID="txtTitle" CssClass="full-width" runat="server" ValidationGroup="sectionGroup"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="sectionGroup" ControlToValidate="txtTitle"
                     Display="Dynamic" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>
             </div>
         </div>
        <div class="row" id="trDescription" runat="server" >
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDesc" TextMode="MultiLine" runat="server" ValidationGroup="sectionGroup"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="trIconUrl" runat="server">
                <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">Icon Url</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:UGITFileUploadManager ID="UGITFileUploadManager1" width="100%" runat="server" AnchorLabel="Upload Icon" hideWiki="true" />
                </div>
        </div>
        <div class="row" id="tr1" runat="server"  visible="false">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Questions<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:CheckBoxList ID="cblQuestions" runat="server">
                
                </asp:CheckBoxList>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <dx:ASPxButton ID="btDelete" runat="server" CssClass="secondary-cancelBtn" OnClick="btDelete_Click" Text="Delete" Visible="false">
                     <ClientSideEvents Click="deleteConfirmation" />
                 </dx:ASPxButton>
                <dx:ASPxButton ID="btCancelSection" CssClass="secondary-cancelBtn" Text="Cancel" OnClick="btCancelSection_Click" runat="server"></dx:ASPxButton>
                <dx:ASPxButton ID="btSaveSection" ValidationGroup="sectionGroup" runat="server" CssClass="primary-blueBtn" Text="Save" OnClick="BtSaveSection_Click">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
 </div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditWikiNavigation.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.EditWikiNavigation" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .fleft {
        float: left;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .pctcomplete {
        text-align: right;
    }

    .estimatedhours {
        text-align: right;
    }

    .actualhours {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .ms-formlabel {
        text-align: right;
        width:190px;
        vertical-align:top;
    }

    .ms-formline {
    border-top: 1px solid #A5A5A5;
    padding-left: 8px;
    padding-right: 8px;
    }

    .existing-section-c {
        float: left;
    }

    .new-section-c {
        float: left;
    }

    .existing-section-a {
        float: left;
        padding: 0px 5px;
    }

        .existing-section-a img {
            cursor: pointer;
        }

    .new-section-a {
        float: left;
        padding-left: 5px;
    }

        .new-section-a img {
            cursor: pointer;
        }

    .ms-standardheader {
        text-align: right;
    }

    .auto-style1 {
        border-top: 1px solid rgb(165, 165, 165);
        text-align: right;
        height: 30px;
        padding-right: 8px;
    }


    .auto-style2 {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        height: 30px;
    }

    .ms-long {
    font-family: Verdana,sans-serif;
    font-size: 8pt;
    width: 386px;
}

    #tdbutton1 ul li, #tdbutton2 ul li {
        color: #FFFFFF;
        cursor: pointer;
        display: inline;
        float: left;
        list-style: none outside none;
        margin: 0 1px;
        overflow: hidden;
        padding: 0;
        text-align: center;
        width: auto;
    }


</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function DeleteCongigVariable() {
        if (confirm('Are you sure want to delete?')) {
            <%=Page.ClientScript.GetPostBackEventReference(lnkDelete1,string.Empty)%>
         }
    }
  function OpenConditionPicker() {
    var Url = '<%= ConditionUrl%>';
    if (hdnSkipOnCondition.Get("SkipCondition") != undefined) {
        Url += "&SkipCondition=" + escape(hdnSkipOnCondition.Get("SkipCondition"));
    }
        
    javascript: UgitOpenPopupDialog(Url, '', 'Condition', '900px', '300px', 0, '');
}
<%--    function pickSiteAsset(Url) {
        var siteAsset = unescape(Url);
        $('#<%=txtImageUrl.ClientID%>').val(siteAsset);
    }
--%>
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding my-2">
    <div class="ms-formtable accomp-popup ">
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Wiki Name</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:Textbox ID="txtTitle" runat="server"></asp:Textbox>
                 <div>
                    <asp:RequiredFieldValidator ID="rfvtxtTilte" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle" ErrorMessage="Field required!" Display="Dynamic" ValidationGroup="WikiNavigation"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="trItemOrder" runtat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Item Order</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:Textbox ID="txtItemOrder" runat="server"> </asp:Textbox>
                <div>
                    <asp:RegularExpressionValidator ID="refvtxtItemOrder" ControlToValidate="txtItemOrder" runat="server" ErrorMessage="Only number." Display="Dynamic" ValidationGroup="WikiNavigation" ValidationExpression="^[0-9]*$"></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr2" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Image URL</h3> 
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:UGITFileUploadManager ID="fileUploadIcon" runat="server" AnchorLabel="Upload Icon" hideWiki="true" />
            </div>
        </div>

        <div class="row" id="tr3">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Wiki Type</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownlist ID="ddlWikiType" runat="server" OnSelectedIndexChanged="ddlWikiType_SelectedIndexChanged" 
                    AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList">
                    <asp:ListItem Text="AllWiki" Value="AllWiki"></asp:ListItem>
                    <asp:ListItem Text="MyWiki" Value="MyWiki"></asp:ListItem>
                    <asp:ListItem Text="PopularWiki" Value="PopularWiki"></asp:ListItem>
                    <asp:ListItem Text="FavoriteWiki" Value="FavoriteWiki"></asp:ListItem>
                    <asp:ListItem Text="ArchiveWiki" Value="ArchiveWiki"></asp:ListItem>
                    <asp:ListItem Text="CustomWiki" Value="CustomWiki"></asp:ListItem>
                </asp:DropDownlist>
            </div>
        </div>
        <div class="row" id="trExpression" runat="server" visible="false">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Condition</h3>
            </div>
            <div class="ms-formbody accomp_inputField">   
                <asp:Label Width="93%" ID="lblCondition" CssClass="skipcondition" runat="server"></asp:Label>
                <img id="Img1" runat="server" src="/Content/Images/editNewIcon.png" onclick="OpenConditionPicker();" 
                    style="padding-left:6px;float:right;cursor:pointer;width:16px;" />                    
            </div>
            <div  class="ms-formbody">
                <dx:ASPxHiddenField ID="hdnSkipOnCondition" runat="server" ClientInstanceName="hdnSkipOnCondition"></dx:ASPxHiddenField>
            </div>         
        </div> 
        <div class="d-flex justify-content-between align-items-center px-1">
            <dx:ASPxButton ID="LnkbtnDelete" runat="server" Visible="true" Text="Delete" ToolTip="Delete" AutoPostBack="false" CssClass="btn-danger1">
                <ClientSideEvents Click="function(s,e){DeleteCongigVariable();}" />
            </dx:ASPxButton>
            <div>
                <asp:LinkButton ID="lnkDelete1" runat="server" OnClick="LnkbtnDelete_Click"></asp:LinkButton>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnClose_Click"></dx:ASPxButton>
                <dx:ASPxButton ValidationGroup="WikiNavigation" ID="btnSave" Visible="true" runat="server" Text="Save" ToolTip="Save as Template" CssClass="primary-blueBtn"  OnClick="btnSave_Click"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>


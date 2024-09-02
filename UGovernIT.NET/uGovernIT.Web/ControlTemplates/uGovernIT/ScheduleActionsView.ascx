<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleActionsView.ascx.cs" Inherits="uGovernIT.Web.ScheduleActionsView" %>

<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function NewWindowPopup() {
        var url = hdnConfiguration.Get("NewUrl");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        window.UgitOpenPopupDialog(url, '', 'Add New Schedule Action', '620px', '650px', 0, escape(requestUrl));
    }

    function EditWindowPopup(id, title) {
        var url = hdnConfiguration.Get("EditUrl");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var param = "&Id=" + id;
        window.UgitOpenPopupDialog(url, param, 'Schedule Action - ' + title, '620px', '650px', 0, escape(requestUrl));
    }

    function ViewWindowPopup(id, title) {
        var url = hdnConfiguration.Get("ViewUrl");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var param = "&Id=" + id;
        window.UgitOpenPopupDialog(url, param, 'Schedule Action - ' + title, '600px', '900px', 0, escape(requestUrl));
    }

  
</script>
<dx:ASPxHiddenField ID="hdnConfiguration" runat="server" ClientInstanceName="hdnConfiguration"></dx:ASPxHiddenField>
<div id="content" class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" runat="server" id="div_header">
        <div class="formLayout-dropDownWrap col-md-4 col-sm-4 col-xs-12 noLeftPadding">
            <div class="ms-formlabel">
                  <h3 class="ms-standardheader budget_fieldLabel">Menu</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <asp:DropDownList ID="ddlActionType" runat="server" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList" 
                     OnSelectedIndexChanged="ddlActionType_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-12 noRightPadding" style="padding-top:25px;">
            <div class="headerContent-right">
                 <div class="headerItem-addItemBtn">
                       <dx:ASPxButton ID="aAddItem_Top" runat="server" AutoPostBack="false" Text="Add New Item" Image-Url="~/Content/Images/plus-symbol.png" Image-Width="12" 
                           CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s,e){NewWindowPopup();}" />
                        </dx:ASPxButton>
                 </div>
            </div>
        </div>
    </div>
    <div class="row">
        <ugit:AspxGridView ID="aspxGridView" runat="server" Width="100%" OnDataBinding="aspxGridView_DataBinding" CssClass="customgridview homeGrid" 
                            OnHtmlRowPrepared="aspxGridView_HtmlRowPrepared" KeyFieldName="ID" >
                <SettingsText EmptyDataRow="No record found." />
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            </Styles>
        </ugit:AspxGridView>
    </div>
    <div class="row bottom-addBtn">
        <div class="headerItem-addItemBtn">
            <dx:ASPxButton ID="aAddItem" runat="server" AutoPostBack="false" Text="Add New Item" Image-Url="~/Content/Images/plus-symbol.png" Image-Width="12" CssClass="primary-blueBtn">
                    <ClientSideEvents Click="function(s,e){NewWindowPopup();}" />
            </dx:ASPxButton>
        </div>
    </div>
</div>




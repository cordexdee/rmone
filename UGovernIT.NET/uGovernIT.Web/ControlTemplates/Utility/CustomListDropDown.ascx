<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomListDropDown.ascx.cs" Inherits="uGovernIT.Web.CustomListDropDown" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .container {
        width: 100%;
        float: left;
    }

    .input-control {
        width: 100%;
        float: left;
    }

    .link-icon {
        width: 2%;
        float: left;
        text-align: left;
        vertical-align: middle;
        padding-left: 5px;
    }

        .link-icon img {
            border: 0;
            background-color: transparent;
        }

    .containerMult {
        padding-top: 4px;
    }

    .contactmail {
        display: none;
        float: left;
        padding: 2px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        updateMarketSector();
        DisplaySearchImage();
    });


    function GridLookup_EndCallback(s, e) {
        DisplaySearchImage();
    }

    function setContactValue(value) {
        if ($(".ContactLookup").find("[id$='_hndContact']").length > 0) {
            $(".ContactLookup").find("[id$='_hndContact']").val(value);
        }
    }

    function updateMarketSector() {

        if ($(".CRMCompanyLookup").find("[id$='_hndCompany']").length > 0) {
            try {
                $(".MarketSector").val($(".CRMCompanyLookup").find("[id$='_hndCompany']").val());
                //$(".MarketSector").parent().parent().parent().find(".labelvalue").html($("[id$='_hndCompany']").val());
                $(".MarketSector").get(0).style.pointerEvents = 'none';
            }
            catch (e) {
            }
        }
    }

    function DisplaySearchImage() {
        $(".containerMult").each(function (i, item) {
            if ($(item).find(".dxgvCommandColumn_UGITClassicDevEx").length > 0 && $($(item).find(".dxgvCommandColumn_UGITClassicDevEx").get(0)).find('.magnifier').length == 0) {

                $($(item).find(".dxgvCommandColumn_UGITClassicDevEx").get(0)).append('<img class="magnifier" style="cursor: pointer; " src="/_layouts/15/images/uGovernIT/search-black.png" alt="Search">');
            }
        });
    }

    function Panel_EndCallback(s, e) {

        if (typeof (s.cpMarketSector) != 'undefined') {
            $(".MarketSector").val(s.cpMarketSector);
        }

        LoadingPanel.Hide();
        DisplaySearchImage();
    }

    function ShowHideControl(obj) {

        if ($(obj).parent().parent().find('.containerMult').length > 0) {
            $(obj).parent().parent().find('.container').css("display", "none");
            $(obj).parent().parent().find('.containerMult').css("display", "block");
            $(obj).parent().css("display", "none");
        }
        else {
            $(obj).parent().parent().find('.container').css("display", "block");
            $(obj).parent().parent().find('.containerMult').css("display", "none");
            $(obj).parent().css("display", "none");
        }
        return false;
    }

    function updateLeadSourceCompany(contactId) {
        var paramsInJson = '{' + '"contactID":"' + contactId + '"}';
        $.ajax({
            type: "POST",
            url: "<%=ajaxHelper %>/GetLeadSourceCompany",
            data: paramsInJson,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                var resultJson = $.parseJSON(message.d);
                if (resultJson.messagecode == 2) {
                    $(".LeadSourceCompanyLabel").val(resultJson.LeadSourceCompany);
                }
                else {

                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //alert(thrownError);
            }
        });
    }


    function showcontacteditbuttonOnhover(obj) {

        $(obj).find('.editiconcontact').css("display", "block");
    }

    function hidecontacteditbuttonOnhover(obj) {

        $(obj).find('.editiconcontact').css("display", "none");
    }

    function showmailbuttonOnhover(obj) {

        $(obj).find('.contactmail').css("display", "block");
    }

    function hidemailbuttonOnhover(obj) {

        $(obj).find('.contactmail').css("display", "none");
    }

    function ShowMultEntities() {
        pcMain.Show();
    }

    //function CloseGridLookup() {
    //    gridLookup.ConfirmCurrentSelection();
    //    gridLookup.HideDropDown();
    //    gridLookup.Focus();
    //}

    function OnEndCallback() {
        LoadingPanel.Hide();

    }

    //function onSelectionChange() {
    //    LoadingPanel.SetText('Loading Contacts ...');
    //    LoadingPanel.Show();
    //    CallbackPanel.PerformCallback('CompanyTitle');
    //}

    function OnDropDown(obj) {             
    }
        
    function OnValueChanged(obj) {     
        $.cookie("dataChanged", 1, { path: "/" });
    }
</script>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True" Visible="false"></dx:ASPxLoadingPanel>
<asp:Panel ID="pnlContact" runat="server" CssClass="customList-dropDownWrap container">

    <div class="input-control">
        <%--<dx:ASPxComboBox ID="ddlList" runat="server" ValueType="System.String" Width="100%" ClientSideEvents-EndCallback="OnEndCallback" OnCallback="ddlList_Callback" ItemStyle-Wrap="True"
            DropDownStyle="DropDown" IncrementalFilteringMode="Contains" CallbackPageSize="100"
            OnItemsRequestedByFilterCondition="ddlList_ItemsRequestedByFilterCondition" OnItemRequestedByValue="ddlList_ItemRequestedByValue" OnSelectedIndexChanged="ddlList_SelectedIndexChanged"
            EnableCallbackMode="True">
           <ClientSideEvents DropDown="function(s, e) { OnDropDown(s); }" ValueChanged="function(s, e) { OnValueChanged(s); }" />
        </dx:ASPxComboBox>--%>

        <dx:ASPxTokenBox ID="ddlList" runat="server" OnCustomFiltering="ddlList_CustomFiltering" CallbackPageSize="100" EnableCallbackMode="true" ShowDropDownOnFocus="Always" AllowCustomTokens="false" AutoPostBack="false" 
            SettingsLoadingPanel-Enabled="false">
            <ClientSideEvents EndCallback="OnEndCallback"  ValueChanged="function(s, e) { $.cookie('dataChanged', 1, { path: '/' }); }" />
        </dx:ASPxTokenBox>

        <div id="dvReload" runat="server" style="display: none;">
            <asp:Button ID="btnReloadDropDown" runat="server" OnClick="btnReloadDropDown_Click" /><asp:HiddenField ID="hndValue" runat="server" />
        </div>
        <div id="dvHiddenFields" runat="server">
            <asp:HiddenField ID="hndContact" runat="server" />
            <asp:HiddenField ID="hndCompany" runat="server" />
        </div>
    </div>
    <div class="link-icon">
        <img id="imgAdd" src="~/Content/Images/ugovernit/add_icon.png" runat="server" style="padding-top: 6px; display: none;" />
    </div>
</asp:Panel>

<asp:Panel ID="pnlContactMult" runat="server" CssClass="containerMult" ClientVisible="false">

    <dx:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="CompanyContact" OnCallback="ASPxCallbackPanel1_Callback">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent3" runat="server">

                <div style="padding-top: 5px">

                    <dx:ASPxGridLookup ID="GridLookup" runat="server" SelectionMode="Multiple" ClientInstanceName="gridLookup"
                        KeyFieldName="TicketId" Width="100%" TextFormatString="{0}" MultiTextSeparator=", " OnDataBinding="GridLookup_DataBinding" GridViewProperties-EnableCallBacks="true"
                        IncrementalFilteringMode="Contains" DropDownWindowStyle-CssClass="aspxGridLookup-dropDown" GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow" 
                        GridViewStyles-FilterRow-CssClass="aspxGridLookUp-FilerWrap" GridViewStyles-FilterCell-CssClass="aspxGridLookUp-FilerCell" 
                        GridViewProperties-SettingsLoadingPanel-Mode="Disabled">

                        <Columns>
                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" />
                            <dx:GridViewDataColumn FieldName="Title" />
                        </Columns>
                        <GridViewProperties>
                            <%--  <Templates>
                                    <StatusBar>
                                        <table class="OptionsTable" style="float: right">
                                            <tr>
                                                <td>
                                                    <dx:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                                </td>
                                            </tr>
                                        </table>
                                    </StatusBar>
                            </Templates>--%>
                            <Settings ShowFilterRow="True" ShowStatusBar="Hidden" ShowColumnHeaders="false" />

                        </GridViewProperties>
                        <ClientSideEvents EndCallback="GridLookup_EndCallback" Init="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());} "
                            DropDown="function(s, e) {	s.GetGridView().SetWidth(s.GetWidth()-2);} " ValueChanged="function(s, e) { $.cookie('dataChanged', 1, { path: '/' }); }" />
                    </dx:ASPxGridLookup>

                </div>


            </dx:PanelContent>
        </PanelCollection>
        <SettingsLoadingPanel Enabled="false" />
        <ClientSideEvents EndCallback="Panel_EndCallback" />
    </dx:ASPxCallbackPanel>

</asp:Panel>

<div id="divEditContact" runat="server" style="min-height: 20px; padding-top: 8px">
    <img id="editContact" class="editiconcontact" src="~/Content/Images/editNewIcon.png" runat="server" style="display: none; float: left;width:14px" onclick="ShowHideControl(this)" />
    <asp:Label ID="lblContact" runat="server" Visible="false" Style="float: left; padding-top: 3px;" />
    <asp:HyperLink ID="hlnk" runat="server" Visible="false" Style="cursor: pointer;"></asp:HyperLink>
    <img id="mailTo" class="contactmail" src="~/Content/Images/MailTo.png" runat="server" style="display: none" onclick="ticketContactEmail(this);" />
</div>

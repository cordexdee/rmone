<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilterTicketsProperties.ascx.cs" Inherits="uGovernIT.Web.FilterTicketsProperties" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function pickFilter(filter)
    {
        filter = unescape(filter).replace(/@/g, "\'");
        $($(".filterexpression").get(0)).text(filter);
        hdnDataFilterExpression.Set("FilterExpression", filter);
    }
    function ClearConditionPicker()
    {
        $($(".filterexpression").get(0)).text('');
        hdnDataFilterExpression.Set("FilterExpression", '');
    }
    function OpenConditionPicker() {
        
    <%--  var Url = '<%= DataFilterUrl%>';
        if ($($(".filterexpression").get(0)).text() != '') {
            Url += "&SkipCondition=" + escape($($(".filterexpression").get(0)).text());
        }
        javascript: UgitOpenPopupDialog(Url, '', 'Filter Expression', '85', '80', 0, '');--%>
    }
   
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 100px;
        vertical-align: middle;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .padding-left5 {
        padding-left: 5px;
    }
</style>


<table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse;overflow-x:auto" width="100%">
    <tr id="tr12" runat="server" >
        <td class="ms-formlabel" style="width:130px!important;">
            <h3 class="ms-standardheader">Title</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                    <dx:ASPxCheckBox ID="chkTitle" runat="server">
                        <ClientSideEvents
                            Init="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }"
                            CheckedChanged="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }" />
                    </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                    <dx:ASPxTextBox ID="txtTitle" ClientInstanceName="txtTitle" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>
<%--    <tr id="tr2" runat="server" style="display:none">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Name</h3>
        </td>
        <td class="ms-formbody">
            <div>                
                <div class="fleft padding-left5">
                    <dx:ASPxTextBox ID="txtName" ClientInstanceName="txtName" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>--%>
    <tr id="trTitle" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Module</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <dx:ASPxComboBox ID="ddlModule" EnableCallbackMode="true"  runat="server" OnInit="ddlModule_Init" NullText="Select Module">
                    <ClientSideEvents SelectedIndexChanged="function(s,e){repeaterTabViewControlCallBackPanel.PerformCallback(s.GetValue());}" />
                </dx:ASPxComboBox>
            </div>
        </td>
    </tr>
    <tr id="tr1" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">No. of Tickets</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <dx:ASPxTextBox ID="txtNoOfTickets" runat="server" NullText="No. Of tickets"></dx:ASPxTextBox>
            </div>
        </td>
    </tr>   
    <tr id="tr3" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Options</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <dx:ASPxCheckBox ID="chkModuleLogo" runat="server" Text="Module Logo" TextAlign="Right"></dx:ASPxCheckBox>
                <dx:ASPxCheckBox ID="chkModuleDescription" runat="server" Text="Module Description" TextAlign="Right"></dx:ASPxCheckBox>
                <dx:ASPxCheckBox ID="chkNewbutton" runat="server" Text="New Button" TextAlign="Right"></dx:ASPxCheckBox>
                <dx:ASPxCheckBox ID="chkFilteredTabs" runat="server" Text="Filter Tabs" TextAlign="Right"></dx:ASPxCheckBox>
                <dx:ASPxCheckBox ID="chkShowBandedRows" runat="server" Text="Banding" TextAlign="Right"></dx:ASPxCheckBox>
                <dx:ASPxCheckBox ID="chkShowCompactRows" runat="server" Text="Compact" TextAlign="Right"></dx:ASPxCheckBox>
                <%--<dx:ASPxCheckBox ID="chkGlobalSearch" runat="server" Text="Status Over Progress Bar" TextAlign="Right"></dx:ASPxCheckBox>
                <dx:ASPxCheckBox ID="chkStatusOverProgressBar" runat="server" Text="Module Search" TextAlign="Right"></dx:ASPxCheckBox>--%>
            </div>
        </td>
    </tr>    
    <tr id="trDataFilterExp" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Data Filter Expression</h3>
        </td>
        <td class="ms-formbody">
 <asp:Label Width="93%" ID="lblDataFilterExpression" CssClass="filterexpression" runat="server"></asp:Label>
            <img id="Img2" runat="server" src="/content/images/delete-icon.png" onclick="ClearConditionPicker();" style="padding-left: 6px; float: right; cursor: pointer;" />
                        <img id="Img1" runat="server" src="/content/images/edit-icon.png" onclick="OpenConditionPicker();" style="padding-left: 6px; float: right; cursor: pointer;" />
             
                        <dx:ASPxHiddenField ID="hdnDataFilterExpression" runat="server" ClientInstanceName="hdnDataFilterExpression"></dx:ASPxHiddenField>
        </td>
      
    </tr>
</table>

<dx:ASPxCallbackPanel ID="control" SettingsLoadingPanel-Enabled="true"  HideContentOnCallback="true" runat="server" ClientInstanceName="repeaterTabViewControlCallBackPanel" OnCallback="control_Callback">
    <PanelCollection>
        <dx:PanelContent>
            <asp:Repeater ID="repeaterTabView" runat="server">
    <HeaderTemplate>
        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse;overflow-x:auto" width="100%">
    </HeaderTemplate>
    <ItemTemplate>
        <tr id="tr12" runat="server">
        <td class="ms-formlabel" style="width:130px!important;">
            <h3 class="ms-standardheader"><%# Eval("TablabelName") %></h3>
            <asp:HiddenField ID="idField" runat="server"  Value=<%# Eval("ID") %> />
            <asp:HiddenField ID="hdnTabOrder" runat="server"  Value=<%# Eval("TabOrder") %> />
            <asp:HiddenField ID="hdnTablabelName" runat="server"  Value=<%# Eval("TablabelName") %> />
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                    <dx:ASPxCheckBox ID="chkTitle" runat="server" Checked=<%# Eval("ShowTab") %> ClientSideEvents-CheckedChanged='<%# "function(s,e) { if(s.GetChecked()){"+ Eval("TabName") +".SetVisible(true)}else{"+ Eval("TabName") +".SetVisible(false)} }" %>' 
                        ClientSideEvents-Init='<%# "function(s,e) { if(s.GetChecked()){"+ Eval("TabName") +".SetVisible(true)}else{"+ Eval("TabName") +".SetVisible(false)} }" %>' >

                        <%--<ClientSideEvents Init='<%# String.Format("function(s,e){ if(s.GetChecked()){"{0}".SetVisible(true)}else{"{0}".SetVisible(false)}", Eval("TabName"))) %> }'
                            CheckedChanged='<%# String.Format("function(s,e){ if(s.GetChecked()){"{0}".SetVisible(true)}else{"{0}".SetVisible(false)}", Eval("TabName"))) %>}' />--%>                        
                    </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                    <dx:ASPxTextBox ID="txtDisplayName" ClientInstanceName=<%# Eval("TabName") %> Text=<%# Eval("TabDisplayName") %> runat="server" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-RequiredField-ErrorText="Required"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>
    </ItemTemplate>
    <FooterTemplate></table></FooterTemplate>
</asp:Repeater>

        </dx:PanelContent>
    </PanelCollection>

</dx:ASPxCallbackPanel>

﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketCountTrendsProperties.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.DockPanels.DockPanelEditControl.TicketCountTrendsProperties" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

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
<script id="dxss_dtc" type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var textSeparator = ";";
    function updateText(listBox, args) {
        var selectedItems = checkListBox.GetSelectedItems();
        checkComboBox.SetText(getSelectedItemsText(selectedItems));
    }
    function synchronizeListBoxValues(dropDown, args) {
        checkListBox.UnselectAll();
        var texts = dropDown.GetText().split(textSeparator);
        var values = getValuesByTexts(texts);
        checkListBox.SelectValues(values);
        updateText(null,null); // for remove non-existing texts
    }
    function getSelectedItemsText(items) {
        var texts = [];
        for (var i = 0; i < items.length; i++)
            texts.push(items[i].text);
        return texts.join(textSeparator);
    }
    function getValuesByTexts(texts) {
        var actualValues = [];
        var item;
        for (var i = 0; i < texts.length; i++) {
            item = checkListBox.FindItemByText(texts[i]);
            if (item != null)
                actualValues.push(item.value);
        }
        return actualValues;
    }
</script>

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
    <tr>
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Module(s)</h3>
        </td>
        <td class="ms-formbody">
            <div class="fleft">
                <dx:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="ddeModules" OnInit="ddeModules_Init" Width="196px" runat="server" AnimationType="None">
                    <DropDownWindowTemplate>
                        <dx:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                            runat="server" EnableSelectAll="true">
                            <Border BorderStyle="None" />
                            <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                            <Items>
                            </Items>
                            <ClientSideEvents SelectedIndexChanged="updateText" Init="updateText" />
                        </dx:ASPxListBox>
                    </DropDownWindowTemplate>
                    <ClientSideEvents TextChanged="synchronizeListBoxValues" DropDown="synchronizeListBoxValues" />
                </dx:ASPxDropDownEdit>
            </div>
        </td>
    </tr>
    <tr>
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Default Module</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <dx:ASPxComboBox ID="cmbdefaultModule" runat="server" ClientInstanceName="cmbdefaultModule" Width="196px"></dx:ASPxComboBox>
            </div>
        </td>
    </tr>
</table>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationRoleEdit.ascx.cs" Inherits="uGovernIT.Web.ApplicationRoleEdit" %>
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
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    tr.alternet {
        background-color: whitesmoke;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var textSeparator = ";";
    function OnListBoxSelectionChanged(listBox, args) {

        if (args.index == 0)
            args.isSelected ? listBox.SelectAll() : listBox.UnselectAll();
        UpdateSelectAllItemState();
        UpdateText();
    }
    function UpdateSelectAllItemState() {
        IsAllSelected() ? checkListBox.SelectIndices([0]) : checkListBox.UnselectIndices([0]);
    }
    function IsAllSelected() {
        var selectedDataItemCount = checkListBox.GetItemCount() - (checkListBox.GetItem(0).selected ? 0 : 1);
        return checkListBox.GetSelectedItems().length == selectedDataItemCount;
    }
    function UpdateText() {
        var selectedItems = checkListBox.GetSelectedItems();
        var selectedItemsCount = checkListBox.GetSelectedItems().length;
        var totalItemCount = checkListBox.GetItemCount();
        if (parseInt(totalItemCount) == parseInt(selectedItemsCount))
            checkComboBox.SetText("All");
        else
            checkComboBox.SetText(GetSelectedItemsText(selectedItems));
    }
    function SynchronizeListBoxValues(dropDown, args) {
        checkListBox.UnselectAll();
        var texts = dropDown.GetText().split(textSeparator);
        var values = GetValuesByTexts(texts);
        checkListBox.SelectValues(values);
        UpdateSelectAllItemState();
        UpdateText(); // for remove non-existing texts
    }
    function GetSelectedItemsText(items) {
        var texts = [];
        for (var i = 0; i < items.length; i++)
            if (items[i].index != 0)
                texts.push(items[i].text);
        return texts.join(textSeparator);
    }
    function GetValuesByTexts(texts) {
        var actualValues = [];
        var item;
        for (var i = 0; i < texts.length; i++) {
            item = checkListBox.FindItemByText(texts[i]);
            if (item != null)
                actualValues.push(item.value);
        }
        return actualValues;
    }
    function selectAll()
    {
        var selectedItemsCount = checkListBox.GetSelectedItems().length;
        if (parseInt(selectedItemsCount) == 0)
        {
            checkListBox.SelectAll();
            UpdateText();
        }
    }
</script>
<table width="100%">
    <tr id="tr2" runat="server">
           <td class="ms-formlabel">
            <h3 class="ms-standardheader">Item Order<b style="color: Red;">*</b>
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:DropDownList ID="ddlItemOrder" runat="server"></asp:DropDownList>
        </td>
     </tr>
    <tr id="trTitle" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Title<b style="color: Red;">*</b>
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtTitle" runat="server" Width="386px" Text='<%# Bind("Title") %>' />
            <div>
                <asp:CustomValidator id="csvTitle"  ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle" ErrorMessage="Role with same name already exists" ForeColor="Red" Display="Dynamic" OnServerValidate="csvTitle_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>
                <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                    ErrorMessage="Enter Title" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
            </div>
        </td>
    </tr>
    <tr>
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Module(s)
            </h3>
        </td>
        <td class="ms-formbody">
            <div id="divAppModules" runat="server"></div>
            <%--<ugit:LookUpValueBox ID="checkComboBox" runat="server" FieldName="ApplicationModulesLookup" IsMulti="true"/>--%>

        </td>
    </tr>
    <tr id="tr1" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Description
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6" cols="20" Text='<%# Bind("UGITDescription") %>' /></td>
    </tr>
    <tr id="tr3" runat="server">
        <td colspan="2">
            <div style="float: right;">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"  OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            </div>
        </td>
    </tr>
</table>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleColumnsAddEdit.ascx.cs" Inherits="uGovernIT.Web.ModuleColumnsAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="ugit" Namespace="uGovernIT.Web" Assembly="uGovernIT.Web" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function SetLengthForDataType(s, e) {

        if (s.GetValue().toLowerCase() == "string")
            $(".clsTruncateText").show();
        else
            $(".clsTruncateText").hide();
    }

    function HideShowNoOfChars(s, e) {
        if (s.GetChecked())
            $('.clsNoOfChars').show();
        else
            $('.clsNoOfChars').hide();
    }

    $(function () {
        
        if (cmbColumnType != null && cmbColumnType != undefined && cmbColumnType.GetValue()!=null && cmbColumnType.GetValue()!=undefined && cmbColumnType.GetValue().toLowerCase() == "string") {
            $(".clsTruncateText").show();
            if (chkTruncate.GetChecked())
                $('.clsNoOfChars').show();
            else
                $('.clsNoOfChars').hide();
        }
        else {
            $(".clsTruncateText").hide();
            $('.clsNoOfChars').hide();
        }
    });

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
            ddlSelectedTabs.SetText("All");
        else
            ddlSelectedTabs.SetText(GetSelectedItemsText(selectedItems));
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
    function SynchronizeListBoxValues(dropDown, args) {
        checkListBox.UnselectAll();
        var texts = dropDown.GetText().split(textSeparator);
        var values = GetValuesByTexts(texts);
        checkListBox.SelectValues(values);
        UpdateSelectAllItemState();
        UpdateText(); // for remove non-existing texts
    }
</script>
<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer py-2">
    <div class="ms-formtable accomp-popup ">
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel"><span id="ModuleLabel" runat="server"> Module</span></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModule" runat="server" CssClass=" itsmDropDownList aspxDropDownList"></asp:DropDownList>
                <div>
                    <asp:RequiredFieldValidator ID="rfvddlModule" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModule"
                        ErrorMessage="Select Module" InitialValue="0" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr12" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Field<b style="color: red">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxComboBox ID="cmbFieldName" runat="server" width="100%" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox"
                        DropDownStyle="DropDownList" TextFormatString="{0}" ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0"
                        EnableSynchronization="True" CallbackPageSize="10" OnSelectedIndexChanged="cmbFieldName_SelectedIndexChanged" AutoPostBack="true">
                        <Columns>
                        </Columns>
                    </dx:ASPxComboBox>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvFieldName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="cmbFieldName"
                            ErrorMessage="Enter Field Name" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr13" runat="server">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Display Name</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDisplayName" runat="server"/>
                     <asp:Label ID="lblerrormsg" ForeColor="Red" runat="server" ></asp:Label>
                </div>
            </div>
        </div>
        <div class="row" >
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr14" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Field Sequence</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlFieldSequence" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Column Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxComboBox ID="cmbColumnType" ClientInstanceName="cmbColumnType" Width="100%" runat="server" CssClass=" aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox"
                        DropDownStyle="DropDown" TextFormatString="{0}" ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" 
                        EnableSynchronization="True" CallbackPageSize="10">
                        <ClientSideEvents ValueChanged="function(s,e){SetLengthForDataType(s,e);}" />
                        <Columns>
                        </Columns>
                    </dx:ASPxComboBox>
                </div>
            </div>
        </div>

       
        <div class="row">
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">Sort Order:</h3>
             </div>
            <div class="ms-formbody accomp_inputField">
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" style="padding-right:6px !important;">
                  <asp:DropDownList ID="ddlSortOrder" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding" style="padding-left:6px !important;">
                  <asp:DropDownList ID="ddlSortDirection" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                </div>
            </div>
        </div>
        
        <div class="row clsTruncateText" id="divTruncateText" runat="server">
            <div class="ms-formbody accomp_inputField">
                <div  class="col-md-6 col-sm-6 col-xs-6 noLeftPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Truncate</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <dx:ASPxCheckBox ID="chkTruncate" runat="server" ClientInstanceName="chkTruncate">
                        <ClientSideEvents CheckedChanged="function(s,e){HideShowNoOfChars(s,e);}" />
                    </dx:ASPxCheckBox>
                        </div>
                </div>
                <div id="divNoOfChars" runat="server" class="col-md-6 col-sm-6 col-xs-6 noLeftPadding clsNoOfChars">
                    <div style="float: left;">
                        <dx:ASPxSpinEdit ID="spnNoOfChars" runat="server" ClientInstanceName="spnNoOfChars" MinValue="10" MaxValue="500" Number="10" ToolTip="Range(10-500) char(s) only" HorizontalAlign="Left" Width="50px"></dx:ASPxSpinEdit>
                    </div>
                    <div style="float: left; margin-top: 5px; margin-left: 5px;"><span style="float: left;">characters</span></div>
                </div>
            </div>    
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr9" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Text Alignment</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:LookUpValueBox ID="ddlTextAlignment" Width="100%" runat="server" IsMulti="False" FieldName="TextAlignmentChoice" CssClass="lookupValueBox-dropown"></ugit:LookUpValueBox>
                </div>
            </div>
           <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="Div1" runat="server">
               <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Show In Tabs</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxDropDownEdit ClientInstanceName="ddlSelectedTabs" ID="ddlSelectedTabs" DisplayFormatString="{0}" CssClass="aspx-dropDown-edit" Width="100%" runat="server" AnimationType="None">
                        <DropDownWindowTemplate>
                            <dx:ASPxListBox Width="100%" ID="listBox" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                                runat="server">
                                <ClientSideEvents SelectedIndexChanged="OnListBoxSelectionChanged" />
                                <Items>
                                </Items>
                            </dx:ASPxListBox>
                            <table style="width: 100%">
                                <tr>
                                    <td style="padding: 4px">
                                        <dx:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                            <ClientSideEvents Click="function(s, e){ ddlSelectedTabs.HideDropDown(); }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </DropDownWindowTemplate>
                    </dx:ASPxDropDownEdit>
                </div>
           </div>
        </div>

        <div class="row" >
             <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr15" runat="server">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkShowInCardView" runat="server" Text="Show In Card View" />
                </div>
            </div>
            
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr5" runat="server">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkShowInMobile" runat="server" Text="Show In Mobile" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr8" runat="server">
                  <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkDisplayForReport" runat="server" Text="Display For Report" />
                </div>
            </div>
           <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr6" runat="server">
                 <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkIsUseInWildCard" runat="server" Text="Use in Search" />
                </div>
            </div>
        </div>
        <div class="row" >
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr3" runat="server">
                 <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkDisplay" runat="server" Text="Display" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr7" runat="server">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkDisplayForClosed" runat="server" Text="Display For Closed" />
                </div>
            </div>
        </div>
        

        <div class="row" id="tr10" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Custom Properties</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtCustomProperties" TextMode="MultiLine" runat="server"></asp:TextBox>
            </div>
        </div>        

        <div class="d-flex justify-content-between align-items-center px-1" id="tr2" runat="server">
            <dx:ASPxButton ID="LnkbtnDelete" runat="server" Visible="true" Text="Delete" ToolTip="Delete" OnClick="LnkbtnDelete_Click" CssClass="btn-danger1">
                <ClientSideEvents Click="function(s,e){return confirm('Are you sure you want to delete?');}" />
            </dx:ASPxButton>
            <div>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass=" secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>

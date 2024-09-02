<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalRolesAddEdit.ascx.cs" Inherits="uGovernIT.Web.GlobalRolesAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .lblstyle {
        font-size: 12px;
        margin: 9px 6px;
        display: inline-block;
        color: #666;
    }

    .dxeMemo_UGITNavyBlueDevEx {
        padding: 0;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function addNewField(s, e) {
        cmbFieldName.SetVisible(false);
        lblFieldName.SetVisible(true);
        imgCancelAddField.SetVisible(true);
        imgAddField.SetVisible(false);
        hdnNewFieldFlag.Set("IsNewField", true);
        var name = txtName.GetText();
        if (name) {
            var newString = name.replace(/[^A-Z0-9]/ig, "") + "User";
            lblFieldName.SetText(newString);
        }
    }

    function cancelNewField(s, e) {
        cmbFieldName.SetVisible(true);
        lblFieldName.SetVisible(false);
        imgCancelAddField.SetVisible(false);
        imgAddField.SetVisible(true);
        hdnNewFieldFlag.Set("IsNewField", false);
    }

    function btnSave_ClientClick(s, e) {
        btnSave.DoClick();
    }

    function txtName_TextChanged(s, e) {
        debugger;
        var name = txtName.GetText();
        if (name) {
            var newString = name.replace(/[^A-Z0-9]/ig, "") + "User";
            lblFieldName.SetText(newString);
        }
        updateShortName(name);
    }
    function updateShortName(Name) {
        // Get the short name by extracting the letters of the Role
        document.getElementById('<%= txtshortName.ClientID %>').value = Name;
    }
</script>
<dx:ASPxHiddenField ID="hdnNewFieldFlag" runat="server" ClientInstanceName="hdnNewFieldFlag"></dx:ASPxHiddenField>
<div class="mt-3">
    <div class="ms-formtable accomp-popup">
        <div class="row bs">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Name<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody">
                    <dx:ASPxTextBox ID="txtName" ClientInstanceName="txtName" runat="server" CssClass="accomp-popup" Width="100%">
                        <ClientSideEvents UserInput="txtName_TextChanged"/>
                        <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="Save" ErrorDisplayMode="ImageWithTooltip"
                            ErrorText="Name is Required.">
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Field Name</h3>
                </div>
                <div class="d-flex align-items-center mb-2">
                    <div class="ms-formbody cmbFieldName mr-1">
                        <dx:ASPxComboBox ID="cmbFieldName" ClientInstanceName="cmbFieldName" runat="server" IncrementalFilteringMode="Contains" ListBoxStyle-CssClass="aspxComboBox-listBox"
                            CssClass="aspxComBox-dropDown">
                        </dx:ASPxComboBox>
                        <dx:ASPxLabel ID="lblFieldName" runat="server" ClientInstanceName="lblFieldName" CssClass="lblstyle" ClientVisible="false"></dx:ASPxLabel>
                    </div>
                    <div style="cursor: pointer;">
                        <dx:ASPxImage ID="imgAddField" runat="server" ClientInstanceName="imgAddField" ImageUrl="/content/images/plus-blue.png" Width="20">
                            <ClientSideEvents Click="addNewField" />
                        </dx:ASPxImage>
                        <dx:ASPxImage ID="imgCancelAddField" runat="server" ClientInstanceName="imgCancelAddField" ImageUrl="/content/images/close-blue.png" ClientVisible="false" Width="20">
                            <ClientSideEvents Click="cancelNewField" />
                        </dx:ASPxImage>
                    </div>
                </div>
            </div>
        </div>
        <div class="row bs">
             <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Short Name <b style="color: Red;">*</b> </h3>
                </div>
                <div class="ms-formbody">
                <asp:TextBox ID="txtshortName" runat="server" ClientIDMode="Static" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtShortName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtshortName"
                        ErrorMessage="Enter Short Name" Display="Dynamic" CssClass="error" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                </div>
                <div class="ms-formbody">
                    <dx:ASPxMemo ID="memoDescription" ClientInstanceName="memoDescription" CssClass="aspxMemo-linkBox" runat="server"></dx:ASPxMemo>
                </div>
            </div>
            
        </div>
        <div class="mt-2">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                </div>
                <div class="ms-formbody">
                    <asp:CheckBox ID="chkDeleted" runat="server" TextAlign="Right" Text="(Prevent use for new Role)" />
                </div>
            </div>
        <div class="d-flex justify-content-end align-items-center pt-2">
            <div class="col-md-8 col-sm-8 col-xs-8">
                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Text=""></asp:Label>
             </div>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" ClientInstanceName="btnSave" CssClass="primary-blueBtn" AutoPostBack="false" runat="server" Text="Save" OnClick="btnSave_Click"
                ValidationGroup="Save">
                <ClientSideEvents Click="btnSave_ClientClick" />
            </dx:ASPxButton>

        </div>
    </div>
</div>

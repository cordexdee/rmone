<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddCheckList.ascx.cs" Inherits="uGovernIT.Web.AddCheckList" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
   
    /*.ms-formbody {
        padding: 3px 6px 4px;
    }*/

</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.fetch-parent').parent().addClass("popupUp-mainContainer");
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(7).addClass('userValueBox-mainContainer');

        $('#lblFileName').html("No file Chosen");
    });
    //var currentdiv = $(".fetch-parent").closest('div');
    //currentdiv.addClass("popupUp-mainContainer");
   
    function UploadFile(fileUpload) {
        if (fileUpload.value != '') {
            var fullFileName = document.getElementById("<%=flpCheckListItemImport.ClientID %>").value;
            var fileNameArr = fullFileName.split("\\");
            var fileName = fileNameArr[fileNameArr.length - 1];
            $('#lblFileName').html(fileName);
        }
    }
</script>


<div id="tb1" class="ms-formtable accomp-popup row mt-2">
    <div class="col-md-6 col-sm-6 col-xs-12 noPadding" id="trModule" runat="server">
        <div class="ms-formlabel">
            <p class="budget_fieldLabel">Module<b style="color: Red;">*</b></p>
        </div>
        <div class="ms-formbody">
            <asp:DropDownList ID="ddlModule" CssClass=" itsmDropDownList aspxDropDownList" runat="server" AppendDataBoundItems="true">
            </asp:DropDownList>
        </div>
    </div>

    <div class="col-md-6 col-sm-6 col-xs-12 noPadding mt-2" id="tr12" runat="server">
        <div class="ms-formlabel">
            <p class="budget_fieldLabel">CheckList Name<b style="color: Red;">*</b></p>
        </div>
        <div class="ms-formbody">
            <asp:TextBox ID="txtCheckListName" runat="server"/>
            <div>
                <asp:RequiredFieldValidator ID="rfvCheckListName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtCheckListName"
                    ErrorMessage="Field required." Display="Dynamic" ValidationGroup="SaveCheckList"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>

    <div class="col-md-6 col-sm-6 col-xs-12 noPadding mt-2" id="trUser" runat="server" visible="false">
        <div class="ms-formlabel">
            <p class="budget_fieldLabel">Assign To</p>
        </div>
        <div class="ms-formbody">
            <ugit:UserValueBox ID="peAssignedTo" MaximumHeight="10" CssClass="userValueBox-dropDown" runat="server"
                VerticalScrollableHeight="10" />
        </div>
    </div>

    <div class="col-md-6 col-sm-6 col-xs-12 noPadding mt-2" id="trAutoLoad" runat="server" style="display: flex; align-items:center">
       <div class="ms-formlabel">
            <p class="budget_fieldLabel" style="margin-bottom:0">Auto Load CheckList</p>
        </div>
        <div class="ms-formbody ml-2" style="padding-bottom: 0; padding-top: 0">
            <asp:CheckBox ID="chkAutoLoadCheckList" runat="server" />
        </div>
    </div>
    

    <div class="col-md-6 col-sm-6 col-xs-12 noPadding cpr_Row chklist-itemWrap mt-2" id="trCheckListItemImport" runat="server">
        <div class="ms-formlabel">
            <p class="budget_fieldLabel">CheckList Item</p>
        </div>
        <div class="ms-formbody accomp_inputField uploadFile-BtnWrap">
             <dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" CssClass="sayashuba" Visible="false"></dx:ASPxSpreadsheet>
            <label for="flpCheckListItemImport" class="custom-file-upload uploadFile-label"><img src="/content/images/plus-blue.png" style="width:20px;"/>  Choose File </label>
            
            <asp:FileUpload ID="flpCheckListItemImport" CssClass="uploadFile-input" runat="server"/>

        </div>
        <div>
            <label id="lblFileName" class="choosedFileName"></label>
        </div>

    </div>
    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
        <div>&nbsp;
            <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Red"></asp:Label>
        </div>
    </div>   
</div>

<div class="d-flex justify-content-between align-items-center">
    <dx:ASPxButton ID="lnkDeleteCheckList" Visible="false" runat="server" Text="Delete"  OnClick="lnkDeleteCheckList_Click" 
        ToolTip="Delete" CssClass="btn-danger1">
        <ClientSideEvents Click="function(){return confirm('Are you sure you want to delete?');}" />
    </dx:ASPxButton>
    <div>
        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="lnkCancel_Click" 
            CssClass="secondary-cancelBtn"></dx:ASPxButton>
        <dx:ASPxButton ID="btnSaveCheckList" runat="server" CssClass="primary-blueBtn" Text="Save" ToolTip="Save" ValidationGroup="SaveCheckList" OnClick="btnSaveCheckList_Click"></dx:ASPxButton>
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddUserCertificates.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.AddUserCertificates" %>
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

    .auto-style1 {
        text-align: right;
        width: 190px;
        vertical-align: top;
        height: 20px;
    }

    .auto-style2 {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        height: 20px;
    }

      .btnDelete {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/_layouts/15/images/uGovernIT/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
    }
    .flex-direction-row-reverse {
    flex-direction: row-reverse;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function hideddlCategory() {
        $("#<%=ddlCategory.ClientID%>").get(0).selectedIndex = 0;
        $(".ddlCategory").hide();
        $("#<%=hdnCategory.ClientID%>").val('1');
    }
    function showddlCategory() {
        $(".ddlCategory").show();
        $("#<%=hdnCategory.ClientID%>").val('0');
    }
    $(document).ready(function () {
        <%if (!lnkDelete.Visible) { %>
        $("#<%=tr2.ClientID%>").addClass("flex-direction-row-reverse");
        <%}%>
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12 pt-1">
    <div class="accomp-popup ms-formtable">
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Category</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="ddlCategory" id="divddlCategory" runat="server">
                    <div style="width:93%; display:inline-block;">
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    </div>
                    <div style="display:inline-block; margin-left:5px;">
                        <img alt="Add Category" id="Img1" src="/content/images/plus-blue.png" style="cursor: pointer; width:16px;"
                        onclick="javascript:$('.divCategory').attr('style','display:block');hideddlCategory();" />
                    </div>
                </div>
                <div runat="server" id="divCategory" class="divCategory" style="display: none; float: left; padding-left: 10px; text-align: center;">
                     <div style="width:93%; display:inline-block;">
                         <asp:TextBox runat="server" ID="txtCategory" CssClass="txtCategory"></asp:TextBox>
                     </div>
                    <div style="display:inline-block;margin-left:5px;">
                        <img alt="Cancel Category" width="16" src="/content/images/close-red.png" class="cancelModule"
                        onclick="javascript:$('.divCategory').attr('style','display:none');showddlCategory();" />
                    </div>
                    
                </div>
                <div style="width: auto; padding: 4px 4px 0px">
                    <asp:CustomValidator ID="csvdivCategory" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtCategory" ErrorMessage="Select Category" ForeColor="Red" Display="Dynamic" OnServerValidate="csvdivCategory_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>
                    <asp:HiddenField ID="hdnCategory" runat="server" />
                </div>
            </div>
        </div>
        <div class="row" id="tr12" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Certification</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtUserCertificate" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtUserCertificate" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtUserCertificate"
                        ErrorMessage="Enter User Certificate" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr13" runat="server">
           <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="4" />
            </div>
        </div>
        <div class="row" id="tr4" runat="server">
            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" ></asp:Label>
        </div>
        <div class="d-flex justify-content-between align-items-center px-1" id="tr2" runat="server">
            <dx:ASPxButton ID="lnkDelete" CssClass="btn-danger1" Text="Delete" ToolTip="Delete" runat="server" OnClick="lnkDelete_Click"></dx:ASPxButton>
            <div>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>

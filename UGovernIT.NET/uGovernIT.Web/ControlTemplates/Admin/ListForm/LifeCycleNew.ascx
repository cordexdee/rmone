
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LifeCycleNew.ascx.cs" Inherits="uGovernIT.Web.LifeCycleNew" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function CloseWithoutSaving()
    {
        var sourceURL = "<%= Request["source"] %>";
        window.parent.CloseWindowCallback(sourceURL);
          
    }
</script>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <asp:Panel runat="server" ID="lifecycleForm" CssClass="full-width">
        <div class="full-width">
            <asp:Label ID="lbMessage"  runat="server" ForeColor="Green"></asp:Label>
        </div>
        <div class="ms-formtable accomp-popup ">
            <div class="row" id="trTitle" runat="server" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtTitle" CssClass="asptextbox-asp" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                        Display="Dynamic" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvTitle" ValidationGroup="Task"  runat="server" ControlToValidate="txtTitle" ErrorMessage="Lifecycle with this name already exists"
                       OnServerValidate="cvTitle_ServerValidate" Display="Dynamic"  ></asp:CustomValidator>
                </div>
            </div>
            <div class="row" id="trNote" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox CssClass="asptextbox-asp" Rows="5" ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
            <div class="row" id="trItemOrder" runat="server" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Order</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox CssClass="asptextbox-asp" ID="txtOrder" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row addEditPopup-btnWrap">
                <dx:ASPxButton ValidationGroup="Task" ID="btSaveLifeCycle" Visible="true" runat="server" Text="Save" CssClass="primary-blueBtn"
                    ToolTip="Save as Template" Style="float: right;" OnClick="btSaveLifeCycle_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="cancel" runat="server" CssClass="secondary-cancelBtn" Text="Cancel" ToolTip="Cancel">
                    <ClientSideEvents Click="function(){CloseWithoutSaving();}" />
                </dx:ASPxButton>
              </div>
        </div>
    </asp:Panel>
</div>